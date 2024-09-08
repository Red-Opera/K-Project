#include <iostream>
#include <fstream>
#include <string>
#include <sstream>
#include <vector>
#include <cstring>

using namespace std;

const int BlockSize = 32; // 32바이트 블록 크기

const int Nb = 4;  // AES 블록 크기 (4 워드, 16 바이트)
const int Nk = 4;  // AES-128 키 크기 (4 워드, 16 바이트)
const int Nr = 10; // AES-128 라운드 수

// S-box 테이블 (AES 표준에 정의됨)
const uint8_t sbox[256] =
{ 0x63 ,0x7c ,0x77 ,0x7b ,0xf2 ,0x6b ,0x6f ,0xc5 ,0x30 ,0x01 ,0x67 ,0x2b ,0xfe ,0xd7 ,0xab ,0x76
,0xca ,0x82 ,0xc9 ,0x7d ,0xfa ,0x59 ,0x47 ,0xf0 ,0xad ,0xd4 ,0xa2 ,0xaf ,0x9c ,0xa4 ,0x72 ,0xc0
,0xb7 ,0xfd ,0x93 ,0x26 ,0x36 ,0x3f ,0xf7 ,0xcc ,0x34 ,0xa5 ,0xe5 ,0xf1 ,0x71 ,0xd8 ,0x31 ,0x15
,0x04 ,0xc7 ,0x23 ,0xc3 ,0x18 ,0x96 ,0x05 ,0x9a ,0x07 ,0x12 ,0x80 ,0xe2 ,0xeb ,0x27 ,0xb2 ,0x75
,0x09 ,0x83 ,0x2c ,0x1a ,0x1b ,0x6e ,0x5a ,0xa0 ,0x52 ,0x3b ,0xd6 ,0xb3 ,0x29 ,0xe3 ,0x2f ,0x84
,0x53 ,0xd1 ,0x00 ,0xed ,0x20 ,0xfc ,0xb1 ,0x5b ,0x6a ,0xcb ,0xbe ,0x39 ,0x4a ,0x4c ,0x58 ,0xcf
,0xd0 ,0xef ,0xaa ,0xfb ,0x43 ,0x4d ,0x33 ,0x85 ,0x45 ,0xf9 ,0x02 ,0x7f ,0x50 ,0x3c ,0x9f ,0xa8
,0x51 ,0xa3 ,0x40 ,0x8f ,0x92 ,0x9d ,0x38 ,0xf5 ,0xbc ,0xb6 ,0xda ,0x21 ,0x10 ,0xff ,0xf3 ,0xd2
,0xcd ,0x0c ,0x13 ,0xec ,0x5f ,0x97 ,0x44 ,0x17 ,0xc4 ,0xa7 ,0x7e ,0x3d ,0x64 ,0x5d ,0x19 ,0x73
,0x60 ,0x81 ,0x4f ,0xdc ,0x22 ,0x2a ,0x90 ,0x88 ,0x46 ,0xee ,0xb8 ,0x14 ,0xde ,0x5e ,0x0b ,0xdb
,0xe0 ,0x32 ,0x3a ,0x0a ,0x49 ,0x06 ,0x24 ,0x5c ,0xc2 ,0xd3 ,0xac ,0x62 ,0x91 ,0x95 ,0xe4 ,0x79
,0xe7 ,0xc8 ,0x37 ,0x6d ,0x8d ,0xd5 ,0x4e ,0xa9 ,0x6c ,0x56 ,0xf4 ,0xea ,0x65 ,0x7a ,0xae ,0x08
,0xba ,0x78 ,0x25 ,0x2e ,0x1c ,0xa6 ,0xb4 ,0xc6 ,0xe8 ,0xdd ,0x74 ,0x1f ,0x4b ,0xbd ,0x8b ,0x8a
,0x70 ,0x3e ,0xb5 ,0x66 ,0x48 ,0x03 ,0xf6 ,0x0e ,0x61 ,0x35 ,0x57 ,0xb9 ,0x86 ,0xc1 ,0x1d ,0x9e
,0xe1 ,0xf8 ,0x98 ,0x11 ,0x69 ,0xd9 ,0x8e ,0x94 ,0x9b ,0x1e ,0x87 ,0xe9 ,0xce ,0x55 ,0x28 ,0xdf
,0x8c ,0xa1 ,0x89 ,0x0d ,0xbf ,0xe6 ,0x42 ,0x68 ,0x41 ,0x99 ,0x2d ,0x0f ,0xb0 ,0x54 ,0xbb ,0x16 };

// 역 S-box 테이블 (AES 복호화용)
const uint8_t inv_sbox[256] =
{ 0x52, 0x09, 0x6A, 0xD5, 0x30, 0x36, 0xA5, 0x38, 0xBF, 0x40, 0xA3, 0x9E, 0x81, 0xF3, 0xD7, 0xFB,
0x7C, 0xE3, 0x39, 0x82, 0x9B, 0x2F, 0xFF, 0x87, 0x34, 0x8E, 0x43, 0x44, 0xC4, 0xDE, 0xE9, 0xCB,
0x54, 0x7B, 0x94, 0x32, 0xA6, 0xC2, 0x23, 0x3D, 0xEE, 0x4C, 0x95, 0x0B, 0x42, 0xFA, 0xC3, 0x4E,
0x08, 0x2E, 0xA1, 0x66, 0x28, 0xD9, 0x24, 0xB2, 0x76, 0x5B, 0xA2, 0x49, 0x6D, 0x8B, 0xD1, 0x25,
0x72, 0xF8, 0xF6, 0x64, 0x86, 0x68, 0x98, 0x16, 0xD4, 0xA4, 0x5C, 0xCC, 0x5D, 0x65, 0xB6, 0x92,
0x6C, 0x70, 0x48, 0x50, 0xFD, 0xED, 0xB9, 0xDA, 0x5E, 0x15, 0x46, 0x57, 0xA7, 0x8D, 0x9D, 0x84,
0x90, 0xD8, 0xAB, 0x00, 0x8C, 0xBC, 0xD3, 0x0A, 0xF7, 0xE4, 0x58, 0x05, 0xB8, 0xB3, 0x45, 0x06,
0xD0, 0x2C, 0x1E, 0x8F, 0xCA, 0x3F, 0x0F, 0x02, 0xC1, 0xAF, 0xBD, 0x03, 0x01, 0x13, 0x8A, 0x6B,
0x3A, 0x91, 0x11, 0x41, 0x4F, 0x67, 0xDC, 0xEA, 0x97, 0xF2, 0xCF, 0xCE, 0xF0, 0xB4, 0xE6, 0x73,
0x96, 0xAC, 0x74, 0x22, 0xE7, 0xAD, 0x35, 0x85, 0xE2, 0xF9, 0x37, 0xE8, 0x1C, 0x75, 0xDF, 0x6E,
0x47, 0xF1, 0x1A, 0x71, 0x1D, 0x29, 0xC5, 0x89, 0x6F, 0xB7, 0x62, 0x0E, 0xAA, 0x18, 0xBE, 0x1B,
0xFC, 0x56, 0x3E, 0x4B, 0xC6, 0xD2, 0x79, 0x20, 0x9A, 0xDB, 0xC0, 0xFE, 0x78, 0xCD, 0x5A, 0xF4,
0x1F, 0xDD, 0xA8, 0x33, 0x88, 0x07, 0xC7, 0x31, 0xB1, 0x12, 0x10, 0x59, 0x27, 0x80, 0xEC, 0x5F,
0x60, 0x51, 0x7F, 0xA9, 0x19, 0xB5, 0x4A, 0x0D, 0x2D, 0xE5, 0x7A, 0x9F, 0x93, 0xC9, 0x9C, 0xEF,
0xA0, 0xE0, 0x3B, 0x4D, 0xAE, 0x2A, 0xF5, 0xB0, 0xC8, 0xEB, 0xBB, 0x3C, 0x83, 0x53, 0x99, 0x61,
0x17, 0x2B, 0x04, 0x7E, 0xBA, 0x77, 0xD6, 0x26, 0xE1, 0x69, 0x14, 0x63, 0x55, 0x21, 0x0C, 0x7D
};

// AES의 Rcon 테이블 (라운드 상수)
const uint8_t Rcon[11] = {
    0x00, 0x01, 0x02, 0x04, 0x08, 0x10, 0x20, 0x40, 0x80, 0x1B, 0x36
};

// 키 확장 함수
void KeyExpansion(const uint8_t* key, uint8_t* expandedKeys) 
{
    uint8_t temp[4];
    int i = 0;

    while (i < Nk * 4) 
    {
        expandedKeys[i] = key[i];
        i++;
    }

    i = Nk * 4;
    while (i < Nb * (Nr + 1) * 4) 
    {
        for (int k = 0; k < 4; k++)
            temp[k] = expandedKeys[i - 4 + k];

        if (i / 4 % Nk == 0) 
        {
            // Rotate and SubBytes
            uint8_t t = temp[0];
            temp[0] = sbox[temp[1]] ^ Rcon[i / (Nk * 4)];
            temp[1] = sbox[temp[2]];
            temp[2] = sbox[temp[3]];
            temp[3] = sbox[t];
        }

        for (int k = 0; k < 4; k++) 
        {
            expandedKeys[i] = expandedKeys[i - Nk * 4] ^ temp[k];
            i++;
        }
    }
}

// SubBytes 변환
void SubBytes(uint8_t state[4][4]) 
{
    for (int i = 0; i < 4; i++) 
    {
        for (int j = 0; j < 4; j++)
            state[i][j] = sbox[state[i][j]];
    }
}

// 역 SubBytes 변환 (복호화용)
void InvSubBytes(uint8_t state[4][4]) 
{
    for (int i = 0; i < 4; i++) 
    {
        for (int j = 0; j < 4; j++)
            state[i][j] = inv_sbox[state[i][j]];
    }
}

// ShiftRows 변환
void ShiftRows(uint8_t state[4][4]) 
{
    uint8_t temp[4];
    for (int i = 1; i < 4; i++) 
    {
        for (int j = 0; j < 4; j++)
            temp[j] = state[i][(j + i) % 4];
        
        for (int j = 0; j < 4; j++) 
            state[i][j] = temp[j];
    }
}

// 역 ShiftRows 변환 (복호화용)
void InvShiftRows(uint8_t state[4][4]) 
{
    uint8_t temp[4];

    for (int i = 1; i < 4; i++) 
    {
        for (int j = 0; j < 4; j++)
            temp[j] = state[i][(j - i + 4) % 4];

        for (int j = 0; j < 4; j++)
            state[i][j] = temp[j];
    }
}

// AddRoundKey 변환
void AddRoundKey(uint8_t state[4][4], uint8_t* roundKey) 
{
    for (int i = 0; i < 4; i++) 
    {
        for (int j = 0; j < 4; j++)
            state[j][i] ^= roundKey[i * 4 + j];
    }
}

// 문자열을 바이트 배열로 변환하는 함수
void StringToByteArray(const string& str, uint8_t* byteArray, int size) 
{
    for (int i = 0; i < size && i < str.size(); i++)
        byteArray[i] = static_cast<uint8_t>(str[i]);

    // 남는 공간은 0으로 패딩
    for (int i = str.size(); i < size; i++)
        byteArray[i] = 0;
}

// 바이트 배열을 16진수 문자열로 변환하는 함수
string ByteArrayToHexString(const uint8_t* byteArray, int size) 
{
    string hexStr;

    for (int i = 0; i < size; i++) 
    {
        char buf[3];
        snprintf(buf, sizeof(buf), "%02x", byteArray[i]);
        hexStr += buf;
    }

    return hexStr;
}

// 16진수 문자열을 바이트 배열로 변환하는 함수
void HexStringToByteArray(const string& hexStr, uint8_t* byteArray, int size)
{
    for (int i = 0; i < size; ++i) 
    {
        std::string byteString = hexStr.substr(i * 2, 2);
        byteArray[i] = static_cast<uint8_t>(std::stoul(byteString, nullptr, 16));
    }
}

// 파일에서 키를 읽어오는 함수
string ReadKeyFromFile(const string& filename) 
{
    ifstream file(filename);
    string key;

    if (file.is_open()) 
    {
        getline(file, key);  // 한 줄을 읽어서 키로 사용
        file.close();
    }

    return key;
}

// 블록을 나누는 함수
vector<string> SplitIntoBlocks(const string& inputStr, int blockSize)
{
    vector<string> blocks;
    size_t pos = 0;

    while (pos < inputStr.size())
    {
        blocks.push_back(inputStr.substr(pos, blockSize));
        pos += blockSize;
    }

    return blocks;
}

// 블록을 연결하여 하나의 문자열로 만드는 함수
string JoinBlocks(const vector<string>& blocks)
{
    string result;
    for (const auto& block : blocks)
        result += block;

    return result;
}

// 암호화 및 복호화 함수 (32바이트 블록으로 처리)
string AESEncrypt(const string& inputStr, const string& keyStr)
{
    uint8_t state[4][4];
    uint8_t expandedKeys[Nb * (Nr + 1) * 4];
    uint8_t key[16];
    uint8_t input[16];
    uint8_t output[16];

    memset(state, 0, sizeof(state));
    memset(expandedKeys, 0, sizeof(expandedKeys));
    memset(key, 0, sizeof(key));
    memset(input, 0, sizeof(input));
    memset(output, 0, sizeof(output));

    StringToByteArray(keyStr, key, 16);
    StringToByteArray(inputStr, input, 16);
    KeyExpansion(key, expandedKeys);

    for (int i = 0; i < 16; i++)
        state[i % 4][i / 4] = input[i];

    AddRoundKey(state, expandedKeys);

    for (int round = 1; round < Nr; round++)
    {
        SubBytes(state);
        ShiftRows(state);
        AddRoundKey(state, expandedKeys + round * Nb * 4);
    }

    SubBytes(state);
    ShiftRows(state);
    AddRoundKey(state, expandedKeys + Nr * Nb * 4);

    for (int i = 0; i < 16; i++)
        output[i] = state[i % 4][i / 4];

    return ByteArrayToHexString(output, 16);
}

string AESDecrypt(const string& encryptedStr, const string& keyStr)
{
    uint8_t state[4][4];
    uint8_t expandedKeys[Nb * (Nr + 1) * 4];
    uint8_t key[16];
    uint8_t input[16];
    uint8_t output[16];

    memset(state, 0, sizeof(state));
    memset(expandedKeys, 0, sizeof(expandedKeys));
    memset(key, 0, sizeof(key));
    memset(input, 0, sizeof(input));
    memset(output, 0, sizeof(output));

    StringToByteArray(keyStr, key, 16);
    HexStringToByteArray(encryptedStr, input, 16);
    KeyExpansion(key, expandedKeys);

    for (int i = 0; i < 16; i++)
        state[i % 4][i / 4] = input[i];

    AddRoundKey(state, expandedKeys + Nr * Nb * 4);

    for (int round = Nr - 1; round > 0; round--)
    {
        InvShiftRows(state);
        InvSubBytes(state);
        AddRoundKey(state, expandedKeys + round * Nb * 4);
    }

    InvShiftRows(state);
    InvSubBytes(state);
    AddRoundKey(state, expandedKeys);

    for (int i = 0; i < 16; i++)
        output[i] = state[i % 4][i / 4];

    string result(reinterpret_cast<char*>(output), 16);
    // Remove padding
    size_t padding = static_cast<size_t>(output[15]);
    if (padding > 0 && padding <= 16)
        result.resize(result.size() - padding);

    return result;
}

string AESEncryptExtend(const string& inputStr, const string& keyStr)
{
    vector<string> blocks = SplitIntoBlocks(inputStr, BlockSize / 2);
    vector<string> encryptedBlocks;

    for (const auto& block : blocks)
        encryptedBlocks.push_back(AESEncrypt(block + "\0\0\0\0\0\0\0", keyStr));

    return JoinBlocks(encryptedBlocks);
}

string AESDecryptExtend(const string& encryptedStr, const string& keyStr)
{
    vector<string> blocks = SplitIntoBlocks(encryptedStr, BlockSize);
    vector<string> decryptedBlocks;

    for (const auto& block : blocks)
        decryptedBlocks.push_back(AESDecrypt(block + "\0\0\0\0\0\0\0", keyStr));

    return JoinBlocks(decryptedBlocks);
}

#if _WINDLL
extern "C"
{
    __declspec(dllexport) void Decode(const char* encryptedStr, char* outString)
    {
        string target = encryptedStr;
        string key = "";

        string result = AESDecryptExtend(target, key);

        strcpy(outString, result.c_str());
    }
}

#else
int main() 
{
    // 파일에서 키를 읽어옴
    string keyStr = ReadKeyFromFile("Encypt.txt");

    // 암호화할 입력 데이터 (최대 16바이트)
    string inputStr = "";

    // AES 암호화 수행
    string encryptedStr = AESEncryptExtend(inputStr, keyStr);

    // 암호화된 출력
    cout << "암호화된 출력: " << encryptedStr << endl;

    // AES 복호화 수행
    string decryptedStr = AESDecryptExtend(encryptedStr, keyStr);

    // 복호화된 출력
    cout << "복호화된 출력: " << decryptedStr << endl;

    return 0;
}
#endif