using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    static partial class CollectionExtensions
    {
        /// <summary>
        /// 顺序连接 <see cref="IEnumerable{T}"/> 中包含的序列。
        /// </summary>
        /// <typeparam name="T"><paramref name="enumerables"/> 包含的
        /// <see cref="IEnumerable{T}"/> 中元素的类型。</typeparam>
        /// <param name="enumerables">要连接包含的序列的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns><paramref name="enumerables"/> 中包含的序列顺序连接后的序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerables"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<T> Concat<T>(this IEnumerable<IEnumerable<T>> enumerables)
        {
            if (enumerables is null)
            {
                throw new ArgumentNullException(nameof(enumerables));
            }

            static IEnumerable<T> ConcatCore(IEnumerable<IEnumerable<T>> enumerables)
            {
                foreach (var enumerable in enumerables)
                {
                    if (enumerable is not null)
                    {
                        foreach (var item in enumerable)
                        {
                            yield return item;
                        }
                    }
                }
            }

            return ConcatCore(enumerables);
        }

        /// <summary>
        /// 获取 <see cref="IEnumerable{T}"/> 对象中指定对象的数量。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="enumerable">要进行计数的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="value">要获取在 <see cref="IEnumerable{T}"/> 中的数量的对象。</param>
        /// <param name="comparer">指定用于进行相等比较的比较器。</param>
        /// <returns><paramref name="enumerable"/> 中与
        /// <paramref name="value"/> 相等的对象的数量。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
        public static int CountOf<T>(this IEnumerable<T> enumerable, T value,
            IEqualityComparer<T>? comparer = null)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            comparer ??= EqualityComparer<T>.Default;

            int count = 0;
            foreach (var item in enumerable)
            {
                if (comparer.Equals(item, value))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 对 <see cref="IEnumerable{T}"/> 对象中的每个元素执行指定操作。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="enumerable">要对元素执行操作的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="action">要对 <see cref="IEnumerable{T}"/> 中每个元素执行的操作。</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/>
        /// 或 <paramref name="action"/> 为 <see langword="null"/>。</exception>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in enumerable)
            {
                action.Invoke(item);
            }
        }

        /// <summary>
        /// 对 <see cref="IEnumerable{T}"/> 对象中的每个带索引的元素执行指定操作。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="enumerable">要对元素执行操作的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="action">要对 <see cref="IEnumerable{T}"/> 中每个带索引的元素执行的操作。</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable"/>
        /// 或 <paramref name="action"/> 为 <see langword="null"/>。</exception>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            var index = 0;
            foreach (var item in enumerable)
            {
                action.Invoke(item, index);
                index++;
            }
        }

        /// <summary>
        /// 将 <see cref="IEnumerable{T}"/> 对象中的每个元素顺序标记索引值并返回索引和元素的元组序列。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="enumerable">要标记元素顺序的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>将 <paramref name="enumerable"/>
        /// 中的每个元素顺序标记索引值得到的索引和元素的元组序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<(int Index, T Item)> Indexed<T>(this IEnumerable<T> enumerable)
        {

            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            static IEnumerable<(int Index, T Item)> IndexedCore(IEnumerable<T> enumerable)
            {
                var index = 0;
                foreach (var item in enumerable)
                {
                    yield return (index, item);
                    index++;
                }
            }

            return IndexedCore(enumerable);
        }
    }
}
