using System;
using System.IO;  // ���� ����� ���� ���ӽ����̽�
using System.Runtime.InteropServices;
using UnityEngine;

public class DecodeString
{
    [DllImport("StringEncrypt", CallingConvention = CallingConvention.Cdecl)]
    private static extern void Decode(string encrypedStr, IntPtr outString);

    private static readonly string[] accessable = { "MultiPlay", "Login" };

    internal static string Revert(string fromText)
    {
        // ���� �Ұ����� �ڵ忡�� ������ ���
        if (!HasAccessDecode())
            return "";

        // ��ȣȭ�� ����� ���� ���� ũ�� ���� (������ ũ��� �����ؾ� ��)
        IntPtr bufferPtr = Marshal.AllocHGlobal(256);

        // C++�� Decode �Լ� ȣ��
        Decode(fromText, bufferPtr);

        // IntPtr���� C#�� string���� ��ȯ
        string decryptedStr = Marshal.PtrToStringAnsi(bufferPtr);

        return decryptedStr;
    }

    // ���Ͽ��� Ű�� �о���� �޼ҵ�
    private static string ReadKeyFromFile(string filePath)
    {
        // ���Ͽ��� Ű�� �� �� �о��
        try
        {
            return File.ReadAllText(filePath).Trim();
        }
        catch (Exception ex)
        {
            Debug.LogError("Ű ������ �д� �� ���� �߻�: " + ex.Message);
            throw new Exception("Ű ������ �д� �� ���� �߻�", ex);  // �� ���ڿ� ��� ���ܸ� �߻�
        }
    }

    // �ش� Ŭ�������� ȣ���� �� �ִ��� ����
    private static bool HasAccessDecode()
    {
        string callClass = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().DeclaringType.Name;

        if (Array.IndexOf(accessable, callClass) < 0)
        {
            Debug.Assert(false, $"Error (Access Exception) : {callClass} Ŭ�������� ������ �� ����");
            return false;
        }

        return true;
    }
}