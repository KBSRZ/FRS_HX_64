using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;


namespace FaceChecker
{
    public partial class Form1 : Form
    {
        public static string dataDirPath = "";
        public static int countItemExist = 0;
        public static int countItemProcessed = 0;
        public static int countCapture = 0;
        public static int countMatch = 0;
        public static int countMatchExact = 0;

        public static Regex rexMatch = new Regex(@"^\d+$");
        public static string dirPathCur = "";
        public static string filePathCapture = "";
        public static string filePathMatch = "";
        public static string fileNameCapture = "";
        public static string fileNameMatch = "";

        public static bool matchState = false;
        public static AutoResetEvent doCheckEvent = new AutoResetEvent(false);

        public delegate void ShowImageCallback(int type, Image img, string imgName);
        public static event ShowImageCallback ShowImageEvent;

        public delegate void ShowCollectInfoCallback(int type, string info);
        public static event ShowCollectInfoCallback ShowCollectInfoEvent;

        public Form1()
        {
            InitializeComponent();

            ShowImageEvent += new ShowImageCallback(ShowImage);
            ShowCollectInfoEvent += new ShowCollectInfoCallback(ShowCollectInfo);

            dataDirPath = ConfigurationManager.AppSettings["DataDirPath"];

            if (null == dataDirPath)
            {
                MessageBox.Show("程序配置文件中无法找到数据目录相关配置，请检查！");
                return;
            }

            if (dataDirPath.Equals("") || false == Directory.Exists(dataDirPath))
            {
                MessageBox.Show("未指明数据目录或目录不存在，请检查！");
                return;
            }

        }

        public static void DoCheck(object obj)
        {
            dirPathCur = "";
            filePathCapture = "";
            filePathMatch = "";
            fileNameCapture = "";
            fileNameMatch = "";

            string[] dirPathSet = Directory.GetDirectories(dataDirPath);
            countItemExist = dirPathSet.Length;

            countItemProcessed = 0;

            while (countItemProcessed < countItemExist)
            {
                countItemProcessed++;

                dirPathCur = Path.Combine(dataDirPath, countItemProcessed.ToString());
                filePathCapture = "";
                filePathMatch = "";

                string[] filePathSet = Directory.GetFiles(dirPathCur);
                int fileCount = filePathSet.Length;

                for (int i = 0; i < fileCount; ++i)
                {
                    string filePath = filePathSet[i];
                    int idxName=filePath.LastIndexOf("\\") + 1;
                    string fileName = filePath.Substring(idxName);

                    if (fileName.Contains("CHC"))
                    {
                        countCapture++;
                    }

                    if (fileName.Contains("NLG"))
                    {
                        filePathCapture = filePath;
                        fileNameCapture = fileName;
                    }

                    string fileNameRex = fileName.Substring(0, fileName.Length - 4);

                    if (rexMatch.IsMatch(fileNameRex))
                    {
                        filePathMatch = filePath;
                        fileNameMatch = fileName;
                        countMatch++;
                    }

                }

                if (false == filePathCapture.Equals("") && false == filePathMatch.Equals(""))
                {
                    Image imgCapture=Image.FromFile(filePathCapture);
                    ShowImageEvent(0, imgCapture, fileNameCapture);

                    Image imgMatch = Image.FromFile(filePathMatch);
                    ShowImageEvent(1, imgMatch, fileNameMatch);

                    doCheckEvent.WaitOne();

                    if (matchState)
                    {
                        countMatchExact++;
                    }
                }

                string itemExistInfo = "当前总项数:" + countItemExist;
                ShowCollectInfoEvent(3, itemExistInfo);

                string itemProcessedInfo = "已处理项数:" + countItemProcessed;
                ShowCollectInfoEvent(4, itemProcessedInfo);

                string captureInfo = "抓拍张数:" + countCapture;
                ShowCollectInfoEvent(0, captureInfo);

                string matchInfo = "匹配张数:" + countMatch;
                ShowCollectInfoEvent(1, matchInfo);

                string matchExactInfo = "精确张数:" + countMatchExact;
                ShowCollectInfoEvent(2, matchExactInfo);
        
                dirPathSet = Directory.GetDirectories(dataDirPath);
                countItemExist = dirPathSet.Length;

            }

            MessageBox.Show("所有项已检查完毕！");

        }

        void ShowImage(int pcbType, Image img, string imgName)
        {
            PictureBox pcb = null;
            Label lbl = null;
            if (0 == pcbType)
            {
                pcb = pcb_Capture;
                lbl = lbl_Capture;
            }
            else
            {
                pcb = pcb_Match;
                lbl = lbl_Match;
            }

            if (pcb.InvokeRequired)
            {
                ShowImageCallback showImgCallBack = new ShowImageCallback(ShowImage);
                this.Invoke(showImgCallBack, new object[] { pcbType, img, imgName });
            }
            else
            {
                pcb.Image = img;
                lbl.Text = imgName;
            }

        }

        void ShowCollectInfo(int lblType, string info)
        {
            Label lbl = null;
            switch (lblType)
            {
                case 0: lbl = lbl_CountCapture; break;
                case 1: lbl = lbl_CountMatch; break;
                case 2: lbl = lbl_CountMatchExact; break;
                case 3: lbl = lbl_CountItemExist; break;
                case 4: lbl = lbl_CountItemProcessed; break;
            }

            if (lbl.InvokeRequired)
            {
                ShowCollectInfoCallback showInfoCallBack = new ShowCollectInfoCallback(ShowCollectInfo);
                this.Invoke(showInfoCallBack, new object[] { lblType, info });
            }
            else
            {
                lbl.Text = info;
            }
        }


        public static void DoClean()
        {
            dirPathCur = "";
            filePathCapture = "";
            filePathMatch = "";

            string[] dirPathSet = Directory.GetDirectories(dataDirPath);
            countItemExist = dirPathSet.Length;

            countItemProcessed = 0;

            while (countItemProcessed < countItemExist)
            {
                countItemProcessed++;

                dirPathCur = Path.Combine(dataDirPath, countItemProcessed.ToString());
                filePathCapture = "";
                filePathMatch = "";

                string[] filePathSet = Directory.GetFiles(dirPathCur);
                int fileCount = filePathSet.Length;

                for (int i = 0; i < fileCount; ++i)
                {
                    string filePath = filePathSet[i];
                    int idxName = filePath.LastIndexOf("\\") + 1;
                    string fileName = filePath.Substring(idxName);

                    if (fileName.Contains("NLG"))
                    {
                        filePathCapture = filePath;
                    }

                    string fileNameRex = fileName.Substring(0, fileName.Length - 4);

                    if (rexMatch.IsMatch(fileNameRex))
                    {
                        filePathMatch = filePath;
                    }
                }

                if (false == filePathCapture.Equals("") && false == filePathMatch.Equals(""))
                {
                    File.Delete(filePathCapture);
                    File.Delete(filePathMatch);
                }
            }

            MessageBox.Show("所有项已整理完毕！");
        }

        private void btn_Match_Click(object sender, EventArgs e)
        {
            matchState = true;
            doCheckEvent.Set();
        }

        private void btn_UnMatch_Click(object sender, EventArgs e)
        {
            matchState = false;
            doCheckEvent.Set();
        }

        private void btn_LastItem_Click(object sender, EventArgs e)
        {

        }

        private void btn_NextItem_Click(object sender, EventArgs e)
        {

        }

        private void btn_StartCheck_Click(object sender, EventArgs e)
        {
            Thread checkThread = new Thread(new ParameterizedThreadStart(DoCheck));
            checkThread.IsBackground = true;
            checkThread.Start();
        }

        private void btn_Clean_Click(object sender, EventArgs e)
        {
            DoClean();
        }
    }
}
