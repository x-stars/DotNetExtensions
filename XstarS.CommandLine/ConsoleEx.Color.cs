﻿using System;
using System.IO;
using System.Security;

namespace XstarS
{
    partial class ConsoleEx
    {
        /// <summary>
        /// 将指定的字符串值以指定的前景色和背景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                Console.Write(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定的字符串值以指定的前景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteInColor(string value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                Console.Write(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色和背景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                Console.Write(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteInColor(object value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                Console.Write(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色和背景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLineInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                Console.WriteLine(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLineInColor(string value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                Console.WriteLine(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色和背景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLineInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                Console.WriteLine(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色写入标准输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLineInColor(object value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                Console.WriteLine(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定的字符串值以指定的前景色和背景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                ConsoleEx.WriteError(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定的字符串值以指定的前景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorInColor(string value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                ConsoleEx.WriteError(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色和背景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                ConsoleEx.WriteError(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式以指定的前景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorInColor(object value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                ConsoleEx.WriteError(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色和背景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorLineInColor(string value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                ConsoleEx.WriteErrorLine(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）以指定的前景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorLineInColor(string value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                ConsoleEx.WriteErrorLine(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色和背景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <param name="background">要使用的控制台背景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorLineInColor(object value, ConsoleColor foreground, ConsoleColor background)
        {
            lock (Console.Out)
            {
                Console.ForegroundColor = foreground;
                Console.BackgroundColor = background;
                ConsoleEx.WriteErrorLine(value);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）以指定的前景色写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <param name="foreground">要使用的控制台前景色。</param>
        /// <exception cref="ArgumentException">
        /// 指定的颜色不是 <see cref="ConsoleColor"/> 的有效成员。</exception>
        /// <exception cref="SecurityException">用户没有设置控制台颜色的权限。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteErrorLineInColor(object value, ConsoleColor foreground)
        {
            lock (Console.Out)
            {
                Console.ResetColor();
                Console.ForegroundColor = foreground;
                ConsoleEx.WriteErrorLine(value);
                Console.ResetColor();
            }
        }
    }
}
