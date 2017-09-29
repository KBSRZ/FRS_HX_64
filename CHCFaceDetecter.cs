using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading;
using System.Drawing;

using FRS;
using DataAngine;


namespace NjustSkyEyeSystem
{
    class CHCFaceDetecter
    {
        private Capture cap;
        private FeatureData featureData;
        private bool m_bInitSDK = false;
        private string sLocalIP = "";
        private CHCNetSDK.MSGCallBack m_falarmData = null;
        private string m_filePathCHC= "";
        private System.Windows.Forms.PictureBox RealPlayWnd;
        private Int32 m_lRealHandle = -1;
        private Int32[] m_lAlarmHandle = new Int32[200];
        private Int32 iListenHandle = -1;

        public string DVRIPAddress = "172.132.18.234";
        public Int16 DVRPortNumber = 8000;
        public string DVRUserName = "admin";
        public string DVRPassword = "admin12345";
        private Int32 m_lUserID = -1;

        private uint iLastErr = 0;
        private string msgErr = "";

        private bool detectStatue = false;

        private int countCHC = 0;
        private int countClean = 0;
        private int countMatchNone = 0;

        public bool DetectStatue
        {
            get 
            { 
                return detectStatue; 
            }
            set 
            {
                detectStatue = value;
            }
        }

        private int detectThreadCount = 3;
        //跟踪同一个人抓拍的最小张数，低于这个张数此人就不进行检测处理
        private int detectImgCount = 3;

        public int DetectImgCount
        {
            get { return detectImgCount; }
            set { detectImgCount = value; }
        }
        //跟踪同一个人抓拍的限定时间，在这个时间内未抓拍满最小张数，则视为无效抓拍主动清除
        private int catchLifeTime = 7; //(s)
        //已完成抓拍指标待处理项的最大存活时间，超过这个时间还未被处理则主动清除
        private int catchMaxLifeTime = 25; //(s)
        private int processDelayTime = 2000; //(ms)
        private int cleanDelayTime = 1000; //(ms)
        private Dictionary<uint, CatchFaceInfo> catchFaceImageMap = new Dictionary<uint, CatchFaceInfo>();
        private Dictionary<uint, CatchFaceInfo> processFaceImageMap = new Dictionary<uint, CatchFaceInfo>();
        private Thread processThread = null;
        private Thread cleanThread = null;
        private Thread[] detectThread = null;


        public delegate void ShowCountCallback(int type,int count);
		public event ShowCountCallback ShowCountEvent;

        public delegate void ShowMsgCallback(string msgStr, Exception ex);
		public event ShowMsgCallback ShowMsgEvent;


        public struct CatchFaceInfo
        {
            public List<Image> faceImgSet;
            public DateTime catchTime;
        }

        public CHCFaceDetecter(Capture cap, FeatureData featureData, System.Windows.Forms.PictureBox RealPlayWnd)
        {
            this.cap = cap;
            this.featureData = featureData;
            this.RealPlayWnd = RealPlayWnd;

        }

        public void InitCHC()
        {
            try
            {
                m_bInitSDK = CHCNetSDK.NET_DVR_Init();
            }
            catch (Exception ex)
            {
                ShowMsgEvent("NET_DVR_Init调用错误", ex);
            }

            if (m_bInitSDK == false)
            {
                ShowMsgEvent("海康设备初始化失败", null);
                return;
            }
            else
            {
                byte[] strIP = new byte[16 * 16];
                uint dwValidNum = 0;
                Boolean bEnableBind = false;

                m_filePathCHC = System.IO.Directory.GetCurrentDirectory();
                m_filePathCHC = Path.Combine(m_filePathCHC, "FileCHC");

                if (!Directory.Exists(m_filePathCHC))//如果不存在就创建file文件夹
                {
                    Directory.CreateDirectory(m_filePathCHC);
                }

                //保存SDK日志 To save the SDK log
                CHCNetSDK.NET_DVR_SetLogToFile(3, m_filePathCHC, true);

                //获取本地PC网卡IP信息
                if (CHCNetSDK.NET_DVR_GetLocalIP(strIP, ref dwValidNum, ref bEnableBind))
                {
                    if (dwValidNum > 0)
                    {
                        //取第一张网卡的IP地址为默认监听端口
                        sLocalIP = System.Text.Encoding.UTF8.GetString(strIP, 0, 16);
                    }

                    //设置报警回调函数
                    m_falarmData = new CHCNetSDK.MSGCallBack(MsgCallback);
                    CHCNetSDK.NET_DVR_SetDVRMessageCallBack_V30(m_falarmData, IntPtr.Zero);
                }
            }
        }

        public void MsgCallback(int lCommand, ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {

//            MessageBox.Show(lCommand.ToString("x8"));
            //通过lCommand来判断接收到的报警信息类型，不同的lCommand对应不同的pAlarmInfo内容
            switch (lCommand)
            {
                case CHCNetSDK.COMM_ALARM: //(DS-8000老设备)移动侦测、视频丢失、遮挡、IO信号量等报警信息
                    ProcessCommAlarm(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_V30://移动侦测、视频丢失、遮挡、IO信号量等报警信息
                    ProcessCommAlarm_V30(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                case CHCNetSDK.COMM_ALARM_FACE_DETECT:
                    MessageBox.Show("有人脸");
                    break;
                case CHCNetSDK.COMM_UPLOAD_FACESNAP_RESULT://人脸结果回调
                    ProcessCommFace(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                    break;
                //case CHCNetSDK.COMM_ALARM_RULE://进出区域、入侵、徘徊、人员聚集等行为分析报警信息
                //    ProcessCommAlarm_RULE(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                //    break;
                //case CHCNetSDK.COMM_UPLOAD_PLATE_RESULT://交通抓拍结果上传(老报警信息类型)
                //    ProcessCommAlarm_Plate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                //    break;
                //case CHCNetSDK.COMM_ITS_PLATE_RESULT://交通抓拍结果上传(新报警信息类型)
                //    ProcessCommAlarm_ITSPlate(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                //    break;
                //case CHCNetSDK.COMM_ALARM_PDC://客流量统计报警信息
                //    ProcessCommAlarm_PDC(ref pAlarmer, pAlarmInfo, dwBufLen, pUser);
                //    break;
                default:
                    break;
            }
        }

/// <summary>
/// 人脸图片抓拍队列处理线程，每隔一定时间将人脸图片抓拍缓存队列中的数据移动到人脸图片待处理队列中
/// </summary>
/// 
/// <param name="obj"></param>
        private void ProcessCatchFaceImageMap(object obj)
        {
            ShowMsgEvent("人脸图片抓拍队列处理线程已启动",null);

            while (DetectStatue)
            {
                Monitor.Enter(catchFaceImageMap);
                Monitor.Enter(processFaceImageMap);

                try
                {
                    List<uint> faceIdSet = catchFaceImageMap.Keys.ToList();
                    foreach (uint faceId in faceIdSet)
                    {
                        CatchFaceInfo catchInfo = catchFaceImageMap[faceId];

                        if (catchInfo.faceImgSet.Count < detectImgCount)
                        {   
                            continue;
                        }

                        if (false == processFaceImageMap.ContainsKey(faceId))
                        {
                            processFaceImageMap.Add(faceId, catchInfo);
                        }

/*
                        if (processFaceImageMap.ContainsKey(faceId))
                        {
                            foreach (Image img in catchInfo.faceImgSet)
                            {
                                processFaceImageMap[faceId].faceImgSet.Add(img);
                            }
                        }
                        else
                        {
                            List<Image> imgSet = new List<Image>();
                            foreach (Image img in catchInfo.faceImgSet)
                            {
                                imgSet.Add(img);
                            }
                            catchInfo.faceImgSet = imgSet;

                            processFaceImageMap.Add(faceId, catchInfo);
                        }
 */ 

                        catchFaceImageMap.Remove(faceId);
                    }

                    ShowCountEvent(1,processFaceImageMap.Count);

                }
                catch (Exception ex)
                {
                    ShowMsgEvent("ProcessCatchFaceImageMap Error", ex);
                }

                Monitor.Exit(processFaceImageMap);
                Monitor.Exit(catchFaceImageMap);

                System.GC.Collect();
                Thread.Sleep(processDelayTime);
            }

            ShowMsgEvent("人脸图片抓拍队列处理线程已退出",null);
        }

/// <summary>
/// 人脸图片抓拍队列清理线程，每隔一定时间清理下不符合处理条件或驻留严重超时的数据
/// </summary>
/// <param name="obj"></param>
        private void CleanCatchFaceImageMap(object obj)
        {
            ShowMsgEvent("人脸图片抓拍队列清理线程已启动", null);

            while (DetectStatue)
            {
                Monitor.Enter(catchFaceImageMap);

                try
                {
                    List<uint> faceIdSet = catchFaceImageMap.Keys.ToList();
                    foreach (uint faceId in faceIdSet)
                    {
                        CatchFaceInfo catchInfo = catchFaceImageMap[faceId];
                        TimeSpan ts = DateTime.Now.Subtract(catchInfo.catchTime).Duration();
                        if (ts.Seconds >= catchLifeTime && catchInfo.faceImgSet.Count < detectImgCount || ts.Seconds >= catchMaxLifeTime)
                        {
                            catchFaceImageMap.Remove(faceId);

                            countClean++;
                            ShowCountEvent(3, countClean);
                        }
                    }

                    ShowCountEvent(4, catchFaceImageMap.Count);
                }
                catch (Exception ex)
                {
                    ShowMsgEvent("CleanCatchFaceImageMap Error", ex);
                }

                Monitor.Exit(catchFaceImageMap);

                System.GC.Collect();
                Thread.Sleep(cleanDelayTime);
            }

            ShowMsgEvent("人脸图片抓拍队列清理线程已退出", null);
        }

/// <summary>
/// 人脸图片检测线程，不断从人脸图片处理队列中取出数据进行检测
/// </summary>
/// <param name="obj"></param>
        private void DetectFace4CHC(object obj)
        {
            int detectChannel = (int)obj;

            ShowMsgEvent("人脸图片检测线程" + detectChannel + "已启动", null);

            while (DetectStatue)
            {
                Monitor.Enter(processFaceImageMap);

                List<Image> detectFaceImageSet = null;
                if(processFaceImageMap.Count > 0)
                {
                    
                    KeyValuePair<uint, CatchFaceInfo> kv = processFaceImageMap.First();

                    detectFaceImageSet = kv.Value.faceImgSet;
                    processFaceImageMap.Remove(kv.Key);
                    
                }


                Monitor.Exit(processFaceImageMap);

                try
                {
                    if (null != detectFaceImageSet)
                    {
                       bool detectState = cap.DetectFace4CHC(detectFaceImageSet, detectChannel);


                       if (false == detectState)
                       {
                           countMatchNone++;
                           ShowCountEvent(5, countMatchNone);
                       }
                    }     
                }
                catch(Exception ex)
                {
                    ShowMsgEvent("DetectFace4CHC Error", ex);
                }

                Thread.Sleep(1);
            }

            ShowMsgEvent("人脸图片检测线程" + detectChannel + "已退出",null);
        }

        private void ProcessCommFace(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            uint faceId = 0;
            Image faceImg = null;

            try
            {
                CHCNetSDK.NET_VCA_FACESNAP_RESULT jpFacedetectAlarm = new CHCNetSDK.NET_VCA_FACESNAP_RESULT();
                uint dwSize = (uint)Marshal.SizeOf(jpFacedetectAlarm);
                jpFacedetectAlarm = (CHCNetSDK.NET_VCA_FACESNAP_RESULT)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_VCA_FACESNAP_RESULT));                  
  
                if (jpFacedetectAlarm.dwFacePicLen > 0)
                {
                    faceId = jpFacedetectAlarm.dwFacePicID;
                    int iLen = (int)jpFacedetectAlarm.dwFacePicLen;
                    byte[] by = new byte[iLen];
                    Marshal.Copy(jpFacedetectAlarm.pBuffer1, by, 0, iLen);

                    MemoryStream ms = new MemoryStream(by);
                    Image imgStream = Image.FromStream(ms);
                    faceImg = imgStream;

//                    Bitmap imgClone= (Bitmap) imgStream;
//                    faceImg = imgClone.Clone(new Rectangle(0,0, imgStream.Width, imgStream.Height), imgStream.PixelFormat);

//                    imgStream.Dispose();
//                    ms.Close();
                }
            }
            catch (Exception ex)
            {
                ShowMsgEvent("ProcessCommFace Get Image From CHC Error:", ex);
            }
           
            if(null != faceImg)
            {
                Monitor.Enter(catchFaceImageMap);
               
                if (catchFaceImageMap.ContainsKey(faceId))
                {
                    CatchFaceInfo catchInfo = catchFaceImageMap[faceId];

                    if (catchInfo.faceImgSet.Count < detectImgCount)
                    {
                      catchInfo.faceImgSet.Add(faceImg);
                    }
                }
                else
                {
                    CatchFaceInfo catchInfo = new CatchFaceInfo();
                    catchInfo.faceImgSet = new List<Image>();
                    catchInfo.faceImgSet.Add(faceImg);
                    catchInfo.catchTime = DateTime.Now;
                    //放入队列
                    catchFaceImageMap.Add(faceId, catchInfo);

//                    countCHC++;
//                    ShowCountEvent(2, countCHC);

                    ShowCountEvent(4, catchFaceImageMap.Count);
                }

                Monitor.Exit(catchFaceImageMap);

            }
        }

        public void ProcessCommAlarm(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {
            CHCNetSDK.NET_DVR_ALARMINFO struAlarmInfo = new CHCNetSDK.NET_DVR_ALARMINFO();

            struAlarmInfo = (CHCNetSDK.NET_DVR_ALARMINFO)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_ALARMINFO));

            string strIP = pAlarmer.sDeviceIP;
            string stringAlarm = "";
            int i = 0;

            switch (struAlarmInfo.dwAlarmType)
            {
                case 0:
                    stringAlarm = "信号量报警，报警报警输入口：" + struAlarmInfo.dwAlarmInputNumber + "，触发录像通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM; i++)
                    {
                        if (struAlarmInfo.dwAlarmRelateChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 1:
                    stringAlarm = "硬盘满，报警硬盘号：";
                    for (i = 0; i < CHCNetSDK.MAX_DISKNUM; i++)
                    {
                        if (struAlarmInfo.dwDiskNumber[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 2:
                    stringAlarm = "信号丢失，报警通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM; i++)
                    {
                        if (struAlarmInfo.dwChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 3:
                    stringAlarm = "移动侦测，报警通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM; i++)
                    {
                        if (struAlarmInfo.dwChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 4:
                    stringAlarm = "硬盘未格式化，报警硬盘号：";
                    for (i = 0; i < CHCNetSDK.MAX_DISKNUM; i++)
                    {
                        if (struAlarmInfo.dwDiskNumber[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 5:
                    stringAlarm = "读写硬盘出错，报警硬盘号：";
                    for (i = 0; i < CHCNetSDK.MAX_DISKNUM; i++)
                    {
                        if (struAlarmInfo.dwDiskNumber[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 6:
                    stringAlarm = "遮挡报警，报警通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM; i++)
                    {
                        if (struAlarmInfo.dwChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 7:
                    stringAlarm = "制式不匹配，报警通道";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM; i++)
                    {
                        if (struAlarmInfo.dwChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 8:
                    stringAlarm = "非法访问";
                    break;
                default:
                    stringAlarm = "其他未知报警信息";
                    break;
            }

            MessageBox.Show("ProcessCommAlarm:" + stringAlarm);
        }

        private void ProcessCommAlarm_V30(ref CHCNetSDK.NET_DVR_ALARMER pAlarmer, IntPtr pAlarmInfo, uint dwBufLen, IntPtr pUser)
        {

            CHCNetSDK.NET_DVR_ALARMINFO_V30 struAlarmInfoV30 = new CHCNetSDK.NET_DVR_ALARMINFO_V30();

            struAlarmInfoV30 = (CHCNetSDK.NET_DVR_ALARMINFO_V30)Marshal.PtrToStructure(pAlarmInfo, typeof(CHCNetSDK.NET_DVR_ALARMINFO_V30));

            string strIP = pAlarmer.sDeviceIP;
            string stringAlarm = "";
            int i;

            switch (struAlarmInfoV30.dwAlarmType)
            {
                case 0:
                    stringAlarm = "信号量报警，报警报警输入口：" + struAlarmInfoV30.dwAlarmInputNumber + "，触发录像通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byAlarmRelateChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + "\\";
                        }
                    }
                    break;
                case 1:
                    stringAlarm = "硬盘满，报警硬盘号：";
                    for (i = 0; i < CHCNetSDK.MAX_DISKNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byDiskNumber[i] == 1)
                        {
                            stringAlarm += (i + 1) + " ";
                        }
                    }
                    break;
                case 2:
                    stringAlarm = "信号丢失，报警通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 3:
                    stringAlarm = "移动侦测，报警通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 4:
                    stringAlarm = "硬盘未格式化，报警硬盘号：";
                    for (i = 0; i < CHCNetSDK.MAX_DISKNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byDiskNumber[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 5:
                    stringAlarm = "读写硬盘出错，报警硬盘号：";
                    for (i = 0; i < CHCNetSDK.MAX_DISKNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byDiskNumber[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 6:
                    stringAlarm = "遮挡报警，报警通道：";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 7:
                    stringAlarm = "制式不匹配，报警通道";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 8:
                    stringAlarm = "非法访问";
                    break;
                case 9:
                    stringAlarm = "视频信号异常，报警通道";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 10:
                    stringAlarm = "录像/抓图异常，报警通道";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 11:
                    stringAlarm = "智能场景变化，报警通道";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 12:
                    stringAlarm = "阵列异常";
                    break;
                case 13:
                    stringAlarm = "前端/录像分辨率不匹配，报警通道";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                case 15:
                    stringAlarm = "智能侦测，报警通道";
                    for (i = 0; i < CHCNetSDK.MAX_CHANNUM_V30; i++)
                    {
                        if (struAlarmInfoV30.byChannel[i] == 1)
                        {
                            stringAlarm += (i + 1) + " \\ ";
                        }
                    }
                    break;
                default:
                    stringAlarm = "其他未知报警信息";
                    break;
            }

            MessageBox.Show("ProcessCommAlarm_V30:" + stringAlarm);
        }


        public bool LoginDVR()
        {
            bool statue = true;
            if (DVRIPAddress.Equals("") || DVRUserName.Equals("") || DVRPassword.Equals("")) 
            {
                MessageBox.Show("配置不正确，请设置IP、端口号、用户名和密码!");
                statue = false;
            }

            if (m_lUserID < 0)
            {
                CHCNetSDK.NET_DVR_DEVICEINFO_V30 DeviceInfo = new CHCNetSDK.NET_DVR_DEVICEINFO_V30();

                //登录设备 Login the device
                m_lUserID = CHCNetSDK.NET_DVR_Login_V30(DVRIPAddress, DVRPortNumber, DVRUserName, DVRPassword, ref DeviceInfo);
                if (m_lUserID < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    msgErr = "NET_DVR_Login_V30 failed, error code= " + iLastErr; //登录失败，输出错误号
                    MessageBox.Show(msgErr);
                    statue = false;
                }
            }

            return statue;
        }

        public bool LogoutDVR()
        {
            bool statue = true;
            if (m_lUserID >= 0)
            {
                if (m_lRealHandle >= 0)
                {
                    MessageBox.Show("请先停止视频预览");
                    statue = false;
                }
                else
                {
                    if (!CHCNetSDK.NET_DVR_Logout(m_lUserID))
                    {
                        iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                        msgErr = "NET_DVR_Logout failed, error code= " + iLastErr;
                        MessageBox.Show(msgErr);
                        statue = false;
                    }
                    else
                    {
                        m_lUserID = -1;
                    }
                }           
            }

            return statue;
        }


        public bool Start()
        {
            bool ret = false;
            if (m_lUserID < 0)
            {
                MessageBox.Show("请先登录连接设备");
                return ret;
            }

            if (m_lRealHandle < 0)
            {
                CHCNetSDK.NET_DVR_PREVIEWINFO lpPreviewInfo = new CHCNetSDK.NET_DVR_PREVIEWINFO();
                lpPreviewInfo.hPlayWnd = RealPlayWnd.Handle;//预览窗口
                lpPreviewInfo.lChannel = 1;//预te览的设备通道
                lpPreviewInfo.dwStreamType = 0;//码流类型：0-主码流，1-子码流，2-码流3，3-码流4，以此类推
                lpPreviewInfo.dwLinkMode = 0;//连接方式：0- TCP方式，1- UDP方式，2- 多播方式，3- RTP方式，4-RTP/RTSP，5-RSTP/HTTP 
                lpPreviewInfo.bBlocked = true; //0- 非阻塞取流，1- 阻塞取流
                lpPreviewInfo.dwDisplayBufNum = 15; //播放库播放缓冲区最大缓冲帧数

                CHCNetSDK.NET_DVR_JPEGPARA jpegpara = new CHCNetSDK.NET_DVR_JPEGPARA();
                jpegpara.wPicSize = 7;
                jpegpara.wPicQuality = 0;

                //CHCNetSDK.NET_VCA_SIZE_FILTER sizeFilter = new CHCNetSDK.NET_VCA_SIZE_FILTER();
                //sizeFilter.byActive = 1;
                ////sizeFilter.byMode = CHCNetSDK.SIZE_FILTER_MODE.IMAGE_PIX_MODE;
                //sizeFilter.struMiniRect;
                //sizeFilter.struMaxRect;

                //CHCNetSDK.NET_VCA_POLYGON polygon=new CHCNetSDK.NET_VCA_POLYGON();
                //polygon.dwPointNum = 3;
                ////polygon.struPos = 10;

                //CHCNetSDK.NET_VCA_SINGLE_FACESNAPCFG singleFaceConfig = new CHCNetSDK.NET_VCA_SINGLE_FACESNAPCFG();
                //singleFaceConfig.byActive = 1;
                //singleFaceConfig.struSizeFilter =;
                ////singleFaceConfig.struVcaPolygon =;


                CHCNetSDK.NET_VCA_FACESNAPCFG lpFacesnapcfg = new CHCNetSDK.NET_VCA_FACESNAPCFG();
                // //lpFacesnapcfg.dwSize=
                // //结构体大小
                lpFacesnapcfg.bySnapTime = 5;
                // //单个目标人脸的抓拍次数，取值范围：0~10，上传评分最高的一张

                lpFacesnapcfg.bySnapInterval = 1;
                // //抓拍间隔，单位：帧

                lpFacesnapcfg.bySnapThreshold = 70;
                // //抓拍阈值，取值范围：0 - 100

                lpFacesnapcfg.byGenerateRate = 1;
                // //目标生成速度，取值范围：[1, 5]

                lpFacesnapcfg.bySensitive = 2;
                // //目标检测灵敏度，取值范围：[1, 5]

                lpFacesnapcfg.byReferenceBright = 50;
                // //参考亮度，取值范围：[0,100]

                lpFacesnapcfg.byMatchType = 1;
                // //比对报警模式：0-目标消失后报警，1-实时报警

                lpFacesnapcfg.byMatchThreshold = 70;
                // //实时比对阈值，取值范围：0~100 

                lpFacesnapcfg.struPictureParam = jpegpara;
                // //图片规格结构（图片分辨率和图片质量） 

                //// //lpFacesnapcfg.struRule = jpFacesnapcfg;
                // //人脸抓拍规则
                //lpFacesnapcfg.wFaceExposureMinDuration = 60;
                // //人脸曝光最短持续时间，单位：秒，范围：1~3600，默认：60 
                //// lpFacesnapcfg.byFaceExposureMode
                // //人脸曝光使能：1- 关闭，2- 开启，0- 自动（根据人脸判断） 
                lpFacesnapcfg.byBackgroundPic = 0;
                // //背景图上传使能：0- 默认值（开启），1- 禁止
                // //lpFacesnapcfg.byRes2
                // //保留

                CHCNetSDK.REALDATACALLBACK RealData = new CHCNetSDK.REALDATACALLBACK(RealDataCallBack);//预览实时流回调函数
                IntPtr pUser = new IntPtr();//用户数据

                //打开预览 Start live view 
                m_lRealHandle = CHCNetSDK.NET_DVR_RealPlay_V40(m_lUserID, ref lpPreviewInfo, null/*RealData*/, pUser);
                if (m_lRealHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    msgErr = "NET_DVR_RealPlay_V40 failed, error code= " + iLastErr; //预览失败，输出错误号
//                    MessageBox.Show(msgErr);
                    LogHelper.WriteErrorLog(msgErr);
                }
                else
                {
                    ret = true;        
                    //开始布防并进行监听
                    startAlarm();   
                }       
            }

            return ret;
        }

        public bool Stop()
        {
            bool ret = true;

            if (m_lRealHandle >= 0)
            {
                if (!CHCNetSDK.NET_DVR_StopRealPlay(m_lRealHandle))
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    msgErr = "NET_DVR_StopRealPlay failed, error code= " + iLastErr;
                    MessageBox.Show(msgErr);
                    ret = false;
                }
                else
                {
                    StopDetect();
                    m_lRealHandle = -1;
                }
            }

            return ret;
        }

        public void StartDetect()
        {
            ShowMsgEvent("检测模块开始启动",null);

            detectStatue = true;

            processThread = new Thread(new ParameterizedThreadStart(ProcessCatchFaceImageMap));
            processThread.IsBackground = true;
            processThread.Start();

            cleanThread = new Thread(new ParameterizedThreadStart(CleanCatchFaceImageMap));
            cleanThread.IsBackground = true;
            cleanThread.Start();

            detectThread = new Thread[detectThreadCount]; ;
            for (int i = 0; i < detectThreadCount; i++)
            {
                detectThread[i] = new Thread(new ParameterizedThreadStart(DetectFace4CHC));
                detectThread[i].IsBackground = true;
                detectThread[i].Start(i);
            }
        }

        public void StopDetect()
        {
            if (false == DetectStatue)
            {
                return;
            }

            ShowMsgEvent("检测模块开始关闭", null);

            DetectStatue = false;

            processThread.Join();
            cleanThread.Join();
            for (int i = 0; i < detectThreadCount; i++)
            {
               detectThread[i].Abort();
            }
        }

        /// <summary>
        /// 开始布防并进行监听
        /// </summary>
        private void startAlarm()
        {
            CHCNetSDK.NET_DVR_SETUPALARM_PARAM struAlarmParam = new CHCNetSDK.NET_DVR_SETUPALARM_PARAM();
            struAlarmParam.dwSize = (uint)Marshal.SizeOf(struAlarmParam);
            struAlarmParam.byLevel = 0; //0- 一级布防,1- 二级布防
            struAlarmParam.byAlarmInfoType = 1;//智能交通设备有效，新报警信息类型

            m_lAlarmHandle[m_lUserID] = CHCNetSDK.NET_DVR_SetupAlarmChan_V41(m_lUserID, ref struAlarmParam);
            if (m_lAlarmHandle[m_lUserID] < 0)
            {
                iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                msgErr = "布防失败，错误号：" + iLastErr; //布防失败，输出错误号
                ShowMsgEvent(msgErr, null);
            }
            else
            {
                //string sLocalIP = sLocalIP;//本地监听地址，可以为空
                ushort wLocalPort = ushort.Parse("7200");//监听端口

                iListenHandle = CHCNetSDK.NET_DVR_StartListen_V30(sLocalIP, wLocalPort, m_falarmData, IntPtr.Zero);
                if (iListenHandle < 0)
                {
                    iLastErr = CHCNetSDK.NET_DVR_GetLastError();
                    msgErr = "启动监听失败，错误号：" + iLastErr; //启动监听失败，输出错误号
                    ShowMsgEvent(msgErr, null);
                }
                else
                {
                    StartDetect();
//                    cap.StartCHC();
                    ShowMsgEvent("监听成功", null);
                }
            }
        }


        public void RealDataCallBack(Int32 lRealHandle, UInt32 dwDataType, ref byte pBuffer, UInt32 dwBufSize, IntPtr pUser)
        {

        }





    }
}
