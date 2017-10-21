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
                return "surveillance-task";
            }
        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public override void OnGet(HttpRequest request, HttpResponse response)
        {


            if (request.RestConvention != null)//根据ID获布控的任务
            {
                Log.Debug("获取一条布控任务");
                //构造响应报文

                SurveillanceTask t = new SurveillanceTask();

                int id = -1;
                try
                {
                    id = Convert.ToInt32(request.RestConvention);
                }
                catch
                {

                }
                t.ID = id;
                t.Name = "布控" + id;
                t.DatasetID = id;
                t.DeviceID = id;



                response.SetContent(t.ToJson());

                response.Send();

            }
            else if (request.Domain != null)
            {//获得所有布控的任务

                SurveillanceTask[] surveillanceTasks = new SurveillanceTask[2] { new SurveillanceTask(), new SurveillanceTask() };
                SurveillanceTask t = new SurveillanceTask();


                surveillanceTasks[0].DatasetID = 1;
                surveillanceTasks[0].DeviceID = 1;
                surveillanceTasks[0].Name = "任务1";
                surveillanceTasks[1].DeviceID = 2;
                surveillanceTasks[1].DeviceID = 2;
                surveillanceTasks[1].Name = "任务2";


                Log.Debug("获得所有布控的任务");

                response.SetContent(JsonConvert.SerializeObject(surveillanceTasks));

                //构造响应报文
                response.Send();

            }
           
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            bool status = true;
           if (request.Operation == null)//添加一条数据
           {
               Log.Debug("添加一条布控任务");
               SurveillanceTask task = SurveillanceTask.CreateInstanceFromJson(request.PostParams);
               if (null != task)
               {
                   //添加到数据库
                   Console.WriteLine(request.PostParams);
               }
           }
           else
           {
              
               if (request.Operation == "update")//更新
               {
                   Log.Debug("更新一条布控任务");
                   SurveillanceTask task = SurveillanceTask.CreateInstanceFromJson(request.PostParams);
                   Log.Debug(request.PostParams);
               }
               else if (request.Operation == "delete")//删除
               {
                   Log.Debug(string.Format("删除一条布控任务{0}", request.RestConvention));
                   int id = -1;
                   try
                   {
                       id = Convert.ToInt32(request.RestConvention);
                   }
                   catch
                   {
                   }
                  
               }
           }
           response.SetContent(status.ToString());
           response.Send();
        }
    }
}
