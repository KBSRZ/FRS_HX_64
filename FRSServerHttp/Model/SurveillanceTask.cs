using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace FRSServerHttp.Model
{
    class SurveillanceTask
    {
        public Device Device { get; set; }
        public Dataset Dataset { get; set; }

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
