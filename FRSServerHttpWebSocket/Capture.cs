using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using FRSServerHttp.Model;
namespace FRSServerHttp.Test
{
    class Capture
    {

        public delegate void HitAlertCallback(HitAlert[] Data);
        public event HitAlertCallback HitAlertReturnEvent;
        private bool isRun = false;
        public bool  IsRun{
            get { return isRun; }
        }
        public void Start()
        {
            isRun = true;
            Model.User user1 = new User();
            user1.CardId = "1";
            user1.Gender = "男";
            user1.FaceImagePath = "RegFaces/1.jpg";
            user1.Name = "小明";
            user1.Type = "v";

            User user2 = new User();
            user2.CardId = "2";
            user2.Gender = "男";
            user2.FaceImagePath = "RegFaces/2.jpg";
            user2.Name = "小华";
            user2.Type = "c";

            HitAlertDetail hd1 = new HitAlertDetail();
            hd1.Score = 0.9f;
            hd1.UserInfo = user1;

            HitAlertDetail hd2 = new HitAlertDetail();
            hd2.Score = 0.8f;
            hd2.UserInfo = user2;

            HitAlertDetail[] hds = new HitAlertDetail[2];
            hds[0] = hd1;
            hds[1] = hd2;

            HitAlert hit = new HitAlert();
            hit.Threshold = 0.6f;
            hit.OccurTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            hit.Details = hds;
            hit.FaceQueryImagePath = ("QueryFaces/1.jpg");

            HitAlert[] hits = new HitAlert[2];
            hits[0] = hit;
            hits[1] = hit;

            Thread t = new Thread(new ThreadStart(() =>
            {
                while (isRun)
                {
                    Thread.Sleep(1000);

                    HitAlertReturnEvent(hits);
                }
            }));
            t.Start();
        }

        public void Stop()
        {
            isRun = false;
        }
    }

}
