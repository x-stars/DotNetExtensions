using System;
using XstarS.Collections.Generic;
using XstarS.Runtime;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 提供用于比较非托管类型的数组是否二进制相等的方法。
    /// </summary>
    /// <typeparam name="T">要比较的非托管类型的数组中的元素的类型。</typeparam>
    [Serializable]
    public sealed class ArrayBinaryEqualityComparer<T> : SimpleEqualityComparer<T[]> where T : unmanaged
    {
        /// <summary>
        /// 初始化 <see cref="ArrayBinaryEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public ArrayBinaryEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="ArrayBinaryEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="ArrayBinaryEqualityComparer{T}"/> 类的默认实例。</returns>
        public static new ArrayBinaryEqualityComparer<T> Default { get; } = new ArrayBinaryEqualityComparer<T>();

        /// <summary>
        /// 确定两个非托管类型的数组是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个非托管类型的数组。</param>
        /// <param name="y">要比较的第二个非托管类型的数组。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 二进制相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T[]? x, T[]? y) => UnmanagedTypeArray.BinaryEquals(x, y);

        /// <summary>
        /// 获取指定非托管类型的数组基于二进制值的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的非托管类型的数组。</param>
        /// <returns><paramref name="obj"/> 基于二进制值的哈希代码。</returns>
        public override int GetHashCode(T[]? obj) => UnmanagedTypeArray.GetBinaryHashCode(obj);
    }
}
