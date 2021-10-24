using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供非泛型集合的枚举数的泛型包装。
    /// </summary>
    /// <typeparam name="T">要枚举的对象的类型。</typeparam>
    [Serializable]
    public sealed class GenericEnumerator<T> : IEnumerator, IEnumerator<T?>
    {
        /// <summary>
        /// 表示当前实例包装的枚举数。
        /// </summary>
        private readonly IEnumerator Enumerator;

        /// <summary>
        /// 将 <see cref="GenericEnumerator{T}"/> 类的新实例初始化为指定枚举数的包装。
        /// </summary>
        /// <param name="enumerator">要包装的枚举数。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
        public GenericEnumerator(IEnumerator enumerator)
        {
            this.Enumerator = enumerator ??
                throw new ArgumentNullException(nameof(enumerator));
        }

        /// <inheritdoc/>
        public T? Current => (T?)this.Enumerator.Current;

        /// <inheritdoc/>
        object? IEnumerator.Current => this.Enumerator.Current;

        /// <inheritdoc/>
        public void Dispose() => (this.Enumerator as IDisposable)?.Dispose();

        /// <inheritdoc/>
        public bool MoveNext() => this.Enumerator.MoveNext();

        /// <inheritdoc/>
        public void Reset() => this.Enumerator.Reset();
    }
}
