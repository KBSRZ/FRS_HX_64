#pragma once
#include<windows.h>
#include <cv.h>
#include <cxcore.h>
#include <highgui.h>
#include <THFaceImage_i.h>
#include <THFeature_i.h>
#include "FaceImage.h"
namespace FRS {
	public ref class  Feature{
	public:
		static short Init(int nChannelNum);
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

		static int Size();
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
		//static int Extract(short nChannelID, array<BYTE>^ pGray, int nWidth, int nHeight, int nChannel, DWORD pFacialData, array<BYTE>^ pFeature);

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

		static float Compare(array<BYTE>^ pFeature1, array<BYTE>^ pFeature2);

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

		static void Release();
		/*
		The EF_Release function will release the Face Feature (THFeature) algorithm module

		Parameters:
		No parameter.
		Return Values:
		No return value.
		Remarks:
		This function can be called one time at program Un-Initialization.
		*/
	internal:

		static int Extract(short nChannelID, BYTE* pBuf, int nWidth, int nHeight, int nChannel, THFI_FacePos*  pFacialData, BYTE* pFeature);
		static int Extract(short nChannelID, cv::Mat &image, int nWidth, int nHeight, int nChannel, FRSFacePos& pFacialData, array<BYTE>^ pFeature);
		static int Extract(short nChannelID, cv::Mat &image, int nWidth, int nHeight, int nChannel, THFI_FacePos*  pFacialData, array<BYTE>^ pFeature);

		static int Extract(BYTE* pBuf, int nWidth, int nHeight, int nChannel, THFI_FacePos*  pFacialData, BYTE* pFeature);
		static int Extract(cv::Mat &image, int nWidth, int nHeight, int nChannel, FRSFacePos& pFacialData, array<BYTE>^ pFeature);
		static int Extract(cv::Mat &image, int nWidth, int nHeight, int nChannel, THFI_FacePos*  pFacialData, array<BYTE>^ pFeature);
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

		static float Compare(BYTE* pFeature1, BYTE* pFeature2);

		static float Feature::Compare(BYTE*pFeature1, array<BYTE>^ pFeature2);
		static float Feature::Compare(array<BYTE>^ pFeature1, BYTE* pFeature2);
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

	private:
		static short channelNum = 0;
		static Mutex ^ channelFlagsMutex = gcnew Mutex();
		static array<bool>^channelFlags;//标志进程是否被空闲

		static short findFreeChannel();//找到空闲的一个通道，并且将对应的标志位值置为false;都不空闲返回-1；
		static Int32 setFreeChannel(short channelID);//channelFlags 对应位置为true；	 
		static Int32 setBusyChannel(short channelID);//channelFlags 对应位置为false；	 

	};

}