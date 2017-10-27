using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DataAngine_Set.Model;
namespace FRSServerHttp.Model
{
    /// <summary>
    /// 布控任务
    /// </summary>
    class SurveillanceTask
    {
        public int ID { get; set; }
        public string Name { get; set; }//
        public int DatasetID { get; set; }
        public int DeviceID { get; set; }


        public int? Type { get; set; }//布控类型

        public string Remark { get; set; }//备注


        public static SurveillanceTask CreateInstanceFromJson(string json)
        {
            SurveillanceTask msg = null;
            try
            {
                msg = (SurveillanceTask)JsonConvert.DeserializeObject(json, typeof(SurveillanceTask));
            }
            catch
            {

            }
            return msg;
        }
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }


        public static SurveillanceTask CreateInstanceFromDataAngineModel(surveillancetask st)
        {
            SurveillanceTask s = new SurveillanceTask();
            s.DatasetID = st.databaseid;
            s.DeviceID = st.deviceid;
            s.ID = st.deviceid;
            s.Name = st.name;
            s.Remark = st.remark;
            return s;
        }

        public static SurveillanceTask[] CreateInstanceFromDataAngineModel(surveillancetask[] sts)
        {
            SurveillanceTask[] ss = new SurveillanceTask[sts.Length];
            for (int i = 0; i < sts.Length; i++)
            {
                SurveillanceTask s = new SurveillanceTask();
                s.DatasetID = sts[i].databaseid;
                s.DeviceID = sts[i].deviceid;
                s.ID = sts[i].id;
                s.Name = sts[i].name;
                s.Remark = sts[i].remark;
                ss[i] = s;
            }
            return ss;
        }
        public surveillancetask ToDataAngineModel()
        {
            surveillancetask s = new surveillancetask();
            s.databaseid = this.DatasetID;
            s.deviceid = this.DeviceID;
            s.id = this.ID;
            s.name = this.Name;
            s.remark = this.Remark;
            s.type = this.Type;
            return s;
        }

    }
}
