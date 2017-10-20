// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <windows.h>
#include <iostream>
extern void csv3_init(HMODULE hModule);
extern void csv3_shield(HMODULE hModule);
bool debug = false;

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		DisableThreadLibraryCalls(hModule);
		if(debug)
		{
		AllocConsole();
		freopen("CONOUT$", "w", stdout);
		}
		csv3_init(hModule);
		csv3_shield(hModule);
		break;
	case DLL_PROCESS_DETACH:	
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:	
		break;
	}
	return TRUE;
}

