using System;
using System.IO;  // 파일 입출력 관련 네임스페이스
using System.Runtime.InteropServices;
using UnityEngine;

public class DecodeString
{
    [DllImport("StringEncrypt", CallingConvention = CallingConvention.Cdecl)]
    private static extern void Decode(string encrypedStr, IntPtr outString);

    private static readonly string[] accessable = { "MultiPlay", "Login" };

    internal static string Revert(string fromText)
    {
        // 접근 불가능한 코드에서 가져올 경우
        if (!HasAccessDecode())
            return "";

        // 복호화된 결과를 받을 버퍼 크기 설정 (적절한 크기로 설정해야 함)
        IntPtr bufferPtr = Marshal.AllocHGlobal(256);

        // C++의 Decode 함수 호출
        Decode(fromText, bufferPtr);

        // IntPtr에서 C#의 string으로 변환
        string decryptedStr = Marshal.PtrToStringAnsi(bufferPtr);

        return decryptedStr;
    }

    // 파일에서 키를 읽어오는 메소드
    private static string ReadKeyFromFile(string filePath)
    {
        // 파일에서 키를 한 줄 읽어옴
        try
        {
            return File.ReadAllText(filePath).Trim();
        }
        catch (Exception ex)
        {
            Debug.LogError("키 파일을 읽는 중 오류 발생: " + ex.Message);
            throw new Exception("키 파일을 읽는 중 오류 발생", ex);  // 빈 문자열 대신 예외를 발생
        }
    }

    // 해당 클래스에서 호출할 수 있는지 여부
    private static bool HasAccessDecode()
    {
        string callClass = new System.Diagnostics.StackTrace().GetFrame(2).GetMethod().DeclaringType.Name;

        if (Array.IndexOf(accessable, callClass) < 0)
        {
            Debug.Assert(false, $"Error (Access Exception) : {callClass} 클래스에서 접근할 수 없음");
            return false;
        }

        return true;
    }
}