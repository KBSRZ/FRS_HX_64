
#pragma once
#include "FeatureData.h"
#include "Vlcopencv.h"

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Text;
using namespace System::Threading;
using namespace System::Collections::Concurrent;


namespace FRS {
	public ref class Capture {
	public:	

		Capture();
		Capture(FeatureData^ featureData);
		~Capture();
		!Capture();

		bool SetDataPath(String^ dataPath);

		/** @brief start  Capturing  device 0
		*/
		Int32 Start();

		/** @brief start Capturing RTSP
		*	@param streamAdress rtsp address
		*/	
		Int32 Start(String^ streamAdress);

		/** @brief stop Capturing device
		*	@param deciceId 
		*/
		Int32 Start(Int32 deciceId);
		
		/** @brief stop Capturing
		*
		*/
		Int32 Stop();

		void CatchFaceImg(Image ^img, int detectChannel);
		bool LocateFaceImg(cv::Mat mat, int detectChannel);
		void MatchFaceImg(Object^ o);

		void DetectFace4CHC(Image^ faceImg, int detectChannel);
		bool DetectFace4CHC(List<Image^>^ faceImgSet, int detectChannel);
		bool CollectTrainData4CHC(List<Image^>^ detectFaceImgSet, List<Image^>^ locateFaceImgSet, List<int>^ idxLocate2Detect, HitAlert^ hit);
		//用在Begin中显示图片
		Bitmap^  Capture::Retrive();

		property bool IsRun {
			bool get()
			{
				return isRun;
			}
		};
		
		/*
		*  interval of capture
		*/
		property Int32 Interval{
			Int32 get()
			{
				return interval;
			}
			void set(Int32 value)
			{
				if (value < 100)
					interval = 100;
				else
					interval = value;

			}
		};

		

		property DateTime TimeStart{
			DateTime get()
			{
				return timeStart;
			}
			void set(DateTime value)
			{
				timeStart = value;
			}
		};

		property Int32 CatchFaceCount{
			Int32 get()
			{
				return catchFaceCount;
			}
			void set(Int32 value)
			{
//				Threading::Monitor::Enter(catchFaceCount);
				catchFaceCount = value;
//				Threading::Monitor::Exit(catchFaceCount);
			}
		};

		property Int32 CollectFaceCount{
			Int32 get()
			{
				return collectFaceCount;
			}
			void set(Int32 value)
			{
//				Threading::Monitor::Enter(collectFaceCount);
				collectFaceCount = value;
//				Threading::Monitor::Exit(collectFaceCount);
			}
		};
		
		
		property Int32 MatchFaceCount{
			Int32 get()
			{
				return matchFaceCount;
			}
			void set(Int32 value)
			{
//				Threading::Monitor::Enter(matchFaceCount);
				matchFaceCount = value;
//				Threading::Monitor::Exit(matchFaceCount);
			}
		};



		/** @brief callback delegte on Hit
		*
		*
		*/
		delegate void HitAlertCallback(array<HitAlert^>^ hitData);
		event HitAlertCallback^ HitAlertReturnEvent;
		

		///** @brief callback delegte on NotHit
		//*
		//*
		//*/
		//delegate void NotHitAlertCallback(NotHitAlert^ notHitData);


		/**
		* usually be used to show imge
		*/
		

		
		/*event NotHitAlertCallback^ NotHitAlertReturnEvent;*/

		/**
		* on frame coming trigered
		*/
		delegate void ImageGrabbedCallback();
		event ImageGrabbedCallback^ ImageGrabbedEvent;


		delegate void LocateFaceCallback(array<Image^>^ existFace);
		event LocateFaceCallback^ LocateFaceReturnEvent;

		delegate void ShowCountCallback(int type,int count);
		event ShowCountCallback^ ShowCountEvent;

		delegate void ShowMsgCallback(String^ msgStr, Exception^ ex);
		event ShowMsgCallback^ ShowMsgEvent;
		
	private:
		/*
		* search user and release event
		*/
		void OnSearch(Object^ stateInfo);
		void Capture::Begin(Object^ deviceId);

		/*FaceDetection *m_ImplFaceDetection;
		FaceAlignment *m_ImplFaceAlignment;
		FaceIdentification *m_ImplFaceIdentification;*/
		FeatureData^ featureData;
		bool isRun = false;
		bool control = false;
		String^ streamAdress = String::Empty;

		float scoreThresh = 0.60f;		
		Int32 topK = 3;		
		Int32 maxPersonNum = 1;
		float qualityThresh = 3.0f;
		float faceRectScale = 1.5f;
		//每组图片中的比值,在SearchBulk中使用，SearchBulk对每张图片返回两个结果，若两个结果的比值小于times则失败
		//
		float times=1.0;

		cv::Mat *catchImage = nullptr;
		ConcurrentQueue<Image^> ^catchFaceImgQueue = gcnew ConcurrentQueue<Image^>();
		
		/*capture interval
		*/
		Int32 interval = 200;
		//System::Threading::Timer ^captureTimer;

		DateTime timeStart;
		DateTime timeEnd;

		Int32 catchFaceCount = 0;
		Int32 collectFaceCount = 0;
		Int32 matchFaceCount = 0;

		Object^ objImgFileIO = gcnew Object();
		
		cv::VideoCapture *cap;
		VlcOpenCV *vp;

		unsigned int VIDEO_WIDTH = 1280;
		unsigned int VIDEO_HEIGHT = 720;


		Mutex ^ capMutex = gcnew Mutex();

		String^ trainingDataDir = "";
		

		Thread^ beginThread = nullptr;
		//开一个线程用来人脸识别
		Thread ^searchThread;

	};
}
