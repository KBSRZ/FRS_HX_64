#include "stdafx.h"
#include "TLVEncodedObject.h"


TLVEncodedObject::TLVEncodedObject()
{
}


TLVEncodedObject::~TLVEncodedObject()
{
}



int TLVEncodedObject::encodeParametersIntoBuff(int & buffLen, byte *&buff)
{
	list<TLV *> tlvs;
	encodeParametersIntoTlvs(tlvs);

    buffLen = 0;
	for each (TLV* tlv in tlvs)
	{
		buffLen += tlv->getEncodedBufferLen();
	}

	buff = new byte[buffLen];
	int index = 0;
	for each (TLV* tlv in tlvs)
	{
		index += tlv->encodeIntoBuffer(buff + index);
		SafeDeleteObj(tlv);
	}

	return buffLen;
}

int TLVEncodedObject::decodeParametersFromTlvs(list<TLV *> & tlvs)
{
	for each (TLV* tlv in tlvs)
	{
		decodeParametersFromTlv(tlv);
	}
	return f_success;
}

void TLVEncodedObject::decodeParametersFromBuffer(byte * buffer, int bufflen)
{
	list<TLV *> tlvs;
	//LOG_PRINT_BUFFER(buffer, bufflen);

	TLV * temp = NULL;
	int index = 0;
	while (index < bufflen && (temp = TLV::decodeFromBuffer(buffer + index, bufflen - index)) != NULL)
	{
		index += temp->getEncodedBufferLen();
		tlvs.push_back(temp);
	}

	decodeParametersFromTlvs(tlvs);

	for each (TLV* tlv in tlvs)
	{
		SafeDeleteObj(tlv);
	}
}




