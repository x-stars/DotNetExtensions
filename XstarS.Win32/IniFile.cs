﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using mstring = System.Text.StringBuilder;

namespace XstarS.Win32
{
    /// <summary>
    /// 提供 INI 配置文件的读写方法。
    /// </summary>
    [Serializable]
    public class IniFile
    {
        /// <summary>
        /// 提供 INI 配置文件读写相关的原生方法。
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            /// 读取指定 INI 配置文件中指定区块的指定键对应的值。
            /// </summary>
            /// <param name="lpAppName">区块名称。</param>
            /// <param name="lpKeyName">键名称。</param>
            /// <param name="lpDefault">结果默认值。</param>
            /// <param name="lpReturnedString">结果缓冲区。</param>
            /// <param name="nSize">结果缓冲区的大小。</param>
            /// <param name="lpFileName">INI 配置文件的路径。</param>
            /// <returns>结果字符串的长度。</returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern int GetPrivateProfileString(
                string lpAppName, string lpKeyName, string lpDefault,
                mstring lpReturnedString, int nSize, string lpFileName);

            /// <summary>
            /// 向指定 INI 配置文件中指定区块的指定键写入指定的值。
            /// </summary>
            /// <param name="lpAppName">区块名称。</param>
            /// <param name="lpKeyName">键名称。</param>
            /// <param name="lpString">要写入的值。</param>
            /// <param name="lpFileName">INI 配置文件的路径。</param>
            /// <returns>若写入成功，则为 <see langword="true"/>；
            /// 否则为 <see langword="false"/>。</returns>
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern bool WritePrivateProfileString(
                string lpAppName, string lpKeyName, string lpString, string lpFileName);

            /// <summary>
            /// 获取当前线程上次 P/Invoke 调用产生的错误码。
            /// </summary>
            /// <returns>当前线程上次 P/Invoke 调用产生的错误码。
            /// 若 P/Invoke 调用没有发生错误，则错误码为 0。</returns>
            [DllImport("kernel32.dll")]
            internal static extern int GetLastError();
        }

        /// <summary>
        /// 以指定 INI 配置文件的路径初始化 <see cref="IniFile"/> 类的新实例。
        /// </summary>
        /// <param name="filePath">INI 配置文件的路径。</param>
        public IniFile(string filePath)
        {
            this.FilePath = filePath;
        }

        /// <summary>
        /// 获取或设置当前 INI 配置文件中指定区块的指定键对应的值。
        /// </summary>
        /// <param name="app">区块名称。</param>
        /// <param name="key">键名称。</param>
        /// <returns>当前 INI 配置文件中指定区块的指定键对应的值。</returns>
        /// <exception cref="Win32Exception">对当前 INI 配置文件的读写失败。</exception>
        [IndexerName("Profile")]
        public string this[string app, string key]
        {
            get => this.ReadProfile(app, key);
            set => this.WriteProfile(app, key, value);
        }

        /// <summary>
        /// 获取 INI 配置文件的路径。
        /// </summary>
        /// <returns>INI 配置文件的路径。</returns>
        public string FilePath { get; }

        /// <summary>
        /// 读取当前 INI 配置文件中指定区块的指定键对应的值。
        /// </summary>
        /// <param name="app">区块名称。</param>
        /// <param name="key">键名称。</param>
        /// <returns>当前 INI 配置文件中指定区块的指定键对应的值。</returns>
        /// <exception cref="Win32Exception">从当前 INI 配置文件读取值失败。</exception>
        public string ReadProfile(string app, string key)
        {
            var result = new mstring(ushort.MaxValue);
            var length = NativeMethods.GetPrivateProfileString(
                app, key, string.Empty, result, result.Capacity, this.FilePath);
            if (length == 0)
            {
                var error = NativeMethods.GetLastError();
                if (error != 0) { throw new Win32Exception(error); }
            }
            result.Length = length;
            return result.ToString();
        }

        /// <summary>
        /// 向当前 INI 配置文件中指定区块的指定键写入指定的值。
        /// </summary>
        /// <param name="app">区块名称。</param>
        /// <param name="key">键名称。</param>
        /// <param name="value">要写入的值。</param>
        /// <exception cref="Win32Exception">向当前 INI 配置文件写入值失败。</exception>
        public void WriteProfile(string app, string key, string value)
        {
            var status = NativeMethods.WritePrivateProfileString(
                app, key, value, this.FilePath);
            if (!status)
            {
                var error = NativeMethods.GetLastError();
                if (error != 0) { throw new Win32Exception(error); }
            }
        }
    }
}
