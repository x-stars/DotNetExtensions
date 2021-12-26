using System;
using System.IO;
using XstarS.Text;
using mstring = System.Text.StringBuilder;

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
        /// 读取当前文本读取器的下一个字符串值。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器的下一个字符串值。
        /// 如果当前没有更多的可用字符串值，则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一个字符串值的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一个字符串值分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static string? ReadToken(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            string? ReadCore(TextReader reader)
            {
                var iChar = -1;
                while ((iChar = reader.Read()) != -1)
                {
                    if (!char.IsWhiteSpace((char)iChar)) { break; }
                }
                var result = new mstring();
                result.Append((char)iChar);
                while ((iChar = reader.Read()) != -1)
                {
                    if (char.IsWhiteSpace((char)iChar)) { break; }
                    result.Append((char)iChar);
                }
                return (result.Length == 0) ? null : result.ToString();
            }

            if (reader.IsSynchronized())
            {
                lock (reader)
                {
                    return ReadCore(reader);
                }
            }
            else
            {
                return ReadCore(reader);
            }
        }

        /// <summary>
        /// 读取当前文本读取器的下一个字符串值，并将其转换为等效的对象形式。
        /// </summary>
        /// <typeparam name="T">要转换为的对象的类型。</typeparam>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <param name="endValue">当没有更多的可用字符串值时使用的替代值。</param>
        /// <returns>文本读取器的下一个字符串值的对象形式。
        /// 如果当前没有更多的可用字符串值，则为 <paramref name="endValue"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一个字符串值的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一个字符串值分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// 读取到的字符串表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static T? ReadTokenAs<T>(this TextReader reader, T? endValue = null)
            where T : class
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var text = reader.ReadToken();
            return (text is null) ? endValue : text.ParseAs<T>();
        }

        /// <summary>
        /// 读取当前文本读取器的下一个字符串值，并将其转换为等效的数值形式。
        /// </summary>
        /// <typeparam name="T">要转换为的数值的类型。</typeparam>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <param name="endValue">当没有更多的可用字符串值时使用的替代值。</param>
        /// <returns>文本读取器的下一个字符串值的对象形式。
        /// 如果当前没有更多的可用字符串值，则为 <paramref name="endValue"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一个字符串值的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一个字符串值分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// 读取到的字符串表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static T? ReadTokenAs<T>(this TextReader reader, T? endValue = null)
            where T : struct
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            var text = reader.ReadToken();
            return (text is null) ? endValue : text.ParseAs<T>();
        }

        /// <summary>
        /// 读取当前文本读取器到下一行的所有字符串值。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器的下一行包含的所有字符串值。
        /// 如果当前没有更多的可用字符串值，则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一行中的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static string[]? ReadLineTokens(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return reader.ReadLine()?.SplitTokens();
        }

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

            var text = reader.ReadToEnd();
            return string.IsNullOrEmpty(text) ?
                Array.Empty<string>() : text.SplitLines();
        }
    }
}
