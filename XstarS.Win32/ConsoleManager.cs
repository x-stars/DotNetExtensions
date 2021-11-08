using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
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

            /// <summary>
            /// 获取当前线程上次 P/Invoke 调用产生的错误码。
            /// </summary>
            /// <returns>当前线程上次 P/Invoke 调用产生的错误码。
            /// 若 P/Invoke 调用没有发生错误，则错误码为 0。</returns>
            [DllImport("kernel32.dll")]
            internal static extern int GetLastError();
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
                    var error = NativeMethods.GetLastError();
                    if (error != 0) { throw new Win32Exception(error); }
                }
                ConsoleManager.InitializeStdOutError();
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
                ConsoleManager.RealeaseStdOutError();
                if (!NativeMethods.FreeConsole())
                {
                    var error = NativeMethods.GetLastError();
                    if (error != 0) { throw new Win32Exception(error); }
                }
            }
        }

        /// <summary>
        /// 重新初始化 <see cref="Console"/> 的标准输出流和错误流。
        /// </summary>
        /// <exception cref="NotSupportedException">不支持当前操作。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static void InitializeStdOutError()
        {
            try
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
            catch (Exception e)
            {
                throw new NotSupportedException(e.Message, e);
            }
        }

        /// <summary>
        /// 释放 <see cref="Console"/> 的标准输出流和错误流。
        /// </summary>
        /// <exception cref="NotSupportedException">不支持当前操作。</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static void RealeaseStdOutError()
        {
            try
            {
                Console.SetOut(TextWriter.Null);
                Console.SetError(TextWriter.Null);
            }
            catch (Exception e)
            {
                throw new NotSupportedException(e.Message, e);
            }
        }
    }
}
