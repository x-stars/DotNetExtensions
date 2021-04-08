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
        /// 读取当前文本读取器的下一个字符串值。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器的下一个字符串值；
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
        public static string ReadToken(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            lock (reader)
            {
                var iChar = -1;
                while ((iChar = reader.Read()) != -1)
                {
                    if (!char.IsWhiteSpace((char)iChar)) { break; }
                }
                var token = new mstring();
                token.Append((char)iChar);
                while ((iChar = reader.Read()) != -1)
                {
                    if (char.IsWhiteSpace((char)iChar)) { break; }
                    token.Append((char)iChar);
                }
                return (token.Length == 0) ? null : token.ToString();
            }
        }

        /// <summary>
        /// 读取当前文本读取器的下一个字符串值，并将其转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器的下一个字符串值的数值形式。</returns>
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
        public static T ReadTokenAs<T>(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return (reader.ReadToken() ?? string.Empty).ParseAs<T>();
        }

        /// <summary>
        /// 读取当前文本读取器到下一行的字符串，并将其转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器的下一行字符串的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一行中的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static T ReadLineAs<T>(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return (reader.ReadLine() ?? string.Empty).ParseAs<T>();
        }

        /// <summary>
        /// 读取当前文本读取器到下一行的所有字符串值。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器的下一行包含的所有字符串值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一行中的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static string[] ReadLineTokens(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return (reader.ReadLine() ?? string.Empty).SplitToTokens();
        }

        /// <summary>
        /// 读取当前文本读取器到下一行的字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器的下一行包含的所有字符串值的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 下一行中的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="OverflowException">
        /// 读取到的字符串表示的值超出了 <typeparamref name="T"/> 能表示的范围。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static T[] ReadLineTokensAs<T>(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return Array.ConvertAll(reader.ReadLineTokens(), ValueParser.ParseAs<T>);
        }

        /// <summary>
        /// 读取当前文本读取器到末尾的字符串，并将其转换为指定的数值形式。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>文本读取器到末尾的字符串的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="ObjectDisposedException">当前文本读取器已关闭。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static T ReadToEndAs<T>(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return (reader.ReadToEnd() ?? string.Empty).ParseAs<T>();
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

            return (reader.ReadToEnd() ?? string.Empty).SplitToLines();
        }

        /// <summary>
        /// 读取当前文本读取器到末尾的所有字符，并将其包含的所有字符串行转换为指定的数值形式。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>文本读取器到末尾的所有字符串行的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="ObjectDisposedException">当前文本读取器已关闭。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static T[] ReadLinesToEndAs<T>(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return Array.ConvertAll(reader.ReadLinesToEnd(), ValueParser.ParseAs<T>);
        }

        /// <summary>
        /// 读取当前文本读取器到末尾的所有字符串值。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <returns>文本读取器到末尾的所有字符串值。</returns>
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
        public static string[] ReadTokensToEnd(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return (reader.ReadToEnd() ?? string.Empty).SplitToTokens();
        }

        /// <summary>
        /// 读取当前文本读取器到末尾的所有字符，并将其包含的所有字符串值转换为指定的数值形式。
        /// </summary>
        /// <param name="reader">要进行读取的文本读取器。</param>
        /// <typeparam name="T">数值形式的类型。</typeparam>
        /// <returns>文本读取器到末尾的所有字符串值的数值形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// 到末尾的字符的字符数大于 <see cref="int.MaxValue"/>。</exception>
        /// <exception cref="ArgumentException">读取到的字符串不表示有效的值。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextReader"/> 已关闭。</exception>
        /// <exception cref="OutOfMemoryException">
        /// 没有足够的内存来为下一行的字符串分配缓冲区。</exception>
        /// <exception cref="ObjectDisposedException">当前文本读取器已关闭。</exception>
        /// <exception cref="FormatException">读取到的字符串的格式不正确。</exception>
        /// <exception cref="InvalidCastException">指定的从字符串的转换无效。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static T[] ReadTokensToEndAs<T>(this TextReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException(nameof(reader));
            }

            return Array.ConvertAll(reader.ReadTokensToEnd(), ValueParser.ParseAs<T>);
        }
    }
}
