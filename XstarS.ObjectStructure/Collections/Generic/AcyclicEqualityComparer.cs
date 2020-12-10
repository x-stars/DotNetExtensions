using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XstarS.Collections.Specialized;

namespace XstarS.Collections.Generic
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 为对象的无环相等比较器 <see cref="IAcyclicEqualityComparer{T}"/> 提供抽象基类。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public abstract class AcyclicEqualityComparer<T>
        : EqualityComparer<T>, IAcyclicEqualityComparer, IAcyclicEqualityComparer<T>
    {
        /// <summary>
        /// 初始化 <see cref="AcyclicEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        protected AcyclicEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="AcyclicEqualityComparer{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="AcyclicEqualityComparer{T}"/> 类的默认实例。</returns>
        public static new AcyclicEqualityComparer<T> Default { get; } =
            new DefaultAcyclicEqualityComparer<T>();

        /// <summary>
        /// 确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public sealed override bool Equals(T x, T y)
        {
            var comparer = PairReferenceEqualityComparer.Default;
            var compared = new HashSet<ObjectPair>(comparer);
            return this.Equals(x, y, compared);
        }

        /// <summary>
        /// 获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        public sealed override int GetHashCode(T obj)
        {
            var comparer = ReferenceEqualityComparer.Default;
            var computed = new HashSet<object>(comparer);
            return this.GetHashCode(obj, computed);
        }

        /// <summary>
        /// 无环地确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected bool Equals(T x, T y, ISet<ObjectPair> compared)
        {
            if (RuntimeHelpers.Equals(x, y)) { return true; }
            if (((object)x is null) ^ ((object)y is null)) { return false; }
            if (x.GetType() != y.GetType()) { return false; }

            var pair = new ObjectPair(x, y);
            if (!compared.Add(pair)) { return true; }

            return this.EqualsCore(x, y, compared);
        }

        /// <summary>
        /// 无环地获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        protected int GetHashCode(T obj, ISet<object> computed)
        {
            if ((object)obj is null) { return 0; }

            if (!computed.Add(obj)) { return 0; }

            return this.GetHashCodeCore(obj, computed);
        }

        /// <summary>
        /// 在派生类中重写，用于确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected abstract bool EqualsCore(T x, T y, ISet<ObjectPair> compared);

        /// <summary>
        /// 在派生类中重写，用于获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        protected abstract int GetHashCodeCore(T obj, ISet<object> computed);

        /// <summary>
        /// 无环地确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        bool IAcyclicEqualityComparer<T>.Equals(T x, T y, ISet<ObjectPair> compared)
        {
            return this.Equals(x, y, compared);
        }

        /// <summary>
        /// 无环地获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        int IAcyclicEqualityComparer<T>.GetHashCode(T obj, ISet<object> computed)
        {
            return this.GetHashCode(obj, computed);
        }

        /// <summary>
        /// 无环地确定指定的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="InvalidCastException">无法强制转换 <paramref name="x"/>
        /// 或 <paramref name="y"/> 到 <typeparamref name="T"/> 类型。</exception>
        bool IAcyclicEqualityComparer.Equals(object x, object y, ISet<ObjectPair> compared)
        {
            return this.Equals((T)x, (T)y, compared);
        }

        /// <summary>
        /// 无环地获取指定对象的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 的哈希代码。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="obj"/> 到 <typeparamref name="T"/> 类型。</exception>
        int IAcyclicEqualityComparer.GetHashCode(object obj, ISet<object> computed)
        {
            return this.GetHashCode((T)obj, computed);
        }
    }
}
