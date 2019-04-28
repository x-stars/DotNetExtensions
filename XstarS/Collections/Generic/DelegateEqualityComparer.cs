using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 表示一个包装相等比较方法委托的相等比较器 <see cref="EqualityComparer{T}"/>。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        /// <summary>
        /// 当前实例包装的相等比较方法的委托。
        /// </summary>
        private readonly Func<T, T, bool> EqualsDelegate;
        /// <summary>
        /// 当前实例包装的哈希函数方法的委托。
        /// </summary>
        private readonly Func<T, int> GetHashCodeDelegate;

        /// <summary>
        /// 使用相等比较方法委托 <see cref="Func{T1, T2, TResult}"/>
        /// 和哈希函数方法委托 <see cref="Func{T, TResult}"/>
        /// 初始化一个 <see cref="DelegateEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        /// <param name="equals">相等比较方法的 <see cref="Func{T1, T2, TResult}"/> 委托。</param>
        /// <param name="getHashCode">哈希函数方法的 <see cref="Func{T, TResult}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="equals"/> 或 <paramref name="getHashCode"/> 为 <see langword="null"/>。</exception>
        public DelegateEqualityComparer(
            Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            this.EqualsDelegate = equals ??
                throw new ArgumentNullException(nameof(equals));
            this.GetHashCodeDelegate = getHashCode ??
                throw new ArgumentNullException(nameof(getHashCode));
        }

        /// <summary>
        /// 确定两个 <typeparamref name="T"/> 类型的对象是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>若 <paramref name="x"/> 和 <paramref name="y"/> 相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T x, T y) => this.EqualsDelegate(x, y);

        /// <summary>
        /// 获取指定 <typeparamref name="T"/> 类型对象的哈希函数。
        /// </summary>
        /// <param name="obj">要获取哈希函数的对象。</param>
        /// <returns><paramref name="obj"/> 的 32 位有符号整数哈希函数。</returns>
        public override int GetHashCode(T obj) => this.GetHashCodeDelegate(obj);
    }
}
