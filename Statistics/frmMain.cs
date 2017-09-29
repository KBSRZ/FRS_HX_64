using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DataAngine.BLL;
namespace Statistics
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        DataAngine.BLL.hitalert habll = new hitalert();
        DataAngine.BLL.hitrecord hrbll = new hitrecord();
        #region 导出命中纪录
        private void btnExportHitrecordPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                txtHitrecordPath.Text = fd.SelectedPath;
            }
        }
        private void btnExportHitrecord_Click(object sender, EventArgs e)
        {

            btnExportHitrecord.Enabled = false;
            DataSet ds = habll.GetAllList();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                DateTime dt = DateTime.Parse(dr["occur_time"].ToString());
                string savePath = Path.Combine(txtHitrecordPath.Text, dt.ToString("yyyy-MM-dd-HH-mm-ss"));
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }
                Console.WriteLine(Environment.CurrentDirectory);
                string saveQueryFacePath = Path.Combine(savePath, dr["id"].ToString() + ".jpg");
                if (!File.Exists(saveQueryFacePath))
                {
                    string queryFacePath = Environment.CurrentDirectory.Replace('\\', '/') + "/" + dr["face_query_image_path"].ToString();
                    queryFacePath = queryFacePath.Replace("/", "\\");
                    System.IO.File.Copy(queryFacePath, saveQueryFacePath);

                }
                string saveCandinateFacePath = Path.Combine(savePath, dr["id"].ToString() + "_" + dr["user_id"].ToString() + "_" + dr["score"] + ".jpg");

                string user_face_image_path = Environment.CurrentDirectory.Replace('\\', '/') + "/" + dr["user_face_image_path"].ToString();
                user_face_image_path = user_face_image_path.Replace("/", "\\");
                System.IO.File.Copy(user_face_image_path, saveCandinateFacePath);


            }
            MessageBox.Show("导出成功");
            btnExportHitrecord.Enabled = true;
        }
        #endregion 

        private void btnExportQueryFace_Click(object sender, EventArgs e)
        {

            btnSelectQueryFacePath.Enabled = false;
            DataSet ds = hrbll.GetAllList();
            int count = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
               
                DateTime dt = DateTime.Parse(dr["occur_time"].ToString());
                string savePath = Path.Combine(txtQueryPath.Text, dt.ToString("yyyy-MM-dd-HH-mm-ss"));
                if (!Directory.Exists(savePath))
                {
                    count = 0;
                    Directory.CreateDirectory(savePath);
                }
                else
                {
                    count++;
                }

                string saveQueryFacePath = Path.Combine(savePath, count + ".jpg");

                string user_face_image_path = Environment.CurrentDirectory.Replace('\\', '/') + "/" + dr["face_query_image_path"].ToString();
                user_face_image_path = user_face_image_path.Replace("/", "\\");
                System.IO.File.Copy(user_face_image_path, saveQueryFacePath);


            }
            MessageBox.Show("导出成功");
            btnSelectQueryFacePath.Enabled = true;

        }

        private void btnSelectQueryFacePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                txtQueryPath.Text = fd.SelectedPath;
            }
        }



      

       
    }
}
