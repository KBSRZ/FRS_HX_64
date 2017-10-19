using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FRSServerHttp.Model;
using FRSServerHttp.Server;
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
                d.Name = "3131";
                d.Remark = "2131";
                response.SetContent(d.ToJson());
                response.Send();
            }
            else if (request.Domain != string.Empty)
            {
                Console.WriteLine("返回所有设备信息");
            }
            
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {

            if (request.Operation == null)//添加一条数据
            {
                Console.WriteLine("添加一个设备");
                Device device = Device.CreateDeviceFromJSON(request.PostParams);
                if (null != device)
                {
                    //添加到数据库
                }
            }
            else
            {
                if (request.Operation == "update")//更新
                {
                    Console.WriteLine("更新一个设备");
                    Device device = Device.CreateDeviceFromJSON(request.PostParams);
                    if (null != device)
                    {
                        //添加到数据库
                    }
                }
                else if (request.Operation == "delete")//删除
                {
                    Console.WriteLine("删除设备");
                    //删除设备
                }
            }
            
        }
    }
}
