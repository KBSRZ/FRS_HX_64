using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
namespace FRSServer.Data
{
    class Device 
    {
        
       public string DeviceName{get;set;}
       public string VideoAddress { get; set; }


       public Device()
        {
            try
            {
                Device de = CreateDeviceFromJSON(ConfigurationManager.AppSettings["SelectedDevice"]);
                this.VideoAddress = de.VideoAddress;
                this.DeviceName = de.DeviceName;

            }
            catch
            {
            }
        }
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
           catch
           {

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
