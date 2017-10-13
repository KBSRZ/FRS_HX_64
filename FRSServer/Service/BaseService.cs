﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Fleck;
using FRS;
using Newtonsoft.Json;
namespace FRSServer.Service
{
   
  
    abstract class BaseService
    {
        public static FeatureData fa;//数据操作类
        public static Capture cap;//视频处理类
        /// <summary>
        /// 访问当前service的URL
        /// </summary>
        public  string URL
        {
            get
            {
                return url;
            }
        }
        protected   string url=string.Empty;
        public BaseService() {  }
        public IWebSocketConnection Socket { get { return socket; } set { socket = value; } }
        protected IWebSocketConnection socket;
        /// <summary>
        /// 在webscoket OnOpen时使用
        /// </summary>
        public virtual void OnOpen() { }
        public virtual void OnClose() { }
        /// <summary>
        /// 在webscoket OnMessage时使用
        /// </summary>
        /// <param name="param">参数为json</param>
        public  int OnMessage(string param)
        {
            try
            {
                Message message = Message.CreateMessageFromJSON(param);
                if (message.Type == Message.MessageType.ADD)
                {
                    return OnAdd(message.Content);
                }
                else if (message.Type == Message.MessageType.DELETE)
                {
                    return OnDelete(message.Content);
                }
                else if (message.Type == Message.MessageType.READ)
                {
                    return OnRead(message.Content);
                }
                else if (message.Type == Message.MessageType.UPDATE)
                {
                    return OnUpdate(message.Content);
                }
                else if (message.Type == Message.MessageType.SET)
                {
                    return OnSet(message.Content);
                }
                else
                {
                    if (null != socket && socket.IsAvailable)
                    {
                        socket.Send(new Message(Message.MessageType.RETURN, PubConstant.ILLEGAL_URL).ToJson());
                    }
                    return ReturnCode.FAIL;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace + e.Message);
            }
            return ReturnFailMessage();
        }



        /// <summary>
        /// 在接收到类型为　UPDATE 的消息时调用
        /// </summary>
        /// <returns></returns>
        protected virtual int OnUpdate(string param) { return ReturnCode.SUCCESS; }
        /// <summary>
        ///在接收到类型为　ADD 的消息时调用
        /// </summary>
        /// <returns></returns>
        protected virtual int OnAdd(string param) { return ReturnCode.SUCCESS; }
        /// <summary>
        ///在接收到类型为　DELETE 的消息时调用
        /// </summary>
        /// <returns></returns>
        protected virtual int OnDelete(string param) { return ReturnCode.SUCCESS; }
        /// <summary>
        ///在接收到类型为　READ 的消息时调用
        /// </summary>
        /// <returns></returns>
        protected virtual int OnRead(string param) { return ReturnCode.SUCCESS; }
        /// <summary>
        ///在接收到类型为　SET 的消息时调用
        /// </summary>
        /// <returns></returns>

        protected virtual int OnSet(string param) { return ReturnCode.SUCCESS; }
       



        /// <summary>
        /// 操作成功
        /// </summary>
        /// <returns></returns>
        protected int ReturnSuccessMessage()
        {
            if (null != socket && socket.IsAvailable)
            {
                socket.Send(new Message(Message.MessageType.RETURN, PubConstant.SUC).ToJson());
            }
            return ReturnCode.SUCCESS;
        }
        /// <summary>
        /// 操作失败
        /// </summary>
        /// <returns></returns>
        protected int ReturnFailMessage()
        {
            if (null != socket && socket.IsAvailable)
            {
                socket.Send(new Message(Message.MessageType.RETURN, PubConstant.SUC).ToJson());
            }
            return ReturnCode.FAIL;
        }


    }
   
   
    
    
    

    
}
