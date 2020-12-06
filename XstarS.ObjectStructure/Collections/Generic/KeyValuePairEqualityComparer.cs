using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 提供 <see cref="KeyValuePair{TKey, TValue}"/> 中的键值的相等比较的方法。
    /// </summary>
    /// <typeparam name="TKey"><see cref="KeyValuePair{TKey, TValue}"/> 的键的类型。</typeparam>
    /// <typeparam name="TValue"><see cref="KeyValuePair{TKey, TValue}"/> 的值的类型。</typeparam>
    [Serializable]
    internal sealed class KeyValuePairEqualityComparer<TKey, TValue>
        : StructureEqualityComparer<KeyValuePair<TKey, TValue>>
    {
        /// <summary>
        /// 初始化 <see cref="KeyValuePairEqualityComparer{TKey, TValue}"/> 类的新实例。
        /// </summary>
        public KeyValuePairEqualityComparer() { }

        /// <summary>
        /// 确定两个指定的 <see cref="KeyValuePair{TKey, TValue}"/> 中的键值是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="KeyValuePair{TKey, TValue}"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="KeyValuePair{TKey, TValue}"/> 对象。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的键值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected override bool EqualsCore(
            KeyValuePair<TKey, TValue> x, KeyValuePair<TKey, TValue> y, ISet<ObjectPair> compared)
        {
            if (x.Key?.GetType() != y.Key?.GetType()) { return false; }
            if (x.Value?.GetType() != y.Value?.GetType()) { return false; }

            var keyComparer = StructureEqualityComparer.OfType(x.Key?.GetType());
            var valueComparer = StructureEqualityComparer.OfType(x.Value?.GetType());
            return keyComparer.Equals(x.Key, y.Key, compared) &&
                valueComparer.Equals(x.Value, y.Value, compared);
        }

        /// <summary>
        /// 获取指定的 <see cref="KeyValuePair{TKey, TValue}"/> 中的键值的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的 <see cref="KeyValuePair{TKey, TValue}"/> 对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns>根据 <paramref name="obj"/> 中的键值得到的哈希代码。</returns>
        protected override int GetHashCodeCore(
            KeyValuePair<TKey, TValue> obj, ISet<object> computed)
        {
            var keyComparer = StructureEqualityComparer.OfType(obj.Key?.GetType());
            var valueComparer = StructureEqualityComparer.OfType(obj.Value?.GetType());
            return this.CombineHashCode(
                keyComparer.GetHashCode(obj, computed),
                valueComparer.GetHashCode(obj, computed));
        }
    }
}
