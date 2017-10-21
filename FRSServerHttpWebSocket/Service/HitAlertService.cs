using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using FRSServerHttp.Model;
using System.IO;
using FRSServerHttp.Server;
using FRSServerHttp.Server.Websocket;
namespace FRSServerHttp.Service
{
    /// <summary>
    /// 视频流实时命中
    /// OnHit 给放在每次命中的回掉函数中
    /// 保证同一个时间只能有一个长连接
    /// </summary>
    class HitAlertService : BaseService
    {
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



       static Test.Capture cap;
       static HttpResponse response = null;
       static bool IsOnSurveillance = false;
       static Object objLock = new Object();

        public HitAlertService()
        {
          
        }
       
        private void  OnHit(Model.HitAlert []hit){
            try
            {
                if (!response.WriterStream.CanWrite)
                {
                    
                    cap.Stop();
                }
                else
                {
                    response.SetContent("11111");
                    //response.SetContent(JsonConvert.SerializeObject(hit));
                    //response.SendOnLongConnetion();
                    string msg = JsonConvert.SerializeObject(hit);
                    response.SendWebsocketData();
                    
                }
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
            Log.Debug(string.Format("布控任务"));
            response.TcpClient.Close();
            HitAlertService.response = null;
            cap.Stop();
           
           
        }
        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            if (request.Upgrade == null || request.Upgrade.Trim() != "websocket")
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

               Log.Debug(string.Format("开始布控任务 布控ID{0}", request.RestConvention));
                //更具p.restConvention（taskID）进行
                //SurveillanceTask task = ;
            }

            
            cap = new Test.Capture();
            cap.HitAlertReturnEvent += new Test.Capture.HitAlertCallback(OnHit);
            cap.Start();

            HitAlertService.response = response;


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
