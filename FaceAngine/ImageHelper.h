using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;
using namespace System::IO;

using namespace System::Text;

namespace FRS{
	namespace Util{
	
		public ref class ImageHelper
		{
			/// <summary>
			/// Convert Image to Byte[]
			/// </summary>
			/// <param name="image"></param>
			/// <returns></returns>
		public:
			static array<Byte>^ ImageToBytes(Image^ image);
			

			/// <summary>
			/// Convert Byte[] to Image
			/// </summary>
			/// <param name="buffer"></param>
			/// <returns></returns>
			static Image^ BytesToImage(array<Byte>^ buffer);
		};
	}
}
