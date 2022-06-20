using System;

namespace XNetEx;

static partial class Operators
{
    /// <summary>
    /// 返回一个长度等于指定 32 位有符号整数的指定类型的数组，
    /// 数组的每个元素由 <see cref="Func{T, TResult}"/> 转换索引得到。
    /// </summary>
    /// <typeparam name="T">要创建的数组的元素的类型。</typeparam>
    /// <param name="length">要创建的数组的长度。</param>
    /// <param name="indexMap">用于将索引转换到数组元素的转换器。</param>
    /// <returns>一个长度为 <paramref name="length"/> 的数组，
    /// 其中的每个元素由 <paramref name="indexMap"/> 转换索引得到。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="indexMap"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="length"/> 小于 0。</exception>
    public static T[] MakeArray<T>(int length, Func<int, T> indexMap)
    {
        if (length < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(length));
        }
        if (indexMap is null)
        {
            throw new ArgumentNullException(nameof(indexMap));
        }

        var result = new T[length];
        for (int index = 0; index < length; index++)
        {
            result[index] = indexMap(index);
        }
        return result;
    }

    /// <summary>
    /// 返回一个大小等于指定 32 位有符号整数二元组的指定类型的二维数组，
    /// 数组的每个元素由 <see cref="Func{T1, T2, TResult}"/> 转换索引得到。
    /// </summary>
    /// <typeparam name="T">要创建的数组的元素的类型。</typeparam>
    /// <param name="lengths">要创建的二维数组的大小。</param>
    /// <param name="indicesMap">用于将索引转换到数组元素的转换器。</param>
    /// <returns>一个大小为 <paramref name="lengths"/> 的数组，
    /// 其中的每个元素由 <paramref name="indicesMap"/> 转换索引得到。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="indicesMap"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="lengths"/> 中有元素小于 0。</exception>
    public static T[,] MakeArray<T>((int, int) lengths, Func<int, int, T> indicesMap)
    {
        var (length1, length2) = lengths;
        if ((length1 < 0) || (length2 < 0))
        {
            throw new ArgumentOutOfRangeException(nameof(lengths));
        }
        if (indicesMap is null)
        {
            throw new ArgumentNullException(nameof(indicesMap));
        }

        var result = new T[length1, length2];
        for (int index1 = 0; index1 < length1; index1++)
        {
            for (int index2 = 0; index2 < length2; index2++)
            {
                result[index1, index2] = indicesMap(index1, index2);
            }
        }
        return result;
    }

    /// <summary>
    /// 返回一个大小等于指定 32 位有符号整数三元组的指定类型的三维数组，
    /// 数组的每个元素由 <see cref="Func{T1, T2, T3, TResult}"/> 转换索引得到。
    /// </summary>
    /// <typeparam name="T">要创建的数组的元素的类型。</typeparam>
    /// <param name="lengths">要创建的三维数组的大小。</param>
    /// <param name="indicesMap">用于将索引转换到数组元素的转换器。</param>
    /// <returns>一个大小为 <paramref name="lengths"/> 的数组，
    /// 其中的每个元素由 <paramref name="indicesMap"/> 转换索引得到。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="indicesMap"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="lengths"/> 中有元素小于 0。</exception>
    public static T[,,] MakeArray<T>((int, int, int) lengths, Func<int, int, int, T> indicesMap)
    {
        var (length1, length2, length3) = lengths;
        if ((length1 < 0) || (length2 < 0) || (length3 < 0))
        {
            throw new ArgumentOutOfRangeException(nameof(lengths));
        }
        if (indicesMap is null)
        {
            throw new ArgumentNullException(nameof(indicesMap));
        }

        var result = new T[length1, length2, length3];
        for (int index1 = 0; index1 < length1; index1++)
        {
            for (int index2 = 0; index2 < length2; index2++)
            {
                for (int index3 = 0; index3 < length3; index3++)
                {
                    result[index1, index2, index3] = indicesMap(index1, index2, index3);
                }
            }
        }
        return result;
    }

    /// <summary>
    /// 返回一个大小等于指定 32 位有符号整数数组的指定类型的多维数组，
    /// 数组的每个元素由 <see cref="Func{T, TResult}"/> 转换索引得到。
    /// </summary>
    /// <typeparam name="T">要创建的数组的元素的类型。</typeparam>
    /// <param name="lengths">要创建的数组在各维度的长度。</param>
    /// <param name="indicesMap">用于将索引转换到数组元素的转换器。</param>
    /// <returns>大小为 <paramref name="lengths"/> 的多维数组，
    /// 其中的每个元素由 <paramref name="indicesMap"/> 转换索引得到。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="lengths"/> 或
    /// <paramref name="indicesMap"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="lengths"/> 中没有任何元素。</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="lengths"/> 中包含小于 0 的整数。</exception>
    public static Array MakeArray<T>(int[] lengths, Func<int[], T> indicesMap)
    {
        if (lengths is null)
        {
            throw new ArgumentNullException(nameof(lengths));
        }
        if (indicesMap is null)
        {
            throw new ArgumentNullException(nameof(indicesMap));
        }

        var result = Array.CreateInstance(typeof(T), lengths);
        foreach (var indices in result.GetIndicesSequence())
        {
            result.SetValue(indicesMap(indices), indices);
        }
        return result;
    }
}
