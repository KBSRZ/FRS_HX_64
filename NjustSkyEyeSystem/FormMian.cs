using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Configuration;
using FRS;
using DataAngine;
using DataAgine_Set;

namespace NjustSkyEyeSystem
{
    public partial class FormMian : Form
    {
        public struct HitUserItemInfo
        {
            public int itemIdx;
            public int similarityPercent;
        }

        #region 变量

        public const int MSG_TIP_DIALOG = 0;//在dialog输出异常
        public const int MSG_TIP_LOG = 1;//在文件中输出异常

        private int msgTipMode = MSG_TIP_LOG;

        public static FeatureData fa;//数据操作类
        private Capture cap;//视频处理类

        private string libraryname = "";//当前操作数据库
        private string devicename = "";//当前操作摄像头
        private string loginrtsp = "";//当前操作rtsp流

        public static string registerDataDir = "";//注册目录
        private Image imageOnShow;//正在显示中的图片帧
        private Image imageCompare1;//证件比对图片
        private Image imageCompare2;//证件比对图片
        private Image image_Library_Compare;//底库比对图片
        readonly DataAngine.BLL.hitalert hitbll = new DataAngine.BLL.hitalert();
        readonly DataAngine.BLL.user user = new DataAngine.BLL.user();
        readonly DataAgine_Set.BLL.frs_database frs_database = new DataAgine_Set.BLL.frs_database();
        readonly DataAgine_Set.BLL.device device = new DataAgine_Set.BLL.device();

        //底库查询
        int PageSize_Library = 36;
        int CurPage_Library;
        int Count_Record_Library;
        int Count_Page_Library;

        //HitAlert查询
        int PageSize_HitAlert = 10;
        int CurPage_HitAlert;
        int Count_Record_HitAlert;
        int Count_Page_HitAlert;

        //已告警的用户项
        private Dictionary<uint, HitUserItemInfo> hitUserItemMap = new Dictionary<uint, HitUserItemInfo>();
        private int hitUserMaxCount = 20;

        public static short channelNumFRS = 5;
        private CHCFaceDetecter chcFaceDetecter;

        public const int REG_IN_BUILK_FROM_DIR = 0;//从目录中注册
        public const int REG_IN_BUILK_FROM_FILE = 1;//文件中注册
        private int registerType = REG_IN_BUILK_FROM_FILE;
        #endregion
        public FormMian()
        {
            InitializeComponent();
            InitFRS();
            InitSetting();
            InitUI();
            this.WindowState = FormWindowState.Maximized;    //最大化窗体

            StartCaptureAuto();
        }


        #region 初始化
        private static void InitFRS()
        {
            //            System.Console.WriteLine(System.Environment.CurrentDirectory);
            FRSParam param = new FRSParam();

            param.nMinFaceSize = 50;
            param.nRollAngle = 10;
            param.bOnlyDetect = true;

            FaceImage.Create(channelNumFRS, param);
            Feature.Init(channelNumFRS);
        }
        private void InitSetting()
        {
            fa = new FeatureData();
            fa.RegisterOneFinisedEvent += new FeatureData.RegisterOneFinisedCallback(RegisterOneFinised);
            fa.FaceDetectedEvent += new FeatureData.FaceDetectedCallback(LocateFaceResult);
            fa.ShowMsgEvent += new FeatureData.ShowMsgCallback(ShowMsgInfo);

            cap = new Capture(fa);
            cap.HitAlertReturnEvent += new Capture.HitAlertCallback(GetHitAlertResult);//关闭提升稳定性
            cap.ImageGrabbedEvent += new Capture.ImageGrabbedCallback(capture_ImageGrabbed);//界面显示
            cap.LocateFaceReturnEvent += new Capture.LocateFaceCallback(LocateFaceResult);//关闭提升稳定性
            cap.ShowCountEvent += new Capture.ShowCountCallback(ShowCountInfo);
            cap.ShowMsgEvent += new Capture.ShowMsgCallback(ShowMsgInfo);

            chcFaceDetecter = new CHCFaceDetecter(cap, picPlayer);
            //chcFaceDetecter.ShowCountEvent += new CHCFaceDetecter.ShowCountCallback(ShowCountInfo);
            chcFaceDetecter.ShowMsgEvent += new CHCFaceDetecter.ShowMsgCallback(ShowMsgInfo);
            chcFaceDetecter.InitCHC();

        }

        private void InitUI()
        {
            //下拉框
            ComboxLibraryList();
            ComboxDevList();

            flowLayoutPanel.Controls.Clear();
            lbl_CountCHC.Text = "";
            lbl_CountCapture.Text = "";
            lbl_CountClean.Text = "";
            lbl_CountWaitDetect.Text = "";
            lbl_CountMatchNone.Text = "";
            lbl_CatchFaceCount.Text = "";
            lbl_CountMatch.Text = "";
            lbl_CountCollect.Text = "";

            cb_RegUserGender.SelectedIndex = 0;



            nudInterval.Value = cap.Interval;

            nudMaxFaceNum.Value = (decimal)fa.MaxPersonNum;
            nudTopK.Value = (decimal)fa.TopK;
            nudThresh.Value = (decimal)fa.ScoreThresh;
            nudSearchFaceQualityThresh.Value = (decimal)fa.SearchFaceQualityThresh;
            nudSearchFaceWidthThresh.Value = (decimal)fa.SearchFaceHeightThresh;
            nudSearchFaceHeightThresh.Value = (decimal)fa.SearchFaceWidthThresh;
            nudSearchYawThresh.Value = (decimal)fa.SearchFaceYawThresh;
            nudSearchRollThresh.Value = (decimal)fa.SearchFaceRollThresh;
            nudSearchPitchThresh.Value = (decimal)fa.SearchFacePitchThresh;


            nudRegisterFaceQualityThresh.Value = (decimal)fa.RegisterFaceQualityThresh;
            nudRegisterFaceWidthThresh.Value = (decimal)fa.RegisterFaceHeightThresh;
            nudRegisterFaceHeightThresh.Value = (decimal)fa.RegisterFaceWidthThresh;
            nudRegisterYawThresh.Value = (decimal)fa.RegisterFaceYawThresh;
            nudRegisterRollThresh.Value = (decimal)fa.RegisterFaceRollThresh;
            nudRegisterPitchThresh.Value = (decimal)fa.RegisterFacePitchThresh;
            nudTimesThresh.Value = (decimal)fa.Times;




            //nudTrackNum.Value = chcFaceDetecter.DetectImgCount;

            string connectionStringCHC = ConfigurationManager.AppSettings["ConnectionStringCHC"];
            string[] consCHC = connectionStringCHC.Split(new char[] { ';' });
            tex_DevIP.Text = consCHC[0].Split(new char[] { '=' })[1];
            tex_DevPort.Text = consCHC[1].Split(new char[] { '=' })[1];
            tex_UserName.Text = consCHC[2].Split(new char[] { '=' })[1];
            tex_UserPwd.Text = consCHC[3].Split(new char[] { '=' })[1];


            string connectionStringMySQL = ConfigurationManager.AppSettings["ConnectionStringMySQL"];
            string[] consMySQL = connectionStringMySQL.Split(new char[] { ';' });
            txtDataBaseAddress.Text = consMySQL[0].Split(new char[] { '=' })[1];
            combox_DataBaseName.Text = consMySQL[1].Split(new char[] { '=' })[1];
            txtDataBaseUid.Text = consMySQL[2].Split(new char[] { '=' })[1];
            txtDataBasePwd.Text = consMySQL[3].Split(new char[] { '=' })[1];

            txtRegisterDir.Text = registerDataDir;
        }
        #endregion

        #region 主界面


        void ShowMsgInfo(string msgStr, Exception ex)
        {
            if (MSG_TIP_DIALOG == msgTipMode)
            {
                if (null != ex)
                {
                    msgStr += ex.Message + "\n" + ex.StackTrace;
                }

                MessageBox.Show(msgStr);
            }
            else if (MSG_TIP_LOG == msgTipMode)
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
        }


        private delegate void LocateFaceCallback(Image[] existFaceImg);
        /// <summary>
        /// 显示抓拍人脸的回掉函数
        /// </summary>
        /// <param name="existFaceImg"></param>
        void LocateFaceResult(Image[] existFaceImg)
        {
            if (0 == existFaceImg.Count())
            {
                return;
            }

            if (listViewFace.InvokeRequired)
            {
                LocateFaceCallback locateCallBack = new LocateFaceCallback(LocateFaceResult);
                this.Invoke(locateCallBack, new object[] { existFaceImg });
            }
            else
            {
                try
                {
                    int maxCount = 45;
                    int faceCount = existFaceImg.Count();
                    int imgCount = imageListFace.Images.Count;
                    int addFaceCount = faceCount;
                    int removeImgCount = faceCount;
                    int addFaceIdx = 0;

                    if (imgCount < maxCount)
                    {
                        int diffMaxCount = maxCount - imgCount;
                        int diffFaceCount = faceCount - diffMaxCount;
                        removeImgCount = diffFaceCount;
                        if (diffFaceCount > 0)
                        {
                            addFaceCount = faceCount - diffFaceCount;
                            addFaceIdx = addFaceCount;
                        }

                        for (int i = 0; i < addFaceCount; ++i)
                        {
                            Image faceImg = existFaceImg[i];
                            imageListFace.Images.Add(faceImg);
                            listViewFace.Items.Add("", 0);
                        }
                        imgCount = imageListFace.Images.Count;
                        int idxLast = imgCount - 1;
                        for (int i = 0; i <= idxLast; i++)
                        {
                            ListViewItem item = listViewFace.Items[i];
                            item.ImageIndex = idxLast - i;
                        }
                    }

                    for (int j = removeImgCount - 1; j >= 0; --j)
                    {
                        Image faceImg = existFaceImg[addFaceIdx];
                        imageListFace.Images.RemoveAt(j);
                        imageListFace.Images.Add(faceImg);
                        addFaceIdx++;
                    }

                    lbl_CatchFaceCount.Text = "抓拍张数:" + cap.CatchFaceCount.ToString();

                    listViewFace.Refresh();
                }
                catch (Exception ex)
                {
                    ShowMsgInfo("LocateFaceResult Error", ex);
                }
            }
        }

        private delegate void ShowCountCallback(int type, int count);
        void ShowCountInfo(int type, int count)
        {
            Label label = null;
            string infoStr = "";


            switch (type)
            {
                case 0:
                    label = lbl_CountCollect;
                    infoStr = "收集张数:";
                    break;
                case 1:
                    label = lbl_CountWaitDetect;
                    infoStr = "待检测队列:";
                    break;
                case 2:
                    label = lbl_CountCHC;
                    infoStr = "海康张数:";
                    break;
                case 3:
                    label = lbl_CountClean;
                    infoStr = "抓拍清理张数:";
                    break;
                case 4:
                    label = lbl_CountCapture;
                    infoStr = "抓拍队列:";
                    break;
                case 5:
                    label = lbl_CountMatchNone;
                    infoStr = "检测无效张数:";
                    break;
            }


            if (label.InvokeRequired)
            {
                ShowCountCallback showCountCallBack = new ShowCountCallback(ShowCountInfo);
                this.Invoke(showCountCallBack, new object[] { type, count });
            }
            else
            {

                try
                {
                    label.Text = infoStr + count;
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLog("ShowCountInfo Error", ex);
                }
            }

        }


        private delegate void HitAlertCallback(HitAlert[] result);

        void GetHitAlertResult(HitAlert[] result)
        {
            if (0 == result.Count())
            {
                return;
            }

            if (flowLayoutPanel.InvokeRequired)
            {
                HitAlertCallback hitCallBack = new HitAlertCallback(GetHitAlertResult);
                this.Invoke(hitCallBack, new object[] { result });
            }
            else
            {
                try
                {
                    foreach (HitAlert person in result)
                    {
                        if (null == person.QueryFace || person.Details.Length == 0)
                        {
                            continue;
                        }

                        HitAlertDetail hitInfo = person.Details[0];
                        int similarityPercent = (int)(hitInfo.Score * 100);

                        HitUserItemInfo hitItemInfo = new HitUserItemInfo();
                        bool newItem = true;

                        if (hitUserItemMap.ContainsKey(hitInfo.UserId))
                        {
                            hitItemInfo = hitUserItemMap[hitInfo.UserId];
                            if (similarityPercent <= hitItemInfo.similarityPercent)
                            {
                                continue;
                            }
                            else
                            {
                                newItem = false;
                            }
                        }

                        hitItemInfo.similarityPercent = similarityPercent;

                        if (newItem)
                        {
                            TableLayoutPanel tableLayout = new TableLayoutPanel();
                            tableLayout.ColumnCount = 3;
                            tableLayout.RowCount = 2;
                            tableLayout.Size = new Size(flowLayoutPanel.Size.Width - 7, 220);
                            ColumnStyle colStyle1 = new ColumnStyle(SizeType.Percent, 45);
                            ColumnStyle colStyle2 = new ColumnStyle(SizeType.Percent, 10);
                            ColumnStyle colStyle3 = new ColumnStyle(SizeType.Percent, 45);
                            tableLayout.ColumnStyles.Add(colStyle1);
                            tableLayout.ColumnStyles.Add(colStyle2);
                            tableLayout.ColumnStyles.Add(colStyle3);
                            RowStyle rowStyle1 = new RowStyle(SizeType.Percent, 65);
                            RowStyle rowStyle2 = new RowStyle(SizeType.Percent, 35);
                            tableLayout.RowStyles.Add(rowStyle1);
                            tableLayout.RowStyles.Add(rowStyle2);
                            tableLayout.BackColor = Color.White;
                            Padding pad = tableLayout.Margin;
                            pad.Bottom = 9;
                            tableLayout.Margin = pad;

                            //视频截获查询的帧图片
                            PictureBox picBoxQuery = new PictureBox();
                            picBoxQuery.Anchor = AnchorStyles.None;
                            picBoxQuery.Cursor = Cursors.Hand;
                            picBoxQuery.Size = new Size(130, 130);
                            picBoxQuery.SizeMode = PictureBoxSizeMode.StretchImage;
                            picBoxQuery.Image = person.QueryFace;
                            tableLayout.Controls.Add(picBoxQuery);
                            tableLayout.SetCellPosition(picBoxQuery, new TableLayoutPanelCellPosition(0, 0));
                            //匹配度
                            Label lblScore = new Label();
                            lblScore.Anchor = AnchorStyles.None;
                            lblScore.Text = similarityPercent.ToString() + "%";
                            lblScore.ForeColor = Color.Red;
                            tableLayout.Controls.Add(lblScore);
                            tableLayout.SetCellPosition(lblScore, new TableLayoutPanelCellPosition(1, 0));
                            //查找匹配到的注册图片
                            PictureBox picBoxMatch = new PictureBox();
                            picBoxMatch.Anchor = AnchorStyles.None;
                            picBoxMatch.Cursor = Cursors.Hand;
                            picBoxMatch.Size = new Size(130, 130);
                            picBoxMatch.SizeMode = PictureBoxSizeMode.StretchImage;


                            Image imgMatch = Image.FromFile(hitInfo.imgPath);
                            Image bmpMatch = new Bitmap(imgMatch);
                            imgMatch.Dispose();
                            picBoxMatch.Image = bmpMatch;

                            /*
                                                        FileStream fileStream = new FileStream(hitInfo.imgPath, FileMode.Open, FileAccess.Read);
                                                        Image imgMatch = Image.FromStream(fileStream);
                                                        fileStream.Close();
                                                        fileStream.Dispose();
                                                        picBoxMatch.Image = imgMatch;
                             */

                            //                            picBoxMatch.Image = Image.FromFile(hitInfo.imgPath);
                            tableLayout.Controls.Add(picBoxMatch);
                            tableLayout.SetCellPosition(picBoxMatch, new TableLayoutPanelCellPosition(2, 0));
                            //查找匹配到的注册信息
                            Label lblUserInfo = new Label();
                            lblUserInfo.Anchor = AnchorStyles.None;
                            lblUserInfo.Text = "peopeID:" + hitInfo.peopleId;
                            tableLayout.Controls.Add(lblUserInfo);
                            tableLayout.SetCellPosition(lblUserInfo, new TableLayoutPanelCellPosition(0, 1));
                            tableLayout.SetColumnSpan(lblUserInfo, 3);

                            if (hitUserMaxCount == flowLayoutPanel.Controls.Count)
                            {
                                flowLayoutPanel.Controls.RemoveAt(hitUserMaxCount - 1);
                            }

                            flowLayoutPanel.Controls.Add(tableLayout);
                            flowLayoutPanel.Controls.SetChildIndex(tableLayout, 0);
                            flowLayoutPanel.ScrollControlIntoView(tableLayout);

                            try
                            {
                                List<uint> userIdSet = hitUserItemMap.Keys.ToList();
                                foreach (uint userId in userIdSet)
                                {
                                    HitUserItemInfo newHitInfo = hitUserItemMap[userId];

                                    if (hitUserMaxCount - 1 == newHitInfo.itemIdx)
                                    {
                                        hitUserItemMap.Remove(userId);
                                    }
                                    else
                                    {
                                        newHitInfo.itemIdx++;
                                        hitUserItemMap[userId] = newHitInfo;
                                    }

                                }
                                hitItemInfo.itemIdx = 0;
                                hitUserItemMap.Add(hitInfo.UserId, hitItemInfo);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("newItem error:" + ex.Message);
                            }

                            cap.MatchFaceCount++;
                            lbl_CountMatch.Text = "匹配张数:" + cap.MatchFaceCount.ToString();

                        }
                        else
                        {
                            hitUserItemMap[hitInfo.UserId] = hitItemInfo;
                            TableLayoutPanel tableLayout = (TableLayoutPanel)flowLayoutPanel.Controls[hitItemInfo.itemIdx];
                            PictureBox picBoxQuery = (PictureBox)tableLayout.Controls[0];
                            Label lblScore = (Label)tableLayout.Controls[1];
                            picBoxQuery.Image = person.QueryFace;
                            lblScore.Text = similarityPercent.ToString() + "%";

                            flowLayoutPanel.ScrollControlIntoView(tableLayout);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.WriteErrorLog("GetHitAlertResult Error", ex);
                }
            }

        }

        private void SetDetectParameters()
        {
            cap.Interval = (int)nudInterval.Value;

            fa.MaxPersonNum = (int)nudMaxFaceNum.Value;
            fa.TopK = (int)nudTopK.Value;
            fa.ScoreThresh = (float)nudThresh.Value;
            fa.SearchFaceQualityThresh = (int)nudSearchFaceQualityThresh.Value;
            fa.SearchFaceHeightThresh = (int)nudSearchFaceWidthThresh.Value;
            fa.SearchFaceWidthThresh = (int)nudSearchFaceHeightThresh.Value;
            fa.SearchFaceYawThresh = (int)nudSearchYawThresh.Value;
            fa.SearchFaceRollThresh = (int)nudSearchRollThresh.Value;
            fa.SearchFacePitchThresh = (int)nudSearchPitchThresh.Value;


            fa.RegisterFaceQualityThresh = (int)nudRegisterFaceQualityThresh.Value;
            fa.RegisterFaceHeightThresh = (int)nudRegisterFaceWidthThresh.Value;
            fa.RegisterFaceWidthThresh = (int)nudRegisterFaceHeightThresh.Value;
            fa.RegisterFaceYawThresh = (int)nudRegisterYawThresh.Value;
            fa.RegisterFaceRollThresh = (int)nudRegisterRollThresh.Value;
            fa.RegisterFacePitchThresh = (int)nudRegisterPitchThresh.Value;
            fa.Times = (float)nudTimesThresh.Value;

            //fa.FaceRectScale = (float)nudFaceRectScale.Value;

            //fa.BlurrThresh = (double)nudClarityThresh.Value;

            //chcFaceDetecter.DetectImgCount = (int)nudTrackNum.Value;

        }

        public void startCaptureCHC()
        {
            SetDetectParameters();

            cap.TimeStart = DateTime.Now;
            cap.MatchFaceCount = 0;
            cap.CollectFaceCount = 0;
            cap.CatchFaceCount = 0;

            if (btnStartCapture.Text.Equals("开始"))
            {
                if (chcFaceDetecter.Start())
                {
                    btnStartCapture.Text = "停止";
                }
            }
            else
            {
                if (chcFaceDetecter.Stop())
                {
                    btnStartCapture.Text = "开始";
                }
            }
        }

        public void startCaptureNLG()
        {
            SetDetectParameters();

            cap.TimeStart = DateTime.Now;
            cap.MatchFaceCount = 0;
            cap.CollectFaceCount = 0;
            cap.CatchFaceCount = 0;

            if (!cap.IsRun)
            {
                //InitFRS();
                //InitUI();
                if (txtVideoAddress.Text.Trim() != string.Empty)
                {
                    cap.Start(txtVideoAddress.Text);
                }
                else { cap.Start(); }

                //showMedia = new Emgu.CV.Capture("rtsp://218.204.223.237:554/live/1/66251FC11353191F/e7ooqwcfbqjoo80j.sdp");

                btnStartCapture.Text = "停止";
            }
            else
            {
                cap.Stop();
                picPlayer.Image = null;
                picPlayer.Refresh();

                btnStartCapture.Text = "开始";

            }
        }

        private void StartCaptureAuto()
        {
            string dataPath = ConfigurationManager.AppSettings["DataPath"];
            bool dataPathState = cap.SetDataPath(dataPath);
            if (false == dataPathState)
            {
                MessageBox.Show("DataPath设置有问题，请检查！");
                return;
            }

            //LoginCHC();
            //startCaptureCHC();
            //startCaptureNLG();
        }

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            string dataPath = ConfigurationManager.AppSettings["DataPath"];
            //ShowMsgInfo(dataPath, null);
            bool dataPathState = cap.SetDataPath(dataPath);

            if (false == dataPathState)
            {
                MessageBox.Show("DataPath设置有问题，请检查！");
                return;
            }

            fa.LoadData(libraryname);
            if (rdb_CameraCHC.Checked)
            {
                startCaptureCHC();
            }
            else
            {
                startCaptureNLG();
            }
        }

        private void capture_ImageGrabbed()
        {

            try
            {

                Bitmap frame = null;
                lock (this)
                {
                    if (cap != null)
                    {
                        frame = cap.Retrive();
                        if (frame == null)
                        {

                            return;
                        }

                        System.GC.Collect();
                        picPlayer.Image = frame;
                        imageOnShow = frame;

                    }
                }
            }
            catch (Exception ex)
            {
#if DEBUG

                        Console.WriteLine(e.Message + System.Environment.CommandLine);
#endif
            }
        }
        //private delegate void ImageGrabbedCallback(Image img);


        //        private void capture_ImageGrabbed(Image img)
        //        {
        //            if (picPlayer.InvokeRequired)
        //            {
        //                ImageGrabbedCallback imageGrabbed = new ImageGrabbedCallback(capture_ImageGrabbed);
        //                this.Invoke(imageGrabbed, new object[] { img });
        //            }
        //            else
        //            {

        //                try
        //                {
        //                    picPlayer.Image = img;
        //                    imageOnShow = img;
        //                }
        //                catch (Exception ex)
        //                {
        //#if DEBUG

        //#endif
        //                }
        //            }
        //        }

        #endregion

        #region 注册

        #region 单张注册

        private void picRegister_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|所有文件 (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imgFilePath = openFileDialog.FileName;
                tex_RegUserName.Text = Path.GetFileNameWithoutExtension(imgFilePath);
                picRegister.ImageLocation = imgFilePath;

            }
        }

        private void btnRegisterOne_Click(object sender, EventArgs e)
        {
            string regUserName = tex_RegUserName.Text;
            string regUserGender = cb_RegUserGender.Text;
            string regUserIdCard = tex_RegUserIdCard.Text;

            if (string.IsNullOrEmpty(regUserName) || string.IsNullOrEmpty(regUserGender) || string.IsNullOrEmpty(regUserIdCard))
            {
                MessageBox.Show("请填写完整相关注册信息！");
                return;
            }
            if (cap.IsRun)
            {
                btnStartCapture_Click(sender, e);
                MessageBox.Show("监控已停止");
            }
            if (picRegister.Image == null) return;

            //fa.BlurrThresh = (double)nudClarityThresh.Value;

            FRS.FRSFacePos[] faces = new FRSFacePos[1];

            int face_num = FaceImage.DetectFace(0, picRegister.Image, 24, faces, 1);
            if (faces == null || faces.Length == 0 || face_num < 0)
            {
                MessageBox.Show("图中无人脸");
                return;
            }
            if (faces.Length > 1)
            {
                MessageBox.Show("图中人脸数大于1");
                return;
            }
            Int32 x = faces[0].rcFace.left;
            Int32 y = faces[0].rcFace.top;
            Int32 width = faces[0].rcFace.right - faces[0].rcFace.left;
            Int32 height = faces[0].rcFace.bottom - faces[0].rcFace.top;


            //Image img=picRegister.Image.Clone();
            //Graphics g = Graphics.FromImage(picRegister.Image);

            //Brush brush = new SolidBrush(Color.Red);
            //Pen pen = new Pen(brush, 2);


            //g.DrawRectangle(pen, new Rectangle(x, y, width, height));

            //g.Dispose();

            UserInfo hitUserInfo = new UserInfo();
            hitUserInfo.name = regUserName;
            hitUserInfo.gender = regUserGender;
            hitUserInfo.cardId = regUserIdCard;

            Bitmap faceBitmap = new Bitmap(width, height);
            Graphics.FromImage(faceBitmap);
            Graphics g = Graphics.FromImage(faceBitmap);
            g.DrawImage(picRegister.Image, new Rectangle(0, 0, width, height), new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            g.Dispose();
            picRegisterFace.Image = faceBitmap;
            if (0 == fa.Register(picRegister.Image, hitUserInfo))
            {
                fa.LoadData(libraryname);
                MessageBox.Show("注册成功");
            }
            else
            {
                MessageBox.Show("注册失败，请检查");
            }

        }
        private void btnRegisterGetCurrentFrame_Click(object sender, EventArgs e)
        {
            picRegister.Image = imageOnShow;
        }

        #endregion

        #region 批量注册,文件名为用户名
        /// <summary>
        /// 每注册一张触发
        /// </summary>
        /// <param name="msg"></param>
        private void RegisterOneFinised(int cout, string msg)
        {
            if (bwRegisterInBulk.CancellationPending) //这里判断一下是否用户要求取消后台进行，并可以尽早退出。
            {
                bwRegisterInBulk.ReportProgress(cout, msg);

            }

            //处理的过程中，通过这个函数，向主线程报告处理进度，最好是折算成百分比，与外边的进度条的最大值必须要对应。这里，我没有折算，而是把界面线程的进度条最大值调整为与这里的总数一致。
            bwRegisterInBulk.ReportProgress(cout, msg);

        }

        private void btnRegisterInBulk_Click(object sender, EventArgs e)
        {
            registerType = REG_IN_BUILK_FROM_DIR;
            if (string.IsNullOrEmpty(txtRegisterDir.Text)) { MessageBox.Show("路径不能为空"); return; }
            if (cap.IsRun)
            {
                btnStartCapture_Click(sender, e);
                MessageBox.Show("监控已停止");
            }
            //if (fa.RegisterInBulk(txtRegisterDir.Text) == 0)
            //{
            //    MessageBox.Show("注册成功");
            //}
            //fa.BlurrThresh = (double)nudClarityThresh.Value;
            btnRegisterInBulk.Enabled = false;
            bwRegisterInBulk.RunWorkerAsync();
        }


        private void bwRegisterInBulk_DoWork(object sender, DoWorkEventArgs e)
        {
            if (registerType == REG_IN_BUILK_FROM_DIR)//从文件夹中注册
                //fa.RegisterInBulk1(txtRegisterDir.Text);
                fa.RegisterInBulk1(txtRegisterDir.Text, libraryname);
            else if (registerType == REG_IN_BUILK_FROM_FILE)//从文件中注册
            {
                fa.RegisterInBulkFromFile(txtRegisterFile.Text);
            }

        }

        private void bwRegisterInBulk_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //rtxRegisterLog.AppendText(e.ProgressPercentage + ":" + e.UserState + "\r\n");
            ShowRegisterLog(e.ProgressPercentage + ":" + e.UserState);
        }

        private void bwRegisterInBulk_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("注册完成");
            btnRegisterInBulk.Enabled = true;
        }

        private void btnSelectRegisterDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            if (folderDlg.ShowDialog() == DialogResult.OK)
            {
                txtRegisterDir.Text = folderDlg.SelectedPath;
                registerDataDir = folderDlg.SelectedPath;

            }
        }
        #endregion

        void ShowRegisterLog(string log)
        {
            if (rtxRegisterLog.Lines.Length > 10)
            { rtxRegisterLog.Clear(); }

            //========richtextbox滚动条自动移至最后一条记录
            //让文本框获取焦点 
            rtxRegisterLog.Focus();
            //设置光标的位置到文本尾 
            rtxRegisterLog.Select(rtxRegisterLog.TextLength, 0);
            //滚动到控件光标处 
            rtxRegisterLog.ScrollToCaret();
            Action<string> ac = (x) => { rtxRegisterLog.AppendText(x + "\r\n"); };
            rtxRegisterLog.Invoke(ac, log);
            rtxRegisterLog.SelectionStart = rtxRegisterLog.Text.Length - 1;
            rtxRegisterLog.Invalidate();


        }
        #region 从文件批量注册
        private void btnRegisterInBulkFromFile_Click(object sender, EventArgs e)
        {
            int registerType = REG_IN_BUILK_FROM_FILE;
            if (string.IsNullOrEmpty(txtRegisterFile.Text)) { MessageBox.Show("文件不能为空"); return; }

            if (cap.IsRun)
            {
                btnStartCapture_Click(sender, e);
                MessageBox.Show("监控已停止");
            }

            //fa.BlurrThresh = (double)nudClarityThresh.Value;
            btnRegisterInBulkFromFile.Enabled = false;
            bwRegisterInBulk.RunWorkerAsync();



        }


        private void selectRegisterFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "(*.csv;*.txt;)|*.csv;*.txt;|所有文件 (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                txtRegisterFile.Text = openFileDialog.FileName;

            }
        }
        #endregion
        #endregion

        #region 窗口关闭
        private void FormMian_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (cap.IsRun)
            {
                btnStartCapture_Click(sender, e);
            }
            Application.Exit();
        }
        #endregion


        #region 检索命中纪录
        private void UpdateDGVHitAlert(DataTable dt)
        {
            dgvHitAlert.Rows.Clear();
            for (int i = dt.Rows.Count - 1; i >= 0 && dt.Rows.Count - i < 1000; i--)
            {

                DataGridViewRow dgvr = new DataGridViewRow();
                dgvr.CreateCells(dgvHitAlert);

                dgvr.Cells[0].Value = dt.Rows[i]["occur_time"].ToString();
                try
                {
                    dgvr.Cells[1].Value = Image.FromFile(dt.Rows[i]["face_query_image_path"].ToString());
                }
                catch { }
                try
                {
                    dgvr.Cells[2].Value = Image.FromFile(dt.Rows[i]["user_face_image_path"].ToString());
                }
                catch { };
                dgvr.Cells[3].Value = dt.Rows[i]["user_id"].ToString();
                dgvr.Cells[4].Value = dt.Rows[i]["score"].ToString();
                dgvr.Cells[5].Value = dt.Rows[i]["user_quality_score"].ToString();
                dgvr.Cells[6].Value = dt.Rows[i]["id"].ToString();
                dgvr.Height = 80;

                Action<DataGridViewRow> action = (x) => { dgvHitAlert.Rows.Add(x); };
                dgvHitAlert.Invoke(action, dgvr);
            }
        }
        private void btnRetriew_Click(object sender, EventArgs e)
        {
                   
            DataSet ds = hitbll.GetListByTime(dtpStart.Value, dtpend.Value, libraryname);

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }

            Count_Record_HitAlert = ds.Tables[0].Rows.Count;
            txt_Count_Record_HitAlert.Text = Count_Record_HitAlert.ToString();
            txt_PageSize_HitAlert.Text = PageSize_HitAlert.ToString();
            Count_Page_HitAlert = Count_Record_HitAlert % PageSize_HitAlert == 0 ? Count_Record_HitAlert / PageSize_HitAlert : Count_Record_HitAlert / PageSize_HitAlert + 1;
            txt_Count_Page_HitAlert.Text = Count_Page_HitAlert.ToString();

            try
            {
                ds = hitbll.GetListByTime(dtpStart.Value, dtpend.Value, 0, PageSize_HitAlert, libraryname);

                CurPage_HitAlert = 1;
                txt_Current_Page_HitAlert.Text = CurPage_HitAlert.ToString(); 

                UpdateDGVHitAlert(ds.Tables[0]);
               
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }

            
           
        }
        #endregion

        #region 数据库测试
        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            if (fa.LoadData(combox_DataBaseName.Text.ToString())==0)
            {
                libraryname = combox_DataBaseName.Text.ToString();
                MessageBox.Show("登录成功");
            }
            else
            {
                MessageBox.Show("登录失败");
            }
            //if (DataAngine.DBUtility.DbHelperMySQL.ConnectionTest())
            //{
            //    MessageBox.Show("连接成功");
            //}
            //else
            //{
            //    MessageBox.Show("连接失败");
            //}
        }
        #endregion

        # region 登陆/退出 海康摄像机
        public bool LoginCHC()
        {
            bool ret = false;

            chcFaceDetecter.DVRIPAddress = tex_DevIP.Text;
            chcFaceDetecter.DVRPortNumber = Int16.Parse(tex_DevPort.Text);
            chcFaceDetecter.DVRUserName = tex_UserName.Text;
            chcFaceDetecter.DVRPassword = tex_UserPwd.Text;

            if (btn_LoginDevice.Text.Equals("登录"))
            {
                ret = chcFaceDetecter.LoginDVR();
                if (ret)
                {
                    ShowMsgInfo("登录成功", null);
                    btn_LoginDevice.Text = "退出";
                }
            }
            else
            {
                ret = chcFaceDetecter.LogoutDVR();
                if (ret)
                {
                    ShowMsgInfo("已退出！", null);
                    btn_LoginDevice.Text = "登录";
                }
            }

            return ret;
        }

        private void btn_LoginCHC_Click(object sender, EventArgs e)
        {
            //LoginCHC();

            devicename = txt_device_name.Text;
            string DeviceName = txt_device_name.Text;
            string DeviceIp = tex_DevIP.Text;
            string DevicePort = tex_DevPort.Text;
            string DeviceUser = tex_UserName.Text;
            string DevicePsw = tex_UserPwd.Text;

            loginrtsp = "rtsp://" + DeviceUser + ":" + DevicePsw + "@" + DeviceIp + ":554";
            txtVideoAddress.Text = loginrtsp;
            MessageBox.Show("登录成功");
        }
        # endregion
        private void btn_ExportResult_Click(object sender, EventArgs e)
        {
            DataSet ds = hitbll.GetListByTime(dtpStart.Value, dtpend.Value);

            if (0 == ds.Tables.Count)
            {
                return;
            }

            string dirName = "比对成果导出集";
            if (false == Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            dirName = Path.Combine(dirName, dtpStart.Value.ToString("MM-dd-HH-mm-ss") + "到" + dtpend.Value.ToString("MM-dd-HH-mm-ss"));
            if (false == Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }

            DataTable dt = ds.Tables[0];

            int faceCount = dt.Rows.Count;
            for (int i = 0; i < faceCount; ++i)
            {
                DataRow row = dt.Rows[i];
                string userIdStr = dt.Rows[i]["user_id"].ToString();
                string userScoreStr = dt.Rows[i]["score"].ToString();
                string queryImgPath = dt.Rows[i]["face_query_image_path"].ToString();
                string matchImgPath = dt.Rows[i]["user_face_image_path"].ToString();

                string dirUser = Path.Combine(dirName, userIdStr);
                if (false == Directory.Exists(dirUser))
                {
                    Directory.CreateDirectory(dirUser);
                }

                Image queryImg = Image.FromFile(queryImgPath);
                Image matchImg = Image.FromFile(matchImgPath);
                string queryUserPath = Path.Combine(dirUser, Path.GetFileName(queryImgPath));
                string matchUserPath = Path.Combine(dirUser, Path.GetFileName(matchImgPath));
                queryImg.Save(queryUserPath);
                matchImg.Save(matchUserPath);
            }

            ShowMsgInfo("导出成功！", null);
        }

        private void btn_DetectParamsUpdate_Click(object sender, EventArgs e)
        {
            SetDetectParameters();
            ShowMsgInfo("参数已更新！", null);
        }

        private void btn_PicCompare_Click(object sender, EventArgs e)
        {
            float score = fa.Compare(imageCompare1, imageCompare2);
            MessageBox.Show("图像相似度为" + score);
            return;
        }

        private void picCompare1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openfile = new OpenFileDialog())
            {
                DialogResult dr = openfile.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string filename = openfile.FileName;
                    this.picCompare1.Image = Image.FromFile(filename);
                    imageCompare1 = Image.FromFile(filename);
                }
            }
        }

        private void picCompare2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openfile = new OpenFileDialog())
            {
                DialogResult dr = openfile.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string filename = openfile.FileName;
                    this.picCompare2.Image = Image.FromFile(filename);
                    imageCompare2 = Image.FromFile(filename);
                }
            }
        }

        private void btn_ViewLibrary_Click(object sender, EventArgs e)
        {
            listViewLibrary.Items.Clear();
            imageListFaceLibrary.Images.Clear();

            DataSet ds = user.GetPicPathList(null,libraryname);

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }

            Count_Record_Library = ds.Tables[0].Rows.Count;
            txt_Count_Record.Text = Count_Record_Library.ToString();
            txt_PageSize.Text = PageSize_Library.ToString();
            Count_Page_Library = Count_Record_Library % PageSize_Library == 0 ? Count_Record_Library / PageSize_Library : Count_Record_Library / PageSize_Library + 1;
            txt_Count_Page.Text = Count_Page_Library.ToString();
  
            try
            {
                ds = user.GetPicPathList(null, 0, PageSize_Library, libraryname);
                DataTable dt = ds.Tables[0];

                CurPage_Library = 1;
                txt_Current_Page.Text = CurPage_Library.ToString();     

                int faceCount = dt.Rows.Count;

                for (int i = 0; i < faceCount; ++i)
                {
                    DataRow row = dt.Rows[i];
                    string userImagePath = dt.Rows[i]["face_image_path"].ToString();
                    Image face = Image.FromFile(userImagePath);
                    imageListFaceLibrary.Images.Add(face);
                    listViewLibrary.Items.Add("", 0);
                    listViewLibrary.Items[i].ImageIndex = i;

                }
                listViewLibrary.Refresh();
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }
        }

        private void btn_PrePage_Click(object sender, EventArgs e)
        {
            if (CurPage_Library <= 1)
            {
                MessageBox.Show("已经到首页");
                return;
            }

            CurPage_Library--;
            listViewLibrary.Items.Clear();
            imageListFaceLibrary.Images.Clear();

            DataSet ds = user.GetPicPathList(null, (CurPage_Library - 1) * PageSize_Library, PageSize_Library, libraryname);

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }          

            try
            {                
                DataTable dt = ds.Tables[0];

                txt_Current_Page.Text = CurPage_Library.ToString();               

                int faceCount = dt.Rows.Count;

                for (int i = 0; i < faceCount; ++i)
                {
                    DataRow row = dt.Rows[i];
                    string userImagePath = dt.Rows[i]["face_image_path"].ToString();
                    Image face = Image.FromFile(userImagePath);
                    imageListFaceLibrary.Images.Add(face);
                    listViewLibrary.Items.Add("", 0);
                    listViewLibrary.Items[i].ImageIndex = i;

                }
                listViewLibrary.Refresh();
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }
        }

        private void btn_NextPage_Click(object sender, EventArgs e)
        {
            if (CurPage_Library >= Count_Page_Library)
            {
                MessageBox.Show("已经到末页");
                return;
            }

            CurPage_Library++;
            listViewLibrary.Items.Clear();
            imageListFaceLibrary.Images.Clear();

            DataSet ds = user.GetPicPathList(null, (CurPage_Library - 1) * PageSize_Library, PageSize_Library, libraryname);

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }

            try
            {
                DataTable dt = ds.Tables[0];

                txt_Current_Page.Text = CurPage_Library.ToString();

                int faceCount = dt.Rows.Count;

                for (int i = 0; i < faceCount; ++i)
                {
                    DataRow row = dt.Rows[i];
                    string userImagePath = dt.Rows[i]["face_image_path"].ToString();
                    Image face = Image.FromFile(userImagePath);
                    imageListFaceLibrary.Images.Add(face);
                    listViewLibrary.Items.Add("", 0);
                    listViewLibrary.Items[i].ImageIndex = i;

                }
                listViewLibrary.Refresh();
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }
        }

        private void btn_GoPage_Click(object sender, EventArgs e)
        {
            int TmpPage = CurPage_Library;
            try
            {
                CurPage_Library = Convert.ToInt32(txt_Current_Page.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("页数错误");
                return;
            }

            if (CurPage_Library < 1 || CurPage_Library > Count_Page_Library)
            {
                MessageBox.Show("页数错误");
                CurPage_Library = TmpPage;
                return;
            }

            listViewLibrary.Items.Clear();
            imageListFaceLibrary.Images.Clear();

            DataSet ds = user.GetPicPathList(null, (CurPage_Library - 1) * PageSize_Library, PageSize_Library, libraryname);

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }

            try
            {
                DataTable dt = ds.Tables[0];

                txt_Current_Page.Text = CurPage_Library.ToString();

                int faceCount = dt.Rows.Count;

                for (int i = 0; i < faceCount; ++i)
                {
                    DataRow row = dt.Rows[i];
                    string userImagePath = dt.Rows[i]["face_image_path"].ToString();
                    Image face = Image.FromFile(userImagePath);
                    imageListFaceLibrary.Images.Add(face);
                    listViewLibrary.Items.Add("", 0);
                    listViewLibrary.Items[i].ImageIndex = i;

                }
                listViewLibrary.Refresh();
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }
        }

        private void pic_Library_Compare_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openfile = new OpenFileDialog())
            {
                DialogResult dr = openfile.ShowDialog();
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    string filename = openfile.FileName;
                    this.pic_Library_Compare.Image = Image.FromFile(filename);
                    image_Library_Compare = Image.FromFile(filename);
                }
            }
        }

        private void btn_Library_Compare_Click(object sender, EventArgs e)
        {
            if (image_Library_Compare == null)
            {
                MessageBox.Show("请选择一张图片");
            }
            fa.LoadData(libraryname);
            HitAlert[] hits = fa.Search(image_Library_Compare);
            if (hits != null)
            {
                this.pic_Library.Image = Image.FromFile(hits[0].Details[0].imgPath);

                MessageBox.Show("图像相似度为" + hits[0].Details[0].Score);
            }
            else
            {
                MessageBox.Show("未匹配到相似人员");
            }
            return;
        }

        /// <summary>
        /// 绑定数据到组合框
        /// </summary>
        public void ComboxLibraryList()
        {
            DataSet ds = frs_database.GetAllList();
            DataTable dt = ds.Tables[0];

            combox_DataBaseName.DataSource = dt;
            combox_DataBaseName.ValueMember = "id";
            combox_DataBaseName.DisplayMember = "name";
          
        }

        public void ComboxDevList()
        {
            DataSet ds = device.GetAllList();
            DataTable dt = ds.Tables[0];

            comboBox_DevName.DataSource = dt;
            comboBox_DevName.ValueMember = "id";
            comboBox_DevName.DisplayMember = "name";

        }

        private void btn_Library_Register_Click(object sender, EventArgs e)
        {
            libraryname = txt_library_name.Text;
            string regLibraryUid = txt_library_uid.Text;
            string regLibraryPsw = txt_library_psw.Text;

            if (string.IsNullOrEmpty(libraryname))
            {
                MessageBox.Show("请填写完整相关注册信息！");
                return;
            }

            DataAgine_Set.Model.frs_database frs_database = new DataAgine_Set.Model.frs_database();
            DataAgine_Set.BLL.frs_database frs_databasebll = new DataAgine_Set.BLL.frs_database();
            frs_database.name = libraryname;

            if (true == frs_databasebll.Add(frs_database))
            {
                combox_DataBaseName.Text = libraryname;
                fa.LoadData(libraryname);
                ComboxLibraryList();
                MessageBox.Show("注册成功");
            }
            else
            {
                MessageBox.Show("注册失败，请检查");
            }
        }

        private void btn_Device_Register_Click(object sender, EventArgs e)
        {
            devicename = txt_device_name.Text;
            string regDeviceName = txt_device_name.Text;
            string regDeviceIp = txt_device_ip.Text;
            string regDevicePort = txt_device_port.Text;
            string regDeviceUser = txt_device_user.Text;
            string regDevicePsw = txt_device_password.Text;

            if (string.IsNullOrEmpty(devicename))
            {
                MessageBox.Show("请填写完整相关注册信息！");
                return;
            }

            DataAgine_Set.Model.device device = new DataAgine_Set.Model.device();
            DataAgine_Set.BLL.device devicebll = new DataAgine_Set.BLL.device();

            device.name = regDeviceName;
            device.ip = regDeviceIp;
            device.port = regDevicePort;
            device.user = regDeviceUser;
            device.password = regDevicePsw;

            if (true == devicebll.Add(device))
            {
                comboBox_DevName.Text = devicename;
                ComboxDevList();
                MessageBox.Show("注册成功");
            }
            else
            {
                MessageBox.Show("注册失败，请检查");
            }
        }

        private void comboBox_DevName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = device.GetDevice(comboBox_DevName.Text.ToString());
            DataTable dt = ds.Tables[0];

            tex_DevIP.Text = dt.Rows[0]["ip"].ToString();
            tex_DevPort.Text = dt.Rows[0]["port"].ToString();
            tex_UserName.Text = dt.Rows[0]["user"].ToString();
            tex_UserPwd.Text = dt.Rows[0]["password"].ToString();
        }

        private void btn_GoPage_HitAlert_Click(object sender, EventArgs e)
        {
            int TmpPage = CurPage_HitAlert;
            try
            {
                CurPage_HitAlert = Convert.ToInt32(txt_Current_Page_HitAlert.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("页数错误");
                return;
            }

            if (CurPage_HitAlert < 1 || CurPage_HitAlert > Count_Page_HitAlert)
            {
                MessageBox.Show("页数错误");
                CurPage_HitAlert = TmpPage;
                return;
            }

            DataSet ds = hitbll.GetListByTime(dtpStart.Value, dtpend.Value, (CurPage_HitAlert - 1) * PageSize_HitAlert, PageSize_HitAlert, libraryname);

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }

            try
            {
                txt_Current_Page_HitAlert.Text = CurPage_HitAlert.ToString();
                UpdateDGVHitAlert(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }
        }

        private void btn_PrePage_HitAlert_Click(object sender, EventArgs e)
        {
            if (CurPage_HitAlert <= 1)
            {
                MessageBox.Show("已经到首页");
                return;
            }

            CurPage_HitAlert--;

            DataSet ds = hitbll.GetListByTime(dtpStart.Value, dtpend.Value, (CurPage_HitAlert - 1) * PageSize_HitAlert, PageSize_HitAlert, libraryname); ;

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }

            try
            {
                txt_Current_Page_HitAlert.Text = CurPage_HitAlert.ToString();
                UpdateDGVHitAlert(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }
        }

        private void btn_NextPage_HitAlert_Click(object sender, EventArgs e)
        {

            if (CurPage_HitAlert >= Count_Page_HitAlert)
            {
                MessageBox.Show("已经到末页");
                return;
            }

            CurPage_HitAlert++;

            DataSet ds = hitbll.GetListByTime(dtpStart.Value, dtpend.Value, (CurPage_HitAlert - 1) * PageSize_HitAlert, PageSize_HitAlert, libraryname);

            if (0 == ds.Tables.Count)
            {
                MessageBox.Show("未查询到符合人员");
                return;
            }

            try
            {
                txt_Current_Page_HitAlert.Text = CurPage_HitAlert.ToString();
                UpdateDGVHitAlert(ds.Tables[0]);
            }
            catch (Exception ex)
            {
                ShowMsgInfo("LocateFaceLibrary Error", ex);
            }
        }   

    }
}
