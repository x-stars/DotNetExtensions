using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    partial class CollectionExtensions
    {
        /// <summary>
        /// 反转当前比较器 <see cref="IComparer{T}"/> 并返回。
        /// </summary>
        /// <typeparam name="T"><see cref="IComparer{T}"/> 要比较的对象的类型。</typeparam>
        /// <param name="comparer">要进行反转的 <see cref="IComparer{T}"/> 对象。</param>
        /// <returns>将 <paramref name="comparer"/> 反转后得到的 <see cref="IComparer{T}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer"/> 为 <see langword="null"/>。</exception>
        public static IComparer<T> Reverse<T>(IComparer<T> comparer)
        {
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return new ReversedComparer<T>(comparer);
        }
    }
}
