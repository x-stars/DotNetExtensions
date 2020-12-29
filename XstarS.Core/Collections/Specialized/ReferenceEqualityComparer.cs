using System;
using System.Runtime.CompilerServices;
using XstarS.Collections.Generic;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 提供用于比较对象的引用是否相等的方法。
    /// </summary>
    [Serializable]
    public sealed class ReferenceEqualityComparer : SimpleEqualityComparer<object>
    {
        /// <summary>
        /// 初始化 <see cref="ReferenceEqualityComparer"/> 类的新实例。
        /// </summary>
        public ReferenceEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="ReferenceEqualityComparer"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="ReferenceEqualityComparer"/> 类的默认实例。</returns>
        public static new ReferenceEqualityComparer Default { get; } = new ReferenceEqualityComparer();

        /// <summary>
        /// 确定两对象的引用是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的引用相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object x, object y) => object.ReferenceEquals(x, y);

        /// <summary>
        /// 获取指定对象基于引用的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 基于引用的哈希代码。</returns>
        public override int GetHashCode(object obj) => RuntimeHelpers.GetHashCode(obj);
    }
}
