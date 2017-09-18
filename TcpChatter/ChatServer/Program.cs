// created 20130809

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
using System.Threading;
using System.Windows.Forms;

using L.vivitue.Server;
using L.vivitue.Common;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int beginner = Win32Manager.TickCounter;
            Console.WriteLine("\r\n-----------------------------------------------------------------------");
            Console.WriteLine(SysInfo.Timestamp + "ChatServer is starting........\r\n");

            IChatAgent agent = new ChatAgent(null);
            int linkCounter = 0;
            bool isStarted = agent.StartChatServer();
            while (!agent.IsAlive)
            {
                if (linkCounter++ > 10)
                {
                    Console.WriteLine(SysInfo.Timestamp + "ChatServer start failed! Try LinkCounter = {0}",linkCounter);
                    break;
                }
                Thread.Sleep(100);
            }

            Console.WriteLine(SysInfo.Timestamp + "Total ElapsedTime = {0}ms", (Win32Manager.TickCounter - beginner));
            if (linkCounter < 10) Console.WriteLine(SysInfo.Timestamp + "ChatServer is running........");

            Console.WriteLine("-----------------------------------------------------------------------\r\n");
            Application.Run();
        }
    }
}
