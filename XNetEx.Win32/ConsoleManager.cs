using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace XstarS.Win32
{
    /// <summary>
    /// 提供窗口应用程序的控制台窗口管理方法。
    /// </summary>
    public static class ConsoleManager
    {
        /// <summary>
        /// 提供控制台相关的原生方法。
        /// </summary>
        private static class NativeMethods
        {
            /// <summary>
            /// 为当前应用程序分配控制台窗口。
            /// </summary>
            /// <returns>是否成功分配控制台窗口。</returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool AllocConsole();

            /// <summary>
            /// 释放当前已经分配的控制台窗口。
            /// </summary>
            /// <returns>是否成功释放控制台窗口。</returns>
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern bool FreeConsole();

            /// <summary>
            /// 获取当前已分配的控制台窗口的句柄。
            /// </summary>
            /// <returns>当前已分配的控制台窗口的句柄。
            /// 若并未分配控制台窗口，则返回 <see cref="IntPtr.Zero"/>。</returns>
            [DllImport("kernel32.dll")]
            internal static extern nint GetConsoleWindow();
        }

        /// <summary>
        /// 指示当前是否已经分配了控制台窗口。
        /// </summary>
        /// <returns>若当前已经分配了控制台窗口，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool HasWindow => NativeMethods.GetConsoleWindow() != (nint)0;

        /// <summary>
        /// 为当前应用程序分配控制台窗口。
        /// </summary>
        /// <exception cref="NotSupportedException">不支持当前操作。</exception>
        public static void ShowWindow()
        {
            if (!ConsoleManager.HasWindow)
            {
                if (!NativeMethods.AllocConsole())
                {
                    var error = Marshal.GetLastWin32Error();
                    if (error != 0) { throw new Win32Exception(error); }
                }
            }
        }

        /// <summary>
        /// 释放当前已经分配的控制台窗口。
        /// </summary>
        /// <exception cref="NotSupportedException">不支持当前操作。</exception>
        public static void CloseWindow()
        {
            if (ConsoleManager.HasWindow)
            {
                if (!NativeMethods.FreeConsole())
                {
                    var error = Marshal.GetLastWin32Error();
                    if (error != 0) { throw new Win32Exception(error); }
                }
            }
        }
    }
}
