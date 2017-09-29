
#include <cv.h>
#include <cxcore.h>
#include <highgui.h>
#include "FaceImage.h"
#include "BitmapConverter.h"
#include "PubConstant.h"
using namespace FRS;
using namespace FRS::Util;

//////Struct define//////




//////API define//////


/******Create API******/

 int FaceImage::Create(short nChannelNum, FRSParam^ pParam)
{

	return THFI_Create( nChannelNum,  &(pParam->TO_THFI_Param()));
	channelNum = nChannelNum;
	channelFlags = gcnew array<bool>(nChannelNum);
	for (int i = 0; i < nChannelNum;i++){
		channelFlags[i] = true;
	}
}
/*
The THFI_Create function will initialize the algorithm engine module

Parameters:
nChannelNum[intput],algorithm channel num,for multi-thread mode,one thread uses one channel
pParam[input],algorithm engine parameter.
Return Values:
If the function succeeds, the return value is valid channel number.
If the function fails, the return value is zero or negative;
Remarks:
This function only can be called one time at program initialization.
*/

/******Detect API(only detect face)******/

int		FaceImage::DetectFace(short nChannelID, Image^ pImage, int bpp, array<FRSFacePos>^ pfps, int nMaxFaceNums)
{
	cv::Mat img = BitmapConverter::ToMat(pImage);
	
	
	
	/*cv::imshow("1", img);
	cv::waitKey();*/
	return FaceImage::DetectFace(nChannelID, img, bpp, img.cols, img.rows, pfps, nMaxFaceNums);

	//int nWidth = img.cols;
	//int nHeight = img.rows;

	////face detect
	//THFI_FacePos* ptfp = new THFI_FacePos[nMaxFaceNums];
	//int k;

	//for (k = 0; k<nMaxFaceNums; k++)
	//{
	//	ptfp[k].dwReserved = (DWORD)new BYTE[512];
	//}

	//int nNum = FaceImage::DetectFace(0, img.data, 24, nWidth, nHeight, ptfp, nMaxFaceNums);


	//for (int i = 0; i < nMaxFaceNums; i++)
	//{
	//	pfps[i] = FRSFacePos::TO_FRSFacePos(ptfp[i]);
	//}
	//RECT rcFace = ptfp[0].rcFace;

	//for (k = 0; k<nMaxFaceNums; k++)
	//{
	//	delete[](BYTE*)ptfp[k].dwReserved;
	//}


	//delete[]ptfp;

	//
	//return   nNum;
}


void	FaceImage::Release()
{
	THFI_Release();
}
/*
The THFI_Release function will release the algorithm engine module

Parameters:
No parameter.
Return Values:
No return value.
Remarks:
This function only can be called one time at program exit.
*/


int		FaceImage::Create(short nChannelNum, THFI_Param* pParam)
{
	return THFI_Create(nChannelNum, pParam);
}
/*
The THFI_Create function will initialize the algorithm engine module

Parameters:
nChannelNum[intput],algorithm channel num,for multi-thread mode,one thread uses one channel
pParam[input],algorithm engine parameter.
Return Values:
If the function succeeds, the return value is valid channel number.
If the function fails, the return value is zero or negative;
Remarks:
This function only can be called one time at program initialization.
*/

/******Detect API(only detect face)******/

int		FaceImage::DetectFace(short nChannelID, BYTE* pImage, int bpp, int nWidth, int nHeight, THFI_FacePos* pfps, int nMaxFaceNums)
{
	return THFI_DetectFace(nChannelID, pImage, bpp, nWidth, nHeight, pfps, nMaxFaceNums);
}




int		FaceImage::DetectFace(short nChannelID,cv::Mat &pImage, int bpp, int nWidth, int nHeight, array<FRSFacePos>^ pfps, int nMaxFaceNums)
{
	//pin_ptr<FRSFacePos> f = &(pfps[0]);
	THFI_FacePos * pfp = new THFI_FacePos[nMaxFaceNums];

	/*cv::imshow("2", pImage);
	cv::waitKey();*/
	

	int face_num = THFI_DetectFace(nChannelID, pImage.data, 24, nWidth, nHeight, pfp, nMaxFaceNums);
	
	


	for (int i = 0; i<face_num; i++)
	{
		pfps[i] = FRSFacePos::TO_FRSFacePos(pfp[i]);
		/*cv::Rect rectFace;
		rectFace.x = pfp[i].rcFace.left;
		rectFace.y = pfp[i].rcFace.top;
		rectFace.width = pfp[i].rcFace.right - pfp[i].rcFace.left + 1;
		rectFace.height = pfp[i].rcFace.bottom - pfp[i].rcFace.top + 1;
		rectangle(pImage, rectFace, cv::Scalar(0, 255, 0));
		cv::circle(pImage, cv::Point(pfp[i].ptLeftEye.x, pfp[i].ptLeftEye.y), 3, cv::Scalar(0, 255, 0), -1);
		cv::circle(pImage, cv::Point(pfp[i].ptRightEye.x, pfp[i].ptRightEye.y), 3, cv::Scalar(0, 255, 0), -1);
		cv::circle(pImage, cv::Point(pfp[i].ptMouth.x, pfp[i].ptMouth.y), 3, cv::Scalar(0, 255, 0), -1);
		cv::circle(pImage, cv::Point(pfp[i].ptNose.x, pfp[i].ptNose.y), 3, cv::Scalar(0, 255, 0), -1);*/
	}
	
	/*cv::imshow("3", pImage);
	cv::waitKey();*/






	delete[]pfp;
	return face_num;

}

int		FaceImage::DetectFace(BYTE* pImage, int bpp, int nWidth, int nHeight, THFI_FacePos* pfps, int nMaxFaceNums)
{
	short nChannelID = findFreeChannel();
	if (nChannelID == ReturnCode::NO_FREE_CHANNEL)
	{
		Random ^ra = gcnew Random();
		nChannelID = ra->Next(0, channelNum);
		setBusyChannel(nChannelID);
	}
	return THFI_DetectFace(nChannelID, pImage, bpp, nWidth, nHeight, pfps, nMaxFaceNums);

}
int FaceImage::DetectFace(cv::Mat &pImage, int bpp, int nWidth, int nHeight, array<FRSFacePos>^ pfps, int nMaxFaceNums)
{
	THFI_FacePos * pfp = new THFI_FacePos[nMaxFaceNums];



	short nChannelID = findFreeChannel();
	if (nChannelID == ReturnCode::NO_FREE_CHANNEL)
	{
		Random ^ra = gcnew Random();
		nChannelID=ra->Next(0, channelNum);
		setBusyChannel(nChannelID);
	}

	int face_num = THFI_DetectFace(nChannelID, pImage.data, 24, nWidth, nHeight, pfp, nMaxFaceNums);

	for (int i = 0; i < face_num; i++)
	{
		pfps[i] = FRSFacePos::TO_FRSFacePos(pfp[i]);
	}
	delete[]pfp;
	return face_num;
}
short FaceImage::findFreeChannel()
 {
	channelFlagsMutex->WaitOne();
	int chnum = -1;
	for (int i = 0; i < channelNum; i++){
		if (channelFlags[i]){
			chnum= i;
		}
	}
	
	channelFlagsMutex->ReleaseMutex();
	return chnum;

 }
Int32 FaceImage::setFreeChannel(short channelID)
{
	if (channelID > channelNum-1) return -1;
	channelFlagsMutex->WaitOne();
	channelFlags[channelID] = true;;
	channelFlagsMutex->ReleaseMutex();
	return ReturnCode::SUCCESS;
}
Int32 FaceImage::setBusyChannel(short channelID)
{
	if (channelID > channelNum - 1) return -1;
	channelFlagsMutex->WaitOne();
	channelFlags[channelID] = false;
	channelFlagsMutex->ReleaseMutex();
	return ReturnCode::SUCCESS;
}
 int		FaceImage::DetectFace(Image^ pImage, int bpp, array<FRSFacePos>^ pfps, int nMaxFaceNums)
{
	 cv::Mat img = BitmapConverter::ToMat(pImage);
	 return FaceImage::DetectFace( img, bpp, img.cols, img.rows, pfps, nMaxFaceNums);
}