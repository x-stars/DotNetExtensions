using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 表示一个指定比较器的反转的比较器 <see cref="Comparer{T}"/>。
    /// </summary>
    /// <typeparam name="T">要比较的对象的类型。</typeparam>
    [Serializable]
    public sealed class ReversedComparer<T> : Comparer<T>
    {
        /// <summary>
        /// 要进行反转的比较器 <see cref="IComparer{T}"/>。
        /// </summary>
        private readonly IComparer<T> Comparer;

        /// <summary>
        /// 使用要进行反转的比较器 <see cref="IComparer{T}"/>
        /// 初始化 <see cref="ReversedComparer{T}"/> 类的新实例。
        /// </summary>
        /// <param name="comparer">要进行反转的比较器 <see cref="IComparer{T}"/>。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer"/> 为 <see langword="null"/>。</exception>
        public ReversedComparer(IComparer<T> comparer)
        {
            this.Comparer = comparer ??
                throw new ArgumentNullException(nameof(comparer));
        }

        /// <summary>
        /// 获取默认的 <see cref="ReversedComparer{T}"/> 类的实例。
        /// </summary>
        /// <returns>默认的 <see cref="ReversedComparer{T}"/> 类的实例。</returns>
        public static new ReversedComparer<T> Default { get; } =
            new ReversedComparer<T>(Comparer<T>.Default);

        /// <summary>
        /// 比较两个 <typeparamref name="T"/> 类型的对象，并返回一个反转的指示两个对象大小关系的值。
        /// </summary>
        /// <param name="x">要比较的第一个对象。</param>
        /// <param name="y">要比较的第二个对象。</param>
        /// <returns>一个指示 <paramref name="x"/> 和 <paramref name="y"/> 的大小关系的有符号整数：
        /// 小于为正，相等为零，大于为负。</returns>
        public override int Compare([AllowNull] T x, [AllowNull] T y) =>
            -this.Comparer.Compare(x!, y!);
    }
}
