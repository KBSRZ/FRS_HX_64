namespace FaceRecognition
{
    partial class Form1
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
            this.Text_Div_Gen = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.Text_Div_Cur = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Text_Div_Gen
            // 
            this.Text_Div_Gen.Location = new System.Drawing.Point(25, 30);
            this.Text_Div_Gen.Name = "Text_Div_Gen";
            this.Text_Div_Gen.Size = new System.Drawing.Size(399, 21);
            this.Text_Div_Gen.TabIndex = 7;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(430, 30);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 11;
            this.button6.Text = "根目录";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Text_Div_Cur
            // 
            this.Text_Div_Cur.Location = new System.Drawing.Point(80, 59);
            this.Text_Div_Cur.Name = "Text_Div_Cur";
            this.Text_Div_Cur.Size = new System.Drawing.Size(399, 21);
            this.Text_Div_Cur.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "当前文件";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(219, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Detect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 135);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Text_Div_Cur);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.Text_Div_Gen);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Text_Div_Gen;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.TextBox Text_Div_Cur;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}

