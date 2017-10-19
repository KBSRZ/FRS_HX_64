using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FRSServerHttp.Server;
namespace FRSServerHttp.Service
{
    /// <summary>
    /// 默认的Service用于文件资源的传输
    /// </summary>
    class FileService:BaseService
    {

        private string serverRoot;
        public FileService(string serverRoot)
        {
            this.serverRoot = serverRoot;
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnGet(HttpRequest request, HttpResponse response) 
        {
            //当文件不存在时应返回404状态码

            string requestURL = request.URL;
            requestURL = requestURL.Replace("/", @"\").Replace("\\..", "").TrimStart('\\');
            string requestFile = Path.Combine(serverRoot, requestURL);

            //判断地址中是否存在扩展名
            string extension = Path.GetExtension(requestFile);

            //根据有无扩展名按照两种不同链接进行处
            if (extension != "")
            {
                //从文件中返回HTTP响应
                response = LoadFromFile(response, requestFile);
            }
            else
            {
                //目录存在且不存在index页面时时列举目录
                if (Directory.Exists(requestFile) && !File.Exists(requestFile + "\\index.html"))
                {
                    requestFile = Path.Combine(serverRoot, requestFile);
                    var content = ListDirectory(requestFile, requestURL);
                    response = response.SetContent(content, Encoding.UTF8);
                    response.ContentType = "text/html; charset=UTF-8";
                }
                else
                {
                    //加载静态HTML页面
                    requestFile = Path.Combine(requestFile, "index.html");
                    response = LoadFromFile(response, requestFile);
                    response.ContentType = "text/html; charset=UTF-8";
                }
            }
            response.Send();
        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response) { }



        /// <summary>
        /// 从文件返回一个HTTP响应
        /// </summary>
        /// <param name="fileName">文件名</param>
        private HttpResponse LoadFromFile(HttpResponse response, string fileName)
        {
            //获取文件扩展名以判断内容类型
            string extension = Path.GetExtension(fileName);

            //获取当前内容类型
            //string contentType = GetContentType(extension);
            string contentType = "";

            //如果文件不存在则返回404否则读取文件内容
            if (!File.Exists(fileName))
            {
                response.SetContent("<html><body><h1>404 - Not Found</h1></body></html>");
                response.StatusCode = "404";
                response.ContentType = "text/html";
                response.Server = "ExampleServer";
            }
            else
            {
                response.SetContent(File.ReadAllBytes(fileName));
                response.StatusCode = "200";
                response.ContentType = contentType;
                response.Server = "ExampleServer";
            }

            //返回数据
            return response;
        }

        private string ConvertPath(string[] urls)
        {
            string html = string.Empty;
            int length = serverRoot.Length;
            foreach (var url in urls)
            {
                var s = url.StartsWith("..") ? url : url.Substring(length).TrimEnd('\\');
                html += String.Format("<li><a href=\"{0}\">{0}</a></li>", s);
            }

            return html;
        }

        private string ListDirectory(string requestDirectory, string requestURL)
        {
            //列举子目录
            var folders = requestURL.Length > 1 ? new string[] { "../" } : new string[] { };
            folders = folders.Concat(Directory.GetDirectories(requestDirectory)).ToArray();
            var foldersList = ConvertPath(folders);

            //列举文件
            var files = Directory.GetFiles(requestDirectory);
            var filesList = ConvertPath(files);

            //构造HTML
            StringBuilder builder = new StringBuilder();
            builder.Append(string.Format("<html><head><title>{0}</title></head>", requestDirectory));
            builder.Append(string.Format("<body><h1>{0}</h1><br/><ul>{1}{2}</ul></body></html>",
                 requestURL, filesList, foldersList));

            return builder.ToString();
        }
    }
}
