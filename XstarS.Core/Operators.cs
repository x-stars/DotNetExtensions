using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace XstarS
{
    /// <summary>
    /// 提供常用的运算符。
    /// </summary>
    /// <remarks>建议静态引入后直接以函数名称调用。</remarks>
    public static partial class Operators
    {
        /// <summary>
        /// 原样返回当前对象。
        /// </summary>
        /// <typeparam name="T">当前对象的类型。</typeparam>
        /// <param name="value">要原样返回的对象。</param>
        /// <returns><paramref name="value"/> 本身。</returns>
        [return: NotNullIfNotNull("value")]
        public static T Self<T>(T value) => value;

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
        /// 创建一个包含指定值的 <see cref="StrongBox{T}"/>。
        /// </summary>
        /// <typeparam name="T"><see cref="StrongBox{T}"/> 中值的类型。</typeparam>
        /// <param name="value">用于创建 <see cref="StrongBox{T}"/> 的值。</param>
        /// <returns>一个值为 <paramref name="value"/> 的 <see cref="StrongBox{T}"/>。</returns>
        public static StrongBox<T> BoxOf<T>(T value) => new StrongBox<T>(value);

        /// <summary>
        /// 创建一个包含指定键值对的 <see cref="Dictionary{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="Dictionary{TKey, TValue}"/> 中键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="Dictionary{TKey, TValue}"/> 中值的类型。</typeparam>
        /// <param name="pairs">用于创建 <see cref="Dictionary{TKey, TValue}"/> 的键值对。</param>
        /// <returns>一个包含 <paramref name="pairs"/>
        /// 中键值的 <see cref="Dictionary{TKey, TValue}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pairs"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pairs"/> 中存在具有相同键的键值对。</exception>
        public static Dictionary<TKey, TValue> DictOf<TKey, TValue>(
            params (TKey Key, TValue Value)[] pairs) where TKey : notnull
        {
            if (pairs is null) { throw new ArgumentNullException(nameof(pairs)); }
            var result = new Dictionary<TKey, TValue>(pairs.Length);
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
        /// 创建一个包含键值的 <see cref="KeyValuePair{TKey, TValue}"/>。
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
        /// 创建一个包含指定键值对的 <see cref="ReadOnlyDictionary{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey"><see cref="ReadOnlyDictionary{TKey, TValue}"/> 中键的类型。</typeparam>
        /// <typeparam name="TValue"><see cref="ReadOnlyDictionary{TKey, TValue}"/> 中值的类型。</typeparam>
        /// <param name="pairs">用于创建 <see cref="ReadOnlyDictionary{TKey, TValue}"/> 的键值对。</param>
        /// <returns>一个包含 <paramref name="pairs"/> 中键值对的
        /// <see cref="ReadOnlyDictionary{TKey, TValue}"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pairs"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="pairs"/> 中存在具有相同键的键值对。</exception>
        public static ReadOnlyDictionary<TKey, TValue> RoDictOf<TKey, TValue>(
            params (TKey Key, TValue Value)[] pairs) where TKey : notnull =>
            new ReadOnlyDictionary<TKey, TValue>(Operators.DictOf(pairs));

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
            new ReadOnlyCollection<T>(Operators.ListOf(items));

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

        /// <summary>
        /// 返回当前对象的字符串表示形式。
        /// </summary>
        /// <typeparam name="T">当前对象的类型。</typeparam>
        /// <param name="value">要返回字符串表示形式的对象。</param>
        /// <returns><paramref name="value"/> 的字符串表示形式。</returns>
        [return: NotNullIfNotNull("value")]
        public static string? StringOf<T>(T value) => value?.ToString();
    }
}
