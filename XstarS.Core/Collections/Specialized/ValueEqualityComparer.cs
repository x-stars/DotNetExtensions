using System;
using System.Collections.Generic;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 用于比较对象的值是否相等的比较器。
    /// </summary>
    /// <remarks>基于反射调用，可能存在性能问题。</remarks>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public sealed class ValueEqualityComparer<T> : EqualityComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="ValueEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        private ValueEqualityComparer() : base() { }

        /// <summary>
        /// 获取默认的 <see cref="ValueEqualityComparer{T}"/> 实例。
        /// </summary>
        public static new ValueEqualityComparer<T> Default { get; } = new ValueEqualityComparer<T>();

        /// <summary>
        /// 确定两对象的值是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T x, T y) => ObjectExtensions.ValueEquals(x, y);

        /// <summary>
        /// 获取指定对象基于值的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 基于值的哈希代码。</returns>
        public override int GetHashCode(T obj) => ObjectExtensions.GetValueHashCode(obj);
    }
}
