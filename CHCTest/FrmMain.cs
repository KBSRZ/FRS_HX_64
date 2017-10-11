using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CHCTest
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            Init();
        }
        void Init()
        {
            chcFaceDetecter = new CHCFaceDetecter(picPlayer);
            chcFaceDetecter.InitCHC();
            chcFaceDetecter.ShowMsgEvent += new CHCFaceDetecter.ShowMsgCallback(ShowMsgInfo);
            
          
        }
        void ShowMsgInfo(string msgStr, Exception ex)
        {
           
                if (null != ex)
                {
                    msgStr += ex.Message + "\n" + ex.StackTrace;
                }

                MessageBox.Show(msgStr);
            
        }
        private CHCFaceDetecter chcFaceDetecter;
        public bool LoginCHC()
        {
            bool ret = false;

            chcFaceDetecter.DVRIPAddress = tex_DevIP.Text;
            chcFaceDetecter.DVRPortNumber = Int16.Parse(tex_DevPort.Text);
            chcFaceDetecter.DVRUserName = tex_UserName.Text;
            chcFaceDetecter.DVRPassword = tex_UserPwd.Text;

            if (btn_LoginCHC.Text.Equals("登录"))
            {
                ret = chcFaceDetecter.LoginDVR();
                if (ret)
                {
                    MessageBox.Show("登录成功", null);
                    btn_LoginCHC.Text = "退出";
                }
            }
            else
            {
                ret = chcFaceDetecter.LogoutDVR();
                if (ret)
                {
                    MessageBox.Show("已退出", null);
                    btn_LoginCHC.Text = "登录";
                }
            }

            return ret;
        }
        private void btn_LoginCHC_Click(object sender, EventArgs e)
        {
            LoginCHC();
        }
        public void startCaptureCHC()
        {
            
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

        private void btnStartCapture_Click(object sender, EventArgs e)
        {
            startCaptureCHC();
        }
    }
}
