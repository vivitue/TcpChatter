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
namespace L.vivitue.Common
{
    public static class ProtocolMsg
    {
        #region Constructor
        static ProtocolMsg() { }
        #endregion

        #region PublicInterfaces

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static CmdRequest GetValidCmd(this object graph,string cmd)
        {
            return ProtocolMsg.GetValidCmd(cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static CmdRequest GetValidCmd(string cmd)
        {
            int cmdID = -1;
            CmdRequest result=CmdRequest.Failed;
            if(!Enum.IsDefined(typeof(CmdRequest),cmd))
            {
                return CmdRequest.InvalidCmd;
            }
            return (CmdRequest) Enum.Parse(typeof(CmdRequest),cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="cmds"></param>
        /// <returns></returns>
        public static CmdRequest GetCmd(this object graph,byte[] cmds)
        {
            return ProtocolMsg.GetCmd(cmds);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmds"></param>
        /// <returns></returns>
        public static CmdRequest GetCmd(byte[] cmds)
        {
            if (cmds == null) return CmdRequest.InvalidCmd;
            int cmdID = (int)cmds[0];
            string cmd = cmdID.ToString();

            if (cmdID < (int)CmdRequest.MinID || cmdID > (int)CmdRequest.MaxID)
            {
                return CmdRequest.InvalidCmd;
            }
            return (CmdRequest)Enum.Parse(typeof(CmdRequest), cmd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="cmdHeader"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CreateWRCmd(this object graph,byte[] cmdHeader, byte[] data)
        {
            return ProtocolMsg.CreateWRCmd(cmdHeader, data);
        }

        /// <summary>
        /// 创建有效发送指令
        /// 指令格式 ： 命令头 + 数据包
        /// </summary>
        /// <param name="cmdHeader"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] CreateWRCmd(byte[] cmdHeader, byte[] data)
        {
            byte[] combineCmds = null;
            if (cmdHeader == null) return null;
            if (cmdHeader.Length != 8)
            {
                Innerlog.Error(dclringType,"Invalid LCmd header!",null);
                return null;
            }
            combineCmds = SysInfo.LinkArray(cmdHeader, data);
            return combineCmds;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="readBuf"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LErrorCode ResolveDataPackage(this object graph,byte[] readBuf, out byte[] header, out byte[] data)
        {
            return ProtocolMsg.ResolveDataPackage(readBuf,out header,out data);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readBuf"></param>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static LErrorCode ResolveDataPackage(byte[] readBuf, out byte[] header, out byte[] data)
        {
            header = null;
            data = null;
            if (readBuf == null) return LErrorCode.InvalidReadData;
            if (readBuf.Length < MSGLENS) return LErrorCode.InvalidReadData;

            header = new byte[MSGLENS];
            if (readBuf.Length != MSGLENS) data = new byte[readBuf.Length - MSGLENS];
            int i = 0;
            for (i = 0; i < readBuf.Length; i++)
            {
                if (i < MSGLENS)
                {
                    header[i] = readBuf[i];
                }
                else
                {
                    data[i - MSGLENS] = readBuf[i];
                }
            }

            return LErrorCode.Success;
        }
        #endregion

        #region Fields & Properties
        private static readonly Type dclringType=typeof(ProtocolMsg);
        public const int MSGLENS = 8;           // TcpChatter 命令头命令长度  8 byte
        public const byte LCML = 0x03;          // TcpChatter 命令头有效起始标志
        public const int MAXBUFSIZE = 20 * 1024;// TcpChatter 发送与接收消息最大缓冲长度 20kb
        #endregion

    }
}
