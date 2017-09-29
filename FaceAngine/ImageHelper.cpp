#include "ImageHelper.h"
using namespace System;
using namespace System::Collections::Generic;
using namespace System::Drawing;
using namespace System::Drawing::Imaging;
using namespace System::IO;

using namespace FRS::Util;
array<Byte>^ ImageHelper::ImageToBytes(Image^ image)
{
	{
		ImageFormat^ format = image->RawFormat;

		MemoryStream^ ms = gcnew MemoryStream();

		if (format->Equals(ImageFormat::Jpeg))
		{
			image->Save(ms, ImageFormat::Jpeg);
		}
		else if (format->Equals(ImageFormat::Png))
		{
			image->Save(ms, ImageFormat::Png);
		}
		else if (format->Equals(ImageFormat::Bmp))
		{
			image->Save(ms, ImageFormat::Bmp);
		}
		else if (format->Equals(ImageFormat::Gif))
		{
			image->Save(ms, ImageFormat::Gif);
		}
		else if (format->Equals(ImageFormat::Icon))
		{
			image->Save(ms, ImageFormat::Icon);
		}
		array<Byte>^ buffer = gcnew array<Byte>(ms->Length);
		//Image.Save()会改变MemoryStream的Position，需要重新Seek到Begin
		ms->Seek(0, SeekOrigin::Begin);
		ms->Read(buffer, 0, buffer->Length);
		return buffer;
	}
}


Image^ ImageHelper::BytesToImage(array<Byte>^ buffer)
{
	MemoryStream ^ms = gcnew MemoryStream(buffer);
	Image^ image = System::Drawing::Image::FromStream(ms);
	return image;
}


