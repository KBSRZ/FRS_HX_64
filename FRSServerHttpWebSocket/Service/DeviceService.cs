using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FRSServerHttp.Model;
using FRSServerHttp.Server;
using Newtonsoft.Json;

namespace FRSServerHttp.Service
{
    class DeviceService:BaseService
    {

        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string Domain
        {
            get
            {
                return "device";
            }
        }
        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            if (request.RestConvention != null)
            {
                Console.WriteLine("返回ID:{0}设备信息", request.RestConvention);
                //根据ID获得 设备
                Device d = new Device();
                d.ID = Int32.Parse(request.RestConvention);
                d.Latitude = 1.0d;
                d.Latitude = 1.0d;
                d.Address = "rtsp://192.168.1.64:556";
                d.Name = "摄像头1";
                d.DepartmentID = "1";
                d.LocationType = 1;
                response.SetContent(d.ToJson());
                
                


            }
            else if (request.Domain != string.Empty)
            {
                Console.WriteLine("返回所有设备信息");
                Device []ds = new Device[2]{new Device(),new Device ()};
                ds[0].ID = 1;
                ds[0].Latitude = 1.0d;
                ds[0].Longitude = 1.0d;
                ds[0].Name = "摄像头1";
                ds[0].DepartmentID = "1";
                ds[0].LocationType = 1;

                ds[1].ID = 2;
                ds[1].Latitude = 1.0d;
                ds[1].Longitude = 1.0d;
                ds[1].Name = "摄像头2";
                ds[1].DepartmentID = "1";
                ds[1].LocationType = 2;
                response.SetContent(JsonConvert.SerializeObject(ds));
               
            }
            response.Send();
            
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            bool status = true;
            if (request.Operation == null)//添加一条数据
            {
                Console.WriteLine("添加一个设备");
                Device device = Device.CreateInstanceFromJSON(request.PostParams);
                if (null != device)
                {
                    //添加到数据库
                    Console.WriteLine(request.PostParams);
                 
                }
            }
            else
            {
                if (request.Operation == "update")//更新
                {
                    Console.WriteLine("更新一个设备");
                    Device device = Device.CreateInstanceFromJSON(request.PostParams);
                    if (null != device)
                    {
                        Console.WriteLine(request.PostParams);
                    }
                }
                else if (request.Operation == "delete")//删除
                {
                    Console.WriteLine("删除设备ID:{0}",request.RestConvention);
                  
                }
            }
            response.SetContent(status.ToString());
            response.Send();
            
        }
    }
}
