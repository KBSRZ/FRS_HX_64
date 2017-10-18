using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
namespace FRSServerHttp.Model.Setting
{
    /// <summary>
    /// 人脸识别参数设置
    /// </summary>
    class SettingFRS
    {


        #region 初始化参数
        ///// <summary>
        ///// 获取连接字符串
        ///// </summary>
        //public  string ConnectionStringMySQL
        //{
        //    get { return connectionStringMySQL; }
        //    set { connectionStringMySQL = value; }
        //}
        /// <summary>
        /// 同时支持的视频路数
        /// </summary>
        public  short ChannelNum
        {
            get { return channelNum; }
            set { channelNum = value; }
        }
        #endregion



        #region 初始化参数
        private string connectionStringMySQL;
        private short channelNum;
        #endregion

        #region 比对时的设置

        /// <summary>
        /// 比对间隔，单位毫秒
        /// </summary>
        public int Interval
        {
            get{return interval;}
            set{interval=value;}
        }
       
        /// <summary>
        /// 比对时人脸宽度阈值
        /// </summary>
        public  int SearchFaceWidthThresh
        {
            get { return searchFaceWidthThresh; }
            set { searchFaceWidthThresh = value; }
        }


        /// <summary>
        /// 比对时人脸高度阈值
        /// </summary>
        public  int SearchFaceHeightThresh
        {
            get { return searchFaceHeightThresh; }
            set { searchFaceHeightThresh = value; }
        }

        /// <summary>
        /// 比对时人脸角度阈值Yaw
        /// </summary>
        public  int SearchFaceYawThresh
        {
            get { return searchFaceYawThresh; }
            set { searchFaceYawThresh = value; }
        }

        /// <summary>
        /// 比对时人脸角度阈值Roll
        /// </summary>
        public  int SearchFaceRollThresh
        {
            get { return searchFaceRollThresh; }
            set { searchFaceRollThresh = value; }
        }


        /// <summary>
        /// 比对时人脸角度阈值Pitch
        /// </summary>
        public  int SearchFacePitchThresh
        {
            get { return searchFacePitchThresh; }
            set { searchFacePitchThresh = value; }
        }


        /// <summary>
        /// 比对时人脸质量阈值
        /// </summary>
        public  int SearchFaceQualityThresh
        {
            get { return searchFaceQualityThresh; }
            set { searchFaceQualityThresh = value; }
        }

        /// <summary>
        /// 比对阈值
        /// </summary>
        public float ScoreThresh
        {
            get { return scoreThresh; }
            set { scoreThresh = value; }
        }
        /// <summary>
        /// 每个人脸返回的最大结果数
        /// </summary>
        public  int TopK
        {
            get { return topK; }
            set { topK = value; }
        }

        /// <summary>
        /// 单帧画面最大人脸数
        /// </summary>
        public  int MaxPersonNum
        {
            get { return maxPersonNum; }
            set { maxPersonNum = value; }
        }

        #endregion

        #region 注册时的设置
        /// <summary>
        /// 注册时人脸宽度阈值
        /// </summary>
        public  int RegisterFaceWidthThresh
        {
            get { return registerFaceWidthThresh; }
            set { registerFaceWidthThresh = value; }
        }


        /// <summary>
        /// 注册时人脸高度阈值
        /// </summary>
        public  int RegisterFaceHeightThresh
        {
            get { return registerFaceHeightThresh; }
            set { registerFaceHeightThresh = value; }
        }

        /// <summary>
        /// 注册时人脸角度阈值Yaw
        /// </summary>
        public  int RegisterFaceYawThresh
        {
            get { return registerFaceYawThresh; }
            set { registerFaceYawThresh = value; }
        }

        /// <summary>
        /// 注册时人脸角度阈值Roll
        /// </summary>
        public  int RegisterFaceRollThresh
        {
            get { return registerFaceRollThresh; }
            set { registerFaceRollThresh = value; }
        }


        /// <summary>
        /// 注册时人脸角度阈值Pitch
        /// </summary>
        public  int RegisterFacePitchThresh
        {
            get { return registerFacePitchThresh; }
            set { registerFacePitchThresh = value; }
        }


        /// <summary>
        /// 注册时人脸质量阈值
        /// </summary>
        public  int RegisterFaceQualityThresh
        {
            get { return registerFaceQualityThresh; }
            set { registerFaceQualityThresh = value; }
        }

        #endregion



        #region 比对时的设置

        /// <summary>
        /// 比对间隔，单位毫秒
        /// </summary>
        private int interval;
       

        /// <summary>
        /// 比对时人脸宽度阈值
        /// </summary>
        private int searchFaceWidthThresh;
      


        /// <summary>
        /// 比对时人脸高度阈值
        /// </summary>
        private int searchFaceHeightThresh;
        

        /// <summary>
        /// 比对时人脸角度阈值Yaw
        /// </summary>
        private int searchFaceYawThresh;
       

        /// <summary>
        /// 比对时人脸角度阈值Roll
        /// </summary>
        private int searchFaceRollThresh;
        


        /// <summary>
        /// 比对时人脸角度阈值Pitch
        /// </summary>
        private int searchFacePitchThresh;
        


        /// <summary>
        /// 比对时人脸质量阈值
        /// </summary>
        private int searchFaceQualityThresh;
       

        /// <summary>
        /// 比对阈值
        /// </summary>
        private float scoreThresh;
      
        /// <summary>
        /// 每个人脸返回的最大结果数
        /// </summary>
        private int topK;


        /// <summary>
        /// 单帧画面最大人脸数
        /// </summary>
        private int maxPersonNum;
       

        #endregion

        #region 注册时的设置
        /// <summary>
        /// 注册时人脸宽度阈值
        /// </summary>
        private int registerFaceWidthThresh;
        


        /// <summary>
        /// 注册时人脸高度阈值
        /// </summary>
        private int registerFaceHeightThresh;
      

        /// <summary>
        /// 注册时人脸角度阈值Yaw
        /// </summary>
        private int registerFaceYawThresh;
      

        /// <summary>
        /// 注册时人脸角度阈值Roll
        /// </summary>
        private int registerFaceRollThresh;
       


        /// <summary>
        /// 注册时人脸角度阈值Pitch
        /// </summary>
        private int registerFacePitchThresh;
        


        /// <summary>
        /// 注册时人脸质量阈值
        /// </summary>
        private int registerFaceQualityThresh;
       
        #endregion



        public SettingFRS()
        {

            #region 初始设置
            try
            {
                connectionStringMySQL=ConfigurationManager.AppSettings["ConnectionStringMySQL"];
            }
            catch
            {
                connectionStringMySQL = "server=127.0.0.1;database=frsdb;uid=root;pwd=12345679";
            }

          
            try
            {
                channelNum = Convert.ToInt16(ConfigurationManager.AppSettings["ChannelNum"]);
            }
            catch
            {
                channelNum = 4;
            }
#endregion

            #region 比对时的设置


            try
            {
                interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);
            }
            catch
            {
                interval = 500;
            }

            try
            {
                searchFaceWidthThresh = Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceWidthThresh"]);
            }
            catch
            {
                searchFaceWidthThresh = 80;
            }

            try
            {
                searchFaceHeightThresh = Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceHeightThresh"]);
            }
            catch
            {
                searchFaceHeightThresh = 80;
            }


            try
            {
                searchFaceYawThresh = Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceYawThresh"]);
            }
            catch
            {
                searchFaceYawThresh = 21;
            }

            try
            {
                searchFaceRollThresh = Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceRollThresh"]);
            }
            catch
            {
                searchFaceRollThresh = 22;
            }


            try
            {
                searchFacePitchThresh = Convert.ToInt32(ConfigurationManager.AppSettings["SearchFacePitchThresh"]);
            }
            catch
            {
                searchFacePitchThresh = 23;
            }

            try
            {
                searchFaceQualityThresh = Convert.ToInt32(ConfigurationManager.AppSettings["SearchFaceQualityThresh"]);
            }
            catch
            {
                searchFaceQualityThresh = 30;
            }


            try
            {
                scoreThresh = Convert.ToSingle(ConfigurationManager.AppSettings["ScoreThresh"]);
            }
            catch
            {
                scoreThresh = 0.65f;
            }
            try
            {
                topK = Convert.ToInt32(ConfigurationManager.AppSettings["TopK"]);
            }
            catch
            {
                topK = 5;
            }

            try
            {
                maxPersonNum = Convert.ToInt32(ConfigurationManager.AppSettings["MaxPersonNum"]);
            }
            catch
            {
                maxPersonNum = 5;
            }

            #endregion

            #region 注册时的设置

            try
            {
                registerFaceWidthThresh = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceWidthThresh"]);
            }
            catch
            {
                registerFaceWidthThresh = 80;
            }

            try
            {
                registerFaceHeightThresh = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceHeightThresh"]);
            }
            catch
            {
                registerFaceHeightThresh = 80;
            }

            try
            {
                registerFaceYawThresh = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceYawThresh"]);
            }
            catch
            {
                registerFaceYawThresh = 21;
            }

            try
            {
                registerFaceRollThresh = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceRollThresh"]);
            }
            catch
            {
                registerFaceRollThresh = 22;
            }

            try
            {
                registerFacePitchThresh = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFacePitchThresh"]);
            }
            catch
            {
                registerFacePitchThresh = 23;
            }



            try
            {
                registerFaceQualityThresh = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterFaceQualityThresh"]);
            }
            catch
            {
                registerFaceQualityThresh = 30;
            }

            #endregion


        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static int Save(SettingFRS setting)
        {
            try
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //写入元素的Value

                //config.AppSettings.Settings["ConnectionStringMySQL"].Value = setting.ConnectionStringMySQL.ToString();
                config.AppSettings.Settings["ChannelNum"].Value = setting.ChannelNum.ToString();

                config.AppSettings.Settings["Interval"].Value = setting.Interval.ToString();
                config.AppSettings.Settings["SearchFaceWidthThresh"].Value = setting.SearchFaceWidthThresh.ToString();
                config.AppSettings.Settings["SearchFaceHeightThresh"].Value = setting.SearchFaceHeightThresh.ToString();
                config.AppSettings.Settings["SearchFaceYawThresh"].Value = setting.SearchFaceYawThresh.ToString();
                config.AppSettings.Settings["SearchFaceRollThresh"].Value = setting.SearchFaceRollThresh.ToString();
                config.AppSettings.Settings["SearchFacePitchThresh"].Value = setting.SearchFacePitchThresh.ToString();
                config.AppSettings.Settings["TopK"].Value = setting.TopK.ToString();

                config.AppSettings.Settings["MaxPersonNum"].Value = setting.MaxPersonNum.ToString();
                config.AppSettings.Settings["SearchFaceQualityThresh"].Value = setting.SearchFaceQualityThresh.ToString();
                config.AppSettings.Settings["RegisterFaceWidthThresh"].Value = setting.RegisterFaceWidthThresh.ToString();
                config.AppSettings.Settings["RegisterFaceYawThresh"].Value = setting.RegisterFaceYawThresh.ToString();
                config.AppSettings.Settings["RegisterFaceRollThresh"].Value = setting.RegisterFaceRollThresh.ToString();
                config.AppSettings.Settings["RegisterFacePitchThresh"].Value = setting.RegisterFacePitchThresh.ToString();
                config.AppSettings.Settings["RegisterFaceQualityThresh"].Value = setting.RegisterFaceQualityThresh.ToString();
                
               
                //一定要记得保存，写不带参数的config.Save()也可以
                config.Save(ConfigurationSaveMode.Modified);
            }
            catch
            {
                return ReturnCode.FAIL;
            }
            return ReturnCode.SUCCESS;
        }


        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static SettingFRS CreateMessageFromJSON(string json)
        {
            SettingFRS msg = null;
            try
            {
                msg = (SettingFRS)JsonConvert.DeserializeObject(json, typeof(SettingFRS));
            }
            catch
            {

            }
            return msg;
        }



    }

    
    
       

    
}
