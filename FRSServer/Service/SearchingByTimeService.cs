using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServer.Service
{
    /// <summary>
    /// 按照时间搜索
    /// </summary>
    class SearchingByTimeService : BaseService
    {
        public SearchingByTimeService()
        {
            url = "/searching-by-time";
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
            Console.WriteLine("SearchingByTimeService::OnClose");
        }
        public override int OnMessage(string param)
        {
            Console.WriteLine("SearchingByTimeService::OnMessage");
            return ReturnCode.SUCCESS;
        }
    }
}
