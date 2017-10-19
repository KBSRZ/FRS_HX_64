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
    class DefualtService:BaseService
    {
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnGet(HttpRequest request, HttpResponse response) 
        {
            //当文件不存在时应返回404状态码
            //string requestFile = Path.Combine(p.srv.ServerRoot, requestURL);

            ////判断地址中是否存在扩展名
            //string extension = Path.GetExtension(requestFile);

            ////根据有无扩展名按照两种不同链接进行处
            //if (extension != "")
            //{
            //    //从文件中返回HTTP响应
            //    response = LoadFromFile(response, requestFile);
            //}
            //else
            //{
            //    //目录存在且不存在index页面时时列举目录
            //    if (Directory.Exists(requestFile) && !File.Exists(requestFile + "\\index.html"))
            //    {
            //        requestFile = Path.Combine(p.ToString.ServerRoot, requestFile);
            //        var content = ListDirectory(requestFile, requestURL);
            //        response = response.SetContent(content, Encoding.UTF8);
            //        response.Content_Type = "text/html; charset=UTF-8";
            //    }
            //    else
            //    {
            //        //加载静态HTML页面
            //        requestFile = Path.Combine(requestFile, "index.html");
            //        response = LoadFromFile(response, requestFile);
            //        response.Content_Type = "text/html; charset=UTF-8";
            //    }
            //}
        }
        /// <summary>
        /// Get时调用
        /// </summary>
        public override void OnPost(HttpRequest request, HttpResponse response) { }
    }
}
