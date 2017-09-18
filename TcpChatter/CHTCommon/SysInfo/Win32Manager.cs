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
using System.Runtime.InteropServices;

namespace L.vivitue.Common
{
    public static class Win32Manager
    {
        #region Constructor
        static Win32Manager()
        {
        }
        #endregion

        #region Win32 Interfaces
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern uint GetTickCount();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static int GetTickCount(this object graph)
        {
            return (int)Win32Manager.GetTickCount();
        }

        #endregion

        #region Fields & Properties

        /// <summary>
        /// 
        /// </summary>
        public static int TickCounter
        {
            get { return (int)Win32Manager.GetTickCount(); }
        }
        #endregion
    }
}
