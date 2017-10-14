using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
namespace FRSServer.Data
{
    /// <summary>
    /// 控制开关
    /// </summary>
    class ControllingData
    {
        
        
        public bool Status { get; set; }//true 表示开启，false 表示关闭



        public static ControllingData CreateControllingDataFromJSON(string json)
        {
            ControllingData msg = null;
            try
            {
                msg = (ControllingData)JsonConvert.DeserializeObject(json, typeof(ControllingData));
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
