#pragma once

#include "FaceImage.h"
#include "Feature.h"
#include <opencv2/core/version.hpp>
#include <opencv2/calib3d/calib3d.hpp>
#include <opencv2/opencv.hpp>
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Text;
using namespace DataAngine;

namespace FRS {
	///ע��ʱ���ڴ������û���Ϣ
	public value class UserInfo
	{
	public:
		property String^ name;
		property String^ peopleId;
		property String^ gender;
		property String^ cardId;
		property String^ imageId;
		property String^ imgPath;
		property String^ type;
	};

	public value class HitAlertDetail
	{
	public:
		property UInt32 UserId;//
		property float Score;
//		property HitUserInfo UserInfo;
		property String^ name;
		property String^ peopleId;
		property String^ gender;
		property String^ cardId;
		property String^ imageId;
		property String^ imgPath;
		property String^ type;

	};
	public ref class HitAlert
	{
	public:
		HitAlert(){};
		property Image ^QueryFace;
		property float ^Threshold;
		property DateTime^ OccurTime;
		property array<HitAlertDetail> ^Details;
	};

	public ref struct  CountScore
	{
	public:
		CountScore(int count,
			float score)
		{
			this->count = count;
			this->score = score;
		}
		int count;
		float score;
		String ^name;
		String ^imgPath;
	};


	public ref class LocateFaceInfo
	{
	public:
		property array<Image^>^ FaceImgs;
		String ^rectImgInfo;
	};

	public ref class FeatureData {
	public:
		
		FeatureData();
		~FeatureData();
		!FeatureData();
#pragma region property


		/*
		*	 ����ʱ�����������ֵ
		*/
		property int SearchFaceWidthThresh
		{
			Int32 get()
			{
				return searchFaceWidthThresh;
			}
			void set(int value)
			{
				searchFaceWidthThresh = value;
			}
		};
		/*
		*	 ����ʱ�������߶���ֵ
		*/
		property int SearchFaceHeightThresh
		{
			Int32 get()
			{
				return searchFaceHeightThresh;
			}
			void set(int value)
			{
				searchFaceHeightThresh = value;
			}
		};
		/*
		*	 ����ʱ����������Ƕ���ֵ
		*/
		property int SearchFaceYawThresh
		{
			Int32 get()
			{
				return searchFaceYawThresh;
			}
			void set(int value)
			{
				searchFaceYawThresh = value;
			}
		};
		/*
		*	 ����ʱ���������½Ƕ���ֵ
		*/
		property int SearchFacePitchThresh
		{
			Int32 get()
			{
				return searchFacePitchThresh;
			}
			void set(int value)
			{
				searchFacePitchThresh = value;
			}
		};
		/*
		*	 ����ʱ���������ҽǶ���ֵ
		*/
		property int SearchFaceRollThresh
		{
			Int32 get()
			{
				return searchFaceRollThresh;
			}
			void set(int value)
			{
				searchFaceRollThresh = value;
			}
		};
		/*
		* ����ʱ��ֵ
		*/
		property float ScoreThresh
		{
			float get()
			{
				return scoreThresh;
			}
			void set(float value)
			{
				scoreThresh = value;
			}
		};
		/*
		* ÿ���������ص��������
		*/
		property Int32 TopK{
			Int32 get()
			{
				return topK;
			}
			void set(int value)
			{
				topK = value;
			}
		}
		/*
		*����ʱÿ֡��������������Ŀ
		*/
		property Int32 MaxPersonNum{
			Int32 get()
			{
				return maxPersonNum;
			}
			void set(int value)
			{
				maxPersonNum = value;
			}
		}

		/*
		*����ʱ������������ֵ
		*/
		property Int32 SearchFaceQualityThresh{
			Int32 get()
			{
				return searchFaceQualityThresh;
			}
			void set(int value)
			{
				searchFaceQualityThresh = value;
			}
		}
		/*
		*����ʱ���֮���ֵ
		*/
		property float Times{
			float get()
			{
				return times;
			}
			void set(float value)
			{
				//				Threading::Monitor::Enter(collectFaceCount);
				times = value;
				//				Threading::Monitor::Exit(collectFaceCount);
			}
		};


		/*
		*	 ע��ʱ�����������ֵ
		*/
		property int RegisterFaceWidthThresh
		{
			Int32 get()
			{
				return registerFaceWidthThresh;
			}
			void set(int value)
			{
				registerFaceWidthThresh = value;
			}
		};
		/*
		*	 ע��ʱ�������߶���ֵ
		*/
		property int RegisterFaceHeightThresh
		{
			Int32 get()
			{
				return registerFaceHeightThresh;
			}
			void set(int value)
			{
				registerFaceHeightThresh = value;
			}
		};
		/*
		*	 ע��ʱ����������Ƕ���ֵ
		*/
		property int RegisterFaceYawThresh
		{
			Int32 get()
			{
				return registerFaceYawThresh;
			}
			void set(int value)
			{
				registerFaceYawThresh = value;
			}
		};
		/*
		*	 ע��ʱ���������½Ƕ���ֵ
		*/
		property int RegisterFacePitchThresh
		{
			Int32 get()
			{
				return registerFacePitchThresh;
			}
			void set(int value)
			{
				registerFacePitchThresh = value;
			}
		};
		/*
		*	 ע��ʱ���������ҽǶ���ֵ
		*/
		property int RegisterFaceRollThresh
		{
			Int32 get()
			{
				return registerFaceRollThresh;
			}
			void set(int value)
			{
				registerFaceRollThresh = value;
			}
		};

		/*
		*ע��ʱ������������ֵ
		*/
		property Int32 RegisterFaceQualityThresh{
			Int32 get()
			{
				return registerFaceQualityThresh;
			}
			void set(int value)
			{
				registerFaceQualityThresh = value;
			}
		}
#pragma endregion

		Int32 FeatureData::RegisterInBulk(String^ fileDirPath);
		Int32 FeatureData::RegisterInBulkFromFile(String^ filePath);
		Int32 FeatureData::RegisterInBulk1(String^ fileDirPath);
		/** @brief  register a user;
		*	@param filePath   the file path of source image
		*	@param dataSavePath Path of the saving the feature data
		*/


		Int32 FeatureData::Register(String^ filePath, String^ username);

		Int32 FeatureData::Register(Image^ img, UserInfo^ userInfo);
		Int32 FeatureData::Register(String^ filePath,  UserInfo^ userInfo);
		Int32 FeatureData::Register(String^ filePath);



		/** @brief  Unregister a user ;
		*	@param username the user name
		*/
		Int32 Unregister(String^ username);

		/** @brief  load feature data from binary file;
		*
		*/
		Int32 LoadData();

		/** @brief search the user
		*	@param feat   the feature
		*/
		array<HitAlertDetail>^ Search(array<BYTE> ^feat);


		/** @brief search the user
		*	@param feat   the feature
		*/

		array<HitAlert^>^ FeatureData::Search(Image^ image);
		//HitAlert^ SearchBulk(List<Image^>^ imgs, float scoreThresh, float qualityThresh, float faceRectScale, int topk, Int32 maxPersonNum, int detectChannel);
		//��ÿ��ͼƬ��������������������������ֵС��times ����Ϊ�ȶ�ʧ�ܣ����ؿգ����򰴴���
		//���ص� HitAlert->Details �Ѿ��ź��򣬸���������List<Image^>����Ŀ
		HitAlert^ SearchBulk(List<Image^>^ imgs);

		float FeatureData::Compare(Image^ src_image, Image^ dst_image);


		LocateFaceInfo^ FeatureData::LocateFace(cv::Mat& frameImg );
		void FeatureData::TagFacePos(cv::Mat& frameImg);

		bool FeatureData::BlurrFaceEliminate(Image^ image);
		bool FeatureData::BlurrFaceEliminate(cv::Mat& frameImg);
		//����hitarray�����ݿ���
		void FeatureData::RecordHitInfo(array<HitAlert^>^ hitArray);
		//void FeatureData::statistics(DateTime timeStart, Int32 catchFaceCount, Int32 matchFaceCount);

		delegate void RegisterOneFinisedCallback(int count,String ^msg);
		event RegisterOneFinisedCallback^ RegisterOneFinisedEvent;

		delegate void ShowMsgCallback(String^ msgStr, Exception^ ex);
		event ShowMsgCallback^ ShowMsgEvent;

		delegate void FaceDetectedCallback(array<Image^>^ faces);
		event FaceDetectedCallback^ FaceDetectedEvent;

		bool SetDataPath(String^ dataPath);
		bool CollectTrainData(List<HitAlert^>^ result);

	internal:
		/** @brief search the user
		*   @param topk   return topk user
		*/
		array<HitAlertDetail>^ Search(BYTE* feat);
		/** @brief search the user
		*	@param image_color  3 channels image
		*	@param maxPersonNum  maximum person number in one image
		*/
		array<HitAlert^>^ FeatureData::Search(cv::Mat& img_color);
		
		float FeatureData::Compare(cv::Mat& src_img, cv::Mat& dst_img);

		/** @brief  register a user ;
		*	@param image_color
		*	@param username the user name
		*/
		Int32 Register(cv::Mat& image_color, UserInfo^ userInfo);
		Int32 FeatureData::Register(cv::Mat& img_color, Model::user ^ usr);

		System::Drawing::Rectangle FeatureData::GetScaleFaceRect(int imgWidth, int imgHeight, RECT rectFacePos, float faceRectScale);

	private:

		BLL::hitrecord ^hitbll;
		BLL::user ^usrbll;
		BLL::hitrecord_detail ^hdbll;
		BLL::hitalert ^habll;
		BLL::statistics ^stbll;
		
		List<Model::user^>^ allUsers = nullptr;
		/**
		*   feature data path
		*/
		String ^regFaceDir = L"RegFaces/";
		String ^queryFaceDir = L"QueryFaces/";
		String ^queryProblemFaceDir = L"QueryProblemFaces/";

		Object^ objImgFileIO = gcnew Object();
		String^ trainingDataDir = "";


#pragma region Search Threashold
		double blurrThresh = 10.0;
		Int32 searchFaceWidthThresh = 30;
		Int32 searchFaceHeightThresh = 30;
		Int32 searchFaceYawThresh = 60;
		Int32 searchFaceRollThresh = 60;
		Int32 searchFacePitchThresh = 60;

		float scoreThresh = 0.65f;
		Int32 topK = 3;
		Int32 maxPersonNum = 5;
		Int32 searchFaceQualityThresh = 30;
		float faceRectScale = 1.5f;
		float times = 1.0f;
#pragma endregion 

#pragma region register Threashold
		
		Int32 registerFaceWidthThresh = 80;
		Int32 registerFaceHeightThresh = 80;
		Int32 registerFaceYawThresh = 10;
		Int32 registerFaceRollThresh = 11;
		Int32 registerFacePitchThresh = 12;
		Int32 registerFaceQualityThresh = 31;
#pragma endregion 


	};
}