using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FRSServer.Data
{
    /// <summary>
    /// 控制开关
    /// </summary>
    class ControlData
    {
        public string VideoAdress { get; set; }// 通道检索

        public bool Status { get; set; }//true 表示开启，false 表示关闭
    }
}
