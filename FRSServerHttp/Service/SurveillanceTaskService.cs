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

                //构造响应报文
               
                SurveillanceTask t = new SurveillanceTask();
                t.ID = 1;
                t.Name = "布控1";
                t.DatasetID = 1;
                t.DeviceID = 2;

                response.SetContent(t.ToJson());
                response.Send();

            }
            else if (request.Domain != null)
            {//获得所有布控的任务

                SurveillanceTask[] surveillanceTasks = new SurveillanceTask[2] { new SurveillanceTask(), new SurveillanceTask() };
                SurveillanceTask t = new SurveillanceTask();


                surveillanceTasks[0].DatasetID = 1;
                surveillanceTasks[0].DeviceID = 2;
                surveillanceTasks[1].DeviceID = 2;
                surveillanceTasks[1].DeviceID = 3;
                
                //构造响应报文

                response.SetContent(JsonConvert.SerializeObject(surveillanceTasks));
                response.Send();

            }
           
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {

           if (request.Operation == string.Empty)//添加一条数据
           {
               SurveillanceTask task = SurveillanceTask.CreateSurveillanceTaskFromJson(request.PostParams);
               if (null != task)
               {
                   //添加到数据库
               }
           }
           else
           {
               if (request.Operation == "update")//更新
               {

               }
               else if (request.Operation == "delete")//删除
               {

               }
           }
            
        }
    }
}
