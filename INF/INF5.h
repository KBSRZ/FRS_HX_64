using namespace  System;
using namespace System::Collections::Generic;
using namespace System::Net;
using namespace System::Text;
using namespace System::Threading::Tasks;
using  namespace System::IO;
using namespace System::Threading;

namespace INF {
	public ref class INF5 {
	public:
		INF5(){}
		INF5(String ^ requestUriString,
			String^ defenseCode,
			String^ cameraCode,
			String^ companyCode)
		{
			this->requestUriString = requestUriString;
			this->defenseCode = defenseCode;
			this->cameraCode = cameraCode;
			this->companyCode = companyCode;
		}
		property String^ RequestUriString{
			String^ get()
			{
				return requestUriString;
			}
			void set(String^ value)
			{
				requestUriString = value;
			}
		};

		property String^ DefenseCode {
			String^ get()
			{
				return defenseCode;
			}
			void set(String^ value)
			{
				defenseCode = value;
			}

		};
		property String^ CameraCode  {
			String^ get()
			{
				return cameraCode;
			}
			void set(String^ value)
			{
				cameraCode = value;
			}

		};
		property String^ CompanyCode  {
			String^ get()
			{
				return companyCode;
			}
			void set(String^ value)
			{
				companyCode = value;
			}

		};
		//capture_time = 2017 - 01 - 01 00:00 : 00 & capture_image = 1007_2017 - 03 - 09 18 - 45 - 30_0_face.jpg&capture_video = &face_image = &people_id = 00001 & image_id = 00001 & match_degree = 0.8561
		Int32 ReportAlarmAsync(String ^captureTime, String ^captureImage, String ^captureVideo, String ^faceImage, String^ peopleId, String ^imageId, String ^matchDegree){
			
			try{
				Thread ^thread = gcnew Thread(gcnew ParameterizedThreadStart(this, &INF5::ReportAlarm));
				String^ postData = String::Format("defense_code={0}&camera_code={1}&company_code={2}&capture_time={3}&capture_image={4}&capture_video={5}&face_image={6}&people_id={7}&image_id={8}&match_degree={9}",
					defenseCode, cameraCode, companyCode, captureTime, captureImage, captureVideo, faceImage, peopleId, imageId, matchDegree);
				thread->Start(postData);
			}
			catch(Exception ^e){
				return -1;
			}
			return 0;
		}
		//capture_time = 2017 - 01 - 01 00:00 : 00 & capture_image = 1007_2017 - 03 - 09 18 - 45 - 30_0_face.jpg&capture_video = &face_image = &people_id = 00001 & image_id = 00001 & match_degree = 0.8561
		//void ReportAlarm(String ^captureTime, String ^captureImage, String ^captureVideo, String ^faceImage, String^ peopleId, String ^imageId, String ^matchDegree)
		void ReportAlarm(Object^ o)
		{
			try{
			

				array<Byte>^ byteArray = Encoding::UTF8->GetBytes(((String^)o)->ToCharArray());

				HttpWebRequest^ objWebRequest = dynamic_cast<HttpWebRequest^>(WebRequest::Create(requestUriString));
				objWebRequest->Method = "POST";
				objWebRequest->ContentType = "application/x-www-form-urlencoded";
				objWebRequest->ContentLength = byteArray->Length;
				Stream ^newStream = objWebRequest->GetRequestStream();
				// Send the data. 
				newStream->Write(byteArray, 0, byteArray->Length); //写入参数 
				newStream->Close();

				HttpWebResponse^ response = dynamic_cast<HttpWebResponse^>(objWebRequest->GetResponse());
				StreamReader^ sr = gcnew StreamReader(response->GetResponseStream(), Encoding::UTF8);
				String^ textResponse = sr->ReadToEnd(); // 返回的数据
				Console::WriteLine(textResponse);
			}
			catch (Exception ^e)
			{
				
			}

			
		}

	private:

		//String ^ requestUriString = "http://123.57.21.32:8088/faceV2/inf/reportalarm";
		String ^ requestUriString = "http://172.18.128.103:8080/face/inf/reportalarm";
		String^ defenseCode = "102";
		String^ cameraCode = "1007";
		String^ companyCode = "NJUST";
	};
}