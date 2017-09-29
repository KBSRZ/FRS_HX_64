#pragma once
//#include <windows.h>
#include "common.h"
typedef enum {
	LoggerLevelDefalt = -1, 
	LoggerLevelTrace = 0, 
	LoggerLevelDebug, 
	LoggerLevelInfo, 
	LoggerLevelWarn, 
	LoggerLevelError, 
	LoggerLevelFatal, 

}LoggerLevel;


typedef enum {
	LoggerModeOnlyFile = 1,
	LoggerModeOnlyTerminal = 2,
	LoggerModeOnlyBothFileAndTerminal = 3
}LoggerMode;

SString getCurrentTimeInChina();


class Logger
{
public:
	Logger();
	~Logger();

	static Logger * sharedInstance();

	void cfg(LoggerLevel logLevel, SString logFile, int maxFileLenth, int maxFileCount, LoggerMode mode = LoggerModeOnlyFile);

	LoggerLevel getLogLevel();

	SString getLogFile();

	FILE* getFLogFile();

	bool isCfged();

	void fileSizeIncreased(int size);

	void doTransfer(); 

	bool isSave2File();

	bool isSave2Ternimal();

private:
	void checkSize();

public:
	static LoggerLevel logLevel;

private:
	static Logger * instance;

	SString logFile;

	FILE * fLogFile;

	int maxFileLength;

	int maxFileCount;

	int currentFileSize;

	CRITICAL_SECTION criticalSection;

	LoggerMode mode;
};


#define LOG_TRACE(FMT, ...) \
{\
	if (Logger::sharedInstance()->isCfged()  && Logger::sharedInstance()->getLogLevel() <= LoggerLevelTrace) {\
		if(Logger::sharedInstance()->isSave2File()){\
			FILE * f = Logger::sharedInstance()->getFLogFile(); \
			if (f)\
						{\
				Logger::sharedInstance()->fileSizeIncreased(_ftprintf(f, _T("%s p[%d]*TRACE: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__)); \
				fflush(f);\
						}\
				}\
				if(Logger::sharedInstance()->isSave2Ternimal())\
				{\
					_tprintf(_T("%hs p[%d]*TRACE: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__);\
				}\
		}\
}


#define LOG_DEBUG(FMT, ...) \
{\
	if (Logger::sharedInstance()->isCfged() && Logger::sharedInstance()->getLogLevel()  <= LoggerLevelDebug) {\
		if(Logger::sharedInstance()->isSave2File()){\
			FILE * f = Logger::sharedInstance()->getFLogFile(); \
			if (f)\
						{\
				Logger::sharedInstance()->fileSizeIncreased(_ftprintf(f, _T("%s p[%d]*DEBUG: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__)); \
				fflush(f);\
						}\
				}\
				if(Logger::sharedInstance()->isSave2Ternimal())\
				{\
					_tprintf(_T("%hs p[%d]*DEBUG: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__);\
				}\
		}\
}

#define LOG_INFO(FMT, ...) \
{\
	if (Logger::sharedInstance()->isCfged() && Logger::sharedInstance()->getLogLevel()  <= LoggerLevelInfo) {\
		if(Logger::sharedInstance()->isSave2File()){\
			FILE * f = Logger::sharedInstance()->getFLogFile(); \
			if (f)\
						{\
				Logger::sharedInstance()->fileSizeIncreased(_ftprintf(f, _T("%s p[%d]*INFO: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__)); \
				fflush(f);\
						}\
				}\
				if(Logger::sharedInstance()->isSave2Ternimal())\
				{\
					_tprintf(_T("%hs p[%d]*INFO: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__);\
				}\
		}\
}

#define LOG_WARN(FMT, ...) \
{\
	if (Logger::sharedInstance()->isCfged() && Logger::sharedInstance()->getLogLevel()  <= LoggerLevelWarn) {\
		if(Logger::sharedInstance()->isSave2File()){\
			FILE * f = Logger::sharedInstance()->getFLogFile(); \
			if (f)\
						{\
				Logger::sharedInstance()->fileSizeIncreased(_ftprintf(f, _T("%s p[%d]*WARN: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__)); \
				fflush(f);\
						}\
				}\
				if(Logger::sharedInstance()->isSave2Ternimal())\
				{\
					_tprintf(_T("%hs p[%d]*WARN: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__);\
				}\
		}\
}


#define LOG_ERROR(FMT, ...) \
{\
	if (Logger::sharedInstance()->isCfged() && Logger::sharedInstance()->getLogLevel()  <= LoggerLevelError) {\
		if(Logger::sharedInstance()->isSave2File()){\
			FILE * f = Logger::sharedInstance()->getFLogFile(); \
			if (f)\
						{\
				Logger::sharedInstance()->fileSizeIncreased(_ftprintf(f, _T("%s p[%d]*ERROR: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__)); \
				fflush(f);\
						}\
				}\
				if(Logger::sharedInstance()->isSave2Ternimal())\
				{\
					_tprintf(_T("%hs p[%d]*ERROR: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__);\
				}\
		}\
}

#define LOG_FATAL(FMT, ...) \
{\
	if (Logger::sharedInstance()->isCfged() && Logger::sharedInstance()->getLogLevel()  <= LoggerLevelError) {\
		if(Logger::sharedInstance()->isSave2File()){\
			FILE * f = Logger::sharedInstance()->getFLogFile(); \
			if (f)\
									{\
				Logger::sharedInstance()->fileSizeIncreased(_ftprintf(f, _T("%s p[%d]*FALTAL: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__)); \
				fflush(f);\
									}\
						}\
				if(Logger::sharedInstance()->isSave2Ternimal())\
								{\
					_tprintf(_T("%hs p[%d]*FALTAL: function: \"%s()\" file: \"%s\" line: \"%u\" MSG: ") FMT _T("\n"), getCurrentTimeInChina().c_str(), ::GetCurrentThreadId(), _T(__FUNCTION__), _T(__FILE__), __LINE__, ##__VA_ARGS__);\
								}\
			}\
}

void LOG_PRINT_BUFFER(void* buffer, int length);

