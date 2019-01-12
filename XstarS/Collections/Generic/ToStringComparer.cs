using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 使用对象的 <see cref="object.ToString()"/> 方法实现的类型无关的通用比较器。
    /// </summary>
    /// <typeparam name="T">要进行比较的对象的类型。</typeparam>
    public class ToStringComparer<T> : IEqualityComparer, IComparer, IEqualityComparer<T>, IComparer<T>
    {
        /// <summary>
        /// 用于进行字符串比较的比较器。
        /// </summary>
        private readonly StringComparer stringComparer;

        /// <summary>
        /// 初始化 <see cref="ToStringComparer{T}"/> 类的新实例。
        /// </summary>
        public ToStringComparer() : this(false) { }

        /// <summary>
        /// 初始化 <see cref="ToStringComparer{T}"/> 类的新实例，并指定进行字符串比较是是否忽略大小写。
        /// </summary>
        /// <param name="ignoreCase">指定进行字符串比较是是否忽略大小写。</param>
        public ToStringComparer(bool ignoreCase) => this.stringComparer = ignoreCase ?
            StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture;

        /// <summary>
        /// 初始化 <see cref="ToStringComparer{T}"/> 类的新实例，并指定进行字符串比较时使用的比较器。
        /// </summary>
        /// <param name="comparer">进行字符串比较时使用的比较器。</param>
        public ToStringComparer(StringComparer comparer) => this.stringComparer = comparer;

        /// <summary>
        /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值进行比较，
        /// 并返回一个值，指示一个对象是小于、 等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>一个指示 <paramref name="x"/> 与 <paramref name="y"/> 的大小关系的有符号整数，
        /// 大于为正，等于为零，小于为负。</returns>
        public int Compare(T x, T y) => this.stringComparer.Compare(x?.ToString(), y?.ToString());

        /// <summary>
        /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值确定两个对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>
        /// 如果两个对象的 <see cref="object.ToString()"/> 方法返回的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(T x, T y) => this.stringComparer.Equals(x?.ToString(), y?.ToString());

        /// <summary>
        /// 获取对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的对象。</param>
        /// <returns>对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。</returns>
        public int GetHashCode(T obj) => this.stringComparer.GetHashCode(obj?.ToString());

        /// <summary>
        /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值进行比较，
        /// 并返回一个值，指示一个对象是小于、 等于还是大于另一个对象。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>一个指示 <paramref name="x"/> 与 <paramref name="y"/> 的大小关系的有符号整数，
        /// 大于为正，等于为零，小于为负。</returns>
        /// <exception cref="InvalidCastException"><paramref name="x"/>
        /// 或 <paramref name="y"/> 无法转换为 <typeparamref name="T"/> 类型的对象。</exception>
        int IComparer.Compare(object x, object y) => this.Compare((T)x, (T)y);

        /// <summary>
        /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值确定两个对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>
        /// 如果两个对象均为 <typeparamref name="T"/> 类型的对象，
        /// 且 <see cref="object.ToString()"/> 方法返回的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        /// <exception cref="InvalidCastException"><paramref name="x"/>
        /// 或 <paramref name="y"/> 无法转换为 <typeparamref name="T"/> 类型的对象。</exception>
        bool IEqualityComparer.Equals(object x, object y) => this.Equals((T)x, (T)y);

        /// <summary>
        /// 获取对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的对象。</param>
        /// <returns>对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="obj"/> 无法转换为 <typeparamref name="T"/> 类型的对象。</exception>
        int IEqualityComparer.GetHashCode(object obj) => this.GetHashCode((T)obj);
    }
}
