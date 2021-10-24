using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XstarS.Collections.Generic
{
    partial class CollectionExtensions
    {
        /// <summary>
        /// 返回指定 <see cref="IDictionary{TKey, TValue}"/> 的只读包装。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">要获取只读包装的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <returns><see cref="IDictionary{TKey, TValue}"/> 的
        /// <see cref="ReadOnlyDictionary{TKey, TValue}"/> 只读包装。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// 对当前 <see cref="IDictionary{TKey, TValue}"/> 对象中的每对键值执行指定操作。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">要对键值执行操作的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="action">要对 <see cref="IDictionary{TKey, TValue}"/> 中每对键值执行的操作。</param>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>
        /// 或 <paramref name="action"/> 为 <see langword="null"/>。</exception>
        public static void ForEach<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, Action<TKey, TValue> action)
            where TKey : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var pair in dictionary)
            {
                action.Invoke(pair.Key, pair.Value);
            }
        }

        /// <summary>
        /// 根据指定的键获取 <see cref="IDictionary{TKey, TValue}"/> 中对应值。
        /// 若指定的键不存在，则将指定的键值对添加到 <see cref="IDictionary{TKey, TValue}"/> 中。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">要获取值的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="key">要从 <see cref="IDictionary{TKey, TValue}"/> 中获取对应值的键。</param>
        /// <param name="value">当指定的键不存在时，要添加到 <see cref="IDictionary{TKey, TValue}"/> 中的值。</param>
        /// <returns>若 <paramref name="dictionary"/> 中存在键 <paramref name="key"/>，
        /// 则为 <paramref name="key"/> 对应的值；否则为 <paramref name="value"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
            where TKey : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            return dictionary.ContainsKey(key) ? dictionary[key] : (dictionary[key] = value);
        }

        /// <summary>
        /// 根据指定的键获取 <see cref="IDictionary{TKey, TValue}"/> 中对应值。
        /// 若指定的键不存在，则将指定的键值对添加到 <see cref="IDictionary{TKey, TValue}"/> 中。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">要获取值的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="key">要从 <see cref="IDictionary{TKey, TValue}"/> 中获取对应值的键。</param>
        /// <param name="valueFactory">当指定的键不存在时，用于创建要添加到
        /// <see cref="IDictionary{TKey, TValue}"/> 中的值的函数。</param>
        /// <returns>若 <paramref name="dictionary"/> 中存在键 <paramref name="key"/>，
        /// 则为 <paramref name="key"/> 对应的值；否则为 <paramref name="valueFactory"/> 创建的值。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>
        /// 或 <paramref name="valueFactory"/> 为 <see langword="null"/>。</exception>
        public static TValue GetOrAdd<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
            where TKey : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }
            if (valueFactory is null)
            {
                throw new ArgumentNullException(nameof(valueFactory));
            }

            return dictionary.ContainsKey(key) ? dictionary[key] : (dictionary[key] = valueFactory(key));
        }

        /// <summary>
        /// 将 <see cref="IDictionary{TKey, TValue}"/> 的键和值反转并返回。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">要反转键值的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="keyComparer">用于比较键的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <param name="valueComparer">用于比较值的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns>将 <paramref name="dictionary"/> 的键和值反转后的结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static IDictionary<TValue, ICollection<TKey>> Inverse<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TKey>? keyComparer = null,
            IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
            where TValue : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            var result = new Dictionary<TValue, ICollection<TKey>>(valueComparer);
            foreach (var item in dictionary)
            {
                if (!result.ContainsKey(item.Value))
                {
                    result.Add(item.Value, new HashSet<TKey>(keyComparer));
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
        /// <param name="dictionary">要获取键的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="value">要从 <see cref="IDictionary{TKey, TValue}"/> 中获取对应键的值。</param>
        /// <param name="keyComparer">用于比较键的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <param name="valueComparer">用于比较值的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns><paramref name="dictionary"/> 中值为 <paramref name="value"/> 的键的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static ICollection<TKey> KeysOf<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, TValue value,
            IEqualityComparer<TKey>? keyComparer = null,
            IEqualityComparer<TValue>? valueComparer = null)
            where TKey : notnull
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            var keys = new HashSet<TKey>(keyComparer);
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
        /// <param name="dictionary">要移除键的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="keys">要从 <see cref="IDictionary{TKey, TValue}"/> 中移除的键的集合。</param>
        /// <returns>从 <paramref name="dictionary"/> 中移除的元素的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dictionary"/>
        /// 或 <paramref name="keys"/> 为 <see langword="null"/>。</exception>
        public static int RemoveRange<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> keys)
            where TKey : notnull
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
        /// 返回包含指定 <see cref="IDictionary{TKey, TValue}"/> 中键值的线程安全集合。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="dictionary">要转换为线程安全集合的 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="comparer">用于比较键的 <see cref="IEqualityComparer{T}"/> 实现。</param>
        /// <returns>包含 <see cref="IDictionary{TKey, TValue}"/> 中键值的
        /// <see cref="ConcurrentDictionary{TKey, TValue}"/> 线程安全集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static ConcurrentDictionary<TKey, TValue> ToConcurrent<TKey, TValue>(
            this IDictionary<TKey, TValue> dictionary,
            IEqualityComparer<TKey>? comparer = null)
            where TKey : notnull
        {
            comparer = comparer ?? EqualityComparer<TKey>.Default;
            return new ConcurrentDictionary<TKey, TValue>(dictionary, comparer);
        }

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
            IEqualityComparer<TKey>? comparer = null)
            where TKey : notnull
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
