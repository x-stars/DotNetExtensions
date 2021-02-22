using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XstarS.Collections.Generic;

namespace XstarS.Collections.Specialized
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 提供用于比较 <see cref="ObjectPair"/> 包含的对象的引用是否相等的方法。
    /// </summary>
    [Serializable]
    internal sealed class PairReferenceEqualityComparer : SimpleEqualityComparer<ObjectPair>
    {
        /// <summary>
        /// 初始化 <see cref="PairReferenceEqualityComparer"/> 类的新实例。
        /// </summary>
        public PairReferenceEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="PairReferenceEqualityComparer"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="PairReferenceEqualityComparer"/> 类的默认实例。</returns>
        public static new PairReferenceEqualityComparer Default { get; } =
            new PairReferenceEqualityComparer();

        /// <summary>
        /// 确定两个 <see cref="ObjectPair"/> 包含的对象的引用是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="ObjectPair"/>。</param>
        /// <param name="y">要比较的第二个 <see cref="ObjectPair"/>。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的包含的对象的引用相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(ObjectPair x, ObjectPair y) =>
            object.ReferenceEquals(x.Key, y.Key) &&
            object.ReferenceEquals(x.Value, y.Value);

        /// <summary>
        /// 获取指定 <see cref="ObjectPair"/> 包含的对象基于引用的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的 <see cref="ObjectPair"/>。</param>
        /// <returns><paramref name="obj"/> 包含的对象基于引用的哈希代码。</returns>
        public override int GetHashCode(ObjectPair obj) =>
            RuntimeHelpers.GetHashCode(obj.Key) * -1521134295 +
            RuntimeHelpers.GetHashCode(obj.Value);
    }
}
