using System;
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
        /// 返回当前 <see cref="IEnumerable"/> 对象的所有元素的字符串表达形式。
        /// </summary>
        /// <param name="source">一个 <see cref="IEnumerable"/> 对象。</param>
        /// <returns>所有元素的字符串表达形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static string SequenceToString(this IEnumerable source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var sequence = new List<string>();
            foreach (var item in source)
            {
                if (item is IEnumerable collection)
                {
                    sequence.Add(collection.SequenceToString());
                }
                else
                {
                    sequence.Add(item.ToString());
                }
            }
            return "{ " + string.Join(", ", sequence) + " }";
        }
    }
}
