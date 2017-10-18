using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using FRSServerHttp.Model;
using System.IO;
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
        public static string URL
        {
            get
            {
                return "/hitalert";
            }
        }



       static Test.Capture cap;
       static  HttpProcessor httpProcessorGet = null;

        //用于线程同步，初始状态设为非终止状态，使用手动重置方式
        static private EventWaitHandle allDone=null;
        public HitAlertService()
        {
          
        }
        private void  OnHit(Model.HitAlert []hit){
            try
            {
                if (!httpProcessorGet.socket.Connected)
                {
                    allDone.Set();//可以继续执行了
                    cap.Stop();
                }
                if (null != httpProcessorGet)
                {
                    httpProcessorGet.outputStream.Write(JsonConvert.SerializeObject(hit));
                    httpProcessorGet.outputStream.Flush();
                   
                }
            }
            catch (Exception e)//客户端主动关闭
            {
                Console.WriteLine(e.Message + e.StackTrace);
                cap.Stop();
                allDone.Set();//可以继续执行了
                
            }
          
        }
        /// <summary>
        /// 关闭前一个 HttpProcessor
        /// </summary>
        private void ClosePreviousHttpProcessor(){
            if (null != cap && cap.IsRun)
            {
                cap.Stop();
                if (null != httpProcessorGet)
                {
                    if (httpProcessorGet.socket.Connected)
                    {
                        httpProcessorGet.outputStream.Flush();
                        httpProcessorGet.socket.Close();
                    }
                }
                if (null!=allDone)
                    allDone.Set();//可以继续执行了

            }
            
        }
        public override void OnGet(HttpProcessor p) 
        {

            if(p.Params.ContainsKey("SurveillanceTask")){
                SurveillanceTask task = SurveillanceTask.CreateSurveillanceTaskFromJson(p.Params["SurveillanceTask"]);
            }
           
            ClosePreviousHttpProcessor();

            cap = new Test.Capture();
            cap.HitAlertReturnEvent += new Test.Capture.HitAlertCallback(OnHit);
            cap.Start();

            httpProcessorGet = p;
            allDone = new EventWaitHandle(false, EventResetMode.ManualReset);
          
            //阻塞当前进程,以便事件不停的发送信息
            allDone.WaitOne();
            ClosePreviousHttpProcessor();

            httpProcessorGet = null;
            //Console.WriteLine("线程{0}关闭", Thread.CurrentThread.ManagedThreadId.ToString());
        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public override void OnPost(HttpProcessor p, StreamReader inputData) { }

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
