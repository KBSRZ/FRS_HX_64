using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FRS;
using System.Data;
using Newtonsoft.Json;

namespace FRSServerHttp.Model
{

    class SearchInfo
    {       
        public DateTime StartTime { get; set; }    
        public DateTime EndTime { get; set; }
        public int StartIndex { get; set; } 
        public int PageSize { get; set; } 

        public static SearchInfo CreateInstanceFromJSON(string json)
        {
            SearchInfo msg = null;
            try
            {
                msg = (SearchInfo)JsonConvert.DeserializeObject(json, typeof(SearchInfo));
            }
            catch
            {
            }
            return msg;
        }

    }

    class HitAlertData
    {
        public int id { get; set; }
        public string FaceQueryImagePath { get; set; }
        public float Threshold { get; set; }
        public string OccurTime { get; set; }//yyyy-MM-dd HH:mm:ss
        public int detail_id { get; set; }
        public int rank { get; set; }
        public float score { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_gander { get; set; }
        public string user_people_id { get; set; }
        public string user_card_id { get; set; }
        public string user_image_id { get; set; }
        public string user_face_image_path { get; set; }
        public string user_type { get; set; }
        public string user_create_time { get; set; }
        public string user_modified_time { get; set; }
        public float user_quality_score { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

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

        public static HitAlertData[] CreateInstanceFromDataAngineDataSet(DataSet ds)
        {
            if (0 == ds.Tables.Count)
            {
                return null;
            }

            DataTable dt = ds.Tables[0];
            int HitCount = dt.Rows.Count;
            HitAlertData[] hits = new HitAlertData[HitCount];

            for (int i = 0; i < HitCount; ++i)
            {
                HitAlertData hitalertdata = new HitAlertData();
                hitalertdata.id = Convert.ToInt32(dt.Rows[i]["id"]);
                hitalertdata.FaceQueryImagePath = dt.Rows[i]["face_query_image_path"].ToString();
                hitalertdata.Threshold = Convert.ToSingle(dt.Rows[i]["Threshold"]);
                hitalertdata.OccurTime = dt.Rows[i]["occur_time"].ToString();
                hitalertdata.detail_id = Convert.ToInt32(dt.Rows[i]["detail_id"]);
                hitalertdata.rank = Convert.ToInt32(dt.Rows[i]["rank"]);
                hitalertdata.score = Convert.ToSingle(dt.Rows[i]["score"]);
                hitalertdata.user_id = Convert.ToInt32(dt.Rows[i]["user_id"]);
                hitalertdata.user_name = dt.Rows[i]["user_name"].ToString();
                hitalertdata.user_gander = dt.Rows[i]["user_gender"].ToString();
                hitalertdata.user_people_id = dt.Rows[i]["user_people_id"].ToString();
                hitalertdata.user_card_id = dt.Rows[i]["user_card_id"].ToString();
                hitalertdata.user_image_id = dt.Rows[i]["user_image_id"].ToString();
                hitalertdata.user_face_image_path = dt.Rows[i]["user_face_image_path"].ToString();
                hitalertdata.user_type = dt.Rows[i]["user_type"].ToString();
                hitalertdata.user_create_time = dt.Rows[i]["user_create_time"].ToString();
                hitalertdata.user_modified_time = dt.Rows[i]["user_modified_time"].ToString();
                hitalertdata.user_quality_score = Convert.ToSingle(dt.Rows[i]["user_quality_score"]);
                hits[i] = hitalertdata;
            }
            return hits;
        }

        
    }
}
