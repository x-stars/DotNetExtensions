using System;
using System.Collections;
using System.Collections.Generic;
using XNetEx.Collections.ObjectModel;

namespace XNetEx.Collections;

/// <summary>
/// 提供集合的扩展方法。
/// </summary>
public static class CollectionExtensions
{
    /// <summary>
    /// 返回 <see cref="IEnumerator"/> 的公开枚举数包装。
    /// </summary>
    /// <param name="enumerator">要包装的 <see cref="IEnumerator"/> 对象。</param>
    /// <returns><paramref name="enumerator"/> 的 <see cref="IEnumerable"/> 包装。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
    public static IEnumerable AsEnumerable(this IEnumerator enumerator)
    {
        return new EnumeratorEnumerable(enumerator);
    }

    /// <summary>
    /// 将 <see cref="IList"/> 强制转换为指定元素类型的泛型集合。
    /// </summary>
    /// <typeparam name="T"><see cref="IList"/> 中的元素要转换为的类型。</typeparam>
    /// <param name="collection">要转换为指定元素类型的 <see cref="IList"/> 对象。</param>
    /// <returns><paramref name="collection"/> 的指定元素类型的泛型包装。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
    public static GenericCollection<T> AsGeneric<T>(this IList collection)
    {
        return new GenericCollection<T>(collection);
    }

    /// <summary>
    /// 将 <see cref="IDictionary"/> 强制转换为指定键值类型的泛型键值对集合。
    /// </summary>
    /// <typeparam name="TKey"><see cref="IDictionary"/> 中的键要转换为的类型。</typeparam>
    /// <typeparam name="TValue"><see cref="IDictionary"/> 中的值要转换为的类型。</typeparam>
    /// <param name="dictionary">要转换为指定键值类型的 <see cref="IDictionary"/> 对象。</param>
    /// <returns><paramref name="dictionary"/> 的指定键值类型的泛型包装。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
    public static GenericDictionary<TKey, TValue> AsGeneric<TKey, TValue>(this IDictionary dictionary)
        where TKey : notnull
    {
        return new GenericDictionary<TKey, TValue>(dictionary);
    }

    /// <summary>
    /// 解构当前 <see cref="DictionaryEntry"/>。
    /// </summary>
    /// <param name="entry">要解构的 <see cref="DictionaryEntry"/>。</param>
    /// <param name="key">当前 <see cref="DictionaryEntry"/> 的键。</param>
    /// <param name="value">当前 <see cref="DictionaryEntry"/> 的值。</param>
    public static void Deconstruct(
        this DictionaryEntry entry, out object key, out object? value)
    {
        key = entry.Key;
        value = entry.Value;
    }

    /// <summary>
    /// 获取 <see cref="IDictionary"/> 中的键值对的公开枚举数。
    /// </summary>
    /// <param name="dictionary">要枚举键值对的 <see cref="IDictionary"/> 对象。</param>
    /// <returns><paramref name="dictionary"/> 中的键值对的泛型枚举数。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
    public static IEnumerable<DictionaryEntry> GetEntries(this IDictionary dictionary)
    {
        if (dictionary is null)
        {
            throw new ArgumentNullException(nameof(dictionary));
        }

        static IEnumerable<DictionaryEntry> EnumerateCore(IDictionary dictionary)
        {
            var enumerator = dictionary.GetEnumerator();
            using (enumerator as IDisposable)
            {
                while (enumerator.MoveNext())
                {
                    yield return enumerator.Entry;
                }
            }
        }

        return EnumerateCore(dictionary);
    }
}
