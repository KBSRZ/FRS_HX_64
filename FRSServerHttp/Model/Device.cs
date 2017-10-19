using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
namespace FRSServerHttp.Model
{

    /// <summary>
    /// 摄像地址
    /// </summary>
    class Device 
    {
       public int ID { get; set; }
       public string Name { get; set; }
       public string Address{get;set;}//视频地址
       public string DepartmentID { get; set; }
       public double Longitude { get; set; }//经度
       public double Latitude { get; set; }//纬度
       public string LocationType { get; set; }//区域类型，汽车站，公交站，酒吧
       public string Remark { get; set; }//备注

       public string ToJson()
       {
           return JsonConvert.SerializeObject(this);
       }

       public static Device CreateDeviceFromJSON(string json)
       {
           Device msg = null;
           try
           {
               msg = (Device)JsonConvert.DeserializeObject(json, typeof(Device));
           }
           catch (Exception e)
           {
               Console.WriteLine(e.StackTrace + e.Message);
           }
           return msg;
       }
    }
    class DeviceData
    {
        public Device[] Devices { get; set; }
        public string SelectedDeviceName
        {
            get
            {
                return selectedDeviceName;
            }
        }

        string selectedDeviceName;

        public DeviceData()
        {
            try
            {
                selectedDeviceName = ConfigurationManager.AppSettings["selectedDeviceName"];
            }
            catch
            {
                selectedDeviceName = "";
            }
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static int SaveSelectedDeviceName(string selectedDeviceName)
        {
            try
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //写入元素的Value

                config.AppSettings.Settings["SelectedDeviceName"].Value = selectedDeviceName;

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
    }
}
