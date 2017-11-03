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
using FRS;
namespace FRSServerHttp.Service
{
   
  
    abstract class BaseService
    {
        protected static Capture cap;
        protected static FeatureData fa;
        protected static bool IsInit = false;

        public BaseService() {
              

        }
       
        protected void  InitFRS()
        {
            
           if (!IsInit)
           {

               fa = new FeatureData();
               var setting = new Model.Setting.SettingFRS();

               fa.MaxPersonNum = setting.MaxPersonNum;
               fa.RegisterFaceHeightThresh = setting.RegisterFaceHeightThresh;
               fa.RegisterFacePitchThresh = setting.RegisterFacePitchThresh;
               fa.RegisterFaceQualityThresh = setting.RegisterFaceQualityThresh;
               fa.RegisterFaceRollThresh = setting.RegisterFaceRollThresh;
               fa.RegisterFaceWidthThresh = setting.RegisterFaceWidthThresh;
               fa.RegisterFaceYawThresh = setting.RegisterFaceYawThresh;
               fa.ScoreThresh = setting.ScoreThresh;
               fa.SearchFaceHeightThresh = setting.SearchFacePitchThresh;
               fa.SearchFacePitchThresh = setting.SearchFacePitchThresh;
               fa.SearchFaceQualityThresh = setting.SearchFaceQualityThresh;
               fa.SearchFaceRollThresh = setting.SearchFaceRollThresh;
               fa.SearchFaceWidthThresh = setting.SearchFaceWidthThresh;
               fa.SearchFaceYawThresh = setting.SearchFaceYawThresh;
               fa.TopK = setting.TopK;
               
               cap = new Capture(fa);
             
               cap.Interval = setting.Interval;


               FRSParam param = new FRSParam();

               param.nMinFaceSize = Math.Min(setting.SearchFaceHeightThresh, setting.MaxPersonNum);

               param.nRollAngle = Math.Min(setting.SearchFaceRollThresh, Math.Min(setting.SearchFaceYawThresh, setting.SearchFacePitchThresh));
               param.bOnlyDetect = true;

               FaceImage.Create(setting.ChannelNum, param);
               Feature.Init(setting.ChannelNum);
               IsInit = true;

           }
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
