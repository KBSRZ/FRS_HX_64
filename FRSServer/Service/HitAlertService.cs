using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FRSServer.Data;
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
        /// <summary>
        /// 命中时发送消息
        /// </summary>
        /// <param name="result"></param>
        public void OnHit(FRS.HitAlert[] result)
        {
            if (result == null || result.Length == 0) return;
            HitAlert[] hitalerts = new Data.HitAlert[result.Length];
            for (int i = 0; i < hitalerts.Length; i++)
            {
                hitalerts[i] = new Data.HitAlert(result[i]);
            }
            if (null != socket && socket.IsAvailable)
            {
                socket.Send(new Message(Message.MessageType.PUSH, JsonConvert.SerializeObject(hitalerts)).ToJson());
            }
        }
    }
}
