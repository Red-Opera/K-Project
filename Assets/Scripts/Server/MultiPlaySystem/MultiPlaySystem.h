// MultiPlaySystem.h: 표준 시스템 포함 파일
// 또는 프로젝트 특정 포함 파일이 들어 있는 포함 파일입니다.

#pragma once

typedef void (*LogCallback)(const char*);

//extern "C"
//{
//	__declspec(dllexport) int StartServer(int port);
//	__declspec(dllexport) void StopServer();
//	__declspec(dllexport) void SetLogCallback(LogCallback callback);
//}