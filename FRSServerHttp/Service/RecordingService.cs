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
    class RecordingService : BaseService
    {

        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string Domain
        {
            get
            {
                return "recording";
            }
        }
        public RecordingService()
        {
            
        }
    }
    
}
