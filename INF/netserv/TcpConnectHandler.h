#pragma once

#include <winsock2.h>

#include "BaseMsg.h"

#include "TLV.h"

#include "SaveImageReq.h"

class TcpConnectHandler 
{
public:
	TcpConnectHandler(SOCKET connection);

	~TcpConnectHandler();


	static void initSocket();

	//socket 消息收发接口
	int readMsgType(SString &msgType);

	int readNextTlv(TLV *& tlv);

	int readMagNum();

	BaseMsg * readMsg();

	int sendMsg(BaseMsg * msg);

public:
	static TcpConnectHandler* connect(SString ip, int port);


public:
	SOCKET connection;

};

