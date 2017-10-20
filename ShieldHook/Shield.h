#include <iostream>
#include <string>
#include<tchar.h>
#include <process.h>
#include <windows.h>
#include <tlhelp32.h>
#include <fstream>
#include <streambuf>
#include "MD5.h"
#include <sstream>
using namespace std;
_declspec(dllexport) void RunShieldCpp();
string readFile(string filepath)
{
	std::ifstream t(filepath);
	std::ostringstream stream;
	string line;
	if(!t) 
	{
		cout << "NO!" << endl;
	}
	while (getline(t,line) ) {
      stream << line;
   }
	t.close();
  return stream.str();
}
std::string MagicEffectMd5() 
{
	string fileLines = readFile("ini/MagicEffect.ini");
	return md5(fileLines);
}
void CheckMD5()
{
	std::string magicEffectINI = MagicEffectMd5();
	if(magicEffectINI != "02e06d94703fd5a10932c215e42d3d45")
	{
		MessageBoxA(NULL,"Please don't play with MagicEffect.ini","Hmm",MB_OK);
		cout << "Magic Effect MD5 faild" << endl;
		exit(1);
	}
}
DWORD WINAPI W_Scan(LPVOID lpParameter){
again:
	RunShieldCpp();
	CheckMD5();
    Sleep(5000);
    goto again;
}

void RunShieldMain(){
	CreateThread(0,0,W_Scan,0,0,0);
}
