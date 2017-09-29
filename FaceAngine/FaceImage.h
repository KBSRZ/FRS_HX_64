#pragma once
#include<windows.h>
#include <cv.h>
#include <cxcore.h>
#include <highgui.h>

#include <THFaceImage_i.h>
#include <THFeature_i.h>
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Text;
using namespace System::Runtime::InteropServices;
using namespace System::Drawing;
using namespace System::Threading;
namespace FRS {

	public value struct FRSFaceAngle
	{
		int   yaw;//angle of yaw,from -90 to +90,left is negative,right is postive
		int   pitch;//angle of pitch,from -90 to +90,up is negative,down is postive
		int   roll;//angle of roll,from -90 to +90,left is negative,right is postive
		
	internal:
		FaceAngle TO_THFI_FaceAngle()
		{
			FaceAngle faceAngle;
			faceAngle.pitch = pitch;
			faceAngle.roll = roll;
			faceAngle.yaw = yaw;
			return faceAngle;
		}
		static FRSFaceAngle TO_FRSFaceAngle(const FaceAngle &faceAngle)
		{
			FRSFaceAngle frsfaceAngle ;
			frsfaceAngle.pitch = faceAngle.pitch;
			frsfaceAngle.roll = faceAngle.roll;
			frsfaceAngle.yaw = faceAngle.yaw;
			return frsfaceAngle;
		}
	};
	public value struct  FRSRECT
	{
		LONG    left;
		LONG    top;
		LONG    right;
		LONG    bottom;
	internal:
		RECT TO_THFI_RECT()
		{
			RECT rect;
			rect.bottom = bottom;
			rect.left = left;
			rect.right = right;
			rect.top = top;
			return rect;
		}
		static FRSRECT TO_FRSRECT(const RECT &rect)
		{
			FRSRECT frsrect ;
			frsrect.bottom = rect.bottom;
			frsrect.left = rect.left;
			frsrect.right = rect.right;
			frsrect.top = rect.top;
			return frsrect;
		}
	};
	public value struct FRSPOINT
	{
		LONG  x;
		LONG  y;
	internal:
		POINT TO_THFI_POINT()
		{
			POINT point;
			point.x = x;
			point.y = y;
			return point;
		}
		static FRSPOINT TO_FRSPOINT(const POINT &point)
		{
			FRSPOINT frspoint;
			frspoint.x = point.x;
			frspoint.y = point.y;
			return frspoint;
		}
	};
	public value struct FRSFacePos
	{
		FRSRECT			rcFace ;//coordinate of face
		FRSPOINT		ptLeftEye;//coordinate of left eye
		FRSPOINT		ptRightEye ;//coordinate of right eye
		FRSPOINT		ptMouth  ;//coordinate of mouth
		FRSPOINT		ptNose ;//coordinate of nose								
		FRSFaceAngle	fAngle ;//value of face angle
		int			nQuality;//quality of face(from 0 to 100)
	internal:
		THFI_FacePos TO_THFI_FacePos()
		{
			THFI_FacePos facepos;
			facepos.rcFace = rcFace.TO_THFI_RECT();
			facepos.ptLeftEye = ptLeftEye.TO_THFI_POINT();
			facepos.ptRightEye = ptRightEye.TO_THFI_POINT();
			facepos.ptMouth = ptMouth.TO_THFI_POINT();
			facepos.ptNose = ptNose.TO_THFI_POINT();
			facepos.fAngle = fAngle.TO_THFI_FaceAngle();
			facepos.nQuality = nQuality;
			
			return facepos;
		}
		static FRSFacePos TO_FRSFacePos(const THFI_FacePos &facepos)
		{
			FRSFacePos frsfacepos ;
		
			frsfacepos.rcFace = FRSRECT::TO_FRSRECT(facepos.rcFace);
			frsfacepos.ptLeftEye = FRSPOINT::TO_FRSPOINT(facepos.ptLeftEye);
			frsfacepos.ptRightEye = FRSPOINT::TO_FRSPOINT(facepos.ptRightEye);
			frsfacepos.ptMouth = FRSPOINT::TO_FRSPOINT(facepos.ptMouth);
			frsfacepos.ptNose = FRSPOINT::TO_FRSPOINT(facepos.ptNose);
			frsfacepos.fAngle = FRSFaceAngle::TO_FRSFaceAngle(facepos.fAngle);
			frsfacepos.nQuality = facepos.nQuality;
			
			return  frsfacepos;
		}
	};



	public value struct FRSParam
	{
		int nMinFaceSize = 50;//min face width size can be detected,default is 50 pixels
		int nRollAngle = 30;//max face roll angle,default is 30(degree)
		bool bOnlyDetect = false;//only detect face or not(if it's true,only Detect API is enable,Feature API is disable),defaule is false(Detect API is enable,and Feature API is enable)
		DWORD dwReserved = NULL;//reserved value,must be NULL
		/*FRSParam()
		{
			nMinFaceSize = 50;
			nRollAngle = 30;
			bOnlyDetect = false;
			dwReserved = NULL;
		}*/
	internal:
		THFI_Param TO_THFI_Param()
		{
			THFI_Param param;
			param.bOnlyDetect = bOnlyDetect;
			param.nRollAngle = nRollAngle;
			param.nMinFaceSize = nMinFaceSize;
			param.dwReserved = dwReserved;
			return param;
		}
		static FRSParam^ TO_FRSParam(const THFI_Param & param)
		{
			FRSParam ^frsParam = gcnew FRSParam();
			frsParam->bOnlyDetect = param.bOnlyDetect;
			frsParam->dwReserved = param.dwReserved;
			frsParam->nMinFaceSize = param.nMinFaceSize;
			frsParam->nRollAngle = param.nRollAngle;
			return frsParam;
		}

		
	};
	public ref class FaceImage {

	public:

		//////Struct define//////

		


		//////API define//////


		/******Create API******/

		static int		Create(short nChannelNum, FRSParam^ pParam);
		

		/******Detect API(only detect face)******/

		//static int		DetectFace(short nChannelID, array<BYTE>^ pImage, int bpp, int nWidth, int nHeight, FRSFacePos^ pfps, int nMaxFaceNums);
		static int		FaceImage::DetectFace(short nChannelID, Image^ pImage, int bpp, array<FRSFacePos>^ pfps, int nMaxFaceNums);


	


		
		static int		FaceImage::DetectFace(Image^ pImage, int bpp, array<FRSFacePos>^ pfps, int nMaxFaceNums);


		/******Release API******/

		static void	Release();
		

		 internal:
			 static	  int		Create(short nChannelNum, THFI_Param* pParam);
			
			 static  int		DetectFace(short nChannelID, BYTE* pImage, int bpp, int nWidth, int nHeight, THFI_FacePos* pfps, int nMaxFaceNums);
			 static int		    DetectFace(short nChannelID, cv::Mat &pImage, int bpp, int nWidth, int nHeight, array<FRSFacePos>^ pfps, int nMaxFaceNums);
			 static  int		DetectFace( BYTE* pImage, int bpp, int nWidth, int nHeight, THFI_FacePos* pfps, int nMaxFaceNums);
			 static int		    DetectFace( cv::Mat &pImage, int bpp, int nWidth, int nHeight, array<FRSFacePos>^ pfps, int nMaxFaceNums);
	private :
		static short channelNum = 0;
		static Mutex ^ channelFlagsMutex = gcnew Mutex();
		static array<bool>^channelFlags ;//标志进程是否被空闲

		static short findFreeChannel();//找到空闲的一个通道，并且将对应的标志位值置为false;都不空闲返回-1；
		static Int32 setFreeChannel(short channelID);//channelFlags 对应位置为true；	 
		static Int32 FaceImage::setBusyChannel(short channelID);//channelFlags 对应位置为false；	 
	};
}
