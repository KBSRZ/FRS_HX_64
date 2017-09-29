namespace Statistics
{
    partial class frmMain
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
            this.txtHitrecordPath = new System.Windows.Forms.TextBox();
            this.btnExportHitrecord = new System.Windows.Forms.Button();
            this.btnExportHitrecordPath = new System.Windows.Forms.Button();
            this.btnSelectQueryFacePath = new System.Windows.Forms.Button();
            this.btnExportQueryFace = new System.Windows.Forms.Button();
            this.txtQueryPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtHitrecordPath
            // 
            this.txtHitrecordPath.Location = new System.Drawing.Point(35, 46);
            this.txtHitrecordPath.Name = "txtHitrecordPath";
            this.txtHitrecordPath.Size = new System.Drawing.Size(100, 21);
            this.txtHitrecordPath.TabIndex = 0;
            // 
            // btnExportHitrecord
            // 
            this.btnExportHitrecord.Location = new System.Drawing.Point(193, 44);
            this.btnExportHitrecord.Name = "btnExportHitrecord";
            this.btnExportHitrecord.Size = new System.Drawing.Size(93, 23);
            this.btnExportHitrecord.TabIndex = 1;
            this.btnExportHitrecord.Text = "导出命中记录";
            this.btnExportHitrecord.UseVisualStyleBackColor = true;
            this.btnExportHitrecord.Click += new System.EventHandler(this.btnExportHitrecord_Click);
            // 
            // btnExportHitrecordPath
            // 
            this.btnExportHitrecordPath.Location = new System.Drawing.Point(141, 44);
            this.btnExportHitrecordPath.Name = "btnExportHitrecordPath";
            this.btnExportHitrecordPath.Size = new System.Drawing.Size(32, 23);
            this.btnExportHitrecordPath.TabIndex = 2;
            this.btnExportHitrecordPath.Text = "...";
            this.btnExportHitrecordPath.UseVisualStyleBackColor = true;
            this.btnExportHitrecordPath.Click += new System.EventHandler(this.btnExportHitrecordPath_Click);
            // 
            // btnSelectQueryFacePath
            // 
            this.btnSelectQueryFacePath.Location = new System.Drawing.Point(141, 126);
            this.btnSelectQueryFacePath.Name = "btnSelectQueryFacePath";
            this.btnSelectQueryFacePath.Size = new System.Drawing.Size(32, 20);
            this.btnSelectQueryFacePath.TabIndex = 8;
            this.btnSelectQueryFacePath.Text = "...";
            this.btnSelectQueryFacePath.UseVisualStyleBackColor = true;
            this.btnSelectQueryFacePath.Click += new System.EventHandler(this.btnSelectQueryFacePath_Click);
            // 
            // btnExportQueryFace
            // 
            this.btnExportQueryFace.Location = new System.Drawing.Point(193, 124);
            this.btnExportQueryFace.Name = "btnExportQueryFace";
            this.btnExportQueryFace.Size = new System.Drawing.Size(93, 20);
            this.btnExportQueryFace.TabIndex = 7;
            this.btnExportQueryFace.Text = "导出所有";
            this.btnExportQueryFace.UseVisualStyleBackColor = true;
            this.btnExportQueryFace.Click += new System.EventHandler(this.btnExportQueryFace_Click);
            // 
            // txtQueryPath
            // 
            this.txtQueryPath.Location = new System.Drawing.Point(35, 126);
            this.txtQueryPath.Name = "txtQueryPath";
            this.txtQueryPath.Size = new System.Drawing.Size(100, 21);
            this.txtQueryPath.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "导出检测到人脸命中的纪录";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(327, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelectQueryFacePath);
            this.Controls.Add(this.btnExportQueryFace);
            this.Controls.Add(this.txtQueryPath);
            this.Controls.Add(this.btnExportHitrecordPath);
            this.Controls.Add(this.btnExportHitrecord);
            this.Controls.Add(this.txtHitrecordPath);
            this.Name = "frmMain";
            this.Text = "统计";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHitrecordPath;
        private System.Windows.Forms.Button btnExportHitrecord;
        private System.Windows.Forms.Button btnExportHitrecordPath;
        private System.Windows.Forms.Button btnSelectQueryFacePath;
        private System.Windows.Forms.Button btnExportQueryFace;
        private System.Windows.Forms.TextBox txtQueryPath;
        private System.Windows.Forms.Label label1;
    }
}

