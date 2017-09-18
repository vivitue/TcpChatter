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
    /// <summary>
    /// 应答请求命令
    /// </summary>
    public enum CmdRequest
    {
        MinID = -1,             
        Online      = 0x01,     // 在线请求
        FixUser     = 0x02,     // 向固定用户发送消息请求
        Flush       = 0x03,     // 向固定用户闪屏请求
        FlushAll    = 0x04,     // 向所有用户闪屏请求
        Broadcast   = 0x05,     // 广播消息请求
        Offline     = 0x06,     // 离线请求
        UpdateUsers = 0x07,     // 用户列表更新请求
        Success     = 0x08,     // 用户连接服务成功应答
        InvalidUser = 0x09,     // 非法用户名 - (预留)
        Failed      = 0x0A,     // 用户连接服务失败应答
        InvalidCmd  = 0xFF,     // 非法命令包 - (预留)
        MaxID,
    }

    /// <summary>
    /// 命令收发模式
    /// </summary>
    public enum LProtocolRight
    {
        MinID = -1,
        RD = 0x00,      // 消息接收模式
        WR = 0x01,      // 消息发送模式
        MaxID,
    }

    /// <summary>
    /// 通信错误码
    /// </summary>
    public enum LErrorCode
    {
        MinID = -1,
        Success = 0,
        Failed = 1,
        InvalidCmd = 10,
        InvalidUser = 11,
        InvalidCmdHead = 12,
        InvalidCmdLength = 13,
        InvalidReadData = 14,
        MaxID
    }
}
