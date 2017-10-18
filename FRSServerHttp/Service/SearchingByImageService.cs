using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServerHttp.Service
{
    /// <summary>
    /// 以图搜图
    /// </summary>
    class SearchingByImageService : BaseService
    {

        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string URL
        {
            get
            {
                return "/searching-by-image";
            }
        }
        public SearchingByImageService()
        {
            
        }
    }
    
}
