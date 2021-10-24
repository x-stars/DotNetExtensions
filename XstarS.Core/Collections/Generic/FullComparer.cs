using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 为对象的相等和大小关系比较器 <see cref="IFullComparer{T}"/> 提供抽象基类。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public abstract class FullComparer<T> : IFullComparer, IFullComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="FullComparer{T}"/> 类的新实例。
        /// </summary>
        protected FullComparer() { }

        /// <summary>
        /// 获取 <see cref="FullComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="FullComparer{T}"/> 类的默认实例。</returns>
        public static FullComparer<T> Default { get; } = new DefaultFullComparer<T>();

        /// <summary>
        /// 比较两个对象并返回一个值，该值指示一个对象小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 小于 <paramref name="y"/>，则为负数；
        /// 若 <paramref name="x"/> 等于 <paramref name="y"/>，则为零；
        /// 若 <paramref name="x"/> 大于 <paramref name="y"/>，则为正数。</returns>
        public abstract int Compare([AllowNull] T x, [AllowNull] T y);

        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第一个对象。</param>
        /// <returns>如果指定的对象相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public abstract bool Equals([AllowNull] T x, [AllowNull] T y);

        /// <summary>
        /// 获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns>指定对象的哈希代码。</returns>
        public abstract int GetHashCode([AllowNull] T obj);

        /// <summary>
        /// 比较两个对象并返回一个值，该值指示一个对象小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 小于 <paramref name="y"/>，则为负数；
        /// 若 <paramref name="x"/> 等于 <paramref name="y"/>，则为零；
        /// 若 <paramref name="x"/> 大于 <paramref name="y"/>，则为正数。</returns>
        int IComparer.Compare(object? x, object? y) => this.Compare((T?)x, (T?)y);

        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第一个对象。</param>
        /// <returns>如果指定的对象相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        bool IEqualityComparer.Equals(object? x, object? y) => this.Equals((T?)x, (T?)y);

        /// <summary>
        /// 获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns>指定对象的哈希代码。</returns>
        int IEqualityComparer.GetHashCode(object? obj) => this.GetHashCode((T?)obj);
    }
}
