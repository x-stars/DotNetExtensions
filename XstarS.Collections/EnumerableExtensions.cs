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
        /// 返回当前 <see cref="IEnumerable{T}"/> 对象的所有元素的字符串表达形式。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 中的元素的类型。</typeparam>
        /// <param name="source">要获取字符串表达形式的 <see cref="IEnumerable{T}"/> 对象。</param>
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
