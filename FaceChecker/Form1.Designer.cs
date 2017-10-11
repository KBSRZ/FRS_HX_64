namespace FaceChecker
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.pcb_Capture = new System.Windows.Forms.PictureBox();
            this.pcb_Match = new System.Windows.Forms.PictureBox();
            this.lbl_Capture = new System.Windows.Forms.Label();
            this.lbl_Match = new System.Windows.Forms.Label();
            this.btn_Match = new System.Windows.Forms.Button();
            this.btn_UnMatch = new System.Windows.Forms.Button();
            this.lbl_CountCapture = new System.Windows.Forms.Label();
            this.lbl_CountMatch = new System.Windows.Forms.Label();
            this.lbl_CountMatchExact = new System.Windows.Forms.Label();
            this.lbl_CountItemExist = new System.Windows.Forms.Label();
            this.lbl_CountItemProcessed = new System.Windows.Forms.Label();
            this.btn_LastItem = new System.Windows.Forms.Button();
            this.btn_NextItem = new System.Windows.Forms.Button();
            this.btn_StartCheck = new System.Windows.Forms.Button();
            this.btn_Clean = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_Capture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_Match)).BeginInit();
            this.SuspendLayout();
            // 
            // pcb_Capture
            // 
            this.pcb_Capture.Location = new System.Drawing.Point(35, 24);
            this.pcb_Capture.Name = "pcb_Capture";
            this.pcb_Capture.Size = new System.Drawing.Size(286, 288);
            this.pcb_Capture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcb_Capture.TabIndex = 0;
            this.pcb_Capture.TabStop = false;
            // 
            // pcb_Match
            // 
            this.pcb_Match.Location = new System.Drawing.Point(353, 24);
            this.pcb_Match.Name = "pcb_Match";
            this.pcb_Match.Size = new System.Drawing.Size(279, 288);
            this.pcb_Match.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcb_Match.TabIndex = 1;
            this.pcb_Match.TabStop = false;
            // 
            // lbl_Capture
            // 
            this.lbl_Capture.AutoSize = true;
            this.lbl_Capture.Location = new System.Drawing.Point(94, 340);
            this.lbl_Capture.Name = "lbl_Capture";
            this.lbl_Capture.Size = new System.Drawing.Size(119, 15);
            this.lbl_Capture.TabIndex = 2;
            this.lbl_Capture.Text = "CaptureImgName";
            // 
            // lbl_Match
            // 
            this.lbl_Match.AutoSize = true;
            this.lbl_Match.Location = new System.Drawing.Point(466, 340);
            this.lbl_Match.Name = "lbl_Match";
            this.lbl_Match.Size = new System.Drawing.Size(103, 15);
            this.lbl_Match.TabIndex = 3;
            this.lbl_Match.Text = "MatchImgName";
            // 
            // btn_Match
            // 
            this.btn_Match.Location = new System.Drawing.Point(660, 186);
            this.btn_Match.Name = "btn_Match";
            this.btn_Match.Size = new System.Drawing.Size(104, 50);
            this.btn_Match.TabIndex = 4;
            this.btn_Match.Text = "正确";
            this.btn_Match.UseVisualStyleBackColor = true;
            this.btn_Match.Click += new System.EventHandler(this.btn_Match_Click);
            // 
            // btn_UnMatch
            // 
            this.btn_UnMatch.Location = new System.Drawing.Point(770, 186);
            this.btn_UnMatch.Name = "btn_UnMatch";
            this.btn_UnMatch.Size = new System.Drawing.Size(94, 50);
            this.btn_UnMatch.TabIndex = 5;
            this.btn_UnMatch.Text = "错误";
            this.btn_UnMatch.UseVisualStyleBackColor = true;
            this.btn_UnMatch.Click += new System.EventHandler(this.btn_UnMatch_Click);
            // 
            // lbl_CountCapture
            // 
            this.lbl_CountCapture.AutoSize = true;
            this.lbl_CountCapture.Location = new System.Drawing.Point(673, 55);
            this.lbl_CountCapture.Name = "lbl_CountCapture";
            this.lbl_CountCapture.Size = new System.Drawing.Size(67, 15);
            this.lbl_CountCapture.TabIndex = 6;
            this.lbl_CountCapture.Text = "抓拍张数";
            // 
            // lbl_CountMatch
            // 
            this.lbl_CountMatch.AutoSize = true;
            this.lbl_CountMatch.Location = new System.Drawing.Point(672, 88);
            this.lbl_CountMatch.Name = "lbl_CountMatch";
            this.lbl_CountMatch.Size = new System.Drawing.Size(67, 15);
            this.lbl_CountMatch.TabIndex = 7;
            this.lbl_CountMatch.Text = "匹配张数";
            // 
            // lbl_CountMatchExact
            // 
            this.lbl_CountMatchExact.AutoSize = true;
            this.lbl_CountMatchExact.Location = new System.Drawing.Point(825, 55);
            this.lbl_CountMatchExact.Name = "lbl_CountMatchExact";
            this.lbl_CountMatchExact.Size = new System.Drawing.Size(67, 15);
            this.lbl_CountMatchExact.TabIndex = 8;
            this.lbl_CountMatchExact.Text = "精确张数";
            // 
            // lbl_CountItemExist
            // 
            this.lbl_CountItemExist.AutoSize = true;
            this.lbl_CountItemExist.Location = new System.Drawing.Point(657, 24);
            this.lbl_CountItemExist.Name = "lbl_CountItemExist";
            this.lbl_CountItemExist.Size = new System.Drawing.Size(82, 15);
            this.lbl_CountItemExist.TabIndex = 9;
            this.lbl_CountItemExist.Text = "当前总项数";
            // 
            // lbl_CountItemProcessed
            // 
            this.lbl_CountItemProcessed.AutoSize = true;
            this.lbl_CountItemProcessed.Location = new System.Drawing.Point(810, 24);
            this.lbl_CountItemProcessed.Name = "lbl_CountItemProcessed";
            this.lbl_CountItemProcessed.Size = new System.Drawing.Size(82, 15);
            this.lbl_CountItemProcessed.TabIndex = 10;
            this.lbl_CountItemProcessed.Text = "已处理项数";
            // 
            // btn_LastItem
            // 
            this.btn_LastItem.Location = new System.Drawing.Point(259, 328);
            this.btn_LastItem.Name = "btn_LastItem";
            this.btn_LastItem.Size = new System.Drawing.Size(75, 39);
            this.btn_LastItem.TabIndex = 11;
            this.btn_LastItem.Text = "上一项";
            this.btn_LastItem.UseVisualStyleBackColor = true;
            this.btn_LastItem.Click += new System.EventHandler(this.btn_LastItem_Click);
            // 
            // btn_NextItem
            // 
            this.btn_NextItem.Location = new System.Drawing.Point(340, 328);
            this.btn_NextItem.Name = "btn_NextItem";
            this.btn_NextItem.Size = new System.Drawing.Size(75, 39);
            this.btn_NextItem.TabIndex = 12;
            this.btn_NextItem.Text = "下一项";
            this.btn_NextItem.UseVisualStyleBackColor = true;
            this.btn_NextItem.Click += new System.EventHandler(this.btn_NextItem_Click);
            // 
            // btn_StartCheck
            // 
            this.btn_StartCheck.Location = new System.Drawing.Point(660, 131);
            this.btn_StartCheck.Name = "btn_StartCheck";
            this.btn_StartCheck.Size = new System.Drawing.Size(204, 49);
            this.btn_StartCheck.TabIndex = 13;
            this.btn_StartCheck.Text = "开始检查";
            this.btn_StartCheck.UseVisualStyleBackColor = true;
            this.btn_StartCheck.Click += new System.EventHandler(this.btn_StartCheck_Click);
            // 
            // btn_Clean
            // 
            this.btn_Clean.Location = new System.Drawing.Point(660, 261);
            this.btn_Clean.Name = "btn_Clean";
            this.btn_Clean.Size = new System.Drawing.Size(204, 51);
            this.btn_Clean.TabIndex = 14;
            this.btn_Clean.Text = "整理数据";
            this.btn_Clean.UseVisualStyleBackColor = true;
            this.btn_Clean.Click += new System.EventHandler(this.btn_Clean_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(980, 417);
            this.Controls.Add(this.btn_Clean);
            this.Controls.Add(this.btn_StartCheck);
            this.Controls.Add(this.btn_NextItem);
            this.Controls.Add(this.btn_LastItem);
            this.Controls.Add(this.lbl_CountItemProcessed);
            this.Controls.Add(this.lbl_CountItemExist);
            this.Controls.Add(this.lbl_CountMatchExact);
            this.Controls.Add(this.lbl_CountMatch);
            this.Controls.Add(this.lbl_CountCapture);
            this.Controls.Add(this.btn_UnMatch);
            this.Controls.Add(this.btn_Match);
            this.Controls.Add(this.lbl_Match);
            this.Controls.Add(this.lbl_Capture);
            this.Controls.Add(this.pcb_Match);
            this.Controls.Add(this.pcb_Capture);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pcb_Capture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcb_Match)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pcb_Capture;
        private System.Windows.Forms.PictureBox pcb_Match;
        private System.Windows.Forms.Label lbl_Capture;
        private System.Windows.Forms.Label lbl_Match;
        private System.Windows.Forms.Button btn_Match;
        private System.Windows.Forms.Button btn_UnMatch;
        private System.Windows.Forms.Label lbl_CountCapture;
        private System.Windows.Forms.Label lbl_CountMatch;
        private System.Windows.Forms.Label lbl_CountMatchExact;
        private System.Windows.Forms.Label lbl_CountItemExist;
        private System.Windows.Forms.Label lbl_CountItemProcessed;
        private System.Windows.Forms.Button btn_LastItem;
        private System.Windows.Forms.Button btn_NextItem;
        private System.Windows.Forms.Button btn_StartCheck;
        private System.Windows.Forms.Button btn_Clean;
    }
}

