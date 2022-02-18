using System;
using System.Collections.Generic;
using System.IO;

namespace XstarS.IO
{
    /// <summary>
    /// 提供文本读取器 <see cref="TextReader"/> 的扩展方法。
    /// </summary>
    public static class TextReaderExtensions
    {
        /// <summary>
        /// 表示线程安全（同步）的 <see cref="TextReader"/> 的类型。
        /// </summary>
        private static readonly Type SyncReaderType =
            TextReader.Synchronized(TextReader.Null).GetType();

        /// <summary>
        /// 确认当前文本读取器是否为线程安全（同步）的包装 <see cref="TextReader"/>。
        /// </summary>
        /// <param name="reader">要确定是否线程安全的文本读取器。</param>
        /// <returns>若 <paramref name="reader"/> 为线程安全（同步）的包装，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        internal static bool IsSynchronized(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.GetType() == TextReaderExtensions.SyncReaderType;
        }

        /// <summary>
        /// 读取当前文本读取器中的每一行文本，并指定是否在迭代完成后释放当前文本读取器。
        /// </summary>
        /// <param name="reader">要按行读取的文本读取器。</param>
        /// <param name="disposing">是否在迭代完成后释放当前文本读取器。</param>
        /// <returns><paramref name="reader"/> 的按行读取的公开枚举数。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<string> ReadLines(
            this TextReader reader, bool disposing = false)
        {
            return new TextLineEnumerator(reader, disposing);
        }

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// 异步读取当前文本读取器中的每一行文本，并指定是否在迭代完成后释放当前文本读取器。
        /// </summary>
        /// <param name="reader">要按行异步读取的文本读取器。</param>
        /// <param name="disposing">是否在迭代完成后释放当前文本读取器。</param>
        /// <returns><paramref name="reader"/> 的按行读取的异步公开枚举数。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        public static IAsyncEnumerable<string> ReadLinesAsync(
            this TextReader reader, bool disposing = false)
        {
            return new TextLineEnumerator(reader, disposing);
        }
#endif

        /// <summary>
        /// 读取当前文本读取器到末尾的所有字符串行。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器到末尾的所有字符串行。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为到末尾的字符串分配缓冲区。</exception>
        /// <exception cref="ObjectDisposedException">当前文本读取器已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static string[] ReadLinesToEnd(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.ReadToEnd().SplitLines();
        }
    }
}
