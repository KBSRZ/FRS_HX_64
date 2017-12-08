using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Newtonsoft.Json;
using DataAngine_Set.Model;
using System.Data;
namespace FRSServerHttp.Model
{
    /// <summary>
    /// 添加人员库时所用的结构
    /// </summary>
    class AddInfo
    {
        public string DatasetName { get; set; }
        //public string Path { get; set; }
        public string Type { get; set; }
        public string Remark { get; set; }

        public static AddInfo CreateInstanceFromJSON(string json)
        {
            AddInfo msg = null;
            try
            {
                msg = (AddInfo)JsonConvert.DeserializeObject(json, typeof(AddInfo));
            }
            catch
            {
            }
            return msg;
        }

    }

    class RegisterInfo
    {
        public int DatasetId { get; set; }
        public string Path { get; set; }

        public static RegisterInfo CreateInstanceFromJSON(string json)
        {
            RegisterInfo msg = null;
            try
            {
                msg = (RegisterInfo)JsonConvert.DeserializeObject(json, typeof(RegisterInfo));
            }
            catch
            {
            }
            return msg;
        }
    }

    class ViewInfo
    {
        public int StartIndex { get; set; }
        public int PageSize { get; set; }

        public static ViewInfo CreateInstanceFromJSON(string json)
        {
            ViewInfo msg = null;
            try
            {
                msg = (ViewInfo)JsonConvert.DeserializeObject(json, typeof(ViewInfo));
            }
            catch
            {
            }
            return msg;
        }

    }

    class UserData
    {
        public int id { get; set; }
        public string people_id { get; set; }
        public string name { get; set; }
        public string gender { get; set; }
        public string card_id { get; set; }
        public string image_id { get; set; }
        public string face_image_path { get; set; }



        public static UserData[] CreateInstanceFromDataAngineDataSet(DataSet ds)
        {
            if (0 == ds.Tables.Count)
            {
                return null;
            }

            DataTable dt = ds.Tables[0];
            int UserCount = dt.Rows.Count;
            UserData[] users = new UserData[UserCount];

            for (int i = 0; i < UserCount; ++i)
            {
                UserData userdata = new UserData();
                userdata.id = Convert.ToInt32(dt.Rows[i]["id"]);
                userdata.people_id = dt.Rows[i]["people_id"].ToString();
                userdata.name = dt.Rows[i]["name"].ToString();
                userdata.gender = dt.Rows[i]["gender"].ToString();
                userdata.card_id = dt.Rows[i]["card_id"].ToString();
                userdata.image_id = dt.Rows[i]["image_id"].ToString();
                userdata.face_image_path = dt.Rows[i]["face_image_path"].ToString();
                users[i] = userdata;
            }
            return users;
        }
    }

    class Dataset
    {
        public int ID { get; set; }
        public string DatasetName { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public int? Type { get; set; }
        public string Remark { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        public static Dataset CreateInstanceFromJSON(string json)
        {
            Dataset msg = null;
            try
            {
                msg = (Dataset)JsonConvert.DeserializeObject(json, typeof(Dataset));
            }
            catch
            {

            }
            return msg;
        }


        public static Dataset CreateInstanceFromDataAngineModel(dataset dt)
        {
            if (null == dt)
            {
                return null;
            }
            Dataset d = new Dataset();
            d.DatasetName = dt.datasetname;
            d.ID = dt.id;
            d.IP = dt.ip;
            d.Password = dt.password;
            d.Port = dt.port;
            d.Type = dt.type;
            d.User = dt.user;
            return d;
        }

        public static Dataset[] CreateInstanceFromDataAngineModel(dataset[] dts)
        {
            if (null == dts)
            {
                return null;
            }
            Dataset[] ds = new Dataset[dts.Length];
            for (int i = 0; i < dts.Length; i++)
            {
                Dataset d = new Dataset();
                d.DatasetName = dts[i].datasetname;
                d.ID = dts[i].id;
                d.IP = dts[i].ip;
                d.Password = dts[i].password;
                d.Port = dts[i].port;
                d.Type = dts[i].type;
                d.User = dts[i].user;
                ds[i] = d;
            }
            return ds;
        }
        public dataset ToDataAngineModel()
        {
            dataset d = new dataset();
            d.datasetname = this.DatasetName;
            d.id = this.ID;
            d.ip = this.IP;
            d.password = this.Password;
            d.port = this.Port;
            d.type = this.Type;
            d.user = this.User;
            return d;
        }     
    }
}
