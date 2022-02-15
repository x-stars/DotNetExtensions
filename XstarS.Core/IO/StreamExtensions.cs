using System;
using System.IO;
using System.Text;

namespace XstarS.IO
{
    /// <summary>
    /// 提供字节流 <see cref="Stream"/> 的扩展方法。
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// 表示 <see cref="Stream"/> 读写文本时默认使用的字符编码。
        /// </summary>
        internal static readonly Encoding DefaultEncoding =
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);

        /// <summary>
        /// 表示线程安全（同步）的 <see cref="Stream"/> 的类型。
        /// </summary>
        private static readonly Type SyncStreamType =
            Stream.Synchronized(Stream.Null).GetType();

        /// <summary>
        /// 创建以当前流为基础的二进制读取器 <see cref="BinaryReader"/>，
        /// 使用指定的字符编码，并选择是否保持流处于打开状态。
        /// </summary>
        /// <param name="stream">要创建二进制读取器的流。</param>
        /// <param name="encoding">要使用的字符编码。</param>
        /// <param name="leaveOpen">是否在释放后保持流处于打开状态。</param>
        /// <returns>以 <paramref name="stream"/> 为基础创建的 <see cref="BinaryReader"/>，
        /// 使用 <paramref name="encoding"/> 作为字符编码。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> 不可读。</exception>
        public static BinaryReader AsBinaryReader(
            this Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            encoding ??= StreamExtensions.DefaultEncoding;
            return new BinaryReader(stream, encoding, leaveOpen);
        }

        /// <summary>
        /// 创建以当前流为基础的二进制写入器 <see cref="BinaryWriter"/>，
        /// 使用指定的字符编码，并选择是否保持流处于打开状态。
        /// </summary>
        /// <param name="stream">要创建二进制写入器的流。</param>
        /// <param name="encoding">要使用的字符编码。</param>
        /// <param name="leaveOpen">是否在释放后保持流处于打开状态。</param>
        /// <returns>以 <paramref name="stream"/> 为基础创建的 <see cref="BinaryWriter"/>，
        /// 使用 <paramref name="encoding"/> 作为字符编码。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> 不可写。</exception>
        public static BinaryWriter AsBinaryWriter(
            this Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            encoding ??= StreamExtensions.DefaultEncoding;
            return new BinaryWriter(stream, encoding, leaveOpen);
        }

        /// <summary>
        /// 创建以当前流为基础的文本读取器 <see cref="TextReader"/>，
        /// 使用指定的字符编码，并选择是否保持流处于打开状态。
        /// </summary>
        /// <param name="stream">要创建文本读取器的流。</param>
        /// <param name="encoding">要使用的字符编码。</param>
        /// <param name="leaveOpen">是否在释放后保持流处于打开状态。</param>
        /// <returns>以 <paramref name="stream"/> 为基础创建的 <see cref="TextReader"/>，
        /// 使用 <paramref name="encoding"/> 作为字符编码。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> 不可读。</exception>
        public static StreamReader AsTextReader(
            this Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            encoding ??= StreamExtensions.DefaultEncoding;
            return new StreamReader(stream, encoding,
                detectEncodingFromByteOrderMarks: true, bufferSize: -1, leaveOpen);
        }

        /// <summary>
        /// 创建以当前流为基础的文本写入器 <see cref="TextWriter"/>，
        /// 使用指定的字符编码，并选择是否保持流处于打开状态。
        /// </summary>
        /// <param name="stream">要创建文本写入器的流。</param>
        /// <param name="encoding">要使用的字符编码。</param>
        /// <param name="leaveOpen">是否在释放后保持流处于打开状态。</param>
        /// <returns>以 <paramref name="stream"/> 为基础创建的 <see cref="TextWriter"/>，
        /// 使用 <paramref name="encoding"/> 作为字符编码。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="stream"/> 不可写。</exception>
        public static StreamWriter AsTextWriter(
            this Stream stream, Encoding? encoding = null, bool leaveOpen = false)
        {
            encoding ??= StreamExtensions.DefaultEncoding;
            return new StreamWriter(stream, encoding, bufferSize: -1, leaveOpen);
        }

        /// <summary>
        /// 确认当前流是否为线程安全（同步）的包装 <see cref="Stream"/>。
        /// </summary>
        /// <param name="stream">要确定是否线程安全的流。</param>
        /// <returns>若 <paramref name="stream"/> 为线程安全（同步）的包装；
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        internal static bool IsSynchronized(this Stream stream)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return stream.GetType() == StreamExtensions.SyncStreamType;
        }

        /// <summary>
        /// 获取当前流的包装 <see cref="Stream"/>，使其在释放后保持打开状态。
        /// </summary>
        /// <param name="stream">要保持打开状态的流。</param>
        /// <returns><paramref name="stream"/> 的包装 <see cref="Stream"/>，
        /// 在流释放后保持 <paramref name="stream"/> 保持打开状态。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        public static Stream LeaveOpen(this Stream stream)
        {
            return (stream is LeaveOpenStream leaveOpenStream) ?
                leaveOpenStream : new LeaveOpenStream(stream);
        }

        /// <summary>
        /// 从当前流读取字节序列到指定的缓冲区，并将此流中的位置提升读取的字节数。
        /// </summary>
        /// <param name="stream">要读取的流。</param>
        /// <param name="buffer">包含读取得到的字节序列的缓冲区。</param>
        /// <returns>读取到缓冲区的总字节数。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>
        /// 或 <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">流已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static int Read(this Stream stream, byte[] buffer)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            return stream.Read(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 从当前流读取指定类型的非托管数据，并将此流中的位置提升非托管数据大小的字节数。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="stream">要读取的流。</param>
        /// <param name="value">读取得到的非托管数据。</param>
        /// <returns>是否读取到完整大小的非托管数据。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">流已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static unsafe bool Read<T>(this Stream stream, out T value)
            where T : unmanaged
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var size = sizeof(T);
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            var buffer = (stackalloc byte[size]);
#else
            var buffer = (new byte[size]);
#endif
            var length = stream.Read(buffer);
            fixed (byte* pBuffer = buffer)
            {
                value = *(T*)pBuffer;
            }
            return length == size;
        }

        /// <summary>
        /// 向当前流写入指定的缓冲区中的字节序列，并将此流中的位置提升写入的字节数。
        /// </summary>
        /// <param name="stream">要写入的流。</param>
        /// <param name="buffer">包含要写入的字节序列的缓冲区。</param>
        /// <exception cref="ArgumentNullException"><paramref name="stream"/>
        /// 或 <paramref name="buffer"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">流已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void Write(this Stream stream, byte[] buffer)
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 向当前流写入指定类型的非托管数据，并将此流中的位置提升非托管数据大小的字节数。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="stream">要写入的流。</param>
        /// <param name="value">要写入的非托管数据。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="stream"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">流已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static unsafe void Write<T>(this Stream stream, in T value)
            where T : unmanaged
        {
            if (stream is null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            var size = sizeof(T);
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            var buffer = (stackalloc byte[size]);
#else
            var buffer = (new byte[size]);
#endif
            fixed (byte* pBuffer = buffer)
            {
                *(T*)pBuffer = value;
            }
            stream.Write(buffer);
        }
    }
}
