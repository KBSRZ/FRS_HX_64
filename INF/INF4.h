using namespace  System;
using namespace System::Collections::Generic;
using namespace System::Net;
using namespace System::Text;
using namespace System::Threading::Tasks;
using  namespace System::IO;

namespace INF {
	 
	public ref class INF4 {
	public:
		INF4(String ^ ip, Int32 port ){
			this->ip = ip;
			this->port = port;
		}
		INF4(){}
		 enum class SaveImageType
		{
			SaveImageType_Normal_Face = 1,

			SaveImageType_Alarm_Face = 2,

			SaveImageType_Alarm_CapturedImage = 3,

			SaveImageType_Alarm_CapturedVideo = 4

		};
		property String^ IP {
			String^ get(){
				return ip;
			}
			void set(String^ value){
				ip = value;
			}
		};
		property Int32 Port {
			Int32 get(){
				return port;
			}
			void set(Int32 value){
				port = value;
			}
		};
		
		
		
		void Send(Object ^o);
		int SendAsync(String ^defenseCode, String^ cameraCode, INF4::SaveImageType type, String ^fileName, array<Byte> ^data);
		

	private:
		 ref class SendParam
		{
		public:
			String ^defenseCode;
			String^ cameraCode; 
			INF4::SaveImageType type;
			String ^fileName;
			array<Byte> ^data;
			SendParam(String ^defenseCode, String^ cameraCode, INF4::SaveImageType type, String ^fileName, array<Byte> ^data)
			{
				this->defenseCode = defenseCode;
				this->cameraCode = cameraCode;
				this->type = type;
				this->fileName = fileName;
				this->data = data;
			}
		};
		//String ^ ip = "47.93.118.203";
		String ^ ip = "172.18.128.103";
		Int32 port = 9000;
	};
}