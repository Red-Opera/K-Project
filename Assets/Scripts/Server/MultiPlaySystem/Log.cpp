#include "Log.h"

#include <iostream>
#include <string>

pid_t Log::serverPID = 0;
std::ofstream Log::logFile;

void Log::FileOpen()
{
    logFile.open("logfile.txt", std::ios::out | std::ios::app);

    if (!logFile.is_open())
    {
        std::cerr << "Failed to open log file." << std::endl;
        exit(1);  // �α� ������ ������ ������ ����
    }
}

void Log::PrintLog()
{
    std::ifstream infile("logfile.txt");
    if (!infile.is_open())
    {
        std::cerr << "Failed to open log file for reading." << std::endl;
        return;
    }

    std::string line;
    while (std::getline(infile, line))
        std::cout << line << std::endl;
}

void Log::Message(const char* message)
{
    if (logFile.is_open())
        logFile << "[" << serverPID << "] " << message << std::endl;

    else
        std::cout << "[" << serverPID << "] " << message << std::endl;
}

void Log::CloseLog()
{
    if (logFile.is_open())
        logFile.close();
}