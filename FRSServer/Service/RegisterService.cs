using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServer.Service
{
    /// <summary>
    /// 注册
    /// </summary>
    class RegisterService : BaseService
    {
        public RegisterService()
        {
            url = "/register";
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
            Console.WriteLine("RegisterService::OnClose");
        }
        public override int OnMessage(string param)
        {
            Console.WriteLine("RegisterService::OnMessage");
            return ReturnCode.SUCCESS;
        }
    }
}
