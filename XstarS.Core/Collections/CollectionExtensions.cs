﻿using System;
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
        /// 将当前 <see cref="IList"/> 强制转换为指定元素类型的泛型集合。
        /// </summary>
        /// <typeparam name="T"><see cref="IList"/> 中的元素要转换为的类型。</typeparam>
        /// <param name="collection">要转换为指定元素类型的 <see cref="IList"/> 对象。</param>
        /// <returns><paramref name="collection"/> 的指定元素类型的泛型包装。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static GenericCollection<T> AsGeneric<T>(this IList collection) =>
            new GenericCollection<T>(collection);

        /// <summary>
        /// 将当前 <see cref="IDictionary"/> 强制转换为指定键值类型的泛型键值对集合。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary"/> 中的键要转换为的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary"/> 中的值要转换为的类型。</typeparam>
        /// <param name="dictionary">要转换为指定键值类型的 <see cref="IDictionary"/> 对象。</param>
        /// <returns><paramref name="dictionary"/> 的指定键值类型的泛型包装。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public static GenericDictionary<TKey, TValue> AsGeneric<TKey, TValue>(
            this IDictionary dictionary) =>
            new GenericDictionary<TKey, TValue>(dictionary);

        /// <summary>
        /// 将当前 <see cref="IEnumerator"/> 强制转换为指定元素类型的泛型集合的枚举数。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerator"/> 中的元素要转换为的类型。</typeparam>
        /// <param name="enumerator">要转换为指定元素类型的 <see cref="IEnumerator"/> 对象。</param>
        /// <returns><paramref name="enumerator"/> 的指定元素类型的泛型包装。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
        public static GenericEnumerator<T> AsGeneric<T>(this IEnumerator enumerator) =>
            new GenericEnumerator<T>(enumerator);

        /// <summary>
        /// 将当前 <see cref="IDictionaryEnumerator"/> 强制转换为指定键值类型的泛型字典的枚举数。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionaryEnumerator"/> 中的键要转换为的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionaryEnumerator"/> 中的值要转换为的类型。</typeparam>
        /// <param name="enumerator">要强制转换键值类型的 <see cref="IDictionaryEnumerator"/> 对象。</param>
        /// <returns><paramref name="enumerator"/> 的指定键值类型的泛型包装。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
        public static GenericDictionaryEnumerator<TKey, TValue> AsGeneric<TKey, TValue>(
            this IDictionaryEnumerator enumerator) =>
            new GenericDictionaryEnumerator<TKey, TValue>(enumerator);

        /// <summary>
        /// 将当前 <see cref="IDictionary"/> 中的键值对强制转换为指定类型。
        /// </summary>
        /// <typeparam name="TKey"><see cref="IDictionary"/> 中键要转换到的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="IDictionary"/> 中值要转换到的类型。</typeparam>
        /// <param name="dictionary">要强制转换键值对类型的 <see cref="IDictionary"/> 对象。</param>
        /// <returns>将 <paramref name="dictionary"/> 中的键值对强制转换类型后得到的序列。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
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
