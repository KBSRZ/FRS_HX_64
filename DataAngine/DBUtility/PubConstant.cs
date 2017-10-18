﻿using System;
using System.Configuration;
namespace DataAngine.DBUtility
{
    
    public class PubConstant
    {        
        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionString
        {           
            get 
            {
                try
                {
                    string _connectionString = ConfigurationManager.AppSettings["ConnectionStringMySQL"];
                    string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                    if (ConStringEncrypt == "true")
                    {
                        _connectionString = DESEncrypt.Decrypt(_connectionString);
                    }
                    if (_connectionString == null || _connectionString==string.Empty)
                         return "server=127.0.0.1;database=frsdb;uid=root;pwd=123456";
                    return _connectionString;
                }
                catch {
                    return "server=127.0.0.1;database=frsdb;uid=root;pwd=123456";
                }
                
            }
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string ConnectionTableString
        {
            get
            {
                try
                {
                    string _connectionTableString = ConfigurationManager.AppSettings["ConnectionTableStringMySQL"];
                    string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                    if (ConStringEncrypt == "true")
                    {
                        _connectionTableString = DESEncrypt.Decrypt(_connectionTableString);
                    }
                    if (_connectionTableString == null || _connectionTableString == string.Empty)
                        return "server=127.0.0.1;database=frs_database_set;uid=root;pwd=123456";
                    return _connectionTableString;
                }
                catch
                {
                    return "server=127.0.0.1;database=frsdb;uid=root;pwd=123456";
                }

            }
        }

        /// <summary>
        /// 得到web.config里配置项的数据库连接字符串。
        /// </summary>
        /// <param name="configName"></param>
        /// <returns></returns>
        public static string GetConnectionString(string configName)
        {
            try
            {
                string connectionString = ConfigurationManager.AppSettings[configName];
                string ConStringEncrypt = ConfigurationManager.AppSettings["ConStringEncrypt"];
                if (ConStringEncrypt == "true")
                {
                    connectionString = DESEncrypt.Decrypt(connectionString);
                }
                if (connectionString == null || connectionString == string.Empty)
                    return "server=127.0.0.1;database=frsdb;uid=root;pwd=123456";
                return connectionString;
            }
            catch
            {
                return "server=127.0.0.1;database=frsdb;uid=root;pwd=123456";
            }
           
        }


    }
}
