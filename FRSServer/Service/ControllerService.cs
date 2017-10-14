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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="param">ControllingData的Json</param>
        /// <returns></returns>
        protected override int OnSet(string param)
        {
            ControllingData cd = ControllingData.CreateControllingDataFromJSON(param);
            int returnCode = ReturnCode.SUCCESS;
            if (cd.Status)
            {

                if (selectedDevice.VideoAddress == "" || selectedDevice.VideoAddress == null)
                    returnCode=cap.Start(0);
                else
                {
                   returnCode= cap.Start(selectedDevice.VideoAddress);
                }
            }
            else
            {
                cap.Stop();
            }


            if (ReturnCode.SUCCESS == returnCode)
            {
                return ReturnSuccessMessage();
            }
            
                return ReturnFailMessage();
        }
       
    
    }

    
}
