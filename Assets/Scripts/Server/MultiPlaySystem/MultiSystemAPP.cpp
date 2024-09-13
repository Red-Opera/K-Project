#include "Log.h"
#include "MultiSystemAPP.h"

#include <netinet/in.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <sys/stat.h>
#include <unistd.h>
#include <fcntl.h>
#include <csignal>
#include <cstring>
#include <sstream>

std::vector<int> MultiSystemAPP::clients;
std::mutex MultiSystemAPP::clientsMutex;
std::thread MultiSystemAPP::serverThread;
std::unordered_map<ClientIdenty, ClientData> MultiSystemAPP::clientData;

int MultiSystemAPP::serverSocket = 0;
bool MultiSystemAPP::serverRunning = false;

void MultiSystemAPP::BroadcastPosition()
{
    std::lock_guard<std::mutex> guard(clientsMutex);
    std::string clientPosText;

    for (const auto& client : clientData)
    {
        std::string clientName = client.first.name;
        ClientData clientData = client.second;

        std::string isFilp = std::to_string(clientData.isFilp);
        std::string animationNormalizedTime = std::to_string(clientData.animationNormalizedTime);
        std::string x = std::to_string(clientData.position.x), y = std::to_string(clientData.position.y), z = std::to_string(clientData.position.z);

        Log::Message("[" + std::to_string(client.first.id) + "]");
        clientPosText += "(" + clientName + "," + 
                               isFilp + "," + 
                               clientData.animationName + "," + 
                               animationNormalizedTime + "," + 
                               x + "," + y + "," + z + "," + 
                               clientData.currentScene + "," + 
                               clientData.characterType + ")\n";
    }
    clientPosText += "*";

    Log::Message(clientPosText);

    for (int client : clients)
        send(client, clientPosText.c_str(), clientPosText.length(), 0);
}

void MultiSystemAPP::HandleClient(int clientSocket)
{
    {
        std::lock_guard<std::mutex> guard(clientsMutex);
        clients.push_back(clientSocket);
    }

    char buffer[1024] = { 0 };
    ClientIdenty clientIdenty = { "", clientSocket };  // 클라이언트 구분하기 위한 정보

    while (serverRunning)
    {
        ssize_t received = recv(clientSocket, buffer, sizeof(buffer), 0);

        if (received <= 0)
            break;

        std::string clientInfo(buffer, received);   // 버퍼를 string으로 변환함
        ClientData receiveData{};                   // 클라리언트에서 받을 데이터

        std::istringstream stream(clientInfo); 
        char discard;
        stream >> discard;                                      // 첫 번째 글자를 읽어서 무시 '('
        std::getline(stream, clientIdenty.name, ',');           // 첫 번째 값을 clientName에 저장
        stream >> receiveData.isFilp;                           // 캐릭터의 이미지를 뒤집는지 여부 저장
        stream.ignore(1, ',');                           
        std::getline(stream, receiveData.animationName, ',');   // 캐릭터의 애니메이션 이름을 가져옴
        stream >> receiveData.animationNormalizedTime;          // 애니메이션의 진헹시간을 가져옴
        stream.ignore(1, ',');
        stream >> receiveData.position.x;                       // 다음 값을 position.x에 저장
        stream.ignore(1, ',');                                
        stream >> receiveData.position.y;                       // 다음 값을 position.y에 저장
        stream.ignore(1, ',');                                  
        stream >> receiveData.position.z;                       // 다음 값을 position.z에 저장
        stream.ignore(1, ',');                                
        std::getline(stream, receiveData.currentScene, ',');    // 캐릭터의 씬 이름을 가져옴
        std::getline(stream, receiveData.characterType, ')');   // 캐릭터의 씬 이름을 가져옴

        // 클라이언트 이름이 비어있거나 다를 경우에만 업데이트
        {
            std::lock_guard<std::mutex> guard(clientsMutex);
            FindErrorClinet(clientIdenty);

            // 클라이언트 이름과 ID로 클라이언트의 위치 및 공격 여부 업데이트
            clientData[clientIdenty] = receiveData;
        }

        BroadcastPosition();
    }

    // 서버가 끊어졌을 때 
    {
        std::lock_guard<std::mutex> guard(clientsMutex);
        auto it = std::find(clients.begin(), clients.end(), clientSocket);

        if (it != clients.end())
            clients.erase(it);

        clientData.erase(clientIdenty);
    }

    close(clientSocket);
}

void MultiSystemAPP::FindErrorClinet(ClientIdenty newClient)
{
    std::vector<ClientIdenty> clientToRemove;

    // 저장되어 있는 모든 클라이언트의 정보를 비교하여 ID 또는 이름이 바뀌는 경우를 찾음
    for (const auto& client : clientData)
    {
        if (client.first.id == newClient.id && client.first.name != newClient.name)
            clientToRemove.push_back(client.first);

        else if (client.first.name == newClient.name && client.first.id != newClient.id)
            clientToRemove.push_back(client.first);
    }

    // 잘못된 클라이언트를 제거함
    for (const auto& clientID : clientToRemove)
        clientData.erase(clientID);
}

int MultiSystemAPP::StartServer(int port)
{
    if (serverRunning)
    {
        Log::Message("Server is already running.");
        return -1;
    }

    serverThread = std::thread(ServerLoop, port);
    serverThread.detach();

    return 0;
}

void MultiSystemAPP::StopServer()
{
    serverRunning = false;

    if (serverSocket >= 0)
        close(serverSocket);

    Log::CloseLog();
}

void MultiSystemAPP::ServerLoop(int port)
{
    serverSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (serverSocket < 0)
    {
        Log::Message("Failed to create server socket.");
        return;
    }

    sockaddr_in serverAddr;
    memset(&serverAddr, 0, sizeof(serverAddr));
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    serverAddr.sin_port = htons(port);

    if (bind(serverSocket, (sockaddr*)&serverAddr, sizeof(serverAddr)) < 0)
    {
        std::string errorMessage = "Failed to bind server socket. Error: " + std::string(strerror(errno));

        Log::Message(errorMessage.c_str());
        close(serverSocket);
        return;
    }

    if (listen(serverSocket, 5) < 0)
    {
        Log::Message("Failed to listen on server socket.");
        close(serverSocket);
        return;
    }

    serverRunning = true;

    std::string logText = "Server started on port " + std::to_string(port);
    Log::Message(logText.c_str());

    while (serverRunning)
    {
        sockaddr_in clientAddr;
        socklen_t clientAddrLen = sizeof(clientAddr);
        int clientSocket = accept(serverSocket, (sockaddr*)&clientAddr, &clientAddrLen);

        if (clientSocket < 0)
        {
            Log::Message("Failed to accept client connection.");
            continue;
        }

        std::thread clientThread(HandleClient, clientSocket);
        clientThread.detach();
    }

    close(serverSocket);
}

void MultiSystemAPP::Daemonize()
{
    Log::serverPID = getpid();  // 서버 PID를 저장

    pid_t pid = fork();
    if (pid < 0)
    {
        Log::Message("Failed to fork.");
        exit(1);
    }

    // 부모 프로세스 종료
    if (pid > 0)
        exit(0);

    if (setsid() < 0)
    {
        Log::Message("Failed to create new session.");
        exit(1);
    }

    pid = fork();
    if (pid < 0)
    {
        Log::Message("Failed to fork.");
        exit(1);
    }

    // 첫 번째 자식 프로세스 종료
    if (pid > 0)
        exit(0);

    umask(0);

    int fd = open("/dev/null", O_RDWR);

    if (fd != -1)
    {
        dup2(fd, STDIN_FILENO);
        dup2(fd, STDOUT_FILENO);
        dup2(fd, STDERR_FILENO);
        close(fd);
    }
}

void MultiSystemAPP::SignalHandler(int signal)
{
    if (signal == SIGTERM || signal == SIGINT)
    {
        StopServer();
        exit(0);
    }
}