using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Linq
{
    /// <summary>
    /// 提供 LINQ 相关类型的扩展方法。
    /// </summary>
    public static class LinqTypeExtensions
    {
        /// <summary>
        /// 从指定的的 <see cref="IGrouping{TKey, TElement}"/> 的集合创建 <see cref="Dictionary{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IGrouping{TKey, TElement}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TElement"><see cref="IGrouping{TKey, TElement}"/> 中的元素的类型。</typeparam>
        /// <param name="groups">要创建 <see cref="Dictionary{TKey, TValue}"/> 的
        /// <see cref="IGrouping{TKey, TElement}"/> 的集合。</param>
        /// <param name="comparer">用于比较键的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns>由 <paramref name="groups"/> 中数据创建的 <see cref="Dictionary{TKey, TValue}"/>。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="groups"/>
        /// 或 <paramref name="groups"/> 中的元素为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <see cref="Dictionary{TKey, TValue}"/> 中已存在具有相同键的元素。</exception>
        public static Dictionary<TKey, IEnumerable<TElement>> ToDictionary<TKey, TElement>(
            this IEnumerable<IGrouping<TKey, TElement>> groups,
            IEqualityComparer<TKey> comparer = null)
        {
            if (groups is null)
            {
                throw new ArgumentNullException(nameof(groups));
            }

            comparer = comparer ?? EqualityComparer<TKey>.Default;

            var result = new Dictionary<TKey, IEnumerable<TElement>>(comparer);
            foreach (var group in groups)
            {
                if (group is null)
                {
                    throw new ArgumentNullException(nameof(group));
                }

                result.Add(group.Key, group);
            }
            return result;
        }
    }
}
