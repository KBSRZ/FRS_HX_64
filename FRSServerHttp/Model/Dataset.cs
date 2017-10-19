using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
namespace FRSServerHttp.Model
{
    class Dataset
    {
        public int ID { get; set; }
        public string DatasetName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }
       
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static Dataset CreateDatasetFromJSON(string json)
        {
            Dataset msg = null;
            try
            {
                msg = (Dataset)JsonConvert.DeserializeObject(json, typeof(Dataset));
            }
            catch
            {

            }
            return msg;
        }
    }
}
