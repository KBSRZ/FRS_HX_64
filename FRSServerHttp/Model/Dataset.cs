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
        public string DatasetName { get; set; }

        public string Info { get; set; }
       
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
