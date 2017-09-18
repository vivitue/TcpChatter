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
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using L.vivitue.Common;

namespace L.vivitue.Server
{
    public class ChatAgent : IChatAgent,IDisposable
    {
        #region Constructor
        public ChatAgent(string name) 
        {
            this.Initialize(name);
        }
        #endregion

        #region Finalizers

        ~ChatAgent()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.isdisposed) return;
            if (disposing)
            {
                // Release unmanaged resources
            }
            
            this.StopChatServer();
            this.Release();
            this.isdisposed = true;
            Console.WriteLine(SysInfo.Timestamp + "ChatServer has been disposed!");
        }

        /// <summary>
        /// 
        /// </summary>
        private void Release()
        {

        }
        #endregion

        #region Public Interfaces

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StartChatServer()
        {
            return StartChatServerHelper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool StopChatServer()
        {
            return StopChatServerHelper();
        }
        #endregion

        #region Private Helpers

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool StartChatServerHelper()
        {
            SetServerPort();
            if (!StartListener()) return false;

            int counter = 0;
            this.isAlive = true;
            this.msgHandle = new Thread(new ThreadStart(MessageProcessThread));
            this.msgHandle.Start();
#if ISWAITALIVE
            while (!msgHandle.IsAlive)
            {
                Thread.Sleep(100);
                if (counter++ > 10)
                {
                    Innerlog.Error(dcrlringType, "Warning : MessageProcessThread start-wait failed!", null);
                    break;
                }
            }
#endif
            //this.isAlive = true;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool StopChatServerHelper()
        {
            CloseMsgHandle(this.msgHandle);
            this.isAlive = false;
            this.msgHandle = null;
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private void Initialize(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                this.name = name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="handle"></param>
        private void CloseMsgHandle(Thread handle)
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
        /// <param name="skt"></param>
        private void CloseSkt(Socket skt)
        {
            if (skt != null)
            {
                if (skt.Connected)
                {
                    skt.Close();
                }
                skt = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetServerPort()
        {
            string intTxt = string.Empty;
            Console.Write("Input listening port of ChatServer : ");
            intTxt = Console.ReadLine();
            if (!string.IsNullOrEmpty(intTxt))
            {
                if (!int.TryParse(intTxt, out this.port) || port < 1024 || port > 65535)
                {
                    this.port = 4001;
                    Console.WriteLine("Invalid input port! Default port 4001 has been be trigged!");
                }
            }
            else
            {
                Console.WriteLine("Invalid input param! Default port 4001 has been be trigged!");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        private IPAddress GetIPv4Addr(string hostName)
        {
            IPAddress ipv4Addr = null;
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            int ipMaxCnt = ipEntry.AddressList.Count();
            for (int i = 0; i < ipMaxCnt; i++)
            {
                if (ipEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                {
                    ipv4Addr = ipEntry.AddressList[i];
                    break;
                }
            }
            return ipv4Addr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>true if start successfully else false</returns>
        private bool StartListener()
        {
            bool result = true;
            try
            {
                this.hostName = Dns.GetHostName();

                //this.ipAddr = Dns.GetHostAddresses(hostName)[0];        // 获取本地IP
                this.ipAddr = GetIPv4Addr(this.hostName);
                this.tcpListener = new TcpListener(ipAddr, port);
                this.tcpListener.Start();

                if (string.IsNullOrEmpty(this.name)) this.name = this.hostName;
                Console.WriteLine("");
                Console.WriteLine(" - 服务器启动成功 - \n");
                Console.WriteLine(" - 服务器名称:{0,20}\n - 服务器IP:{1,27}\n - 注册服务器别名: {2,15}\n - 端口号：{3,19}\n",
                    hostName, ipAddr, name, port);
            }
            catch (Exception ex)
            {
                result = false;
                Innerlog.Error(dcrlringType, "StartListener failed!", ex);
            }
            return result;
        }

        /// <summary>
        /// 客户端 消息处理主线程
        /// </summary>
        private void MessageProcessThread()
        {
            ClientContext client = null;
            while (IsAlive)
            {
                try
                {
                    byte[] useNameBuf = new byte[MAXBUFSIZE];

                    // 监听连接请求对像
                    Socket msgSkt = tcpListener.AcceptSocket();

                    // 等待上线请求
                    int actualLens = msgSkt.Receive(useNameBuf);        

                    // 获取实际数据长度
                    byte[] buf = this.CopyArrayFrom(useNameBuf, actualLens);

                    byte[] header = null;
                    byte[] dataBuf = null;

                    // 解析上线请求命令包 : 上线请求 + 用户名
                    LErrorCode error = this.ResolveDataPackage(buf, out header, out dataBuf);
                    if (error != LErrorCode.Success)
                    {
                        Console.Error.WriteLine("ResolveDataPackage failed! LErrorCode = {0}", error);
                        continue;
                    }

                    // 校验命令头
                    if (header[0] != ProtocolMsg.LCML)
                    {
                        Console.Error.WriteLine("Invalid cmmand head = {0}", header[0]);
                        continue;
                    }
                    // 是否是上线请求   -  第 1 个命令必须是: 上线请求命令包 + 用户名
                    CmdRequest request = (CmdRequest)header[1];
                    if (request != CmdRequest.Online)
                    {
                        Console.Error.WriteLine("Invalid request command! Cmd = {0}", request);
                        continue;
                    }

                    // 校验用户名的合法性
                    string user = this.GetStringFrom(dataBuf);
                    if (!CheckUserInvalid(user))
                    {
                        string msg = "User name " + user + " has been existed in TcpChatter system! User tried to join chatting failed!";

                        this.currentRequest = CmdRequest.Failed;
                        this.currentRight = LProtocolRight.WR;

                        msgSkt.Send(CurrentCmd);
                        Console.Error.WriteLine(msg);
                        continue;
                    }


                    // 服务端生成用户信息 并动态分配独立用户ID
                    client = new ClientContext();

                    client.UserID = ChatAgent.ActiveID;
                    client.UserName = user;
                    client.Skt = msgSkt;
                    dicClientContext.Add(user, client);

                    this.currentRequest = CmdRequest.Success;
                    this.currentRight = LProtocolRight.WR;
                    this.senderID = client.UserID;

                    // 发送登陆成功命令
                    msgSkt.Send(CurrentCmd);   

                    string sysmsg = string.Format("[系统消息]\n新用户 {0} 在[{1}] 已成功连接服务器[当前在线人数: {2}]\r\n\r\n", 
                        user, DateTime.Now, dicClientContext.Count);
                    Console.WriteLine(SysInfo.Timestamp + sysmsg);

                    Thread.Sleep(1000);         // Sleep 1s

                    Thread handle = new Thread(() =>
                        {
                            if (PreMessageProcess(client, sysmsg))
                            {
                                // 启用用户 消息监听线程
                                SubMsgProcessThread(client, sysmsg);
                            }
                        });
                    handle.Start();

                    dicClientContext[user].MsgHandle = handle;
                }
                catch (SocketException se)
                {
                    Innerlog.Error(dcrlringType, "SocketException Current user =  " + client.UserName + " was offline!", se);
                }
                catch (Exception ex)
                {
                    Innerlog.Error(dcrlringType, "Exception Current user =  " + client.UserName + " was offline!", ex);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private bool PreMessageProcess(ClientContext client, string message)
        {
            bool result = true;
            try
            {
                // 上线更新
                this.UpdateCurrentUserList();

                // 上线通知
                this.NotifyAllOnlineUsers(message);
            }
            catch (SocketException se)
            {
                result = false;
                ClientOfflineProcess(client);
                Innerlog.Error(dcrlringType, "PreMessageProcess exception ocurred!", se);
            }
            catch (Exception ex)
            {
                result=false;
                ClientOfflineProcess(client);
                Innerlog.Error(dcrlringType, "PreMessageProcess exception ocurred!", ex);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void NotifyAllOnlineUsers(string message)
        {
            this.currentRequest = CmdRequest.Broadcast;
            this.recvID = 0;
            byte[] msgBuf = this.GetBytesFrom(message);
            byte[] msgSender = this.CreateWRCmd(CurrentCmd, msgBuf);
            foreach (ClientContext user in dicClientContext.Values)
            {
                user.Skt.Send(msgSender);
            }
        }

        /// <summary>
        /// 用户消息监听线程
        /// </summary>
        /// <param name="client"></param>
        private void SubMsgProcessThread(ClientContext clientx, string message)
        {
            ClientContext client = clientx;
            while (true)
            {
                try
                {
                    byte[] msgBuf = new byte[MAXBUFSIZE];
                    // 监听 并接收数据
                    int actualLens = client.Skt.Receive(msgBuf);
                    byte[] totalBuf = this.CopyArrayFrom(msgBuf, actualLens);

                    byte[] headBuf = null;
                    byte[] dataBuf = null;

                    // 解析命令包
                    LErrorCode error = this.ResolveDataPackage(totalBuf, out headBuf, out dataBuf);

                    client.HeadBuf = headBuf;
                    client.DataBuf = dataBuf;
                    client.Buf = totalBuf;

                    if (error != LErrorCode.Success) continue;

                    // 是否是有效命令
                    if (headBuf[0] != ProtocolMsg.LCML) continue;

                    
                    CmdRequest cmdHead = (CmdRequest)headBuf[1];
                    if (cmdHead == CmdRequest.InvalidCmd ||
                        cmdHead == CmdRequest.MaxID ||
                        cmdHead == CmdRequest.MinID)
                    {
                        Console.Error.WriteLine("Invalid Send Message!");
                        continue;
                    }
                    else
                    {
                        // 用户消息转发
                        UserMessageProcess(client);
                    }
                }
                catch (Exception ex)
                {
                    ClientOfflineProcess(client);
                    //Innerlog.Error(dcrlringType, "Current user =  " + client.UserName + " was offline!", ex);
                    Thread.CurrentThread.Abort();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        [Obsolete("Discard version", false)]
        private void OnlineProcess(ClientContext client)
        {
            byte[] clientBytes = this.SerializeGraph<StringCollection>(GetCurrentUserCollections());
            foreach (ClientContext cc in dicClientContext.Values)
            {
                cc.Skt.Send(new byte[] { (int)CmdRequest.Online });
                cc.Skt.Send(clientBytes);
            }
        }

        /// <summary>
        /// 向指定用户发送消息
        /// 消息格式:   命令模式 + 消息正文
        /// </summary>
        /// <param name="client"></param>
        [Obsolete("Discard version", false)]
        private void FixUserProcess(ClientContext client)
        {
            byte[] userInfo = new byte[128];
            byte[] msgBuf=new byte[MAXBUFSIZE];
            client.Skt.Receive(userInfo);           // 接收用户名
            Thread.Sleep(100);
            client.Skt.Receive(msgBuf);             // 接收消息正文
            string userName = this.GetStringFrom(userInfo);
            if (CheckUserInvalid(userName))
            {
                // 发送消息
                dicClientContext[userName].Skt.Send(msgBuf);
            }
            else
            {
                string failedMsg = string.Format("[系统消息]您刚才的内容没有发送成功。\r\n可能原因：用户 {0} 已离线或者网络阻塞。\r\n\r\n", userName);
                client.Skt.Send(this.GetBytesFrom(failedMsg));
            }
         }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        [Obsolete("Discard version", false)]
        private void FlushProcess(ClientContext client)
        {
            string svmsg = string.Format("[系统提示]用户 {0} 向您发送了一个闪屏振动。\r\n\r\n", client.UserName);
            byte[] userInfo = new byte[128];
            client.Skt.Receive(userInfo);
            string userName = this.GetStringFrom(userInfo);
            if (dicClientContext.ContainsKey(userName))
            {
                dicClientContext[userName].Skt.Send(new byte[] { (int)CmdRequest.Flush });
                dicClientContext[userName].Skt.Send(this.GetBytesFrom(svmsg));
            }
            else
            {
                string failedMsg = string.Format("[系统消息]您刚才的闪屏振动没有发送成功。\r\n可能原因：用户 {0} 已离线或者网络阻塞。\r\n\r\n", userName);
                client.Skt.Send(this.GetBytesFrom(failedMsg));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        [Obsolete("Discard version", false)]
        private void FlushAllProcess(ClientContext client)
        {
            string svmsg = string.Format("[系统提示]用户 {0} 向您发送了一个闪屏振动。\r\n\r\n", client.UserName);
            foreach (ClientContext cc in dicClientContext.Values)
            {
                if (cc.UserName != client.UserName)
                {
                    cc.Skt.Send(new byte[] { (int)CmdRequest.FlushAll });
                    cc.Skt.Send(this.GetBytesFrom(svmsg));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        [Obsolete("Discard version", false)]
        private void BroadcastProcess(ClientContext client)
        {
            byte[] msgBuf=new byte[MAXBUFSIZE];
            client.Skt.Receive(msgBuf);
            foreach (ClientContext cc in dicClientContext.Values)
            {
                if (cc.UserName != client.UserName)
                {
                    cc.Skt.Send(msgBuf);
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        [Obsolete("Use ClientOfflineProcess instead",false)]
        private void OfflineProcess(ClientContext client)
        {
            this.dicClientContext.Remove(client.UserName);

            CloseMsgHandle(client.MsgHandle);
            string sktmsg = string.Format("[系统消息]用户 {0} 在 {1} 已离线... 当前在线人数: {2}\r\n\r\n", 
                client.UserName, DateTime.Now, dicClientContext.Count);
            Console.WriteLine(sktmsg);
            this.UpdateCurrentUserList();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateCurrentUserList()
        {
            byte[] clientBytes = this.SerializeGraph<Dictionary<int,string>>(DicClient);
            
            this.currentRequest = CmdRequest.UpdateUsers;
            this.recvID = 0;
            byte[] msgBuf = this.CreateWRCmd(CurrentCmd, clientBytes);

            foreach (ClientContext cc in dicClientContext.Values)
            {
                cc.Skt.Send(msgBuf);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void PrintCurrentUsers()
        {
            if (dicClientContext.Count == 0) return;
            Console.WriteLine("\r\n-----------------------------------------------------------------------");
            Console.WriteLine(SysInfo.Timestamp + "CurrentUserInfo  TotalUser Counter = {0,2}\n", dicClientContext.Count);

            foreach (KeyValuePair<string, ClientContext> item in dicClientContext)
            {
                Console.WriteLine(SysInfo.Timestamp + "CurrentUserInfo  UserName = {0,2}  UserID = {1,2}", item.Key, item.Value.UserID);
            }
            Console.WriteLine("-----------------------------------------------------------------------\r\n");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void ClientOfflineProcess(ClientContext client)
        {
            if (CheckUserInvalid(client.UserName)) return;

            this.CloseSkt(client.Skt);

            this.senderID = client.UserID;

            this.dicClientContext.Remove(client.UserName);

            this.PrintCurrentUsers();

            this.UpdateCurrentUserList();
            string svmsg = string.Format("[系统消息] 用户 {0} 客户端在 {1} 离线！当前在线人数：{2}\r\n\r\n",
                client.UserName, DateTime.Now,dicClientContext.Count);
            Console.WriteLine(svmsg);
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        private void UserMessageProcess(ClientContext client)
        {
            CmdRequest offlineCmd = (CmdRequest)client.HeadBuf[1];
            if (offlineCmd == CmdRequest.Offline)
            {
                CloseMsgHandle(this.dicClientContext[client.UserName].MsgHandle);

                this.dicClientContext.Remove(client.UserName);

                string svmsg = string.Format("[系统消息]用户 {0} 在 {1} 离线... 当前在线人数: {2}\r\n\r\n",
                    client.UserName, DateTime.Now, this.dicClientContext.Count);
                Console.WriteLine(svmsg);

                foreach (ClientContext cc in dicClientContext.Values)
                {
                    this.currentRequest = CmdRequest.Offline;
                    this.senderID = client.UserID;
                    byte[] clientBuf = this.SerializeGraph<Dictionary<int, string>>(DicClient);
                    byte[] msgBuf = this.CreateWRCmd(CurrentCmd,clientBuf);
                    cc.Skt.Send(msgBuf);
                }

            }
            else
            {
                int senderID = client.HeadBuf[2];
                int recvID = client.HeadBuf[4];
                string userName = null;
                if (recvID == 0x00)
                {
                    foreach (ClientContext cc in dicClientContext.Values)
                    {
                        if (senderID == cc.UserID) continue;
                        cc.Skt.Send(client.Buf);
                    }
                }
                else
                {
                    if (!CheckUserID(recvID, out userName)) return;
                    this.dicClientContext[userName].Skt.Send(client.Buf);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="cmd"></param>
        [Obsolete("User UserMessageProcess instead!", true)]
        private void UserMsgProcess(ClientContext client, CmdRequest cmd)
        {
            CmdRequest currentCmd = cmd;
            switch (currentCmd)
            {
                case CmdRequest.Online:
                    OnlineProcess(client);
                    break;
                case CmdRequest.FixUser:
                    FixUserProcess(client);
                    break;
                case CmdRequest.Flush:
                    FlushProcess(client);
                    break;
                case CmdRequest.FlushAll:
                    FlushAllProcess(client);
                    break;
                case CmdRequest.Broadcast:
                    BroadcastProcess(client);
                    break;
                case CmdRequest.Offline:
                    OfflineProcess(client);
                    break;
                case CmdRequest.UpdateUsers:
                    this.UpdateCurrentUserList();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool CheckUserInvalid(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            if (this.dicClientContext.ContainsKey(name))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private bool CheckUserID(int userid,out string userName)
        {
            bool isExisted = false;
            userName = null;
            foreach (ClientContext cc in dicClientContext.Values)
            {
                if (cc.UserID == userid)
                {
                    userName = cc.UserName;
                    isExisted = true;
                }
            }
            return isExisted;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Obsolete("Discard Version,Use DicClient instead please!", false)]
        private StringCollection GetCurrentUserCollections()
        {
            StringCollection sc = new StringCollection();
            foreach (string user in dicClientContext.Keys)
            {
                sc.Add(user);
            }
            return sc;
        }

        /// <summary>
        /// 命令更新并打包
        /// 命令头格式: 命令头标志 + 请求模式+ 发送者 + 读写模式+ 接收者 + 预留
        /// 命令头长度: 8字节
        /// 
        /// 有效通信命令格式 ： 命令头 + 数据包
        /// </summary>
        private void UpdatePackageLCmd()
        {
            LCmd cmd = this.CreateCurrentLCmd();        // 获取当前命令包

            this.currentCmd[0] = (byte)cmd.Head;        // 有效命令开始标志(命令头)
            this.currentCmd[1] = (byte)cmd.CmdMode;     // 命令请求模式
            this.currentCmd[2] = (byte)cmd.SendID;      // 发送者用户ID
            this.currentCmd[3] = (byte)cmd.WR;          // 发送或读写模式
            this.currentCmd[4] = (byte)cmd.RecvID;      // 接收者用户ID
            this.currentCmd[5] = (byte)cmd.Resv2;       // 预留
            this.currentCmd[6] = (byte)cmd.Resv3;       // 预留
            this.currentCmd[7] = (byte)cmd.Resv4;       // 预留

        }

        /// <summary>
        /// 创建当前命令包
        /// </summary>
        /// <returns></returns>
        private LCmd CreateCurrentLCmd()
        {
            LCmd cmd = new LCmd();
            cmd.Head    = (int)ProtocolMsg.LCML;
            cmd.CmdMode = (int)currentRequest;
            cmd.SendID  = senderID;
            cmd.WR      = (int)currentRight;
            cmd.RecvID  = recvID;
            cmd.Resv2   = 0x00;
            cmd.Resv3   = 0x00;
            cmd.Resv4   = 0x00;
            return cmd;
        }
        #endregion

        #region InnerClass - Client Instance Context

        class ClientContext
        {
            internal ClientContext()
            {
            }
            internal byte[] Buf { get; set; }
            internal byte[] HeadBuf { get; set; }
            internal byte[] DataBuf { get; set; }
            internal int UserID { get; set; }
            internal string UserName { get; set; }
            internal Thread MsgHandle { get; set; }
            internal Socket Skt { get; set; }
        }
        #endregion

        #region Private Fields & Properties
        private const byte LHEAD = ProtocolMsg.LCML;
        private LProtocolRight currentRight = LProtocolRight.MinID;
        private CmdRequest currentRequest = CmdRequest.Failed;
        private int senderID = -1;
        private int recvID = 0;
        private byte[] currentCmd = new byte[ProtocolMsg.MSGLENS];
        private byte[] CurrentCmd
        {
            get 
            {
                this.UpdatePackageLCmd();
                return currentCmd; 
            }
        }


        private bool isdisposed = false;
        private bool isAlive = false;
        public bool IsAlive
        {
            get { return isAlive; }
        }
        private string hostName = string.Empty;
        private string name = string.Empty;
        public string Name
        {
            get { return name; }
        }

        private static object idlock = new object();
        private static int activeID = 0;
        private static int ActiveID
        {
            get
            {
                lock (ChatAgent.idlock)
                {
                    ChatAgent.activeID++;
                }
                return ChatAgent.activeID;
            }
        }

        private int port = 4001;                // Default port
        private TcpListener tcpListener = null;
        private IPAddress ipAddr = null;        //

        // Clients table
        private Dictionary<int, string> dicClient = new Dictionary<int, string>();
        private Dictionary<int, string> DicClient
        {
            get
            {
                try
                {
                    if (dicClientContext.Count == 0) return dicClient;
                    if (dicClient.Count != 0) this.dicClient.Clear();

                    foreach (ClientContext client in dicClientContext.Values)
                    {
                        this.dicClient.Add(client.UserID, client.UserName);
                    }
                }
                catch (Exception ex)
                {
                    if (dicClient == null)
                    {
                        dicClient = new Dictionary<int, string>();
                    }
                    if (dicClient.Count != 0) this.dicClient.Clear();
                    Innerlog.Error(dcrlringType, "Get the lastest client list error!", ex);
                }
                return dicClient;
            }
        }
        private Dictionary<string, ClientContext> dicClientContext = new Dictionary<string, ClientContext>();
        private Thread msgHandle = null;
        private Thread MsgHandle
        {
            get
            {
                return msgHandle;
            }
        }

        private readonly Type dcrlringType = typeof(ChatAgent);
        private const int MAXBUFSIZE = ProtocolMsg.MAXBUFSIZE;
        #endregion
    }
}
