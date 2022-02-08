using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Threading;
using System.Threading.Tasks;
#endif

namespace XstarS.IO
{
    /// <summary>
    /// 为 <see cref="TextReader"/> 提供按行迭代的公开枚举数。
    /// </summary>
    internal sealed class TextLineEnumerator : IEnumerable<string>, IEnumerator<string>
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        , IAsyncEnumerable<string>, IAsyncEnumerator<string>
#endif
    {
        /// <summary>
        /// 表示要按行进行迭代的文本读取器。
        /// </summary>
        private readonly TextReader Reader;

        /// <summary>
        /// 表示是否在迭代完成后释放输入的文本读取器。
        /// </summary>
        private readonly bool Disposing;

        /// <summary>
        /// 表示当前迭代读取到的文本行。
        /// </summary>
        private volatile string? CurrentLine;

        /// <summary>
        /// 以指定的文本读取器初始化 <see cref="TextLineEnumerator"/> 类的新实例，
        /// 并指定是否在迭代完成后释放输入的文本读取器。
        /// </summary>
        /// <param name="reader">要按行进行迭代的文本读取器。</param>
        /// <param name="disposing">指定是否在迭代完成后释放输入的文本读取器。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="reader"/> 为 <see langword="null"/>。</exception>
        internal TextLineEnumerator(TextReader reader, bool disposing = false)
        {
            this.Reader = reader ??
                throw new ArgumentNullException(nameof(reader));
            this.Disposing = disposing;
        }

        /// <inheritdoc/>
        public string Current => this.CurrentLine!;

        /// <inheritdoc/>
        object IEnumerator.Current => this.CurrentLine!;

        /// <inheritdoc/>
        public void Dispose() { if (this.Disposing) { this.Reader.Dispose(); } }

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public ValueTask DisposeAsync()
        {
            try { this.Dispose(); return default(ValueTask); }
            catch (Exception e) { return new ValueTask(Task.FromException(e)); }
        }
#endif

        /// <inheritdoc/>
        public IEnumerator<string> GetEnumerator() => this;

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this;

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public IAsyncEnumerator<string> GetAsyncEnumerator(
            CancellationToken cancellationToken = default) => this;
#endif

        /// <inheritdoc/>
        public bool MoveNext() => (this.CurrentLine = this.Reader.ReadLine()) != null;

#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        /// <inheritdoc/>
        public async ValueTask<bool> MoveNextAsync() =>
            (this.CurrentLine = await this.Reader.ReadLineAsync()) != null;
#endif

        /// <summary>
        /// 不支持此操作，永远抛出 <see cref="NotSupportedException"/> 异常。
        /// </summary>
        /// <exception cref="NotSupportedException">永远抛出此异常。</exception>
        void IEnumerator.Reset() => throw new NotSupportedException();
    }
}
