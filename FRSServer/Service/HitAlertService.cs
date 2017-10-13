using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace FRSServer.Service
{
    /// <summary>
    /// 视频流实时命中
    /// OnHit 给放在每次命中的回掉函数中
    /// </summary>
    class HitAlertService : BaseService
    {


        public HitAlertService()
        {
            url = "/hitalert";
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
            Console.WriteLine("HitAlertService::OnClose");
        }
        
    }
}
