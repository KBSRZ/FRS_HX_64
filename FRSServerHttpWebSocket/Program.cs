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

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            Console.WriteLine("CurrentDomain_UnhandledException"+ ex.StackTrace+ex.Message);
            Console.Read();
        }
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Log.Level = LogLevel.Debug;
            HttpServer httpServer;
            if (args.GetLength(0) > 0)
            {
                httpServer = new HttpServerImp(Convert.ToInt16(args[0]), args[1], args[2]);
                
            }
            else
            {
                httpServer = new HttpServerImp(8080, "127.0.0.1", System.Environment.CurrentDirectory);
            }
            httpServer.Start();            
        }
    }
}
