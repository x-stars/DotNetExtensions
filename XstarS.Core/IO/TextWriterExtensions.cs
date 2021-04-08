using System;
using System.IO;

namespace XstarS.IO
{
    /// <summary>
    /// 提供文本写入器 <see cref="TextWriter"/> 的扩展方法。
    /// </summary>
    public static class TextWriterExtensions
    {
        /// <summary>
        /// 将多个字符串值按空格分隔后写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteTokens(this TextWriter writer, params string[] values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            writer.WriteLine(string.Join(" ", values));
        }

        /// <summary>
        /// 将多个对象的文本表示形式按空格分隔后写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteTokens(this TextWriter writer, params object[] values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            writer.WriteLine(string.Join(" ", values));
        }

        /// <summary>
        /// 将多个字符串值按行分隔后写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLines(this TextWriter writer, params string[] values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            writer.WriteLine(string.Join(writer.NewLine, values));
        }

        /// <summary>
        /// 将多个对象的文本表示形式按行分隔后写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLines(this TextWriter writer, params object[] values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            writer.WriteLine(string.Join(writer.NewLine, values));
        }
    }
}
