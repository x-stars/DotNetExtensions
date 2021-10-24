using System;
using System.Diagnostics.CodeAnalysis;
using XstarS.Collections.Generic;
using XstarS.Runtime;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 提供用于比较对象的直接值是否相等的方法。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public sealed class DirectValueEqualityComaprer<T> : SimpleEqualityComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="DirectValueEqualityComaprer{T}"/> 类的新实例。
        /// </summary>
        public DirectValueEqualityComaprer() { }

        /// <summary>
        /// 获取 <see cref="DirectValueEqualityComaprer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="DirectValueEqualityComaprer{T}"/> 类的默认实例。</returns>
        public static new DirectValueEqualityComaprer<T> Default { get; } = new DirectValueEqualityComaprer<T>();

        /// <summary>
        /// 确定两对象的直接值是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的直接值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals([AllowNull] T x, [AllowNull] T y) => ObjectDirectValue.Equals(x, y);

        /// <summary>
        /// 获取指定对象基于直接值的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 基于直接值的哈希代码。</returns>
        public override int GetHashCode([AllowNull] T obj) => ObjectDirectValue.GetHashCode(obj);
    }
}
