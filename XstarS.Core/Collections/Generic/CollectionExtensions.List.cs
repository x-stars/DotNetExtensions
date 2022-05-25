using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XstarS.Collections.Generic
{
    static partial class CollectionExtensions
    {
        /// <summary>
        /// 返回 <see cref="IList{T}"/> 的只读包装。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要获取只读包装的 <see cref="IList{T}"/> 对象。</param>
        /// <returns><see cref="IList{T}"/> 的 <see cref="ReadOnlyCollection{T}"/> 只读包装。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }

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
            for (int offset = 0; offset < count; offset++)
            {
                range.Add(list[index + offset]);
            }
            return range;
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
            var comparer = EqualityComparer<T>.Default;
            for (int index = 0; index < list.Count; index++)
            {
                if (comparer.Equals(list[index], item))
                {
                    indices.Add(index);
                }
            }
            return indices.ToArray();
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
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
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
        /// 移除并返回位于 <see cref="IList{T}"/> 结尾处的对象。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要移除尾部值的 <see cref="IList{T}"/> 对象。</param>
        /// <returns>从 <paramref name="list"/> 的结尾处移除的对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="list"/> 为空。</exception>
        public static T Pop<T>(this IList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var lastIndex = list.Count - 1;
            var lastValue = list[lastIndex];
            list.RemoveAt(lastIndex);
            return lastValue;
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

            for (int offset = 0; offset < count; offset++)
            {
                list.RemoveAt(index);
            }
        }

        /// <summary>
        /// 将 <see cref="IList{T}"/> 中的元素的顺序反转。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要反转元素的顺序的 <see cref="IList{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        public static void Reverse<T>(this IList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var items = new T[list.Count];
            list.CopyTo(items, 0);
            Array.Reverse(items);
            list.Clear();
            list.AddRange(items);
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

            var random = new Random();
            for (int index = 0; index < list.Count; index++)
            {
                int rIndex = random.Next(list.Count);
                (list[rIndex], list[index]) = (list[index], list[rIndex]);
            }
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
            Converter<T, TKey> keySelector, IComparer<TKey>? comparer = null)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            if (keySelector is null)
            {
                throw new ArgumentNullException(nameof(keySelector));
            }
            comparer ??= Comparer<TKey>.Default;

            var items = new T[list.Count];
            list.CopyTo(items, 0);
            var keys = Array.ConvertAll(items, keySelector);
            Array.Sort(keys, items, comparer);
            list.Clear();
            list.AddRange(items);
        }

        /// <summary>
        /// 使用指定的比较器将 <see cref="IList{T}"/> 中的元素进行排序。
        /// </summary>
        /// <typeparam name="T"><see cref="IList{T}"/> 中的元素的类型。</typeparam>
        /// <param name="list">要进行排序的 <see cref="IList{T}"/> 对象。</param>
        /// <param name="comparer">用于比较排序的 <see cref="IComparer{T}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        public static void Sort<T>(this IList<T> list, IComparer<T>? comparer = null)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }
            comparer ??= Comparer<T>.Default;

            var items = new T[list.Count];
            list.CopyTo(items, 0);
            Array.Sort(items, comparer);
            list.Clear();
            list.AddRange(items);
        }
    }
}
