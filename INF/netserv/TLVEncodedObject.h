#pragma once

#include "TLV.h"


class TLVEncodedObject
{
public:
	TLVEncodedObject();
	~TLVEncodedObject();

	int encodeParametersIntoBuff(int & buffLen, byte *&buff);

	void decodeParametersFromBuffer(byte * buffer, int bufflen);

	int decodeParametersFromTlvs(list<TLV *> & tlvs);

	virtual int decodeParametersFromTlv(TLV* tlv) = 0;

	virtual int encodeParametersIntoTlvs(list<TLV *> & tlvs) = 0;
};

