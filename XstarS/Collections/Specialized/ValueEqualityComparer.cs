using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 用于比较对象的值是否相等的比较器。
    /// </summary>
    /// <remarks>基于反射调用，可能存在性能问题。</remarks>
    [Serializable]
    public sealed class ValueEqualityComparer : EqualityComparer<object>
    {
        /// <summary>
        /// 初始化 <see cref="ValueEqualityComparer"/> 类的新实例。
        /// </summary>
        private ValueEqualityComparer() : base() { }

        /// <summary>
        /// 返回一个默认的 <see cref="ValueEqualityComparer"/> 实例。
        /// </summary>
        public static new ValueEqualityComparer Default { get; } = new ValueEqualityComparer();

        /// <summary>
        /// 确定两对象的值是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object x, object y) => new ValueEquatablePair(x, y).ValueEquals();

        /// <summary>
        /// 获取指定对象基于值的哈希函数。
        /// </summary>
        /// <param name="obj">要获取哈希函数的对象。</param>
        /// <returns><paramref name="obj"/> 基于值的哈希函数。</returns>
        public override int GetHashCode(object obj) => new ValueHashCodeObject(obj).GetValueHashCode();
    }
}
