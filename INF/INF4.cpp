#include "stdafx.h"
#include "INF4.h"
#include "common/common.h"
#include "common/Logger.h"
#include "netserv/SaveImageReq.h"
#include "netserv/TcpConnectHandler.h"
#include "netserv/SaveImageResp.h"
using namespace  System;
using namespace System::Collections::Generic;
using namespace System::Net;
using namespace System::Text;
using namespace System::Threading::Tasks;
using  namespace System::IO;
using namespace System::Runtime::InteropServices;
using namespace INF;
using namespace System::Threading;


void INF4::Send(Object ^o)
{
	SendParam ^param = (SendParam ^)o;
	int ret = 0;
	SString ip = (char*)(void*)Marshal::StringToHGlobalAnsi((String^)this->ip);
	SString workStationCode = (char*)(void*)Marshal::StringToHGlobalAnsi((String^)param->defenseCode);
	SString equipmentCode = (char*)(void*)Marshal::StringToHGlobalAnsi((String^)param->cameraCode);
	SString strFileName = (char*)(void*)Marshal::StringToHGlobalAnsi((String^)param->fileName);
	SYSTEMTIME st = { 0 };
	GetLocalTime(&st);

	Logger::sharedInstance()->cfg((LoggerLevel)LoggerLevelInfo, "Logs/client.log", 200, 20, LoggerModeOnlyBothFileAndTerminal);
	//Logger::sharedInstance()->cfg((LoggerLevel)LoggerLevelInfo, "log/client.log", 200, 20, LoggerModeOnlyTerminal);


	byte * buffer = new byte[param->data->Length];
	Marshal::Copy(param->data, 0, System::IntPtr(buffer), param->data->Length);
	//buffer在request析构的时候自动释放
	SaveImageReq *send_req = new SaveImageReq(workStationCode, equipmentCode, (int)param->type, st, strFileName, buffer, param->data->Length);

	TcpConnectHandler::initSocket();
	TcpConnectHandler *handler = TcpConnectHandler::connect(ip, port);
	if (handler)
	{
		if (handler->sendMsg(send_req) == f_success)
		{
			LOG_INFO(_T("send SaveImageReq msg succeed"));
			SaveImageResp * read_resp = (SaveImageResp *)handler->readMsg();
			if (read_resp)
			{
				LOG_INFO(_T("read SaveImageReq msg succeed with handleResult:%d and resultIllustration:%s"), read_resp->handleResult, read_resp->resultIllustration);
			}
			else
			{
				LOG_ERROR(_T("read SaveImageReq msg failed"));
				ret= - 3;
			}
			SafeDeleteObj(read_resp);
		}
		else
		{
			LOG_ERROR(_T("send SaveImageReq msg failed"));
			ret= - 2;
		}
	}
	else
	{
		LOG_ERROR(_T("tcp connect failed"));
		ret= - 1;
	}

	SafeDeleteObj(handler);
	SafeDeleteObj(send_req);
	
}
int INF4::SendAsync(String ^defenseCode, String^ cameraCode, INF4::SaveImageType type, String ^fileName, array<Byte> ^data)
{
	SendParam ^param = gcnew SendParam(defenseCode, cameraCode, type, fileName, data);

	try{
		Thread ^thread = gcnew Thread(gcnew ParameterizedThreadStart(this, &INF4::Send));
		
		thread->Start(param);
	}
	catch (Exception ^e){
		return -1;
	}
	return 0;
}
