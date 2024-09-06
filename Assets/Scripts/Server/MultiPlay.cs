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

    private void ConnectToServer(string ip, int port)
    {
        try
        {
            client = new TcpClient(ip, port);
            stream = client.GetStream();
            isConnected = true;

            receiveThread = new Thread(ReceiveData) { IsBackground = true };
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
            string positionString = $"(aa,{position.x},{position.y},{position.z})";
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
            StringBuilder completeMessage = new();

            while (isConnected)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    completeMessage.Append(data);

                    // '*' ���ڸ� �������� �޽��� �и� �� ó��
                    while (true)
                    {
                        string completeData = completeMessage.ToString();
                        int newLineIndex = completeData.IndexOf('*');

                        Debug.Log("Get Data : " + completeData);

                        if (newLineIndex != -1)
                        {
                            // �ٹٲ� ������ �����͸� ������ �޽����� �����ϰ� ó��
                            string singleMessage = completeData[..newLineIndex].Trim();
                            UpdatePlayerPositions(singleMessage);

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
        // �� �޽����� �� ������ ����
        string[] positions = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        foreach (string position in positions)
        {
            // �̸�, x, y, z ��ǥ�� ���ڿ��� ����
            string[] coords = position.Trim('(', ')').Split(',');

            // ��ǥ�� ���� 3������ Ȯ��
            if (coords.Length == 4)
            {
                try
                {
                    // ������ ��ǥ ���ڿ��� float�� ��ȯ
                    string clientName = coords[0];
                    float x = float.Parse(coords[1].Trim());
                    float y = float.Parse(coords[2].Trim());
                    float z = float.Parse(coords[3].Trim());

                    // ��ȯ�� ��ǥ�� Vector3�� ����
                    Vector3 receivedPosition = new(x, y, z);
                    Debug.Log($"Received Position from {clientName}: {receivedPosition}");
                }

                catch (FormatException ex)
                {
                    Debug.LogError("Data format error: " + position + " - " + ex.Message);
                }
            }

            // ��ǥ�� ���� 3���� �ƴ� ��� ���� ó��
            else
                Debug.LogError("Incorrect position data format: " + position);

        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}