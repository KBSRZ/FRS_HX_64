using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServer.Service
{
    class SettingDatasetService:BaseService
    {
        public SettingDatasetService()
        {
            url = "setting-dataset";
        }

        public override void OnOpen()
        {
            Console.WriteLine("SettingVideoAddressService::OnOpen");

        }
        public override void OnClose()
        {
            if (null != socket)
            {
                socket.Close();
            }
            Console.WriteLine("SettingVideoAddressService::OnClose");
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
