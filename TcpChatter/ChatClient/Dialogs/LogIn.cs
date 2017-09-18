// 

// Copyright (c) 2017 vivitue

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using L.vivitue.Common;

namespace ChatClient
{
    public partial class LogIn : Form, IDisposable
    {
        #region Constructor
        public LogIn()
        {
            InitializeComponent();
            this.Initialize();
        }
        #endregion

        #region Deconstructor
        public virtual void Dispose()
        {
            base.Dispose();
            Disposed(true);
        }

        protected virtual void Disposed(bool disposing)
        {
            if (isDisposed) return;
            if (disposing)
            {

            }
            if (this.netwkStream != null)
            {
                this.netwkStream.Close();
                this.netwkStream = null;
            }

            if (this.tcpClient != null)
            {
                this.tcpClient.Close();
                this.tcpClient = null;
            }
            this.isDisposed = true;
        }
        #endregion

        #region PrivateHelpers

        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            RegisterClickEvents();
            this.txtBoxIP.Text = "172.16.26.157";
            this.txtBoxPort.Text = "6023";
            this.txtBoxUserID.Focus();
            this.txtBoxUserID.Select(0, 0);

        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdatePackageLCmd()
        {
            LCmd cmd = this.CreatCurrentLCmd();

            this.currentCmd[0] = (byte)cmd.Head;
            this.currentCmd[1] = (byte)cmd.CmdMode;
            this.currentCmd[2] = (byte)cmd.SendID;
            this.currentCmd[3] = (byte)cmd.WR;
            this.currentCmd[4] = (byte)cmd.RecvID;
            this.currentCmd[5] = (byte)cmd.Resv2;
            this.currentCmd[6] = (byte)cmd.Resv3;
            this.currentCmd[7] = (byte)cmd.Resv4;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private LCmd CreatCurrentLCmd()
        {
            LCmd cmd = new LCmd();
            cmd.Head = (int)ProtocolMsg.LCML;
            cmd.CmdMode = (int)currentRequest;
            cmd.SendID = senderID;
            cmd.WR = (int)currentRight;
            cmd.RecvID = recvID;
            cmd.Resv2 = 0x00;
            cmd.Resv3 = 0x00;
            cmd.Resv4 = 0x00;
            return cmd;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckUserInfo()
        {
            if (string.IsNullOrEmpty(txtBoxUserID.Text.Trim()))
            {
                MessageBox.Show("请输入有效用户名",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
            if (!IPAddress.TryParse(txtBoxIP.Text, out ipAddr))
            {
                MessageBox.Show("IP地址不合法!",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
            if (!int.TryParse(txtBoxPort.Text, out port) || port < 1024 || port > 65535)
            {
                MessageBox.Show("输入端口不合法",
                    "提示",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <returns></returns>
        private bool CheckTcpClient(TcpClient tcpClient)
        {
            if (tcpClient == null)
            {
                MessageBox.Show(
                    "无法连接到服务器,请重试",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                    );
                return false;
            }
            if (!tcpClient.Connected)
            {
                MessageBox.Show(
                        "无法连接到服务器,请重试",
                        "错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation
                        );
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ShowLinkError()
        {
            MessageBox.Show(
                    "超时!无法连接到服务器,请查看服务是否启动,请重试",
                    "错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                    );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private bool CheckLogInfoCmd(CmdRequest cmd)
        {
            if (cmd != CmdRequest.Success)
            {
                MessageBox.Show(
                                "您的用户名已经被使用，请尝试其他用户名!",
                                "提示",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                                );
                return false;
            }
            return true;
        }
        #endregion

        #region ClickEvents

        /// <summary>
        /// 
        /// </summary>
        private void RegisterClickEvents()
        {
            this.btnLink.Click += new EventHandler(OnLogIn);
            this.txtBoxUserID.KeyPress += new KeyPressEventHandler(OnEnterMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnterMessage(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 0x0D)
            {
                this.btnLink.Focus();
                OnLogIn(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLogIn(object sender, EventArgs e)
        {
            // 校验用户输入信息
            if (!CheckUserInfo()) return;

            string userName = this.txtBoxUserID.Text.Trim();
            this.userName = userName;
            byte[] logBuf = new byte[MAXBUFSIZE];
            try
            {
                this.tcpClient = new TcpClient();
                tcpClient.Connect(this.ipAddr, this.port);     //连接服务

                // 校验Tcp连接信息
                if (!CheckTcpClient(this.tcpClient)) return;

                this.netwkStream = tcpClient.GetStream();
                byte[] userInfo = this.GetBytesFrom(userName);  // 获取用户名

                // 设置当前命令
                this.currentRequest = CmdRequest.Online;
                this.currentRight = LProtocolRight.WR;

                byte[] sendBuf = this.CreateWRCmd(CurrentCmd,userInfo);

                //向服务器发送登陆信息
                netwkStream.Write(sendBuf, 0, sendBuf.Length);

                //获取登录结果
                int actualLens = netwkStream.Read(logBuf, 0, logBuf.Length);

                byte[] actualBuf = this.CopyArrayFrom(logBuf, actualLens);

                byte[] cmdHeader = null;
                byte[] dataBuf=null;
                LErrorCode error = this.ResolveDataPackage(actualBuf, out cmdHeader, out dataBuf);

                if (error != LErrorCode.Success)
                {
                    return;
                }
                CmdRequest cmd = (CmdRequest)cmdHeader[1];

                // 效验结果信息
                if (!CheckLogInfoCmd(cmd)) return;

                this.senderID=(int) cmdHeader[2];   // 获取 UserID

                // 校验成功则登陆
                this.chatFrm = new ChatClientFrm(this);
                this.chatFrm.Owner = this;
                this.Hide();
                this.chatFrm.Show();
            }
            catch (Exception ex)
            {
                this.ShowLinkError();
                this.tcpClient = null;
                this.netwkStream = null;
            }
        }
        #endregion

        #region Fileds & Properties
        private const int MAXBUFSIZE = ProtocolMsg.MAXBUFSIZE;       // Max buffer size of sending & receiving message (20KB)
        private bool isDisposed = false;                // Disposed flag of this form
        private string userName = null;                 // Current user name
        public string UserName
        {
            get
            {
                return this.userName;
            }
        }
        private IPAddress ipAddr = null;
        public IPAddress IPAddr
        {
            get
            {
                return ipAddr;
            }
        }
        private int port = -1;
        public int Port
        {
            get
            {
                return port;
            }
        }
        private ChatClientFrm chatFrm = null;

        private TcpClient tcpClient = null;
        public TcpClient TcpClient
        {
            get
            {
                return tcpClient;
            }
        }
        private NetworkStream netwkStream = null;
        public NetworkStream Nws
        {
            get
            {
                return netwkStream;
            }
        }

        private LProtocolRight currentRight = LProtocolRight.MinID; // Send message right
        private CmdRequest currentRequest = CmdRequest.Failed;      // Command mode
        private int senderID = -1;
        public int UserID
        {
            get
            {
                return senderID;
            }
        }
        private int recvID = 0;
        private byte[] currentCmd = new byte[8];                    // CurrentCmd : Current lastest command
        private byte[] CurrentCmd
        {
            get
            {
                this.UpdatePackageLCmd();
                return currentCmd;
            }
        }
        #endregion
    }
}
