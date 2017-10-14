using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
namespace FRSServer.Data
{
    class DatasetData
    {
        public string[] DatasetNames { get; set; }
        public string SelectedDatasetName
        {
            get
            {
                return selectedDatasetName;
            }
        }

        string selectedDatasetName;

        public DatasetData()
        {
            try
            {
                selectedDatasetName= ConfigurationManager.AppSettings["SelectedDatasetName"];
            }
            catch
            {
                selectedDatasetName= "frsdb";
            }
        }
        /// <summary>
        /// 保存设置
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static int SaveSelectedDataSetName(string dataSetName)
        {
            try
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                //写入元素的Value

                config.AppSettings.Settings["SelelctedDataSetName"].Value = dataSetName;

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
