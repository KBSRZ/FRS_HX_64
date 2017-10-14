using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace FRSServer.Data
{
   /// <summary>
   /// 用户信息 对原来的类进行再次封装方便序列化
   /// </summary>
   /// 

        public class User
        {
            public User() { }
            public string Name { get; set; }


            public string Gender { get; set; }

            public string CardId { get; set; }


            public string Image { get; set; } //base64 形式存储 图片

            public string Type { get; set; }

        };
        /// <summary>
        /// HitAlertDetail 对原来的类进行再次封装方便序列化
        /// </summary>
        /// 

        public class HitAlertDetail
        {

           
            public float Score { get; set; }

            public User UserInfo { get; set; }
            public HitAlertDetail(FRS.HitAlertDetail hd){
                this.Score = hd.Score;
                this.UserInfo=new User() ;
                this.UserInfo.CardId = hd.cardId;
                this.UserInfo.Gender = hd.gender;
                this.UserInfo.Image = ImageUtils.ImageToBase64(Image.FromFile(hd.imgPath));
              
                this.UserInfo.Name = hd.name;
                this.UserInfo.Type = hd.type;
            }

            public HitAlertDetail()
            {
                // TODO: Complete member initialization
            }


        };
        /// <summary>
        /// HitAlert 对原来的类进行再次封装方便序列化
        /// </summary>
        /// 

        class HitAlert
        {
           public HitAlert() { }
           public  HitAlert(FRS.HitAlert hitAlert)
            {

                this.OccurTime=((DateTime)(hitAlert.OccurTime)).ToString("yyyy-MM-dd HH:mm:ss");
                
                this.QueryFace = ImageUtils.ImageToBase64(hitAlert.QueryFace);
               
               
                this.Threshold = ((float)(hitAlert.Threshold));
             
                this.Details = new HitAlertDetail[hitAlert.Details.Length];
                
              
                for (int i = 0; i < this.Details.Length; i++)
                {
                    this.Details[i] = new HitAlertDetail(hitAlert.Details[i]);
                }
                
            }
            public string QueryFace { get; set; }//base64 形式存储

            public float Threshold { get; set; }

            public string OccurTime { get; set; }//yyyy-MM-dd HH:mm:ss

            public HitAlertDetail[] Details { get; set; }
    }


        
       
}
