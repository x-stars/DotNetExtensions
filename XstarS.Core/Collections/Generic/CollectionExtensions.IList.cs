using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    partial class CollectionExtensions
    {
        /// <summary>
        /// 创建 <see cref="IList{T}"/> 中指定元素范围的浅表复制。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要获取元素的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="index">范围开始处的从零开始的索引。</param>
        /// <param name="count">范围中的元素数。</param>
        /// <returns><see cref="IList{T}"/> 中指定元素范围的浅表复制。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="index"/> 和 <paramref name="count"/>
        /// 不表示 <see cref="IList{T}"/> 中元素的有效范围。</exception>
        public static IList<T> GetRange<T>(this IList<T> list, int index, int count)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var range = new List<T>(count);
            for (int i = 0; i < count; i++)
            {
                range.Add(list[index + i]);
            }
            return range;
        }

        /// <summary>
        /// 将指定集合的元素插入到 <see cref="IList{T}"/> 的的指定索引处。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要插入元素的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="index">应在此处插入新元素的从零开始的索引。</param>
        /// <param name="collection">应将其元素添加到 <see cref="IList{T}"/> 的末尾的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="IndexOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/> 大于 <see cref="ICollection{T}.Count"/>。</exception>
        public static void InsertRange<T>(this IList<T> list, int index, IEnumerable<T> collection)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                list.Insert(index, item);
                index++;
            }
        }

        /// <summary>
        /// 从 <see cref="IList{T}"/> 中移除一定范围的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要移除元素的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="index">要移除的元素范围的从零开始的起始索引。</param>
        /// <param name="count">要移除的元素数。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 小于 0。或 <paramref name="count"/> 小于 0。</exception>
        /// <exception cref="ArgumentException"><paramref name="index"/>
        /// 和 <paramref name="count"/> 不表示 <see cref="IList{T}"/> 中元素的有效范围。</exception>
        public static void RemoveRange<T>(this IList<T> list, int index, int count)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            for (int i = 0; i < count; i++)
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// 从 <see cref="IList{T}"/> 中移除所有满足指定条件的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要移除元素的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="match">要从 <see cref="IList{T}"/> 中移除的元素应满足的条件。</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/>
        /// 或 <paramref name="match"/> 为 <see langword="null"/>。</exception>
        public static void RemoveAll<T>(this IList<T> list, Predicate<T> match)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (match is null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                {
                    list.RemoveAt(i);
                    i--;
                }
            }
        }

        /// <summary>
        /// 确定 <see cref="IList{T}"/> 中特定项的所有索引。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要获取索引的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="item">要在 <see cref="IList{T}"/> 中定位的对象。</param>
        /// <returns><paramref name="list"/> 中所有与 <paramref name="item"/> 相等的元素的索引；
        /// 若未在 <paramref name="list"/> 中找到 <paramref name="item"/>，则为空列表。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        public static int[] IndicesOf<T>(this IList<T> list, T item)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var indices = new List<int>();
            for (int i = 0; i < list.Count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(list[i], item))
                {
                    indices.Add(i);
                }
            }
            return indices.ToArray();
        }

        /// <summary>
        /// 使用指定的比较器将 <see cref="IList{T}"/> 中的元素按指定键进行排序。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TKey">作为排序基准的属性的类型。</typeparam>
        /// <param name="list">要进行排序的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="keySelector">用于从元素中提取键的函数。</param>
        /// <param name="comparer">用于比较排序的 <see cref="IComparer{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        public static void Sort<T, TKey>(this IList<T> list,
            Func<T, TKey> keySelector, IComparer<TKey> comparer = null)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (keySelector is null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            comparer = comparer ?? Comparer<TKey>.Default;

            var listCopy = new List<T>(list);
            list.Clear();
            list.AddRange(listCopy.OrderBy(keySelector, comparer));
        }

        /// <summary>
        /// 使用指定的比较器将 <see cref="IList{T}"/> 中的元素进行排序。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要进行排序的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="comparer">用于比较排序的 <see cref="IComparer{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        public static void Sort<T>(this IList<T> list, IComparer<T> comparer = null)
        {
            list.Sort(item => item, comparer);
        }

        /// <summary>
        /// 将 <see cref="IList{T}"/> 中的元素随机重新排列。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要进行随机重排的 <see cref="IList{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        public static void Shuffle<T>(this IList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var randGen = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                int r = randGen.Next(list.Count);
                var temp = list[i];
                list[i] = list[r];
                list[r] = temp;
            }
        }
    }
}
