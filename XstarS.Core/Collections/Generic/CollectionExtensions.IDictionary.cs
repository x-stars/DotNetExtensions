﻿using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    partial class CollectionExtensions
    {
        /// <summary>
        /// 根据指定的键获取 <see cref="IDictionary{TKey, TValue}"/> 中对应值。
        /// 若指定的键不存在，则将指定的键值对添加到 <see cref="IDictionary{TKey, TValue}"/> 中。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="key">要从 <see cref="IDictionary{TKey, TValue}"/> 中获取对应值的键。</param>
        /// <param name="value">当指定的键不存在时，要添加到 <see cref="IDictionary{TKey, TValue}"/> 中的值。</param>
        /// <returns>若 <paramref name="dictionary"/> 中存在键 <paramref name="key"/>，
        /// 则为 <paramref name="key"/> 对应的值；否则为 <paramref name="value"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary.ContainsKey(key) ? dictionary[key] : (dictionary[key] = value);
        }

        /// <summary>
        /// 将 <see cref="IDictionary{TKey, TValue}"/> 的键和值反转并返回。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="valueComparer">用于比较值的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns>将 <paramref name="dictionary"/> 的键和值反转后的结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static IDictionary<TValue, ICollection<TKey>> Inverse<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TValue> valueComparer = null)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            var result = new Dictionary<TValue, ICollection<TKey>>(valueComparer);
            foreach (var item in dictionary)
            {
                if (!result.ContainsKey(item.Value))
                {
                    result.Add(item.Value, new List<TKey>());
                }
                result[item.Value].Add(item.Key);
            }
            return result;
        }

        /// <summary>
        /// 根据指定的值获取 <see cref="IDictionary{TKey, TValue}"/> 中对应的所有键。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="value">要从 <see cref="IDictionary{TKey, TValue}"/> 中获取对应键的值。</param>
        /// <param name="valueComparer">用于比较值的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns><paramref name="dictionary"/> 中值为 <paramref name="value"/> 的键的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static ICollection<TKey> KeysOf<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TValue value,
            IEqualityComparer<TValue> valueComparer = null)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            var keys = new HashSet<TKey>();
            foreach (var item in dictionary)
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
        /// <param name="dictionary">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="keys">要从 <see cref="IDictionary{TKey, TValue}"/> 中移除的键的集合。</param>
        /// <returns>从 <paramref name="dictionary"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>
        /// 或 <paramref name="keys"/> 为 <see langword="null"/>。</exception>
        public static int RemoveRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (keys is null)
            {
                throw new ArgumentNullException(nameof(keys));
            }

            int result = 0;
            foreach (var key in keys)
            {
                if (dictionary.Remove(key))
                {
                    result++;
                }
            }
            return result;
        }

        /// <summary>
        /// 从指定的的 <see cref="KeyValuePair{TKey, TValue}"/> 的集合创建
        /// <see cref="Dictionary{TKey, TValue}"/>，并使用指定的 <see cref="IEqualityComparer{T}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="KeyValuePair{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="KeyValuePair{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="pairs">一个 <see cref="KeyValuePair{TKey, TValue}"/> 的集合。</param>
        /// <param name="comparer">用于比较键的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns>由 <paramref name="pairs"/> 中数据创建的 <see cref="Dictionary{TKey, TValue}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pairs"/> 为 <see langword="null"/>。</exception>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(
            this IEnumerable<KeyValuePair<TKey, TValue>> pairs,
            IEqualityComparer<TKey> comparer = null)
        {
            if (pairs is null)
            {
                throw new ArgumentNullException(nameof(pairs));
            }

            comparer = comparer ?? EqualityComparer<TKey>.Default;
            var dictionary = new Dictionary<TKey, TValue>(comparer);
            foreach (var pair in pairs)
            {
                dictionary[pair.Key] = pair.Value;
            }
            return dictionary;
        }
    }
}
