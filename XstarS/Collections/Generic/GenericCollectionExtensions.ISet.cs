using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    partial class GenericCollectionExtensions
    {
        /// <summary>
        /// 将指定集合的元素添加到 <see cref="ISet{T}"/> 中。
        /// </summary>
        /// <typeparam name="T"><see cref="ISet{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ISet{T}"/> 对象。</param>
        /// <param name="collection">应将其元素添加到 <see cref="ISet{T}"/> 中的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <returns>添加到 <paramref name="source"/> 中的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static int AddRange<T>(this ISet<T> source, IEnumerable<T> collection)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            int result = 0;
            foreach (var item in collection)
            {
                if (source.Add(item))
                {
                    result++;
                }
            }
            return result;
        }
    }
}
