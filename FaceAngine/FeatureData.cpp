#include "FeatureData.h"

#include <opencv2/core/version.hpp>
#include <opencv2/calib3d/calib3d.hpp>
#include <opencv2/opencv.hpp>
#include "BitmapConverter.h"
#include "PubConstant.h"
using namespace System::Runtime::InteropServices;
using namespace System::IO;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;
using namespace System::Runtime::Serialization::Formatters::Binary;
using namespace System::Windows::Forms;

using namespace FRS;
using namespace FRS::Util;
using namespace DataAngine;


FeatureData::FeatureData()
{
	hitbll = gcnew BLL::hitrecord();
	usrbll = gcnew BLL::user();
	hdbll = gcnew BLL::hitrecord_detail();
	habll = gcnew BLL::hitalert();
	//stbll = gcnew BLL::statistics();

	if (!Directory::Exists(regFaceDir))//如果不存在就创建file文件夹
	{
		Directory::CreateDirectory(regFaceDir);
	}
	if (!Directory::Exists(queryFaceDir))//如果不存在就创建file文件夹
	{
		Directory::CreateDirectory(queryFaceDir);
	}
	if (!Directory::Exists(queryProblemFaceDir))//如果不存在就创建file文件夹
	{
		Directory::CreateDirectory(queryProblemFaceDir);
	}


	//recordHitUserMap = gcnew Dictionary<int, float>();
	/*
	String ^ server1RegDir = Path::Combine(server1Dir, regFaceDir);
	String ^ server2RegDir = Path::Combine(server2Dir, regFaceDir);

	if (!Directory::Exists(server1RegDir))
	{
	Directory::CreateDirectory(server1RegDir);
	}

	if (!Directory::Exists(server2RegDir))
	{
	Directory::CreateDirectory(server2RegDir);
	}

	String ^ server1QueryDir = Path::Combine(server1Dir, queryFaceDir);
	String ^ server2QueryDir = Path::Combine(server2Dir, queryFaceDir);

	if (!Directory::Exists(server1QueryDir))
	{
	Directory::CreateDirectory(server1QueryDir);
	}

	if (!Directory::Exists(server2QueryDir))
	{
	Directory::CreateDirectory(server2QueryDir);
	}
	*/
	//LoadData();
}
FeatureData::~FeatureData()
{
	this->!FeatureData();
}

FeatureData::!FeatureData()
{

}

bool FeatureData::SetDataPath(String^ dataPath)
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


Int32 FeatureData::RegisterInBulk(String^ fileDirPath)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif

	if (Directory::Exists(fileDirPath)){
		return ReturnCode::DIR_NOT_EXITS;
	}
	if (allUsers == nullptr) allUsers = gcnew List<Model::user^>();

	DirectoryInfo^ dir = gcnew DirectoryInfo(fileDirPath);
	array<FileInfo^>^files = dir->GetFiles();
	int count = 0;

	for (int i = 0; i < files->Length; i++)
	{

		int status = -1;
		cv::Mat cvImg = cv::imread((char*)(void*)Marshal::StringToHGlobalAnsi(files[i]->FullName), 1);
		if (!cvImg.empty())
		{
			UserInfo^ userInfo = gcnew UserInfo();
			array<String^> ^items = files[i]->Name->Split('_');
			userInfo->name = items[0];

			////
			////填入其他信息
			///

			userInfo->name = System::IO::Path::GetFileNameWithoutExtension(files[i]->Name);

			status = Register(cvImg, userInfo);
		}

		count++;

		try {
			RegisterOneFinisedEvent(count, files[i]->FullName + ": " + status);
		}
		catch (Exception ^e)
		{
			MessageBox::Show(e->Message);
		}
	}

	LoadData();
	return count;


}
Int32 FeatureData::RegisterInBulkFromFile(String^ filePath)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif

	if (allUsers == nullptr) allUsers = gcnew List<Model::user^>();

	FileStream ^fs = gcnew FileStream(filePath, FileMode::Open, FileAccess::Read);
	StreamReader ^read = gcnew StreamReader(fs, Encoding::Default);
	String^ strReadline;
	int count = 0;

	while ((strReadline = read->ReadLine()) != nullptr)
	{

		int status = ReturnCode::SUCCESS;
		array<String^>^ Items = strReadline->Split(',');
		cv::Mat cvImg = cv::imread((char*)(void*)Marshal::StringToHGlobalAnsi(Items[3]->Trim()), 1);
		if (!cvImg.empty())
		{
			UserInfo^ usr = gcnew UserInfo();
			usr->peopleId = Items[0]->Trim();
			usr->imageId = Items[1]->Trim();
			usr->type = Items[2]->Trim();

			status = Register(cvImg, usr);
		}
		count++;
		try {
			RegisterOneFinisedEvent(count, Items[3]->Trim() + ": " + status);
		}
		catch (Exception ^e)
		{
			MessageBox::Show(e->Message);
		}


	}
	fs->Close();
	read->Close();
	LoadData();
	return count;


}
Int32 FeatureData::RegisterInBulk1(String^ fileDirPath)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif

	if (allUsers == nullptr) allUsers = gcnew List<Model::user^>();

	DirectoryInfo^ dir = gcnew DirectoryInfo(fileDirPath);
	array<FileInfo^>^files = dir->GetFiles();
	int count = 0;

	for (int i = 0; i < files->Length; i++)
	{

		int status = -1;
		cv::Mat cvImg = cv::imread((char*)(void*)Marshal::StringToHGlobalAnsi(files[i]->FullName), 1);
		if (!cvImg.empty())
		{
			UserInfo^ userInfo = gcnew UserInfo();
			userInfo->name = System::IO::Path::GetFileNameWithoutExtension(files[i]->Name);//440100Z009992016120002_1
			userInfo->imageId = System::IO::Path::GetFileNameWithoutExtension(files[i]->Name);//440100Z009992016120002_1 
			userInfo->peopleId = System::IO::Path::GetFileNameWithoutExtension(files[i]->Name)->Split('_')[0];//440100Z009992016120002
			status = Register(cvImg, userInfo);
		}

		count++;

		try {
			RegisterOneFinisedEvent(count, files[i]->FullName + ": " + status);
		}
		catch (Exception ^e)
		{
			MessageBox::Show(e->Message);
		}
	}

	LoadData();
	return count;


}

Int32 FeatureData::RegisterInBulk1(String^ fileDirPath, String^ library)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif

	if (!Directory::Exists(fileDirPath)){
		return ReturnCode::DIR_NOT_EXITS;
	}
	if (allUsers == nullptr) allUsers = gcnew List<Model::user^>();

	DirectoryInfo^ dir = gcnew DirectoryInfo(fileDirPath);
	array<FileInfo^>^files = dir->GetFiles();
	int count = 0;

	for (int i = 0; i < files->Length; i++)
	{

		int status = -1;
		cv::Mat cvImg = cv::imread((char*)(void*)Marshal::StringToHGlobalAnsi(files[i]->FullName), 1);
		if (!cvImg.empty())
		{
			UserInfo^ userInfo = gcnew UserInfo();
			userInfo->name = System::IO::Path::GetFileNameWithoutExtension(files[i]->Name);//440100Z009992016120002_1
			userInfo->imageId = System::IO::Path::GetFileNameWithoutExtension(files[i]->Name);//440100Z009992016120002_1 
			userInfo->peopleId = System::IO::Path::GetFileNameWithoutExtension(files[i]->Name)->Split('_')[0];//440100Z009992016120002
			status = Register(cvImg, userInfo, library);
		}

		count++;

		try {
			RegisterOneFinisedEvent(count, files[i]->FullName + ": " + status);
		}
		catch (Exception ^e)
		{
			MessageBox::Show(e->Message);
		}
	}

	LoadData(library);
	return count;


}

Int32 FeatureData::Register(cv::Mat& cvImg, Model::user ^ usr)
{
	if (cvImg.channels() < 3) throw gcnew   ArgumentException("image must have more than 3 channels");
	try{
		if (allUsers == nullptr) allUsers = gcnew List<Model::user^>();

		int nWidth = cvImg.cols;
		int nHeight = cvImg.rows;

		if (nWidth < registerFaceWidthThresh || nHeight < registerFaceHeightThresh)
		{
			return ReturnCode::IMAGE_TOO_SMALL;
		}

		/*
		if (BlurrFaceEliminate(cvImg,1))
		{
		return -1;
		}
		*/

		const int n = 1;
		//face detect
		THFI_FacePos ptfp1[n];
		int k;

		int face_num = FaceImage::DetectFace(0, cvImg.data, 24, nWidth, nHeight, ptfp1, 1);

		if (face_num <= 0)
		{
			return ReturnCode::NO_FACE;
		}
		if (abs(ptfp1[0].fAngle.pitch) > registerFaceYawThresh || abs(ptfp1[0].fAngle.roll) > registerFaceRollThresh || abs(ptfp1[0].fAngle.yaw) > registerFaceYawThresh){
			return ReturnCode::ILLEGAL_FACE_ANGLE;
		}
		if (registerFaceQualityThresh > ptfp1[0].nQuality)
			return ReturnCode::ILLEGAL_FACE_QUALITY;

		if (abs(ptfp1[0].rcFace.bottom - ptfp1[0].rcFace.top) < registerFaceHeightThresh || abs(ptfp1[0].rcFace.right - ptfp1[0].rcFace.left) < registerFaceWidthThresh)
			return ReturnCode::ILLEGAL_FACE_SIZE;

		//BYTE* pFeature1 = new BYTE[EF_Size()];
		array<BYTE>^ pFeature = gcnew array<BYTE>(Feature::Size());
		//only extract the first face(max size face)
		int ret = Feature::Extract(0, cvImg, nWidth, nHeight, 3, (THFI_FacePos*)&ptfp1[0], pFeature);
		usr->feature_data = pFeature;
		usr->quality_score = ptfp1[0].nQuality;

		String^ fileName = System::Guid::NewGuid().ToString() + L".jpg";

		String ^savePath = Path::Combine(regFaceDir, fileName);

		usr->face_image_path = savePath;

		//缩放
		cv::Mat Registerface;
		cv::resize(cvImg, Registerface, cv::Size(100, cvImg.rows * 100 / cvImg.cols));

		if (usrbll->Add(usr))
		{
			cv::imwrite(((char*)(void*)Marshal::StringToHGlobalAnsi(savePath)), Registerface);
			return ReturnCode::SUCCESS;
		}
		else
		{
			return ReturnCode::WRITE_TO_DATABASE_FAILED;
		}
	}
	catch (Exception ^e)
	{
		ShowMsgEvent("Register Error:", e);
		Console::WriteLine(e->Message + "," + System::Environment::CommandLine);
		return ReturnCode::UNKOWN_EXCEPTION;
	}
	return ReturnCode::SUCCESS;

}

Int32 FeatureData::Register(cv::Mat& cvImg, Model::user ^ usr, String^ library)
{
	if (cvImg.channels() < 3) throw gcnew   ArgumentException("image must have more than 3 channels");
	try{
		if (allUsers == nullptr) allUsers = gcnew List<Model::user^>();

		int nWidth = cvImg.cols;
		int nHeight = cvImg.rows;

		if (nWidth < registerFaceWidthThresh || nHeight < registerFaceHeightThresh)
		{
			return ReturnCode::IMAGE_TOO_SMALL;
		}

		/*
		if (BlurrFaceEliminate(cvImg,1))
		{
		return -1;
		}
		*/

		const int n = 1;
		//face detect
		THFI_FacePos ptfp1[n];
		int k;

		int face_num = FaceImage::DetectFace(0, cvImg.data, 24, nWidth, nHeight, ptfp1, 1);

		if (face_num <= 0)
		{
			return ReturnCode::NO_FACE;
		}
		if (abs(ptfp1[0].fAngle.pitch) > registerFaceYawThresh || abs(ptfp1[0].fAngle.roll) > registerFaceRollThresh || abs(ptfp1[0].fAngle.yaw) > registerFaceYawThresh){
			return ReturnCode::ILLEGAL_FACE_ANGLE;
		}
		if (registerFaceQualityThresh > ptfp1[0].nQuality)
			return ReturnCode::ILLEGAL_FACE_QUALITY;

		if (abs(ptfp1[0].rcFace.bottom - ptfp1[0].rcFace.top) < registerFaceHeightThresh || abs(ptfp1[0].rcFace.right - ptfp1[0].rcFace.left) < registerFaceWidthThresh)
			return ReturnCode::ILLEGAL_FACE_SIZE;

		//BYTE* pFeature1 = new BYTE[EF_Size()];
		array<BYTE>^ pFeature = gcnew array<BYTE>(Feature::Size());
		//only extract the first face(max size face)
		int ret = Feature::Extract(0, cvImg, nWidth, nHeight, 3, (THFI_FacePos*)&ptfp1[0], pFeature);
		usr->feature_data = pFeature;
		usr->quality_score = ptfp1[0].nQuality;

		String^ fileName = System::Guid::NewGuid().ToString() + L".jpg";

		String ^savePath = Path::Combine(regFaceDir, fileName);

		usr->face_image_path = savePath;

		//缩放
		cv::Mat Registerface;
		cv::resize(cvImg, Registerface, cv::Size(100, cvImg.rows * 100 / cvImg.cols));

		if (usrbll->Add(usr, library))
		{
			cv::imwrite(((char*)(void*)Marshal::StringToHGlobalAnsi(savePath)), Registerface);
			return ReturnCode::SUCCESS;
		}
		else
		{
			return ReturnCode::WRITE_TO_DATABASE_FAILED;
		}
	}
	catch (Exception ^e)
	{
		ShowMsgEvent("Register Error:", e);
		Console::WriteLine(e->Message + "," + System::Environment::CommandLine);
		return ReturnCode::UNKOWN_EXCEPTION;
	}
	return ReturnCode::SUCCESS;

}
Int32 FeatureData::Register(String^ filePath, String^ username)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");
#endif
	cv::Mat cvImg = cv::imread((char*)(void*)Marshal::StringToHGlobalAnsi(filePath), 1);
	if (cvImg.data == nullptr)  {
		Console::WriteLine("Load{0} image error!", filePath);
		return ReturnCode::IMAGE_RAED_FAILED;
	}

	UserInfo^ userInfo = gcnew UserInfo();
	userInfo->name = username;

	return Register(cvImg, userInfo);

}
Int32 FeatureData::Register(cv::Mat& cvImg, UserInfo^ userInfo)
{
	Model::user ^usr = gcnew Model::user();
	usr->name = userInfo->name;
	usr->gender = userInfo->gender;
	usr->card_id = userInfo->cardId;
	usr->people_id = userInfo->peopleId;
	usr->image_id = userInfo->imageId;
	usr->type = userInfo->type;

	return Register(cvImg, usr);
}

Int32 FeatureData::Register(cv::Mat& cvImg, UserInfo^ userInfo, String^ library)
{
	Model::user ^usr = gcnew Model::user();
	usr->name = userInfo->name;
	usr->gender = userInfo->gender;
	usr->card_id = userInfo->cardId;
	usr->people_id = userInfo->peopleId;
	usr->image_id = userInfo->imageId;
	usr->type = userInfo->type;

	return Register(cvImg, usr, library);
}


Int32 FeatureData::Register(Image^ img, UserInfo^ userInfo)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	cv::Mat cvImg = BitmapConverter::ToMat(img);
	/*imshow("demo", cvImg);
	cv::waitKey(-1);*/
	return Register(cvImg, userInfo);
}


Int32 FeatureData::Register(String^ filePath)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	FileInfo ^file = gcnew FileInfo(filePath);
	return Register(filePath, System::IO::Path::GetFileNameWithoutExtension(file->Name));
}
Int32 FeatureData::Register(String^ filePath, UserInfo^ userInfo)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	cv::Mat cvImg = cv::imread((char*)(void*)Marshal::StringToHGlobalAnsi(filePath), 1);
	if (cvImg.empty()) return ReturnCode::IMAGE_RAED_FAILED;

	return  Register(cvImg, userInfo);
}
Int32 FeatureData::Unregister(String^ name)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	if (allUsers == nullptr) return -1;
	if (usrbll->DeleteByName(name))
		return ReturnCode::SUCCESS;
	else
		return ReturnCode::UNKOWN_EXCEPTION;

}

#pragma region Load data into stack

Int32 FeatureData::LoadData()
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif

	try{
		allUsers = usrbll->GetAllUser();
	}
	catch (Exception ^e){
		MessageBox::Show("GetAllUser Error:" + e->Message);
		return ReturnCode::UNKOWN_EXCEPTION;
	}
	return ReturnCode::SUCCESS;
}

Int32 FeatureData::LoadData(String^ libraryname)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif

	try{
		allUsers = usrbll->GetAllUser(libraryname);
	}
	catch (Exception ^e){
		MessageBox::Show("GetAllUser Error:" + e->Message);
		return ReturnCode::UNKOWN_EXCEPTION;
	}
	return ReturnCode::SUCCESS;
}

#pragma endregion



array<HitAlertDetail>^ FeatureData::Search(array<BYTE> ^feats)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	pin_ptr<BYTE> feat_src = &(feats[0]);
	return Search(feat_src);

}
array<HitAlert^>^ FeatureData::Search(Image^ image)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	if (allUsers == nullptr) return nullptr;
	System::Drawing::Bitmap ^ _image = safe_cast<System::Drawing::Bitmap ^>(image);
	cv::Mat cvImg = BitmapConverter::ToMat(_image);
	return this->Search(cvImg);

}

array<HitAlert^>^ FeatureData::Search(cv::Mat& cvImg)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	if (cvImg.empty() && allUsers == nullptr)
	{
		MessageBox::Show("allUsers == nullptr");
		return nullptr;
	}


	if (cvImg.channels() < 3)
	{
		ShowMsgEvent("Over!", nullptr);
		//throw gcnew ArgumentException("src_image must have more than 3 channel");
		return nullptr;
	}
	//Bitmap ^srcImage = gcnew System::Drawing::Bitmap(cvImg.cols, cvImg.rows, cvImg.step, System::Drawing::Imaging::PixelFormat::Format24bppRgb, (System::IntPtr) cvImg.data);
	Bitmap ^srcImage = BitmapConverter::ToBitmap(&cvImg);



	int nWidth = cvImg.cols;
	int nHeight = cvImg.rows;

	// Detect faces

	THFI_FacePos *faces = new THFI_FacePos[maxPersonNum];

	int face_num = FaceImage::DetectFace(0, cvImg.data, 24, cvImg.cols, cvImg.rows, faces, maxPersonNum);

	if (face_num <= 0)
	{
		return nullptr;
	}

#pragma region CS架构主界面下侧显示抓拍人脸

	List < Image^>^ facesimg = gcnew  List<Image^>();

	for (int i = 0; i < MIN(maxPersonNum, face_num); i++)
	{
		if (searchFaceQualityThresh > faces[i].nQuality){
			ShowMsgEvent("searchFaceQualityThresh:" + faces[i].nQuality, nullptr);
			continue;
		}
		if (abs(faces[i].fAngle.yaw) > searchFaceYawThresh || abs(faces[i].fAngle.pitch) > searchFacePitchThresh || abs(faces[i].fAngle.roll) > searchFaceRollThresh)
		{
			ShowMsgEvent("angle" + faces[i].fAngle.yaw + ",roll:" + faces[i].fAngle.roll + ",pitch:" + faces[i].fAngle.pitch, nullptr);
			continue;
		}
		if (faces[i].rcFace.bottom - faces[i].rcFace.top < searchFaceHeightThresh || faces[i].rcFace.right - faces[i].rcFace.left < searchFaceWidthThresh)
		{
			ShowMsgEvent("size,height:" + (faces[i].rcFace.bottom - faces[i].rcFace.top) + ",width:" + (faces[i].rcFace.bottom - faces[i].rcFace.top), nullptr);
			continue;
		}

		System::Drawing::Rectangle retFaceRect = GetScaleFaceRect(nWidth, nHeight, faces[i].rcFace, faceRectScale);

		Image^ imgFace = srcImage->Clone(retFaceRect, srcImage->PixelFormat);

		facesimg->Add(imgFace);

		/*cv::Rect rect(faces[i].rcFace.left, faces[i].rcFace.top, faces[i].rcFace.right - faces[i].rcFace.left, faces[i].rcFace.bottom - faces[i].rcFace.top);
		cv::Mat faceimg = cvImg(rect);
		facesimg->Add((Image^)BitmapConverter::ToBitmap(&faceimg));		*/

	}

	if (facesimg->Count > 0)
		FaceDetectedEvent(facesimg->ToArray());
#pragma endregion

	//array<HitAlert^>^ resultNoThresh = gcnew array<HitAlert^> (MIN(maxPersonNum, face_num));
	List<HitAlert^>^ result = gcnew List<HitAlert^>();
	for (int i = 0; i < MIN(maxPersonNum, face_num); i++)
	{
		if (searchFaceQualityThresh > faces[i].nQuality){
			ShowMsgEvent("searchFaceQualityThresh:" + faces[i].nQuality, nullptr);
			continue;
		}
		if (abs(faces[i].fAngle.yaw) > searchFaceYawThresh || abs(faces[i].fAngle.pitch) > searchFacePitchThresh || abs(faces[i].fAngle.roll) > searchFaceRollThresh)
		{
			ShowMsgEvent("angle" + faces[i].fAngle.yaw + ",roll:" + faces[i].fAngle.roll + ",pitch:" + faces[i].fAngle.pitch, nullptr);
			continue;
		}
		if (faces[i].rcFace.bottom - faces[i].rcFace.top < searchFaceHeightThresh || faces[i].rcFace.right - faces[i].rcFace.left < searchFaceWidthThresh)
		{
			ShowMsgEvent("size,height:" + (faces[i].rcFace.bottom - faces[i].rcFace.top) + ",width:" + (faces[i].rcFace.bottom - faces[i].rcFace.top), nullptr);
			continue;
		}

		HitAlert ^hit = gcnew HitAlert();
		array<HitAlertDetail>^ details = nullptr;

		/* extract features */
		BYTE* feats = new BYTE[Feature::Size()];
		//array<BYTE>^ pFeature = gcnew array<BYTE>(Feature::Size());
		//only extract the first face(max size face)

		int ret = Feature::Extract(0, cvImg.data, cvImg.cols, cvImg.rows, 3, (THFI_FacePos*)&faces[i], feats);

		details = Search(feats);

		//std::cout << faces[i].bbox.x<<"," << faces[i].bbox.y <<","<< faces[i].bbox.width <<","<< faces[i].bbox.height << std::endl;

		//printf("%d\n", details->Length);
		System::Drawing::Rectangle retFaceRect = GetScaleFaceRect(nWidth, nHeight, faces[i].rcFace, faceRectScale);
		Image^ imgFace = srcImage->Clone(retFaceRect, srcImage->PixelFormat);

		//缩放
		Int32 width = 100;
		Int32 height = imgFace->Height * 100 / imgFace->Width;
		Bitmap^ faceBitmap = gcnew Bitmap(width, height);
		Graphics^ g = Graphics::FromImage(faceBitmap);
		g->DrawImage(imgFace, System::Drawing::Rectangle(0, 0, width, height), System::Drawing::Rectangle(0, 0, imgFace->Width, imgFace->Height), GraphicsUnit::Pixel);


		hit->QueryFace = faceBitmap;
		hit->OccurTime = DateTime::Now;
		hit->Details = details;
		hit->Threshold = scoreThresh;
		result->Add(hit);
		delete[]feats;

	}


#pragma region 保存至数据库
	for (int n = 0; n < result->Count; n++){
		HitAlert ^frsha = result[n];
		Model::hitalert ^ha = gcnew Model::hitalert();
		ha->details = gcnew array<Model::hitrecord_detail^>(frsha->Details->Length);
		String ^savePath = queryFaceDir + System::Guid::NewGuid().ToString() + L".jpg";
		frsha->QueryFace->Save(savePath);

		frsha->QueryFacePath = savePath;

		ha->hit = gcnew Model::hitrecord();
		ha->hit->face_query_image_path = savePath;
		ha->hit->occur_time = DateTime::Now;
		ha->hit->threshold = scoreThresh;

		for (int i = 0; i < frsha->Details->Length; i++)
		{
			Model::hitrecord_detail ^hd = gcnew Model::hitrecord_detail();
			hd->rank = i;
			hd->score = safe_cast<HitAlertDetail^>(frsha->Details[i])->Score;
			hd->user_id = safe_cast<HitAlertDetail^>(frsha->Details[i])->UserId;
			ha->details[i] = hd;
		}

		habll->Add(ha);
	}

#pragma endregion

#pragma region 上传至云平台
	//	int sequenceCode = 0;
	//	for each(Image^ im in faceImgSet)
	//	{
	//		String^ faceFileName = String::Format("{0}_{1}_{2}_face.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_HH_mm_ss"), sequenceCode);
	//		//inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Normal_Face, faceFileName, ImageHelper::ImageToBytes(im));
	//	}
	//	for each(auto hit in result){
	//		if (nullptr != hit->Details&&hit->Details->Length > 0){
	//			long sequenceNumber = 0;
	//			String^ faceFileName = String::Format("{0}_{1}_{2}_alarm_face.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_HH_mm_ss"), 1);
	//			//inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Alarm_Face, faceFileName, ImageHelper::ImageToBytes(hit->QueryFace));
	//			String^ captFileName = String::Format("{0}_{1}_{2}_alarm_capt.jpg", cameraCode, DateTime::Now.ToString("yyyy_MM_dd_HH_mm_ss"), 1);
	//			//inf4->SendAsync(defenseCode, cameraCode, INF4::SaveImageType::SaveImageType_Alarm_CapturedImage, captFileName, ImageHelper::ImageToBytes(faceImgSet[0]));
	//
	//			/*inf5->ReportAlarmAsync(
	//			DateTime::Now.ToString("yyyy-MM-dd HH:mm:ss"),
	//			captFileName, String::Empty, faceFileName,
	//			safe_cast<HitAlertDetail>(hit->Details[0]).peopleId,
	//			safe_cast<HitAlertDetail>(hit->Details[0]).imageId,
	//			safe_cast<HitAlertDetail>(hit->Details[0]).Score.ToString("F4"));*/
	//			ShowMsgEvent(
	//				"capture_Time:" + DateTime::Now.ToString("yyyy-MM-dd HH:mm:ss") +
	//				" capture_image:" + captFileName +
	//				" capture_video：" + String::Empty +
	//				" face_image:" + faceFileName +
	//				" people_id：" + safe_cast<HitAlertDetail>(hit->Details[0]).peopleId +
	//				" image_id:" + safe_cast<HitAlertDetail>(hit->Details[0]).imageId +
	//				" match_degree：" + safe_cast<HitAlertDetail>(hit->Details[0]).Score.ToString("F4"), nullptr);
	//}
	//
	//	}
#pragma endregion

#pragma region 保存识别数据C版本 
	//String^ GenDir = "record\\";
	////std::cout << "Record_hitResult=" << hitResult->Count << std::endl;
	//for each(auto hit in result){
	//	if (nullptr != hit->Details&&hit->Details->Length > 0){
	//		String^ FileName = safe_cast<HitAlertDetail>(hit->Details[0]).peopleId;
	//		float score = safe_cast<HitAlertDetail>(hit->Details[0]).Score;
	//		std::string dir = msclr::interop::marshal_as<std::string>(GenDir + FileName);
	//		std::cout << dir << std::endl;
	//		if (_access(dir.c_str(), 0)){
	//			_mkdir(dir.c_str()); /*这是在程序所在当前文件夹下创建*/
	//		}
	//		//detect face
	//		system(("dir /b /s /a-d G:\\新街口\\FRS_new\\FaceAngineNew\\FaceAngine\\" + dir + "\\*.* | find /c \":\" >D:\\nfiles.txt").c_str());
	//		//读文件d:\\nfiles.txt的内容即d:\\mydir目录下的文件数
	//		std::ifstream in;
	//		std::string str;
	//		in.open("D:\\nfiles.txt");
	//		if (!in.is_open())
	//		{
	//			std::cout << "Error opening file"; exit(1);
	//		}
	//		else
	//		{
	//			std::copy(std::istream_iterator<unsigned char>(in), std::istream_iterator<unsigned char>(), back_inserter(str));
	//		}
	//		std::stringstream ss;
	//		ss << score;
	//		std::string t;
	//		ss >> t;
	//		cv::imwrite(dir + "\\ptoto_" + str + "_" + t + ".jpg", copy);
	//	}
	//}
#pragma endregion
	//保存识别数据
	CollectTrainData(result);
	System::GC::Collect();
	delete[]faces;
	return result->ToArray();

}


#pragma region 保存识别数据
bool FeatureData::CollectTrainData(List<HitAlert^>^ result)
{
	bool dataPathState = SetDataPath("训练样本数据");
	if (dataPathState == false){
		ShowMsgEvent("dataPath设置有问题", nullptr);
		return false;
	}
	for each(auto hit in result){
		if (nullptr != hit->Details&&hit->Details->Length > 0){
			ShowMsgEvent("NLG Save", nullptr);
			Threading::Monitor::Enter(objImgFileIO);
			int imgCountMatch = hit->Details->Length;
			Image^ imgLocateBest = hit->QueryFace;
			if (nullptr == imgLocateBest || 0 == imgCountMatch)
			{
				return false;
			}
			try
			{
				if (hit->Details->Length > 0){
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
						array<String^>^ dirPathSet = Directory::GetDirectories(trainingDataDir, matchFaceId + "_*");
						int existDirCount = dirPathSet->Length;

						String^ matchDir = matchFaceId.ToString() + "_" + existDirCount;
						userDataDir = Path::Combine(trainingDataDir, matchDir);
						Directory::CreateDirectory(userDataDir);
					}
					ShowMsgEvent(userDataDir + "here", nullptr);
					Threading::Monitor::Exit(objImgFileIO);
					ShowMsgEvent(userDataDir, nullptr);
					//保存南理工最佳的人脸检测截取图片
					String^ locateBestImgPath = "NLG-Best_" + safe_cast<HitAlertDetail>(hit->Details[0]).Score.ToString() + ".jpg";
					locateBestImgPath = Path::Combine(userDataDir, locateBestImgPath);
					ShowMsgEvent(locateBestImgPath, nullptr);
					imgLocateBest->Save(locateBestImgPath);

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
				}

			}
			catch (Exception^ ex)
			{
				ShowMsgEvent("Image File IO Save Error", ex);
			}
		}
	}

	return true;
}
#pragma endregion

array<HitAlertDetail>^ FeatureData::Search(BYTE* feat_src)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	if (allUsers == nullptr) return nullptr;

	List<HitAlertDetail>^ details = gcnew List<HitAlertDetail>();
	for each(auto user in allUsers)
	{
		pin_ptr<Byte> feat_dst = &((safe_cast<array<Byte>^>(user->feature_data))[0]);
		float score = Feature::Compare(feat_src, feat_dst);
		//printf("%f \n", score);
		/*only add the record which score >scoreThresh*/
		if (scoreThresh > score) continue;
		HitAlertDetail detail;

		detail.Score = score;
		detail.UserId = user->id;
		detail.name = user->name;
		detail.gender = user->gender;
		detail.imgPath = user->face_image_path;
		detail.imageId = user->image_id;
		detail.peopleId = user->people_id;
		detail.cardId = user->card_id;
		for (int i = 0; i < details->Count; i++)
		{
			if (details[i].Score < detail.Score)
			{
				HitAlertDetail tmp = details[i];
				details[i] = detail;
				detail = tmp;
			}
		}
		if (details->Count < MIN(topK, allUsers->Count))
		{
			details->Add(detail);
		}
	}
	//printf("%d\n", details->Count);
	return details->ToArray();
}

//HitAlert^ FeatureData::SearchBulk(List<Image^>^ imgs, float scoreThresh, float qualityThresh, float faceRectScale, int topk, Int32 maxPersonNum, int detectChannel)
//{
//	HitAlert ^hit = gcnew HitAlert();
//#pragma region 找出一最好的张人脸
//	Image ^bestFaceImg = imgs[0];
//	int minAngle = INT_MAX;
//
//	THFI_FacePos *ptfp = new THFI_FacePos[maxPersonNum];
//	for (int k = 0; k< maxPersonNum; k++)
//	{
//		ptfp[k].dwReserved = (DWORD)new BYTE[512];
//	}
//
//	for each (auto img in imgs)
//	{
//		cv::Mat cvImg = FRS::Util::BitmapConverter::ToMat(img);
//
//		Bitmap^ bitMapImg = (Bitmap^)img;
//
//		int nFace = FaceImage::DetectFace(detectChannel, cvImg.data, 24, cvImg.cols, cvImg.rows, ptfp, maxPersonNum);
//		if (nFace > 0)
//		{
//			int angle = 0;
//			//可以替换其他策略
//			angle = ptfp->fAngle.pitch + ptfp->fAngle.roll + ptfp->fAngle.yaw;
//			if (angle < minAngle)
//			{		
//				Drawing::Rectangle retFaceRect = GetScaleFaceRect(img->Width, img->Height, ptfp->rcFace, faceRectScale);
//				if (retFaceRect.IsEmpty)
//				{
//					continue;
//				}
//
//				/*
//				retFaceRect.X = 0;
//				retFaceRect.Y= 0;
//				retFaceRect.Width = img->Width;
//				retFaceRect.Height = img->Height;
//				*/
//
//				minAngle = angle;		
//				bestFaceImg = bitMapImg->Clone(retFaceRect, bitMapImg->PixelFormat);
//			}
//		}
//	}
//
//	for (int k = 0; k < maxPersonNum; k++)
//	{
//		delete[](BYTE*)ptfp[k].dwReserved;
//	}
//
//	hit->OccurTime = DateTime::Now;
//	hit->QueryFace = bestFaceImg;
//	hit->Threshold = scoreThresh;
//#pragma endregion
//
//#pragma region 找出最好的三个结果
//	List<HitAlert^>^ resall = gcnew List<HitAlert^>();
//	Dictionary<int, CountScore^>^ allres = gcnew Dictionary<int, CountScore^>();
//	for each (auto img in imgs)
//	{
//		array<HitAlert^>^res = Search(img, scoreThresh, qualityThresh, faceRectScale, topk, maxPersonNum, detectChannel);
//		if (res != nullptr && res->Length > 0)
//		{
//			for each (auto detail in res[0]->Details)
//			{
//				if (!allres->ContainsKey(detail.UserId))
//				{
//					CountScore^ countScore = gcnew CountScore(1, detail.Score);
//					countScore->name = detail.name;
//					countScore->imgPath = detail.imgPath;
//					allres->Add(detail.UserId, countScore);
//				}
//				allres[detail.UserId]->score = allres[detail.UserId]->score*allres[detail.UserId]->count + detail.Score;
//				allres[detail.UserId]->count++;
//				allres[detail.UserId]->score /= allres[detail.UserId]->count;
//			}
//		}
//	}
//
//	List<HitAlertDetail>^ details = gcnew List<HitAlertDetail>();
//
//	/*
//	for each(auto r in allres)
//	{
//		HitAlertDetail detail;
//
//		detail.UserId = r.Key;
//		detail.Score = r.Value->score;
//		detail.name = r.Value->name;
//		detail.imgPath = r.Value->imgPath;
//
//
//		for (int i = 0; i < details->Count; i++)
//		{
//
//			if (details[i].Score < detail.Score)
//			{
//				HitAlertDetail tmp = details[i];
//				details[i] = detail;
//				detail = tmp;
//			}
//		}
//		if (details->Count <topk)
//		{
//			details->Add(detail);;
//		}
//	}
//	*/
//
//	int max_count0 = 0, max_count1 = 0, max_count2 = 0;
//	HitAlertDetail max_detail0, max_detail1, max_detail2;
//	for each(auto r in allres)
//	{
//		HitAlertDetail detail;
//
//		detail.UserId = r.Key;
//		detail.Score = r.Value->score;
//		detail.name = r.Value->name;
//		detail.imgPath = r.Value->imgPath;
//
//		if (r.Value->count > max_count0)
//		{
//			max_count0 = r.Value->count;
//			max_detail0 = detail;
//		}
//		else if (r.Value->count > max_count1)
//		{
//			max_count1 = r.Value->count;
//			max_detail1 = detail;
//		}
//		else if (r.Value->count > max_count2)
//		{
//			max_count2 = r.Value->count;
//			max_detail2 = detail;
//		}
//	}
//	
//	if (max_count0 > 0)
//	{
//		details->Add(max_detail0);
//	}
//	if (max_count1 > 0)
//	{
//		details->Add(max_detail1);
//	}
//	if (max_count2 > 0)
//	{
//		details->Add(max_detail2);
//	}
//
//	hit->Details = details->ToArray();
//#pragma endregion
//	return hit;
//}

float FeatureData::Compare(Image^ src_image, Image^ dst_image)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	System::Drawing::Bitmap ^ _src_image = safe_cast<System::Drawing::Bitmap ^>(src_image);
	cv::Mat src_img = BitmapConverter::ToMat(_src_image);
	System::Drawing::Bitmap ^ _dst_image = safe_cast<System::Drawing::Bitmap ^>(dst_image);
	cv::Mat dst_img = BitmapConverter::ToMat(_dst_image);
	return this->Compare(src_img, dst_img);
}

float FeatureData::Compare(cv::Mat& src_img, cv::Mat& dst_img)
{
#if USE_EXPIRE
	if (IsExpired()) throw gcnew Exception("software has been expired");;
#endif
	if (src_img.channels() < 3 || dst_img.channels() < 3)
	{
		ShowMsgEvent("Picture Error!", nullptr);
		//throw gcnew ArgumentException("src_image must have more than 3 channel");
		return 0;
	}
	//Bitmap ^srcImage = gcnew System::Drawing::Bitmap(cvImg.cols, cvImg.rows, cvImg.step, System::Drawing::Imaging::PixelFormat::Format24bppRgb, (System::IntPtr) cvImg.data);
	Bitmap ^srcImage = BitmapConverter::ToBitmap(&src_img);
	Bitmap ^dstImage = BitmapConverter::ToBitmap(&dst_img);

	// Detect faces
	THFI_FacePos *src_faces = new THFI_FacePos[1];
	THFI_FacePos *dst_faces = new THFI_FacePos[1];

	int src_face_num = FaceImage::DetectFace(0, src_img.data, 24, src_img.cols, src_img.rows, src_faces, 1);
	int dst_face_num = FaceImage::DetectFace(0, dst_img.data, 24, dst_img.cols, dst_img.rows, dst_faces, 1);

	if (src_faces <= 0 || dst_faces <= 0)
	{
		ShowMsgEvent("Face Error!", nullptr);
		return 0;
	}

	//Extract features
	BYTE* src_feats = new BYTE[Feature::Size()];
	BYTE* dst_feats = new BYTE[Feature::Size()];
	int src_ret = Feature::Extract(0, src_img.data, src_img.cols, src_img.rows, 3, (THFI_FacePos*)&src_faces[0], src_feats);
	int dst_ret = Feature::Extract(0, dst_img.data, dst_img.cols, dst_img.rows, 3, (THFI_FacePos*)&dst_faces[0], dst_feats);

	float score = Feature::Compare(src_feats, dst_feats);
	delete[]src_feats;
	delete[]dst_feats;
	delete[]src_faces;
	delete[]dst_faces;
	return score;
}

HitAlert^ FeatureData::SearchBulk(List<Image^>^ imgs)
{

	HitAlert ^hit = gcnew HitAlert();
	List<HitAlertDetail>^ details = gcnew List<HitAlertDetail>();
#pragma region 找出一最好的张人脸
	Image ^bestFaceImg = imgs[0];
	int minAngle = INT_MAX;

	for each (auto img in imgs)
	{
		THFI_FacePos ptfp[1];
		cv::Mat image = FRS::Util::BitmapConverter::ToMat(img);
		int nFace = FaceImage::DetectFace(0, image.data, 24, image.cols, image.rows, ptfp, 1);
		if (nFace > 0)
		{
			int angle = 0;
			//可以替换其他策略
			angle = ptfp->fAngle.pitch + ptfp->fAngle.roll + ptfp->fAngle.yaw;
			if (angle < minAngle)
			{
				minAngle = angle;
				bestFaceImg = img;
			}
		}
	}
	hit->OccurTime = DateTime::Now;
	hit->QueryFace = bestFaceImg;
	hit->Threshold = scoreThresh;
	hit->Details = details->ToArray();
#pragma endregion

#pragma region 找出最好的结果
	List<HitAlert^>^ resall = gcnew List<HitAlert^>();
	Dictionary<int, CountScore^>^ allres = gcnew Dictionary<int, CountScore^>();
	for each (auto img in imgs)
	{

		array<HitAlert^>^res = Search(img);

		if (res != nullptr && res[0] != nullptr && res->Length > 1)
		{
			//若返回的结果大于两个，比较这两个之间是否大于times倍,小于的话清空Details，直接返回
			if (res[0]->Details != nullptr&&res[0]->Details->Length >= 2)
			{
				if (safe_cast<HitAlertDetail>(res[0]->Details[0]).Score / safe_cast<HitAlertDetail>(res[0]->Details[1]).Score < times)
				{
					hit->Details = gcnew array<HitAlertDetail>(0);
					return hit;
				}

			}
			if (res[0]->Details != nullptr && res[0]->Details->Length > 1)
			{

				HitAlertDetail minDetail = safe_cast<HitAlertDetail>(res[0]->Details[0]);
				for (int i = 0; i < details->Count; i++)
				{
					if (details[i].Score < minDetail.Score)
					{
						HitAlertDetail tmp = details[i];
						details[i] = minDetail;
						minDetail = tmp;
					}
				}
				if (details->Count < topK)
					details->Add(minDetail);
			}

		}

	}
#pragma endregion
	hit->Details = details->ToArray();
	return hit;


}
LocateFaceInfo^ FeatureData::LocateFace(cv::Mat& cvImg)
{
	LocateFaceInfo ^lfi = gcnew LocateFaceInfo();

	if (cvImg.channels() != 3)
	{
		return nullptr;
	}

	int nWidth = cvImg.cols;
	int nHeight = cvImg.rows;
	int stride = cvImg.step;


	THFI_FacePos *faces = new THFI_FacePos[maxPersonNum];

	int faceNum = FaceImage::DetectFace(0, cvImg.data, 24, nWidth, nHeight, faces, maxPersonNum);
	if (faceNum <= 0)
	{
		//		String ^savePath = queryProblemFaceDir + System::Guid::NewGuid().ToString() + L".jpg";
		//		cv::imwrite(((char*)(void*)Marshal::StringToHGlobalAnsi(savePath)), cvImg);

		return nullptr;
	}

	faceNum = MIN(maxPersonNum, faceNum);

	Bitmap ^srcImage = gcnew Bitmap(nWidth, nHeight, stride, PixelFormat::Format24bppRgb, (System::IntPtr) cvImg.data);

	List<Image^>^ imgs = gcnew List<Image^>();

	for (int i = 0; i < faceNum; ++i)
	{
		THFI_FacePos face = faces[i];
		if (face.nQuality < searchFaceQualityThresh)
		{
			continue;
		}

		/*
		System::Drawing::Rectangle blurrFaceRect = GetScaleFaceRect(nWidth, nHeight, face.rcFace, 0.6);
		Image^ blurrFace = srcImage->Clone(blurrFaceRect, srcImage->PixelFormat);
		if (BlurrFaceEliminate(blurrFace))
		{
		continue;
		}
		*/


		Drawing::Rectangle retFaceRect = GetScaleFaceRect(nWidth, nHeight, face.rcFace, faceRectScale);
		if (retFaceRect.IsEmpty)
		{
			continue;
		}

		/*
		retFaceRect.X = 0;
		retFaceRect.Y = 0;
		retFaceRect.Width = srcImage->Width;
		retFaceRect.Height = srcImage->Height;
		*/
		Image^ imgFace = srcImage->Clone(retFaceRect, srcImage->PixelFormat);

		if (nullptr != imgFace)
		{
			imgs->Add(imgFace);

			lfi->rectImgInfo = "widthCHC:" + nWidth + " heightCHC:" + nHeight + " rectX:" + retFaceRect.X + " rectY:" + retFaceRect.Y + " rectWidth:" + retFaceRect.Width + " rectHeight:" + retFaceRect.Height;
		}
	}


	lfi->FaceImgs = imgs->ToArray();

	return lfi;
}


void FeatureData::TagFacePos(cv::Mat& cvImg)
{
	if (cvImg.channels() != 3) return;

	int nWidth = cvImg.cols;
	int nHeight = cvImg.rows;

	THFI_FacePos *faces = new THFI_FacePos[maxPersonNum];

	int faceNum = FaceImage::DetectFace(0, cvImg.data, 24, nWidth, nHeight, faces, maxPersonNum);

	if (faceNum <= 0)
	{
		return;
	}

	faceNum = MIN(maxPersonNum, faceNum);

	for (int i = 0; i < faceNum; ++i)
	{
		THFI_FacePos face = faces[i];
		if (face.nQuality < searchFaceQualityThresh) continue;

		System::Drawing::Rectangle retFaceRect = GetScaleFaceRect(nWidth, nHeight, face.rcFace, faceRectScale);
		cv::Rect rectFaceImg(retFaceRect.X, retFaceRect.Y, retFaceRect.Width, retFaceRect.Height);
		cv::Scalar lineColor(0, 0, 255);
		rectangle(cvImg, rectFaceImg, lineColor);
	}


}

bool FeatureData::BlurrFaceEliminate(Image^ image)
{
	cv::Mat cvImg = BitmapConverter::ToMat(image);
	return BlurrFaceEliminate(cvImg);
}

bool FeatureData::BlurrFaceEliminate(cv::Mat& cvImg)
{
	bool eliminate = true;

	cv::Mat _tmp = cvImg.clone();
	if (cvImg.channels() != 1)
		cv::cvtColor(_tmp, _tmp, cv::COLOR_BGR2GRAY);
	cv::Scalar mean, stddev;

	cv::Laplacian(_tmp, _tmp, cvImg.depth());
	cv::meanStdDev(_tmp, mean, stddev);
	double stddev_pxl = stddev.val[0];

	if (stddev_pxl >= blurrThresh)
	{
		//		MessageBox::Show("stddev_pxl >= blurrThresh:" + stddev_pxl);
		eliminate = false;
	}

	return eliminate;
}

void FeatureData::RecordHitInfo(array<HitAlert^>^ hitArray)
{
	int hitCount = hitArray->Length;

	for (int i = 0; i < hitCount; ++i)
	{
		HitAlert^ hit = hitArray[i];

		int hitDetailCount = hit->Details->Length;
		if (hitDetailCount <= 0)
		{
			continue;
		}

		Model::hitalert ^ha = gcnew Model::hitalert();
		ha->details = gcnew array<Model::hitrecord_detail^>(1);

		String^ fileName = System::Guid::NewGuid().ToString() + L".jpg";

		String ^savePath = Path::Combine(queryFaceDir, fileName);

		Image^ queryImg = (Image^)hit->QueryFace->Clone();
		queryImg->Save(savePath);

		ha->hit = gcnew Model::hitrecord();
		ha->hit->face_query_image_path = savePath;
		ha->hit->occur_time = DateTime::Now;
		ha->hit->threshold = scoreThresh;

		HitAlertDetail^ hitDetail = safe_cast<HitAlertDetail^>(hit->Details[0]);

		Model::hitrecord_detail ^hd = gcnew Model::hitrecord_detail();
		hd->rank = 0;
		hd->score = hitDetail->Score;
		hd->user_id = hitDetail->UserId;
		ha->details[0] = hd;
		habll->Add(ha);

		/*if (recordHitUserMap->ContainsKey(hitDetail->UserId))
		{
		float hitScore = recordHitUserMap[hitDetail->UserId];

		if (hitScore >= hitDetail->Score)
		{
		return;
		}
		else
		{
		recordHitUserMap[hitDetail->UserId] = hitDetail->Score;
		}
		}
		else
		{
		recordHitUserMap->Add(hitDetail->UserId, hitDetail->Score);

		habll->Add(ha);
		}*/


		//String ^ localFilePath = Path::GetFullPath(savePath);

		/*String ^ server1FilePath = Path::Combine(server1Dir, savePath);
		String ^ server2FilePath = Path::Combine(server2Dir, savePath);

		File::Copy(localFilePath, server1FilePath, true);
		File::Copy(localFilePath, server2FilePath, true);*/

	}

}

//void FeatureData::statistics(DateTime timeStart, Int32 catchFaceCount, Int32 matchFaceCount)
//{
//	Model::statistics ^stModel = gcnew Model::statistics();
//	stModel->Id = 0;
//	stModel->StartTime = timeStart;
//	stModel->CatchFaceImgCount = catchFaceCount;
//	stModel->MatchFaceCount = matchFaceCount;
//	stModel->EndTime = DateTime::Now;
//
//	stbll->Update(stModel);
//}
//人脸框缩放
Drawing::Rectangle FeatureData::GetScaleFaceRect(int imgWidth, int imgHeight, RECT rectFacePos, float faceRectScale)
{
	int centerx = (rectFacePos.right + rectFacePos.left) / 2;
	int centery = (rectFacePos.bottom + rectFacePos.top) / 2;
	int width = rectFacePos.right - rectFacePos.left + 1;
	int height = rectFacePos.bottom - rectFacePos.top + 1;

	Drawing::Rectangle retFaceRect;

	int scaleRectX = centerx - width*faceRectScale / 2;
	retFaceRect.X = scaleRectX > 0 ? scaleRectX : 0;
	int scaleRectY = centery - height*faceRectScale / 2;
	retFaceRect.Y = scaleRectY > 0 ? scaleRectY : 0;
	int scaleWidth = width*faceRectScale;
	retFaceRect.Width = retFaceRect.X + scaleWidth < imgWidth ? scaleWidth : imgWidth - retFaceRect.X;
	int scaleHeight = height*faceRectScale;
	retFaceRect.Height = retFaceRect.Y + scaleHeight < imgHeight ? scaleHeight : imgHeight - retFaceRect.Y;

	if (retFaceRect.Width <= 0 || retFaceRect.X + retFaceRect.Width > imgWidth
		|| retFaceRect.Height <= 0 || retFaceRect.Y + retFaceRect.Height > imgHeight)
	{
		retFaceRect = Rectangle::Empty;
	}

	return retFaceRect;
}

