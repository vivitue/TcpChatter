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
using System.IO;
using System.Threading;
using System.Net.Sockets;
using System.Diagnostics;
using System.Collections.Specialized;
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
    public partial class ChatClientFrm : Form, IDisposable
    {
        #region Constructor
        public ChatClientFrm(LogIn logFrm)
        {
            InitializeComponent();
            this.Initialize(logFrm);
            this.InitiallzeControls();
            this.RegisterClickEvents();
            this.StartMsgProcessThread();
        }
        #endregion

        #region Disposed
        public void Dispose()
        {
            this.isAlive = false;
            this.currentRequest = CmdRequest.Offline;
            this.senderID = this.UserID;
            this.nws.Write(CurrentCmd, 0, CurrentCmd.Length);
            this.OnRelease();
            this.Close();
        }
        #endregion

        #region OnMessages

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectedUser(object sender, EventArgs e)
        {
            if (this.cbUserList.SelectedItem == null)
            {
                this.currentChatter = string.Empty;
                return;
            }
            if (string.IsNullOrEmpty(this.cbUserList.SelectedItem.ToString()))
            {
                this.currentChatter = string.Empty;
                return;
            }
            this.currentChatter = this.cbUserList.SelectedItem.ToString();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSend(object sender, EventArgs e)
        {
            SendMessage();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "确定与服务器断开连接吗?",
                "退出",
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2
            );
            if (result == DialogResult.OK)
            {
                this.OnRelease();
            }
            else
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnSplashScreen()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSuperLink(object sender, EventArgs e)
        {
            Process.Start("http://172.16.26.47/");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSelectModeChanged(object sender, EventArgs e)
        {
            if (this.rbBroadcast.Checked)
            {
                this.cbUserList.Enabled = false;
                this.rbPrivate.Checked = false;
            }
            else
            {
                this.cbUserList.Enabled = true;
                this.rbPrivate.Checked = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSave(object sender,EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtRecv.Text)) return;

            DialogResult dret = DialogResult.None;
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "文本文件(*.txt)|*.txt";
            file.AddExtension = true;
            if ((dret = file.ShowDialog()) == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(file.FileName, this.txtRecv.Text);
                }
                catch (IOException ioex)
                {
                    MessageBox.Show(
                        ioex.Message,
                        "Simple Editor SaveText",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        ex.Message,
                        "SaveFailed!",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearRecv(object sender, EventArgs e)
        {
            this.txtRecv.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnEnter(object sender, EventArgs e)
        {
            KeyPressEventArgs args = e as KeyPressEventArgs;
            if (args.KeyChar == 0x0D)
            {
                this.btnSend.Focus();
                OnSend(sender, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnFrmCloseing(object sender, EventArgs e)
        {
            FormClosingEventArgs arg = e as FormClosingEventArgs;
            if (arg.CloseReason == CloseReason.UserClosing)
            {
                DialogResult result = MessageBox.Show(
                "确定与服务器断开连接吗?",
                "退出",
                 MessageBoxButtons.OKCancel,
                 MessageBoxIcon.Question,
                 MessageBoxDefaultButton.Button2
            );
                if (result == DialogResult.OK)
                {
                    OnRelease();
                }
                else
                {
                    arg.Cancel = true;
                }
            }
            else if (arg.CloseReason == CloseReason.FormOwnerClosing)
            {

            }
        }
        #endregion

        #region PrivateHelpers

        /// <summary>
        /// 
        /// </summary>
        private void InitiallzeControls()
        {
            this.SetRb(true);           // 
            this.lbIp.Text = "服务IP : " + this.ipAddr.ToString();
            this.lbUser.Text = "用户 : " + this.userName;
            this.txtRecv.Clear();
            this.txtSend.Clear();
            this.cbUserList.Enabled = false;
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        private void RegisterClickEvents()
        {
            this.llbSuperLink.LinkClicked += new LinkLabelLinkClickedEventHandler(OnSuperLink);
            this.rbBroadcast.CheckedChanged += new EventHandler(OnSelectModeChanged);
            this.btnSend.Click += new EventHandler(OnSend);
            this.cbUserList.SelectedIndexChanged += new EventHandler(OnSelectedUser);
            this.txtSend.KeyPress += new KeyPressEventHandler(OnEnter);
            this.btnExit.Click += new EventHandler(OnExit);
            this.FormClosing += new FormClosingEventHandler(OnFrmCloseing);
            this.btnSave.Click += new EventHandler(OnSave);
            this.btnClearRecv.Click += new EventHandler(OnClearRecv);
        }

        /// <summary>
        /// 广播模式设定
        /// </summary>
        /// <param name="enable">true 广播 否则发送指定用户</param>
        private void SetRb(bool enable)
        {
            this.rbPrivate.Checked = !enable;
            this.rbBroadcast.Checked = enable;
        }

        /// <summary>
        /// 请求当前用户在线列表
        /// </summary>
        private void RequestOnlineList()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frm"></param>
        private void Initialize(LogIn frm)
        {
            this.userName = frm.UserName;

            this.ipAddr = frm.IPAddr;
            this.port = frm.Port;
            this.nws = frm.Nws;
            this.tcpClient = frm.TcpClient;
            this.userID = frm.UserID;
            this.Instance = frm;
        }

        /// <summary>
        /// 
        /// </summary>
        private void StartMsgProcessThread()
        {
            int counter = 0;
            this.msgHandle = null;
            this.isAlive = true;
            this.msgHandle = new Thread(new ThreadStart(MsgProcessThread));
            this.msgHandle.Start();
            //while (!msgHandle.IsAlive)
            //{
            //    Thread.Sleep(100);
            //    if (counter++ > 10) break;
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        [Obsolete("Discard version!",true)]
        private bool PreMsgProcess()
        {
            byte[] byteHeader = new byte[128];
            byte[] clientBytes = new byte[MAXBUFSIZE];
            this.nws.Read(byteHeader, 0, byteHeader.Length);
            CmdRequest cmd = this.GetCmd(byteHeader);
            if (cmd != CmdRequest.UpdateUsers)
            {
                return false;
            }
            this.OnlineProcess();
            return true;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void MessageAppend(string message)
        {
            this.txtRecv.AppendText(message + Environment.NewLine);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckSelectedUserValid()
        {
            if (this.cbUserList.SelectedItem == null)
            {
                MessageBox.Show("请选择发送用户",
                                "提示",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return false;
            }

            if (string.IsNullOrEmpty(this.cbUserList.SelectedItem.ToString()))
            {
                MessageBox.Show("请选择发送用户",
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
        /// <returns></returns>
        private bool CheckMessageValid()
        {
            if (string.IsNullOrEmpty(this.txtSend.Text.Trim()))
            {
                MessageBox.Show("不能发送空消息",
                                "提示",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送消息处理
        /// </summary>
        private void SendMessage()
        {
            if (!CheckMessageValid()) return;
            string message = this.txtSend.Text.Trim();
            byte[] bytes = this.GetBytesFrom(message);
            string localTxt = string.Empty;
            string remoteTxt = string.Empty;

            if (!this.rbBroadcast.Checked)
            {

                if (!CheckSelectedUserValid()) return;

                this.senderID = this.GetClientIDFrom(this.userName);
                this.recvID = this.GetClientIDFrom(this.RecvName);
                this.currentRequest = CmdRequest.FixUser;
                this.currentRight = LProtocolRight.WR;

                localTxt = string.Format("[私聊]您在 {0} 对 {1} 说：\r\n{2}\r\n\r\n",
                    DateTime.Now, this.currentChatter, message);

                remoteTxt = string.Format("[私聊] {0}在 {1} 对 你 说：\r\n{2}\r\n\r\n",
                    this.userName, DateTime.Now, message);


                byte[] dataBuf = this.GetBytesFrom(remoteTxt);
                byte[] msgBuf = this.CreateWRCmd(CurrentCmd, dataBuf);
                this.nws.Write(msgBuf, 0, msgBuf.Length);
            }
            else
            {
                this.senderID = this.GetClientIDFrom(this.userName);
                this.recvID = 0x00;                                 // 广播 接收者ID无效
                this.currentRequest = CmdRequest.Broadcast;
                this.currentRight = LProtocolRight.WR;

                localTxt = string.Format("[广播]您在 {0} 对所有人说：\r\n{1}\r\n\r\n",
                    DateTime.Now, message);

                remoteTxt = string.Format("[广播]{0}在 {1} 对所有人说：\r\n{2}\r\n\r\n",
                    this.userName, DateTime.Now, message);

                byte[] dataBuf = this.GetBytesFrom(remoteTxt);
                byte[] msgBuf = this.CreateWRCmd(CurrentCmd, dataBuf);
                this.nws.Write(msgBuf, 0, msgBuf.Length);
            }
            this.MessageAppend(localTxt);
            this.txtSend.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void ReceiveCommonMsg(out string message)
        {
            byte[] msgBuf = new byte[MAXBUFSIZE];
            this.nws.Read(msgBuf, 0, msgBuf.Length);
            string msg = this.GetStringFrom(msgBuf);
            message = msg;
        }

        /// <summary>
        /// 
        /// </summary>
        private void CommonMsgProcess()
        {
            string message = string.Empty;
            ReceiveCommonMsg(out message);
            if (!base.InvokeRequired)
            {
                this.MessageAppend(message);
            }
            else
            {
                base.Invoke(new AsyncMsghandle(MessageAppend), new object[] { message });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnlineProcess()
        {
            byte[] users = new byte[MAXBUFSIZE];
            int lens = this.nws.Read(users, 0, users.Length);

            byte[] actualUserInfo = this.CopyArrayFrom(users, lens);

            if (this.Clients.Count != 0) this.Clients.Clear();
            this.Clients = this.DeserializeGraph<StringCollection>(actualUserInfo);
            if (this.cbUserList.Items.Count != 0) this.cbUserList.Items.Clear();
            foreach (string user in Clients)
            {
                if (user == this.userName) continue;
                this.cbUserList.Items.Add(user);
            }
        }

        /// <summary>
        /// 在线用户控件更新
        /// </summary>
        private void OnlineUsersUpdate()
        {
            //if (this.dicClient == null) return;
            //if (this.dicClient.Count == 0) return;

            //this.cbUserList.SelectedIndex = 0;

            if (this.cbUserList.Items.Count != 0) this.cbUserList.Items.Clear();

            foreach (string key in dicClient.Values)
            {
                if (key == this.userName) continue;
                this.cbUserList.Items.Add(key);
            }

            string message = string.Format("[系统消息] {0} 新用户变动", DateTime.Now);
            if (this.cbUserList.Items.Count!=0) this.cbUserList.SelectedIndex = 0;
            MessageAppend(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void OnlineNotify(string message)
        {
            MessageAppend(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userBuf"></param>
        private void UpdateUserListFrom(byte[] header, byte[] userBuf)
        {
            if (userBuf == null) return;
            if (this.dicClient != null)
            {
                if (this.dicClient.Count != 0) this.dicClient.Clear();
            }
            this.dicClient = this.DeserializeGraph<Dictionary<int, string>>(userBuf);
            if (this.dicClient == null)
            {
                return;
            }

            if (!base.InvokeRequired)
            {
                this.OnlineUsersUpdate();
            }
            else
            {
                Invoke(new AsyncHandle(OnlineUsersUpdate), null);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headbuf"></param>
        /// <param name="messageBuf"></param>
        private void DisplayFixUserInfo(byte[] headbuf, byte[] messageBuf)
        {
            if (messageBuf == null) return;
            string message = this.GetStringFrom(messageBuf);
            if (!base.InvokeRequired)
            {

                MessageAppend(message);
            }
            else
            {
                Invoke(new AsyncMsghandle(MessageAppend), new object[] { message });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <param name="buf"></param>
        private void MessageReceiveProcess(byte[] header, byte[] data, byte[] buf)
        {
            if (header == null) return;         
            if (header[0] != 0x03) return;      // 无效命令
            CmdRequest cmd = (CmdRequest)header[1];
            switch (cmd)
            {
                case CmdRequest.Online:
                    UpdateUserListFrom(header, data);
                    break;
                case CmdRequest.UpdateUsers:
                    UpdateUserListFrom(header, data);
                    break;
                case CmdRequest.FixUser:
                    DisplayFixUserInfo(header, data);
                    break;
                case CmdRequest.Broadcast:
                    DisplayFixUserInfo(header, data);
                    break;
                case CmdRequest.Flush:
                    DisplayFixUserInfo(header, data);
                    OnSplashScreen();
                    break;
                case CmdRequest.FlushAll:
                    DisplayFixUserInfo(header, data);
                    OnSplashScreen();
                    break;
                case CmdRequest.Offline:
                    //UpdateUserListFrom(header, data);
                    this.isAlive = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 用户消息处理线程
        /// </summary>
        private void MsgProcessThread()
        {

            while (isAlive)
            {
                try
                {
                    byte[] totalBuf = new byte[MAXBUFSIZE];

                    int actualLens = this.nws.Read(totalBuf, 0, totalBuf.Length);
                    byte[] msgBuf = this.CopyArrayFrom(totalBuf, actualLens);
                    byte[] headBuf = null;
                    byte[] dataBuf = null;
                    LErrorCode error = this.ResolveDataPackage(msgBuf, out headBuf, out dataBuf);
                    if (error != LErrorCode.Success)
                    {
                        continue;
                    }
                    this.MessageReceiveProcess(headBuf, dataBuf, msgBuf);
                }
                catch (Exception ex)
                {

                }

            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void StopMsgProcessThread()
        {
            CloseHandle(this.msgHandle);
            this.msgHandle = null;
            isAlive = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        private void CloseHandle(Thread handle)
        {
            if (handle != null)
            {
                if (handle.IsAlive)
                {
                    handle.Abort();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnRelease()
        {
            this.currentRequest = CmdRequest.Offline;
            this.senderID = this.UserID;
            this.nws.Write(CurrentCmd, 0, CurrentCmd.Length);
            this.CloseHandle(this.msgHandle);
            this.Release();
            this.Owner.Close();
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Release()
        {
            if (Instance != null)
            {
                Instance.Dispose();
            }
            Instance = null;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdatePackageLCmd()
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
        /// <param name="name"></param>
        /// <returns></returns>
        private int GetClientIDFrom(string name)
        {
            int result = -1;
            if (!dicClient.ContainsValue(name)) return result;
            foreach (KeyValuePair<int, string> item in dicClient)
            {
                if (item.Value == name)
                {
                    result = item.Key;
                    break;
                }
            }
            return result;

        }
        #endregion

        #region Fields & Properties
        private int userID = -1;
        private int UserID
        {
            get
            {
                return userID;
            }
        }
        private LProtocolRight currentRight = LProtocolRight.MinID;
        private CmdRequest currentRequest = CmdRequest.Failed;
        private int senderID = -1;
        private int recvID = 0;
        private string RecvName
        {
            get
            {
                return currentChatter;
            }
        }
        private byte[] currentCmd = new byte[ProtocolMsg.MSGLENS];
        private byte[] CurrentCmd
        {
            get
            {
                this.UpdatePackageLCmd();
                return currentCmd;
            }
        }

        private string currentChatter = string.Empty;
        private const int MAXBUFSIZE = ProtocolMsg.MAXBUFSIZE;
        private Dictionary<int, string> dicClient = new Dictionary<int, string>();
        private StringCollection Clients = new StringCollection();
        private bool isAlive = false;
        private NetworkStream nws = null;
        private TcpClient tcpClient = null;
        private IPAddress ipAddr = null;
        private int port = -2;
        private string userName = null;
        private LogIn Instance = null;
        private Thread msgHandle = null;
        private delegate void AsyncHandle();
        private delegate void AsyncMsghandle(string message);
        #endregion


    }
}
