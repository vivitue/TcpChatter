/***********************************************************************
 * Copyright(C) vivitue 2012-2013
 * 
 * Project    ： SysInfo - TcpChatter
 * Created by ： vivitue     
 * CreateDate ： 2013.08.09
 * ReviseData :  
 * References :  
 * 
 * Version    ： 1.0.0.0  
 * Description： System information process for CHTCommon component
 * ReviseDscpt:
 *              
 * *********************************************************************/

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
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using System.Runtime.InteropServices;

namespace L.vivitue.Common
{
    public static class SysInfo
    {
        #region Constructor
        static SysInfo()
        {
        }
        #endregion

        #region PublicInterfaces

        /// <summary>
        /// Extention method of SerializeGraph
        /// Refer to SerializeGraph
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exgraph"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        public static byte[] SerializeGraph<T>(this object exgraph, T graph)
        {
            return SysInfo.SerializeGraph<T>(graph);
        }

        /// <summary>
        /// Serialize object graph(Generic T) to byte stream
        /// </summary>
        /// <typeparam name="T">Generic type of any data type</typeparam>
        /// <param name="graph">Object needed to serialize</param>
        /// <returns>Byte stream</returns>
        public static byte[] SerializeGraph<T>(T graph)
        {
            byte[] bytesRet = null;
            MemoryStream mstream = null;
            IFormatter formatter = null;

            try
            {
                mstream = new MemoryStream();
                formatter = new BinaryFormatter();
                formatter.Serialize(mstream, graph);
                bytesRet = mstream.ToArray();
            }
            catch (Exception ex)
            {
                bytesRet = null;
                Innerlog.Error(dclringType, "SerializeGraph failed!", ex);
            }
            finally
            {
                if (mstream != null)
                {
                    mstream.Close();
                }
                mstream = null;
            }

            return bytesRet;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="exgraph"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static T DeserializeGraph<T>(this object exgraph, byte[] bytes)
        {
            return SysInfo.DeserializeGraph<T>(bytes);
        }

        /// <summary>
        /// Deserialize byte stream to specific object graph
        /// </summary>
        /// <typeparam name="T">Generic type of any object graph</typeparam>
        /// <param name="bytes">Byte stream needed to deserialize</param>
        /// <returns>object graph</returns>
        public static T DeserializeGraph<T>(byte[] bytes)
        {
            if (bytes == null) return default(T);

            T graph = default(T);

            MemoryStream mstream = null;
            IFormatter formatter = null;
            try
            {
                formatter = new BinaryFormatter();
                mstream = new MemoryStream();
                mstream.Write(bytes, 0, bytes.Length);
                mstream.Position = 0;
                graph = (T)formatter.Deserialize(mstream);

            }
            catch (Exception ex)
            {
                graph = default(T);
                Innerlog.Error(dclringType, "DeserializeGraph failed!", ex);
            }
            finally
            {
                if (mstream != null)
                {
                    mstream.Close();
                }
                mstream = null;
            }
            return graph;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static byte[] GetBytesFrom(this object graph,string message)
        {
            return SysInfo.GetBytesFrom(message);
        }

        /// <summary>
        /// Convert string to byte stream with UTF8 encoding mode
        /// </summary>
        /// <param name="message">string message needed to convert</param>
        /// <returns>byte stream</returns>
        public static byte[] GetBytesFrom(string message)
        {
            if (string.IsNullOrEmpty(message)) return null;
            return Encoding.UTF8.GetBytes(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetStringFrom(this object graph,byte[] bytes)
        {
            return SysInfo.GetStringFrom(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string GetStringFrom(byte[] bytes)
        {
            if (bytes == null) return null;
            return Encoding.UTF8.GetString(bytes).TrimEnd('\0');
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="bytes"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] CopyArrayFrom(this object graph,byte[] bytes, int length)
        {
            return SysInfo.CopyArrayFrom(bytes,length);
        }

        /// <summary>
        /// Get one part of an array
        /// </summary>
        /// <param name="bytes">Item byte array</param>
        /// <param name="length">Data length need to cut (less than bytes.Length)</param>
        /// <returns>byte array converted</returns>
        public static byte[] CopyArrayFrom(byte[] bytes, int length)
        {
            if (length <= 0) return null;
            if (bytes == null) return null;
            if (length > bytes.Length) return null;

            byte[] buf=new byte[length];
            for (int i = 0; i < length; i++)
            {
                buf[i] = bytes[i];
            }
            return buf;
        }

        /// <summary>
        /// Link two byte-arraies
        /// </summary>
        /// <param name="bytesOne"></param>
        /// <param name="bytesTwo"></param>
        /// <returns></returns>
        public static byte[] LinkArray(byte[] bytesOne, byte[] bytesTwo)
        {
            byte[] results = null;

            if (null == bytesOne && bytesTwo == null) return null;
            if (null == bytesOne && bytesTwo != null) return bytesTwo;

            if (null == bytesTwo)
            {

                results = new byte[bytesOne.Length];

                Buffer.BlockCopy(bytesOne, 0, results, 0, bytesOne.Length);
            }
            else
            {
                results = new byte[bytesOne.Length + bytesTwo.Length];
                Buffer.BlockCopy(bytesOne, 0, results, 0, bytesOne.Length);
                Buffer.BlockCopy(bytesTwo, 0, results, bytesOne.Length, sizeof(byte) * bytesTwo.Length);
            }
            return results;
        }
        #endregion

        #region Fields & Properties
        /// <summary>
        /// 
        /// </summary>
        public static string Timestamp
        {
            get { return DateTime.Now.ToString() + " : "; }
        }

        private static readonly Type dclringType = typeof(SysInfo);
        #endregion

    }
}
