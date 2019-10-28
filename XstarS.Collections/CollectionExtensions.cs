﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供非泛型集合的扩展方法。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 枚举当前 <see cref="IEnumerable"/> 对象的所有元素，将递归至元素类型不可枚举。
        /// </summary>
        /// <param name="enumerable">要进行枚举的 <see cref="IEnumerable"/> 对象。</param>
        /// <returns><paramref name="enumerable"/> 包含和递归包含的所有不可枚举的元素。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable EnumerateEnumerable(this IEnumerable enumerable)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            foreach (var item in enumerable)
            {
                if (item is IEnumerable innerEnumerable)
                {
                    foreach (var innerItem in innerEnumerable.EnumerateEnumerable())
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

        /// <summary>
        /// 返回当前 <see cref="IEnumerable"/> 对象的所有元素的字符串表达形式。
        /// </summary>
        /// <param name="enumerable">要获取字符串表达形式的 <see cref="IEnumerable"/> 对象。</param>
        /// <returns><paramref name="enumerable"/> 的所有元素的字符串表达形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerable"/> 为 <see langword="null"/>。</exception>
        public static string SequenceToString(this IEnumerable enumerable)
        {
            if (enumerable is null)
            {
                throw new ArgumentNullException(nameof(enumerable));
            }

            var sequence = new List<string>();
            foreach (var item in enumerable)
            {
                if (item is IEnumerable innerEnumerable)
                {
                    sequence.Add(innerEnumerable.SequenceToString());
                }
                else
                {
                    sequence.Add(item?.ToString());
                }
            }
            return "{ " + string.Join(", ", sequence) + " }";
        }
    }
}