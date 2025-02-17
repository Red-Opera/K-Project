﻿cmake_minimum_required(VERSION 3.12)

# 지원되는 경우 MSVC 컴파일러에 대해 핫 다시 로드 사용하도록 설정합니다.
if (POLICY CMP0141)
  cmake_policy(SET CMP0141 NEW)
  set(CMAKE_MSVC_DEBUG_INFORMATION_FORMAT "$<IF:$<AND:$<C_COMPILER_ID:MSVC>,$<CXX_COMPILER_ID:MSVC>>,$<$<CONFIG:Debug,RelWithDebInfo>:EditAndContinue>,$<$<CONFIG:Debug,RelWithDebInfo>:ProgramDatabase>>")
endif()


project("MultiPlaySystem")

# 이 프로젝트의 실행 파일에 소스를 추가합니다.
add_executable(MultiPlaySystem "MultiSystemAPP.cpp" "MultiSystemAPP.h" "Log.cpp" "Log.h" "Run.cpp")

if (CMAKE_VERSION VERSION_GREATER 3.12)
  set_property(TARGET MultiPlaySystem PROPERTY CXX_STANDARD 20)
endif()

# Find Threads package
find_package(Threads REQUIRED)

# Link pthread library to the executable
target_link_libraries(MultiPlaySystem PRIVATE Threads::Threads)