namespace ChatClient
{
    partial class LogIn
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
            this.txtBoxUserID = new System.Windows.Forms.TextBox();
            this.txtBoxPort = new System.Windows.Forms.TextBox();
            this.txtBoxIP = new System.Windows.Forms.TextBox();
            this.btnLink = new System.Windows.Forms.Button();
            this.groupBoxLogIn = new System.Windows.Forms.GroupBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxLogIn.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtBoxUserID
            // 
            this.txtBoxUserID.Location = new System.Drawing.Point(87, 117);
            this.txtBoxUserID.Name = "txtBoxUserID";
            this.txtBoxUserID.Size = new System.Drawing.Size(100, 21);
            this.txtBoxUserID.TabIndex = 2;
            // 
            // txtBoxPort
            // 
            this.txtBoxPort.Location = new System.Drawing.Point(87, 83);
            this.txtBoxPort.Name = "txtBoxPort";
            this.txtBoxPort.Size = new System.Drawing.Size(100, 21);
            this.txtBoxPort.TabIndex = 2;
            // 
            // txtBoxIP
            // 
            this.txtBoxIP.Location = new System.Drawing.Point(87, 50);
            this.txtBoxIP.Name = "txtBoxIP";
            this.txtBoxIP.Size = new System.Drawing.Size(100, 21);
            this.txtBoxIP.TabIndex = 2;
            // 
            // btnLink
            // 
            this.btnLink.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLink.Location = new System.Drawing.Point(230, 51);
            this.btnLink.Name = "btnLink";
            this.btnLink.Size = new System.Drawing.Size(75, 23);
            this.btnLink.TabIndex = 1;
            this.btnLink.Text = "连接";
            this.btnLink.UseVisualStyleBackColor = true;

            // 
            // groupBoxLogIn
            // 
            this.groupBoxLogIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBoxLogIn.Controls.Add(this.txtBoxUserID);
            this.groupBoxLogIn.Controls.Add(this.txtBoxPort);
            this.groupBoxLogIn.Controls.Add(this.txtBoxIP);
            this.groupBoxLogIn.Controls.Add(this.btnExit);
            this.groupBoxLogIn.Controls.Add(this.btnLink);
            this.groupBoxLogIn.Controls.Add(this.label3);
            this.groupBoxLogIn.Controls.Add(this.label2);
            this.groupBoxLogIn.Controls.Add(this.label1);
            this.groupBoxLogIn.Location = new System.Drawing.Point(27, 29);
            this.groupBoxLogIn.Name = "groupBoxLogIn";
            this.groupBoxLogIn.Size = new System.Drawing.Size(342, 168);
            this.groupBoxLogIn.TabIndex = 2;
            this.groupBoxLogIn.TabStop = false;
            this.groupBoxLogIn.Text = "登陆";
            // 
            // btnExit
            // 
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(231, 113);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(28, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "用户名";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(28, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "端口";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(28, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "服务器IP";
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 219);
            this.ControlBox = false;
            this.Controls.Add(this.groupBoxLogIn);
            this.Name = "LogIn";
            this.Text = "LogIn";
            this.groupBoxLogIn.ResumeLayout(false);
            this.groupBoxLogIn.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtBoxUserID;
        private System.Windows.Forms.TextBox txtBoxPort;
        private System.Windows.Forms.TextBox txtBoxIP;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.GroupBox groupBoxLogIn;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}