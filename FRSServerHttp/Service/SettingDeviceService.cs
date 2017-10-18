using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRSServerHttp.Model;
namespace FRSServerHttp.Service
{
    class SettingDeviceService:BaseService
    {
        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string URL
        {
            get
            {
                return "/setting-device";
            }
        }


        public SettingDeviceService()
        {
           
        }


        //readonly DataAngine.BLL.hitalert hitbll = new DataAngine.BLL.hitalert();
        //readonly DataAngine.BLL.user user = new DataAngine.BLL.user();
        //readonly DataAngine.BLL.table table = new DataAngine.BLL.table();
        //readonly DataAngine.BLL.device device = new DataAngine.BLL.device();
        
       

        ///// <summary>
        ///// 获得数据库的初始状态
        ///// </summary>
        ///// <param name="param"></param>
        ///// <returns></returns>
        //protected override int OnRead(string param)
        //{
        //    Console.WriteLine("SettingDatasetService::OnRead");
        //    if (null != socket && socket.IsAvailable)
        //    {
        //        Console.WriteLine(datasetData.ToJson());
        //        socket.Send(new Message(Message.MessageType.READ, datasetData.ToJson()).ToJson());
        //    }
        //    return ReturnCode.SUCCESS;
        //}
        // /// <summary>
        // /// 增加一个设备
        // /// </summary>
        // /// <param name="param">数据库的名字</param>
        // /// <returns></returns>
        //protected override int OnAdd(string param)
        //{
        //    Console.WriteLine("SettingVideoAddressService::OnAdd");
        //    DataAngine.Model.table table = new DataAngine.Model.table();
        //    DataAngine.BLL.table tablebll = new DataAngine.BLL.table();
        //    table.name = param;

        //    if (true == tablebll.Add(table))
        //    {
        //        return ReturnSuccessMessage();
        //    }
        //    return ReturnFailMessage();
        //}

        ///// <summary>
        ///// 设置 选择设备
        ///// </summary>
        ///// <param name="param">Device类型Json</param>
        ///// <returns></returns>
        //protected override int OnSet(string param)
        //{
        //    Console.WriteLine("SettingDatasetService::OnSet");

        //    selectedDevice = Device.CreateDeviceFromJSON(param);
        //    DatasetData.SaveSelectedDataSetName(param);
        //    return ReturnSuccessMessage();
           
        //}


    

    }
}
