#pragma once
#include "common/common.h"
#include "common/Logger.h"


extern const int endTag ;
extern const int msgTypeTag;

#define TLV_INT_LEN  4


class TLV
{
public:
	TLV(int tag, int length);
	~TLV();

	bool isEndTlv();

	bool isMsgTypeTlv();

	int getIntValue();

	int getEncodedBufferLen();

	int encodeIntoBuffer(byte * buffer);

	static TLV * decodeFromBuffer(byte * buffer, int bufferlen);

	int tag;
	int length;
	char * buffer;

	bool needReleaseBuffer = true;

};

TLV *createTlvByBufferValue(int tag, int length, byte * value);

TLV * createTlvByIntValue(int tag, int value);

TLV * createTlvByStringValue(int tag, const SString & value);

void releaseTlvs(list <TLV *>  & tlvs);
