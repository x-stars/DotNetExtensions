using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    partial class GenericCollectionExtensions
    {
        /// <summary>
        /// 将指定集合的元素添加到 <see cref="ICollection{T}"/> 中。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="collection">应将其元素添加到 <see cref="ICollection{T}"/> 中的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                source.Add(item);
            }
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中移除特定对象的所有匹配项。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="item">要从 <see cref="ICollection{T}"/> 中移除的对象。</param>
        /// <returns>从 <paramref name="source"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static int RemoveAll<T>(this ICollection<T> source, T item)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int result = 0;
            while (source.Remove(item))
            {
                result++;
            }
            return result;
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中依次移除指定集合中的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="collection">应从 <see cref="ICollection{T}"/> 中移除的元素的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <returns>从 <paramref name="source"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static int RemoveRange<T>(this ICollection<T> source, IEnumerable<T> collection)
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
                if (source.Remove(item))
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中移除所有指定集合中的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="collection">应从 <see cref="ICollection{T}"/> 中移除的元素的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <returns>从 <paramref name="source"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static int RemoveRangeAll<T>(this ICollection<T> source, IEnumerable<T> collection)
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
                while (source.Remove(item))
                {
                    result++;
                }
            }
            return result;
        }
    }
}
