using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRS;

namespace FRSServerHttp.Model
{
    /// <summary>
    /// 用户信息 对原来的类进行再次封装方便序列化
    /// </summary>
    /// 

    public class User
    {
        public User() { }
        public int ID { get; set; }
        public string Name { get; set; }
        public string PeopleID { get; set; }
        public string Gender { get; set; }
        public string CardId { get; set; }
        public string ImageID { get; set; }
        public string FaceImagePath { get; set; }
        public string CreateTime { get; set; }
        public string ModifiedTime { get; set; }
        private float? QualityScore;
        public string Type { get; set; }
    };
    /// <summary>
    /// HitAlertDetail 对原来的类进行再次封装方便序列化
    /// </summary>
    /// 

    public class HitAlertDetail
    {

        public int ID { get; set; }
        public float Score { get; set; }
        private int Rank;
        public User UserInfo { get; set; }

        public HitAlertDetail(FRS.HitAlertDetail hd)
        {
            this.Score = hd.Score;
            this.UserInfo = new User();
            this.UserInfo.CardId = hd.cardId;
            this.UserInfo.Gender = hd.gender;
            this.UserInfo.FaceImagePath = hd.imgPath;
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
        public HitAlert(FRS.HitAlert hitAlert)
        {
            this.OccurTime = ((DateTime)(hitAlert.OccurTime)).ToString("yyyy-MM-dd HH:mm:ss");
            this.FaceQueryImagePath = hitAlert.QueryFacePath;
            this.Threshold = ((float)(hitAlert.Threshold));
            this.Details = new HitAlertDetail[hitAlert.Details.Length];
            for (int i = 0; i < this.Details.Length; i++)
            {
                this.Details[i] = new HitAlertDetail(hitAlert.Details[i]);
            }

        }
        public string FaceQueryImagePath { get; set; }//base64 形式存储
        public float Threshold { get; set; }
        public string OccurTime { get; set; }//yyyy-MM-dd HH:mm:ss
        public HitAlertDetail[] Details { get; set; }
        public static HitAlert[] CreateInstanceFromFRSHitAlert(FRS.HitAlert[] result)
        {
            if (null == result) return null;
            HitAlert[] hits = new HitAlert[result.Length];
            for (int i = 0; i < result.Length; i++)
            {
                hits[i] = new HitAlert(result[i]);
            }
            return hits;
        }
       
    }

    
}
