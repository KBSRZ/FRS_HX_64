using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServerHttp.Service
{
    /// <summary>
    /// 按照时间搜索
    /// </summary>
    class SearchingByTimeService : BaseService
    {

        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string URL
        {
            get
            {
                return "/searching-by-time";
            }
        }
        public SearchingByTimeService()
        {
           
        }
       
    }
}
