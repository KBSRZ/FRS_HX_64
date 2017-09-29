#pragma once
#include "BaseMsg.h"

#define MSG_TYPE_SAVE_IMAGE_RESP "SaveImageResp"

typedef enum
{
	SaveImageRespTag_handleResult = 1,

	SaveImageRespTag_resultIllustration = 2

} SaveImageRespTag;


typedef enum
{
	SaveImageRespResult_success = 200,

	SaveImageRespResult_fail = 201,

}SaveImageRespResult;

class SaveImageResp :
	public BaseMsg
{
public:
	SaveImageResp();
	SaveImageResp(int handleResult, SString resultIllustration);
	~SaveImageResp();

	virtual int decodeParametersFromTlv(TLV* tlv);

	virtual int encodeParametersIntoTlvs(list<TLV *> & tlvs);

	//处理结果
	int handleResult;

	//结果说明
	SString resultIllustration;
};

