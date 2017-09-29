
#include "Feature.h"
#include "FaceImage.h"
#include <cv.h>
#include <cxcore.h>
#include <highgui.h>
#include "PubConstant.h"
using namespace FRS;
using namespace System;
using namespace System::Drawing;
short Feature::Init(int nChannelNum)
{
	return EF_Init(nChannelNum);
}

/*
The EF_Init function will initialize the Face Feature(THFeature) algorithm module

Parameters:
nChannelNum,the channel number,support for muti-thread,one channel stand for one thread.max value is 255.
Return Values:
If the function succeeds, the return value is valid channel number.
If the function fails, the return value is 0 or nagative;
Remarks:
This function can be called one time at program initialization.
*/

int Feature::Size()
{
	return EF_Size();
}
/*
The EF_Size function will return face feature size.

Parameters:
No parameter.
Return Values:
If the function succeeds, the return value is face feature size.
If the function fails, the return value is 0;
Remarks:
No remark.
*/
//int Feature::Extract(short nChannelID, array<BYTE>^ pGray, int nWidth, int nHeight, int nChannel, DWORD pFacialData, array<BYTE>^ pFeature)
//{
//	pin_ptr<BYTE> img_gray = &(pGray[0]);
//	BYTE *feature = new BYTE[Size()];
//	int status= EF_Extract(nChannelID, img_gray, nWidth, nHeight, nChannel, pFacialData, feature);
//	delete[]feature;
//	return  status;
//	
//}
//int Feature::Extract(short nChannelID, Image ^ imge, int nWidth, int nHeight, int nChannel, FRSFacePos &pFacialData, array<BYTE>^ pFeature)
//{
//	
//	
//	BYTE *feature = new BYTE[Size()];
//	int status = EF_Extract(nChannelID, img_gray, nWidth, nHeight, nChannel, pFacialData, feature);
//	delete[]feature;
//	return  status;
//
//}
int Feature::Extract(short nChannelID, cv::Mat &image, int nWidth, int nHeight, int nChannel, FRSFacePos &pFacialData,  array<BYTE>^ pFeature)
{
	
	THFI_FacePos fps = pFacialData.TO_THFI_FacePos();
	
	BYTE *feature = new BYTE[Size()];
	int status = EF_Extract(nChannelID, image.data, nWidth, nHeight, nChannel, (THFI_FacePos*)&fps, feature);
	Marshal::Copy((IntPtr)feature, pFeature, 0, Size());
	delete[]feature;
	
	return  status;
}
/*
The EF_Extract function execute face feature extraction.

Parameters:
nChannelID[input],channel ID(from 0 to nChannelNum-1)
pGray[input],point to an image buffer,BGR format.
nWidth[input],the image width.
nHeight[input],the image height.
nChannel[input],image buffer channel,must be 3
pFacialData[input],the face position rectangle data.
pFeatures[output],the face feature buffer
Return Values:
If the function succeeds, the return value is 1.
If the function fails, the return value is -1.
Remarks:
No remark.
*/

float Feature::Compare(array<BYTE>^ pFeature1, array<BYTE>^ pFeature2)
{
	pin_ptr<BYTE> img1 = &(pFeature1[0]);
	pin_ptr<BYTE> img2 = &(pFeature2[0]);
	return EF_Compare(img1, img2);
}
/*
The EF_Compare function execute two face features compare.

Parameters:
pFeature1[input],point to one face feature buffer.
pFeature2[input],point to another face feature buffer.
Return Values:
the return value is the two face features's similarity.
Remarks:
No remark.
*/

void Feature::Release()
{
	EF_Release();
}
/*
The EF_Release function will release the Face Feature (THFeature) algorithm module

Parameters:
No parameter.
Return Values:
No return value.
Remarks:
This function can be called one time at program Un-Initialization.
*/
 int Feature::Extract(short nChannelID, BYTE* pBuf, int nWidth, int nHeight, int nChannel, THFI_FacePos*  pFacialData, BYTE* pFeature)
{
	return EF_Extract(nChannelID, pBuf, nWidth, nHeight, nChannel, pFacialData, pFeature);
}
 int  Feature::Extract(short nChannelID, cv::Mat &image, int nWidth, int nHeight, int nChannel, THFI_FacePos*  pFacialData, array<BYTE>^ pFeature)
 {
	  BYTE *f = new BYTE[Size()];
	 
	  int status= EF_Extract(nChannelID, image.data, nWidth, nHeight, nChannel, pFacialData, f);
	  
	  Marshal::Copy((IntPtr)f, pFeature, 0, Size() );
	  delete[]f;

	  return status;
 }
/*
The EF_Extract function execute face feature extraction.

Parameters:
nChannelID[input],channel ID(from 0 to nChannelNum-1)
pGray[input],point to an image buffer,BGR format.
nWidth[input],the image width.
nHeight[input],the image height.
nChannel[input],image buffer channel,must be 3
pFacialData[input],the face position rectangle data.
pFeatures[output],the face feature buffer
Return Values:
If the function succeeds, the return value is 1.
If the function fails, the return value is -1.
Remarks:
No remark.
*/

 float Feature::Compare(BYTE* pFeature1, BYTE* pFeature2)
{
	return EF_Compare(pFeature1, pFeature2);
}
 float Feature::Compare(array<BYTE>^ pFeature1, BYTE* pFeature2)
 {
	 pin_ptr < BYTE> f1= &(pFeature1[0]);
	
	 return EF_Compare(f1, pFeature2);
 }
 float Feature::Compare(BYTE*pFeature1, array<BYTE>^ pFeature2)
 {
	 pin_ptr < BYTE> f2 = &(pFeature2[0]);

	 return EF_Compare(pFeature1, f2);
 }
/*
The EF_Compare function execute two face features compare.

Parameters:
pFeature1[input],point to one face feature buffer.
pFeature2[input],point to another face feature buffer.
Return Values:
the return value is the two face features's similarity.
Remarks:
No remark.
*/

 int Feature::Extract(BYTE* pBuf, int nWidth, int nHeight, int nChannel, THFI_FacePos*  pFacialData, BYTE* pFeature)
 {
	 short nChannelID = findFreeChannel();
	 if (nChannelID == ReturnCode::NO_FREE_CHANNEL)
	 {
		 Random ^ra = gcnew Random();
		 nChannelID = ra->Next(0, channelNum);
		 setBusyChannel(nChannelID);
	 }
	 return EF_Extract(nChannelID, pBuf, nWidth, nHeight, nChannel, pFacialData, pFeature);
 }
  int Feature::Extract(cv::Mat &image, int nWidth, int nHeight, int nChannel, FRSFacePos& pFacialData, array<BYTE>^ pFeature)
 {
	  short nChannelID = findFreeChannel();
	  if (nChannelID == ReturnCode::NO_FREE_CHANNEL)
	  {
		  Random ^ra = gcnew Random();
		  nChannelID = ra->Next(0, channelNum);
		  setBusyChannel(nChannelID);
	  }
	  THFI_FacePos fps = pFacialData.TO_THFI_FacePos();

	  BYTE *feature = new BYTE[Size()];
	  int status = EF_Extract(nChannelID, image.data, nWidth, nHeight, nChannel, (THFI_FacePos*)&fps, feature);
	  Marshal::Copy((IntPtr)feature, pFeature, 0, Size());
	  delete[]feature;

	  return  status;

 }
  int Feature::Extract(cv::Mat &image, int nWidth, int nHeight, int nChannel,THFI_FacePos*  pFacialData, array<BYTE>^ pFeature)
 {
	  short nChannelID = findFreeChannel();
	  if (nChannelID == ReturnCode::NO_FREE_CHANNEL)
	  {
		  Random ^ra = gcnew Random();
		  nChannelID = ra->Next(0, channelNum);
		  setBusyChannel(nChannelID);
	  }
	  BYTE *f = new BYTE[Size()];

	  int status = EF_Extract(nChannelID, image.data, nWidth, nHeight, nChannel, pFacialData, f);

	  Marshal::Copy((IntPtr)f, pFeature, 0, Size());
	  delete[]f;

	  return status;
 }

 short Feature::findFreeChannel()
 {
	 channelFlagsMutex->WaitOne();
	 int chnum = -1;
	 for (int i = 0; i < channelNum; i++){
		 if (channelFlags[i]){
			 chnum = i;
		 }
	 }

	 channelFlagsMutex->ReleaseMutex();
	 return chnum;

 }
 Int32 Feature::setFreeChannel(short channelID)
 {
	 if (channelID > channelNum - 1) return -1;
	 channelFlagsMutex->WaitOne();
	 channelFlags[channelID] = true;;
	 channelFlagsMutex->ReleaseMutex();
	 return ReturnCode::SUCCESS;
 }
 Int32 Feature::setBusyChannel(short channelID)
 {
	 if (channelID > channelNum - 1) return -1;
	 channelFlagsMutex->WaitOne();
	 channelFlags[channelID] = false;
	 channelFlagsMutex->ReleaseMutex();
	 return ReturnCode::SUCCESS;
 }