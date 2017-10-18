using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FRSServerHttp.Model;
using Newtonsoft.Json;
namespace FRSServerHttp.Service
{
    class SurveillanceTaskService:BaseService
    {
        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string URL
        {
            get
            {
                return "/surveillance-task-service";
            }
        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public override void OnGet(HttpProcessor p)
        {
            SurveillanceTask[] surveillanceTasks = new SurveillanceTask[2] { new SurveillanceTask(), new SurveillanceTask() };
            SurveillanceTask t=new SurveillanceTask() ;
            Device d1 = new Device();
            d1.DeviceName = "d1";
            d1.VideoAddress="rtsp://192.168.1.109:554";
           
            Dataset da1 = new Dataset();
            da1.DatasetName = "da1";
            

            Device d2 = new Device();
            d2.DeviceName = "d2";
            d2.VideoAddress = "rtsp://192.168.1.109:554";
            Dataset da2 = new Dataset();
            da2.DatasetName = "da1";

            surveillanceTasks[0].Dataset = da1;
            surveillanceTasks[0].Device = d1;
            surveillanceTasks[1].Dataset = da2;
            surveillanceTasks[1].Device = d2;
            p.outputStream.Write(JsonConvert.SerializeObject(surveillanceTasks));

           
        }
        /// <summary>
        /// Set时调用
        /// </summary>
        public override void OnPost(HttpProcessor p, StreamReader inputData) {

           SurveillanceTask task= SurveillanceTask.CreateSurveillanceTaskFromJson(inputData.ReadToEnd());
            //做添加添加一条信息的操作
            
        }
    }
}
