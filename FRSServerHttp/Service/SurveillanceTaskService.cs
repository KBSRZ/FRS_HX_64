using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FRSServerHttp.Model;
using Newtonsoft.Json;
using FRSServerHttp.Server;
namespace FRSServerHttp.Service
{
    class SurveillanceTaskService:BaseService
    {
        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string Domain
        {
            get
            {
                return "surveillance-task-service";
            }
        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public override void OnGet(HttpProcessor p)
        {


            if (p.restConvention != string.Empty)//根据ID获布控的任务
            {
                


            }
            else if(p.domain!= string.Empty){//获得所有布控的任务

                SurveillanceTask[] surveillanceTasks = new SurveillanceTask[2] { new SurveillanceTask(), new SurveillanceTask() };
                SurveillanceTask t = new SurveillanceTask();
                Device d1 = new Device();

                d1.Address = "rtsp://192.168.1.109:554";
                d1.Name = "d1";
                Dataset da1 = new Dataset();
                da1.DatasetName = "da1";


                Device d2 = new Device();
                d2.Name = "d2";
                d2.Address = "rtsp://192.168.1.109:554";
                Dataset da2 = new Dataset();
                da2.DatasetName = "da1";

                surveillanceTasks[0].DatasetID = 1;
                surveillanceTasks[0].DeviceID = 2;
                surveillanceTasks[1].DeviceID = 2;
                surveillanceTasks[1].DeviceID = 3;
                p.outputStream.Write(JsonConvert.SerializeObject(surveillanceTasks));

            }
           
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpProcessor p, StreamReader inputData) {

           if (p.operation == string.Empty)//添加一条数据
           {
               SurveillanceTask task = SurveillanceTask.CreateSurveillanceTaskFromJson(inputData.ReadToEnd());
               if (null != task)
               {
                   //添加到数据库
               }
           }
           else
           {
               if (p.operation == "update")//更新
               {

               }
               else if (p.operation == "delete")//删除
               {

               }
           }
            
        }
    }
}
