using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using FRSServerHttp.Model;
using System.IO;
//using FRS;
using Newtonsoft.Json;
using FRSServerHttp.Server;
namespace FRSServerHttp.Service
{
   
  
    abstract class BaseService
    {
        //public static FeatureData fa ;
        //public static Capture cap;
        public static Dataset selectedDevice = null;
        public static Model.Setting.SettingFRS settingFRS = null;
       
        public static int initFRS (){

            //settingFRS = new Data.Setting.SettingFRS();

            //selectedDevice = new Data.Device();

            //FRSParam param = new FRSParam();

            //param.nMinFaceSize = Math.Min(settingFRS.SearchFaceHeightThresh, settingFRS.MaxPersonNum);

            //param.nRollAngle = Math.Min(settingFRS.SearchFaceRollThresh, Math.Min(settingFRS.SearchFaceYawThresh, settingFRS.SearchFacePitchThresh));
            //param.bOnlyDetect = true;

            //FaceImage.Create(settingFRS.ChannelNum, param);
            //Feature.Init(settingFRS.ChannelNum);
            //fa = new FeatureData();
            //fa.MaxPersonNum = settingFRS.MaxPersonNum;
            //fa.ScoreThresh = settingFRS.ScoreThresh;
            //fa.SearchFaceHeightThresh = settingFRS.SearchFaceHeightThresh;
            //fa.SearchFaceWidthThresh = settingFRS.SearchFaceWidthThresh;
            //fa.SearchFaceYawThresh = settingFRS.SearchFaceYawThresh;
            //fa.SearchFacePitchThresh = settingFRS.SearchFacePitchThresh;
            //fa.SearchFaceRollThresh = settingFRS.SearchFaceRollThresh;
            //fa.SearchFaceQualityThresh = settingFRS.SearchFaceQualityThresh;
            //fa.TopK = settingFRS.TopK;

            //fa.RegisterFaceHeightThresh = settingFRS.RegisterFaceHeightThresh;
            //fa.RegisterFaceWidthThresh = settingFRS.RegisterFaceWidthThresh;
            //fa.RegisterFaceYawThresh = settingFRS.RegisterFaceYawThresh;
            //fa.RegisterFacePitchThresh = settingFRS.RegisterFacePitchThresh;
            //fa.RegisterFaceRollThresh = settingFRS.RegisterFaceRollThresh;
            //fa.RegisterFaceQualityThresh = settingFRS.RegisterFaceQualityThresh;
            //cap = new Capture(fa);
            //cap.Interval = settingFRS.Interval;

            return 0;

        }
       
        
        public BaseService() {
              

        }
       
        /// <summary>
        /// Post时调用
        /// </summary>
        public virtual void OnGet(HttpRequest request, HttpResponse response)
        {

        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public virtual void OnPost(HttpRequest request, HttpResponse response) { }
        
       
    }
   
   
    
    
    

    
}
