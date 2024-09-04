using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class MultiPlay : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    private static readonly string IP = "";
    private static readonly int port = 0;
    private bool isConnected = false;

    public Vector3 playerPosition;

    private void Start()
    {
        ConnectToServer(IP, port);
        Debug.Log("Server started successfully on port " + port);
    }

    private void Update()
    {
        if (isConnected)
        {
            playerPosition = transform.position;
            SendPosition(playerPosition);
        }
    }

    private static void LogMessage(string message)
    {
        Debug.Log(message);
    }

    private void ConnectToServer(string ip, int port)
    {
        try
        {
            client = new TcpClient(ip, port);
            stream = client.GetStream();
            isConnected = true;

            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        catch (Exception e)
        {
            Debug.LogError("Online Server Connection Error: " + e.Message);
        }
    }

    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            stream?.Close();
            client?.Close();
        }
    }

    private void SendPosition(Vector3 position)
    {
        if (!isConnected)
            return;

        try
        {
            string positionString = $"({position.x},{position.y},{position.z})\n";
            byte[] data = Encoding.UTF8.GetBytes(positionString);

            stream.Write(data, 0, data.Length);
        }

        catch (Exception e)
        {
            Debug.LogError("Send Error: " + e.Message);
        }
    }

    private void ReceiveData()
    {
        try
        {
            byte[] buffer = new byte[1024];
            StringBuilder completeMessage = new StringBuilder();

            while (isConnected)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    completeMessage.Append(data);

                    // �����Ͱ� ���� �� ������ ���� �� �����Ƿ� �ٹٲ��� �������� ����
                    while (true)
                    {
                        string completeData = completeMessage.ToString();
                        int newLineIndex = completeData.IndexOf('\n');

                        if (newLineIndex != -1)
                        {
                            // �ٹٲ� ������ �����͸� ������ �޽����� �����ϰ� ó��
                            string singleMessage = completeData.Substring(0, newLineIndex).Trim();

                            if (!string.IsNullOrEmpty(singleMessage))
                            {
                                Debug.Log("Received complete data: " + singleMessage);
                                UpdatePlayerPositions(singleMessage);
                            }

                            // �����ִ� ������ ó��
                            completeMessage.Remove(0, newLineIndex + 1);
                        }

                        // ���� �����Ͱ� �ٹٲ��� ������ ���� ���� ���
                        else
                            break;
                    }
                }
            }
        }

        catch (Exception e)
        {
            Debug.LogError("Server Receive Error: " + e.Message);
        }
    }

    private void UpdatePlayerPositions(string data)
    {
        // ������ �յ��� ���� ����
        data = data.Trim();

        // ������ ��ȿ�� �˻�: �� ���������� Ȯ��
        if (string.IsNullOrEmpty(data))
        {
            Debug.LogError("Received empty data");
            return;
        }

        // ��ǥ�� ��ǥ�� �и�
        string[] coords = data.Split(',');

        // ��ǥ�� ���� 3������ Ȯ��
        if (coords.Length == 3)
        {
            try
            {
                // ������ ��ǥ ���ڿ��� float�� ��ȯ
                float x = float.Parse(coords[0].Trim());
                float y = float.Parse(coords[1].Trim());
                float z = float.Parse(coords[2].Trim());

                // ��ȯ�� ��ǥ�� Vector3�� ����
                Vector3 receivedPosition = new Vector3(x, y, z);
                Debug.Log("Received Position: " + receivedPosition);
            }

            catch (FormatException ex)
            {
                Debug.LogError("Data format error: " + data + " - " + ex.Message);
            }
        }

        // ��ǥ�� ���� 3���� �ƴ� ��� ���� ó��
        else
            Debug.LogError("Incorrect position data format: " + data);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}