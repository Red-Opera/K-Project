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
                // �����͸� ���� ������ ���� ���
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

                    // '*' ���ڸ� �������� �޽��� �и� �� ó��
                    while (true)
                    {
                        string completeData = completeMessage.ToString();
                        int newLineIndex = completeData.IndexOf('*');

                        if (newLineIndex != -1)
                        {
                            // �ٹٲ� ������ �����͸� ������ �޽����� �����ϰ� ó��
                            string singleMessage = completeData[..newLineIndex].Trim();
                            UpdatePlayerPositions(singleMessage);
                            currentServerFrame++;
                            currentServerFrame %= 1000;

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
            if (isConnected)
                Debug.LogError("Server Receive Error: " + e.Message);
        }
    }

    private void UpdatePlayerPositions(string data)
    {
        // �� �޽����� �� ������ ����
        string[] positions = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // �����κ��� ��ġ�� ���� Ŭ���̾�Ʈ 
        HashSet<string> receivedClients = new HashSet<string>();

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

            // ��ǥ�� ���� 3���� �ƴ� ��� ���� ó��
            else
                Debug.LogError("Incorrect position data format: " + position);
        }

        // �������� ���� ���� �÷��̾� ����
        List<string> playersToRemove = new List<string>();

        foreach (var player in playerPositions.Keys)
        {
            if (!receivedClients.Contains(player))
                playersToRemove.Add(player);
        }

        // ���� �����忡�� ��ü ����
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