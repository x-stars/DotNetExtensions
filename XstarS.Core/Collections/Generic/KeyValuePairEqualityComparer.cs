using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供 <see cref="KeyValuePair{TKey, TValue}"/> 的键值的相等比较的方法。
    /// </summary>
    /// <typeparam name="TKey"><see cref="KeyValuePair{TKey, TValue}"/> 的键的类型。</typeparam>
    /// <typeparam name="TValue"><see cref="KeyValuePair{TKey, TValue}"/> 的值的类型。</typeparam>
    [Serializable]
    internal sealed class KeyValuePairEqualityComparer<TKey, TValue>
        : CollectionEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 表示用于比较 <see cref="KeyValuePair{TKey, TValue}"/> 的键的比较器。
        /// </summary>
        private readonly IEqualityComparer<TKey> KeyComparer;

        /// <summary>
        /// 表示用于比较 <see cref="KeyValuePair{TKey, TValue}"/> 的值的比较器。
        /// </summary>
        private readonly IEqualityComparer<TValue> ValueComparer;

        /// <summary>
        /// 初始化 <see cref="KeyValuePairEqualityComparer{TKey, TValue}"/> 类的新实例。
        /// </summary>
        public KeyValuePairEqualityComparer()
        {
            this.KeyComparer = CollectionEqualityComparer<TKey>.Default;
            this.ValueComparer = CollectionEqualityComparer<TValue>.Default;
        }

        /// <summary>
        /// 确定两个指定的 <see cref="KeyValuePair{TKey, TValue}"/> 的键值是否均相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="KeyValuePair{TKey, TValue}"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="KeyValuePair{TKey, TValue}"/> 对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 的键值均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y)
        {
            return this.KeyComparer.Equals(x.Key, y.Key) &&
                this.ValueComparer.Equals(x.Value, y.Value);
        }

        /// <summary>
        /// 获取指定的 <see cref="KeyValuePair{TKey, TValue}"/> 的键值的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的 <see cref="KeyValuePair{TKey, TValue}"/> 对象。</param>
        /// <returns>根据 <paramref name="obj"/> 的键值得到的哈希代码。</returns>
        public override int GetHashCode(KeyValuePair<TKey, TValue> obj)
        {
            return this.CombineHashCode(
                this.KeyComparer.GetHashCode(obj.Key),
                this.ValueComparer.GetHashCode(obj.Value));
        }
    }
}
