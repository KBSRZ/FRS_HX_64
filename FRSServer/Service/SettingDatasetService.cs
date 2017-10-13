using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DataAngine;
using Newtonsoft.Json;
namespace FRSServer.Service
{
    
    class SettingDatasetService:BaseService
    {

        readonly DataAngine.BLL.hitalert hitbll = new DataAngine.BLL.hitalert();
        readonly DataAngine.BLL.user user = new DataAngine.BLL.user();
        readonly DataAngine.BLL.table table = new DataAngine.BLL.table();
        readonly DataAngine.BLL.device device = new DataAngine.BLL.device();
        string[] databaseNames=null;
        public SettingDatasetService()
        {
            url = "setting-dataset";
            DataSet ds = table.GetAllTable();
            DataTable dt = ds.Tables[0];

            databaseNames = (from r in dt.AsEnumerable() select r.Field<string>("name")).ToArray<string>();
        }

       protected override int  OnRead(string param){
           Console.WriteLine("SettingVideoAddressService::OnRead");
           if (null != socket && socket.IsAvailable)
           {
               socket.Send(new Message(Message.MessageType.READ, JsonConvert.SerializeObject(databaseNames)).ToJson());
           }
           return ReturnCode.SUCCESS;
       }
       protected override int OnAdd(string param)
       {
           Console.WriteLine("SettingVideoAddressService::OnAdd");
           DataAngine.Model.table table = new DataAngine.Model.table();
           DataAngine.BLL.table tablebll = new DataAngine.BLL.table();
           table.name = param;

           if (true == tablebll.Add(table))
           {
               return ReturnSuccessMessage();
           }
           return ReturnFailMessage();
       }
       protected override int OnSet(string param)
       {
           Console.WriteLine("SettingVideoAddressService::OnSet");
           return  fa.LoadData(param);
       }

    }
}
