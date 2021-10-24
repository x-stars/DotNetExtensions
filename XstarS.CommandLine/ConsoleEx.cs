using System;
using System.IO;
using System.Runtime.CompilerServices;
using XstarS.IO;

namespace XstarS
{
    /// <summary>
    /// 提供控制台应用程序的输入流、输出流和错误流的扩展方法。
    /// </summary>
    public static partial class ConsoleEx
    {
        /// <summary>
        /// 提供控制台的标准输入流、输出流和错误流。
        /// </summary>
        private static class StandardStreams
        {
            /// <summary>
            /// 表示标准输入流。
            /// </summary>
            private static volatile Stream? Input;

            /// <summary>
            /// 表示标准输出流。
            /// </summary>
            private static volatile Stream? Output;

            /// <summary>
            /// 表示标准错误流。
            /// </summary>
            private static volatile Stream? Error;

            /// <summary>
            /// 获取标准输入流；若标准输入流已经释放，则重新获取标准输入流。
            /// </summary>
            /// <returns>标准输入流。</returns>
            internal static Stream GetOrReloadInput()
            {
                if (StandardStreams.IsNullOrDisposed(StandardStreams.Input))
                {
                    lock (typeof(StandardStreams))
                    {
                        if (StandardStreams.IsNullOrDisposed(StandardStreams.Input))
                        {
                            StandardStreams.Input = Console.OpenStandardInput();
                        }
                    }
                }
                return StandardStreams.Input!;
            }

            /// <summary>
            /// 获取标准输出流；若标准输出流已经释放，则重新获取标准输出流。
            /// </summary>
            /// <returns>标准输出流。</returns>
            internal static Stream GetOrReloadOutput()
            {
                if (StandardStreams.IsNullOrDisposed(StandardStreams.Output))
                {
                    lock (typeof(StandardStreams))
                    {
                        if (StandardStreams.IsNullOrDisposed(StandardStreams.Output))
                        {
                            StandardStreams.Output = Console.OpenStandardOutput();
                        }
                    }
                }
                return StandardStreams.Output!;
            }

            /// <summary>
            /// 获取标准错误流；若标准错误流已经释放，则重新获取标准错误流。
            /// </summary>
            /// <returns>标准错误流。</returns>
            internal static Stream GetOrReloadError()
            {
                if (StandardStreams.IsNullOrDisposed(StandardStreams.Error))
                {
                    lock (typeof(StandardStreams))
                    {
                        if (StandardStreams.IsNullOrDisposed(StandardStreams.Error))
                        {
                            StandardStreams.Error = Console.OpenStandardError();
                        }
                    }
                }
                return StandardStreams.Error!;
            }

            /// <summary>
            /// 确定流是否为 <see langword="null"/> 或已经被释放。
            /// </summary>
            /// <param name="stream">要确定状态的流。</param>
            /// <returns>若流为 <see langword="null"/> 或已经被释放，
            /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
            private static bool IsNullOrDisposed(Stream? stream) =>
                (stream is null) || (!stream.CanRead && !stream.CanWrite);
        }

        /// <summary>
        /// 获取标准输入流。
        /// </summary>
        /// <returns>标准输入流。</returns>
        public static Stream InStream => StandardStreams.GetOrReloadInput();

        /// <summary>
        /// 获取标准输出流。
        /// </summary>
        /// <returns>标准输出流。</returns>
        public static Stream OutStream => StandardStreams.GetOrReloadOutput();

        /// <summary>
        /// 获取标准错误流。
        /// </summary>
        /// <returns>标准错误流。</returns>
        public static Stream ErrorStream => StandardStreams.GetOrReloadError();

        /// <summary>
        /// 从标准输入流读取下一个字符串值。
        /// </summary>
        /// <returns>输入流中的下一个字符串值；
        /// 如果当前没有更多的可用字符串值，则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一个字符串值的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一个字符串值分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string ReadToken() => Console.In.ReadToken();

        /// <summary>
        /// 从标准输入流读取下一个字符串值，并将其转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流中的下一个字符串值的数值形式。</returns>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一个字符串值的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一个字符串值分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// 读取到的字符串表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T ReadTokenAs<T>() => Console.In.ReadTokenAs<T>();

        /// <summary>
        /// 从标准输入流读取下一行字符串，并将其转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流中的下一行字符串的数值形式。</returns>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一个字符串值的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一个字符串值分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// 读取到的字符串表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T ReadLineAs<T>() => Console.In.ReadLineAs<T>();

        /// <summary>
        /// 从标准输入流读取下一行的所有字符串值。
        /// </summary>
        /// <returns>输入流中的下一行包含的所有字符串值。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一行中的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string[] ReadLineTokens() => Console.In.ReadLineTokens();

        /// <summary>
        /// 从标准输入流读取下一行字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流中的下一行包含的所有字符串值的数值形式。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一行中的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// 读取到的字符串表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T[] ReadLineTokensAs<T>() => Console.In.ReadLineTokensAs<T>();

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符。
        /// </summary>
        /// <returns>输入流到末尾的所有字符。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为到末尾的字符串分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string ReadToEnd() => Console.In.ReadToEnd();

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符串，并将其转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流读取到末尾的字符串的数值形式。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T ReadToEndAs<T>() => Console.In.ReadToEndAs<T>();

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符串行。
        /// </summary>
        /// <returns>输入流到末尾的所有字符串行。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为到末尾的字符串分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string[] ReadLinesToEnd() => Console.In.ReadLinesToEnd();

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符，并将其包含的所有字符串行转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流读取到末尾的所有字符串行的数值形式。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T[] ReadLinesToEndAs<T>() => Console.In.ReadLinesToEndAs<T>();

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符串值。
        /// </summary>
        /// <returns>输入流到末尾的所有字符串值。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为到末尾的字符串分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string[] ReadTokensToEnd() => Console.In.ReadTokensToEnd();

        /// <summary>
        /// 从标准输入流读取到末尾的所有字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>输入流读取到末尾的所有字符串值的数值形式。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static T[] ReadTokensToEndAs<T>() => Console.In.ReadTokensToEndAs<T>();

        /// <summary>
        /// 将指定的多个字符串值逐个（后跟当前行终止符）写入标准输出流。
        /// </summary>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteTokens(params string[] values) => Console.Out.WriteTokens(values);

        /// <summary>
        /// 将指定的多个对象的文本表示形式逐个（后跟当前行终止符）写入标准输出流。
        /// </summary>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteTokens(params object[] values) => Console.Out.WriteTokens(values);

        /// <summary>
        /// 将指定的多个字符串值逐行（后跟当前行终止符）写入标准输出流。
        /// </summary>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLines(params string[] values) => Console.Out.WriteLines(values);

        /// <summary>
        /// 将指定的多个对象的文本表示形式逐行（后跟当前行终止符）写入标准输出流。
        /// </summary>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteLines(params object[] values) => Console.Out.WriteLines(values);

        /// <summary>
        /// 将指定的字符串值写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteError(string value) => Console.Error.Write(value);

        /// <summary>
        /// 将指定对象的文本表示形式写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteError(object value) => Console.Error.Write(value);

        /// <summary>
        /// 将指定的 Unicode 字符数组写入标准错误输出流。
        /// </summary>
        /// <param name="buffer">Unicode 字符数组。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteError(char[] buffer) => Console.Error.Write(buffer);

        /// <summary>
        /// 将指定的 Unicode 字符子数组写入标准错误输出流。
        /// </summary>
        /// <param name="buffer">Unicode 字符的数组。</param>
        /// <param name="index"><paramref name="buffer"/> 中的起始位置。</param>
        /// <param name="count">要写入的字符数。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 或 <paramref name="count"/> 小于零。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> 加 <paramref name="count"/>
        /// 指定不在 <paramref name="buffer"/> 内的位置。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteError(char[] buffer, int index, int count) =>
            Console.Error.Write(buffer, index, count);

        /// <summary>
        /// 使用指定的格式信息，将指定对象的文本表示形式写入标准错误输出流。
        /// </summary>
        /// <param name="format">复合格式字符串。</param>
        /// <param name="arg">要使用 <paramref name="format"/> 写入的对象的数组。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="format"/> 或 <paramref name="arg"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FormatException"><paramref name="format"/> 中的格式规范无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteError(string format, params object[] arg) =>
            Console.Error.Write(format, arg);

        /// <summary>
        /// 将当前行终止符写入标准错误输出流。
        /// </summary>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLine() => Console.Error.WriteLine();

        /// <summary>
        /// 将指定的字符串值（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLine(string value) => Console.Error.WriteLine(value);

        /// <summary>
        /// 将指定对象的文本表示形式（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="value">要写入的值。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLine(object value) => Console.Error.WriteLine(value);

        /// <summary>
        /// 将指定的 Unicode 字符数组（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="buffer">Unicode 字符数组。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLine(char[] buffer) => Console.Error.WriteLine(buffer);

        /// <summary>
        /// 将指定的 Unicode 字符子数组（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="buffer">Unicode 字符的数组。</param>
        /// <param name="index"><paramref name="buffer"/> 中的起始位置。</param>
        /// <param name="count">要写入的字符数。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 或 <paramref name="count"/> 小于零。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> 加 <paramref name="count"/>
        /// 指定不在 <paramref name="buffer"/> 内的位置。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLine(char[] buffer, int index, int count) =>
            Console.Error.WriteLine(buffer, index, count);

        /// <summary>
        /// 使用指定的格式信息，将指定对象的文本表示形式（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="format">复合格式字符串。</param>
        /// <param name="arg">要使用 <paramref name="format"/> 写入的对象的数组。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="format"/> 或 <paramref name="arg"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FormatException"><paramref name="format"/> 中的格式规范无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLine(string format, params object[] arg) =>
            Console.Error.WriteLine(format, arg);

        /// <summary>
        /// 将指定的多个字符串值逐个（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorTokens(params string[] values) => Console.Error.WriteTokens(values);

        /// <summary>
        /// 将指定的多个对象的文本表示形式逐个（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorTokens(params object[] values) => Console.Error.WriteTokens(values);

        /// <summary>
        /// 将指定的多个字符串值逐行（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLines(params string[] values) => Console.Error.WriteLines(values);

        /// <summary>
        /// 将指定的多个对象的文本表示形式逐行（后跟当前行终止符）写入标准错误输出流。
        /// </summary>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorLines(params object[] values) => Console.Error.WriteLines(values);
    }
}
