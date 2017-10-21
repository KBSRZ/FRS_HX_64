using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using FRSServerHttp.Service;
namespace FRSServerHttp.Server
{
    public class HttpServerImp : HttpServer
    {


       
        private BaseService service = null; 
        public HttpServerImp(int port,string  ip= "127.0.0.1",string root=".")
            :  base( port,  ip ,  root )
       
        {
        }
        public override void OnGet(HttpRequest request, HttpResponse response)
        {
            Console.WriteLine("request: {0}", request.URL);
            if (!request.IsStatic)
            {
                service = ServiceHelper.GetService(request.Domain);
            }
            else
            {
              service= new  FileService(serverRoot);
            }
            if (null != service)
                service.OnGet( request, response);


        }
        
        public override void OnPost(HttpRequest request, HttpResponse response)
        {
            Console.WriteLine("POST request: {0}", request.URL);


            if (!request.IsStatic)
            {
                service = ServiceHelper.GetService(request.Domain);
            }
            else
            {
                service = new FileService(serverRoot);
            }
            if(null!=service)
                service.OnPost( request,  response);


        }
        
       
    }
}
