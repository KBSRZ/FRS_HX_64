#include "stdafx.h"
#include "TLV.h"

const int endTag = 9909;
const int msgTypeTag = 1000;


void releaseTlvs(list <TLV *> &  tlvs)
{
	list <TLV *>::iterator iter;
	for (iter = tlvs.begin(); iter != tlvs.end(); iter++)
	{
		TLV * tlv = *iter;
		SafeDeleteObj(tlv);
	}
	tlvs.clear();
}

TLV * createTlvByIntValue(int tag, int value)
{
	TLV * ret = new TLV(tag, TLV_INT_LEN);
	memset(ret->buffer, 0, TLV_INT_LEN);
	ret->buffer[0] = ((byte*)(&value))[3];
	ret->buffer[1] = ((byte*)(&value))[2];
	ret->buffer[2] = ((byte*)(&value))[1];
	ret->buffer[3] = ((byte*)(&value))[0];
	return ret;
}

TLV *createTlvByBufferValue(int tag, int length, byte * value)
{
	TLV * ret = new TLV(tag, length);
	
	memcpy(ret->buffer, value, length);

	return ret;
}
TLV * createTlvByStringValue(int tag, const SString & value)
{
	TLV * ret = new TLV(tag, value.length());
	memcpy(ret->buffer, value.c_str(), value.length());
	return ret;
}


TLV::TLV(int tag, int length)
{
	this->tag = tag;
	this->length = length;
	if (length > 0)
	{
		this->buffer = new char[length + 1]; //给c字符串多一点保护
		memset(buffer, 0, length + 1);
	}
	else
	{
		this->buffer = NULL;
	}

}

bool TLV::isEndTlv()
{
	return this->tag == endTag;
}


bool TLV::isMsgTypeTlv()
{
	return this->tag == msgTypeTag;
}


int TLV::getIntValue()
{
	int ret;
	((byte*)(&ret))[3] = buffer[0];
	((byte*)(&ret))[2] = buffer[1];
	((byte*)(&ret))[1] = buffer[2];
	((byte*)(&ret))[0] = buffer[3];
	return ret;
}

int TLV::getEncodedBufferLen()
{
	return TLV_INT_LEN + TLV_INT_LEN + length;
}

TLV *  TLV::decodeFromBuffer(byte * buffer, int bufferlen)
{
	if (bufferlen < 8) //tag and len都需要八位
	{
		return NULL;
	}
	else
	{
		int tag = 0;
		byte bLoop;

		for (int i = 0; i < 4; i++) {
			bLoop = buffer[i];
			tag = tag * 256 + bLoop;
		}

		int length = 0;
		for (int i = 4; i < 8; i++) {
			bLoop = buffer[i];
			length = length * 256 + bLoop;
		}
		
		if (bufferlen - 8 < length)
		{
			return NULL;
		}
		else
		{
			TLV * ret = new TLV(tag, length);
			memcpy(ret->buffer, buffer + 8, length);
			return ret;
		}
	}
}
int TLV::encodeIntoBuffer(byte * buf)
{

	buf[0] = (byte)(tag >> 24);
	buf[1] = (byte)(tag >> 16);
	buf[2] = (byte)(tag >> 8);
	buf[3] = (byte)(tag);

	buf += TLV_INT_LEN;

	buf[0] = (byte)(length >> 24);
	buf[1] = (byte)(length >> 16);
	buf[2] = (byte)(length >> 8);
	buf[3] = (byte)(length);

	buf += TLV_INT_LEN;

	memcpy(buf, buffer, length);

	return TLV_INT_LEN + TLV_INT_LEN + length;
}

TLV::~TLV()
{
	if (needReleaseBuffer)
	{
		SafeDeleteArray(buffer);
	}
}
