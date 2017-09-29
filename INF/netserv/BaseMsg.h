#pragma once

#include "common/common.h"
#include "common/Logger.h"

#include "TLVEncodedObject.h"

class BaseMsg : public TLVEncodedObject
{
public:
	BaseMsg(SString msgType);
	~BaseMsg();

	SString msgType;
	
	int encodeIntoWholeTlvs(list<TLV *> & tlvs);

	TLV * createMsgTypeTlv();
	TLV * createEndTagTlv();

};

