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
        /// 确定当前 <see cref="IEnumerable"/> 与指定 <see cref="IEnumerable"/> 的元素是否对应相等。
        /// </summary>
        /// <param name="enumerable">要进行相等比较的 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="other">要与当前集合进行相等比较的 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="recurse">指示是否对内层 <see cref="IEnumerable"/> 对象递归。</param>
        /// <param name="comparer">用于比较集合元素的比较器。</param>
        /// <returns>若 <paramref name="enumerable"/> 与 <paramref name="other"/> 的每个元素都对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool CollectionEquals(this IEnumerable enumerable, IEnumerable other,
            bool recurse = false, IEqualityComparer comparer = null)
        {
            return new CollectionEqualityComparer(recurse, comparer).Equals(enumerable, other);
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

            var sequence = new List<string>();
            foreach (var item in enumerable)
            {
                if (recurse && (item is IEnumerable innerEnum))
                {
                    sequence.Add(innerEnum.CollectionToString(recurse));
                }
                else
                {
                    sequence.Add(item?.ToString());
                }
            }
            return "{ " + string.Join(", ", sequence) + " }";
        }

        /// <summary>
        /// 获取当前 <see cref="IEnumerable"/> 遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="enumerable">要为其获取哈希代码的 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="recurse">指示是否对内层 <see cref="IEnumerable"/> 对象递归。</param>
        /// <param name="comparer">用于获取集合元素的哈希代码的比较器。</param>
        /// <returns>遍历 <paramref name="enumerable"/> 的元素得到的哈希代码。</returns>
        public static int GetCollectionHashCode(this IEnumerable enumerable,
            bool recurse = false, IEqualityComparer comparer = null)
        {
            return new CollectionEqualityComparer(recurse, comparer).GetHashCode(enumerable);
        }

        /// <summary>
        /// 枚举当前 <see cref="IEnumerable"/> 对象的所有元素，将递归至元素类型不可枚举。
        /// </summary>
        /// <param name="enumerable">要进行枚举的 <see cref="IEnumerable"/> 对象。</param>
        /// <returns><paramref name="enumerable"/> 包含和递归包含的所有不可枚举的元素。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable RecurseEnumerate(this IEnumerable enumerable)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            foreach (var item in enumerable)
            {
                if (item is IEnumerable innerEnumerable)
                {
                    foreach (var innerItem in innerEnumerable.RecurseEnumerate())
                    {
                        yield return innerItem;
                    }
                }
                else
                {
                    yield return item;
                }
            }
        }
    }
}
