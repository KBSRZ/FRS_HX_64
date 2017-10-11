using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyRemoteTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Copy_Click(object sender, EventArgs e)
        {
            string fileName = "TestFile.txt";
            string filePath = Path.GetFullPath(fileName);

            string remoteFilePath = Path.Combine(tex_RemoteDir.Text, fileName);

            File.Copy(filePath, remoteFilePath, true);

            MessageBox.Show("文件已上传!");
        }
    }
}
