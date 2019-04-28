using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    partial class GenericCollectionExtensions
    {
        /// <summary>
        /// 根据指定的键获取 <see cref="IDictionary{TKey, TValue}"/> 中对应值。
        /// 若指定的键不存在，则将指定的键值对添加到 <see cref="IDictionary{TKey, TValue}"/> 中。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="key">要从 <see cref="IDictionary{TKey, TValue}"/> 中获取对应值的键。</param>
        /// <param name="value">当指定的键不存在时，要添加到 <see cref="IDictionary{TKey, TValue}"/> 中的值。</param>
        /// <returns>若 <paramref name="source"/> 中存在键 <paramref name="key"/>，
        /// 则为 <paramref name="key"/> 对应的值；否则为 <paramref name="value"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ContainsKey(key) ? source[key] : (source[key] = value);
        }

        /// <summary>
        /// 根据指定的值获取 <see cref="IDictionary{TKey, TValue}"/> 中对应的所有键。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="value">要从 <see cref="IDictionary{TKey, TValue}"/> 中获取对应键的值。</param>
        /// <param name="valueComparer">用于比较值的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns><paramref name="source"/> 中值为 <paramref name="value"/> 的键的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static ICollection<TKey> KeysOf<TKey, TValue>(
            this IDictionary<TKey, TValue> source, TValue value,
            IEqualityComparer<TValue> valueComparer = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            var keys = new HashSet<TKey>();
            foreach (var item in source)
            {
                if (valueComparer.Equals(item.Value, value))
                {
                    keys.Add(item.Key);
                }
            }
            keys.TrimExcess();
            return keys;
        }

        /// <summary>
        /// 从 <see cref="IDictionary{TKey, TValue}"/> 中移除所有键在指定集合中的元素。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="keyCollection">要从 <see cref="IDictionary{TKey, TValue}"/> 中移除的键的集合。</param>
        /// <returns>从 <paramref name="source"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static int RemoveRange<TKey, TValue>(
            this IDictionary<TKey, TValue> source, IEnumerable<TKey> keyCollection)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int result = 0;
            foreach (var key in keyCollection)
            {
                if (source.Remove(key))
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// 将 <see cref="IDictionary{TKey, TValue}"/> 的键和值反转并返回。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="valueComparer">用于比较值的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns>将 <paramref name="source"/> 的键和值反转后的结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IDictionary<TValue, ICollection<TKey>> Inverse<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            IEqualityComparer<TValue> valueComparer = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            var result = new Dictionary<TValue, ICollection<TKey>>(valueComparer);
            foreach (var item in source)
            {
                if (!result.ContainsKey(item.Value))
                {
                    result.Add(item.Value, new List<TKey>());
                }
                result[item.Value].Add(item.Key);
            }
            return result;
        }
    }
}
