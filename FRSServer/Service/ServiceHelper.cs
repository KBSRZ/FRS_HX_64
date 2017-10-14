using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fleck;

namespace FRSServer.Service
{
    /// <summary>
    /// 工厂类，根据url 返回不同的服务
    /// </summary>
    class ServiceHelper
    {


        //存储服务类型<url,Service>

        static Dictionary<string, BaseService> services = new Dictionary<string, BaseService>();

        /// <summary>
        /// 
        /// </summary>
        private static int InitServices()
        {
            ControllerService controllerService = new ControllerService();
            HitAlertService hitAlertService = new HitAlertService();

            RegisterService registerService = new RegisterService();
            SearchingByImageService searchingByImageService = new SearchingByImageService();
            SearchingByTimeService searchingByTimeService = new SearchingByTimeService();
            SettingFRSService settingFRSService = new SettingFRSService();
            SettingDeviceService settingDeviceService = new SettingDeviceService();
            SettingDatasetService settingDatasetService = new SettingDatasetService();
            VerifyingService verifyingService = new VerifyingService();

            services.Add(controllerService.URL, new ControllerService());
            services.Add(hitAlertService.URL, new HitAlertService());
            services.Add(registerService.URL, new RegisterService());
            services.Add(searchingByImageService.URL, new SearchingByImageService());
            services.Add(searchingByTimeService.URL, new SearchingByTimeService());
            services.Add(settingFRSService.URL, new SettingFRSService());
            services.Add(settingDeviceService.URL, new SettingDeviceService());
            services.Add(settingDatasetService.URL, new SettingDatasetService());
            services.Add(verifyingService.URL, new VerifyingService());

            
            return  0;

        }
        private static int initFlag=InitServices();

      
       /// <summary>
       /// 根据URL查找服务
       /// </summary>
       /// <param name="url"></param>
       /// <returns>null表示查找失败</returns>
       public static BaseService GetService(string url)
       {
           Console.WriteLine(url);

           if (services.ContainsKey(url))
           {
               return services[url];
           }
               
           else
               return null;
       }

    }
}
