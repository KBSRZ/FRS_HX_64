#include "Capture.h"
#include "PubConstant.h"

#include <opencv2/core/version.hpp>
#include <opencv2/calib3d/calib3d.hpp>
#include <opencv2/opencv.hpp>
#include "BitmapConverter.h"
#include "ImageHelper.h"

using namespace System::Runtime::InteropServices;
using namespace System::IO;
using namespace System::Drawing::Imaging;
using namespace System::Windows::Forms;
using namespace System::Runtime::Serialization::Formatters::Binary;

using namespace FRS;
using namespace FRS::Util;




Capture::Capture() 
{
#if USE_EXPIRE
	if (IsExpired()) return;
#endif
	featureData = gcnew FeatureData();
}
Capture::Capture(FeatureData^ featureData)
{
#if USE_EXPIRE
	if (IsExpired()) return;
#endif
	this->featureData = featureData;
}

Capture::~Capture()
{
	
	this->!Capture();
}


Capture::!Capture()
{
	this->featureData->~FeatureData();
}


Int32 Capture::Start(Int32 deviceId)
{
	Console::WriteLine("开启设备{0}", deviceId);

	beginThread = gcnew Thread(gcnew ParameterizedThreadStart(this, &Capture::Begin));
	
	beginThread->Start(deviceId);

	int count = 10;
	while (count > 0 && isRun == false){ //等待1秒钟
		Thread::Sleep(100);
		count--;
	}
	if (isRun){
		return ReturnCode::SUCCESS;
	}
	else{
		Stop();
		return ReturnCode::OPEN_VIDEO_DEVICE_FAILED;
	}
}
Int32 Capture::Start()
{
	return Start(0);
}
Int32 Capture::Start(String ^streamAddress)
{
	

	beginThread = gcnew Thread(gcnew ParameterizedThreadStart(this, &Capture::Begin));

	beginThread->Start(streamAddress);

	int count = 10;
	while (count > 0&& isRun==false){ //等待1秒钟
		Thread::Sleep(100);
		count--;
	}
	if (isRun){
		return ReturnCode::SUCCESS;
	}
	else{
		Stop();
		return ReturnCode::OPEN_VIDEO_STREAM_FAILED;
	}
	
}
Int32 Capture::Stop()
{
	control = false;
	isRun = false;
	catchImage = nullptr;
	
	return ReturnCode::SUCCESS;
}



void Capture::Begin(Object^ o)
{
	if (isRun)
	{
#if DEBUG
		Console::WriteLine("you should first call the Stop()");
#endif
		return;
	}

	
	control = true;
	bool isvideoFile = false;
	double fps;
	cap = new cv::VideoCapture();

	if (o->GetType() == Int32::typeid) 
		cap->open((Int32)o);//本地摄像头
	else
	{
		//本地视频文件
		String ^extention = Path::GetExtension((String^)o)->ToLower();
		if (extention->Equals(".avi") || extention->Equals(".mpg") || extention->Equals(".mp4"))
		{
			isvideoFile = true;
			if (false == cap->open((char*)(void*)Marshal::StringToHGlobalAnsi((String^)o)))//打开失败
			{
				return;
			}
			fps = cap->get(CV_CAP_PROP_FPS);
			//double fps = 50;

		}
		else
		{
			//视频流		
			vp = new VlcOpenCV((char*)(void*)Marshal::StringToHGlobalAnsi((String^)o), VIDEO_WIDTH, VIDEO_HEIGHT);
			if (vp->Start() != 0){//打开失败
				return;
			}

		}
	}

	if (cap->isOpened())
	{
		//开一个线程用来人脸识别
		 searchThread = gcnew Thread(gcnew ParameterizedThreadStart(this, &Capture::OnSearch));
		searchThread->Start(1);
		isRun = true;
		while (control)
		{
			cv::Mat frame;

			capMutex->WaitOne();
			*cap >> frame;
			capMutex->ReleaseMutex();

			if (frame.empty())
				continue;

			ImageGrabbedEvent();//调用retirive 函数用来显示

			if (isvideoFile)
				Thread::Sleep((int)(2000 / fps));
			/*Bitmap ^frame_bitmap  = gcnew System::Drawing::Bitmap(m.cols, m.rows, m.step, System::Drawing::Imaging::PixelFormat::Format24bppRgb, (System::IntPtr) m.data);
			currentFrame = frame_bitmap->Clone(Rectangle(0, 0, frame_bitmap->Width, frame_bitmap->Height), frame_bitmap->PixelFormat);*/
			//ImageGrabbedEvent(frametmp);
		}
	}

	else if (vp->IsOpen() ){
		isRun = true;
		//开一个线程用来人脸识别
		searchThread = gcnew Thread(gcnew ParameterizedThreadStart(this, &Capture::OnSearch));
		searchThread->Start(1);

		while (control)
		{

			cv::Mat frame;

			/*capMutex->WaitOne();
			*cap >> frame;
			capMutex->ReleaseMutex();*/
			capMutex->WaitOne();
			frame = vp->frame();
			capMutex->ReleaseMutex();
			if (frame.empty())
				continue;
			
			ImageGrabbedEvent();//调用retirive 函数用来显示

		}
	}
	else{
		ShowMsgEvent("Video Error", nullptr);
	}
	isRun = false;
	catchImage = nullptr;
	cap->release();
	if (cap){
		delete cap;
		cap = NULL;
	}
	if (vp){
		delete vp;
		vp = NULL;
	}
	
	

}


Bitmap^  Capture::Retrive()
{
	if (cap == nullptr || (!cap->isOpened()) && (vp->Start() == -1))return nullptr;
	cv::Mat matframe;
	if (cap->isOpened())
	{
		capMutex->WaitOne();
		*cap >> matframe;
		capMutex->ReleaseMutex();
		GC::Collect();
	}
	else if (vp->IsOpen()){
		//capMutex->WaitOne();
		matframe = vp->frame();
		//capMutex->ReleaseMutex();
		GC::Collect();

	}
	return BitmapConverter::ToBitmap(&matframe);
}

bool Capture::SetDataPath(String^ dataPath)
{
	bool dataPathState = false;

	if (dataPath->Equals(""))
	{
		return dataPathState;
	}

	try
	{
		trainingDataDir = dataPath;
		if (false == Directory::Exists(trainingDataDir))
		{
			Directory::CreateDirectory(trainingDataDir);
		}

		trainingDataDir = Path::Combine(trainingDataDir, DateTime::Now.ToString("yyyy-MM-dd"));
		if (false == Directory::Exists(trainingDataDir))
		{
			Directory::CreateDirectory(trainingDataDir);
		}

		dataPathState = true;
	}

	catch (Exception^ ex)
	{

	}

	return dataPathState;

}


void Capture::OnSearch(Object^ o)
{
	if (o->GetType() != Int32::typeid) return;
	int detectChannel = (Int32)o;

	while (control)
	{
		cv::Mat mat;
		if (cap->isOpened())
		{
			capMutex->WaitOne();
			if (cap)
			*cap >> mat;
			capMutex->ReleaseMutex();
		}
		else if (vp->IsOpen() )
		{
			//capMutex->WaitOne();
			mat = vp->frame();
			//capMutex->ReleaseMutex();
		}

		if (nullptr == featureData) {
			continue;
		}

		/*Image^ img = BitmapConverter::ToBitmap(&mat);
		CatchFaceImg(img, detectChannel);*/


		/*LocateFaceInfo^ locateFaceInfo = featureData->LocateFace(img, qualityThresh, faceRectScale, maxPersonNum,1);
		if (nullptr == locateFaceInfo || locateFaceInfo->FaceImgs->Length <= 0)
		{
		continue;
		}

		LocateFaceReturnEvent(locateFaceInfo->FaceImgs);
		*/
		/*array<HitAlert^>^ result = featureData->Search(img, scoreThresh, qualityThresh, faceRectScale, topK, maxPersonNum,);*/
		array<HitAlert^>^ result = featureData->Search(mat);
		
		if (nullptr == result)
		{
			continue;
		}
		
		//featureData->RecordHitInfo(result);
		try{
			HitAlertReturnEvent(result);
		}
		catch (Exception ^e){
			ShowMsgEvent("HitAlertReturnEvent Error", e);
		}


#pragma region 上传至云平台
		/*for each(auto hit in result){
		if (nullptr != hit->Details&&hit->Details->Length > 0){
		long sequenceNumber = 0;
		String^ faceFileName = String::Format("{0}_{1}_{2}_alarm_face.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_hh_mm_ss"), 1);
		inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Alarm_Face, faceFileName, ImageHelper::ImageToBytes(hit->QueryFace));
		String^ captFileName = String::Format("{0}_{1}_{2}_alarm_capt.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_hh_mm_ss"), 1);
		inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Alarm_CapturedImage, captFileName, ImageHelper::ImageToBytes(BitmapConverter::ToBitmap(&mat)));

		inf5->ReportAlarmAsync(
		DateTime::Now.ToString("yyyy-MM-dd hh:mm:ss"),
		captFileName, String::Empty, faceFileName,
		safe_cast<HitAlertDetail>(hit->Details[0]).peopleId,
		safe_cast<HitAlertDetail>(hit->Details[0]).imageId,
		safe_cast<HitAlertDetail>(hit->Details[0]).Score.ToString("F4"));

		}
		}*/
#pragma endregion



		Sleep(interval);
	}
}


void Capture::CatchFaceImg(Image ^img, int detectChannel)
{
	try
	{
		cv::Mat mat = BitmapConverter::ToMat(img);	
		if (LocateFaceImg(mat, detectChannel))//检测人脸
		{
			Image^ catchImg = (Image^)img->Clone();
			catchFaceImgQueue->Enqueue(catchImg);
		}
	}
	catch (Exception^ ex)
	{
		MessageBox::Show("CatchFaceImg Error:" + ex->Message + ex->Source);
	}
}


bool Capture::LocateFaceImg(cv::Mat mat, int detectChannel)
{
	bool isLocate = false;

	LocateFaceInfo^ locateFaceInfo = featureData->LocateFace(mat);
	if (nullptr != locateFaceInfo && locateFaceInfo->FaceImgs->Length > 0)
	{
		isLocate = true;
		LocateFaceReturnEvent(locateFaceInfo->FaceImgs);
	}
	
	return isLocate;
}


void Capture::MatchFaceImg(Object^ o)
{
	if (o->GetType() != Int32::typeid) return;  
	int detectChannel =(Int32)o;

	while (control)
	{
		bool ret = false;
		Image ^img = nullptr;

		try
		{			
			ret = catchFaceImgQueue->TryDequeue(img);
		}
		catch (Exception^ ex)
		{
			MessageBox::Show("MatchFaceImg Error:" + ex->Message + ex->Source);
		}

		if (ret)
		{
			ShowCountEvent(1, catchFaceImgQueue->Count);

			array<HitAlert^>^ result = featureData->Search(img);
			if (nullptr != result && result->Length > 0)
			{
				HitAlertReturnEvent(result);

//				featureData->RecordHitInfo(result, scoreThresh);
			}
		}

		Sleep(1);
	}
	

}

void Capture::DetectFace4CHC(Image^ faceImg, int detectChannel)
{
	CatchFaceImg(faceImg, detectChannel);
}

bool Capture::DetectFace4CHC(List<Image^>^ faceImgSet, int detectChannel)
{
	ShowMsgEvent("DetectFace4CHC", nullptr);
	int imgCountCHC = faceImgSet->Count;
	if (0 == imgCountCHC )
	{
		return false;
	}

	int curThreadId = Thread::CurrentThread->ManagedThreadId;
	String^ curThreadTag = "Thread:" + curThreadId;

	int matchFaceId = 0;
	Image^ matchFaceImg = nullptr;
	float matchScore = 0.0f;

	List<Image^>^ detectFaceImgSet = gcnew List<Image^>();
	List<Image^>^ locateFaceImgSet = gcnew List<Image^>();
	List<int>^ idxLocate2Detect = gcnew List<int>();

	try
	{
		//ShowMsgEvent(curThreadTag + " LocateFace All CHC Img Start imgCountCHC:" + imgCountCHC, nullptr);

		for (int i = 0; i < imgCountCHC; i++)
		{
			Image^ chcImg = faceImgSet[i];

			if (PixelFormat::Format24bppRgb != chcImg->PixelFormat)
			{
				continue;
			}

			detectFaceImgSet->Add(chcImg);

			cv::Mat mat = BitmapConverter::ToMat(chcImg);

			//ShowMsgEvent(curThreadTag + " Start featureData->LocateFace" , nullptr);

			LocateFaceInfo^ locateFaceInfo = featureData->LocateFace(mat);
			if (nullptr == locateFaceInfo || locateFaceInfo->FaceImgs->Length <= 0)
			{
				continue;
			}

			Image^ locateFaceImg = (Image^)(locateFaceInfo->FaceImgs[0]);
			locateFaceImgSet->Add(locateFaceImg);

			int idxDetect = detectFaceImgSet->Count - 1;
			idxLocate2Detect->Add(idxDetect);

			//ShowMsgEvent("NLG" + idxDetect + ":" + locateFaceInfo->rectImgInfo, nullptr);

		}

		int imgCountLocate = locateFaceImgSet->Count;
		if (0 == imgCountLocate)
		{
			return false;
		}

	
		//ShowMsgEvent(curThreadTag + " LocateFaceReturnEvent Start locateFaceImgSet Count : " + locateFaceImgSet->Count, nullptr);

		catchFaceCount += imgCountLocate;
		LocateFaceReturnEvent(locateFaceImgSet->ToArray());

	
	}
	catch (Exception^ ex)
	{
		ShowMsgEvent("LocateFace Error",ex);
		return false;
	}


	HitAlert^ hit = nullptr;

	try
	{
//		ShowMsgEvent(curThreadTag + " featureData->SearchBulk Start at channel:" + detectChannel, nullptr);

		//hit = featureData->SearchBulk(detectFaceImgSet, scoreThresh, qualityThresh, faceRectScale, topK, maxPersonNum, detectChannel);
		hit = featureData->SearchBulk(detectFaceImgSet);//搜索库中相似的人
		

	}
	catch (Exception^ ex)
	{
		ShowMsgEvent("featureData->SearchBulk Error", ex);
		return false;
	}


	List<HitAlert^> ^hitResult = gcnew List<HitAlert^>();
	hitResult->Add(hit);

	try
	{
//		ShowMsgEvent(curThreadTag + " HitAlertReturnEvent Start", nullptr);
		
		HitAlertReturnEvent(hitResult->ToArray());//返回搜索结果


	}
	catch (Exception^ ex)
	{
		ShowMsgEvent("HitAlertReturnEvent Error", ex);
		return false;
	}
	
	featureData->RecordHitInfo(hitResult->ToArray());//纪录结果
//#pragma region 上传至云平台
//	int sequenceCode=0;
//	for each(Image^ im in faceImgSet)
//	{
//		String^ faceFileName = String::Format("{0}_{1}_{2}_face.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_HH_mm_ss"), sequenceCode);
//		//inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Normal_Face, faceFileName, ImageHelper::ImageToBytes(im));
//	}
//	for each(auto hit in hitResult){
//		if (nullptr != hit->Details&&hit->Details->Length > 0){
//			long sequenceNumber = 0;
//			String^ faceFileName = String::Format("{0}_{1}_{2}_alarm_face.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_HH_mm_ss"), 1);
//			//inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Alarm_Face, faceFileName, ImageHelper::ImageToBytes(hit->QueryFace));
//			String^ captFileName = String::Format("{0}_{1}_{2}_alarm_capt.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_HH_mm_ss"), 1);
//			//inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Alarm_CapturedImage, captFileName, ImageHelper::ImageToBytes(faceImgSet[0]));
//
//			/*inf5->ReportAlarmAsync(
//				DateTime::Now.ToString("yyyy-MM-dd HH:mm:ss"),
//				captFileName, String::Empty, faceFileName,
//				safe_cast<HitAlertDetail>(hit->Details[0]).peopleId,
//				safe_cast<HitAlertDetail>(hit->Details[0]).imageId,
//				safe_cast<HitAlertDetail>(hit->Details[0]).Score.ToString("F4"));*/
//			ShowMsgEvent(
//				"capture_Time:"+DateTime::Now.ToString("yyyy-MM-dd HH:mm:ss") +
//				" capture_image:"+captFileName + 
//				" capture_video："+String::Empty + 
//				" face_image:"+faceFileName +
//				" people_id："+safe_cast<HitAlertDetail>(hit->Details[0]).peopleId +
//				" image_id:"+safe_cast<HitAlertDetail>(hit->Details[0]).imageId +
//				" match_degree："+safe_cast<HitAlertDetail>(hit->Details[0]).Score.ToString("F4"),nullptr);
//		}
//		
//	}
//#pragma endregion

	
	return CollectTrainData4CHC(detectFaceImgSet, locateFaceImgSet, idxLocate2Detect, hit);
}

/*
 * 收集南理工需要的训练数据
 */
bool Capture::CollectTrainData4CHC(List<Image^>^ detectFaceImgSet, List<Image^>^ locateFaceImgSet, List<int>^ idxLocate2Detect, HitAlert^ hit)
{
	int imgCountDetect = detectFaceImgSet->Count;
	int imgCountLocate = locateFaceImgSet->Count;

	int curThreadId = Thread::CurrentThread->ManagedThreadId;
	String^ curThreadTag = "Thread:" + curThreadId;

/*

	Threading::Monitor::Enter(objImgFileIO);

	array<String^>^ dirPathSet = Directory::GetDirectories(trainingDataDir);
	int existDirCount = dirPathSet->Length;

	int curDirIdx = existDirCount + 1;
	String^ userDataDir = Path::Combine(trainingDataDir, curDirIdx.ToString());
	Directory::CreateDirectory(userDataDir);

	Threading::Monitor::Exit(objImgFileIO);

	//保存海康人脸抓拍原图
	for (int i = 0; i < imgCountDetect; i++)
	{
		Image^ chcImg = detectFaceImgSet[i];
		String^ chcImgPath = "CHC-" + i + ".jpg";
		chcImgPath = Path::Combine(userDataDir, chcImgPath);
		chcImg->Save(chcImgPath);
	}

	int imgCountMatch = hit->Details->Length;
	Image^ imgLocateBest = hit->QueryFace;
	if (nullptr == imgLocateBest || 0 == imgCountMatch)
	{
		return false;
	}

	//保存南理工最佳的人脸检测截取图片
	String^ locateBestImgPath = "NLG-Best.jpg";
	locateBestImgPath = Path::Combine(userDataDir, locateBestImgPath);
	imgLocateBest->Save(locateBestImgPath);


	//保存检测后匹配到的底库图片
	HitAlertDetail^ hitDetail = (HitAlertDetail^)(hit->Details[0]);
	int matchFaceId = hitDetail->UserId;
	String^ matchImgPath = matchFaceId.ToString() + ".jpg";
	matchImgPath = Path::Combine(userDataDir, matchImgPath);
	File::Copy(hitDetail->imgPath, matchImgPath);

*/
	ShowMsgEvent("NLG Save", nullptr);

	int imgCountMatch = hit->Details->Length;
	Image^ imgLocateBest = hit->QueryFace;
	if (nullptr == imgLocateBest || 0 == imgCountMatch)
	{
		return false;
	}

	try
	{
		Threading::Monitor::Enter(objImgFileIO);

		HitAlertDetail^ hitDetail = (HitAlertDetail^)(hit->Details[0]);
		int matchFaceId = hitDetail->UserId;
		ShowMsgEvent("save NLG" + matchFaceId, nullptr);
		String^ userDataDir = Path::Combine(trainingDataDir, matchFaceId.ToString());

		if (false == Directory::Exists(userDataDir))
		{
			Directory::CreateDirectory(userDataDir);
		}
		else
		{
			array<String^>^ dirPathSet = Directory::GetDirectories(trainingDataDir, matchFaceId + "-*_");
			int existDirCount = dirPathSet->Length;
			
			String^ matchDir = matchFaceId.ToString() + "-" + existDirCount;
			userDataDir = Path::Combine(trainingDataDir, matchDir);
			Directory::CreateDirectory(userDataDir);
		}

		Threading::Monitor::Exit(objImgFileIO);

//		ShowMsgEvent(curThreadTag + " UserDataDir:" + userDataDir, nullptr);

//		ShowMsgEvent(curThreadTag +  " Copy CHC Image Start:" + imgCountDetect, nullptr);

		//保存海康人脸抓拍原图
		for (int i = 0; i < imgCountDetect; i++)
		{
			Image^ chcImg = detectFaceImgSet[i];
			String^ chcImgPath = "CHC-" + i + ".jpg";
			chcImgPath = Path::Combine(userDataDir, chcImgPath);
			chcImg->Save(chcImgPath);
		}

//		ShowMsgEvent(curThreadTag + " Copy NLG Image Start:" + imgCountLocate, nullptr);

		//保存南理工人脸检测截取图片
		for (int j = 0; j < imgCountLocate; j++)
		{
			int idxDetect = idxLocate2Detect[j];
			Image^ locateImg = locateFaceImgSet[j];
			String^ locateImgPath = "NLG-" + idxDetect + ".jpg";
			locateImgPath = Path::Combine(userDataDir, locateImgPath);
			locateImg->Save(locateImgPath);
		}

//		ShowMsgEvent(curThreadTag + " Copy Best Image Start", nullptr);

		//保存南理工最佳的人脸检测截取图片
		String^ locateBestImgPath = "NLG-Best_" + safe_cast<HitAlertDetail>(hit->Details[0]).Score.ToString() + ".jpg";;
		locateBestImgPath = Path::Combine(userDataDir, locateBestImgPath);
		imgLocateBest->Save(locateBestImgPath);

//		ShowMsgEvent(curThreadTag + " Copy Match Image Start imgCountMatch:" + imgCountMatch, nullptr);

		//保存检测后匹配到的底库图片
		for (int j = 0; j < imgCountMatch; j++)
		{
			HitAlertDetail^ hitDetail = (HitAlertDetail^)(hit->Details[j]);
			matchFaceId = hitDetail->UserId;
			String^ matchImgPath = "备用-" + matchFaceId.ToString() + ".jpg";
			ShowMsgEvent(hitDetail->imgPath, nullptr);
			ShowMsgEvent(userDataDir, nullptr);
			matchImgPath = Path::Combine(userDataDir, matchImgPath);
			File::Copy(hitDetail->imgPath, matchImgPath);
		}

		CollectFaceCount++;
		ShowCountEvent(0, CollectFaceCount);

//		ShowMsgEvent(curThreadTag + " ShowCountEvent End And CollectFaceCount:" + CollectFaceCount, nullptr);
	}
	catch (Exception^ ex)
	{
		ShowMsgEvent("Image File IO Save Error", ex);
	}

	return true;

}