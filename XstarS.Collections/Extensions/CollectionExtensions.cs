using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstarS.Collections.Generic.Extensions
{
    /// <summary>
    /// 提供泛型集合 <see cref="ICollection{T}"/> 的扩展方法。
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// 将指定集合的元素添加到 <see cref="ICollection{T}"/> 中。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="collection">
        /// 应将其元素添加到 <see cref="ICollection{T}"/> 中的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            foreach (var item in collection) { source.Add(item); }
        }

        /// <summary>
        /// 从 <see cref="ICollection{T}"/> 中移除所有指定集合中的元素。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="collection">应从 <see cref="ICollection{T}"/> 中移除的元素的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static void RemoveRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            foreach (var item in collection) { source.Remove(item); }
        }
    }
}
