using System;
using System.Collections.Generic;

namespace XNetEx;

/// <summary>
/// 提供数组的扩展方法。
/// </summary>
public static class ArrayExtensions
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

    /// <summary>
    /// 返回将当前值类型数组中的每个值装箱后得到的新数组。
    /// </summary>
    /// <typeparam name="T">要装箱的值的类型。</typeparam>
    /// <param name="array">要将每个值装箱的数组。</param>
    /// <returns>将 <paramref name="array"/> 中的每个值装箱后得到的新数组。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public static object[] Box<T>(this T[] array) where T : struct
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var length = array.Length;
        var result = new object[length];
        for (int index = 0; index < length; index++)
        {
            result[index] = array[index];
        }
        return result;
    }

    /// <summary>
    /// 返回将当前可空值类型数组中的每个值装箱后得到的新数组。
    /// </summary>
    /// <typeparam name="T">要装箱的值的类型。</typeparam>
    /// <param name="array">要将每个值装箱的数组。</param>
    /// <returns>将 <paramref name="array"/> 中的每个值装箱后得到的新数组。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public static object?[] Box<T>(this T?[] array) where T : struct
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var length = array.Length;
        var result = new object?[length];
        for (int index = 0; index < length; index++)
        {
            result[index] = array[index];
        }
        return result;
    }

    /// <summary>
    /// 返回一个新数组，此数组为当前数组和指定数组连接后的结果。
    /// </summary>
    /// <typeparam name="T"><paramref name="array"/> 中元素的类型。</typeparam>
    /// <param name="array">要进行连接的数组。</param>
    /// <param name="other">要与当前数组连接的数组。</param>
    /// <returns>一个新数组，此数组为当前数组和指定数组连接后的结果。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/>
    /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
    public static T[] Concat<T>(this T[] array, T[] other)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (other is null)
        {
            throw new ArgumentNullException(nameof(other));
        }

        var result = new T[array.Length + other.Length];
        Array.Copy(array, 0, result, 0, array.Length);
        Array.Copy(other, 0, result, array.Length, other.Length);
        return result;
    }

    /// <summary>
    /// 返回一个新数组，此数组为当前交错数组中包含的数组顺序连接后的结果。
    /// </summary>
    /// <typeparam name="T"><paramref name="arrays"/> 包含的数组中元素的类型。</typeparam>
    /// <param name="arrays">要将内层数组顺序相连的数组。</param>
    /// <returns>一个新数组，此数组为指定交错数组中包含的数组顺序连接后的结果。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="arrays"/> 为 <see langword="null"/>。</exception>
    public static T[] Concat<T>(this T[][] arrays)
    {
        if (arrays is null)
        {
            throw new ArgumentNullException(nameof(arrays));
        }

        int length = 0;
        foreach (var item in arrays)
        {
            var array = item ?? Array.Empty<T>();
            length += array.Length;
        }

        var result = new T[length];
        int offset = 0;
        foreach (var item in arrays)
        {
            var array = item ?? Array.Empty<T>();
            Array.Copy(array, 0, result, offset, array.Length);
            offset += array.Length;
        }
        return result;
    }

    /// <summary>
    /// 获取当前多维数组的索引数组的 <see cref="IEnumerable{T}"/> 序列。
    /// </summary>
    /// <param name="array">要获取索引数组序列的多维数组。</param>
    /// <param name="reuseIndices">指定序列返回的索引数组是否复用同一数组。</param>
    /// <returns><paramref name="array"/> 的索引数组的
    /// <see cref="IEnumerable{T}"/> 序列。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public static IEnumerable<int[]> GetIndicesSequence(
        this Array array, bool reuseIndices = false)
    {
        return new ArrayIndicesSequence(array, reuseIndices);
    }

    /// <summary>
    /// 返回一个新数组，此数组为当前数组指定位置插入指定元素后的结果。
    /// </summary>
    /// <typeparam name="T"><paramref name="array"/> 中元素的类型。</typeparam>
    /// <param name="array">要插入元素的数组。</param>
    /// <param name="index">要插入元素的位置的索引。</param>
    /// <param name="item">要插入到当前数组的元素。</param>
    /// <returns>一个新数组，此数组为 <paramref name="array"/>
    /// 在 <paramref name="index"/> 位置擦汗如 <paramref name="item"/> 后的结果。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>
    /// 小于 0，或者大于或等于 <paramref name="array"/> 的长度。</exception>
    public static T[] Insert<T>(this T[] array, int index, T item)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (index < 0 || index > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var result = new T[array.Length + 1];
        Array.Copy(array, 0, result, 0, index);
        result[index] = item;
        Array.Copy(array, index, result, index + 1, array.Length - index);
        return result;
    }

    /// <summary>
    /// 返回一个新数组，此数组为当前数组指定位置插入另一个数组后的结果。
    /// </summary>
    /// <typeparam name="T"><paramref name="array"/> 中元素的类型。</typeparam>
    /// <param name="array">要插入另一个数组的数组。</param>
    /// <param name="index">要插入另一个数组的位置的索引。</param>
    /// <param name="items">要插入到当前数组的另一个数组。</param>
    /// <returns>一个新数组，此数组为 <paramref name="array"/>
    /// 在 <paramref name="index"/> 位置插入 <paramref name="items"/> 后的结果。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="array"/>
    /// 或 <paramref name="items"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/>
    /// 小于 0，或者大于或等于 <paramref name="array"/> 的长度。</exception>
    public static T[] InsertRange<T>(this T[] array, int index, T[] items)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }
        if (index < 0 || index > array.Length)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        var result = new T[array.Length + items.Length];
        Array.Copy(array, 0, result, 0, index);
        Array.Copy(items, 0, result, index, items.Length);
        Array.Copy(array, index, result, index + items.Length, array.Length - index);
        return result;
    }

    /// <summary>
    /// 确定当前数组是否为一个下限为零的一维数组。
    /// </summary>
    /// <param name="array">要进行判断的数组。</param>
    /// <returns>若 <paramref name="array"/> 为下限为零的一维数组，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public static bool IsSZArray(this Array array)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var type = array.GetType();
        var itemType = type.GetElementType()!;
        return type == itemType.MakeArrayType();
    }

    /// <summary>
    /// 将当前数组的指定偏移量转换为对应的多维索引。
    /// </summary>
    /// <param name="array">要获取索引的数组。</param>
    /// <param name="offset">要获取对应多维索引的偏移量。</param>
    /// <returns><paramref name="array"/> 偏移量为
    /// <paramref name="offset"/> 的元素对应的多维索引。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> 不在 0 和
    /// <paramref name="array"/> 的 <see cref="Array.Length"/> 之间。</exception>
    public static int[] OffsetToIndices(this Array array, int offset)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }
        if ((offset < 0) || (offset >= array.Length))
        {
            throw new ArgumentOutOfRangeException(nameof(offset));
        }

        var rank = array.Rank;
        var result = new int[rank];
        var scale = array.Length;
        for (int dim = 0; dim < rank; dim++)
        {
            var start = array.GetLowerBound(rank);
            scale /= array.GetLength(rank);
            result[rank] = start + offset / scale;
            offset %= scale;
        }
        return result;
    }

    /// <summary>
    /// 返回一个新数组，此数组为当前数组开头添加指定元素后的结果。
    /// </summary>
    /// <typeparam name="T"><paramref name="array"/> 中元素的类型。</typeparam>
    /// <param name="array">要添加元素的数组。</param>
    /// <param name="item">要添加到当前数组的元素。</param>
    /// <returns>一个新数组，此数组为 <paramref name="array"/>
    /// 在开头添加 <paramref name="item"/> 后的结果。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public static T[] Prepend<T>(this T[] array, T item)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var result = new T[array.Length + 1];
        result[0] = item;
        Array.Copy(array, 0, result, 1, array.Length);
        return result;
    }

    /// <summary>
    /// 返回将当前数组中元素的顺序反转后得到的新数组。
    /// </summary>
    /// <typeparam name="T">数组的元素的类型。</typeparam>
    /// <param name="array">要反转元素的顺序的数组。</param>
    /// <returns>将当前数组中元素的顺序反转后得到的新数组。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
    public static T[] Reverse<T>(this T[] array)
    {
        if (array is null)
        {
            throw new ArgumentNullException(nameof(array));
        }

        var length = array.Length;
        var result = new T[length];
        Array.Copy(array, result, length);
        Array.Reverse(result);
        return result;
    }
}
