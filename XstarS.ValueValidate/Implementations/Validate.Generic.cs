using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    partial class Validate
    {
        /// <summary>
        /// 验证对象是否等于指定值。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">对象应等于的值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不等于 <paramref name="value"/>。</exception>
        public static IValueInfo<T> EqualsTo<T>(
            this IValueInfo<T> valueInfo, T value, string message = null)
        {
            if (!EqualityComparer<T>.Default.Equals(valueInfo.Value, value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否不等于指定值。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="value">对象不应等于的值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值等于 <paramref name="value"/>。</exception>
        public static IValueInfo<T> NotEqualsTo<T>(
            this IValueInfo<T> valueInfo, T value, string message = null)
        {
            if (EqualityComparer<T>.Default.Equals(valueInfo.Value, value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否不等于 <see langword="null"/>。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="valueInfo"/> 的值等于 <see langword="null"/>。</exception>
        public static IValueInfo<T> NotEqualsToNull<T>(
            this IValueInfo<T> valueInfo, string message = null)
        {
            if (object.Equals(valueInfo.Value, null))
            {
                ThrowHelper.ThrowArgumentNullException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否为指定类型的实例。
        /// </summary>
        /// <typeparam name="T">对象期望的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不为 <typeparamref name="T"/> 类型的实例。</exception>
        public static IValueInfo<object> IsInstanceOf<T>(
            this IValueInfo<object> valueInfo, string message = null)
        {
            if (!(valueInfo.Value is T))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message,
                    new InvalidCastException());
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否为指定类型的实例。
        /// </summary>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="type">对象期望的类型。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不为 <paramref name="type"/> 类型的实例。</exception>
        public static IValueInfo<object> IsInstanceOf(
            this IValueInfo<object> valueInfo, Type type, string message = null)
        {
            if (!type.IsInstanceOfType(valueInfo.Value))
            {
                ThrowHelper.ThrowArgumentException(
                    valueInfo.Name, message, new InvalidCastException());
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否满足条件。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="condition">对象应满足的条件。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="condition"/> 返回 <see langword="false"/>。</exception>
        public static IValueInfo<T> MatchesCondition<T>(
            this IValueInfo<T> valueInfo, bool condition, string message = null)
        {
            if (condition)
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否满足条件。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="predicate">对象应满足的条件。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="predicate"/> 返回 <see langword="false"/>。</exception>
        public static IValueInfo<T> MatchesCondition<T>(
            this IValueInfo<T> valueInfo, Predicate<T> predicate, string message = null)
        {
            if (predicate(valueInfo.Value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否包含在指定集合中。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="collection">应该包含当前对象的集合。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值不在 <paramref name="collection"/> 中。</exception>
        public static IValueInfo<T> IsInCollection<T>(
            this IValueInfo<T> valueInfo, IEnumerable<T> collection, string message = null)
        {
            if (!collection.Contains(valueInfo.Value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否不包含在指定集合中。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="collection">不应包含当前对象的集合。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="valueInfo"/> 的值在 <paramref name="collection"/> 中。</exception>
        public static IValueInfo<T> IsNotInCollection<T>(
            this IValueInfo<T> valueInfo, IEnumerable<T> collection, string message = null)
        {
            if (!collection.Contains(valueInfo.Value))
            {
                ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否包含在键的集合中。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="keyCollection">应包含当前对象的键的集合。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="KeyNotFoundException">
        /// <paramref name="valueInfo"/> 的值不在 <paramref name="keyCollection"/> 中。</exception>
        public static IValueInfo<T> IsInKeys<T>(
            this IValueInfo<T> valueInfo, IEnumerable<T> keyCollection, string message = null)
        {
            if (!keyCollection.Contains(valueInfo.Value))
            {
                ThrowHelper.ThrowKeyNotFoundException(message);
            }

            return valueInfo;
        }

        /// <summary>
        /// 验证对象是否在指定范围内。
        /// </summary>
        /// <typeparam name="T">待验证的对象的类型。</typeparam>
        /// <param name="valueInfo">要验证的对象的值和名称。</param>
        /// <param name="minValue">允许的最小值。</param>
        /// <param name="maxValue">允许的最大值。</param>
        /// <param name="message">自定义抛出异常的消息。</param>
        /// <returns><paramref name="valueInfo"/> 本身。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="valueInfo"/> 的值不在
        /// <paramref name="minValue"/> 和 <paramref name="maxValue"/> 之间。</exception>
        public static IValueInfo<T> IsInRange<T>(
            this IValueInfo<T> valueInfo, T minValue, T maxValue, string message = null)
            where T : IComparable<T>
        {
            if ((valueInfo.Value.CompareTo(minValue) < 0) ||
                (valueInfo.Value.CompareTo(maxValue) > 0))
            {
                ThrowHelper.ThrowArgumentOutOfRangeException(
                    valueInfo.Name, valueInfo.Value, message);
            }

            return valueInfo;
        }
    }
}
