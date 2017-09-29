#pragma once

#pragma optimize( "", off )


#include <iostream>
#include <fstream>
#include <list>
#include <vector>
#include <map>
#include <direct.h>  
#include <io.h>  



#define ACCESS _access  
#define MKDIR(a) _mkdir((a))  

#define f_success 0
#define f_fail    -1

typedef unsigned char byte;

using namespace std;


struct cJSON;

#if 0
#ifdef _DEBUG  
#define New   new(_NORMAL_BLOCK, __FILE__, __LINE__)  
#endif  

#define CRTDBG_MAP_ALLOC  
#endif

typedef string SString;


SString & tirm(SString& strString);

typedef map<SString, SString> STRING_MAP;

typedef map<SString, SString>::iterator STRING_MAP_ITER;

typedef map<SString, SString>::const_iterator STRING_MAP_CONST_ITER;

typedef map<SString, SString>::value_type STRING_MAP_VALUETYPE;

BOOL FindFirstFileExists(LPCTSTR lpPath, DWORD dwFilter);

BOOL FilePathExists(LPCTSTR lpPath);

SString & GetStringFromPointer(SString &strString, char *pCStr);

void GetIntFromPointer(int &i, char * pCStr, int nullPointerDefaultValue);

void GetBoolFromPointer(bool &i, char * pCStr, bool nullPointerDefaultValue);

void GetFloatFromPointer(float &f, char * pCStr, float nullPointerDefaultValue);

SString int2Str(int i);

SString long2Str(long i);

SString float2Str(float f);


SString float2Str2(float f);
SString float2Str4(float f);

SYSTEMTIME parseTime(SString & timeStr);


SString sayTimeWithoutDay(SYSTEMTIME st);

SString sayTimeWithoutDay2(SYSTEMTIME st);

SString sayTimeWithoutSpace(SYSTEMTIME st);

SString sayTime(SYSTEMTIME st);

SString sayDay(SYSTEMTIME st);

SString sayHour(SYSTEMTIME st);

LONGLONG  miniSecondsBettwen(SYSTEMTIME st1, SYSTEMTIME st2);

LONGLONG  secondsBettwen(SYSTEMTIME st1, SYSTEMTIME st2);

cJSON * pointToJson(double x, double y);

int CreatDir(const char *pDir);

int getCpuCount();

#define SafeDeleteObj(obj) \
{\
if (obj)\
{\
	delete obj;\
	obj = NULL;\
}\
}

#define SafeDeleteArray(array) \
{\
if (array)\
{\
	delete []array;\
	array = NULL;\
}\
};


#define SafeCloseFile(file)\
{\
	if(file)\
	{\
		fflush(file);\
		fclose(file);\
		file = NULL;\
	}\
};
LPCTSTR SString2LPCTSTR(const SString & str);

#define EXEC_STATIC_PREPARE \
long exec_static_start = GetTickCount();\
int exec_static_count = 0;\
long exec_static_sum = 0

#define EXEC_STATIC_BEGIN \
exec_static_start = GetTickCount()

#define EXEC_STATIC_END(static_name,static_fire_count) \
	exec_static_count += 1;exec_static_sum +=GetTickCount() - exec_static_start;\
 if(exec_static_count == static_fire_count) { LOG_INFO(_T("%hs equal execute time:%ld ms in %d actions"),static_name, exec_static_sum/static_fire_count, static_fire_count); exec_static_count = 0;exec_static_sum = 0; }



#define  LIST_SORT_ASC(l) { l.sort(); }
#define  LIST_SORT_DESC(l) {l.sort(); l.reverse();}
