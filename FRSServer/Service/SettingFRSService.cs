using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using FRSServer.Data.Setting;

namespace FRSServer.Service
{
    /// <summary>
    /// 设置服务  
    /// 设计思想在  
    /// OnOpen时返回对应的设置，
    /// OnMessage时进行设置
    /// 只改变配置文件，重启生效
    /// </summary>
    /// </summary>
    class SettingFRSService : BaseService
    {
        private static Data.Setting.SettingFRS setting = new Data.Setting.SettingFRS();
        public SettingFRSService()
        {
            url = "/setting-frs";
        }
        public override void OnOpen()
        {
           

        }
        public override void OnClose()
        {
            if (null != socket)
            {
                socket.Close();
            }
            Console.WriteLine("SettingService::OnClose");
        }
        public override int OnMessage(string param)
        {
            Console.WriteLine("SettingService::OnMessage");
            Console.WriteLine(param);
            try
            {
                
                Message message = Message.CreateMessageFromJSON(param);
              
                if (Message.MessageType.READ == message.Type)//获得
                {
                  
                    if (null != socket && socket.IsAvailable)
                    {
                        Console.WriteLine(setting.ToJson());
                        socket.Send(new Message(Message.MessageType.READ, setting.ToJson()).ToJson());
                    }
                }
                else if (Message.MessageType.UPDATE == message.Type)//设置
                {
                   
                    setting = SettingFRS.CreateMessageFromJSON(message.Content);
                    
                    if (Data.Setting.SettingFRS.Save(setting) == ReturnCode.SUCCESS)
                    {
                        if (socket.IsAvailable)
                            socket.Send(new Message(Message.MessageType.RETURN, PubConstant.SUC).ToJson());
                        Console.WriteLine("设置成功");
                        return ReturnCode.SUCCESS;

                    }
                }
            }
            catch (Exception e)
            {
                LogHelper.WriteErrorLog(null, e);
                if (socket.IsAvailable)
                    socket.Send(new Message(Message.MessageType.RETURN, PubConstant.FAIL).ToJson());
            }
            return ReturnCode.FAIL;

        }
        //private void Resetting(Data.Setting.SettingFRS setting)
        //{

        //    fa.MaxPersonNum = setting.MaxPersonNum;
        //    fa.ScoreThresh = setting.ScoreThresh;
        //    fa.SearchFaceHeightThresh = setting.SearchFaceHeightThresh;
        //    fa.SearchFaceWidthThresh = setting.SearchFaceWidthThresh;
        //    fa.SearchFaceYawThresh = setting.SearchFaceYawThresh;
        //    fa.SearchFacePitchThresh = setting.SearchFacePitchThresh;
        //    fa.SearchFaceRollThresh = setting.SearchFaceRollThresh;
        //    fa.SearchFaceQualityThresh = setting.SearchFaceQualityThresh;
        //    fa.TopK = setting.TopK;

        //    fa.RegisterFaceHeightThresh = setting.RegisterFaceHeightThresh;
        //    fa.RegisterFaceWidthThresh = setting.RegisterFaceWidthThresh;
        //    fa.RegisterFaceYawThresh = setting.RegisterFaceYawThresh;
        //    fa.RegisterFacePitchThresh = setting.RegisterFacePitchThresh;
        //    fa.RegisterFaceRollThresh = setting.RegisterFaceRollThresh;
        //    fa.RegisterFaceQualityThresh = setting.RegisterFaceQualityThresh;

        //    cap.Interval = setting.Interval;

        //}
    }

}
