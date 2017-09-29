#include "stdafx.h"
#include "BaseMsg.h"


BaseMsg::BaseMsg(SString msgType)
{
	this->msgType = msgType;
}


BaseMsg::~BaseMsg()
{
}



TLV * BaseMsg::createMsgTypeTlv()
{
	return createTlvByStringValue(msgTypeTag, msgType);
}
TLV * BaseMsg::createEndTagTlv()
{
	return new TLV(endTag, 0);
}

int BaseMsg::encodeIntoWholeTlvs(list<TLV *> & tlvs)
{
	tlvs.push_back(createMsgTypeTlv());
	encodeParametersIntoTlvs(tlvs);
	tlvs.push_back(createEndTagTlv());

	return f_success;
}