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
		//�ļ�δ�ҵ�
		static const Int32 FILE_NOT_FOUND = -1;
		//ͼƬ����ʧ��
		static const Int32  IMAGE_RAED_FAILED = -2;
		//ͼƬ̫С
		static const Int32  IMAGE_TOO_SMALL = -3;
		//д�����ݿ�ʧ��
		static const Int32  WRITE_TO_DATABASE_FAILED = -4;
		//ͼƬ��������
		static const Int32  NO_FACE = -5;
		//�����ǶȲ�����
		static const Int32  ILLEGAL_FACE_ANGLE = -6;
		//��������������
		static const Int32  ILLEGAL_FACE_QUALITY = -7;
		//������С������
		static const Int32  ILLEGAL_FACE_SIZE = -7;
		//δ֪�쳣
		static const Int32  UNKOWN_EXCEPTION = -99999;


		//û�п���ͨ��
		static const Int32 NO_FREE_CHANNEL = -1;

	};
	
}