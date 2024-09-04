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

                    // 데이터가 여러 개 합쳐져 있을 수 있으므로 줄바꿈을 기준으로 나눔
                    while (true)
                    {
                        string completeData = completeMessage.ToString();
                        int newLineIndex = completeData.IndexOf('\n');

                        if (newLineIndex != -1)
                        {
                            // 줄바꿈 이전의 데이터를 완전한 메시지로 간주하고 처리
                            string singleMessage = completeData.Substring(0, newLineIndex).Trim();

                            if (!string.IsNullOrEmpty(singleMessage))
                            {
                                Debug.Log("Received complete data: " + singleMessage);
                                UpdatePlayerPositions(singleMessage);
                            }

                            // 남아있는 데이터 처리
                            completeMessage.Remove(0, newLineIndex + 1);
                        }

                        // 남은 데이터가 줄바꿈이 없으면 다음 수신 대기
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
        // 데이터 앞뒤의 공백 제거
        data = data.Trim();

        // 데이터 유효성 검사: 빈 데이터인지 확인
        if (string.IsNullOrEmpty(data))
        {
            Debug.LogError("Received empty data");
            return;
        }

        // 좌표를 쉼표로 분리
        string[] coords = data.Split(',');

        // 좌표의 수가 3개인지 확인
        if (coords.Length == 3)
        {
            try
            {
                // 각각의 좌표 문자열을 float로 변환
                float x = float.Parse(coords[0].Trim());
                float y = float.Parse(coords[1].Trim());
                float z = float.Parse(coords[2].Trim());

                // 변환된 좌표를 Vector3로 만듦
                Vector3 receivedPosition = new Vector3(x, y, z);
                Debug.Log("Received Position: " + receivedPosition);
            }

            catch (FormatException ex)
            {
                Debug.LogError("Data format error: " + data + " - " + ex.Message);
            }
        }

        // 좌표의 수가 3개가 아닌 경우 오류 처리
        else
            Debug.LogError("Incorrect position data format: " + data);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}