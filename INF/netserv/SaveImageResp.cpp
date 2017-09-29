#include "stdafx.h"
#include "SaveImageResp.h"


SaveImageResp::SaveImageResp() :BaseMsg(MSG_TYPE_SAVE_IMAGE_RESP)
{
}

SaveImageResp::SaveImageResp(int handleResult, SString resultIllustration) : BaseMsg(MSG_TYPE_SAVE_IMAGE_RESP)
{
	this->handleResult = handleResult;
	this->resultIllustration = resultIllustration;
}


SaveImageResp::~SaveImageResp()
{
}


int SaveImageResp::decodeParametersFromTlv(TLV* tlv)
{
	if (tlv->tag == SaveImageRespTag_handleResult)
	{
		this->handleResult = tlv->getIntValue();
	}
	else if (tlv->tag == SaveImageRespTag_resultIllustration)
	{
		this->resultIllustration = tlv->buffer;
	}

	return f_success;
}

int SaveImageResp::encodeParametersIntoTlvs(list<TLV *> & tlvs)
{
	tlvs.push_back(createTlvByIntValue(SaveImageRespTag_handleResult, handleResult));
	tlvs.push_back(createTlvByStringValue(SaveImageRespTag_resultIllustration, resultIllustration));
	return f_success;
}