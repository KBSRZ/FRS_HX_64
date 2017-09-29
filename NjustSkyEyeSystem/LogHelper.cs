using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace NjustSkyEyeSystem
{
    public class LogHelper
    {
        public static log4net.ILog log = log4net.LogManager.GetLogger("Daily.Logging");

        public static void WriteInfoLog(string msg)
        {
            log.Info(msg);
        }

        /// <summary>
        /// 输出错误日志到Log4Net
        /// </summary>
        /// <param name="ex"></param>
        #region static void WriteErrorLog(string msg, Exception ex)

        public static void WriteErrorLog(string msg, Exception ex=null)
        {
            if (null == ex)
            {
                log.Error(msg);
            }
            else
            {
                log.Error(msg, ex);
            }
        }
   
        #endregion

    }
}
