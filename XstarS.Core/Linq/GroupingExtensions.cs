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
        /// 对序列的分组结果按组进行计数，并返回分组键和计数的键值对的序列。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IGrouping{TKey, TElement}"/> 键的类型。</typeparam>
        /// <typeparam name="TElement"><see cref="IGrouping{TKey, TElement}"/> 中的值的类型。</typeparam>
        /// <param name="groupings">要分组进行计数的 <see cref="IGrouping{TKey, TElement}"/> 的序列。</param>
        /// <returns>将 <paramref name="groupings"/> 按组计数的结果的键值对的序列。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="groupings"/>
        /// 或 <paramref name="groupings"/> 中的分组为 <see langword="null"/>。</exception>
        public static IEnumerable<KeyValuePair<TKey, int>> Count<TKey, TElement>(
            this IEnumerable<IGrouping<TKey, TElement>> groupings)
        {
            if (groupings is null) { throw new ArgumentNullException(nameof(groupings)); }
            return groupings.Select(GroupingExtensions.ToCount);
        }

        /// <summary>
        /// 对序列分组的单个分组结果进行计数，并返回分组键和计数的键值对。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IGrouping{TKey, TElement}"/> 键的类型。</typeparam>
        /// <typeparam name="TElement"><see cref="IGrouping{TKey, TElement}"/> 中的值的类型。</typeparam>
        /// <param name="grouping">要进行计数的 <see cref="IGrouping{TKey, TElement}"/>。</param>
        /// <returns>将 <paramref name="grouping"/> 进行计数得到的键值对。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="grouping"/> 为 <see langword="null"/>。</exception>
        public static KeyValuePair<TKey, int> ToCount<TKey, TElement>(
            this IGrouping<TKey, TElement> grouping)
        {
            if (grouping is null) { throw new ArgumentNullException(nameof(grouping)); }
            return new KeyValuePair<TKey, int>(grouping.Key, grouping.Count());
        }
    }
}
