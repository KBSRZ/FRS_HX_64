#ifndef VLCOPENCV_H
#define VLCOPENCV_H
#include <opencv2/opencv.hpp>  
#include <vlc/vlc.h>
#include <Windows.h>

#pragma comment(lib,"../VLC/lib/libvlc.lib")
#pragma comment(lib,"../VLC/lib/libvlccore.lib")

class TCallBackParam
{
public:
	TCallBackParam(){}
	HANDLE  mutex;
	uchar *pixels;
	//VlcOpenCV *vlc;
	cv::Mat* mat;
};

class VlcOpenCV 
{	
public:

	VlcOpenCV();
	//VlcOpenCV(const char *url);
	VlcOpenCV(const char *url, unsigned int width,unsigned int height);
	~VlcOpenCV();
	//���� 0 �����ʼ
	//����-1 ��ʾ����
	int Start();

	void Stop();
	cv::Mat frame();
	
private:
	TCallBackParam* param;
	libvlc_instance_t *vlcInstance;
	libvlc_media_player_t *mp = 0;
	libvlc_media_t *media;

	cv::Mat mat;

	HANDLE  mat_mutex;

	bool isStop = false;
	const char *url;//ͼ���ַ
	unsigned int width;//ͼ���
	unsigned int height;//ͼ���

};

#endif // VLCOPENCV_H