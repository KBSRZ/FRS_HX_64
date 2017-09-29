#include "vlcopencv.h"

void *lock(void *op, void **plane)
{
	TCallBackParam *p = (TCallBackParam *)op;
	WaitForSingleObject(p->mutex, INFINITE);
	*plane = p->pixels;
	return NULL;
}

void unlock(void *op, void *pic, void * const *plane)
{
	TCallBackParam *p = (TCallBackParam *)op;
	ReleaseMutex(p->mutex);
}

cv::Mat VlcOpenCV::frame(){
	cv::Mat temp;
	WaitForSingleObject(mat_mutex, INFINITE);
	param->mat->copyTo(temp);
	ReleaseMutex(mat_mutex);
	return temp;

}


VlcOpenCV::VlcOpenCV()
{	
}

VlcOpenCV::VlcOpenCV(const char *url, unsigned int width, unsigned int height)
{
	this->url = url;
	param = new TCallBackParam;
	vlcInstance = libvlc_new(0, NULL);
	media = libvlc_media_new_location(vlcInstance, url);

	if (media == NULL)
	{
		return;
	}

	mp = libvlc_media_player_new_from_media(media);
	libvlc_media_release(media);
	param->mat = new cv::Mat(height, width, CV_8UC3);
	param->pixels = (unsigned char *)param->mat->data;
	mat = *param->mat;
	
	libvlc_video_set_callbacks(mp, &lock, &unlock, NULL, param);

	const char *chroma = "RV24";
	unsigned pitch = width * 24 / 8;

	libvlc_video_set_format(mp, chroma, width, height, pitch);
}

VlcOpenCV::~VlcOpenCV()
{
	if (mp)
		libvlc_media_player_release(mp);
	libvlc_release(vlcInstance);
	delete param;
}

int VlcOpenCV::Start()
{
	bool isStop = true;
	return libvlc_media_player_play(mp);	
}

void VlcOpenCV::Stop()
{
	libvlc_media_player_stop(mp);
	isStop = true;	
}
