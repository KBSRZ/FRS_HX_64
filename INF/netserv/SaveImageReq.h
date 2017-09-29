#pragma once
#include "BaseMsg.h"

#define MSG_TYPE_SAVE_IMAGE_REQ "SaveImageReq"


typedef enum
{
	SaveImageType_Normal_Face = 1,

	SaveImageType_Alarm_Face = 2,

	SaveImageType_Alarm_CapturedImage = 3,

	SaveImageType_Alarm_CapturedVideo = 4

} SaveImageType;

typedef enum
{
	SaveImageReqTag_workStationCode = 1,

	SaveImageReqTag_equipmentCode = 2,

	SaveImageReqTag_type = 3,

	SaveImageReqTag_captureTime = 4,

	SaveImageReqTag_fileName = 5,

	SaveImageReqTag_fileData = 6,

} SaveImageReqTag;


class SaveImageReq :
	public BaseMsg
{
public:
	SaveImageReq();
	SaveImageReq(SString workStationCode, SString equipmentCode, int type, SYSTEMTIME captureTime, SString fileName, byte * buffer, long bufflen);

	~SaveImageReq();

	virtual int decodeParametersFromTlv(TLV* tlv);

	virtual int encodeParametersIntoTlvs(list<TLV *> & tlvs);

	//布防点CODE
	SString workStationCode;
	//摄像机CODE
	SString equipmentCode;
	/*摄像机CODE
	1：普通抓拍人脸；
	2：告警抓拍人脸；
	3：告警抓拍大图；
	4：告警抓拍视频
	*/
	int type = SaveImageType_Normal_Face;

	SYSTEMTIME captureTime;

	SString fileName;

	byte * buffer = NULL;

	long bufflen = 0;
	
};

