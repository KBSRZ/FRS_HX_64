using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FRSServerHttp.Model;
using FRSServerHttp.Server;
using Newtonsoft.Json;
using DataAngine_Set.BLL;
namespace FRSServerHttp.Service
{
    class DeviceService : BaseService
    {
        /// <summary>
        /// 数据操作类
        /// </summary>
        device bll = new device();

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
                Log.Debug(string.Format("返回ID:{0}设备信息", request.RestConvention));
                ////根据ID获得 设备
                //Device d = new Device();
                //d.ID = Int32.Parse(request.RestConvention);
                //d.Latitude = 1.0d;
                //d.Latitude = 1.0d;
                //d.Name = "3131";
                //d.Remark = "2131";
                //response.SetContent(d.ToJson());
                //response.Send();
                int id = -1;
                try
                {
                    id = Convert.ToInt32(request.RestConvention);
                }
                catch
                {

                }
                Device da = Device.CreateInstanceFromDataAngineModel(bll.GetModel(id));
                if (null != da)
                {
                    response.SetContent(da.ToJson());
                }


            }
            else if (request.Domain != string.Empty)
            {
                
                Log.Debug(string.Format("返回所有设备信息"));
                List<DataAngine_Set.Model.device> devices = bll.DataTableToList(bll.GetAllList().Tables[0]);
                response.SetContent(JsonConvert.SerializeObject(Device.CreateInstanceFromDataAngineModel(devices.ToArray())));
            }
            response.Send();

        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            bool status = false;
            if (request.Operation == null)//添加一条数据
            {
                Log.Debug("添加一个设备");
                Device device = Device.CreateInstanceFromJSON(request.PostParams);
                if (null != device)
                {
                    //添加到数据库

                    status = bll.Add(device.ToDataAngineModel());
                }
            }
            else
            {
                if (request.Operation == "update")//更新
                {
                    Log.Debug("更新一个设备");
                    Device device = Device.CreateInstanceFromJSON(request.PostParams);
                    if (null != device)
                    {
                        status = bll.Update(device.ToDataAngineModel());
                    }
                }
                else if (request.Operation == "delete")//删除
                {
                    Log.Debug("删除设备");

                    int id = -1;
                    try
                    {
                        id = Convert.ToInt32(request.RestConvention);
                    }
                    catch
                    {

                    }
                    status = bll.Delete(id);
                    //删除设备
                }
            }
            response.SetContent(status.ToString());
            response.Send();

        }
    }
}
