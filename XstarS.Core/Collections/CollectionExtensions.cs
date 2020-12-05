using System;
using System.Collections;
using System.Collections.Generic;

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
    }
}
