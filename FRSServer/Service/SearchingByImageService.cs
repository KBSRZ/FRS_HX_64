using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServer.Service
{
    /// <summary>
    /// 以图搜图
    /// </summary>
    class SearchingByImageService : BaseService
    {

        public SearchingByImageService()
        {
            url = "/searching-by-image";
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
            Console.WriteLine("SearchingByImageService::OnClose");
        }
        public override int OnMessage(string param)
        {
            Console.WriteLine("SearchingByImageService::OnMessage");
            return ReturnCode.SUCCESS;
        }
    }
    
}
