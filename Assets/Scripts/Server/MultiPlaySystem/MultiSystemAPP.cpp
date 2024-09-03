#include <iostream>
#include <thread>
#include <mutex>
#include <vector>
#include <sys/socket.h>
#include <netinet/in.h>
#include <unistd.h>
#include <cstring>
#include <sys/types.h>
#include <sys/stat.h>
#include <fcntl.h>
#include <csignal>
#include <cerrno>

#include "Log.h"

// 전역 변수
std::vector<int> clients;
std::mutex clientsMutex;
std::thread serverThread;

int serverSocket;
bool serverRunning = false;

void BroadcastPosition(const char* data, int length)
{
    std::lock_guard<std::mutex> guard(clientsMutex);
    for (int client : clients)
    {
        send(client, data, length, 0);
    }
}

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

void ServerLoop(int port)
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

int StartServer(int port)
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

void StopServer()
{
    serverRunning = false;

    if (serverSocket >= 0)
        close(serverSocket);

    Log::CloseLog();
}

void Daemonize()
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

void SignalHandler(int signal)
{
    if (signal == SIGTERM || signal == SIGINT)
    {
        StopServer();
        exit(0);
    }
}

int main(int argc, char* argv[])
{
    if (argc < 2)
    {
        std::cerr << "Usage: " << argv[0] << " <port>" << std::endl;
        return 1;
    }

    int port = std::stoi(argv[1]);

    Daemonize();
    Log::FileOpen();

    if (StartServer(port) != 0)
    {
        Log::Message("Failed to start server.");
        return 1;
    }

    Log::Message("Server is running in the background...");
    std::this_thread::sleep_for(std::chrono::milliseconds(10));

    // 신호 처리기 설정
    signal(SIGTERM, SignalHandler);
    signal(SIGINT, SignalHandler);

    // 메인 스레드를 종료시키지 않도록 무한 루프 추가
    while (serverRunning)
        std::this_thread::sleep_for(std::chrono::minutes(5));

    StopServer();
    return 0;
}