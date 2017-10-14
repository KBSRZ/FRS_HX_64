using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
namespace FRSServer.Test
{
    class Capture
    {

        public delegate void HitAlertCallback(Data.HitAlert[] Data);
        public event HitAlertCallback HitAlertReturnEvent;
        public void Start()
        {
            Data.User user1 = new Data.User();
            user1.CardId = "1";
            user1.Gender = "男";
            user1.Image = ImageUtils.ImageToBase64(Image.FromFile("D:/1.jpg"));
            user1.Name = "小明";
            user1.Type = "v";

            Data.User user2 = new Data.User();
            user2.CardId = "2";
            user2.Gender = "女";
            user2.Image = ImageUtils.ImageToBase64(Image.FromFile("D:/3.jpg"));
            user2.Name = "小红";
            user2.Type = "c";

            Data.HitAlertDetail hd1 = new Data.HitAlertDetail();
            hd1.Score = 0.9f;
            hd1.UserInfo = user1;

            Data.HitAlertDetail hd2 = new Data.HitAlertDetail();
            hd2.Score = 0.8f;
            hd2.UserInfo = user2;

            Data.HitAlertDetail[] hds = new Data.HitAlertDetail[2];
            hds[0] = hd1;
            hds[1] = hd2;

            Data.HitAlert hit = new Data.HitAlert();
            hit.Threshold = 0.6f;
            hit.OccurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hit.Details = hds;
            hit.QueryFace = ImageUtils.ImageToBase64(Image.FromFile("D:/3.jpg"));

            Data.HitAlert[] hits = new Data.HitAlert[2];
            hits[0] = hit;
            hits[1] = hit;

            Thread t = new Thread(new ThreadStart(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);

                    HitAlertReturnEvent(hits);
                }
            }));
            t.Start();
        }
    }

}
