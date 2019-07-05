using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    partial class GenericCollectionExtensions
    {
        /// <summary>
        /// 将指定集合的元素添加到 <see cref="ISet{T}"/> 中。
        /// </summary>
        /// <typeparam name="T"><see cref="ISet{T}"/> 中的元素的类型。</typeparam>
        /// <param name="set">要添加元素的 <see cref="ISet{T}"/> 对象。</param>
        /// <param name="items">应将其元素添加到 <see cref="ISet{T}"/> 中的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <returns>添加到 <paramref name="set"/> 中的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="set"/>
        /// 或 <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static int AddRange<T>(this ISet<T> set, IEnumerable<T> items)
        {
            if (set is null)
            {
                throw new ArgumentNullException(nameof(set));
            }
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            int result = 0;
            foreach (var item in items)
            {
                if (set.Add(item))
                {
                    result++;
                }
            }
            return result;
        }
    }
}
