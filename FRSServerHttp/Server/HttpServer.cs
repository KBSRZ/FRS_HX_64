using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FRSServerHttp.Server
{
    public abstract class HttpServer
    {

        public string  ServerRoot{get{return serverRoot;  }}
        public int Port { get { return port; } }
        public string IP { get { return ip; } }

        /// <summary>
        /// 服务器目录
        /// </summary>
        protected string serverRoot;
        protected int port;
        protected string ip = "127.0.0.1";
        TcpListener listener;
        bool is_active = true;
        //用于线程同步，初始状态设为非终止状态，使用手动重置方式
        private EventWaitHandle allDone = new EventWaitHandle(false, EventResetMode.ManualReset);
        public HttpServer(int port, string ip = "127.0.0.1", string root =".")
        {
            this.port = port;
            this.ip = ip;
            if (!Directory.Exists(root))
                this.serverRoot = AppDomain.CurrentDomain.BaseDirectory;
            this.serverRoot = root;
        }
        void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
           

            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);
            HttpProcessor processor = new HttpProcessor(client, this);
            Thread thread = new Thread(new ThreadStart(processor.Process));
            thread.Start();
            Console.WriteLine("-------------------------------------------");
            //将事件状态设置为终止状态，允许一个或多个等待线程继续
            allDone.Set();
        }
        public void Listen()
        {
            IPAddress localaddr = IPAddress.Parse(ip);
            listener = new TcpListener(localaddr, port);
            listener.Start();
            while (is_active)
            {
                //TcpClient s = listener.AcceptTcpClient();
                //HttpProcessor processor = new HttpProcessor(s, this);
                //Thread thread = new Thread(new ThreadStart(processor.process));
                //thread.Start();

                //将事件的状态设置为非终止,非终止状态 WaitOne 将会阻塞
                allDone.Reset();
                listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback),listener);
                allDone.WaitOne();
                
            }
        }


      
        public abstract void HandleGETRequest(HttpProcessor p);
        public abstract void HandlePOSTRequest(HttpProcessor p, StreamReader inputData);
    }

}
