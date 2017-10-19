using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FRSServerHttp.Server
{
    /// <summary>
    /// HTTP请求定义
    /// </summary>
    public class HttpRequest : BaseHeader
    {
        /// <summary>
        /// 可接受的内容类型
        /// </summary>
        public string[] AcceptTypes { get; private set; }

        /// <summary>
        /// 可接受的内容编码
        /// </summary>
        public string[] AcceptEncoding { get; private set; }

        /// <summary>
        /// 可接受的内容字符集
        /// </summary>
        public string[] AcceptCharset { get; private set; }

        /// <summary>
        /// 可接受的内容语言
        /// </summary>
        public string[] AcceptLanguage { get; private set; }

        /// <summary>
        /// HTTP授权证书
        /// </summary>
        public string Authorization { get; private set; }

        /// <summary>
        /// If-Match字段
        /// </summary>
        public string IfMatch { get; private set; }

        /// <summary>
        /// IfNoneMatch字段
        /// </summary>
        public string IfNoneMatch { get; private set; }

        /// <summary>
        /// IfModifiedSince字段
        /// </summary>
        public string IfModifiedSince { get; private set; }

        /// <summary>
        /// IfUnmodifiedSince字段
        /// </summary>
        public string IfUnmodifiedSince { get; private set; }

        /// <summary>
        /// IfRange字段
        /// </summary>
        public string IfRange { get; private set; }

        /// <summary>
        /// Range字段
        /// </summary>
        public string Range { get; private set; }

        /// <summary>
        /// Proxy-Authenticate字段
        /// </summary>
        public string ProxyAuthenticate { get; private set; }

        /// <summary>
        /// ProxyAuthorization字段
        /// </summary>
        public string ProxyAuthorization { get; private set; }

        /// <summary>
        /// 客户端指定的主机地址和端口号
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// 客户端指定从跳转到该网站的原始连接
        /// </summary>
        public string Referer { get; private set; }

        /// <summary>
        /// 浏览器标识
        /// </summary>
        public string UserAgent { get; private set; }

        /// <summary>
        /// HTTP请求方式
        /// </summary>
        public string Method { get; private set; }

        /// <summary>
        /// 请求地址
        /// </summary>
        public string URL { get; private set; }

        /// <summary>
        /// Get请求参数
        /// </summary>
        public Dictionary<string, string> GetParams { get; private set; }

        /// <summary>
        /// Post请求参数
        /// </summary>
        public string PostParams { get; private set; }

        const int MAXSIZE = 1024 * 1024 * 2;

        /// <summary>
        /// 定义缓冲区
        /// </summary>
        private byte[] bytes = new byte[MAXSIZE];

        /// <summary>
        /// 客户端请求报文
        /// </summary>
        private string content = "";





        /// <summary>
        /// 是否是静态资源
        /// </summary>
        public bool IsStatic {get ;private set;}
        //http(s)://server.com/{version}/{domain}/{rest-convention}
        public string Version { get; private set; }
        public string Domain { get; private set; }//是一个你可以用来定义任何技术的区域 
        public string RestConvention { get; private set; }//代表这个域(domain)下，约定的rest接口集合。
        public string Operation { get; private set; }//操作。





        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handler">客户端Socket</param>
        public HttpRequest(Stream handler)
        {
            int length = 0;

            do
            {
                //缓存客户端请求报文
                length = handler.Read(bytes, 0, MAXSIZE);
                content += Encoding.UTF8.GetString(bytes, 0, length);
            } while (length == 0);

            if (string.IsNullOrEmpty(content)) return;

            //按行分割请求报文
            string[] lines = content.Split('\n');

            //获取请求方法
            var firstLine = lines[0].Split(' ');

            if (firstLine.Length > 0)
            {
                this.Method = firstLine[0];
            }

            if (firstLine.Length > 1)
            {
                this.URL = Uri.UnescapeDataString(firstLine[1]);
            }

            //获取请求参数
            if (this.Method == "GET" && this.URL.Contains('?'))
            {
                this.GetParams = GetGetRequestParams(URL.Split('?')[1]);
            }
            else if (this.Method == "POST")
            {
                this.PostParams = lines[lines.Length - 1];
            }


            IsStatic = GetRequestType(URL);

            if (!IsStatic)//如果是动态请求
            {
                string[] urls = this.URL.Split('?')[0].Trim(new char[] { '/' }).Split('/');
                if (urls.Length > 0)
                {
                    Version = urls[0];
                }
                if (urls.Length > 1)
                {
                    Domain = urls[1];
                }
                if (urls.Length > 2)
                {
                    RestConvention = urls[2];
                }
                if (urls.Length > 3)
                {
                    Operation = urls[3];
                }
            }


            //获取各种请求报文参数
            this.AcceptTypes = GetKeyValueArrayByKey(content, "Accept");
            this.AcceptCharset = GetKeyValueArrayByKey(content, "Accept-Charset");
            this.AcceptEncoding = GetKeyValueArrayByKey(content, "Accept-Encoding");
            this.AcceptLanguage = GetKeyValueArrayByKey(content, "Accept-Langauge");
            this.Authorization = GetKeyValueByKey(content, "Authorization");
            this.IfMatch = GetKeyValueByKey(content, "If-Match");
            this.IfNoneMatch = GetKeyValueByKey(content, "If-None-Match");
            this.IfModifiedSince = GetKeyValueByKey(content, "If-Modified-Since");
            this.IfUnmodifiedSince = GetKeyValueByKey(content, "If-Unmodified-Since");
            this.IfRange = GetKeyValueByKey(content, "If-Range");
            this.Range = GetKeyValueByKey(content, "Range");
            this.ProxyAuthenticate = GetKeyValueByKey(content, "Proxy-Authenticate");
            this.ProxyAuthorization = GetKeyValueByKey(content, "Proxy-Authorization");
            this.Host = GetKeyValueByKey(content, "Host");
            this.Referer = GetKeyValueByKey(content, "Referer");
            this.UserAgent = GetKeyValueByKey(content, "User-Agent");

            //设置HTTP通用头信息
            this.CacheControl = GetKeyValueByKey(content, "Cache-Control");
            this.Pragma = GetKeyValueByKey(content, "Pragma");
            this.Connection = GetKeyValueByKey(content, "Connection");
            this.Date = GetKeyValueByKey(content, "Date");
            this.TransferEncoding = GetKeyValueByKey(content, "Transfe-Encoding");
            this.Upgrade = GetKeyValueByKey(content, "Upgrade");
            this.Via = GetKeyValueByKey(content, "Via");

            //设置HTTP实体头部信息
            this.Allow = GetKeyValueByKey(content, "Allow");
            this.Location = GetKeyValueByKey(content, "Location");
            this.ContentBase = GetKeyValueByKey(content, "Content-Base");
            this.ContentEncoding = GetKeyValueByKey(content, "Content-Encoidng");
            this.ContentLanguage = GetKeyValueByKey(content, "Content-Language");
            this.ContentLength = GetKeyValueByKey(content, "Content-Length");
            this.ContentLocation = GetKeyValueByKey(content, "Content-Location");
            this.ContentMD5 = GetKeyValueByKey(content, "Content-MD5");
            this.ContentRange = GetKeyValueByKey(content, "Content-Range");
            this.ContentType = GetKeyValueByKey(content, "Content-Type");
            this.Etag = GetKeyValueByKey(content, "Etag");
            this.Expires = GetKeyValueByKey(content, "Expires");
            this.LastModified = GetKeyValueByKey(content, "Last-Modified");
        }

        /// <summary>
        /// 构建请求头部
        /// </summary>
        /// <returns></returns>
        public string BuildHeader()
        {
            return this.content;
        }

        public TValue From<TValue>() where TValue : new()
        {
            return default(TValue);
        }

        /// <summary>
        /// 判断是否是静态资源
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private bool GetRequestType(string URL)
        {
            if (URL.Split('.').Length >1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
