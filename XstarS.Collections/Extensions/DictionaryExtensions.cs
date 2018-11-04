using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供键/值对的泛型集合 <see cref="IDictionary{TKey, TValue}"/> 的扩展方法。
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// 根据指定的值获取 <see cref="IDictionary{TKey, TValue}"/> 中对应的键的方法。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary{TKey, TValue}"/> 中的键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary{TKey, TValue}"/> 中的值的类型。</typeparam>
        /// <param name="source">一个 <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <param name="value">指定要查询的值。</param>
        /// <returns><paramref name="source"/> 中值为 <paramref name="value"/> 的键的集合。</returns>
        public static ICollection<TKey> GetKeys<TKey, TValue>(
            this IDictionary<TKey, TValue> source, TValue value)
        {
            var keys = new HashSet<TKey>();
            var valueComparer = EqualityComparer<TValue>.Default;
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
    }
}
