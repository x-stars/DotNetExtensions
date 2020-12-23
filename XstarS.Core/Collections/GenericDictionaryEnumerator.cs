using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供非泛型键值对集合的枚举数的泛型包装。
    /// </summary>
    /// <typeparam name="TKey">要枚举的键的类型。</typeparam>
    /// <typeparam name="TValue">要枚举的值的类型。</typeparam>
    [Serializable]
    public sealed class GenericDictionaryEnumerator<TKey, TValue>
        : IEnumerator, IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 表示当前实例包装的字典的枚举数。
        /// </summary>
        private readonly IDictionaryEnumerator Enumerator;

        /// <summary>
        /// 将 <see cref="GenericDictionaryEnumerator{TKey, TValue}"/>
        /// 类的新实例初始化为指定字典的枚举数的包装。
        /// </summary>
        /// <param name="enumerator">要包装的字典的枚举数。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
        public GenericDictionaryEnumerator(IDictionaryEnumerator enumerator)
        {
            this.Enumerator = enumerator ??
                throw new ArgumentNullException(nameof(enumerator));
        }

        /// <inheritdoc/>
        public KeyValuePair<TKey, TValue> Current =>
            new KeyValuePair<TKey, TValue>(
                (TKey)this.Enumerator.Key, (TValue)this.Enumerator.Value);

        /// <inheritdoc/>
        object IEnumerator.Current => this.Enumerator.Current;

        /// <inheritdoc/>
        object IDictionaryEnumerator.Key => this.Enumerator.Key;

        /// <inheritdoc/>
        object IDictionaryEnumerator.Value => this.Enumerator.Value;

        /// <inheritdoc/>
        DictionaryEntry IDictionaryEnumerator.Entry => this.Enumerator.Entry;

        /// <inheritdoc/>
        public void Dispose() => (this.Enumerator as IDisposable)?.Dispose();

        /// <inheritdoc/>
        public bool MoveNext() => this.Enumerator.MoveNext();

        /// <inheritdoc/>
        public void Reset() => this.Enumerator.Reset();
    }
}
