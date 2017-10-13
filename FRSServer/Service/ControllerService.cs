using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Newtonsoft.Json;
using FRSServer.Data;
namespace FRSServer.Service
{
    /// <summary>
    /// 控制服务
    /// OnMessage 进行主要的设置
    /// </summary>
    class ControllerService : BaseService
    {



        public ControllerService()
        {
            // TODO: Complete member initialization
            url = "/controlling";
        }
       
    }

    
}
