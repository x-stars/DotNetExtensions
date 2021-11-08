using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Linq
{
    /// <summary>
    /// 提供 LINQ 查询方法的扩展。
    /// </summary>
    public static partial class LinqExtensions
    {
        /// <summary>
        /// 原样返回当前对象。
        /// </summary>
        /// <typeparam name="TSource">当前对象的类型。</typeparam>
        /// <param name="source">要原样返回的对象。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        internal static TSource Self<TSource>(TSource source) => source;

#if NET461
        /// <summary>
        /// 将一个值追加到序列末尾。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要在末尾追加值的序列。</param>
        /// <param name="element">要追加到 <paramref name="source"/> 的值。</param>
        /// <returns>以 <paramref name="element"/> 结尾的新序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> Append<TSource>(
            this IEnumerable<TSource> source, TSource element)
        {
            return source.Concat(new[] { element });
        }
#endif

        /// <summary>
        /// 使用指定的比较器对序列的元素进行分组计数。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要对其元素进行分组计数的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="comparer">对元素进行比较的 <see cref="IEqualityComparer{T}"/>。</param>
        /// <returns><paramref name="source"/> 中元素分组计数的
        /// <see cref="KeyValuePair{TKey, TValue}"/> 的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<KeyValuePair<TSource, int>> GroupCount<TSource>(
            this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TSource>.Default;
            return source.GroupBy(Self, GroupingExtensions.ToCount, comparer);
        }

        /// <summary>
        /// 根据指定的键选择器函数并将进行比较的键使用指定的比较器对序列的元素进行分组。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TKey"><paramref name="keySelector"/> 返回的键的类型。</typeparam>
        /// <param name="source">要对其元素进行按键分组计数的 <see cref="IEnumerable{T}"/>。</param>
        /// <param name="keySelector">用于提取每个元素的键的函数。</param>
        /// <param name="comparer">对键进行比较的 <see cref="IEqualityComparer{T}"/>。</param>
        /// <returns><paramref name="source"/> 中元素按映射的键分组计数的
        /// <see cref="KeyValuePair{TKey, TValue}"/> 的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="keySelector"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<KeyValuePair<TKey, int>> GroupCountBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            return source.GroupBy(keySelector, GroupingExtensions.ToCount, comparer);
        }

        /// <summary>
        /// 将另一个值插入到序列的指定位置。
        /// </summary>
        /// <typeparam name="TSource">输入序列中的元素的类型。</typeparam>
        /// <param name="source">要插入值的序列。</param>
        /// <param name="index">要在 <paramref name="source"/> 中插入值的位置。</param>
        /// <param name="element">要插入到 <paramref name="source"/> 的值。</param>
        /// <returns>在 <paramref name="source"/> 的 <paramref name="index"/>
        /// 处插入 <paramref name="element"/> 得到的新序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> Insert<TSource>(
            this IEnumerable<TSource> source, int index, TSource element)
        {
            return source.Take(index).Append(element).Concat(source.Skip(index));
        }

        /// <summary>
        /// 将另一个序列插入到序列的指定位置。
        /// </summary>
        /// <typeparam name="TSource">输入序列中的元素的类型。</typeparam>
        /// <param name="source">要插入另一序列的序列。</param>
        /// <param name="index">要在 <paramref name="source"/> 中插入序列的位置。</param>
        /// <param name="other">要插入到 <paramref name="source"/> 的序列。</param>
        /// <returns>在 <paramref name="source"/> 的 <paramref name="index"/>
        /// 处插入 <paramref name="other"/> 得到的新序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> InsertRange<TSource>(
            this IEnumerable<TSource> source, int index, IEnumerable<TSource> other)
        {
            return source.Take(index).Concat(other).Concat(source.Skip(index));
        }

        /// <summary>
        /// 按使用指定的比较器按升序对序列的元素进行排序。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个要排序的值序列。</param>
        /// <param name="comparer">对元素进行比较的 <see cref="IComparer{T}"/>。</param>
        /// <returns>对 <paramref name="source"/>
        /// 的元素进行排序的 <see cref="IOrderedEnumerable{TElement}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IOrderedEnumerable<TSource> Order<TSource>(
            this IEnumerable<TSource> source, IComparer<TSource> comparer = null)
        {
            comparer = comparer ?? Comparer<TSource>.Default;
            return source.OrderBy(LinqExtensions.Self, comparer);
        }

        /// <summary>
        /// 按使用指定的比较器按降序对序列的元素进行排序。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个要排序的值序列。</param>
        /// <param name="comparer">对元素进行比较的 <see cref="IComparer{T}"/>。</param>
        /// <returns>对 <paramref name="source"/>
        /// 的元素进行降序排序的 <see cref="IOrderedEnumerable{TElement}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IOrderedEnumerable<TSource> OrderDescending<TSource>(
            this IEnumerable<TSource> source, IComparer<TSource> comparer = null)
        {
            comparer = comparer ?? Comparer<TSource>.Default;
            return source.OrderByDescending(LinqExtensions.Self, comparer);
        }

#if NET461
        /// <summary>
        /// 向序列的开头添加值。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要在开头添加值的序列。</param>
        /// <param name="element">要放置在 <paramref name="source"/> 前面的值。</param>
        /// <returns>以 <paramref name="element"/> 开头的新序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> Prepend<TSource>(
            this IEnumerable<TSource> source, TSource element)
        {
            return (new[] { element }).Concat(source);
        }
#endif
    }
}
