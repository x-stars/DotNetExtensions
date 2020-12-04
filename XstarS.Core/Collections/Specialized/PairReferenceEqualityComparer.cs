using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 用于比较两个 <see cref="KeyValuePair{TKey, TValue}"/> 包含的对象的引用是否相等的比较器。
    /// </summary>
    [Serializable]
    internal sealed class PairReferenceEqualityComparer : EqualityComparer<KeyValuePair<object, object>>
    {
        /// <summary>
        /// 初始化 <see cref="PairReferenceEqualityComparer"/> 类的新实例。
        /// </summary>
        public PairReferenceEqualityComparer() { }

        /// <summary>
        /// 返回一个默认的 <see cref="PairReferenceEqualityComparer"/> 实例。
        /// </summary>
        /// <returns>默认的 <see cref="PairReferenceEqualityComparer"/> 实例。</returns>
        public static new PairReferenceEqualityComparer Default { get; } =
            new PairReferenceEqualityComparer();

        /// <summary>
        /// 确定两个 <see cref="KeyValuePair{TKey, TValue}"/> 包含的对象的引用是否相等。
        /// </summary>
        /// <param name="x">要比较对象引用的第一个 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
        /// <param name="y">要比较对象引用的第二个 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的包含的对象的引用分别相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(KeyValuePair<object, object> x, KeyValuePair<object, object> y) =>
            object.ReferenceEquals(x.Key, y.Key) && object.ReferenceEquals(x.Value, y.Value);

        /// <summary>
        /// 获取指定 <see cref="KeyValuePair{TKey, TValue}"/> 包含的对象基于引用的哈希代码。
        /// </summary>
        /// <param name="obj">要获取包含的对象的哈希代码的 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
        /// <returns><paramref name="obj"/> 包含的对象基于引用的哈希代码。</returns>
        public override int GetHashCode(KeyValuePair<object, object> obj) =>
            RuntimeHelpers.GetHashCode(obj.Key) * -1521134295 + RuntimeHelpers.GetHashCode(obj.Value);
    }
}
