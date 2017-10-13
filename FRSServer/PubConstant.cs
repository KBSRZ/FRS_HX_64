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

        #region 初始化参数
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionStringMySQL
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ConnectionStringMySQL"];
                }
                catch
                {
                    return "server=127.0.0.1;database=frsdb;uid=root;pwd=12345679";
                }
            }
        }
        /// <summary>
        /// 同时支持的视频路数
        /// </summary>
        public static short ChannelNum
        {
            get
            {
                try
                {
                    return Convert.ToInt16(ConfigurationManager.AppSettings["ChannelNum"]);
                }
                catch
                {
                    return 4;
                }
            }
        }
        #endregion

        #region 比对时的设置

        /// <summary>
        /// 比对间隔，单位毫秒
        /// </summary>
        public static int Interval
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);
                }
                catch
                {
                    return 500;
                }
            }
            set
            {
                ConfigurationManager.AppSettings["Interval"]=value.ToString();
            }
        }
       

        /// <summary>
        /// 比对时人脸宽度阈值
        /// </summary>
        public static int SearchFaceWidthThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceWidthThresh"]);
                }
                catch
                {
                    return 80;
                }
            }
        }


        /// <summary>
        /// 比对时人脸高度阈值
        /// </summary>
        public static int SearchFaceHeightThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceHeightThresh"]);
                }
                catch
                {
                    return 80;
                }
            }
        }

         /// <summary>
        /// 比对时人脸角度阈值Yaw
        /// </summary>
        public static int SearchFaceYawThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceYawThresh"]);
                }
                catch
                {
                    return 21;
                }
            }
        }

         /// <summary>
        /// 比对时人脸角度阈值Roll
        /// </summary>
        public static int SearchFaceRollThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceRollThresh"]);
                }
                catch
                {
                    return 22;
                }
            }
        }


         /// <summary>
        /// 比对时人脸角度阈值Pitch
        /// </summary>
        public static int SearchFacePitchThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SearchFacePitchThresh"]);
                }
                catch
                {
                    return 23;
                }
            }
        }


         /// <summary>
        /// 比对时人脸质量阈值
        /// </summary>
        public static int SearchFaceQualityThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceQualityThresh"]);
                }
                catch
                {
                    return 30;
                }
            }
        }
     
        /// <summary>
        /// 比对阈值
        /// </summary>
        public static float ScoreThresh
        {
            get
            {
                try
                {
                    return Convert.ToSingle(ConfigurationManager.AppSettings["ScoreThresh"]);
                }
                catch
                {
                    return 0.65f;
                }
            }
        }
        /// <summary>
        /// 每个人脸返回的最大结果数
        /// </summary>
        public static int TopK
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["TopK"]);
                }
                catch
                {
                    return 5;
                }
            }
        }

        /// <summary>
        /// 单帧画面最大人脸数
        /// </summary>
        public static int MaxPersonNum
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["MaxPersonNum"]);
                }
                catch
                {
                    return 5;
                }
            }
        }
        
      #endregion 

        #region 注册时的设置
        /// <summary>
        /// 注册时人脸宽度阈值
        /// </summary>
        public static int RegisterFaceWidthThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceWidthThresh"]);
                }
                catch
                {
                    return 80;
                }
            }
        }


        /// <summary>
        /// 注册时人脸高度阈值
        /// </summary>
        public static int RegisterFaceHeightThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceHeightThresh"]);
                }
                catch
                {
                    return 80;
                }
            }
        }

         /// <summary>
        /// 注册时人脸角度阈值Yaw
        /// </summary>
        public static int RegisterFaceYawThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceYawThresh"]);
                }
                catch
                {
                    return 21;
                }
            }
        }

         /// <summary>
        /// 注册时人脸角度阈值Roll
        /// </summary>
        public static int RegisterFaceRollThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceRollThresh"]);
                }
                catch
                {
                    return 22;
                }
            }
        }


         /// <summary>
        /// 注册时人脸角度阈值Pitch
        /// </summary>
        public static int RegisterFacePitchThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFacePitchThresh"]);
                }
                catch
                {
                    return 23;
                }
            }
        }


         /// <summary>
        /// 注册时人脸质量阈值
        /// </summary>
        public static int RegisterFaceQualityThresh
        {
            get
            {
                try
                {
                    return Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceQualityThresh"]);
                }
                catch
                {
                    return 30;
                }
            }
        }
     
#endregion
        
        
        /// <summary>
        /// video地址
        /// </summary>
        public static string[] VideoAdresses
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["VideoAdresses"].Trim(new char[]{';',' '}).Split(';');
                }
                catch
                {
                    return new string[] { "rtsp://192.168.1.64:554", "rtsp://192.168.1.64:554" };
                }
            }
        }
        /// <summary>
        /// Caputure的初始值，true表示启动时对应的监控为开启状态
        /// </summary>
        public static bool[] CaptureInitStatus
        {
            get
            {
                string[] status = ConfigurationManager.AppSettings["CaptureInitStatus"].Split(';');
                bool[] st = new bool[status.Length];
                for (int i = 0; i < st.Length; i++)
                {
                    st[i] = false;
                }
                for (int i = 0; i < status.Length; i++)
                {
                    try
                    {
                        st[i] = Convert.ToBoolean(status[i]);
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
