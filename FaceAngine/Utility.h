#pragma once
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;
using namespace System::Text;
using namespace System::Runtime::InteropServices;

namespace FRS{
	namespace Util{
		public ref class Utility
		{

			/// <summary>
			/// 
			/// </summary>
			/// <param name="outDest"></param>
			/// <param name="inSrc"></param>
			/// <param name="inNumOfBytes"></param>
		public:
			static  void copymemory(void* outDest, void* inSrc, UInt32 inNumOfBytes);

			static  void copymemory(void* outDest, void* inSrc, int inNumOfBytes);
			static  void copymemory(IntPtr outDest, IntPtr inSrc, UInt32 inNumOfBytes);

			static  void copymemory(IntPtr outDest, IntPtr inSrc, int inNumOfBytes);

			//[DllImport("kernel32")]
			//public static unsafe extern void CopyMemory(void* outDest, void* inSrc, [MarshalAs(UnmanagedType.U4)] int inNumOfBytes);
			//[DllImport("kernel32")]
			//public static extern void CopyMemory(IntPtr outDest, IntPtr inSrc, [MarshalAs(UnmanagedType.U4)] int inNumOfBytes);

			/// <summary>
			/// 
			/// </summary>
			/// <param name="outDest"></param>
			/// <param name="inNumOfBytes"></param>
			static  void zeromemory(void* outDest, UInt32 inNumOfBytes);

			static  void zeromemory(void* outDest, int inNumOfBytes);
			static  void zeromemory(IntPtr outDest, UInt32 inNumOfBytes);

			static  void zeromemory(IntPtr outDest, int inNumOfBytes);


			/// <summary>
			/// 
			/// </summary>
			/// <param name="t"></param>
			/// <returns></returns>

			static int SizeOf(Type^ t);
		};



	}
}