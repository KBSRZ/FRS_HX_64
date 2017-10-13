using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace FRSServer
{


    
        
    class Message
    {
        public static class MessageType
        {
            /// <summary>
            /// 增
            /// </summary>
            public  const string ADD = "ADD";
            /// <summary>
            /// 删
            /// </summary>
            public  const string DELETE = "DELETE";
            /// <summary>
            /// 改
            /// </summary>
            public  const string UPDATE = "UPDATE";
            /// <summary>
            /// 读
            /// </summary>
            public  const string READ = "READ";

            /// <summary>
            /// 用于返回
            /// </summary>
            public  const string RETURN = "RETURN";
        }

        public Message(string messageType)
        {
            this.messageType = messageType;
        }
        //必须有用于json序列化
        public Message()
        {
           
        }
        /// <summary>
        /// 增:"ADD";
        /// 删:"DELETE"
        /// 改:"UPDATE"
        /// 读:"READ"
        /// 用于返回:"RETURN"
        /// </summary>
        public string Type
        {
            get { return messageType; }
            set { messageType = value; }
        }
        protected  string messageType = string.Empty;

        public string Content
        {
            get { return content; }
            set { content = value; }
        }
        protected string content = string.Empty;

        public Message(string messageType,string content){
            this.messageType = messageType;
            this.content = content;
        }
        public string ToJson()
        {
           return  JsonConvert.SerializeObject(this);
        }
        public static Message CreateMessageFromJSON (string json){
            Message msg=null;
            try
            {
               
                msg = (Message)JsonConvert.DeserializeObject(json, typeof(Message));
               
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message +e.StackTrace);
            }
            return msg;
        }
    }
}
