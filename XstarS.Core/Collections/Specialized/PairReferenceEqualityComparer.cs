﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS.Collections.Specialized
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 用于比较两个 <see cref="ObjectPair"/> 包含的对象的引用是否相等的比较器。
    /// </summary>
    [Serializable]
    public sealed class PairReferenceEqualityComparer : EqualityComparer<ObjectPair>
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
        /// 确定两个 <see cref="ObjectPair"/> 包含的对象的引用是否相等。
        /// </summary>
        /// <param name="x">要比较对象引用的第一个 <see cref="ObjectPair"/>。</param>
        /// <param name="y">要比较对象引用的第二个 <see cref="ObjectPair"/>。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的包含的对象的引用分别相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(ObjectPair x, ObjectPair y) =>
            object.ReferenceEquals(x.Key, y.Key) &&
            object.ReferenceEquals(x.Value, y.Value);

        /// <summary>
        /// 获取指定 <see cref="ObjectPair"/> 包含的对象基于引用的哈希代码。
        /// </summary>
        /// <param name="obj">要获取包含的对象的哈希代码的 <see cref="ObjectPair"/>。</param>
        /// <returns><paramref name="obj"/> 包含的对象基于引用的哈希代码。</returns>
        public override int GetHashCode(ObjectPair obj) =>
            RuntimeHelpers.GetHashCode(obj.Key) * -1521134295 +
            RuntimeHelpers.GetHashCode(obj.Value);
    }
}
