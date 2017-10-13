using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using FRSServer.Data;
namespace FRSServer.Service
{
    /// <summary>
    /// 控制服务
    /// OnMessage 进行主要的设置
    /// </summary>
    class ControllerService : BaseService
    {



        public ControllerService()
        {
            // TODO: Complete member initialization
            url = "/controlling";
        }
        public override void OnOpen()
        {

        }
        public override void OnClose()
        {
            if (null != socket)
            {
                socket.Close();
            }
            Console.WriteLine("ControllerService::OnClose");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">例如：代表第一个监控停止 ，第二个监控开始</param>
        /// <returns></returns>
        public override int OnMessage(string param)
        {
            Console.WriteLine("ControllerService::OnMessage");
            Console.WriteLine(param);
            ControlData[] contorls = (ControlData[])JsonConvert.DeserializeObject(param, typeof(ControlData[]));
           ///
           /// 
           ///
            if(contorls[0].Status){
                if (contorls[0].VideoAdress == "") 
                cap.Start(0);
            }
            else
            {
                cap.Stop();
            }
            return ReturnCode.SUCCESS;
        }
    }

    
}
