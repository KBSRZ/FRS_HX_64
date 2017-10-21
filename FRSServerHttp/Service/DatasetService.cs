using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRSServerHttp.Model;
using System.IO;
using Newtonsoft.Json;
using FRSServerHttp.Server;
using DataAngine_Set.BLL;
namespace FRSServerHttp.Service
{
    class DatasetService:BaseService
    {

        dataset bll = new dataset();
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
                
                Console.WriteLine("返回ID{0}的数据库信息", request.RestConvention);
                int id=-1;
                try{
                 id =Convert.ToInt32(request.RestConvention);
                }
                catch {

                }
                Dataset da = Dataset.CreateInstanceFromDataAngineModel(bll.GetModel(id));
                if (null != da)
                {
                    response.SetContent(da.ToJson());
                }

            }
            else if (request.Domain != null)//获得所有数据库
            {
                List<DataAngine_Set.Model.dataset> datasets = bll.DataTableToList(bll.GetAllList().Tables[0]);
                response.SetContent(JsonConvert.SerializeObject(Dataset.CreateInstanceFromDataAngineModel(datasets.ToArray())));

            }
            response.Send();
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {

            bool status = false;
            if (request.Operation == null)//添加一条数据
            {
                Dataset da = Dataset.CreateInstanceFromJSON(request.PostParams);
               
                if (null != da)
                {
                    //添加到数据库
                    Console.WriteLine("添加数据库信息");
                    status=bll.Add(da.ToDataAngineModel());

                }
            }
            else
            {
                if (request.Operation == "update")//更新
                {
                    Console.WriteLine("更新数据库信息");
                    Dataset da = Dataset.CreateInstanceFromJSON(request.PostParams);
                    if (null != da)
                    {
                        status = bll.Update(da.ToDataAngineModel());
                    }
                }
                else if (request.Operation == "delete")//删除
                {
                    int id = -1;
                    try
                    {
                        id = Convert.ToInt32(request.RestConvention);
                    }
                    catch
                    {

                    }
                    status = bll.Delete(id);
                }
            }
            response.SetContent(status.ToString());
            response.Send();
           
        }
    }
}
