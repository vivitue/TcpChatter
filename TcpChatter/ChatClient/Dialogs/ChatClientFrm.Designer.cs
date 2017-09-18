namespace ChatClient
{
    partial class ChatClientFrm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChatClientFrm));
            this.gbRecv = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtRecv = new System.Windows.Forms.TextBox();
            this.llbSuperLink = new System.Windows.Forms.LinkLabel();
            this.lbUser = new System.Windows.Forms.Label();
            this.lbIp = new System.Windows.Forms.Label();
            this.lbUserList = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnClearRecv = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.cbUserList = new System.Windows.Forms.ComboBox();
            this.rbPrivate = new System.Windows.Forms.RadioButton();
            this.rbBroadcast = new System.Windows.Forms.RadioButton();
            this.txtSend = new System.Windows.Forms.TextBox();
            this.gbSend = new System.Windows.Forms.GroupBox();
            this.gbRecv.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbRecv
            // 
            this.gbRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbRecv.Controls.Add(this.panel1);
            this.gbRecv.Location = new System.Drawing.Point(24, 32);
            this.gbRecv.Name = "gbRecv";
            this.gbRecv.Size = new System.Drawing.Size(617, 200);
            this.gbRecv.TabIndex = 8;
            this.gbRecv.TabStop = false;
            this.gbRecv.Text = "信息接收区";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.txtRecv);
            this.panel1.Location = new System.Drawing.Point(12, 18);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(591, 174);
            this.panel1.TabIndex = 1;
            // 
            // txtRecv
            // 
            this.txtRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRecv.BackColor = System.Drawing.SystemColors.Window;
            this.txtRecv.Location = new System.Drawing.Point(0, 0);
            this.txtRecv.Multiline = true;
            this.txtRecv.Name = "txtRecv";
            this.txtRecv.ReadOnly = true;
            this.txtRecv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRecv.Size = new System.Drawing.Size(586, 167);
            this.txtRecv.TabIndex = 0;
            // 
            // llbSuperLink
            // 
            this.llbSuperLink.AutoSize = true;
            this.llbSuperLink.Location = new System.Drawing.Point(22, 9);
            this.llbSuperLink.Name = "llbSuperLink";
            this.llbSuperLink.Size = new System.Drawing.Size(83, 12);
            this.llbSuperLink.TabIndex = 20;
            this.llbSuperLink.TabStop = true;
            this.llbSuperLink.Text = "LinkSevenstar";
            // 
            // lbUser
            // 
            this.lbUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUser.AutoSize = true;
            this.lbUser.Location = new System.Drawing.Point(525, 17);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(29, 12);
            this.lbUser.TabIndex = 17;
            this.lbUser.Text = "用户";
            // 
            // lbIp
            // 
            this.lbIp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbIp.AutoSize = true;
            this.lbIp.Location = new System.Drawing.Point(288, 17);
            this.lbIp.Name = "lbIp";
            this.lbIp.Size = new System.Drawing.Size(41, 12);
            this.lbIp.TabIndex = 16;
            this.lbIp.Text = "服务IP";
            // 
            // lbUserList
            // 
            this.lbUserList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lbUserList.AutoSize = true;
            this.lbUserList.Location = new System.Drawing.Point(215, 239);
            this.lbUserList.Name = "lbUserList";
            this.lbUserList.Size = new System.Drawing.Size(53, 12);
            this.lbUserList.TabIndex = 18;
            this.lbUserList.Text = "成员列表";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(406, 238);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "聊天模式";
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(552, 405);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "退出";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // btnClearRecv
            // 
            this.btnClearRecv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearRecv.Location = new System.Drawing.Point(150, 405);
            this.btnClearRecv.Name = "btnClearRecv";
            this.btnClearRecv.Size = new System.Drawing.Size(104, 23);
            this.btnClearRecv.TabIndex = 12;
            this.btnClearRecv.Text = "清除接收";
            this.btnClearRecv.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(24, 405);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(104, 23);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "保存记录";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSend.Image = ((System.Drawing.Image)(resources.GetObject("btnSend.Image")));
            this.btnSend.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSend.Location = new System.Drawing.Point(386, 378);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(148, 76);
            this.btnSend.TabIndex = 13;
            this.btnSend.Text = "发送";
            this.btnSend.UseVisualStyleBackColor = true;
            // 
            // cbUserList
            // 
            this.cbUserList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cbUserList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUserList.FormattingEnabled = true;
            this.cbUserList.Location = new System.Drawing.Point(274, 236);
            this.cbUserList.Name = "cbUserList";
            this.cbUserList.Size = new System.Drawing.Size(92, 20);
            this.cbUserList.TabIndex = 11;
            // 
            // rbPrivate
            // 
            this.rbPrivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rbPrivate.AutoSize = true;
            this.rbPrivate.Location = new System.Drawing.Point(465, 236);
            this.rbPrivate.Name = "rbPrivate";
            this.rbPrivate.Size = new System.Drawing.Size(59, 16);
            this.rbPrivate.TabIndex = 9;
            this.rbPrivate.TabStop = true;
            this.rbPrivate.Text = "稍稍话";
            this.rbPrivate.UseVisualStyleBackColor = true;
            // 
            // rbBroadcast
            // 
            this.rbBroadcast.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rbBroadcast.AutoSize = true;
            this.rbBroadcast.Location = new System.Drawing.Point(527, 237);
            this.rbBroadcast.Name = "rbBroadcast";
            this.rbBroadcast.Size = new System.Drawing.Size(59, 16);
            this.rbBroadcast.TabIndex = 10;
            this.rbBroadcast.TabStop = true;
            this.rbBroadcast.Text = "聊天室";
            this.rbBroadcast.UseVisualStyleBackColor = true;
            // 
            // txtSend
            // 
            this.txtSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSend.Location = new System.Drawing.Point(36, 271);
            this.txtSend.Multiline = true;
            this.txtSend.Name = "txtSend";
            this.txtSend.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSend.Size = new System.Drawing.Size(592, 86);
            this.txtSend.TabIndex = 7;
            // 
            // gbSend
            // 
            this.gbSend.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSend.Location = new System.Drawing.Point(24, 252);
            this.gbSend.Name = "gbSend";
            this.gbSend.Size = new System.Drawing.Size(618, 117);
            this.gbSend.TabIndex = 6;
            this.gbSend.TabStop = false;
            this.gbSend.Text = "信息发送区";
            // 
            // ChatClientFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(666, 471);
            this.Controls.Add(this.gbRecv);
            this.Controls.Add(this.llbSuperLink);
            this.Controls.Add(this.lbUser);
            this.Controls.Add(this.lbIp);
            this.Controls.Add(this.lbUserList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnClearRecv);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.cbUserList);
            this.Controls.Add(this.rbPrivate);
            this.Controls.Add(this.rbBroadcast);
            this.Controls.Add(this.txtSend);
            this.Controls.Add(this.gbSend);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(674, 503);
            this.Name = "ChatClientFrm";
            this.Text = "ChatClient";
            this.gbRecv.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbRecv;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRecv;
        private System.Windows.Forms.LinkLabel llbSuperLink;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Label lbIp;
        private System.Windows.Forms.Label lbUserList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnClearRecv;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.ComboBox cbUserList;
        private System.Windows.Forms.RadioButton rbPrivate;
        private System.Windows.Forms.RadioButton rbBroadcast;
        private System.Windows.Forms.TextBox txtSend;
        private System.Windows.Forms.GroupBox gbSend;
    }
}

