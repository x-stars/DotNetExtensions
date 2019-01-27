using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    partial class GenericCollectionExtensions
    {
        /// <summary>
        /// 获取当前 <see cref="IEnumerable{T}"/> 对象中指定对象的数量。
        /// </summary>
        /// <typeparam name="T"><see cref="IEnumerable{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="value">要获取数量的对象。</param>
        /// <param name="comparer">指定用于进行相等比较的比较器。</param>
        /// <returns><paramref name="source"/> 中与
        /// <paramref name="value"/> 相等的对象的数量。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static int CountOf<T>(this IEnumerable<T> source, T value,
            IEqualityComparer<T> comparer = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            comparer = comparer ?? EqualityComparer<T>.Default;

            int count = 0;
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
        /// <param name="collection">要获取数量的多个对象。</param>
        /// <param name="comparer">指定用于进行相等比较的比较器。</param>
        /// <returns><paramref name="source"/> 中包含在
        /// <paramref name="collection"/> 中的对象的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static int CountOf<T>(this IEnumerable<T> source, IEnumerable<T> collection,
            IEqualityComparer<T> comparer = null)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            comparer = comparer ?? EqualityComparer<T>.Default;

            int count = 0;
            foreach (var item in source)
            {
                if (collection.Contains(item, comparer))
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
        /// <param name="collection">要获取数量的多个对象。</param>
        /// <returns><paramref name="source"/> 中包含在
        /// <paramref name="collection"/> 中的对象的数量。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public static int CountOf<T>(this IEnumerable<T> source, ICollection<T> collection)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            int count = 0;
            foreach (var item in source)
            {
                if (collection.Contains(item))
                {
                    count++;
                }
            }
            return count;
        }
    }
}
