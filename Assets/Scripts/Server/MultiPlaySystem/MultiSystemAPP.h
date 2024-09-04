// MultiPlaySystem.h: 표준 시스템 포함 파일
// 또는 프로젝트 특정 포함 파일이 들어 있는 포함 파일입니다.

#pragma once

#include <mutex>
#include <vector>
#include <thread>

class MultiSystemAPP
{
public:
	static void BroadcastPosition(const char* data, int length);	// 플레이어 위치 전송하는 메소드
	static void HandleClient(int clientSocket);						// 서버 데이터 전송, 수신 메소드

	static int StartServer(int port);		// 서버 시작 메소드
	static void StopServer();				// 서버 종료 메소드
	static void ServerLoop(int port);		// 서버 Loop

	static void Daemonize();				// 프로그램 데몬화 메소드
	static void SignalHandler(int signal);	// 프로그램 시그널 처리 메소드

	static bool serverRunning;

private:
	static std::vector<int> clients;
	static std::mutex clientsMutex;
	static std::thread serverThread;

	static int serverSocket;
};