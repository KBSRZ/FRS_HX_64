namespace CHCTest
{
    partial class FrmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picPlayer = new System.Windows.Forms.PictureBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tex_UserPwd = new System.Windows.Forms.TextBox();
            this.tex_UserName = new System.Windows.Forms.TextBox();
            this.tex_DevPort = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tex_DevIP = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.btn_LoginCHC = new System.Windows.Forms.Button();
            this.label19 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.listViewFace = new System.Windows.Forms.ListView();
            this.btnStartCapture = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picPlayer)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // picPlayer
            // 
            this.picPlayer.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.picPlayer.Location = new System.Drawing.Point(0, -2);
            this.picPlayer.Margin = new System.Windows.Forms.Padding(4);
            this.picPlayer.Name = "picPlayer";
            this.picPlayer.Size = new System.Drawing.Size(845, 474);
            this.picPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPlayer.TabIndex = 1;
            this.picPlayer.TabStop = false;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(33, 156);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(37, 15);
            this.label20.TabIndex = 61;
            this.label20.Text = "密码";
            // 
            // tex_UserPwd
            // 
            this.tex_UserPwd.Location = new System.Drawing.Point(87, 153);
            this.tex_UserPwd.Name = "tex_UserPwd";
            this.tex_UserPwd.Size = new System.Drawing.Size(178, 25);
            this.tex_UserPwd.TabIndex = 60;
            this.tex_UserPwd.Text = "tuhui123456";
            // 
            // tex_UserName
            // 
            this.tex_UserName.Location = new System.Drawing.Point(87, 117);
            this.tex_UserName.Name = "tex_UserName";
            this.tex_UserName.Size = new System.Drawing.Size(178, 25);
            this.tex_UserName.TabIndex = 59;
            this.tex_UserName.Text = "admin";
            // 
            // tex_DevPort
            // 
            this.tex_DevPort.Location = new System.Drawing.Point(87, 77);
            this.tex_DevPort.Name = "tex_DevPort";
            this.tex_DevPort.Size = new System.Drawing.Size(178, 25);
            this.tex_DevPort.TabIndex = 58;
            this.tex_DevPort.Text = "8000";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 42);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 15);
            this.label6.TabIndex = 57;
            this.label6.Text = "设备IP";
            // 
            // tex_DevIP
            // 
            this.tex_DevIP.Location = new System.Drawing.Point(87, 39);
            this.tex_DevIP.Name = "tex_DevIP";
            this.tex_DevIP.Size = new System.Drawing.Size(178, 25);
            this.tex_DevIP.TabIndex = 57;
            this.tex_DevIP.Text = "172.18.132.237";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.btn_LoginCHC);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.tex_UserPwd);
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.tex_UserName);
            this.groupBox5.Controls.Add(this.tex_DevPort);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.tex_DevIP);
            this.groupBox5.Location = new System.Drawing.Point(999, 59);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(281, 226);
            this.groupBox5.TabIndex = 58;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "海康摄像头参数";
            // 
            // btn_LoginCHC
            // 
            this.btn_LoginCHC.Location = new System.Drawing.Point(116, 195);
            this.btn_LoginCHC.Name = "btn_LoginCHC";
            this.btn_LoginCHC.Size = new System.Drawing.Size(85, 29);
            this.btn_LoginCHC.TabIndex = 58;
            this.btn_LoginCHC.Text = "登录";
            this.btn_LoginCHC.UseVisualStyleBackColor = true;
            this.btn_LoginCHC.Click += new System.EventHandler(this.btn_LoginCHC_Click);
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(18, 120);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(52, 15);
            this.label19.TabIndex = 57;
            this.label19.Text = "用户名";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(7, 80);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(67, 15);
            this.label17.TabIndex = 57;
            this.label17.Text = "设备端口";
            // 
            // listViewFace
            // 
            this.listViewFace.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.listViewFace.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewFace.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listViewFace.Location = new System.Drawing.Point(0, 479);
            this.listViewFace.MultiSelect = false;
            this.listViewFace.Name = "listViewFace";
            this.listViewFace.Size = new System.Drawing.Size(1347, 161);
            this.listViewFace.TabIndex = 59;
            this.listViewFace.UseCompatibleStateImageBehavior = false;
            // 
            // btnStartCapture
            // 
            this.btnStartCapture.Location = new System.Drawing.Point(881, 347);
            this.btnStartCapture.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartCapture.Name = "btnStartCapture";
            this.btnStartCapture.Size = new System.Drawing.Size(127, 29);
            this.btnStartCapture.TabIndex = 60;
            this.btnStartCapture.Text = "开始";
            this.btnStartCapture.UseVisualStyleBackColor = true;
            this.btnStartCapture.Click += new System.EventHandler(this.btnStartCapture_Click);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1347, 640);
            this.Controls.Add(this.btnStartCapture);
            this.Controls.Add(this.listViewFace);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.picPlayer);
            this.Name = "FrmMain";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picPlayer)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picPlayer;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tex_UserPwd;
        private System.Windows.Forms.TextBox tex_UserName;
        private System.Windows.Forms.TextBox tex_DevPort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tex_DevIP;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button btn_LoginCHC;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ListView listViewFace;
        private System.Windows.Forms.Button btnStartCapture;
    }
}

