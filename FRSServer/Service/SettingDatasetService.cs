using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAngine;
using DataAgine_Set;
using Newtonsoft.Json;
using FRSServer.Data;

namespace FRSServer.Service
{

    class SettingDatasetService : BaseService
    {

        readonly DataAngine.BLL.hitalert hitbll = new DataAngine.BLL.hitalert();
        readonly DataAngine.BLL.user user = new DataAngine.BLL.user();
        readonly DataAgine_Set.BLL.frs_database frs_database = new DataAgine_Set.BLL.frs_database();
        readonly DataAgine_Set.BLL.device device = new DataAgine_Set.BLL.device();
        DatasetData datasetData = new DatasetData();
        public SettingDatasetService()
        {
            url = "/setting-dataset";
            DataSet ds = frs_database.GetAllList();
            DataTable dt = ds.Tables[0];

            datasetData.DatasetNames = (from r in dt.AsEnumerable() select r.Field<string>("name")).ToArray<string>();



        }

        /// <summary>
        /// 获得数据库的初始状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        protected override int OnRead(string param)
        {
            Console.WriteLine("SettingDatasetService::OnRead");
            if (null != socket && socket.IsAvailable)
            {
                Console.WriteLine(datasetData.ToJson());
                socket.Send(new Message(Message.MessageType.READ, datasetData.ToJson()).ToJson());
            }
            return ReturnCode.SUCCESS;
        }
        // /// <summary>
        // /// 增加一个数据库
        // /// </summary>
        // /// <param name="param">数据库的名字</param>
        // /// <returns></returns>
        //protected override int OnAdd(string param)
        //{
        //    Console.WriteLine("SettingVideoAddressService::OnAdd");
        //    DataAngine.Model.table table = new DataAngine.Model.table();
        //    DataAngine.BLL.table tablebll = new DataAngine.BLL.table();
        //    table.name = param;

        //    if (true == tablebll.Add(table))
        //    {
        //        return ReturnSuccessMessage();
        //    }
        //    return ReturnFailMessage();
        //}

        /// <summary>
        /// 设置 选择哪个数据库
        /// </summary>
        /// <param name="param">DatasetName</param>
        /// <returns></returns>
        protected override int OnSet(string param)
        {
            Console.WriteLine("SettingDatasetService::OnSet");


            if (ReturnCode.SUCCESS == fa.LoadData(param))
            {
                DatasetData.SaveSelectedDataSetName(param);
                return ReturnSuccessMessage();
            }
            else
            {
                return ReturnFailMessage();
            }
        }


    }

}

