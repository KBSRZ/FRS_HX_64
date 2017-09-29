using namespace  System;
using namespace System::Collections::Generic;
using namespace System::Net;
using namespace System::Text;
using namespace System::Threading::Tasks;
using  namespace System::IO;
namespace INF {
	public ref class INF2 {
	public:
		 INF2(String ^requestUriString){
			 this->requestUriString = requestUriString;
		}
		 INF2(){}
		property String^ RequestUriString {
			String^ get(){
				return requestUriString;
			}
			void set(String^ value){
				requestUriString = value;
			}
		};
		

	private:
		//String ^ requestUriString = "http ://123.57.21.32:8088/faceV2/inf/getcameras";
		String ^ requestUriString = "http://172.18.128.103:8080/face/inf/getcameras";
	};
}