using System;
using System.IO;
using XNetEx.Text;

namespace XNetEx.IO
{
    /// <summary>
    /// 提供文本读取器 <see cref="TextReader"/> 数值解析相关的扩展方法。
    /// </summary>
    public static class TextReaderParseExtensions
    {
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
    }
}
