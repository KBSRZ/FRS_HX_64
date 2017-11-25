using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FRSServerHttp.Server;
using DataAngine_Set.BLL;
using FRSServerHttp.Model;
using Newtonsoft.Json;
using FRS;
namespace FRSServerHttp.Service
{


    class PersonDatabaseService : BaseService
    {
        dataset bll = new dataset();
        public static string Domain
        {
            get
            {
                return "person-database";
            }
        }


        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            if (request.RestConvention != null)
            {
                Log.Debug(string.Format("返回ID:{0}人员库", request.RestConvention));

                int id = -1;
                try
                {
                    id = Convert.ToInt32(request.RestConvention);
                }
                catch
                {

                }
                Dataset da = Dataset.CreateInstanceFromDataAngineModel(bll.GetModel(id));
                if (null != da)
                {
                    response.SetContent(da.ToJson());
                }


            }
            else if (request.Domain != string.Empty)
            {

                Log.Debug(string.Format("返回所有库信息"));
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
                Log.Debug("添加一个人员库");

                AddInfo addinfo = AddInfo.CreateInstanceFromJSON(request.PostParams);
                if (addinfo != null)
                {
                    DataAngine_Set.Model.dataset ds = new DataAngine_Set.Model.dataset();
                    ds.datasetname = addinfo.DatasetName;
                    ds.remark = addinfo.Remark;
                    status = bll.Add(ds);
                    if (status)
                    {
                        Log.Debug(string.Format("创建人员库成功"));
                        //初始化
                        //InitFRS();
                        //int num = fa.RegisterInBulk1(addinfo.Path, ds.datasetname);
                        //Log.Debug(string.Format("共注册{0}人", num));
                    }
                }

            }
            else
            {
                if (request.Operation == "update")//更新
                {
                    Log.Debug("更新一个人员库");
                    RegisterInfo registerInfo = RegisterInfo.CreateInstanceFromJSON(request.PostParams);
                    if (registerInfo != null)
                    {
                        int DatasetId = Convert.ToInt32(request.RestConvention);
                        DataAngine_Set.Model.dataset ds = new DataAngine_Set.Model.dataset();
                        ds=bll.GetModel(DatasetId);
                        //初始化                   
                        InitFRS();
                        int num = fa.RegisterInBulk1(registerInfo.Path, ds.datasetname);
                        if (num > 0)
                            status = true;
                        Log.Debug(string.Format("共注册{0}人", num));
                    }

                }
                else if (request.Operation == "delete")//删除
                {
                    Log.Debug("删除更新一个人员库");

                    int id = -1;
                    try
                    {
                        id = Convert.ToInt32(request.RestConvention);
                    }
                    catch
                    {

                    }
                    status = bll.Delete(id);
                    //删除设备
                }
            }
            response.SetContent(status.ToString());
            response.Send();
        }
    }
}
