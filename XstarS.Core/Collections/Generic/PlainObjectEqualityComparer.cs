using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供非结构化对象的默认的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">非结构化对象的类型。</typeparam>
    [Serializable]
    internal sealed class PlainObjectEqualityComparer<T> : StructuralEqualityComparer<T>
    {
        /// <summary>
        /// 表示用于比较非结构化对象的 <see cref="IEqualityComparer{T}"/>。
        /// </summary>
        private readonly IEqualityComparer<T> InternalComparer;

        /// <summary>
        /// 初始化 <see cref="PlainObjectEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public PlainObjectEqualityComparer()
        {
            this.InternalComparer = EqualityComparer<T>.Default;
        }

        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T x, T y) => this.InternalComparer.Equals(x, y);

        /// <summary>
        /// 获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        public override int GetHashCode(T obj) => this.InternalComparer.GetHashCode(obj);
    }
}
