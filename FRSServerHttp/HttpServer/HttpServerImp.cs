using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using FRSServerHttp.Service;
namespace FRSServerHttp
{
    public class HttpServerImp : HttpServer
    {
        private BaseService service = null; 
        public HttpServerImp(int port)
            : base(port)
        {
        }
        public override void HandleGETRequest(HttpProcessor p)
        {
            Console.WriteLine("request: {0}", p.httpUrl);





            service = ServiceHelper.GetService(p.httpUrl);
            service.OnGet(p);

            //int i = 0;
          
            //    p.outputStream.WriteLine("<html><body><h1>" + i + "</h1>");
            //    p.outputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
            //    p.outputStream.WriteLine("</body></html>");
            //    p.outputStream.Flush();
            //    Thread.Sleep(10000);
            //    i++;
            
            
            
        }

        public override void HandlePOSTRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.httpUrl);
            string data = inputData.ReadToEnd();

            p.outputStream.WriteLine("<html><body><h1>test server</h1>");
            p.outputStream.WriteLine("<a href=/test>return</a><p>");
            p.outputStream.WriteLine("postbody: <pre>{0}</pre>", data);


        }
    }
}
