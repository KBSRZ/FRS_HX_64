//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using System.Threading;
//using System.Text;
//namespace FRSServerHttp.Server
//{
//    public class HttpProcessor
//    {
      
//        public TcpClient socket;
//        public HttpServer srv;

//        private Stream inputStream;
//        public StreamWriter outputStream;

//        public String httpMethod;
//        public String httpUrl;
//        public String httpProtocolVersionString;
//        public Hashtable httpHeaders = new Hashtable();
//        /// <summary>
//        /// 是否是静态资源
//        /// </summary>
//        public bool isStatic = false;
//       //http(s)://server.com/{version}/{domain}/{rest-convention}
//        public string version=string.Empty;//version 
//        public string domain = string.Empty;//是一个你可以用来定义任何技术的区域 
//        public string restConvention = string.Empty;//代表这个域(domain)下，约定的rest接口集合。
//        public string operation = string.Empty;//操作。
//        /// <summary>
//        /// 请求参数
//        /// </summary>
//        public Dictionary<string, string> Params { get; private set; }


//        private static int MAX_POST_SIZE = 10 * 1024 * 1024; // 10MB

//        public HttpProcessor(TcpClient s, HttpServer srv)
//        {
//            this.socket = s;
//            this.srv = srv;
//        }


//        private string StreamReadLine(Stream inputStream)
//        {
//            int next_char;
//            string data = "";
//            while (true)
//            {
//                next_char = inputStream.ReadByte();
//                if (next_char == '\n') { break; }
//                if (next_char == '\r') { continue; }
//                if (next_char == -1) { Thread.Sleep(1); continue; };
//                data += Convert.ToChar(next_char);
//            }
//            return data;
//        }




//        public void Process()
//        {
//            //处理请求
//            Stream clientStream = socket.GetStream();
           
//            if (clientStream == null) return;

//            //构造HTTP请求
//            HttpRequest request = new HttpRequest(clientStream);
            

//            //构造HTTP响应
//            HttpResponse response = new HttpResponse(clientStream);


//            try
//            {
//                ParseRequest();
//                ReadHeaders();


//                if (httpMethod.Equals("GET"))
//                {

//                    if (this.httpUrl.Contains("?"))
//                    {
//                        this.Params = GetRequestParams(this.httpUrl.Split('?')[1]);
//                        this.httpUrl = this.httpUrl.Split('?')[0];

//                    }
//                    if (!isStatic)//如果是动态请求
//                    {
//                        string[] urls = this.httpUrl.Trim(new char[] { '/' }).Split('/');
//                        if (urls.Length > 0)
//                        {
//                            version = urls[0];
//                        }
//                        if (urls.Length > 1)
//                        {
//                            domain = urls[1];
//                        }
//                        if (urls.Length > 2)
//                        {
//                            restConvention = urls[2];
//                        }
//                        if (urls.Length > 3)
//                        {
//                            operation = urls[3];
//                        }
//                    }
//                    HandleGETRequest();



//                }
//                else if (httpMethod.Equals("POST"))
//                {

//                    string[] urls = this.httpUrl.Trim(new char[] { '/' }).Split('/');
//                    if (!isStatic)//如果是动态请求
//                    {
//                        if (urls.Length > 0)
//                        {
//                            version = urls[0];
//                        }
//                        if (urls.Length > 1)
//                        {
//                            domain = urls[1];
//                        }
//                        if (urls.Length > 2)
//                        {
//                            restConvention = urls[2];
//                        }
//                        if (urls.Length > 3)
//                        {
//                            operation = urls[3];
//                        }

//                    }
//                    HandlePOSTRequest();
//                    //this.Params = GetRequestParams(lines[lines.Length - 1]);
//                }
//            }
//            catch (Exception e)
//            {

//                Console.WriteLine("Exception: " + e.ToString());
//                WriteFailure();
//            }
//            if (socket.Connected)
//                outputStream.Flush();
//            // bs.Flush(); // flush any remaining output
//            inputStream = null;
//            outputStream = null; // bs = null;            
//            socket.Close();
//        }










        
//        //public void Process()
//        //{






//        //    // we can't use a StreamReader for input, because it buffers up extra data on us inside it's
//        //    // "processed" view of the world, and we want the data raw after the headers
//        //    inputStream = new BufferedStream(socket.GetStream());


//        //    // we probably shouldn't be using a streamwriter for all output from handlers either
//        //    outputStream = new StreamWriter(new BufferedStream(socket.GetStream()));
//        //    try
//        //    {
//        //        ParseRequest();
//        //        ReadHeaders();


//        //        if (httpMethod.Equals("GET"))
//        //        {
                    
//        //            if (this.httpUrl.Contains("?"))
//        //            {
//        //                this.Params = GetRequestParams(this.httpUrl.Split('?')[1]);
//        //                this.httpUrl = this.httpUrl.Split('?')[0];
                       
//        //            }
//        //            if (!isStatic)//如果是动态请求
//        //            {
//        //                string[] urls = this.httpUrl.Trim(new char[] { '/' }).Split('/');
//        //                if (urls.Length > 0)
//        //                {
//        //                    version = urls[0];
//        //                }
//        //                if (urls.Length > 1)
//        //                {
//        //                    domain = urls[1];
//        //                }
//        //                if (urls.Length > 2)
//        //                {
//        //                    restConvention = urls[2];
//        //                }
//        //                if (urls.Length > 3)
//        //                {
//        //                    operation = urls[3];
//        //                }
//        //            }
//        //            HandleGETRequest();

                    
                    
//        //        }
//        //        else if (httpMethod.Equals("POST"))
//        //        {

//        //            string[] urls = this.httpUrl.Trim(new char[] { '/' }).Split('/');
//        //            if (!isStatic)//如果是动态请求
//        //            {
//        //                if (urls.Length > 0)
//        //                {
//        //                    version = urls[0];
//        //                }
//        //                if (urls.Length > 1)
//        //                {
//        //                    domain = urls[1];
//        //                }
//        //                if (urls.Length > 2)
//        //                {
//        //                    restConvention = urls[2];
//        //                }
//        //                if (urls.Length > 3)
//        //                {
//        //                    operation = urls[3];
//        //                }

//        //            }
//        //            HandlePOSTRequest();
//        //            //this.Params = GetRequestParams(lines[lines.Length - 1]);
//        //        }
//        //    }
//        //    catch (Exception e)
//        //    {
                
//        //        Console.WriteLine("Exception: " + e.ToString());
//        //        WriteFailure();
//        //    }
//        //    if (socket.Connected) 
//        //    outputStream.Flush();
//        //    // bs.Flush(); // flush any remaining output
//        //    inputStream = null; 
//        //    outputStream = null; // bs = null;            
//        //    socket.Close();
//        //}

//        private bool  GetRequestTpe(string httpUrl){
//            return File.Exists(Path.Combine(srv.ServerRoot,httpUrl));
//        }
//        public void ParseRequest()
//        {
//            String request = StreamReadLine(inputStream);
//            string[] tokens = request.Split(' ');
//            if (tokens.Length != 3)
//            {
//                throw new Exception("invalid http request line");
//            }
//            httpMethod = tokens[0].ToUpper();
//            httpUrl = tokens[1];

//            this.isStatic = GetRequestTpe(httpUrl);

//            httpProtocolVersionString = tokens[2];
//            Console.WriteLine("starting: " + request);
            
//        }


//        public void ReadHeaders()
//        {
//            //Console.WriteLine("ReadHeaders()");
//            String line;
//            while ((line = StreamReadLine(inputStream)) != null)
//            {
//                if (line.Equals(""))
//                {
//                    Console.WriteLine("got headers");
                    
//                    return;
//                }

//                int separator = line.IndexOf(':');
//                if (separator == -1)
//                {
//                    throw new Exception("invalid http header line: " + line);
//                }
//                String name = line.Substring(0, separator);
//                int pos = separator + 1;
//                while ((pos < line.Length) && (line[pos] == ' '))
//                {
//                    pos++; // strip any spaces
//                }

//                string value = line.Substring(pos, line.Length - pos);

//                Console.WriteLine(string.Format("header: {0}:{1}", name, value));
                
//                httpHeaders[name] = value;
//            }
//        }

//        public void HandleGETRequest()
//        {
//            srv.HandleGETRequest(this);
//        }

//        private const int BUF_SIZE = 4096;
//        public void HandlePOSTRequest()
//        {
//            // this post data processing just reads everything into a memory stream.
//            // this is fine for smallish things, but for large stuff we should really
//            // hand an input stream to the request processor. However, the input stream 
//            // we hand him needs to let him see the "end of the stream" at this content 
//            // length, because otherwise he won't know when he's seen it all! 

           
//            int content_len = 0;
//            MemoryStream ms = new MemoryStream();
//            if (this.httpHeaders.ContainsKey("Content-Length"))
//            {
//                content_len = Convert.ToInt32(this.httpHeaders["Content-Length"]);
//                if (content_len > MAX_POST_SIZE)
//                {
//                    throw new Exception(
//                        String.Format("POST Content-Length({0}) too big for this simple server",
//                          content_len));
//                }
//                byte[] buf = new byte[BUF_SIZE];
//                int to_read = content_len;
//                while (to_read > 0)
//                {
//                    //Console.WriteLine("starting Read, to_read={0}", to_read);

//                    int numread = this.inputStream.Read(buf, 0, Math.Min(BUF_SIZE, to_read));
//                   // Console.WriteLine("read finished, numread={0}", numread);
//                    if (numread == 0)
//                    {
//                        if (to_read == 0)
//                        {
//                            break;
//                        }
//                        else
//                        {
//                            throw new Exception("client disconnected during post");
//                        }
//                    }
//                    to_read -= numread;
//                    ms.Write(buf, 0, numread);
//                }
//                ms.Seek(0, SeekOrigin.Begin);
//            }
//            //Console.WriteLine("get post data end");
//            srv.HandlePOSTRequest(this, new StreamReader(ms));

//        }
//        /// <summary>
//        /// 从内容中解析请求参数并返回一个字典
//        /// </summary>
//        /// <param name="content">使用&连接的参数字符串</param>
//        /// <returns>如果存在参数则返回参数否则返回null</returns>
//        protected Dictionary<string, string> GetRequestParams(string content)
//        {
//            //防御编程
//            if (string.IsNullOrEmpty(content))
//                return null;

//            //按照&对字符进行分割
//            string[] reval = content.Split('&');
//            if (reval.Length <= 0)
//                return null;

//            //将结果添加至字典
//            Dictionary<string, string> dict = new Dictionary<string, string>();
//            foreach (string val in reval)
//            {
//                string[] kv = val.Split('=');
//                if (kv.Length <= 1)
//                    dict.Add(kv[0], "");
//                dict.Add(kv[0], kv[1]);
//            }

//            //返回字典
//            return dict;
//        }
       
//        public void WriteSuccess()
//        {
//            outputStream.WriteLine("HTTP/1.0 200 OK");
//            outputStream.WriteLine("Content-Type: text/html");
//            outputStream.WriteLine("Connection: close");
//            outputStream.WriteLine("");
//        }

//        public void WriteFailure()
//        {
//            outputStream.WriteLine("HTTP/1.0 404 File not found");
//            outputStream.WriteLine("Connection: close");
//            outputStream.WriteLine("");
//        }
//    }
//}
