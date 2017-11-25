using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FRSServerHttp.Model
{
    /// <summary>
    /// 比较图片相似度的结构
    /// </summary>
    class VerifyOneVsOne
    {
        public byte[] PicSrc { get; set; }
        public byte[] PicDst { get; set; }
        public static VerifyOneVsOne CreateInstanceFromJSON(string json)
        {
            VerifyOneVsOne msg = null;
            try
            {
                msg = (VerifyOneVsOne)JsonConvert.DeserializeObject(json, typeof(VerifyOneVsOne));
            }
            catch
            {
            }
            return msg;
        }
    }

    class VerifyOneVsN
    {
        public byte[] PicSrc { get; set; }
        public static VerifyOneVsN CreateInstanceFromJSON(string json)
        {
            VerifyOneVsN msg = null;
            try
            {
                msg = (VerifyOneVsN)JsonConvert.DeserializeObject(json, typeof(VerifyOneVsN));
            }
            catch
            {
            }
            return msg;
        }
    }
}
