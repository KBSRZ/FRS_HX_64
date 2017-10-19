using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace FRSServerHttp.Model
{
    /// <summary>
    /// 布控任务
    /// </summary>
    class SurveillanceTask
    {
        public int ID { get; set; }
        public string Name { get; set; }//
        public int DatasetID { get; set; }
        public int DeviceID { get; set; }

    
        public string Type { get; set; }//布控类型
       
        public string Remark { get; set; }//备注


        public static SurveillanceTask CreateSurveillanceTaskFromJson(string json){
            SurveillanceTask msg = null;
            try
            {
                msg = (SurveillanceTask)JsonConvert.DeserializeObject(json, typeof(SurveillanceTask));
            }
            catch
            {

            }
            return msg;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
       
    }
}
