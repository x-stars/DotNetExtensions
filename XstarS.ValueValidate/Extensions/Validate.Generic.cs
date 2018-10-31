using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    public static partial class Validate
    {
        /// <summary>
        /// 验证对象是否等于指定值。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">对象应等于的值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不等于 <paramref name="value"/>。
        /// </exception>
        public static IValidate<T> EqualsTo<T>(this IValidate<T> source,
            T value,
            string message = null)
        {
            if (!EqualityComparer<T>.Default.Equals(source.Value, value))
            {
                ThrowHelper.ThrowArgumentNullException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否不等于指定值。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="value">对象不应等于的值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值等于 <paramref name="value"/>。
        /// </exception>
        public static IValidate<T> NotEqualsTo<T>(this IValidate<T> source,
            T value,
            string message = null)
        {
            if (EqualityComparer<T>.Default.Equals(source.Value, value))
            {
                ThrowHelper.ThrowArgumentNullException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否不等于 <see langword="null"/>。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 的值等于 <see langword="null"/>。
        /// </exception>
        public static IValidate<T> NotEqualsToNull<T>(this IValidate<T> source,
            string message = null)
        {
            if (object.Equals(source.Value, null))
            {
                ThrowHelper.ThrowArgumentNullException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否为指定类型的实例。
        /// </summary>
        /// <typeparam name="T">对象期望的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不为 <typeparamref name="T"/> 类型的实例。
        /// </exception>
        public static IValidate<object> IsInstanceOf<T>(this IValidate<object> source,
            string message = null)
        {
            if (!(source.Value is T))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message,
                    new InvalidCastException());
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否为指定类型的实例。
        /// </summary>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="type">对象期望的类型。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 的值不为 <paramref name="type"/> 类型的实例。
        /// </exception>
        public static IValidate<object> IsInstanceOf(this IValidate<object> source,
            Type type,
            string message = null)
        {
            if (!type.IsInstanceOfType(source.Value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message,
                    new InvalidCastException());
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否满足条件。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="condition">对象应满足的条件。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="condition"/> 返回 <see langword="false"/>。
        /// </exception>
        public static IValidate<T> Meets<T>(this IValidate<T> source,
            bool condition,
            string message = null)
        {
            if (condition)
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否满足条件。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="predicate">对象应满足的条件。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="predicate"/> 返回 <see langword="false"/>。
        /// </exception>
        public static IValidate<T> Meets<T>(this IValidate<T> source,
            Predicate<T> predicate,
            string message = null)
        {
            if (predicate(source.Value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否包含在指定集合中。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="collection">应该包含当前对象的集合。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="KeyNotFoundException">
        /// <paramref name="source"/> 的值不在 <paramref name="collection"/> 中。
        /// </exception>
        public static IValidate<T> IsInCollection<T>(this IValidate<T> source,
            IEnumerable<T> collection,
            string message = null)
        {
            if (!collection.Contains(source.Value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return null;
        }

        /// <summary>
        /// 验证对象是否不包含在指定集合中。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="collection">不应包含当前对象的集合。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="KeyNotFoundException">
        /// <paramref name="source"/> 的值在 <paramref name="collection"/> 中。
        /// </exception>
        public static IValidate<T> IsNotInCollection<T>(this IValidate<T> source,
            IEnumerable<T> collection,
            string message = null)
        {
            if (!collection.Contains(source.Value))
            {
                ThrowHelper.ThrowArgumentException(source.Name, message);
            }

            return null;
        }

        /// <summary>
        /// 验证对象是否包含在键的集合中。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="keys">应包含当前对象的键的集合。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="KeyNotFoundException">
        /// <paramref name="source"/> 的值不在 <paramref name="keys"/> 中。
        /// </exception>
        public static IValidate<T> IsInKeys<T>(this IValidate<T> source,
            IEnumerable<T> keys,
            string message = null)
        {
            if (!keys.Contains(source.Value))
            {
                ThrowHelper.ThrowKeyNotFoundException(message);
            }

            return source;
        }

        /// <summary>
        /// 验证对象是否在指定范围内。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="source">一个 <see cref="Validate{T}"/> 对象。</param>
        /// <param name="minValue">允许的最小值。</param>
        /// <param name="maxValue">允许的最大值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="source"/> 本身。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> 的值不在
        /// <paramref name="minValue"/> 和 <paramref name="maxValue"/> 之间。
        /// </exception>
        public static IValidate<T> IsInRange<T>(this IValidate<T> source,
            T minValue, T maxValue,
            string message = null)
            where T : IComparable<T>
        {
            if ((source.Value.CompareTo(minValue) < 0) ||
                (source.Value.CompareTo(maxValue) > 0))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(source.Name, source.Value, message);
            }

            return source;
        }
    }
}
