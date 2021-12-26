using System;
using System.IO;
using System.Runtime.CompilerServices;
using XstarS.IO;

namespace XstarS
{
    partial class ConsoleEx
    {
        /// <summary>
        /// 提供控制台使用的标准输入输出流。
        /// </summary>
        private static class StandardStreams
        {
            /// <summary>
            /// 表示标准输入流。
            /// </summary>
            internal static readonly Stream Input =
                Stream.Synchronized(Console.OpenStandardInput().LeaveOpen());

            /// <summary>
            /// 表示标准输出流。
            /// </summary>
            internal static readonly Stream Output =
                Stream.Synchronized(Console.OpenStandardOutput().LeaveOpen());

            /// <summary>
            /// 表示标准错误输出流。
            /// </summary>
            internal static readonly Stream Error =
                Stream.Synchronized(Console.OpenStandardError().LeaveOpen());
        }

        /// <summary>
        /// 获取标准输入流。
        /// </summary>
        /// <returns>标准输入流。</returns>
        public static Stream InStream => StandardStreams.Input;

        /// <summary>
        /// 获取标准输出流。
        /// </summary>
        /// <returns>标准输出流。</returns>
        public static Stream OutStream => StandardStreams.Output;

        /// <summary>
        /// 获取标准错误输出流。
        /// </summary>
        /// <returns>标准错误输出流。</returns>
        public static Stream ErrorStream => StandardStreams.Error;

        /// <summary>
        /// 从标准输入流读取字节序列到指定的缓冲区，并将此流中的位置提升读取的字节数。
        /// </summary>
        /// <param name="buffer">包含读取得到的字节序列的缓冲区。</param>
        /// <param name="offset"><paramref name="buffer"/>
        /// 中的从零开始的字节偏移量，从此处开始存储从标准输入流中读取的数据。</param>
        /// <param name="count">要从标准输入流中最多读取的字节数。</param>
        /// <returns>读入缓冲区中的总字节数。</returns>
        /// <exception cref="ArgumentException"><paramref name="offset"/>
        /// 和 <paramref name="count"/> 的总和大于缓冲区长度。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> 或 <paramref name="count"/> 为负。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int ReadBytes(byte[] buffer, int offset, int count) =>
            ConsoleEx.InStream.Read(buffer, offset, count);

        /// <summary>
        /// 从标准输入流读取字节序列到指定的缓冲区，并将此流中的位置提升读取的字节数。
        /// </summary>
        /// <param name="buffer">包含读取得到的字节序列的缓冲区。</param>
        /// <returns>读入缓冲区中的总字节数。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public static int ReadBytes(Span<byte> buffer) => ConsoleEx.InStream.Read(buffer);
#else
        public static int ReadBytes(byte[] buffer) => ConsoleEx.InStream.Read(buffer);
#endif

        /// <summary>
        /// 向标准输出流写入指定的缓冲区中的字节序列，并将此流中的位置提升写入的字节数。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <param name="offset"><paramref name="buffer"/>
        /// 中的从零开始的字节偏移量，从此处开始将字节复制到标准输出流。</param>
        /// <param name="count">要写入标准输出流的字节数。</param>
        /// <exception cref="ArgumentException"><paramref name="offset"/>
        /// 和 <paramref name="count"/> 的总和大于缓冲区长度。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> 或 <paramref name="count"/> 为负。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteBytes(byte[] buffer, int offset, int count) =>
            ConsoleEx.OutStream.Write(buffer, offset, count);

        /// <summary>
        /// 向标准输出流写入指定的缓冲区中的字节序列，并将此流中的位置提升写入的字节数。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public static void WriteBytes(ReadOnlySpan<byte> buffer) => ConsoleEx.OutStream.Write(buffer);
#else
        public static void WriteBytes(byte[] buffer) => ConsoleEx.OutStream.Write(buffer);
#endif

        /// <summary>
        /// 向标准错误输出流写入指定的缓冲区中的字节序列，并将此流中的位置提升写入的字节数。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <param name="offset"><paramref name="buffer"/>
        /// 中的从零开始的字节偏移量，从此处开始将字节复制到标准错误输出流。</param>
        /// <param name="count">要写入标准错误输出流的字节数。</param>
        /// <exception cref="ArgumentException"><paramref name="offset"/>
        /// 和 <paramref name="count"/> 的总和大于缓冲区长度。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="offset"/> 或 <paramref name="count"/> 为负。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorBytes(byte[] buffer, int offset, int count) =>
            ConsoleEx.ErrorStream.Write(buffer, offset, count);

        /// <summary>
        /// 向标准错误输出流写入指定的缓冲区中的字节序列，并将此流中的位置提升写入的字节数。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        public static void WriteErrorBytes(ReadOnlySpan<byte> buffer) => ConsoleEx.ErrorStream.Write(buffer);
#else
        public static void WriteErrorBytes(byte[] buffer) => ConsoleEx.ErrorStream.Write(buffer);
#endif
    }
}
