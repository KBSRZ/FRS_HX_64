#include "stdafx.h"
#include "SaveImageReq.h"


SaveImageReq::SaveImageReq() :BaseMsg(MSG_TYPE_SAVE_IMAGE_REQ)
{
}

SaveImageReq::SaveImageReq(SString workStationCode, SString equipmentCode, int type, SYSTEMTIME captureTime, SString fileName, byte * buffer, long bufflen) : BaseMsg(MSG_TYPE_SAVE_IMAGE_REQ)
{
	this->workStationCode = workStationCode;
	this->equipmentCode = equipmentCode;
	this->type = type;
	this->captureTime = captureTime;
	this->fileName = fileName;
	this->buffer = buffer;
	this->bufflen = bufflen;
}


SaveImageReq::~SaveImageReq()
{
	SafeDeleteArray(buffer);
	bufflen = 0;
}

int SaveImageReq::decodeParametersFromTlv(TLV* tlv)
{
	if (tlv->tag == SaveImageReqTag_workStationCode)
	{
		this->workStationCode = tlv->buffer;
	}
	else if (tlv->tag == SaveImageReqTag_equipmentCode)
	{
		this->equipmentCode = tlv->buffer;
	}
	else if (tlv->tag == SaveImageReqTag_type)
	{
		this->type = tlv->getIntValue();
	}
	else if (tlv->tag == SaveImageReqTag_captureTime)
	{
		SString captureTimeStr = tlv->buffer;

		this->captureTime = parseTime(captureTimeStr);
	}
	else if (tlv->tag == SaveImageReqTag_fileName)
	{
		this->fileName = tlv->buffer;
	}
	else if (tlv->tag == SaveImageReqTag_fileData)
	{
		this->buffer = (byte *)tlv->buffer;
		this->bufflen = tlv->length;
		tlv->needReleaseBuffer = false;
	}

	return f_success;
}

int SaveImageReq::encodeParametersIntoTlvs(list<TLV *> & tlvs)
{
	tlvs.push_back(createTlvByStringValue(SaveImageReqTag_workStationCode, workStationCode));
	tlvs.push_back(createTlvByStringValue(SaveImageReqTag_equipmentCode, equipmentCode));
	tlvs.push_back(createTlvByIntValue(SaveImageReqTag_type, type));
	tlvs.push_back(createTlvByStringValue(SaveImageReqTag_captureTime, sayTime(captureTime)));
	tlvs.push_back(createTlvByStringValue(SaveImageReqTag_fileName, fileName));
	tlvs.push_back(createTlvByBufferValue(SaveImageReqTag_fileData, bufflen, buffer));
	return f_success;
}
