using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FRS;
using Fleck;
using Newtonsoft.Json;
using FRSServer.Service;
namespace FRSServer
{
     
    class Program
    {


        /// <summary>
        /// 用于存储当前的 Service，可能SettingService，HitAlertService，VerifyingService，SearchingByImageService，SearchingByTimeService
        /// 任何情况只能有一个socket,新的socket 会顶替掉旧的socket
        /// </summary>
        static BaseService curService = null;
        //测试用 Capture
        //static Test.Capture cap = new Test.Capture();

        /// <summary>
        /// 初始化
        /// </summary>
        private static void InitFRS()
        {
                        
            FRSParam param = new FRSParam();

            param.nMinFaceSize = Math.Min (PubConstant.SearchFaceHeightThresh,PubConstant.MaxPersonNum);

            param.nRollAngle = Math.Min (PubConstant.SearchFaceRollThresh,Math.Min (PubConstant.SearchFaceYawThresh,PubConstant.SearchFacePitchThresh));
            param.bOnlyDetect = true;

            FaceImage.Create(PubConstant.ChannelNum, param);
            Feature.Init(PubConstant.ChannelNum);


            BaseService.fa = new FeatureData();


            BaseService.fa.MaxPersonNum = PubConstant.MaxPersonNum;
            BaseService.fa.ScoreThresh = PubConstant.ScoreThresh;
            BaseService.fa.SearchFaceHeightThresh = PubConstant.SearchFaceHeightThresh;
            BaseService.fa.SearchFaceWidthThresh = PubConstant.SearchFaceWidthThresh;
            BaseService.fa.SearchFaceYawThresh = PubConstant.SearchFaceYawThresh;
            BaseService.fa.SearchFacePitchThresh = PubConstant.SearchFacePitchThresh;
            BaseService.fa.SearchFaceRollThresh = PubConstant.SearchFaceRollThresh;
            BaseService.fa.SearchFaceQualityThresh = PubConstant.SearchFaceQualityThresh;
            BaseService.fa.TopK = PubConstant.TopK;

            BaseService.fa.RegisterFaceHeightThresh = PubConstant.RegisterFaceHeightThresh;
            BaseService.fa.RegisterFaceWidthThresh = PubConstant.RegisterFaceWidthThresh;
            BaseService.fa.RegisterFaceYawThresh = PubConstant.RegisterFaceYawThresh;
            BaseService.fa.RegisterFacePitchThresh = PubConstant.RegisterFacePitchThresh;
            BaseService.fa.RegisterFaceRollThresh = PubConstant.RegisterFaceRollThresh;
            BaseService.fa.RegisterFaceQualityThresh = PubConstant.RegisterFaceQualityThresh;


            //fa.RegisterOneFinisedEvent += new FeatureData.RegisterOneFinisedCallback(RegisterOneFinised);
            BaseService.fa.ShowMsgEvent += new FeatureData.ShowMsgCallback(ShowMsgInfo);

            BaseService.cap = new Capture(BaseService.fa);
            BaseService.cap.Interval = PubConstant.Interval;

            BaseService.cap.HitAlertReturnEvent += new Capture.HitAlertCallback(OnHitAlert);//

            BaseService.fa.LoadData();
            //BaseService.cap.Start(0);


            //cap.HitAlertReturnEvent += new Test.Capture.HitAlertCallback(OnHitAlert);
            //cap.Start();
        }

        static void ShowMsgInfo(string msgStr, Exception ex)
        {
            
                if (null != ex)
                {
                    LogHelper.WriteErrorLog(msgStr, ex);
                }
                else
                {
                    LogHelper.WriteInfoLog(msgStr);
                }
            }
    
        //回掉函数
        static void OnHitAlert(HitAlert[] result)
        {
            if (result == null || 0 == result.Count())
            {
                return;
            }
            Console.WriteLine("11111111111111111111111");
            lock (objlock)
            {
                if (curService is HitAlertService)//当前如果是HitAlertService那么就 发送
                {
                    Data.HitAlert[] hitalerts = new Data.HitAlert[result.Length];
                    for (int i = 0; i < hitalerts.Length; i++)
                    {
                        hitalerts[i] = new Data.HitAlert(result[i]);
                    }
                    

                    //if (curService is HitAlertService )
                    //    ((HitAlertService)curService).OnHit(hitalerts);
                }

                Console.WriteLine("222222222222222222222222");
            }
        }

        /// <summary>
        /// 互斥变量， 每次连接都是 重新开启一个线程   curService 在不同 OnOpen OnClose OnMessage 中 会不同步
        /// 比如在OnPen时curService 被赋值
        /// 但另一个socket 关闭OnClose,curService又被改变了
        /// </summary>
        private static object objlock = new object();
        static void main()
        {
            FleckLog.Level = LogLevel.Debug;

            var server = new WebSocketServer("ws://127.0.0.1");
            

            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    lock (objlock)
                    {
                        BaseService ser = ServiceHelper.GetService(socket.ConnectionInfo.Path);
                        
                        if (null==ser)//查找服务失败
                        {
                           
                            socket.Send(new Message(Message.MessageType.RETURN,PubConstant.ILLEGAL_URL).ToJson());
                            socket.Close();
                            return;
                        }

                        if (null != curService && null != curService.Socket && curService.Socket.IsAvailable)//若service不为null 则主动关闭
                        {
                            curService.Socket.Close();
                        }
                        curService = ser;//更新当前服务
                        curService.Socket = socket;
                       
                        Console.WriteLine("当前service " + curService.GetType());

                       
                    }

                };
                socket.OnClose = () =>
                {
                    
                    Console.WriteLine("Socket Close!");
                    //curService = null;不能直接置空 , OnClose 会在调用Close()函数另一个线程中执行
                    lock (objlock)
                    {
                        if (curService.Socket == socket)//在关闭之前 curService没有变动 也就是没有新的连接
                        {
                            //当前服务设置为无用服务
                            curService = null;
                        }

                    }
                    
                };
                socket.OnMessage = message =>
                {
                    
                    lock (objlock)
                    {
                            curService.OnMessage(message);
                    }
                   
                };
            });

        }
        static void Main(string[] args)
        {
            
            main();
            InitFRS();
            Console.ReadLine();
        }
    }



}
