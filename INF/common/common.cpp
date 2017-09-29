#include "stdafx.h"
#include "common/common.h"
#include "common/Logger.h"



SString & tirm(SString& strString)
{
	SString strBuff(strString);
	char cSpace = ' ';
	strString.assign(strBuff.begin() + strBuff.find_first_not_of(cSpace), 
		strBuff.begin() + strBuff.find_last_not_of(cSpace) + 1);

	return strString;
}

/*
* 检查一个  路径 是否存在（绝对路径、相对路径，文件或文件夹均可）
* 存在则返回 1 (TRUE)
*/
BOOL FilePathExists(LPCTSTR lpPath)
{
	return FindFirstFileExists(lpPath, FALSE);
}

BOOL FindFirstFileExists(LPCTSTR lpPath, DWORD dwFilter)
{
	WIN32_FIND_DATA fd;
	HANDLE hFind = FindFirstFile(lpPath, &fd);
	BOOL bFilter = (FALSE == dwFilter) ? TRUE : fd.dwFileAttributes & dwFilter;
	BOOL RetValue = ((hFind != INVALID_HANDLE_VALUE) && bFilter) ? TRUE : FALSE;
	FindClose(hFind);
	return RetValue;
}

SYSTEMTIME parseTime(SString & timeStr)
{
	SYSTEMTIME st;
	sscanf(timeStr.c_str(), "%04d-%02d-%02d %02d:%02d:%02d.%03d", &st.wYear, &st.wMonth, &st.wDay, &st.wHour, &st.wMinute, &st.wSecond, &st.wMilliseconds);

	return st;
}

SString sayTime(SYSTEMTIME st)
{

	char strDate[50] = { '\0' };
	_snprintf(strDate, 49, "%04d-%02d-%02d %02d:%02d:%02d.%03d", st.wYear, st.wMonth, st.wDay, st.wHour, st.wMinute, st.wSecond, st.wMilliseconds);
	return strDate;
}

SString int2Str(int i)
{
	char buffer[20];
	sprintf_s(buffer, "%d", i);
	
	SString ret(buffer);
	return ret;
}
SString long2Str(long i)
{
	char buffer[40];
	sprintf_s(buffer, "%ld", i);

	SString ret(buffer);
	return ret;
}

SString float2Str(float f)
{
	char buffer[40];
	sprintf_s(buffer, "%f", f);

	SString ret(buffer);
	return ret;
}

//小数点后面只取两位
SString float2Str2(float f)
{
	f = f - 0.005;
	char buffer[40];

	sprintf_s(buffer, "%.02f", f);

	SString ret(buffer);
	return ret;
}

SString float2Str4(float f)
{
	f = f - 0.00005;
	char buffer[40];
	sprintf_s(buffer, "%.04f", f);

	SString ret(buffer);
	return ret;
}

void GetIntFromPointer(int &i, char * pCStr, int nullPointerDefaultValue)
{
	if (pCStr == NULL)
	{
		i = nullPointerDefaultValue;
	}
	else
	{
		i = atoi(pCStr);
	}
}

void GetBoolFromPointer(bool &i, char * pCStr, bool nullPointerDefaultValue)
{
	if (pCStr == NULL)
	{
		i = nullPointerDefaultValue;
	}
	else
	{
		i = (atoi(pCStr) != 0);
	}
}

void GetFloatFromPointer(float &f, char * pCStr, float nullPointerDefaultValue)
{
	if (pCStr == NULL)
	{
		f = nullPointerDefaultValue;
	}
	else
	{
		f = atof(pCStr);
	}
}


SString & GetStringFromPointer(SString &strString, char *pCStr)
{
	if (NULL == pCStr)
	{
		strString = "";
	}
	else
	{
		strString = pCStr;
	}
	return strString;
};




LPCTSTR SString2LPCTSTR(const SString & str)
{
	
#ifdef UNICODE

	int bufferLen = str.length() * 4;
	wchar_t * dBuf = new wchar_t[bufferLen];
	wmemset(dBuf, 0, bufferLen);

	_stprintf_s(dBuf, bufferLen - 1, _T("%hs"), str.c_str());
	return dBuf;

	#if 0
	//进行转换  
	int nRet = MultiByteToWideChar(CP_ACP, 0, str.c_str(), str.length(), dBuf, dBufSize);

	if (nRet <= 0)
	{
		
		DWORD dwErr = GetLastError();
		switch (dwErr)
		{
		case ERROR_INSUFFICIENT_BUFFER:
			LOG_ERROR(_T("尝试转码%hs到UNICODE失败:%hs"), str.c_str(), "ERROR_INSUFFICIENT_BUFFER");
			SafeDeleteArray(dBuf);
			break;
		case ERROR_INVALID_FLAGS:
			LOG_ERROR(_T("尝试转码%hs到UNICODE失败:%hs"), str.c_str(), "ERROR_INVALID_FLAGS");\
			SafeDeleteArray(dBuf);
			break;
		case ERROR_INVALID_PARAMETER:
			LOG_ERROR(_T("尝试转码%hs到UNICODE失败:%hs"), str.c_str(), "ERROR_INVALID_PARAMETER");
			SafeDeleteArray(dBuf);
			break;
		case ERROR_NO_UNICODE_TRANSLATION:
			LOG_ERROR(_T("尝试转码%hs到UNICODE失败:%hs"), str.c_str(), "ERROR_NO_UNICODE_TRANSLATION");
			SafeDeleteArray(dBuf);
			break;
		}
	}

	return dBuf;
	#endif

#else

	char * buff = new char [str.length() + 1];
	memset(buff, 0, str.length() + 1);
	strcpy(buff, str.c_str());
	return buff;
#endif

}

