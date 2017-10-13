using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServer.Service
{
    /// <summary>
    /// 比对两张图片
    /// </summary>
    class VerifyingService : BaseService
    {
        public VerifyingService()
        {
            url = "/verifying";
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
            Console.WriteLine("VerifyingService::OnClose");
        }
        public override int OnMessage(string param)
        {
            Console.WriteLine("VerifyingService::OnMessage");
            Console.WriteLine(param);
            if (null != socket && socket.IsAvailable)
            {
                socket.Send("0.6");
            }

            return ReturnCode.SUCCESS;
        }
    }
}
