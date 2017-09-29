#pragma once

using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;
#include <opencv2/core/version.hpp>
#include <opencv2/calib3d/calib3d.hpp>
#include <opencv2/opencv.hpp>
#include "Utility.h"
namespace FRS {
	namespace Util{

		public ref class BitmapConverter
		{
			
		public:
			/// <summary>
			/// Converts System.Drawing.Bitmap to Mat
			/// </summary>
			/// <param name="src">System.Drawing.Bitmap object to be converted</param>
			/// <returns>A Mat object which is converted from System.Drawing.Bitmap</returns>
			static cv::Mat ToMat(Bitmap^ src);
			static cv::Mat ToMat(Image^ src);

			/// <summary>
			/// Converts System.Drawing.Bitmap to Mat
			/// </summary>
			/// <param name="src">System.Drawing.Bitmap object to be converted</param>
			/// <param name="dst">A Mat object which is converted from System.Drawing.Bitmap</param>

			static  void ToMat(Bitmap ^src, cv::Mat* dst);

			/// <summary>
			/// Converts Mat to System.Drawing.Bitmap
			/// </summary>
			/// <param name="src">Mat</param>
			/// <returns></returns>
			static Bitmap^ ToBitmap(cv::Mat *src);

			/// <summary>
			/// Converts Mat to System.Drawing.Bitmap
			/// </summary>
			/// <param name="src">Mat</param>
			/// <param name="pf">Pixel Depth</param>
			/// <returns></returns>

			static Bitmap^ ToBitmap(cv::Mat* src, PixelFormat& pf);

			/// <summary>
			/// Converts Mat to System.Drawing.Bitmap
			/// </summary>
			/// <param name="src">Mat</param>
			/// <param name="dst">Mat</param>
			/// <remarks>Author: shimat, Gummo (ROI support)</remarks>
			static  void ToBitmap(cv::Mat *src, Bitmap ^dst);
		};
	}
}