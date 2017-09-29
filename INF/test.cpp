#include "INF4.h"
#include "INF5.h"
using namespace INF;
int main()
{
	INF4^ i4 = gcnew INF4();
	array<Byte>^ data = gcnew array<Byte>(300 * 400);
	/*for (int i = 0; i < 1; i++){
		int ret = i4->SendAsync("103", "1007", INF4::SaveImageType::SaveImageType_Normal_Face, "1007_2017-03-09 18-45-30_0_face.jpg", data);

		Console::WriteLine(ret);
	}*/
	INF5^ i5 = gcnew INF5();
	//capture_time = 2017 - 01 - 01 00:00 : 00 & capture_image = 1007_2017 - 03 - 09 18 - 45 - 30_0_face.jpg&capture_video = &face_image = &people_id = 00001 & image_id = 00001 & match_degree = 0.8561
	i5->ReportAlarmAsync("2017-01-01 00:00:00", "1007_2017-03-09 18-45-30_0_face.jpg", "", "", "0001", "0001", "0.8461");
	
	Console::ReadLine();
	return 0;
}