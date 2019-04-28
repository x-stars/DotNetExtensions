using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 表示一个包装比较方法委托的比较器 <see cref="Comparer{T}"/>。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public class DelegateComparer<T> : Comparer<T>
    {
        /// <summary>
        /// 当前实例包装的比较方法委托。
        /// </summary>
        private readonly Func<T, T, int> CompareDelegate;

        /// <summary>
        /// 使用比较方法委托 <see cref="Func{T1, T2, TResult}"/>
        /// 初始化 <see cref="DelegateComparer{T}"/> 类的新实例。
        /// </summary>
        /// <param name="compare">比较方法的 <see cref="Func{T1, T2, TResult}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="compare"/> 为 <see langword="null"/>。</exception>
        public DelegateComparer(Func<T, T, int> compare)
        {
            this.CompareDelegate = compare ??
                throw new ArgumentNullException(nameof(compare));
        }

        /// <summary>
        /// 比较两个 <typeparamref name="T"/> 类型的对象，并返回一个指示两个对象大小关系的值。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>一个指示 <paramref name="x"/> 和 <paramref name="y"/> 的大小关系的有符号整数：
        /// 小于为负，相等为零，大于为正。</returns>
        public override int Compare(T x, T y) => this.CompareDelegate(x, y);
    }
}
