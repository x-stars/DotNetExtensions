using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供对象相等比较和大小关系比较的默认方法。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    internal sealed class DefaultFullComparer<T> : SimpleFullComparer<T>
    {
        /// <summary>
        /// 表示用于比较对象大小关系的比较器。
        /// </summary>
        private readonly Comparer<T> Comparer;

        /// <summary>
        /// 表示用于比较对象是否相等的比较器。
        /// </summary>
        private readonly EqualityComparer<T> EqualityComparer;

        /// <summary>
        /// 初始化 <see cref="DefaultFullComparer{T}"/> 类的新实例。
        /// </summary>
        public DefaultFullComparer()
        {
            this.Comparer = Comparer<T>.Default;
            this.EqualityComparer = EqualityComparer<T>.Default;
        }

        /// <summary>
        /// 比较两个对象并返回一个值，该值指示一个对象小于、等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 小于 <paramref name="y"/>，则为负数；
        /// 若 <paramref name="x"/> 等于 <paramref name="y"/>，则为零；
        /// 若 <paramref name="x"/> 大于 <paramref name="y"/>，则为正数。</returns>
        public override int Compare([AllowNull] T x, [AllowNull] T y) =>
            this.Comparer.Compare(x!, y!);

        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第一个对象。</param>
        /// <returns>如果指定的对象相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals([AllowNull] T x, [AllowNull] T y) =>
            this.EqualityComparer.Equals(x!, y!);

        /// <summary>
        /// 获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns>指定对象的哈希代码。</returns>
        public override int GetHashCode([AllowNull] T obj) =>
            this.EqualityComparer.GetHashCode(obj!);
    }
}
