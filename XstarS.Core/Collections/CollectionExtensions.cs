using System;
using System.Collections;
using System.Collections.Generic;
using XstarS.Collections.Specialized;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供集合的扩展方法。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 将当前 <see cref="IDictionary"/> 中的键值对强制转换为指定类型。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary"/> 中键要转换到的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary"/> 中值要转换到的类型。</typeparam>
        /// <param name="dictionary">要强制转换键值对类型的 <see cref="IDictionary"/> 对象。</param>
        /// <returns>将 <paramref name="dictionary"/> 中的键值对强制转换类型后得到的序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="InvalidCastException">
        /// <paramref name="dictionary"/> 中包含无法转换为 <typeparamref name="TKey"/>
        /// 类型的键或无法转换为 <typeparamref name="TValue"/> 类型的值。</exception>
        public static IEnumerable<KeyValuePair<TKey, TValue>> Cast<TKey, TValue>(
            this IDictionary dictionary)
        {
            if (dictionary is null)
            {
                throw new ArgumentNullException(nameof(dictionary));
            }

            foreach (DictionaryEntry entry in dictionary)
            {
                yield return new KeyValuePair<TKey, TValue>((TKey)entry.Key, (TValue)entry.Value);
            }
        }

        /// <summary>
        /// 返回当前 <see cref="IEnumerable"/> 对象的所有元素的字符串表达形式。
        /// </summary>
        /// <param name="enumerable">要获取字符串表达形式的 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="recurse">指示是否对内层 <see cref="IEnumerable"/> 递归。</param>
        /// <returns><paramref name="enumerable"/> 的所有元素的字符串表达形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
        public static string CollectionToString(this IEnumerable enumerable, bool recurse = false)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var comparer = ReferenceEqualityComparer<IEnumerable>.Default;
            return enumerable.CollectionToString(recurse, new HashSet<IEnumerable>(comparer));
        }

        /// <summary>
        /// 返回当前 <see cref="IEnumerable"/> 对象的所有元素的字符串表达形式。
        /// </summary>
        /// <param name="enumerable">要获取字符串表达形式的 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="recurse">指示是否对内层 <see cref="IEnumerable"/> 递归。</param>
        /// <param name="pathed">当前路径已经访问的 <see cref="IEnumerable"/> 对象。</param>
        /// <returns><paramref name="enumerable"/> 的所有元素的字符串表达形式。</returns>
        private static string CollectionToString(this IEnumerable enumerable,
            bool recurse, HashSet<IEnumerable> pathed)
        {
            if (!pathed.Add(enumerable))
            {
                return "{ ... }";
            }

            var sequence = new List<string>();
            foreach (var item in enumerable)
            {
                if (recurse && (item is IEnumerable innerEnum))
                {
                    sequence.Add(innerEnum.CollectionToString(recurse, pathed));
                }
                else
                {
                    sequence.Add(item?.ToString());
                }
            }

            pathed.Remove(enumerable);
            return "{ " + string.Join(", ", sequence) + " }";
        }
    }
}
