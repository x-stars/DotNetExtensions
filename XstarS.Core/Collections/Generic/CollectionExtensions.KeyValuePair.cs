using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    partial class CollectionExtensions
    {
        /// <summary>
        /// 从指定的的 <see cref="KeyValuePair{TKey, TValue}"/> 的集合创建 <see cref="Dictionary{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="KeyValuePair{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="KeyValuePair{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="pairs">要创建 <see cref="Dictionary{TKey, TValue}"/> 的
        /// <see cref="KeyValuePair{TKey, TValue}"/> 的集合。</param>
        /// <param name="comparer">用于比较键的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns>由 <paramref name="pairs"/> 中数据创建的 <see cref="Dictionary{TKey, TValue}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pairs"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <see cref="Dictionary{TKey, TValue}"/> 中已存在具有相同键的元素。</exception>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> pairs,
            IEqualityComparer<TKey> comparer = null)
        {
            if (pairs is null)
            {
                throw new ArgumentNullException(nameof(pairs));
            }

            comparer = comparer ?? EqualityComparer<TKey>.Default;

            var result = new Dictionary<TKey, TValue>(comparer);
            foreach (var pair in pairs)
            {
                result.Add(pair.Key, pair.Value);
            }
            return result;
        }
    }
}
