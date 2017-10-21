using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using DataAngine_Set.Model;
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
       public string DepartmentID { get; set; }//公安ID
       public double ?Longitude { get; set; }//经度
       public double ?Latitude { get; set; }//纬度
       public int? LocationType { get; set; }//区域类型，汽车站，公交站，酒吧
       public string Remark { get; set; }//备注

       public string ToJson()
       {
           return JsonConvert.SerializeObject(this);
       }

       public static Device CreateInstanceFromJSON(string json)
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
        public  device ToDataAngineModel(){
            device d = new device();
            d.id = this.ID;
            d.latitude = this.Latitude;
            d.locationtype = this.LocationType;
            d.longitude = this.Longitude;
            d.name = this.Name;
            d.remark = this.Remark;
            d.address = this.Address;
            d.departmentmentid = this.DepartmentID;
            return d;

        }

        public static  Device CreateInstanceFromDataAngineModel(device d)
        {
            if (null == d) return null;
            Device de = new Device();
            de.ID = d.id;
            de.Latitude = d.latitude;
            de.LocationType=d.locationtype;
            de.Longitude=d.longitude;
            de.Name = d.name;
            de.Remark = d.remark;
            de.Address = d.address;
            de.DepartmentID = d.departmentmentid;
            return de;

        }

        public static Device[] CreateInstanceFromDataAngineModel(device [] ds)
        {
            if (null == ds) return null;
            Device[] des = new Device[ds.Length];
            for (int i = 0; i < ds.Length; i++)
            {


                Device de = new Device();
                de.ID = ds[i].id;
                de.Latitude = ds[i].latitude;
                de.LocationType = ds[i].locationtype;
                de.Longitude = ds[i].longitude;
                de.Name = ds[i].name;
                de.Remark = ds[i].remark;
                de.Address = ds[i].address;
                de.DepartmentID = ds[i].departmentmentid;
                des[i] = de;
            }
            return des;

        }
    }
    //class DeviceData
    //{
    //    public Device[] Devices { get; set; }
    //    public string SelectedDeviceName
    //    {
    //        get
    //        {
    //            return selectedDeviceName;
    //        }
    //    }

    //    string selectedDeviceName;

    //    public DeviceData()
    //    {
    //        try
    //        {
    //            selectedDeviceName = ConfigurationManager.AppSettings["selectedDeviceName"];
    //        }
    //        catch
    //        {
    //            selectedDeviceName = "";
    //        }
    //    }
    //    /// <summary>
    //    /// 保存设置
    //    /// </summary>
    //    /// <param name="setting"></param>
    //    /// <returns></returns>
    //    public static int SaveSelectedDeviceName(string selectedDeviceName)
    //    {
    //        try
    //        {
    //            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

    //            //写入元素的Value

    //            config.AppSettings.Settings["SelectedDeviceName"].Value = selectedDeviceName;

    //            //一定要记得保存，写不带参数的config.Save()也可以
    //            config.Save(ConfigurationSaveMode.Modified);
    //        }
    //        catch
    //        {
    //            return ReturnCode.FAIL;
    //        }
    //        return ReturnCode.SUCCESS;
    //    }

    //    public string ToJson()
    //    {
    //        return JsonConvert.SerializeObject(this);
    //    }
    //}
}
