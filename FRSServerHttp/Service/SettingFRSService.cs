using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FRSServerHttp.Model;
//using FRS;
namespace FRSServerHttp.Service
{
    /// <summary>
    /// 设置服务  
    /// 设计思想在  
    /// OnOpen时返回对应的设置，
    /// OnMessage时进行设置
    /// 只改变配置文件，重启生效
    /// </summary>
    /// </summary>
    class SettingFRSService : BaseService
    {
        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string URL
        {
            get
            {
                return "/setting-frs";
            }
        }

        public SettingFRSService()
        {
           

        }

        //protected override int OnRead(string param)
        //{
        //    Console.WriteLine("SettingService::OnRead");
        //    if (null != socket && socket.IsAvailable)
        //    {
        //        Console.WriteLine(settingFRS.ToJson());
        //        socket.Send(new Message(Message.MessageType.READ, settingFRS.ToJson()).ToJson());
        //    }
        //    return ReturnCode.SUCCESS;
        //}

        //protected override int OnUpdate(string param)
        //{
        //    settingFRS = SettingFRS.CreateMessageFromJSON(param);
        //    Console.WriteLine("SettingService::OnUpdate");
        //    if (Data.Setting.SettingFRS.Save(settingFRS) == ReturnCode.SUCCESS)
        //    {
        //        return ReturnSuccessMessage();

        //    }
        //    else
        //    {
        //        return ReturnFailMessage();
        //    }
        //}

      
       
    }

}
