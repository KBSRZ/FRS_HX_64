using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using FRSServerHttp.Server;
namespace FRSServerHttp
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer httpServer;
            if (args.GetLength(0) > 0)
            {
                httpServer = new HttpServerImp(Convert.ToInt16(args[0]));
                
            }
            else
            {
                httpServer = new HttpServerImp(8080,"127.0.0.1","D:\\");
            }
            httpServer.Start();
           
            
        }

    }
}
