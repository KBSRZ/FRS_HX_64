﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using FRSServerHttp.Model;
using System.IO;
using FRSServerHttp.Server;
using FRSServerHttp.Server.Websocket;
using DataAngine_Set.BLL;
using FRS;

namespace FRSServerHttp.Service
{
    /// <summary>
    /// 视频流实时命中
    /// OnHit 给放在每次命中的回掉函数中
    /// 保证同一个时间只能有一个长连接
    /// </summary>
    class HitAlertService : BaseService
    {

        surveillancetask taskBll = new surveillancetask();
        device deviceBll = new device();
        dataset datasetBll = new dataset();

        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string Domain
        {
            get
            {
                return "hitalert";
            }
        }



       static Capture cap;
       static FeatureData fa;
       static HttpResponse response = null;
       static bool IsOnSurveillance = false;
       static Object objLock = new Object();
       static bool IsInit = false;

        public HitAlertService()
        {
          
        }
       
        private void  OnHit(FRS.HitAlert []hits){
            if (hits == null || hits.Length==0) return;
            Log.Debug("OnHit");
            try
            {
                
                    //response.SetContent(JsonConvert.SerializeObject(hit));
                    //response.SendOnLongConnetion();
                string msg = JsonConvert.SerializeObject(Model.HitAlert.CreateInstanceFromFRSHitAlert(hits));
                response.SetContent(msg);
                response.SendWebsocketData();
                    
                
            }
            catch (Exception e)//客户端主动关闭
            {
               
                
                //Console.WriteLine(e.Message + e.StackTrace);
                cap.Stop();
               // allDone.Set();//可以继续执行了
                
            }
          
        }
        void StopSurveillance()
        {

            lock (objLock)
            {
                IsOnSurveillance = false;
            }
            Log.Debug(string.Format("布控任务停止"));
            response.TcpClient.Close();
            HitAlertService.response = null;
            cap.Stop();
            System.GC.Collect();
           
        }
        bool  InitFRS(int taskID)
        {



            DataAngine_Set.Model.surveillancetask task=taskBll.GetModel(taskID);
            if (null == task) { Log.Debug("检索任务失败"); return false; }
            DataAngine_Set.Model.device device = deviceBll.GetModel(task.deviceid);
            if (null == device) { Log.Debug("检索设备失败"); return false; }
            DataAngine_Set.Model.dataset dataset = datasetBll.GetModel(task.databaseid);
            if (null == dataset) { Log.Debug("检索库失败"); return false; }

           
            fa = new FeatureData();
           var setting = new Model.Setting.SettingFRS();


           if (!IsInit)
           {

               FRSParam param = new FRSParam();

               param.nMinFaceSize = Math.Min(setting.SearchFaceHeightThresh, setting.MaxPersonNum);

               param.nRollAngle = Math.Min(setting.SearchFaceRollThresh, Math.Min(setting.SearchFaceYawThresh, setting.SearchFacePitchThresh));
               param.bOnlyDetect = true;

               FaceImage.Create(setting.ChannelNum, param);
               Feature.Init(setting.ChannelNum);
               IsInit = true;
           }
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
           fa.LoadData(dataset.datasetname);
           cap = new Capture(fa);
           cap.HitAlertReturnEvent += new Capture.HitAlertCallback(OnHit);
           cap.Interval = setting.Interval;
           int id=-1;
           try
           {
               Log.Debug(device.address);
              id= Convert.ToInt32(device.address);
           }
           catch
           {

           }
           if (id == -1)
           {


               if (cap.Start(device.address) != ReturnCode.SUCCESS)
               {
                   Log.Debug("打开摄像头失败");
                   return false;
               }
           }
           else
           {
               if (cap.Start(id) != ReturnCode.SUCCESS)
               {
                   Log.Debug("打开摄像头失败");
                   return false;
               }
           }
           
           return true;
        }
        public override void OnGet(HttpRequest request, HttpResponse response)
        {      
            if (request.Upgrade == null || request.Upgrade.Trim().ToLower() != "websocket")
            {            
                response.Send();
                return;
            }
                
            lock (objLock)
            {              
                if (IsOnSurveillance)//已经用客户端连接了
                {                 
                    response.SetContent("False");
                    //response.SetContent(JsonConvert.SerializeObject(hit));
                    //response.SendOnLongConnetion();
                    response.SendWebsocketData();

                    response.TcpClient.Close();
                    return;
                }
            }
            lock (objLock)
            {
                IsOnSurveillance = true;
            }

            if (request.RestConvention != null)
            {

               Log.Debug(string.Format("准备开始布控任务 布控ID{0}", request.RestConvention));
                //更具p.restConvention（taskID）进行
                //SurveillanceTask task = ;
            }

          

            HitAlertService.response = response;
            int id = -1;
            try {
               id= Convert.ToInt32(request.RestConvention);
            }
            catch
            {
                return;
            }

            if (!InitFRS(id)) {//开启监控失败
                Log.Debug("开启布控任务失败");
                response.SetContent("False");
                
                response.SendWebsocketData();

                response.TcpClient.Close();
                IsOnSurveillance = false;
                return;
            };

            byte[] buffer = new byte[1024];
            FrameType type = FrameType.Continuation;
            while(type != FrameType.Close )
            {
                int length=0;
               
                if(response.TcpClient.Connected)
                     length = response.TcpClient.Client.Receive(buffer);//等待客户端的数据,主要等待客户端发送关闭数据
                byte[] data = new byte[length];
                Array.Copy(buffer, data, length);
                type = Hybi13Handler.GetFrameType(new List<byte>(data));
                //FRSServerHttp.Server.Websocket.Hybi13Handler.ReceiveData(new List<byte>(data), readState);
            }

            StopSurveillance();

        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response) 
        {
            if (request.RestConvention != null)
            {
                 Console.WriteLine("停止 布控ID{0}", request.RestConvention);
                 
            }
            bool status = true;
            response.SetContent(status.ToString());
            response.Send();

            
        }

        /// <summary>
        /// 命中时发送消息
        /// </summary>
        /// <param name="result"></param>
        //public void OnHit(FRS.HitAlert[] result)
        //{
        //    if (result == null || result.Length == 0) return;
        //    HitAlert[] hitalerts = new Data.HitAlert[result.Length];
        //    for (int i = 0; i < hitalerts.Length; i++)
        //    {
        //        hitalerts[i] = new Data.HitAlert(result[i]);
        //    }
           
        //}
    }
}
