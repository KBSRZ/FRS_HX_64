using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServer.Service
{
    class SettingDeviceService:BaseService
    {
        public SettingDeviceService()
        {
            url="setting-device";
        }
        public override void OnOpen()
        {
            Console.WriteLine("SettingDeviceService::OnOpen");
        }
        public override void OnClose()
        {
            if (null != socket)
            {
                socket.Close();
            }
            Console.WriteLine("SettingDeviceService::OnClose");
        }
        public override int OnMessage(string param)
        {
            Console.WriteLine("SettingVideoAddressService::OnMessage");
            Console.WriteLine(param);
            if (null != socket && socket.IsAvailable)
            {
                socket.Send("0.6");
            }

            return ReturnCode.SUCCESS;
        }
    }
}
