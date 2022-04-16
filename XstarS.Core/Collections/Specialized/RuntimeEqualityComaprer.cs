using System;
using XstarS.Collections.Generic;
using XstarS.Runtime.CompilerServices;

namespace XstarS.Collections.Specialized
{
    /// <summary>
    /// 提供用于比较对象的计算堆栈值是否相等的方法。
    /// </summary>
    /// <remarks>对于引用类型，计算堆栈值为对象的引用；
    /// 对于值类型，计算堆栈值为对象所有字段的序列。</remarks>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public sealed class RuntimeEqualityComaprer<T> : SimpleEqualityComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="RuntimeEqualityComaprer{T}"/> 类的新实例。
        /// </summary>
        public RuntimeEqualityComaprer() { }

        /// <summary>
        /// 获取 <see cref="RuntimeEqualityComaprer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="RuntimeEqualityComaprer{T}"/> 类的默认实例。</returns>
        public static new RuntimeEqualityComaprer<T> Default { get; } = new RuntimeEqualityComaprer<T>();

        /// <summary>
        /// 确定两对象的计算堆栈值是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的计算堆栈值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T? x, T? y) => ObjectRuntimeValue.RuntimeEquals(x, y);

        /// <summary>
        /// 获取指定对象基于计算堆栈值的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 基于计算堆栈值的哈希代码。</returns>
        public override int GetHashCode(T? obj) => ObjectRuntimeValue.GetRuntimeHashCode(obj);
    }
}
