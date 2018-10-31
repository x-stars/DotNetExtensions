using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供公开枚举数 <see cref="IEnumerable{T}"/> 的扩展方法。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 获取当前 <see cref="IEnumerable{T}"/> 对象中指定对象的数量。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="value">要获取数量的对象。</param>
        /// <param name="comparer">指定用于进行相等比较的比较器。</param>
        /// <returns><paramref name="source"/> 中与 <paramref name="value"/> 相等的对象的数量。</returns>
        public static int CountOf<T>(this IEnumerable<T> source, T value,
            IEqualityComparer<T> comparer = null)
        {
            int count = 0;
            comparer = comparer ?? EqualityComparer<T>.Default;
            foreach (var item in source)
            {
                if (comparer.Equals(item, value))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 获取当前 <see cref="IEnumerable{T}"/> 对象中多个指定对象的数量。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="values">要获取数量的多个对象。</param>
        /// <param name="comparer">指定用于进行相等比较的比较器。</param>
        /// <returns><paramref name="source"/> 中包含在 <paramref name="values"/> 中的对象的数量。</returns>
        public static int CountOf<T>(this IEnumerable<T> source, IEnumerable<T> values,
            IEqualityComparer<T> comparer = null)
        {
            int count = 0;
            comparer = comparer ?? EqualityComparer<T>.Default;
            foreach (var item in source)
            {
                if (values.Contains(item, comparer))
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 返回当前 <see cref="IEnumerable{T}"/> 对象的所有元素的字符串表达形式。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>所有元素的字符串表达形式。</returns>
        public static string SequenceToString<T>(this IEnumerable<T> source)
        {
            var collectionStringBuilder = new StringBuilder("{ ");
            foreach (var item in source) { collectionStringBuilder.Append($"{item.ToString()}, "); }
            collectionStringBuilder.Remove(collectionStringBuilder.Length - ", ".Length, ", ".Length);
            collectionStringBuilder.Append(" }");
            return collectionStringBuilder.ToString();
        }
    }
}
