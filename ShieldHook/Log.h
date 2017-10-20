// log.h : contains the definition for the simple logging class.

#pragma once

#include "stdafx.h"
#include <stdint.h>
#include <iostream>
#include <stdio.h>
#include <cstdio>
#include <time.h>
#include <fstream>
enum LogLevel
{
	kError,
	kWarning,
	kInfo,
	kDebug
};

class Log
{
public:
	Log(char* filename, bool enabled);
	~Log();
	void Close();
	void Write(const char* message, LogLevel level = kInfo);
	void Writef(const char* format, uint32_t value);
	void Writef(const char* format, std::string value);

private:
	static char* ToString(LogLevel level);
	static LogLevel FromString(const std::string& level);
	static void NowTimeString(char* out);

private:
	bool logging_enabled_;
	std::ofstream out_file_stream_;
};

