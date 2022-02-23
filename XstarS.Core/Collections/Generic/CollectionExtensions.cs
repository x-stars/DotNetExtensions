using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供泛型集合的扩展方法。
    /// </summary>
    public static partial class CollectionExtensions
    {
        /// <summary>
        /// 返回 <see cref="IEnumerator{T}"/> 的公开枚举数包装。
        /// </summary>
        /// <typeparam name="T">要枚举的对象的类型。</typeparam>
        /// <param name="enumerator">要包装的 <see cref="IEnumerator{T}"/> 对象。</param>
        /// <returns><paramref name="enumerator"/> 的 <see cref="IEnumerable{T}"/> 包装。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<T> AsEnumerable<T>(IEnumerator<T> enumerator)
        {
            if (enumerator is null)
            {
                throw new ArgumentNullException(nameof(enumerator));
            }

            return new EnumeratorEnumerable<T>(enumerator);
        }

        /// <summary>
        /// 解构当前 <see cref="KeyValuePair{TKey, TValue}"/>。
        /// </summary>
        /// <typeparam name="TKey">当前 <see cref="KeyValuePair{TKey, TValue}"/> 的键的类型。</typeparam>
        /// <typeparam name="TValue">当前 <see cref="KeyValuePair{TKey, TValue}"/> 的值的类型。</typeparam>
        /// <param name="pair">要解构的 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
        /// <param name="key">当前 <see cref="KeyValuePair{TKey, TValue}"/> 的键。</param>
        /// <param name="value">当前 <see cref="KeyValuePair{TKey, TValue}"/> 的值。</param>
        public static void Deconstruct<TKey, TValue>(
            this KeyValuePair<TKey, TValue> pair, out TKey key, out TValue value)
        {
            key = pair.Key;
            value = pair.Value;
        }

        /// <summary>
        /// 反转 <see cref="IComparer{T}"/> 的比较顺序并返回。
        /// </summary>
        /// <typeparam name="T"><see cref="IComparer{T}"/> 要比较的对象的类型。</typeparam>
        /// <param name="comparer">要进行反转的 <see cref="IComparer{T}"/> 对象。</param>
        /// <returns>将 <paramref name="comparer"/> 反转后得到的 <see cref="IComparer{T}"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comparer"/> 为 <see langword="null"/>。</exception>
        public static IComparer<T> Reverse<T>(this IComparer<T> comparer)
        {
            if (comparer is null)
            {
                throw new ArgumentNullException(nameof(comparer));
            }

            return (comparer is ReversedComparer<T> reversedComparer) ?
                reversedComparer.BaseComparer : new ReversedComparer<T>(comparer);
        }
    }
}
