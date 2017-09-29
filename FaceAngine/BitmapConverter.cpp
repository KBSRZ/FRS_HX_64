
#include "BitmapConverter.h"
using namespace FRS::Util;

 cv::Mat BitmapConverter::ToMat(Image^ src)
{
	 System::Drawing::Bitmap ^ _image = safe_cast< System::Drawing::Bitmap ^ >(src);
	 return ToMat(_image);
}
 cv::Mat BitmapConverter::ToMat(Bitmap^ src)
	{
		if (src == nullptr)
			throw gcnew ArgumentNullException("src");

		int w = src->Width;
		int h = src->Height;
		int channels;
		switch (src->PixelFormat)
		{
		case PixelFormat::Format24bppRgb:
		case PixelFormat::Format32bppRgb:
			channels = 3; break;
		case PixelFormat::Format32bppArgb:
		case PixelFormat::Format32bppPArgb:
			channels = 4; break;
		case PixelFormat::Format8bppIndexed:
		case PixelFormat::Format1bppIndexed:
			channels = 1; break;
		default:
			throw gcnew NotImplementedException();
		}

		cv::Mat dst(h, w, CV_8UC(channels));
		ToMat(src, &dst);
		return dst;
	}


	/// <summary>
	/// Converts System.Drawing.Bitmap to Mat
	/// </summary>
	/// <param name="src">System.Drawing.Bitmap object to be converted</param>
	/// <param name="dst">A Mat object which is converted from System.Drawing.Bitmap</param>

   void BitmapConverter::ToMat(Bitmap ^src, cv::Mat* dst)
	{
		if (src == nullptr)
			throw gcnew ArgumentNullException("src");
		if (dst == nullptr)
			throw gcnew ArgumentNullException("dst");

		if (dst->depth() != CV_8U)
			throw gcnew NotSupportedException("Mat depth != CV_8U");
		if (dst->dims != 2)
			throw gcnew NotSupportedException("Mat dims != 2");
		if (src->Width != dst->cols || src->Height != dst->rows)
			throw gcnew ArgumentException("src.Size != dst.Size");

		int w = src->Width;
		int h = src->Height;
		Rectangle rect(0, 0, w, h);
		BitmapData ^bd = nullptr;
		try
		{
			bd = src->LockBits(rect, ImageLockMode::ReadOnly, src->PixelFormat);

			Byte* p = (Byte*)bd->Scan0.ToPointer();
			int sstep = bd->Stride;
			int offset = sstep - (w / 8);
			UInt32 dstep = (UInt32)dst->step;
			IntPtr dstData(dst->data);
			IntPtr dstDataEnd(dst->dataend);
			Byte* dstPtr = (Byte*)dstData.ToPointer();

			bool submat = dst->isSubmatrix();
			bool continuous = dst->isContinuous();

			switch (src->PixelFormat)
			{
			case PixelFormat::Format1bppIndexed:
			{
												   if (dst->channels() != 1)
													   throw gcnew ArgumentException("Invalid nChannels");
												   if (submat)
													   throw gcnew NotImplementedException("submatrix not supported");

												   int x = 0;
												   int y;
												   int bytePos;
												   Byte b;
												   int i;
												   for (y = 0; y < h; y++)
												   {
													   // ��ϱؤ�4byte�����Ф��Ϥ����롣
													   // �����Фθ��Х��Ȥ��{�٤Ƥ���
													   for (bytePos = 0; bytePos < sstep; bytePos++)
													   {
														   if (x < w)
														   {
															   // �F�ڤ�λ�äΥХ��Ȥ��餽�줾��Υӥå�8�Ĥ�ȡ�����
															   b = p[bytePos];
															   for (i = 0; i < 8; i++)
															   {
																   if (x >= w)
																   {
																	   break;
																   }
																   // IplImage��8bit/pixel
																   dstPtr[dstep * y + x] = ((b & 0x80) == 0x80) ? (Byte)255 : (Byte)0;
																   b <<= 1;
																   x++;
															   }
														   }
													   }
													   // �Τ��Ф�
													   x = 0;
													   p += sstep;
												   }
			}
				break;

			case PixelFormat::Format8bppIndexed:
			case PixelFormat::Format24bppRgb:
			{
												if (src->PixelFormat == PixelFormat::Format8bppIndexed)
												if (dst->channels() != 1)
													throw gcnew ArgumentException("Invalid nChannels");
												if (src->PixelFormat == PixelFormat::Format24bppRgb)
												if (dst->channels() != 3)
													throw gcnew ArgumentException("Invalid nChannels");

												// ���ƥåפ�ͬ�����B�A�ʤ顢һ�ݤ˥��ԩ`
												if (dstep == sstep && !submat && continuous)
												{
													UInt32 length = (UInt32)(dstDataEnd.ToInt64() - dstData.ToInt64());
													Utility::copymemory(dstData, bd->Scan0, length);
												}
												else
												{
													// ���Ф��Ȥ�dst���ХХ��ȷ����ԩ`
													Byte* sp = (Byte*)bd->Scan0.ToPointer();
													Byte* dp = (Byte*)dst->data;
													for (int y = 0; y < h; y++)
													{
														Utility::copymemory(dp, sp, dstep);
														sp += sstep;
														dp += dstep;
													}
												}
			}
				break;

			case PixelFormat::Format32bppRgb:
			case PixelFormat::Format32bppArgb:
			case PixelFormat::Format32bppPArgb:
			{
												  switch (dst->channels())
												  {
												  case 4:
													  if (!submat && continuous)
													  {
														  //UInt32 length = (UInt32)(dst.DataEnd.ToInt64() - dstData.ToInt64());
														  UInt32 length = (UInt32)(dstDataEnd.ToInt64() - dstData.ToInt64());
														  Utility::copymemory(dstData, bd->Scan0, length);
													  }
													  else
													  {
														  Byte* sp = (Byte*)bd->Scan0.ToPointer();
														  Byte* dp = (Byte*)dst->data;
														  for (int y = 0; y < h; y++)
														  {
															  Utility::copymemory(dp, sp, dstep);
															  sp += sstep;
															  dp += dstep;
														  }
													  }
													  break;
												  case 3:
													  for (int y = 0; y < h; y++)
													  {
														  for (int x = 0; x < w; x++)
														  {
															  dstPtr[y * dstep + x * 3 + 0] = p[y * sstep + x * 4 + 0];
															  dstPtr[y * dstep + x * 3 + 1] = p[y * sstep + x * 4 + 1];
															  dstPtr[y * dstep + x * 3 + 2] = p[y * sstep + x * 4 + 2];
														  }
													  }
													  break;
												  default:
													  throw gcnew ArgumentException("Invalid nChannels");
												  }
			}
				break;
			}
		}
		finally
		{
			if (bd != nullptr)
				src->UnlockBits(bd);
		}
	}

	/// <summary>
	/// Converts Mat to System.Drawing.Bitmap
	/// </summary>
	/// <param name="src">Mat</param>
	/// <returns></returns>
	 Bitmap^ BitmapConverter::ToBitmap(cv::Mat *src)
	{
		if (src == nullptr)
		{
			throw gcnew ArgumentNullException("src");
		}

		PixelFormat pf;
		switch (src->channels())
		{
		case 1:
			pf = PixelFormat::Format8bppIndexed; break;
		case 3:
			pf = PixelFormat::Format24bppRgb; break;
		case 4:
			pf = PixelFormat::Format32bppArgb; break;
		default:
			throw gcnew ArgumentException("Number of channels must be 1, 3 or 4.", "src");
		}
		return ToBitmap(src, pf);

	}

	/// <summary>
	/// Converts Mat to System.Drawing.Bitmap
	/// </summary>
	/// <param name="src">Mat</param>
	/// <param name="pf">Pixel Depth</param>
	/// <returns></returns>

	  Bitmap^ BitmapConverter::ToBitmap(cv::Mat* src, PixelFormat& pf)
	{
		if (src == nullptr)
			throw gcnew ArgumentNullException("src");
		//src->ThrowIfDisposed();

		Bitmap^ bitmap = gcnew Bitmap(src->cols, src->rows, pf);
		ToBitmap(src, bitmap);
		return bitmap;
	}

	/// <summary>
	/// Converts Mat to System.Drawing.Bitmap
	/// </summary>
	/// <param name="src">Mat</param>
	/// <param name="dst">Bitmap</param>
	/// <remarks>Author: shimat, Gummo (ROI support)</remarks>
	 void BitmapConverter::ToBitmap(cv::Mat *src, Bitmap ^dst)
	{
		if (src == nullptr)
			throw gcnew ArgumentNullException("src");
		if (dst == nullptr)
			throw gcnew ArgumentNullException("dst");
		/*if (src.IsDisposed)
		throw gcnew ArgumentException("The image is disposed.", "src");*/
		if (src->depth() != CV_8U)
			throw gcnew ArgumentException("Depth of the image must be CV_8U");
		//if (src.IsSubmatrix())
		//    throw gcnew ArgumentException("Submatrix is not supported");
		//std::cout << src->cols << "," << dst->Width << "," << src->rows << "," << dst->Height << std::endl;
		if (src->cols != dst->Width || src->rows != dst->Height)
			throw gcnew ArgumentException("");

		PixelFormat pf = dst->PixelFormat;

		// 1�ץ�`���äΈ��ϡ�����`�����`��Υѥ�å��������ɤ���
		if (pf == PixelFormat::Format8bppIndexed)
		{
			ColorPalette ^plt = dst->Palette;
			for (int x = 0; x < 256; x++)
			{
				plt->Entries[x] = Color::FromArgb(x, x, x);
			}
			dst->Palette = plt;
		}

		int w = src->cols;
		int h = src->rows;
		Rectangle rect(0, 0, w, h);
		BitmapData^ bd = nullptr;

		IntPtr srcData(src->data);
		IntPtr srcDataEnd(src->dataend);

		bool submat = src->isSubmatrix();
		bool continuous = src->isContinuous();

		try
		{
			bd = dst->LockBits(rect, ImageLockMode::WriteOnly, pf);

			IntPtr srcData(src->data);
			Byte* pSrc = (Byte*)(srcData.ToPointer());
			Byte* pDst = (Byte*)(bd->Scan0.ToPointer());
			int ch = src->channels();
			int sstep = (int)src->step;
			int dstep = ((src->cols * ch) + 3) / 4 * 4; // 4�α����˓B����
			int stride = bd->Stride;

			switch (pf)
			{
			case PixelFormat::Format1bppIndexed:
			{
												   if (submat)
													   throw gcnew NotImplementedException("submatrix not supported");

												   // BitmapData��4byte��������IplImage��1byte��
												   // �����I���Ƥ��椨��				 
												   //int offset = stride - (w / 8);
												   int x = 0;
												   int y;
												   int bytePos;
												   Byte mask;
												   Byte b = 0;
												   int i;
												   for (y = 0; y < h; y++)
												   {
													   for (bytePos = 0; bytePos < stride; bytePos++)
													   {
														   if (x < w)
														   {
															   for (i = 0; i < 8; i++)
															   {
																   mask = (Byte)(0x80 >> i);
																   if (x < w && pSrc[sstep * y + x] == 0)
																	   b &= (Byte)(mask ^ 0xff);
																   else
																	   b |= mask;

																   x++;
															   }
															   pDst[bytePos] = b;
														   }
													   }
													   x = 0;
													   pDst += stride;
												   }
												   break;
			}

			case PixelFormat::Format8bppIndexed:
			case PixelFormat::Format24bppRgb:
			case PixelFormat::Format32bppArgb:
				if (sstep == dstep && !submat && continuous)
				{
					//UInt32 imageSize = (UInt32)(src->DataEnd.ToInt64() - src->data.ToInt64());
					UInt32 imageSize = (UInt32)(srcDataEnd.ToInt64() - srcData.ToInt64());
					Utility::copymemory(pDst, pSrc, imageSize);
				}
				else
				{
					for (int y = 0; y < h; y++)
					{
						long offsetSrc = (y * sstep);
						long offsetDst = (y * dstep);
						// һ�Ф��Ȥ˥��ԩ`
						Utility::copymemory(pDst + offsetDst, pSrc + offsetSrc, w * ch);
					}
				}
				break;

			default:
				throw gcnew NotImplementedException();
			}
		}
		finally
		{
			dst->UnlockBits(bd);
		}
	}