using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;

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
        [SuppressUnmanagedCodeSecurity]
        private static class UnsafeNativeMethods
        {
            /// <summary>
            /// 为当前应用程序分配控制台窗口。
            /// </summary>
            /// <returns>是否成功分配控制台窗口。</returns>
            [DllImport("kernel32.dll")]
            internal static extern bool AllocConsole();

            /// <summary>
            /// 释放当前已经分配的控制台窗口。
            /// </summary>
            /// <returns>是否成功释放控制台窗口。</returns>
            [DllImport("kernel32.dll")]
            internal static extern bool FreeConsole();

            /// <summary>
            /// 获取当前已分配的控制台窗口的句柄。
            /// </summary>
            /// <returns>当前已分配的控制台窗口的句柄。
            /// 若并未分配控制台窗口，则返回 <see cref="IntPtr.Zero"/>。</returns>
            [DllImport("kernel32.dll")]
            internal static extern IntPtr GetConsoleWindow();
        }

        /// <summary>
        /// 指示当前是否已经分配了控制台窗口。
        /// </summary>
        public static bool HasConsole =>
            ConsoleManager.UnsafeNativeMethods.GetConsoleWindow() != IntPtr.Zero;

        /// <summary>  
        /// 为当前应用程序分配控制台窗口。
        /// </summary>  
        public static void ShowConsole()
        {
            if (!ConsoleManager.HasConsole)
            {
                ConsoleManager.UnsafeNativeMethods.AllocConsole();
                ConsoleManager.InitializeStdOutError();
            }
        }

        /// <summary>  
        /// 释放当前已经分配的控制台窗口。
        /// </summary>  
        public static void CloseConsole()
        {
            if (ConsoleManager.HasConsole)
            {
                ConsoleManager.RealeaseStdOutError();
                ConsoleManager.UnsafeNativeMethods.FreeConsole();
            }
        }

        /// <summary>
        /// 重新初始化 <see cref="Console"/> 的标准输出流和错误流。
        /// </summary>
        private static void InitializeStdOutError()
        {
            var fieldStdOut = typeof(Console).GetField(
                "_out", BindingFlags.Static | BindingFlags.NonPublic);
            var fieldStdError = typeof(Console).GetField(
                "_error", BindingFlags.Static | BindingFlags.NonPublic);
            var methodInitializeStdOutError = typeof(Console).GetMethod(
                "InitializeStdOutError", BindingFlags.Static | BindingFlags.NonPublic);
            fieldStdOut.SetValue(null, null);
            fieldStdError.SetValue(null, null);
            methodInitializeStdOutError.Invoke(null, new object[] { true });
        }

        /// <summary>
        /// 释放 <see cref="Console"/> 的标准输出流和错误流。
        /// </summary>
        private static void RealeaseStdOutError()
        {
            Console.SetOut(TextWriter.Null);
            Console.SetError(TextWriter.Null);
        }
    }
}
