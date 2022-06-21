using System;

namespace XNetEx;

/// <summary>
/// 提供数组的扩展方法。
/// </summary>
internal static class ArrayExtensions
{
    /// <summary>
    /// 返回一个新数组，此数组为当前数组末尾追加指定元素后的结果。
    /// </summary>
    /// <typeparam name="T"><paramref name="array"/> 中元素的类型。</typeparam>
    /// <param name="array">要追加元素的数组。</param>
    /// <param name="item">要追加到当前数组的元素。</param>
    /// <returns>一个新数组，此数组为 <paramref name="array"/>
    /// 在末尾追加 <paramref name="item"/> 后的结果。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public static T[] Append<T>(this T[] array, T item)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var result = new T[array.Length + 1];
        Array.Copy(array, 0, result, 0, array.Length);
        result[array.Length] = item;
        return result;
    }
}
