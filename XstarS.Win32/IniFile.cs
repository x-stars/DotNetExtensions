using System.Runtime.InteropServices;
using mstring = System.Text.StringBuilder;

namespace XstarS.Win32
{
    /// <summary>
    /// 提供 INI 配置文件的读写方法。
    /// </summary>
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
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
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
            [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
            internal static extern bool WritePrivateProfileString(
                string lpAppName, string lpKeyName, string lpString, string lpFileName);
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
        /// 获取 INI 配置文件的路径。
        /// </summary>
        public string FilePath { get; }

        /// <summary>
        /// 读取当前 INI 配置文件中指定区块的指定键对应的值。
        /// </summary>
        /// <param name="app">区块名称。</param>
        /// <param name="key">键名称。</param>
        /// <returns>当前 INI 配置文件中指定区块的指定键对应的值。</returns>
        public string ReadValue(string app, string key)
        {
            var result = new mstring(ushort.MaxValue);
            IniFile.NativeMethods.GetPrivateProfileString(
                app, key, string.Empty, result, ushort.MaxValue, this.FilePath);
            return result.ToString();
        }

        /// <summary>
        /// 向当前 INI 配置文件中指定区块的指定键写入指定的值。
        /// </summary>
        /// <param name="app">区块名称。</param>
        /// <param name="key">键名称。</param>
        /// <param name="value">要写入的值。</param>
        /// <returns>若写入成功，则为 <see langword="true"/>；
        /// 否则为 <see langword="false"/>。</returns>
        public void WriteValue(string app, string key, string value)
        {
            IniFile.NativeMethods.WritePrivateProfileString(
                app, key, value, this.FilePath);
        }
    }
}
