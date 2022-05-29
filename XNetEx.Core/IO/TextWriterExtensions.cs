using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XstarS.IO
{
    /// <summary>
    /// 提供文本写入器 <see cref="TextWriter"/> 的扩展方法。
    /// </summary>
    public static class TextWriterExtensions
    {
        /// <summary>
        /// 表示线程安全（同步）的 <see cref="TextWriter"/> 的类型。
        /// </summary>
        private static readonly Type SyncReaderType =
            TextWriter.Synchronized(TextWriter.Null).GetType();

        /// <summary>
        /// 确认当前文本读取器是否为线程安全（同步）的包装 <see cref="TextWriter"/>。
        /// </summary>
        /// <param name="writer">要确定是否线程安全的文本写入器。</param>
        /// <returns>若 <paramref name="writer"/> 为线程安全（同步）的包装，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="writer"/> 为 <see langword="null"/>。</exception>
        internal static bool IsSynchronized(this TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }

            return writer.GetType() == TextWriterExtensions.SyncReaderType;
        }

        /// <summary>
        /// 将多个字符串值按行分隔并后跟行结束符写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLines(this TextWriter writer, params string?[] values)
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
        /// 将多个对象的文本表示形式按行分隔并后跟行结束符写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLines(this TextWriter writer, params object?[] values)
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
        /// 将多个字符串值逐个后跟行结束符写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLines(this TextWriter writer, IEnumerable<string?> values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                writer.WriteLine(value);
            }
        }

        /// <summary>
        /// 将多个对象的文本表示形式逐个后跟行结束符写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteLines(this TextWriter writer, IEnumerable<object?> values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                writer.WriteLine(value?.ToString());
            }
        }

        /// <summary>
        /// 将多个字符串值逐个后跟行结束符异步写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static async Task WriteLinesAsync(
            this TextWriter writer, IEnumerable<string?> values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                await writer.WriteLineAsync(value);
            }
        }

        /// <summary>
        /// 将多个对象的文本表示形式逐个后跟行结束符异步写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static async Task WriteLinesAsync(
            this TextWriter writer, IEnumerable<object?> values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            foreach (var value in values)
            {
                await writer.WriteLineAsync(value?.ToString());
            }
        }

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// 将多个字符串值逐个后跟行结束符异步写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static async ValueTask WriteLinesAsync(
            this TextWriter writer, IAsyncEnumerable<string?> values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            await foreach (var value in values)
            {
                await writer.WriteLineAsync(value);
            }
        }

        /// <summary>
        /// 将多个对象的文本表示形式逐个后跟行结束符异步写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static async ValueTask WriteLinesAsync(
            this TextWriter writer, IAsyncEnumerable<object?> values)
        {
            if (writer is null)
            {
                throw new ArgumentNullException(nameof(writer));
            }
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            await foreach (var value in values)
            {
                await writer.WriteLineAsync(value?.ToString());
            }
        }
#endif

        /// <summary>
        /// 将多个字符串值按空格分隔并后跟行结束符写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个字符串值。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteTokensInLine(this TextWriter writer, params string?[] values)
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
        /// 将多个对象的文本表示形式按空格分隔并后跟行结束符写入文本字符串或流。
        /// </summary>
        /// <param name="writer">要进行写入的文本写入器。</param>
        /// <param name="values">要写入的多个对象。</param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/>
        /// 或 <paramref name="values"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ObjectDisposedException">
        /// <see cref="TextWriter"/> 已关闭。</exception>
        /// <exception cref="IOException">出现 I/O 错误。</exception>
        public static void WriteTokensInLine(this TextWriter writer, params object?[] values)
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
    }
}
