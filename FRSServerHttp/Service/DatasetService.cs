﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRSServerHttp.Model;
using System.IO;
using FRSServerHttp.Server;
namespace FRSServerHttp.Service
{
    class DatasetService:BaseService
    {
        public static string Domain
        {
            get
            {
                return "dataset";
            }
        }

        public override void OnGet(HttpProcessor p)
        {
            if (p.restConvention != string.Empty)//根据ID获得数据库
            {

                Console.WriteLine("返回ID{0}的数据库信息", p.restConvention);
                Dataset d = new Dataset();
                d.ID = Int32.Parse(p.restConvention);
                d.Password = "1231";
                d.Port = "664";
                d.IP = "127.0.0.1";
                d.User = "sa";
                d.Type = "mysql";
                p.outputStream.Write(d.ToJson());
                

            }
            else if (p.domain!=null)//获得所有数据库
            {
                Console.WriteLine("返回所有数据库信息");
            }
        }
        /// <summary>
        /// Post时调用
        /// </summary>
        public override void OnPost(HttpProcessor p, StreamReader inputData) {

            if (p.operation == string.Empty)//添加一条数据
            {
                Dataset device = Dataset.CreateDatasetFromJSON(inputData.ReadToEnd());
                if (null != device)
                {
                    //添加到数据库
                    Console.WriteLine("添加数据库信息");
                }
            }
         else
            {
                if (p.operation == "update")//更新
                {
                    Console.WriteLine("更新数据库信息");
                    Dataset device = Dataset.CreateDatasetFromJSON(inputData.ReadToEnd());
                }
                else if (p.operation == "delete")//删除
                {

                }
            }
           
        }
    }
}
