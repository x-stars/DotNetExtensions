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
        /// 从标准输入流中读取一个字节，或者如果已到达流结尾，则返回 -1。
        /// </summary>
        /// <returns>强制转换为 <see cref="int"/> 的无符号字节，如果到达流的末尾，则为 -1。</returns>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int ReadByte() => ConsoleEx.InStream.ReadByte();

        /// <summary>
        /// 从标准输入流读取字节序列到指定的缓冲区。
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
        /// 从标准输入流读取字节序列到指定的缓冲区。
        /// </summary>
        /// <param name="buffer">包含读取得到的字节序列的缓冲区。</param>
        /// <returns>读入缓冲区中的总字节数。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int ReadBytes(byte[] buffer) => ConsoleEx.InStream.Read(buffer);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// 从标准输入流读取字节序列到指定的缓冲区。
        /// </summary>
        /// <param name="buffer">包含读取得到的字节序列的缓冲区。</param>
        /// <returns>读入缓冲区中的总字节数。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int ReadBytes(Span<byte> buffer) => ConsoleEx.InStream.Read(buffer);
#endif

        /// <summary>
        /// 从标准输入流读取指定类型的非托管数据。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="value">读取得到的非托管数据。</param>
        /// <returns>是否读取到完整大小的非托管数据。</returns>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool ReadBinary<T>(out T value) where T : unmanaged =>
            ConsoleEx.InStream.Read(out value);

        /// <summary>
        /// 向标准输出流写入指定的缓冲区中的字节序列。
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
        /// 向标准输出流写入指定的缓冲区中的字节序列。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteBytes(byte[] buffer) => ConsoleEx.OutStream.Write(buffer);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// 向标准输出流写入指定的缓冲区中的字节序列。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteBytes(ReadOnlySpan<byte> buffer) => ConsoleEx.OutStream.Write(buffer);
#endif

        /// <summary>
        /// 向标准输出流写入指定类型的非托管数据。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="value">要写入的非托管数据。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteBinary<T>(in T value) where T : unmanaged =>
            ConsoleEx.OutStream.Write(in value);

        /// <summary>
        /// 向标准错误输出流写入指定的缓冲区中的字节序列。
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
        /// 向标准错误输出流写入指定的缓冲区中的字节序列。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorBytes(byte[] buffer) => ConsoleEx.ErrorStream.Write(buffer);

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// 向标准错误输出流写入指定的缓冲区中的字节序列。
        /// </summary>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorBytes(ReadOnlySpan<byte> buffer) => ConsoleEx.ErrorStream.Write(buffer);
#endif

        /// <summary>
        /// 向标准错误输出流写入指定类型的非托管数据。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="value">要写入的非托管数据。</param>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void WriteErrorBinary<T>(in T value) where T : unmanaged =>
            ConsoleEx.ErrorStream.Write(in value);
    }
}
