using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServerHttp.Service
{
    /// <summary>
    /// 注册
    /// </summary>
    class RegisterService : BaseService
    {
        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public static string URL
        {
            get
            {
                return "/register";
            }
        }

        public RegisterService()
        {
           
        }
       
    }
}
