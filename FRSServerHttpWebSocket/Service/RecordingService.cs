using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRSServerHttp.Model;
using System.IO;
using Newtonsoft.Json;
using FRSServerHttp.Server;
using DataAngine.BLL;

namespace FRSServerHttp.Service
{
    /// <summary>
    /// 以图搜图
    /// </summary>
    class RecordingService : BaseService
    {

        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        hitalert bll = new hitalert();
        public static string Domain
        {
            get
            {
                return "recording";
            }
        }
        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            if (request.RestConvention != null)//根据ID获得数据库
            {
                Log.Debug(string.Format("返回数据库{0}的信息", request.RestConvention));
                string library = "frsdb";
                try
                {
                    library = request.RestConvention;
                }
                catch
                {
                }

                if(request.PostParams!=null)
                {
                    DateTime starttime=new DateTime(); 
                    DateTime endtime=new DateTime();
                    int startindex = 0;
                    int pagesize = 30;
                    starttime = Convert.ToDateTime(request.GetParams["starttime"]);
                    endtime = Convert.ToDateTime(request.GetParams["endtime"]);
                    startindex = Convert.ToInt32(request.GetParams["startindex"]);
                    pagesize = Convert.ToInt32(request.GetParams["pagesize"]);
                    HitAlertData[] ha = HitAlertData.CreateInstanceFromDataAngineDataSet(bll.GetListByTime(starttime, endtime, startindex, pagesize, library));
                     response.SetContent(JsonConvert.SerializeObject(ha));
                   
                }          
            }           
            response.Send();
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response)
        {         

        }
       
    }
    
}
