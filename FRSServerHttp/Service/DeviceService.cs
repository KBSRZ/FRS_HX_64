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
        public override void OnGet(HttpProcessor p)
        {
            if (p.restConvention != string.Empty)
            {
                Console.WriteLine("返回ID:{0}设备信息", p.restConvention);
                //根据ID获得 设备
                Device d = new Device();
                d.ID = Int32.Parse(p.restConvention);
                d.Latitude = 1.0d;
                d.Latitude = 1.0d;
                d.Name = "3131";
                d.Remark = "2131";
                p.outputStream.Write(d.ToJson());
            }
            else if (p.domain != string.Empty)
            {
                Console.WriteLine("返回所有设备信息");
            }
            
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpProcessor p, StreamReader inputData) {

            if (p.operation == string.Empty)//添加一条数据
            {
                Console.WriteLine("添加一个设备");
                Device device = Device.CreateDeviceFromJSON(inputData.ReadToEnd());
                if (null != device)
                {
                    //添加到数据库
                }
            }
            else
            {
                if (p.operation == "update")//更新
                {
                    Console.WriteLine("更新一个设备");
                    Device device = Device.CreateDeviceFromJSON(inputData.ReadToEnd());
                    if (null != device)
                    {
                        //添加到数据库
                    }
                }
                else if (p.operation == "delete")//删除
                {
                    Console.WriteLine("删除设备");
                    //删除设备
                }
            }
           

        }
    }
}
