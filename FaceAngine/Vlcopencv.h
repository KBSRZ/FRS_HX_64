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
	//返回 0 如果开始
	//返回-1 表示错误
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
	const char *url;//图像地址
	unsigned int width;//图像宽
	unsigned int height;//图像高

};

#endif // VLCOPENCV_H