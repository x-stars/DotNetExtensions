using System;
using System.Collections;
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

        /// <summary>
        /// 将序列的每个分组结果序列聚合为新的值，并返回分组键和聚合结果的键值对的序列。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IGrouping{TKey, TElement}"/> 键的类型。</typeparam>
        /// <typeparam name="TElement"><see cref="IGrouping{TKey, TElement}"/> 中的值的类型。</typeparam>
        /// <typeparam name="TResult">聚合结果值的类型。</typeparam>
        /// <param name="source">要分组结果序列进行聚合的 <see cref="IGrouping{TKey, TElement}"/> 的序列。</param>
        /// <param name="valuesAggregator">应用于每个分组结果序列的聚合函数。</param>
        /// <returns>将 <paramref name="source"/> 按组映射结果的键值对的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="valuesAggregator"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<KeyValuePair<TKey, TResult>> AggregateValues<TKey, TElement, TResult>(
            this IEnumerable<IGrouping<TKey, TElement>> source,
            Func<IEnumerable<TElement>, TResult> valuesAggregator)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (valuesAggregator is null) { throw new ArgumentNullException(nameof(valuesAggregator)); }
            return source.Select(grouping =>
                new KeyValuePair<TKey, TResult>(grouping.Key, valuesAggregator.Invoke(grouping)));
        }

#if NETFRAMEWORK && !NET471_OR_GREATER
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
        /// 将 <see cref="IDictionary"/> 中的键值对强制转换为指定类型。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary"/> 中键要转换到的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary"/> 中值要转换到的类型。</typeparam>
        /// <param name="dictionary">要强制转换键值对类型的 <see cref="IDictionary"/> 对象。</param>
        /// <returns>将 <paramref name="dictionary"/> 中的键值对强制转换类型后得到的序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<KeyValuePair<TKey, TValue?>> Cast<TKey, TValue>(
            this IDictionary dictionary) where TKey : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            var entryEnum = dictionary.GetEnumerator();
            using (entryEnum as IDisposable)
            {
                while (entryEnum.MoveNext())
                {
                    var key = (TKey)entryEnum.Key;
                    var value = (TValue?)entryEnum.Value;
                    yield return new KeyValuePair<TKey, TValue?>(key, value);
                }
            }
        }

#if !NET6_0_OR_GREATER
        /// <summary>
        /// 使用键选择器函数和指定的比较器对键进行比较，返回序列中的非重复元素。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <typeparam name="TKey"><paramref name="keySelector"/> 返回的键的类型。</typeparam>
        /// <param name="source">要从中移除重复元素的序列。</param>
        /// <param name="keySelector">用于提取每个元素的键的函数。</param>
        /// <param name="comparer">对键进行比较的 <see cref="IEqualityComparer{T}"/>。</param>
        /// <returns>一个包含源序列中的非重复元素的 <see cref="IEnumerable{T}"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="keySelector"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector,
            IEqualityComparer<TKey>? comparer = null)
        {
            comparer ??= EqualityComparer<TKey>.Default;
            return source.GroupBy(keySelector, comparer).Select(Enumerable.First);
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
            this IEnumerable<TSource> source, IEqualityComparer<TSource>? comparer = null)
        {
            comparer ??= EqualityComparer<TSource>.Default;
            return source.GroupBy(Self, LinqExtensions.ToCount, comparer);
        }

        /// <summary>
        /// 根据指定的键选择器函数对序列中的元素进行分组计数，并使用指定的比较器对键进行比较。
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
            IEqualityComparer<TKey>? comparer = null)
        {
            comparer ??= EqualityComparer<TKey>.Default;
            return source.GroupBy(keySelector, LinqExtensions.ToCount, comparer);
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
        /// <param name="elements">要插入到 <paramref name="source"/> 的序列。</param>
        /// <returns>在 <paramref name="source"/> 的 <paramref name="index"/>
        /// 处插入 <paramref name="elements"/> 得到的新序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="elements"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<TSource> InsertRange<TSource>(
            this IEnumerable<TSource> source, int index, IEnumerable<TSource> elements)
        {
            return source.Take(index).Concat(elements).Concat(source.Skip(index));
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
            this IEnumerable<TSource> source, IComparer<TSource>? comparer = null)
        {
            comparer ??= Comparer<TSource>.Default;
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
            this IEnumerable<TSource> source, IComparer<TSource>? comparer = null)
        {
            comparer ??= Comparer<TSource>.Default;
            return source.OrderByDescending(LinqExtensions.Self, comparer);
        }

#if NETFRAMEWORK && !NET471_OR_GREATER
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

        /// <summary>
        /// 对序列分组的单个分组结果进行计数，并返回分组键和计数的键值对。
        /// </summary>
        /// <typeparam name="TKey">序列分组的键的类型。</typeparam>
        /// <typeparam name="TElement"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="key">序列分组的键。</param>
        /// <param name="elements">要进行计数的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>将 <paramref name="elements"/>
        /// 进行计数得到的以 <paramref name="key"/> 为键的键值对。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="elements"/> 为 <see langword="null"/>。</exception>
        internal static KeyValuePair<TKey, int> ToCount<TKey, TElement>(
            TKey key, IEnumerable<TElement> elements)
        {
            if (elements is null) { throw new ArgumentNullException(nameof(elements)); }
            return new KeyValuePair<TKey, int>(key, elements.Count());
        }
    }
}
