using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    public static partial class Validate
    {
        /// <summary>
        /// 验证集合是否不为空集合。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值为一个空集合。
        /// </exception>
        public static IValidate<IEnumerable<T>> IsNotEmpty<T>(this IValidate<IEnumerable<T>> source,
            string message = null)
        {
            if (!source.Value.Any())
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证集合中元素的数量是否在指定范围内。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="minCount">集合允许的最小元素数量。</param>
        /// <param name="maxCount">集合允许的最大元素数量。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值的元素数量不在
        /// <paramref name="minCount"/> 和 <paramref name="maxCount"/> 之间。
        /// </exception>
        public static IValidate<IEnumerable<T>> CountIsInRange<T>(this IValidate<IEnumerable<T>> source,
            int minCount, int maxCount,
            string message = null)
        {
            if ((source.Value.Count() < minCount) || (source.Value.Count() > maxCount))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message, new IndexOutOfRangeException());
            }

            return source;
        }

        /// <summary>
        /// 验证集合中是否包含指定元素。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">集合中应该包含的元素。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不包含 <paramref name="value"/>。
        /// </exception>
        public static IValidate<IEnumerable<T>> Contains<T>(this IValidate<IEnumerable<T>> source,
            T value,
            string message = null)
        {
            if (!source.Value.Contains(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证集合中是否不包含指定元素。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">集合中不应包含的元素。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值包含 <paramref name="value"/>。
        /// </exception>
        public static IValidate<IEnumerable<T>> NotContains<T>(this IValidate<IEnumerable<T>> source,
            T value,
            string message = null)
        {
            if (source.Value.Contains(value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证集合是否不包含为 <see langword="null"/> 的元素。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值包含 <see langword="null"/>。
        /// </exception>
        public static IValidate<IEnumerable<T>> NotContainsNull<T>(this IValidate<IEnumerable<T>> source,
            string message = null)
            where T : class
        {
            return source.NotContains(null);
        }

        /// <summary>
        /// 验证集合是否不包含为 <see langword="null"/> 的元素。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的可空值类型元素的基础类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值包含 <see langword="null"/>。
        /// </exception>
        public static IValidate<IEnumerable<T?>> NotContainsNull<T>(this IValidate<IEnumerable<T?>> source,
            string message = null)
            where T : struct
        {
            return source.NotContains(null);
        }

        /// <summary>
        /// 验证集合中是否全部元素都满足指定条件。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="predicate">集合中的每个元素都应满足的条件。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值存在不满足
        /// <paramref name="predicate"/> 条件的元素。
        /// </exception>
        public static IValidate<IEnumerable<T>> AllMeets<T>(this IValidate<IEnumerable<T>> source,
            Predicate<T> predicate,
            string message = null)
        {
            if (source.Value.Any(item => !predicate(item)))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证集合中是否存在元素满足指定条件。
        /// </summary>
        /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="predicate">集合中至少一个元素应该满足的条件。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不存在满足
        /// <paramref name="predicate"/> 条件的元素。
        /// </exception>
        public static IValidate<IEnumerable<T>> AnyMeets<T>(this IValidate<IEnumerable<T>> source,
            Predicate<T> predicate,
            string message = null)
        {
            if (source.Value.All(item => !predicate(item)))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }
    }
}
