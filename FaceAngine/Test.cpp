#include "FaceImage.h"
#include "Feature.h"
#include "FeatureData.h"
#include <cv.h>
#include <cxcore.h>
#include <highgui.h>
#include<time.h>
using namespace FRS;
using namespace System::IO;
using namespace System::Runtime::InteropServices;

void TestDetectFace()
{
#pragma region C++
	FRSParam ^param = gcnew FRSParam();

	param->nMinFaceSize = 50;
	param->nRollAngle = 60;
	param->bOnlyDetect = true;
	
	FaceImage::Create(4, param);

	clock_t startTime, endTime;
	startTime = clock();
	
		//BYTE* pImgData = NULL;
		cv::Mat image = cv::imread("D:/3.jpg", CV_LOAD_IMAGE_COLOR), img_gray;
		//pImgData = (BYTE *)(image.ptr());
		int nWidth = image.cols;
		int nHeight = image.rows;
		//BYTE* pRgb = new BYTE[nWidth*nHeight];
		//for (int ih = 0; ih<nHeight; ++ih)
		//{
		//	for (int iw = 0; iw<nWidth; ++iw)
		//	{
		//		pRgb[ih*nWidth + iw] = (pImgData[ih*nWidth * 3 + iw * 3 + 0] + pImgData[ih*nWidth * 3 + iw * 3 + 1] + pImgData[ih*nWidth * 3 + iw * 3 + 2]) / 3;  //to gray
		//	}
		//}

		THFI_FacePos ptfp[10];
		for (int n = 0; n < 100; n++){
			int nFace = FaceImage::DetectFace( image.data, 24, nWidth, nHeight, ptfp, 10);

			for (int i = 0; i < nFace; i++)
			{
				cv::Rect rectFace;
				rectFace.x = ptfp[i].rcFace.left;
				rectFace.y = ptfp[i].rcFace.top;
				rectFace.width = ptfp[i].rcFace.right - ptfp[i].rcFace.left + 1;
				rectFace.height = ptfp[i].rcFace.bottom - ptfp[i].rcFace.top + 1;
				rectangle(image, rectFace, cv::Scalar(0, 255, 0));
				circle(image, cv::Point(ptfp[i].ptLeftEye.x, ptfp[i].ptLeftEye.y), 3, cv::Scalar(0, 255, 0), -1);
				circle(image, cv::Point(ptfp[i].ptRightEye.x, ptfp[i].ptRightEye.y), 3, cv::Scalar(0, 255, 0), -1);
				circle(image, cv::Point(ptfp[i].ptMouth.x, ptfp[i].ptMouth.y), 3, cv::Scalar(0, 255, 0), -1);
				circle(image, cv::Point(ptfp[i].ptNose.x, ptfp[i].ptNose.y), 3, cv::Scalar(0, 255, 0), -1);
			}

		}
		endTime = clock();
		std::cout << "Totle Time : " << (double)(endTime - startTime) / CLOCKS_PER_SEC << "s" << std::endl;
	
#pragma endregion 

	//FRSParam ^param = gcnew FRSParam();

	//param->nMinFaceSize = 50;
	//param->nRollAngle = 60;
	//param->bOnlyDetect = true;

	//FaceImage::Create(1, param);



	////BYTE* pImgData = NULL;
	//cv::Mat image = cv::imread("E:/’’∆¨/sunkang.jpg", CV_LOAD_IMAGE_COLOR), img_gray;
	////pImgData = (BYTE *)(image.ptr());
	//int nWidth = image.cols;
	//int nHeight = image.rows;

	//cv::cvtColor(image, img_gray, cv::COLOR_BGR2GRAY);
	//
	//array<FRSFacePos> ^ptfp = gcnew array<FRSFacePos>(10);
	//int nFace = 0;
	//nFace = FaceImage::DetectFace(0, img_gray, 8, nWidth, nHeight, ptfp, 10);
	//for (int i = 0; i<nFace; i++)
	//{
	//	cv::Rect rectFace;
	//	rectFace.x = ptfp[i].rcFace.left;
	//	rectFace.y = ptfp[i].rcFace.top;
	//	rectFace.width = ptfp[i].rcFace.right - ptfp[i].rcFace.left + 1;
	//	rectFace.height = ptfp[i].rcFace.bottom - ptfp[i].rcFace.top + 1;
	//	rectangle(image, rectFace, cv::Scalar(0, 255, 0));
	//	cv::circle(image, cv::Point(ptfp[i].ptLeftEye.x, ptfp[i].ptLeftEye.y), 3, cv::Scalar(0, 255, 0), -1);
	//	cv::circle(image, cv::Point(ptfp[i].ptRightEye.x, ptfp[i].ptRightEye.y), 3, cv::Scalar(0, 255, 0), -1);
	//	cv::circle(image, cv::Point(ptfp[i].ptMouth.x, ptfp[i].ptMouth.y), 3, cv::Scalar(0, 255, 0), -1);
	//	cv::circle(image, cv::Point(ptfp[i].ptNose.x, ptfp[i].ptNose.y), 3, cv::Scalar(0, 255, 0), -1);
	//}
	//cv::imshow("image", image);
	//cv::waitKey();


#pragma region Csharp
	//FRSParam ^param = gcnew FRSParam();

	//param->nMinFaceSize = 50;
	//param->nRollAngle = 60;
	//param->bOnlyDetect = true;

	//FaceImage::Create(1, param);

	//const int n = 1;

	//Image^img  =Image::FromFile("D:/3.jpg");
	//int nMaxFaceNums = 5;

	//array<FRSFacePos>^ pfp = gcnew array<FRSFacePos>(nMaxFaceNums);
	//int face_num = FaceImage::DetectFace(0, img, 24, pfp, nMaxFaceNums);
	//Console::WriteLine(face_num);
#pragma endregion 


	//for (int i = 0; i<face_num; i++)
	//{
	//	cv::Rect rectFace;
	//	rectFace.x = pfp[i].rcFace.left;
	//	rectFace.y = pfp[i].rcFace.top;
	//	rectFace.width = pfp[i].rcFace.right - pfp[i].rcFace.left + 1;
	//	rectFace.height = pfp[i].rcFace.bottom - pfp[i].rcFace.top + 1;
	//	rectangle(img_color, rectFace, cv::Scalar(0, 255, 0));
	//	cv::circle(img_color, cv::Point(pfp[i].ptLeftEye.x, pfp[i].ptLeftEye.y), 3, cv::Scalar(0, 255, 0), -1);
	//	cv::circle(img_color, cv::Point(pfp[i].ptRightEye.x, pfp[i].ptRightEye.y), 3, cv::Scalar(0, 255, 0), -1);
	//	cv::circle(img_color, cv::Point(pfp[i].ptMouth.x, pfp[i].ptMouth.y), 3, cv::Scalar(0, 255, 0), -1);
	//	cv::circle(img_color, cv::Point(pfp[i].ptNose.x, pfp[i].ptNose.y), 3, cv::Scalar(0, 255, 0), -1);
	//}
	//cv::imshow("image", img_color);
	//cv::waitKey();










}
void TestVerify()
{
	
	FRSParam ^param = gcnew FRSParam();

	param->nMinFaceSize = 50;
	param->nRollAngle = 60;
	param->bOnlyDetect = true;

	FaceImage::Create(1, param);
	Feature::Init(1);




	int i, j;

	cv::Mat im1 = cv::imread("D:/3.jpg", CV_LOAD_IMAGE_COLOR);
	if (im1.empty())
	{
		
		return;
	}

	
	

	int nWidth1 = im1.cols;
	int nHeight1 = im1.rows;

	//face detect
	THFI_FacePos ptfp1[10];
	int k;

	

	int nNum1 = FaceImage::DetectFace(0, im1.data, 24, nWidth1, nHeight1, ptfp1, 10);

	RECT rcFace = ptfp1[0].rcFace;

	//BYTE* pFeature1 = new BYTE[EF_Size()];
	array<BYTE>^ pFeature1 = gcnew array<BYTE>(Feature::Size());
	//only extract the first face(max size face)
	int ret = Feature::Extract(0, im1, nWidth1, nHeight1, 3, (THFI_FacePos*)&ptfp1[0], pFeature1);
	if (ret)
	{

	}
	else
	{
		delete[]pFeature1;
		//pFeature1 = NULL;
	}
	



	cv::Mat im2 = cv::imread("D:/3.jpg", CV_LOAD_IMAGE_COLOR);

	if (im2.empty())
	{
		delete[]pFeature1;

		return;
	}


	int nWidth2 = im2.cols;
	int nHeight2 = im2.rows;

	//face detect
	THFI_FacePos ptfp2[10];
	
	int nNum2 = FaceImage::DetectFace(0, im2.data, 24, nWidth2, nHeight2, ptfp2, 10);


	rcFace = ptfp2[0].rcFace;

	//BYTE* pFeature2 = new BYTE[EF_Size()];
	array<BYTE>^ pFeature2 = gcnew array<BYTE>(Feature::Size());
	//only extract the first face(max size face)
	ret = Feature::Extract(0, im2, nWidth2, nHeight2, 3, (THFI_FacePos*)&ptfp2[0], pFeature2);
	
	if (ret)
	{

	}
	else
	{
		delete[]pFeature2;
		
	}

	

	



	float score = 0.0f;


	score = Feature::Compare(pFeature1, pFeature2);

	std::cout << score << std::endl;

	
}
void TestFeatureData()
{
	FRSParam ^param = gcnew FRSParam();

	param->nMinFaceSize = 50;
	param->nRollAngle = 60;
	param->bOnlyDetect = true;

	FaceImage::Create(1, param);
	Feature::Init(1);
	
	FeatureData ^fa = gcnew FeatureData();
	//fa->RegisterInBulk(L"F:/ª˙≥°’’∆¨test/temp");
	fa->LoadData();
	
	Image^ img= Image::FromFile(L"D:/3.jpg");
	UserInfo ^uinfo = gcnew UserInfo();
	uinfo->name = "sun";
	
	fa->Register(img, uinfo);

	//fa->Register(L"E:/’’∆¨/sunkang.jpg", L"sunkang");
	//fa->Register(L"E:/’’∆¨/3.jpg", L"f");
	
	
	array<HitAlert^>^ res = fa->Search(img);
	Console::WriteLine(safe_cast<HitAlertDetail>(res[0]->Details[0]).name);
	fa->Unregister("sunkang");


	Console::WriteLine(res[0]->Threshold);
	

}
//void Test()
//{
//	FRSParam ^param = gcnew FRSParam();
//
//	param->nMinFaceSize = 50;
//	param->nRollAngle = 60;
//	param->bOnlyDetect = true;
//
//	FaceImage::Create(1, param);
//	Feature::Init(1);
//
//	FeatureData ^fa = gcnew FeatureData();
//	//fa->RegisterInBulk(L"F:/ª˙≥°’’∆¨test/temp");
//	fa->LoadData();
//	DirectoryInfo^ dir = gcnew DirectoryInfo("F:/ª˙≥°’’∆¨test/current");
//	int count = 0;
//	int index = 0;
//	for each (FileInfo ^ file in dir->GetFiles())
//	{
//		Image ^img = Image::FromFile(file->FullName);
//		index++;
//		Console::WriteLine(index);
//		array<array<HitAlert^>^>^ res = fa->Search(img, 0, 1, 1);
//		if (res == nullptr) continue;
//		if (res[0][0]->Name->Equals(file->Name))
//			count++;
//		
//	}
//	Console::WriteLine("correct num£∫"+count);
//
//}
void TestDetectFaceOnStream()
{

}
void DetectFaceFromStream()
{


	FRSParam ^param = gcnew FRSParam();

	param->nMinFaceSize = 120;
	param->nRollAngle = 25;
	param->bOnlyDetect = true;

	FaceImage::Create(1, param);
	

	
	cv::VideoCapture* cap = new cv::VideoCapture();

	
	cap->open(0);



	cv::Mat frame;
	double fps = cap->get(CV_CAP_PROP_FPS);

	std::cout << fps << std::endl;
	if (cap->isOpened())
	{
		while (true)
		{

			*cap >> frame;
			if (frame.empty())
				break;
			long time = (long)cap->get(CV_CAP_PROP_POS_MSEC);
			
			cv::Mat  img_gray;

			int nWidth = frame.cols;
			int nHeight = frame.rows;

			cv::cvtColor(frame, img_gray, cv::COLOR_BGR2GRAY);

			THFI_FacePos ptfp[10];
			int nFace = FaceImage::DetectFace(0, frame.data, 24, nWidth, nHeight, ptfp, 10);
			float nscale = 1;
			
			for (int i = 0; i<nFace; i++)
			{
				cv::Rect rectFace;
				int centerx = (ptfp[i].rcFace.right + ptfp[i].rcFace.left) / 2;
				int centery = (ptfp[i].rcFace.bottom + ptfp[i].rcFace.top) / 2;
				int width = ptfp[i].rcFace.right - ptfp[i].rcFace.left + 1;
				int height = ptfp[i].rcFace.bottom - ptfp[i].rcFace.top + 1;
				Console::WriteLine("pitch: " + ptfp[i].fAngle.pitch + ", roll: " + ptfp[i].fAngle.roll + ", yaw: " + ptfp[i].fAngle.yaw + ", nQuality: " + ptfp[i].nQuality);
				

				rectFace.x = centerx - width*nscale / 2>0 ? centerx - width*nscale / 2 : 0;
				rectFace.y = centery - height*nscale / 2>0 ? centery - height*nscale / 2 : 0;
				rectFace.width = rectFace.x + width*nscale<nWidth ? width*nscale : nWidth - rectFace.x;
				rectFace.height = rectFace.y + height*nscale<nHeight ? height*nscale : nHeight - rectFace.y;
				
				cv::rectangle(frame, rectFace, cv::Scalar(0, 255, 0));

			}
			cv::imshow("demo", frame);
			cv::waitKey(30);


			
		}

	}
	
	cap->release();
	delete cap;
}

void main()
{

	float s[1024];
	
	//TestDetectFace();
	//TestVerify();
	//TestFeatureData();
	//std::cout << Feature::Size() << std::endl;
	DetectFaceFromStream();
	//DetectFaceFromStream();
	Console::ReadLine();
}