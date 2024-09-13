using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

// �������� ���� Ŭ���̾�Ʈ�� ����
public class ClientData 
{ 
    public int isFilp;                      // ĳ���� ������ �������� ���������� �Ǻ�
    public string animationName;            // ĳ���Ͱ� �����ϰ� �ִ� �ִϸ��̼� �̸�
    public float animationNormalizedTime;   // �� �ִϸ��̼��� ���� �ð�
    public Vector3 position;                // ĳ������ ��ġ
    public string currentScene;             // ĳ���Ͱ� ��ġ�ϴ� �� �̸�
    public string characterType;            // ĳ���� ����
    public GameObject clientObject;         // ĳ���� ������Ʈ
}

// �������� ���� Ŭ���̾�Ʈ�� �ִϸ��̼� ����
public class AnimatorClientData
{
    public AnimatorClientData(string animationName, float animationNormalizedTime) 
    {
        this.animationName = animationName;
        this.animationNormalizedTime = animationNormalizedTime;
    }

    public string animationName;            // �ִϸ��̼��� �̸�
    public float animationNormalizedTime;   // �ִϸ��̼� ���൵
}

public class MultiPlay : MonoBehaviour
{
    public static List<string> currentClientSpriteFlip;                                 // �̹����� ������ ĳ���� Ŭ���̾�Ʈ �̸�
    public static Dictionary<string, AnimatorClientData> currentClientAnimationName;    // Ŭ���̾�Ʈ�� ���� �����ϰ� �ִ� �ִϸ��̼� ����
    public static Dictionary<string, ClientData> clients;                               // ��� Ŭ���̾�Ʈ�� ����

    public static int currentServerFrame = 0;                                           // ���� ���� ������

    [SerializeField] private GameObject enchantressPlayer;
    [SerializeField] private GameObject berserkPlayer;
    [SerializeField] private GameObject knightPlayer;

    private TcpClient connectServer;
    private NetworkStream stream;
    private Thread receiveThread;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Transform bossStage;

    private static readonly string IP = "e2ec3761d72ed8486148a959f79c4627";
    private static readonly string port = "47ed4ff4f62e7248fedf65a9dd6f4654";
    private string currentSceneName;
    private bool isConnected = false;

    private void Start()
    {
        clients ??= new Dictionary<string, ClientData>();
        currentClientAnimationName ??= new Dictionary<string, AnimatorClientData>();
        currentClientSpriteFlip ??= new List<string>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        Debug.Assert(spriteRenderer != null, "������Ʈ�� Sprite Renderer�� �����ϴ�.");

        animator = GetComponent<Animator>();
        Debug.Assert(animator != null, "������Ʈ�� Animator�� �����ϴ�.");

        ConnectToServer(DecodeString.Revert(IP), int.Parse(DecodeString.Revert(port)));
        Debug.Log("Server started successfully on port " + port);

        SceneManager.sceneLoaded += GetBossStage;

        Application.runInBackground = true;
    }

    private void Update()
    {
        currentSceneName = SceneManager.GetActiveScene().name;


        if (isConnected)
            SendData(transform.position, (spriteRenderer.flipX ? 1 : 0));
    }

    private void ConnectToServer(string ip, int port)
    {
        try
        {
            connectServer = new TcpClient(ip, port);
            stream = connectServer.GetStream();
            isConnected = true;

            receiveThread = new Thread(ReceiveData) { IsBackground = true };
            receiveThread.Start();
        }

        catch (Exception e)
        {
            OnApplicationQuit();
            Debug.LogError("Online Server Connection Error: " + e.Message);
        }
    }

    private void Disconnect()
    {
        if (isConnected)
        {
            isConnected = false;
            stream?.Close();
            connectServer?.Close();
        }
    }

    private void SendData(Vector3 position, int isFilp)
    {
        if (!isConnected)
            return;

        try
        {
            string positionString = $"(" +
                $"{System.Diagnostics.Process.GetCurrentProcess().Id:D6}," +
                $"{isFilp}," +
                $"{GetAnimattionName()}," +
                $"{animator.GetCurrentAnimatorStateInfo(0).normalizedTime}," +
                $"{position.x},{position.y},{position.z}," +
                $"{currentSceneName}," +
                $"{GetPlayerTypeName(animator.runtimeAnimatorController.name)})";

            byte[] data = Encoding.UTF8.GetBytes(positionString);

            stream.Write(data, 0, data.Length);
        }

        catch (Exception e)
        {
            OnApplicationQuit();
            Debug.LogError("Send Error: " + e.Message);
        }
    }

    private void GetClientData(string singleMessage)
    {
        // �� �޽����� �� ������ ����
        string[] clientDataStrings = singleMessage.Split('\n', StringSplitOptions.RemoveEmptyEntries);

        // �����κ��� ��ġ�� ���� Ŭ���̾�Ʈ 
        HashSet<string> receivedClients = new();

        foreach (string clientDataString in clientDataStrings)
        {
            // �̸�, x, y, z ��ǥ�� ���ڿ��� ����
            string[] clientInfo = clientDataString.Trim('(', ')').Split(',');

            // ��ǥ�� ���� 6������ Ȯ��
            if (clientInfo.Length == 9)
            {
                try
                {
                    // ������ ��ǥ ���ڿ��� float�� ��ȯ
                    string clientName = clientInfo[0];
                    ClientData clientData = new()
                    {
                        isFilp = int.Parse(clientInfo[1].Trim()),
                        animationName = clientInfo[2].Trim(),
                        animationNormalizedTime = float.Parse(clientInfo[3].Trim()),
                        position = new Vector3(float.Parse(clientInfo[4].Trim(), System.Globalization.CultureInfo.InvariantCulture),
                                               float.Parse(clientInfo[5].Trim(), System.Globalization.CultureInfo.InvariantCulture),
                                               float.Parse(clientInfo[6].Trim(), System.Globalization.CultureInfo.InvariantCulture)),
                        currentScene = clientInfo[7].Trim(),
                        characterType = clientInfo[8].Trim()
                    };

                    if (!IsCorrectData(clientName, clientData))
                        continue;

                    receivedClients.Add(clientName);

                    // ��ȯ�� ��ǥ�� Vector3�� ����
                    //Debug.Log($"Received Position from {clientName}: {clientData.position.x:F6}, {clientData.position.y:F6}, {clientData.position.z:F6}, {clientData.characterType}, {clientData.animationNormalizedTime}");

                    // �ڽ��� ������ �ٸ� Ŭ���̾�Ʈ�� ������ ������Ʈ
                    if (clientName != System.Diagnostics.Process.GetCurrentProcess().Id.ToString("D6"))
                    {
                        MainTheadAction.Enqueue(() =>
                        {
                            if (clients.ContainsKey(clientName) && clients[clientName].characterType != clientData.characterType)
                            {
                                Destroy(clients[clientName].clientObject);
                                clients.Remove(clientName);
                            }

                            if (!clients.ContainsKey(clientName))
                            {
                                clients.Add(clientName, clientData);
                                clients[clientName].clientObject = Instantiate(GetInstancePlayer(clientData.characterType), clientData.position, Quaternion.identity);
                                clients[clientName].clientObject.GetComponent<OnlinePlayer>().playerName = clientName.PadLeft(6, '0');
                            }

                            clients[clientName].isFilp = clientData.isFilp;
                            clients[clientName].animationName = clientData.animationName;
                            clients[clientName].animationNormalizedTime = clientData.animationNormalizedTime;
                            clients[clientName].position = clientData.position;
                            clients[clientName].currentScene = clientData.currentScene;
                            clients[clientName].characterType = clientData.characterType;
                        });
                    }
                }

                catch (FormatException ex)
                {
                    Debug.LogError("Data format error: " + clientDataString + " - " + ex.Message);
                }
            }

            // ��ǥ�� ���� 6���� �ƴ� ��� ���� ó��
            else
                Debug.LogError("Incorrect client data format: " + clientDataString);
        }

        // �������� ���� ���� �÷��̾ ������ �迭
        List<string> clientsToRemove = new();

        foreach (var player in clients.Keys)
        {
            if (!receivedClients.Contains(player))
                clientsToRemove.Add(player);
        }

        // ���� �����忡�� ��ü ����
        MainTheadAction.Enqueue(() =>
        {
            foreach (var clientName in clientsToRemove)
            {
                if (clients.ContainsKey(clientName))
                {
                    Destroy(clients[clientName].clientObject);
                    clients.Remove(clientName);
                }
            }
        });
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

                            // ���� �����忡�� ������Ʈ
                            MainTheadAction.Enqueue(() =>
                            {
                                GetClientData(singleMessage);
                                UpdateClientCharacterPositions();
                                UpdateClientCharacterFlip();
                                UpdateClientAnimationName();
                                currentServerFrame++;
                                currentServerFrame %= 1000;
                            });

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
            {
                OnApplicationQuit();
                Debug.LogError("Server Receive Error: " + e.Message);
            }
        }
    }

    private bool IsCorrectData(string clientName, ClientData receiveData)
    {
        // �̸��� 6���ڰ� �ƴ� ���
        if (clientName.Length != 6)
            return false;

        if (receiveData.isFilp != 0 && receiveData.isFilp != 1)
            return false;

        if (receiveData.currentScene != currentSceneName)
            return false;

        if (bossStage != null && bossStage.gameObject.activeSelf)
            return false;

        return true;
    }

    private void UpdateClientCharacterPositions()
    {
        foreach (var client in clients)
        {
            ClientData clientData = client.Value;

            if (clientData.currentScene != currentSceneName)
                continue;

            clientData.clientObject.transform.position = clientData.position;
        }
    }

    private void UpdateClientCharacterFlip()
    {
        currentClientSpriteFlip.Clear();

        foreach (var client in clients)
        {
            ClientData clientData = client.Value;

            bool isFilp = clientData.isFilp == 1;

            if (isFilp)
                currentClientSpriteFlip.Add(client.Key);
        }
    }

    private void UpdateClientAnimationName()
    {
        currentClientAnimationName.Clear();

        foreach(var client in clients)
        {
            ClientData clientData = client.Value;
            AnimatorClientData animatorInfo = new(clientData.animationName, clientData.animationNormalizedTime);

            currentClientAnimationName.Add(client.Key, animatorInfo);
        }
    }

    private string GetPlayerTypeName(string typeName)
    {
        if (typeName.ToLower().Contains("enchantress"))
            return "Enchantress";

        else if (typeName.ToLower().Contains("berserk"))
            return "Berserk";

        else if (typeName.ToLower().Contains("knight"))
            return "Knight";

        return "";
    }

    private GameObject GetInstancePlayer(string name)
    {
        if (name.Contains("Enchantress"))
            return enchantressPlayer;

        else if (name.Contains("Berserk"))
            return berserkPlayer;

        else if (name.Contains("Knight"))
            return knightPlayer;

        return null;
    }

    private string GetAnimattionName()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        // ���� �ִϸ��̼� ������ �̸� ��������
        string currentAnimationName = state.shortNameHash.ToString();
        
        return currentAnimationName;
    }

    private void GetBossStage(Scene scene, LoadSceneMode mode)
    {        
        string currentSceneName = SceneManager.GetActiveScene().name;

        GameObject stages = GameObject.Find(currentSceneName[5..] + "Stages");

        if (stages != null)
        {
            for (int i = 0; i < stages.transform.childCount; i++)
            {
                Transform currentStage = stages.transform.GetChild(i);

                if (currentStage.name == "BossRoom")
                {
                    bossStage = currentStage;
                    return;
                }
            }
        }
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}