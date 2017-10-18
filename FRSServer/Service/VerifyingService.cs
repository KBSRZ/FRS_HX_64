using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">图片</param>
        /// <returns></returns>
        protected override int OnSet(byte[] imgsrc, byte[] imgdst)
        {
            Image img1 = FRS.Util.ImageHelper.BytesToImage(imgsrc);
            Image img2 = FRS.Util.ImageHelper.BytesToImage(imgdst);
            float score = 0;
            score = fa.Compare(img1, img2);

            if (null != socket && socket.IsAvailable)
            {
                socket.Send(new Message(Message.MessageType.PUSH, JsonConvert.SerializeObject(score)).ToJson());
            }


            if (score > 0)
            {
                return ReturnSuccessMessage();
            }

            return ReturnFailMessage();
        }
       
    }
}
