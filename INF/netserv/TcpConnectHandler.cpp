#include "stdafx.h"
#include "TcpConnectHandler.h"

#include "SaveImageResp.h"







const char magicNum[] =  { '@', '_', 'M', 'N', 'B', 'G', '_', '@' };
#define MAGC_NUM_LEN (sizeof(magicNum))

const int headLength = 20;


const int tlvMaxLenght = 1024 * 1024 * 20; //20M
const int MaxPdu = 1000;

#pragma optimize( "", off )

int findMagNum(char *buffer, int bufferLen)
{
	
	if (buffer == NULL || bufferLen < MAGC_NUM_LEN)
	{
		LOG_ERROR(_T("invalid parameter, can't find magic num"));
		return -1;
	}

	bool found = false;
	int i = 0;
	for (i = 0; i < (bufferLen - MAGC_NUM_LEN); i++) {
		bool eq = true;
		for (int j = 0; j < MAGC_NUM_LEN && eq; j++) {
			if (buffer[i + j] != magicNum[j]) {
				eq = false;
				break;
			}
		}

		if (eq) {
			found = true;
			break;
		}
		else
		{
			continue;
		}
	}

	if (found) {
		return i;
	}
	else {
		return -1;
	}

}

int sendDataToSocket(SOCKET connection, char * buffer, int bufferLen)
{
	LOG_DEBUG(_T("send data buffer to socket with lenth:%d "), bufferLen);
	//LOG_PRINT_BUFFER(buffer, bufferLen);
	return send(connection, buffer, bufferLen, 0);
}

int sendHeadToSocket(SOCKET connection)
{
	LOG_DEBUG(_T("send head out"));
	char head[headLength];
	memset(head, 0, headLength);
	strncpy(head, magicNum, MAGC_NUM_LEN);

	return sendDataToSocket(connection, head, headLength) == headLength ? f_success : f_fail;
}

int sendIntValueToSocket(SOCKET connection, int value)
{

	LOG_DEBUG(_T("send int value to socket:%d"), value);
	byte buffer[TLV_INT_LEN];
	memset(buffer, 0, TLV_INT_LEN);
	buffer[0] = (byte)(value >> 24);
	buffer[1] = (byte)(value >> 16);
	buffer[2] = (byte)(value >> 8);
	buffer[3] = (byte)(value);

	return sendDataToSocket(connection, (char *)(buffer), TLV_INT_LEN);
}

int sendTlvToSocket(SOCKET connection, const TLV * const tlv)
{
	if (sendIntValueToSocket(connection, tlv->tag) == TLV_INT_LEN
		&& sendIntValueToSocket(connection, tlv->length) == TLV_INT_LEN
		&& sendDataToSocket(connection, tlv->buffer, tlv->length) == tlv->length
		)
	{
		return f_success;
	}
	else
	{
		return f_fail;
	}
}

int sentTlvsToSocket(SOCKET connection, list<TLV *> tlvs)
{
	list<TLV *>::iterator iter;
	for (iter = tlvs.begin(); iter != tlvs.end(); iter++)
	{
		if (sendTlvToSocket(connection, *iter) != f_success)
		{
			return f_fail;
		}
	}
	return f_success;
}

int readDataFromSocket(SOCKET connection, char * buffer, int bufferLen)
{
	int offSet = 0;
	int wanted = bufferLen;
	char * buff = buffer;

	if (buff != NULL) {
		while (wanted > 0) {
			int readed = 0;
			if (wanted > MaxPdu) {
			
				readed = recv(connection, buff, MaxPdu, 0);

			}
			else {
				readed = recv(connection, buff, wanted, 0);
			}

			if (readed <= 0) {
				LOG_ERROR(_T("读取socket数据失败，返回值：%d"), readed);
				return readed;
			}
			else
			{
				wanted = wanted - readed;
				offSet = offSet + readed;
				buff += readed;
				LOG_DEBUG(_T("从Socket读取到长度为:%d的数据, 剩余读取长度为：%d 的数据"), readed, wanted);
			}
		}
	}

	//LOG_PRINT_BUFFER(buffer, offSet);
	return offSet;
}


void TcpConnectHandler::initSocket()
{
	static bool inited = false;
	if (inited == false)
	{
		WSADATA wsaData;
		WSAStartup(MAKEWORD(2, 2), &wsaData);
		inited = true;
	}
}

TcpConnectHandler* TcpConnectHandler::connect(SString ip, int port)
{
	//WSADATA wsaData;
	//WSAStartup(MAKEWORD(2, 2), &wsaData);

	TcpConnectHandler::initSocket();

	SOCKET sclient = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
	if (sclient == INVALID_SOCKET)
	{
		LOG_ERROR(_T("invalid socket !"));
		return NULL;
	}

	sockaddr_in serAddr;
	serAddr.sin_family = AF_INET;
	serAddr.sin_port = htons(port);
	serAddr.sin_addr.s_addr = inet_addr(ip.c_str());
	memset(serAddr.sin_zero, 0x00, 8);
	if (::connect(sclient, (struct sockaddr *)&serAddr, sizeof(serAddr)) == SOCKET_ERROR)
	{
		LOG_ERROR(_T("try to connect to tpc server:%hs, port:%d failed"), ip.c_str(), port);
		closesocket(sclient);
		return NULL;
	}
	else
	{
		LOG_INFO(_T("try to connect to tpc server:%hs, port:%d  success"), ip.c_str(), port);
	}

	return new TcpConnectHandler(sclient);
}

TcpConnectHandler::TcpConnectHandler(SOCKET connection)
{

	this->connection = connection;
}


TcpConnectHandler::~TcpConnectHandler()
{
	//如果没有关闭socket，这里强制关闭下
	if (this->connection > 0)
	{
		closesocket(this->connection);
		this->connection = -1;
	}
}







BaseMsg * TcpConnectHandler::readMsg()
{

	SString msgType;
	BaseMsg * msg = NULL;
	if (f_success == readMagNum() && f_success == readMsgType(msgType))
	{
		if (msgType.compare(MSG_TYPE_SAVE_IMAGE_REQ) == 0)
		{
			msg = new SaveImageReq();
		}
		else if (msgType.compare(MSG_TYPE_SAVE_IMAGE_RESP) == 0)
		{
			msg = new SaveImageResp();
		}
		else
		{
			LOG_ERROR(_T("invalid msg type:%hs, not supported yet"), msgType.c_str());
			return NULL;
		}

		if (msg != NULL)
		{
			TLV * nextTlv = NULL;
			while (readNextTlv(nextTlv) == f_success && nextTlv != NULL && !nextTlv->isEndTlv())
			{
				msg->decodeParametersFromTlv(nextTlv);
				SafeDeleteObj(nextTlv);
			}
			SafeDeleteObj(nextTlv);
			return msg;
		}
		else
		{
			LOG_ERROR(_T("req is null, may msgtype:%hs decode not finished yet?"), msgType.c_str());
			return NULL;
		}
	}
	else
	{
		LOG_ERROR(_T("read req from socket failed"));
		return NULL;
	}


}


int TcpConnectHandler::sendMsg(BaseMsg * msg)
{
	if (sendHeadToSocket(connection) == f_success)
	{
		list<TLV *> tlvs; 
		if (msg->encodeIntoWholeTlvs(tlvs) == f_success && sentTlvsToSocket(connection, tlvs) == f_success)
		{
			releaseTlvs(tlvs);
			return f_success;
		}
		else
		{
			releaseTlvs(tlvs);
			LOG_ERROR(_T("try to send out msg into tlvs failed"));
			return f_fail;
		}
	}
	else
	{
		LOG_ERROR(_T("try to send out head failed, send msg failed"));
		return f_fail;
	}

}

int readInt32FromSocket(SOCKET connection, int & intValue)
{
	char buffer[TLV_INT_LEN];
	if (readDataFromSocket(connection, buffer, TLV_INT_LEN) == TLV_INT_LEN)
	{
		intValue = 0;
		byte bLoop;

		for (int i = 0; i < TLV_INT_LEN; i++) {
			bLoop = buffer[i];
			intValue = intValue * 256 + bLoop;
		}

		LOG_DEBUG(_T("read one int32 from socket with value:%d"), intValue);
		return f_success;
	}
	else
	{
		return f_fail;
	}
}


int TcpConnectHandler::readMagNum()
{
	char head[headLength];
	
	int readed;
	int iMagNumIndex;
	if ((readed = readDataFromSocket(connection, head, headLength)) == headLength)
	{
		iMagNumIndex = findMagNum(head, headLength);
		while (iMagNumIndex == -1)
		{
			LOG_WARN(_T("获取MagicNum失败，继续读取"));
			for (int end = headLength - MAGC_NUM_LEN; end < headLength; end++) {
				int begin = 0;
				head[end] = head[begin];
				begin++;
			}

			readed = readDataFromSocket(connection, head + MAGC_NUM_LEN, headLength - MAGC_NUM_LEN);
			if (readed == headLength - MAGC_NUM_LEN)
			{
				iMagNumIndex = findMagNum(head, headLength);
			}
			else
			{
				LOG_ERROR(_T("socket error, readMagNum failed"));
				return f_fail;
			}
		}
		LOG_DEBUG(_T("read magic num success at index:%d"), iMagNumIndex);
		return f_success;
	}

	else
	{
		LOG_ERROR(_T("read head with return value:%d, not eqaul to headlenth:%d, read magNum failed"), readed, headLength);
		return f_fail;
	}
}

int TcpConnectHandler::readNextTlv(TLV *& tlv)
{
	SafeDeleteObj(tlv);

	int tag;
	int length;
	if (f_success == readInt32FromSocket(connection, tag) && f_success == readInt32FromSocket(connection, length))
	{
		tlv = new TLV(tag, length);
		if (readDataFromSocket(connection, tlv->buffer, tlv->length) != tlv->length)
		{
			SafeDeleteObj(tlv);
			return f_fail;
		}
		else
		{
			return f_success;
		}
	}
	else
	{
		return f_fail;
	}
}



int TcpConnectHandler::readMsgType(SString &msgType)
{
	TLV * tlv = NULL;
	if (f_success == readNextTlv(tlv))
	{
		if (tlv->isMsgTypeTlv())
		{
			msgType = tlv->buffer;
			SafeDeleteObj(tlv);
			return f_success;
		}
		else
		{
			SafeDeleteObj(tlv);
			LOG_ERROR(_T("read msg type failed, tag is not msgtype"));
			return f_fail;
		}
		
	}
	else
	{
		SafeDeleteObj(tlv);
		LOG_ERROR(_T("read msg type failed, read next tlv failed"));
		return f_fail;
	}
}


#pragma optimize( "", on ) 
