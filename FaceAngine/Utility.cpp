#include "Utility.h"
using namespace  FRS::Util;
void Utility::copymemory(void* outDest, void* inSrc, UInt32 inNumOfBytes)
{
	// ܞ���Ȥ�UInt32����align����
	const UInt32 align = sizeof(UInt32)-1;
	UInt32 offset = (UInt32)outDest & align;
	// ���ݥ��󥿤�32bit�Ȥ��ޤ�ʤ��ΤǱ������Υ��㥹�Ȥ�UInt32�Ǥϥ��������
	// �����λ2bit��������Ф����ΤǤ����OK��
	if (offset != 0)
		offset = align - offset;
	offset = Math::Min(offset, inNumOfBytes);

	// ���^����겿�֤�Byte�Ǥ��ޤ��ޥ��ԩ`
	Byte* srcBytes = (Byte*)inSrc;
	Byte* dstBytes = (Byte*)outDest;
	for (UInt32 i = 0; i < offset; i++)
		dstBytes[i] = srcBytes[i];

	// UInt32��һ�ݤ�ܞ��
	UInt32* dst = (UInt32*)((Byte*)outDest + offset);
	UInt32* src = (UInt32*)((Byte*)inSrc + offset);
	UInt32 numOfUInt = (inNumOfBytes - offset) / sizeof(UInt32);
	for (UInt32 i = 0; i < numOfUInt; i++)
		dst[i] = src[i];

	// ĩβ����겿�֤�Byte�Ǥ��ޤ��ޥ��ԩ`
	for (UInt32 i = offset + numOfUInt * sizeof(UInt32); i < inNumOfBytes; i++)
		dstBytes[i] = srcBytes[i];
}
void Utility::copymemory(void* outDest, void* inSrc, int inNumOfBytes)
{
	copymemory(outDest, inSrc, (UInt32)inNumOfBytes);
}
void Utility::copymemory(IntPtr outDest, IntPtr inSrc, UInt32 inNumOfBytes)
{
	copymemory(outDest.ToPointer(), inSrc.ToPointer(), inNumOfBytes);
}
void Utility::copymemory(IntPtr outDest, IntPtr inSrc, int inNumOfBytes)
{
	copymemory(outDest.ToPointer(), inSrc.ToPointer(), (UInt32)inNumOfBytes);
}
//[DllImport("kernel32")]
//public static unsafe extern void CopyMemory(void* outDest, void* inSrc, [MarshalAs(UnmanagedType.U4)] int inNumOfBytes);
//[DllImport("kernel32")]
//public static extern void CopyMemory(IntPtr outDest, IntPtr inSrc, [MarshalAs(UnmanagedType.U4)] int inNumOfBytes);

/// <summary>
/// 
/// </summary>
/// <param name="outDest"></param>
/// <param name="inNumOfBytes"></param>
void Utility::zeromemory(void* outDest, UInt32 inNumOfBytes)
{
	// ܞ���Ȥ�UInt32����align����
	const UInt32 align = sizeof(UInt32)-1;
	UInt32 offset = (UInt32)outDest & align;
	// ���ݥ��󥿤�32bit�Ȥ��ޤ�ʤ��ΤǱ������Υ��㥹�Ȥ�UInt32�Ǥϥ��������
	// �����λ2bit��������Ф����ΤǤ����OK��
	if (offset != 0)
		offset = align - offset;
	offset = Math::Min(offset, inNumOfBytes);

	// ���^����겿�֤�Byte�Ǥ��ޤ��ޥ��ԩ`
	Byte* dstBytes = (Byte*)outDest;
	for (UInt32 i = 0; i < offset; i++)
		dstBytes[i] = 0;

	// UInt32��һ�ݤ�ܞ��
	UInt32* dst = (UInt32*)((Byte*)outDest + offset);
	UInt32 numOfUInt = (inNumOfBytes - offset) / sizeof(UInt32);
	for (UInt32 i = 0; i < numOfUInt; i++)
		dst[i] = 0;

	// ĩβ����겿�֤�Byte�Ǥ��ޤ��ޥ��ԩ`
	for (UInt32 i = offset + numOfUInt * sizeof(UInt32); i < inNumOfBytes; i++)
		dstBytes[i] = 0;
}
void Utility::zeromemory(void* outDest, int inNumOfBytes)
{
	zeromemory(outDest, (UInt32)inNumOfBytes);
}
void Utility::zeromemory(IntPtr outDest, UInt32 inNumOfBytes)
{
	zeromemory(outDest.ToPointer(), inNumOfBytes);
}
void Utility::zeromemory(IntPtr outDest, int inNumOfBytes)
{
	zeromemory(outDest.ToPointer(), (UInt32)inNumOfBytes);
}

/// <summary>
/// 
/// </summary>
/// <param name="t"></param>
/// <returns></returns>
  int Utility::SizeOf(Type^ t)
  {
	  if (t->IsValueType)
	  {
		  return Marshal::SizeOf(t);
	  }
	  else
	  {
		  /*
		  FieldInfo info = t.GetField("SizeOf", BindingFlags.Static | BindingFlags.Public);
		  if (info != null)
		  {
		  return (int)info.GetValue(null);
		  }
		  else
		  {
		  throw new OpenCvSharpException("Not defined sizeof({0}) operation", t.Name);
		  }
		  */
		  return IntPtr::Size;
	  }
  }

		