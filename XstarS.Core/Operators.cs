using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace XstarS
{
    /// <summary>
    /// 提供常用的运算符。
    /// </summary>
    /// <remarks>建议静态引入后直接以函数名称调用。</remarks>
    public static class Operators
    {
        /// <summary>
        /// 创建一个包含指定元素的数组。
        /// </summary>
        /// <typeparam name="T">数组中元素的类型。</typeparam>
        /// <param name="items">用于创建数组的元素。</param>
        /// <returns>一个包含 <paramref name="items"/> 中元素的数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static T[] ArrayOf<T>(params T[] items) =>
            (T[])(items ?? throw new ArgumentNullException(nameof(items))).Clone();

        /// <summary>
        /// 创建一个包含指定键值的 <see cref="Dictionary{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="Dictionary{TKey, TValue}"/> 中键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="Dictionary{TKey, TValue}"/> 中值的类型。</typeparam>
        /// <param name="pairs">用于创建 <see cref="Dictionary{TKey, TValue}"/> 的键值。</param>
        /// <returns>一个包含 <paramref name="pairs"/>
        /// 中键值的 <see cref="Dictionary{TKey, TValue}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pairs"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pairs"/> 中存在具有相同键的键值。</exception>
        public static Dictionary<TKey, TValue> DictOf<TKey, TValue>(
            params (TKey Key, TValue Value)[] pairs)
        {
            if (pairs is null) { throw new ArgumentNullException(nameof(pairs)); }
            var result = new Dictionary<TKey, TValue>();
            foreach (var (key, value) in pairs) { result.Add(key, value); }
            return result;
        }

        /// <summary>
        /// 创建一个包含指定元素的 <see cref="List{T}"/>。
        /// </summary>
        /// <typeparam name="T"><see cref="List{T}"/> 中元素的类型。</typeparam>
        /// <param name="items">要创建 <see cref="List{T}"/> 的元素。</param>
        /// <returns>一个包含 <paramref name="items"/> 中元素的 <see cref="List{T}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static List<T> ListOf<T>(params T[] items) =>
            new List<T>(items ?? throw new ArgumentNullException(nameof(items)));

        /// <summary>
        /// 创建一个键值为指定值的 <see cref="KeyValuePair{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="KeyValuePair{TKey, TValue}"/> 中键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="KeyValuePair{TKey, TValue}"/> 中值的类型。</typeparam>
        /// <param name="key">用于创建 <see cref="KeyValuePair{TKey, TValue}"/> 的键。</param>
        /// <param name="value">用于创建 <see cref="KeyValuePair{TKey, TValue}"/> 的值。</param>
        /// <returns>一个键为 <paramref name="key"/> 值为 <paramref name="value"/>
        /// 的 <see cref="KeyValuePair{TKey, TValue}"/>。</returns>
        public static KeyValuePair<TKey, TValue> PairOf<TKey, TValue>(TKey key, TValue value) =>
            new KeyValuePair<TKey, TValue>(key, value);

        /// <summary>
        /// 创建一个包含指定键值的 <see cref="ReadOnlyDictionary{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="ReadOnlyDictionary{TKey, TValue}"/> 中键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="ReadOnlyDictionary{TKey, TValue}"/> 中值的类型。</typeparam>
        /// <param name="pairs">用于创建 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 的键值。</param>
        /// <returns>一个包含 <paramref name="pairs"/> 中键值的
        /// <see cref="ReadOnlyDictionary{TKey, TValue}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pairs"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pairs"/> 中存在具有相同键的键值。</exception>
        public static ReadOnlyDictionary<TKey, TValue> RoDictOf<TKey, TValue>(
            params (TKey Key, TValue Value)[] pairs) =>
            new ReadOnlyDictionary<TKey, TValue>(DictOf(pairs));

        /// <summary>
        /// 创建一个包含指定元素的 <see cref="ReadOnlyCollection{T}"/>。
        /// </summary>
        /// <typeparam name="T"><see cref="ReadOnlyCollection{T}"/> 中元素的类型。</typeparam>
        /// <param name="items">要创建 <see cref="ReadOnlyCollection{T}"/> 的元素。</param>
        /// <returns>一个包含 <paramref name="items"/> 中元素的
        /// <see cref="ReadOnlyCollection{T}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static ReadOnlyCollection<T> RoListOf<T>(params T[] items) =>
            new ReadOnlyCollection<T>(ListOf(items));

        /// <summary>
        /// 创建一个包含指定元素的 <see cref="HashSet{T}"/>。
        /// </summary>
        /// <typeparam name="T"><see cref="HashSet{T}"/> 中元素的类型。</typeparam>
        /// <param name="items">要创建 <see cref="HashSet{T}"/> 的元素。</param>
        /// <returns>一个包含 <paramref name="items"/> 中元素的 <see cref="HashSet{T}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="items"/> 为 <see langword="null"/>。</exception>
        public static HashSet<T> SetOf<T>(params T[] items) =>
            new HashSet<T>(items ?? throw new ArgumentNullException(nameof(items)));
    }
}
