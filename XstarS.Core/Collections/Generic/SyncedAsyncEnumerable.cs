#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供 <see cref="IAsyncEnumerable{T}"/> 的 <see cref="IEnumerable{T}"/> 包装。
    /// </summary>
    /// <typeparam name="T">要枚举的对象的类型。</typeparam>
    internal sealed class SyncedAsyncEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// 表示当前包装的 <see cref="IAsyncEnumerable{T}"/> 对象。
        /// </summary>
        private readonly IAsyncEnumerable<T> AsyncEnumerable;

        /// <summary>
        /// 以要包装为 <see cref="IEnumerable{T}"/> 的 <see cref="IAsyncEnumerable{T}"/>
        /// 初始化 <see cref="SyncedAsyncEnumerable{T}"/> 类的新实例。
        /// </summary>
        /// <param name="asyncEnumerable">要包装的 <see cref="IAsyncEnumerable{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asyncEnumerable"/> 为 <see langword="null"/>。</exception>
        public SyncedAsyncEnumerable(IAsyncEnumerable<T> asyncEnumerable)
        {
            this.AsyncEnumerable = asyncEnumerable ??
                throw new ArgumentNullException(nameof(asyncEnumerable));
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator()
        {
            var asyncEnumerator = this.AsyncEnumerable.GetAsyncEnumerator();
            return new SyncedAsyncEnumerator<T>(asyncEnumerator);
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    /// <summary>
    /// 提供 <see cref="IAsyncEnumerator{T}"/> 的 <see cref="IEnumerator{T}"/> 包装。
    /// </summary>
    /// <typeparam name="T">要枚举的对象的类型。</typeparam>
    internal sealed class SyncedAsyncEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// 表示当前包装的 <see cref="IAsyncEnumerator{T}"/> 对象。
        /// </summary>
        private readonly IAsyncEnumerator<T> AsyncEnumerator;

        /// <summary>
        /// 以要包装为 <see cref="IEnumerator{T}"/> 的 <see cref="IAsyncEnumerator{T}"/>
        /// 初始化 <see cref="SyncedAsyncEnumerator{T}"/> 类的新实例。
        /// </summary>
        /// <param name="asyncEnumerator">要包装的 <see cref="IAsyncEnumerator{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="asyncEnumerator"/> 为 <see langword="null"/>。</exception>
        public SyncedAsyncEnumerator(IAsyncEnumerator<T> asyncEnumerator)
        {
            this.AsyncEnumerator = asyncEnumerator ??
                throw new ArgumentNullException(nameof(asyncEnumerator));
        }

        /// <inheritdoc/>
        public T Current => this.AsyncEnumerator.Current;

        /// <inheritdoc/>
        object IEnumerator.Current => this.Current!;

        /// <inheritdoc/>
        public void Dispose()
        {
            var task = this.AsyncEnumerator.DisposeAsync().AsTask();
            task.GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            var task = this.AsyncEnumerator.MoveNextAsync().AsTask();
            return task.GetAwaiter().GetResult();
        }

        /// <inheritdoc/>
        public void Reset() => throw new NotSupportedException();
    }
}
#endif
