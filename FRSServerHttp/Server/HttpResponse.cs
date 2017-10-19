using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace FRSServerHttp.Server
{


    public class HttpResponse : BaseHeader
    {
        /// <summary>
        /// Age字段
        /// </summary>
        public string Age { get; set; }

        /// <summary>
        /// Sever字段
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Accept-Ranges字段
        /// </summary>
        public string AcceptRanges { get; set; }

        /// <summary>
        /// Vary字段
        /// </summary>
        public string Vary { get; set; }

        /// <summary>
        /// 状态码
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// 响应内容
        /// </summary>
        public byte[] Content { get; private set; }

        /// <summary>
        /// 编码类型
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// 数据流
        /// </summary>
        /// 
        public Stream WriterStream { get { return handler; } }
        private Stream handler;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="stream">数据流</param>
        public HttpResponse(Stream stream)
        {
            this.handler = stream;

            //构造HTTP响应
            this.Server = "FutureHTTP";
            this.StatusCode = "200";
            //初始化响应头部信息
            this.Age = "";
            this.AcceptRanges = "";
            this.Vary = "";

            //设置HTTP通用头信息
            this.CacheControl = "";
            this.Pragma = "";
            this.Connection = "";
            this.Date = "";
            this.TransferEncoding = "";
            this.Upgrade = "";
            this.Via = "";

            //设置HTTP实体头部信息
            this.Allow = "";
            this.Location = "";
            this.ContentBase = "";
            this.ContentEncoding = "";
            this.ContentLanguage = "";
            this.ContentLocation = "";
            this.ContentMD5 = "";
            this.ContentRange = "";
            this.ContentType = "";
            this.Etag = "";
            this.Expires = "";
            this.LastModified = "";
        }

        /// <summary>
        /// 设置响应内容
        /// </summary>
        /// <param name="content">响应内容</param>
        /// <param name="encoding">内容编码</param>
        public HttpResponse SetContent(byte[] content, Encoding encoding = null)
        {
            //初始化内容
            this.Content = content;
            this.Encoding = encoding != null ? encoding : Encoding.UTF8;
            this.ContentLength = Content.Length.ToString();
            return this;
        }

        /// <summary>
        /// 设置响应内容
        /// </summary>
        /// <param name="content">响应内容</param>
        /// <param name="encoding">内容编码</param>
        public HttpResponse SetContent(string content, Encoding encoding = null)
        {
            //初始化内容
            encoding = encoding != null ? encoding : Encoding.UTF8;
            return SetContent(encoding.GetBytes(content), encoding);
        }

        /// <summary>
        /// 构建响应头部
        /// </summary>
        /// <returns></returns>
        protected string BuildHeader()
        {
            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(StatusCode))
                builder.Append("HTTP/1.1 " + StatusCode + "\r\n");

            if (!string.IsNullOrEmpty(Age))
                builder.Append("Age:" + Age + "\r\n");

            if (!string.IsNullOrEmpty(Server))
                builder.Append("Server:" + Server + "\r\n");

            if (!string.IsNullOrEmpty(AcceptRanges))
                builder.Append("Accept-Ranges:" + AcceptRanges + "\r\n");

            if (!string.IsNullOrEmpty(Vary))
                builder.Append("Vary:" + Vary + "\r\n");

            if (!string.IsNullOrEmpty(Vary))
                builder.Append("Vary:" + Vary + "\r\n");

            if (!string.IsNullOrEmpty(CacheControl))
                builder.Append("Cache-Control:" + CacheControl + "\r\n");

            if (!string.IsNullOrEmpty(Pragma))
                builder.Append("Pragma:" + Pragma + "\r\n");

            if (!string.IsNullOrEmpty(Connection))
                builder.Append("Connection:" + Connection + "\r\n");

            if (!string.IsNullOrEmpty(Date))
                builder.Append("Date:" + Date + "\r\n");

            if (!string.IsNullOrEmpty(TransferEncoding))
                builder.Append("Transfer-Encoding:" + TransferEncoding + "\r\n");

            if (!string.IsNullOrEmpty(Upgrade))
                builder.Append("Upgrade:" + Upgrade + "\r\n");

            if (!string.IsNullOrEmpty(Via))
                builder.Append("Via:" + Via + "\r\n");

            if (!string.IsNullOrEmpty(Allow))
                builder.Append("Allow:" + Allow + "\r\n");

            if (!string.IsNullOrEmpty(Location))
                builder.Append("Location:" + Location + "\r\n");

            if (!string.IsNullOrEmpty(ContentBase))
                builder.Append("Content-Base:" + ContentBase + "\r\n");

            if (!string.IsNullOrEmpty(ContentEncoding))
                builder.Append("Content-Encoding:" + ContentEncoding + "\r\n");

            if (!string.IsNullOrEmpty(ContentLength))
                builder.Append("Content-Length:" + ContentLength + "\r\n");

            if (!string.IsNullOrEmpty(ContentLocation))
                builder.Append("Content-Location:" + ContentLocation + "\r\n");

            if (!string.IsNullOrEmpty(ContentMD5))
                builder.Append("Content-MD5:" + ContentMD5 + "\r\n");

            if (!string.IsNullOrEmpty(ContentRange))
                builder.Append("ContentRange:" + ContentRange + "\r\n");

            if (!string.IsNullOrEmpty(ContentType))
                builder.Append("Content-Type:" + ContentType + "\r\n");

            if (!string.IsNullOrEmpty(Etag))
                builder.Append("Etag:" + Etag + "\r\n");

            if (!string.IsNullOrEmpty(Expires))
                builder.Append("Expires:" + Expires + "\r\n");

            if (!string.IsNullOrEmpty(LastModified))
                builder.Append("Last-Modified :" + LastModified + "\r\n");

            return builder.ToString();
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        public void Send()
        {
            if (!handler.CanWrite) return;

            try
            {
                //发送响应头
                var header = BuildHeader();
                byte[] headerBytes = this.Encoding.GetBytes(header);
                handler.Write(headerBytes, 0, headerBytes.Length);

                //发送空行
                byte[] lineBytes = this.Encoding.GetBytes(System.Environment.NewLine);
                handler.Write(lineBytes, 0, lineBytes.Length);

                //发送内容
                handler.Write(Content, 0, Content.Length);
            }
            catch (Exception e)
            {

            }
            finally
            {
                handler.Close();
            }
        }


        /// <summary>
        /// 发送无http头的数据
        /// </summary>
        public void SendOnLongConnetion()
        {
           
            //发送内容
            handler.Write(Content, 0, Content.Length);
            handler.Flush();


        }

    }
}
