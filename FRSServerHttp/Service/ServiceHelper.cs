using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FRSServerHttp.Service
{
    /// <summary>
    /// 工厂类，根据url 返回不同的服务
    /// </summary>
    class ServiceHelper
    {
      
       /// <summary>
       /// 根据URL查找服务
       /// </summary>
       /// <param name="url"></param>
       /// <returns>null表示查找失败</returns>
       public static BaseService GetService(string url)
       {
          

           if (HitAlertService.URL == url)
           {
               return new HitAlertService();
           }
           else if (SurveillanceTaskService.URL == url)
           {
               return new SurveillanceTaskService();
           }

           return null;
       }

    }
}
