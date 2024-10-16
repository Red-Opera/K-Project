#pragma once

#include <fstream>
#include <string>

class Log
{
public:
	static void FileOpen();							// �α� ���Ͽ��� �޼ҵ�
	static void Message(const char* message);		// �α׿� ����� �޼����� �޴� �޼ҵ�
	static void Message(const std::string message);	// �α׿� ����� �޼����� �޴� �޼ҵ�

	static void CloseLog();			// �α� ���� �ݴ� �޼ҵ�

	static pid_t serverPID;			// ���� ������ ���α׷��� PID�� ������ ����

private:
	static void PrintLog();			// �α׸� ���Ͽ� ����ϴ� �޼ҵ�
	static std::ofstream logFile;	
};