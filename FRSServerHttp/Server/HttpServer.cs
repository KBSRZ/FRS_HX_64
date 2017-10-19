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
        /// 是否运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 服务器目录
        /// </summary>
        protected string serverRoot;
        protected int port;
        protected string ip = "127.0.0.1";
        TcpListener serverListener;
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
      


        /// <summary>
        /// 开启服务器
        /// </summary>
        public void Start()
        {
            if (IsRunning) return;

            //创建服务端Socket
            this.serverListener = new TcpListener(IPAddress.Parse(ip), port);
          
            this.IsRunning = true;
            this.serverListener.Start();

            try
            {
                while (IsRunning)
                {
                    TcpClient client = serverListener.AcceptTcpClient();
                    Thread requestThread = new Thread(() => { ProcessRequest(client); });
                    requestThread.Start();
                }
            }
            catch (Exception e)
            {
               Console.WriteLine(e.Message);
            }
        }
        /// <summary>
        /// 处理客户端请求
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        private void ProcessRequest(TcpClient handler)
        {
            //处理请求
            Stream clientStream = handler.GetStream();

            if (clientStream == null) return;

            //构造HTTP请求
            HttpRequest request = new HttpRequest(clientStream);
            

            //构造HTTP响应
            HttpResponse response = new HttpResponse(clientStream);
          

            //处理请求类型
            switch (request.Method)
            {
                case "GET":
                    OnGet(request, response);
                    break;
                case "POST":
                    OnPost(request, response);
                    break;
                default:
                    OnDefault(request, response);
                    break;
            }
        }



        #region 虚方法

        /// <summary>
        /// 响应Get请求
        /// </summary>
        /// <param name="request">请求报文</param>
        public virtual void OnGet(HttpRequest request, HttpResponse response)
        {

        }

        /// <summary>
        /// 响应Post请求
        /// </summary>
        /// <param name="request"></param>
        public virtual void OnPost(HttpRequest request, HttpResponse response)
        {

        }

        /// <summary>
        /// 响应默认请求
        /// </summary>

        public virtual void OnDefault(HttpRequest request, HttpResponse response)
        {

        }

        #endregion
    }

}
