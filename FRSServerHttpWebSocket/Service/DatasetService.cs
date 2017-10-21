using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRSServerHttp.Model;
using System.IO;
using Newtonsoft.Json;
using FRSServerHttp.Server;

namespace FRSServerHttp.Service
{
    class DatasetService:BaseService
    {

     
        public static string Domain
        {
            get
            {
                return "dataset";
            }
        }

        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            if (request.RestConvention != null)//根据ID获得数据库
            {
                
               Log.Debug(string.Format("返回ID{0}的数据库信息", request.RestConvention));
                int id=-1;
                try{
                 id =Convert.ToInt32(request.RestConvention);
                }
                catch {

                }

                Dataset ds = new Dataset();
                ds.DatasetName = "frsdb1";
                ds.ID = 1;
                ds.IP = "127.0.0.1";
                ds.Port = "212";
                response.SetContent(JsonConvert.SerializeObject(ds));

            }
            else if (request.Domain != null)//获得所有数据库
            {
                Log.Debug(string.Format("所有数据库信息", request.RestConvention));
                Dataset[] ds = new Dataset[2]{new Dataset (),new Dataset()};
                ds[0].DatasetName = "frsdb1";
                ds[0].ID = 1;
                ds[0].IP = "127.0.0.1";
                ds[0].Port = "212";
                ds[1].DatasetName = "frsdb2";
                ds[1].ID = 2;
                ds[1].IP = "127.0.0.1";
                ds[1].Port = "2112";

                response.SetContent(JsonConvert.SerializeObject(ds));

            }
            response.Send();
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {

            bool status = true;
            if (request.Operation == null)//添加一条数据
            {
                Dataset da = Dataset.CreateInstanceFromJSON(request.PostParams);
               
                if (null != da)
                {
                    //添加到数据库
                   Log.Debug("添加数据库信息");
                   Log.Debug(request.PostParams);

                }
            }
            else
            {
                if (request.Operation == "update")//更新
                {
                   Log.Debug("更新数据库信息");
                    Log.Debug(request.PostParams);
                }
                else if (request.Operation == "delete")//删除
                {

                    Log.Debug(string.Format("删除ID:{0}数据库", request.RestConvention));
                }
            }
            response.SetContent(status.ToString());
            response.Send();
           
        }
    }
}
