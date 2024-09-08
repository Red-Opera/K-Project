using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class MultiPlay : MonoBehaviour
{
    public static int currentServerFrame = 0;

    [SerializeField] private GameObject otherPlayer;

    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    private static readonly string IP = "e2ec3761d72ed8486148a959f79c4627";
    private static readonly string port = "47ed4ff4f62e7248fedf65a9dd6f4654";
    private bool isConnected = false;

    private static Dictionary<string, GameObject> playerPositions; 

    private void Start()
    {
        playerPositions ??= new Dictionary<string, GameObject>();

        ConnectToServer(DecodeString.Revert(IP), int.Parse(DecodeString.Revert(port)));
        Debug.Log("Server started successfully on port " + port);

        Application.runInBackground = true;
    }

    private void Update()
    {
        if (isConnected)
            SendPosition(transform.position);
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
            string positionString = $"({System.Diagnostics.Process.GetCurrentProcess().Id},{position.x},{position.y},{position.z})";
            byte[] data = Encoding.UTF8.GetBytes(positionString);

            stream.Write(data, 0, data.Length);
        }

        catch (Exception e)
        {
            Debug.LogError("Send Error: " + e.Message);
        }
    }

    private async void ReceiveData()
    {
        try
        {
            byte[] buffer = new byte[1024];
            StringBuilder completeMessage = new();

            while (isConnected)
            {
                // 데이터를 아직 보내지 않은 경우
                if (!stream.DataAvailable)
                {
                    await Task.Delay(10);
                    continue;
                }

                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string data = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    completeMessage.Append(data);

                    // '*' 문자를 기준으로 메시지 분리 및 처리
                    while (true)
                    {
                        string completeData = completeMessage.ToString();
                        int newLineIndex = completeData.IndexOf('*');

                        if (newLineIndex != -1)
                        {
                            // 줄바꿈 이전의 데이터를 완전한 메시지로 간주하고 처리
                            string singleMessage = completeData[..newLineIndex].Trim();
                            UpdatePlayerPositions(singleMessage);
                            currentServerFrame++;
                            currentServerFrame %= 1000;

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
            if (isConnected)
                Debug.LogError("Server Receive Error: " + e.Message);
        }
    }

    private void UpdatePlayerPositions(string data)
    {
        // 각 메시지를 줄 단위로 나눔
        string[] positions = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // 서버로부터 위치를 받은 클라이언트 
        HashSet<string> receivedClients = new HashSet<string>();

        foreach (string position in positions)
        {
            // 이름, x, y, z 좌표를 문자열로 저장
            string[] coords = position.Trim('(', ')').Split(',');

            // 좌표의 수가 3개인지 확인
            if (coords.Length == 4)
            {
                try
                {
                    // 각각의 좌표 문자열을 float로 변환
                    string clientName = coords[0];
                    float x = float.Parse(coords[1].Trim());
                    float y = float.Parse(coords[2].Trim());
                    float z = float.Parse(coords[3].Trim());

                    // 변환된 좌표를 Vector3로 만듦
                    Vector3 receivedPosition = new(x, y, z);
                    receivedClients.Add(clientName);
                    Debug.Log($"Received Position from {clientName}: {receivedPosition}");

                    if (clientName != System.Diagnostics.Process.GetCurrentProcess().Id.ToString())
                    {
                        MainTheadAction.Enqueue(() =>
                        {
                            if (playerPositions.ContainsKey(clientName))
                                playerPositions[clientName].transform.position = receivedPosition;

                            else
                            {
                                GameObject newPlayer = Instantiate(otherPlayer, receivedPosition, Quaternion.identity);
                                playerPositions[clientName] = newPlayer;
                            }
                        });
                    }
                }

                catch (FormatException ex)
                {
                    Debug.LogError("Data format error: " + position + " - " + ex.Message);
                }
            }

            // 좌표의 수가 3개가 아닌 경우 오류 처리
            else
                Debug.LogError("Incorrect position data format: " + position);
        }

        // 서버에서 받지 못한 플레이어 제거
        List<string> playersToRemove = new List<string>();

        foreach (var player in playerPositions.Keys)
        {
            if (!receivedClients.Contains(player))
                playersToRemove.Add(player);
        }

        // 메인 스레드에서 객체 제거
        MainTheadAction.Enqueue(() =>
        {
            foreach (var player in playersToRemove)
            {
                Destroy(playerPositions[player]);
                playerPositions.Remove(player);
            }
        });
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}