using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供对象的默认的无环相等比较的方法。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    internal sealed class DefaultAcyclicEqualityComparer<T> : AcyclicEqualityComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="DefaultAcyclicEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public DefaultAcyclicEqualityComparer() { }

        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <param name="compared">已经访问过的对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected override bool EqualsCore(T x, T y, ISet<KeyValuePair<object, object>> compared)
        {
            return EqualityComparer<T>.Default.Equals(x, y);
        }

        /// <summary>
        /// 获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <param name="computed">已经访问过的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        protected override int GetHashCodeCore(T obj, ISet<object> computed)
        {
            return EqualityComparer<T>.Default.GetHashCode(obj);
        }
    }
}
