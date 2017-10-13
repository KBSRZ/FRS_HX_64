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

       // /// <summary>
       // /// 根据请求路径 获得服务类型
       // /// </summary>
       // /// <param name="path"></param>
       // /// <returns></returns>
       //public  static ServiceType GetServiceType(string path)
       // {
       //     //设置
       //     if (path == "/setting")
       //     {

       //         Console.WriteLine("准备设置服务");
       //         return ServiceType.SettingService;
       //     }
       //     else if (path == "/controlling")
       //     {

       //         Console.WriteLine("准备控制服务");
       //         return ServiceType.ControllerService;
       //     }
       //     else if (path == "/register")
       //     {

       //         Console.WriteLine("开启注册服务");
       //         return ServiceType.RegisterService;
       //     }
       //     //
       //     else if (path == "/hitalert")
       //     {
       //         Console.WriteLine("开启实时命中服务");
       //         return ServiceType.HitAlertService;
       //     }
       //     //比较两张图片
       //     else if (path == "/verifying")
       //     {

       //         Console.WriteLine("开启验证服务");
       //         return ServiceType.VerifyingService;
       //     }
       //     //以图搜图
       //     else if (path == "/searching-by-image")
       //     {

       //         Console.WriteLine("开启以图搜图服务");
       //         return ServiceType.SearchingByImageService;
       //     }
       //     // 通过时间搜索
       //     else if (path == "/searching-by-time")
       //     {

       //         Console.WriteLine("开启时间服务");
       //         return ServiceType.SearchingByTimeService;
       //     }
       //     return ServiceType.NoService;
       // }
       /// <summary>
       /// 根据URL查找服务
       /// </summary>
       /// <param name="url"></param>
       /// <returns>null表示查找失败</returns>
       public static BaseService GetService(string url)
       {
           if (services.ContainsKey(url))
               return services[url];
           else
               return null;
       }

    }
}
