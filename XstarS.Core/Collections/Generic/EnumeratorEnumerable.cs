using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供 <see cref="IEnumerator{T}"/> 的 <see cref="IEnumerable{T}"/> 包装。
    /// </summary>
    /// <typeparam name="T">要枚举的对象的类型。</typeparam>
    internal sealed class EnumeratorEnumerable<T> : IEnumerable<T>
    {
        /// <summary>
        /// 表示当前包装的 <see cref="IEnumerable{T}"/> 对象。
        /// </summary>
        private readonly IEnumerator<T> Enumerator;

        /// <summary>
        /// 以要包装为 <see cref="IEnumerable{T}"/> 的 <see cref="IEnumerator{T}"/>
        /// 初始化 <see cref="EnumeratorEnumerable{T}"/> 类的新实例。
        /// </summary>
        /// <param name="enumerator">要包装的 <see cref="IEnumerator{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
        internal EnumeratorEnumerable(IEnumerator<T> enumerator)
        {
            this.Enumerator = enumerator ??
                throw new ArgumentNullException(nameof(enumerator));
        }

        /// <inheritdoc/>
        public IEnumerator<T> GetEnumerator() => this.Enumerator;

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.Enumerator;
    }
}
