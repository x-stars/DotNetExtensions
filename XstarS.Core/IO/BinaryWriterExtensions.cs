using System;
using System.IO;

namespace XstarS.IO
{
    /// <summary>
    /// 提供二进制写入器 <see cref="BinaryWriter"/> 的扩展方法。
    /// </summary>
    public static class BinaryWriterExtensions
    {
        /// <summary>
        /// 将指定类型的非托管数据写入当前流，并将当前位置前移对应的字节数。
        /// </summary>
        /// <typeparam name="T">非托管数据的类型。</typeparam>
        /// <param name="writer">要写入数据的二进制写入器。</param>
        /// <param name="value">要写入基础流的非托管数据。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> 为 <see langword="null"/>。</exception>
        public static unsafe void Write<T>(this BinaryWriter writer, T value)
            where T : unmanaged
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
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
            writer.Write(buffer);
        }

        /// <summary>
        /// 将指定的的字符串后跟 <c>NUL</c> 字符根据所使用的编码写入当前流，并提升流的当前位置。
        /// </summary>
        /// <param name="writer">要写入字符串的二进制写入器。</param>
        /// <param name="value">要写入基础流的字符串。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> 为 <see langword="null"/>。</exception>
        public static void WriteStringEndNull(this BinaryWriter writer, string value)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var chars = new char[value.Length + 1];
            value.CopyTo(0, chars, 0, value.Length);
            writer.Write(chars);
        }
    }
}
