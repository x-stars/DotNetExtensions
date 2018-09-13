using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供 <see cref="IEnumerable{T}"/> 的扩展方法的静态类。
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// 确定当前 <see cref="IEnumerable{T}"/> 对象
        /// 与另一个 <see cref="IEnumerable{T}"/> 对象中所包含的元素是否均相等。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要比较的源 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="other">要比较的另一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>如果 <paramref name="source"/> he <paramref name="other"/> 中的所有元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ElementsEquals<T>(this IEnumerable<T> source, IEnumerable<T> other) =>
                new CollectionEqualityComparer<T>().Equals(source, other);

        /// <summary>
        /// 根据当前 <see cref="IEnumerable{T}"/> 对象的所有元素生成哈希代码。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要生成哈希代码的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>所有元素生成的 32 位有符号整数哈希代码。</returns>
        public static int ElementsGetHashCode<T>(this IEnumerable<T> source)
        {
            if (source is null) { return 0; }

            int hashCode = new Random(source.GetType().Name.GetHashCode()).Next();
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in source)
            { hashCode = hashCode * -1521134295 + comparer.GetHashCode(item); }
            return hashCode;
        }

        /// <summary>
        /// 返回当前 <see cref="IEnumerable{T}"/> 对象的所有元素的字符串表达形式。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要获取字符串表达形式的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>所有元素的字符串表达形式。</returns>
        public static string ElementsToString<T>(this IEnumerable<T> source)
        {
            var collectionStringBuilder = new StringBuilder("{ ");
            foreach (var item in source)
            { collectionStringBuilder.Append($"{item.ToString()}, "); }
            collectionStringBuilder.Append("}");
            return collectionStringBuilder.ToString();
        }
    }
}
