using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Fleck;
using FRS;
using Newtonsoft.Json;
namespace FRSServer.Service
{
   
  
    abstract class BaseService
    {
        public static FeatureData fa;//数据操作类
        public static Capture cap;//视频处理类
        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public  string URL
        {
            get
            {
                return url;
            }
        }
        protected   string url=string.Empty;
        public BaseService() {  }
        public IWebSocketConnection Socket { get { return socket; } set { socket = value; } }
        protected IWebSocketConnection socket;
        /// <summary>
        /// 在webscoket OnOpen时使用
        /// </summary>
        public abstract void OnOpen();
        public abstract void OnClose();
        /// <summary>
        /// 在webscoket OnMessage时使用
        /// </summary>
        /// <param name="param">参数为json</param>
        public abstract int OnMessage(string param);

    }
   
   
    
    
    

    
}
