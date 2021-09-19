using System;
using XstarS.Collections.Generic;
using XstarS.Runtime;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 提供用于比较非托管类型的值是否二进制相等的方法。
    /// </summary>
    /// <typeparam name="T">要比较的非托管值的类型。</typeparam>
    [Serializable]
    public sealed class BinaryEqualityComparer<T> : SimpleEqualityComparer<T> where T : unmanaged
    {
        /// <summary>
        /// 初始化 <see cref="BinaryEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public BinaryEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="BinaryEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="BinaryEqualityComparer{T}"/> 类的默认实例。</returns>
        public static new BinaryEqualityComparer<T> Default { get; } = new BinaryEqualityComparer<T>();

        /// <summary>
        /// 确定两个非托管类型的值是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个非托管类型的值。</param>
        /// <param name="y">要比较的第二个非托管类型的值。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T x, T y) => UnmanagedType.BinaryEquals(x, y);

        /// <summary>
        /// 获取指定非托管类型的值基于二进制的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的非托管类型的值。</param>
        /// <returns><paramref name="obj"/> 基于二进制的哈希代码。</returns>
        public override int GetHashCode(T obj) => UnmanagedType.GetBinaryHashCode(obj);
    }
}
