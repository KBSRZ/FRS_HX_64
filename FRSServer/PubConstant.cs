using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace FRSServer
{
    class PubConstant
    {

     
        /// <summary>
        /// video地址
        /// </summary>
        public static string SelelctedDataSetName
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["SelelctedDataSetName"].Trim();
                }
                catch
                {
                    return  "rtsp://192.168.1.64:554";
                }
            }
        }
        /// <summary>
        /// Caputure的初始值，true表示启动时对应的监控为开启状态
        /// </summary>
        public static bool CaptureInitStatus
        {
            get
            {
                string status = ConfigurationManager.AppSettings["CaptureInitStatus"];
                bool st = false ;
              
                for (int i = 0; i < status.Length; i++)
                {
                    try
                    {
                        st = Convert.ToBoolean(status);
                    }
                    catch
                    { }
                }
                return st;

            }
        }
        


        /// <summary>
        /// 非法URL
        /// </summary>
        public static string ILLEGAL_URL = "ILLEGAL_URL";

        /// <summary>
        /// 成功
        /// </summary>
        public static string SUC  = "SUC";
        /// <summary>
        /// 失败
        /// </summary>
        public static string FAIL = "FAIL";

       
        /// <summary>
        /// 非法的 消息类型
        /// </summary>
        public static string ILLEGAL_MESSAGETYPE = "ILLEGAL_MESSAGETYPE";


       
        
    }

}
