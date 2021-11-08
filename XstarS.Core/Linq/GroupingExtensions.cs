using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Linq
{
    /// <summary>
    /// 提供 <see cref="IGrouping{TKey, TElement}"/> 的扩展方法。
    /// </summary>
    public static class GroupingExtensions
    {
        /// <summary>
        /// 对序列的分组结果按组进行聚合，并返回分组键和聚合结果的键值对的序列。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IGrouping{TKey, TElement}"/> 键的类型。</typeparam>
        /// <typeparam name="TElement"><see cref="IGrouping{TKey, TElement}"/> 中的值的类型。</typeparam>
        /// <typeparam name="TResult">聚合结果值的类型。</typeparam>
        /// <param name="groupings">要分组进行聚合的 <see cref="IGrouping{TKey, TElement}"/> 的序列。</param>
        /// <param name="aggregator">对每个分组调用的聚合函数。</param>
        /// <returns>将 <paramref name="groupings"/> 按组聚合的结果的键值对的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="groupings"/>
        /// 或 <paramref name="aggregator"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<KeyValuePair<TKey, TResult>> Aggregate<TKey, TElement, TResult>(
            this IEnumerable<IGrouping<TKey, TElement>> groupings,
            Func<IEnumerable<TElement>, TResult> aggregator)
        {
            if (groupings is null) { throw new ArgumentNullException(nameof(groupings)); }
            if (aggregator is null) { throw new ArgumentNullException(nameof(aggregator)); }
            return groupings.Select(grouping =>
                new KeyValuePair<TKey, TResult>(grouping.Key, aggregator.Invoke(grouping)));
        }

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
