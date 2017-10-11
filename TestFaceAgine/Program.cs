using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using FRS;
namespace TestFaceAgine
{
    class Program
    {
        static void TestDetectFace()
        {
            FRSParam param = new FRSParam();

            param.nMinFaceSize = 50;
            param.nRollAngle = 60;
            param.bOnlyDetect = true;

            FaceImage.Create(1, param);
            Feature.Init(1);
            Image img=Image.FromFile("E:/照片/3.jpg");
            FRSFacePos []faces=new FRSFacePos[1];
            FaceImage.DetectFace(0, img, 24, faces, 1);
            Console.WriteLine(faces[0].rcFace.left);
        }
        static void Main(string[] args)
        {
            TestDetectFace();
            Console.Read();
        }
    }
}
