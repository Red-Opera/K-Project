#include <iostream>
#include <fstream>
#include <string>
#include <thread>
#include <mutex>
#include <vector>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include <cstring>

// 전역 변수
std::vector<int> clients;
std::mutex clientsMutex;
std::thread serverThread;
std::ofstream logFile;

int serverSocket;
bool serverRunning = false;

// 로그 파일 초기화
void InitLogFile()
{
    logFile.open("logfile.txt", std::ios::out | std::ios::app);
    if (!logFile.is_open())
    {
        std::cerr << "Failed to open log file." << std::endl;
    }
}

// 로그 메시지 기록
void LogMessage(const char* message)
{
    if (logFile.is_open())
        logFile << message << std::endl;
    else
        std::cout << message << std::endl;
}

// 모든 클라이언트에게 데이터 브로드캐스트
void BroadcastPosition(const char* data, int length)
{
    std::lock_guard<std::mutex> guard(clientsMutex);
    for (int client : clients)
    {
        send(client, data, length, 0);
    }
}

// 클라이언트 핸들링
void HandleClient(int clientSocket)
{
    {
        std::lock_guard<std::mutex> guard(clientsMutex);
        clients.push_back(clientSocket);
    }

    char buffer[1024] = { 0 };

    while (serverRunning)
    {
        ssize_t received = recv(clientSocket, buffer, sizeof(buffer), 0);
        if (received <= 0)
            break;

        BroadcastPosition(buffer, received);
    }

    {
        std::lock_guard<std::mutex> guard(clientsMutex);
        clients.erase(std::remove(clients.begin(), clients.end(), clientSocket), clients.end());
    }

    close(clientSocket);
}

// 서버 루프
void ServerLoop(int port)
{
    serverSocket = socket(AF_INET, SOCK_STREAM, 0);
    if (serverSocket < 0)
    {
        LogMessage("Failed to create server socket.");
        return;
    }

    sockaddr_in serverAddr;
    memset(&serverAddr, 0, sizeof(serverAddr));
    serverAddr.sin_family = AF_INET;
    serverAddr.sin_addr.s_addr = INADDR_ANY;
    serverAddr.sin_port = htons(port);

    if (bind(serverSocket, (sockaddr*)&serverAddr, sizeof(serverAddr)) < 0)
    {
        LogMessage("Failed to bind server socket.");
        close(serverSocket);
        return;
    }

    if (listen(serverSocket, 5) < 0)
    {
        LogMessage("Failed to listen on server socket.");
        close(serverSocket);
        return;
    }

    serverRunning = true;

    std::string logText = "Server started on port " + std::to_string(port);
    LogMessage(logText.c_str());

    while (serverRunning)
    {
        sockaddr_in clientAddr;
        socklen_t clientAddrLen = sizeof(clientAddr);
        int clientSocket = accept(serverSocket, (sockaddr*)&clientAddr, &clientAddrLen);
        if (clientSocket < 0)
        {
            LogMessage("Failed to accept client connection.");
            continue;
        }

        std::thread clientThread(HandleClient, clientSocket);
        clientThread.detach();
    }

    close(serverSocket);
}

// 서버 시작
int StartServer(int port)
{
    if (serverRunning)
        return -1;

    serverThread = std::thread(ServerLoop, port);
    serverThread.detach();

    return 0;
}

// 서버 중지
void StopServer()
{
    serverRunning = false;

    if (serverSocket >= 0)
        close(serverSocket);

    if (logFile.is_open())
        logFile.close();
}

// 메인 함수
int main(int argc, char* argv[])
{
    if (argc < 2)
    {
        std::cerr << "Usage: " << argv[0] << " <port>" << std::endl;
        return 1;
    }

    int port = std::stoi(argv[1]);

    InitLogFile();

    if (StartServer(port) != 0)
    {
        std::cerr << "Failed to start server." << std::endl;
        return 1;
    }

    std::cout << "Server is running. Press Enter to stop..." << std::endl;
    std::cin.get();  // 서버가 계속 실행되도록 대기

    StopServer();
    return 0;
}
