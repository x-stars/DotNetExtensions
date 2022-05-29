using System;
using System.Collections;

namespace XNetEx.Collections
{
    /// <summary>
    /// 提供 <see cref="IEnumerator"/> 的 <see cref="IEnumerable"/> 包装。
    /// </summary>
    internal sealed class EnumeratorEnumerable : IEnumerable
    {
        /// <summary>
        /// 当前实例包装的 <see cref="IEnumerator"/> 对象。
        /// </summary>
        private readonly IEnumerator Enumerator;

        /// <summary>
        /// 以要包装为 <see cref="IEnumerable"/> 的 <see cref="IEnumerator"/>
        /// 初始化 <see cref="EnumeratorEnumerable"/> 类的新实例。
        /// </summary>
        /// <param name="enumerator">要包装的 <see cref="IEnumerator"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
        internal EnumeratorEnumerable(IEnumerator enumerator)
        {
            this.Enumerator = enumerator ??
                throw new ArgumentNullException(nameof(enumerator));
        }

        /// <inheritdoc/>
        public IEnumerator GetEnumerator() => this.Enumerator;
    }
}
