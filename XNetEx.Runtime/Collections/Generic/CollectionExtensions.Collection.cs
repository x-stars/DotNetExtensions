using System;
using System.Collections.Generic;

namespace XNetEx.Collections.Generic
{
    static partial class CollectionExtensions
    {
        /// <summary>
        /// 将指定集合的元素添加到 <see cref="ICollection{T}"/> 中。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="collection">要添加元素 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="items">应将其元素添加到 <see cref="ICollection{T}"/> 中的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/>
        /// 或 <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            foreach (var item in items)
            {
                collection.Add(item);
            }
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中移除特定对象的所有匹配项。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="collection">要移除元素 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="item">要从 <see cref="ICollection{T}"/> 中移除的对象。</param>
        /// <returns>从 <paramref name="collection"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static int RemoveAll<T>(this ICollection<T> collection, T item)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var removed = 0;
            while (collection.Remove(item))
            {
                removed++;
            }
            return removed;
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中移除所有指定集合中的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="collection">要移除元素 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="items">应从 <see cref="ICollection{T}"/> 中移除的元素的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <returns>从 <paramref name="collection"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/>
        /// 或 <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static int RemoveAll<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var removed = 0;
            foreach (var item in items)
            {
                removed += collection.RemoveAll(item);
            }
            return removed;
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中移除所有满足指定条件的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="collection">要移除元素的 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="match">要从 <see cref="ICollection{T}"/> 中移除的元素应满足的条件。</param>
        /// <returns>从 <see cref="ICollection{T}"/> 中移除的元素数。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/>
        /// 或 <paramref name="match"/> 为 <see langword="null"/>。</exception>
        public static int RemoveAll<T>(this ICollection<T> collection, Predicate<T> match)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            var removing = new List<T>(collection);
            var count = removing.RemoveAll(match);
            collection.Clear();
            collection.AddRange(removing);
            return count;
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中依次移除指定集合中的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="collection">要移除元素 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="items">应从 <see cref="ICollection{T}"/> 中移除的元素的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <returns>从 <paramref name="collection"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/>
        /// 或 <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static int RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }
            if (items is null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            var removed = 0;
            foreach (var item in items)
            {
                if (collection.Remove(item))
                {
                    removed++;
                }
            }
            return removed;
        }
    }
}
