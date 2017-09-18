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
using System.Diagnostics;

namespace L.vivitue.Common
{
    public static class Innerlog
    {
        #region Constructor
        static Innerlog()
        {
            Initialize();
        }
        #endregion

        #region Private Helpers

        /// <summary>
        /// 
        /// </summary>
        private static void Initialize()
        {
            Trace.Listeners.Clear();
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private static void EmitConsoleMessage(string message)
        {
            try
            {
                Trace.TraceError(SysInfo.Timestamp + message);
            }
            catch (Exception ex)
            {
                // ignore
            }
        }
        #endregion

        #region PublicInterfaces

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="message"></param>
        public static void Error(this object graph, string message)
        {
            Innerlog.Error(dcrlringType, message, null);
        }

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(Type type, string message, Exception ex)
        {
            if (type == null) return;
            EmitConsoleMessage(type.FullName + " : " + message);
            if (ex != null)
            {
                EmitConsoleMessage(ex.Message);
            }
        }
        #endregion

        #region Properties & Fields
        private static readonly Type dcrlringType = typeof(Innerlog);
        #endregion
    }
}
