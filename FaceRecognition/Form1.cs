using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using FRS;

namespace FaceRecognition
{
    public partial class Form1 : Form
    {
        string[] fn_gen;
        string path;
        FeatureData fa = new FeatureData();

        public Form1()
        {
            InitFRS();
            fa.LoadData();
            //MessageBox.Show("load data finished!");
            InitializeComponent();    
        }

        static void InitFRS()
        {
            //System.Console.WriteLine(System.Environment.CurrentDirectory);
            FRSParam param = new FRSParam();

            param.nMinFaceSize = 50;
            param.nRollAngle = 10;
            param.bOnlyDetect = true;

            FaceImage.Create(1, param);
            Feature.Init(1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = "D:";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                this.Text_Div_Gen.Text = fbd.SelectedPath;
                path = fbd.SelectedPath;
                fn_gen = Directory.GetFiles(fbd.SelectedPath);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {   
            foreach (string gen in fn_gen)
            {
                this.Text_Div_Cur.Text = gen;
                //MessageBox.Show(gen);
                Image image = Image.FromFile(gen);
                HitAlert[] hits = fa.Search(image);
            }
            MessageBox.Show("finished!");
        }
    }
}
