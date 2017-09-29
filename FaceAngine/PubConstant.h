using namespace System;
using namespace System::Collections::Generic;
using namespace System::Data;
using namespace System::Drawing;
using namespace System::Text;
namespace FRS {
	public value class ReturnCode
	{
	public:
		//
		static const Int32 SUCCESS = 0;
		//文件未找到
		static const Int32 FILE_NOT_FOUND = -1;
		//图片读入失败
		static const Int32  IMAGE_RAED_FAILED = -2;
		//图片太小
		static const Int32  IMAGE_TOO_SMALL = -3;
		//写入数据库失败
		static const Int32  WRITE_TO_DATABASE_FAILED = -4;
		//图片中无人脸
		static const Int32  NO_FACE = -5;
		//人脸角度不符合
		static const Int32  ILLEGAL_FACE_ANGLE = -6;
		//人脸质量不符合
		static const Int32  ILLEGAL_FACE_QUALITY = -7;
		//人脸大小不符合
		static const Int32  ILLEGAL_FACE_SIZE = -7;
		//未知异常
		static const Int32  UNKOWN_EXCEPTION = -99999;


		//没有空闲通道
		static const Int32 NO_FREE_CHANNEL = -1;

	};
	
}