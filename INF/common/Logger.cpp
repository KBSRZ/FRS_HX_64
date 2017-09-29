#include "stdafx.h"
#include "Logger.h"


Logger* Logger::instance = NULL;
LoggerLevel Logger::logLevel = LoggerLevelDebug;

void LOG_PRINT_BUFFER(void* buffer, int length)
{

	if (Logger::logLevel >LoggerLevelDefalt && Logger::logLevel<= LoggerLevelInfo && buffer  && length > 0)
	{
		unsigned char * _buffer = (unsigned char*)buffer;
		char out[20000] = { 0 };

		char temp[50] = { 0 };

		unsigned char oneLine[100] = { 0 };

		if (length > 2000)
		{
			length = 2000;
		}
		if (buffer && length > 0)
		{
			int i = 0;
			for (i; i < length; i++)
			{
				if (i % 16 == 0)
				{
					if (i > 0)
					{
						strcat(out, "|");
					}
					if (strlen((char *)oneLine) > 0)
					{
						strcat(out, (char *)oneLine);
					}

					strcat(out, "\n");
					sprintf(temp, "%04d:", i);
					strcat(out, temp);
					memset(oneLine, 0, 100);
				}

				sprintf(temp, "  %02x", _buffer[i]);
				oneLine[i % 16] = _buffer[i];
				if ((unsigned char)oneLine[i % 16] < (unsigned char)32 || (unsigned char)oneLine[i % 16] > (unsigned char)128)
				{
					oneLine[i % 16] = '.';
				}
				strcat(out, temp);
			}

			int tail = length % 16;
			//先补充16 - tail个空字符串
			for (int i = 0; i < 16 - tail; i++)
			{
				strcat(out, "    ");
			}

			strcat(out, "|");
			memset(oneLine, 0, 100);

			for (int i = 0; i < tail; i++)
			{
				oneLine[i] = _buffer[length - tail + i];
				if ((unsigned char)oneLine[i] < (unsigned char)32 || (unsigned char)oneLine[i] > (unsigned char)128)
				{
					oneLine[i] = '.';
				}
			}

			strcat(out, (char *)oneLine);
			LOG_INFO(_T("%hs"), out);
		}
	}
	
}

SString getCurrentTimeInChina()
{
	SYSTEMTIME st;

	GetLocalTime(&st);

	char strDate[100] = {0};

	_snprintf(strDate,99, "%04d-%02d-%02d %02d:%02d:%02d.%03d", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);

	SString ret = strDate;
	return ret;
}

Logger * Logger::sharedInstance()
{
	if (Logger::instance == NULL)
	{
		Logger::instance = new Logger();
	}

	return Logger::instance;
}
void Logger::cfg(LoggerLevel logLevel, SString logFile, int maxFileLength, int maxFileCount, LoggerMode mode)
{
	this->logLevel = logLevel;
	this->logFile = logFile;

	this->maxFileLength = maxFileLength;
	this->maxFileCount = maxFileCount;

	this->mode = mode;

	InitializeCriticalSection(&criticalSection);

	if (isSave2File())
	{
		doTransfer();
	}
	
	//BOOL logFileExist = FilePathExists(logFile.c_str());

	//_tfopen_s(&(this->fLogFile), logFile.c_str(), _T("ab+"));


#ifdef UNICODE
	if (!logFileExist)
	{
		WORD wSignature = 0xFEFF;
		fwrite(&wSignature, 2, 1, this->fLogFile);
		fflush(this->fLogFile);
	}
#endif



}


LoggerLevel Logger::getLogLevel()
{
	return Logger::logLevel;
}

SString Logger::getLogFile()
{
	return this->logFile;
}

FILE * Logger::getFLogFile()
{
	return fLogFile;
}

bool Logger::isCfged()
{

	return logFile.size() > 0 && logLevel > LoggerLevelDefalt && fLogFile != NULL;
}


void Logger::fileSizeIncreased(int size)
{
	EnterCriticalSection(&criticalSection);
	currentFileSize = currentFileSize + size;
	checkSize();
	LeaveCriticalSection(&criticalSection);
}

void Logger::checkSize()
{
	if (currentFileSize > maxFileLength*1024*1024)
	{
		doTransfer();
	}
}

bool Logger::isSave2File()
{
	return mode == LoggerModeOnlyFile || mode == LoggerModeOnlyBothFileAndTerminal;
}

bool Logger::isSave2Ternimal()
{
	return mode == LoggerModeOnlyTerminal || mode == LoggerModeOnlyBothFileAndTerminal;
}

void Logger::doTransfer()
{
	if (fLogFile)
	{
		fflush(fLogFile);
		fclose(fLogFile);
		fLogFile = NULL;
	}
	SString fileName = getLogFile();

	for (int i = maxFileCount; i >= 2; i--)
	{
		
		char temp[20] = { '\0' };

		sprintf_s(temp, 19, "%d", i);

		char temp2[20] = { '\0' };

		sprintf_s(temp2, 19, "%d", i - 1);


		SString iFileName = fileName;
		iFileName.insert(iFileName.find_last_of('.'), temp);

		SString i_1FileName = fileName;
		i_1FileName.insert(i_1FileName.find_last_of('.'), temp2);

		//第一步，删除掉iFileName，将i_1FileName重命名为iFileName
		if (FilePathExists(iFileName.c_str()))
		{
			remove(iFileName.c_str());
		}
		
		if (FilePathExists(i_1FileName.c_str()))
		{
			rename(i_1FileName.c_str(), iFileName.c_str());
		}
	}

	SString fileName1 = fileName;
	fileName1.insert(fileName1.find_last_of('.'), "1");

	_tfopen_s(&(this->fLogFile), fileName1.c_str(), _T("ab+"));

	currentFileSize = 0;
}

Logger::Logger()
{
	logFile = _T("");
	logLevel = LoggerLevelDefalt;
	fLogFile = NULL;

	maxFileLength = 0;

	maxFileCount = 0;

	currentFileSize = 0;

}


Logger::~Logger()
{
	if (fLogFile != NULL)
	{
		fclose(fLogFile);
	}
}
