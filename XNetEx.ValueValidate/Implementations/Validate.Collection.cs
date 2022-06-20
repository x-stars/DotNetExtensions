using System;
using System.Collections.Generic;
using System.Linq;

namespace XNetEx;

static partial class Validate
{
    /// <summary>
    /// 验证集合是否为空集合。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="valueInfo"/> 的值不为一个空集合。</exception>
    public static IValueInfo<IEnumerable<T>> IsEmpty<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, string? message = null)
    {
        if (valueInfo.Value.Any())
        {
            ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
        }

        return valueInfo;
    }

    /// <summary>
    /// 验证集合是否不为空集合。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="valueInfo"/> 的值为一个空集合。</exception>
    public static IValueInfo<IEnumerable<T>> IsNotEmpty<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, string? message = null)
    {
        if (!valueInfo.Value.Any())
        {
            ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
        }

        return valueInfo;
    }

    /// <summary>
    /// 验证集合中元素的数量是否在指定范围内。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="minCount">集合允许的最小元素数量。</param>
    /// <param name="maxCount">集合允许的最大元素数量。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException"><paramref name="valueInfo"/> 的值的元素数量不在
    /// <paramref name="minCount"/> 和 <paramref name="maxCount"/> 之间。</exception>
    public static IValueInfo<IEnumerable<T>> CountIsInRange<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, int minCount, int maxCount, string? message = null)
    {
        if ((valueInfo.Value.Count() < minCount) || (valueInfo.Value.Count() > maxCount))
        {
            ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
        }

        return valueInfo;
    }

    /// <summary>
    /// 验证集合中是否包含指定元素。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="value">集合中应该包含的元素。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="valueInfo"/> 的值不包含 <paramref name="value"/>。</exception>
    public static IValueInfo<IEnumerable<T>> Contains<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, T value, string? message = null)
    {
        if (!valueInfo.Value.Contains(value))
        {
            ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
        }

        return valueInfo;
    }

    /// <summary>
    /// 验证集合中是否不包含指定元素。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="value">集合中不应包含的元素。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="valueInfo"/> 的值包含 <paramref name="value"/>。</exception>
    public static IValueInfo<IEnumerable<T>> NotContains<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, T value, string? message = null)
    {
        if (valueInfo.Value.Contains(value))
        {
            ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
        }

        return valueInfo;
    }

    /// <summary>
    /// 验证集合是否不包含为 <see langword="null"/> 的元素。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="valueInfo"/> 的值包含 <see langword="null"/>。</exception>
    public static IValueInfo<IEnumerable<T>> NotContainsNull<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, string? message = null)
        where T : class
    {
        return valueInfo.NotContains(null!, message);
    }

    /// <summary>
    /// 验证集合是否不包含为 <see langword="null"/> 的元素。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的可空值类型元素的基础类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException">
    /// <paramref name="valueInfo"/> 的值包含 <see langword="null"/>。</exception>
    public static IValueInfo<IEnumerable<T?>> NotContainsNull<T>(
        this IValueInfo<IEnumerable<T?>> valueInfo, string? message = null)
        where T : struct
    {
        return valueInfo.NotContains(null, message);
    }

    /// <summary>
    /// 验证集合中是否全部元素都满足指定条件。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="predicate">集合中的每个元素都应满足的条件。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException"><paramref name="valueInfo"/>
    /// 的值存在不满足<paramref name="predicate"/> 条件的元素。</exception>
    public static IValueInfo<IEnumerable<T>> AllMatch<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, Predicate<T> predicate, string? message = null)
    {
        if (!valueInfo.Value.All(item => predicate(item)))
        {
            ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
        }

        return valueInfo;
    }

    /// <summary>
    /// 验证集合中是否存在元素满足指定条件。
    /// </summary>
    /// <typeparam name="T">待验证的集合中的元素的类型。</typeparam>
    /// <param name="valueInfo">要验证的对象的值和名称。</param>
    /// <param name="predicate">集合中至少一个元素应该满足的条件。</param>
    /// <param name="message">自定义抛出异常的消息。</param>
    /// <returns><paramref name="valueInfo"/> 本身。</returns>
    /// <exception cref="ArgumentException"><paramref name="valueInfo"/>
    /// 的值不存在满足 <paramref name="predicate"/> 条件的元素。</exception>
    public static IValueInfo<IEnumerable<T>> AnyMatches<T>(
        this IValueInfo<IEnumerable<T>> valueInfo, Predicate<T> predicate, string? message = null)
    {
        if (!valueInfo.Value.Any(item => predicate(item)))
        {
            ThrowHelper.ThrowArgumentException(valueInfo.Name, message);
        }

        return valueInfo;
    }
}
