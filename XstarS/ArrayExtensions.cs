using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS
{
    /// <summary>
    /// 提供数组的扩展方法。
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// 获取数组中元素的声明类型。对于交错数组，将递归至数组元素的声明类型不为数组。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <returns><paramref name="source"/> 中元素的声明类型。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static Type GetElementTypeRecursive(this Array source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var itemType = source.GetType().GetElementType();
            while (itemType.IsArray)
            {
                itemType = itemType.GetElementType();
            }
            return itemType;
        }

        /// <summary>
        /// 将当前数组的指定偏移量转换为对应的多维索引。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <param name="offset">要获取对应多维索引的偏移量。</param>
        /// <returns><paramref name="source"/> 偏移量为
        /// <paramref name="offset"/> 的元素对应的多维索引。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="offset"/> 不在 0 和
        /// <paramref name="source"/> 的 <see cref="Array.LongLength"/> 之间。</exception>
        public static int[] OffsetToIndices(this Array source, long offset)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if ((offset < 0L) || (offset >= source.LongLength))
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }

            var result = new int[source.Rank];
            for (int i = 0; i < source.Rank; i++)
            {
                long scale = 1L;
                for (int j = source.Rank - 1; j >= i + 1; j--)
                {
                    scale *= source.GetLength(j);
                }
                result[i] = (int)(offset / scale);
                offset %= scale;
            }
            return result;
        }

        /// <summary>
        /// 确定当前数组与指定数组的元素是否对应相等。
        /// 对于交错数组会递归进行元素相等比较，直至数组元素的声明类型不为数组。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <param name="other">要与当前数组进行相等比较的数组。</param>
        /// <param name="comparer">用于比较数组元素的比较器。</param>
        /// <returns>若 <paramref name="source"/> 与 <paramref name="other"/> 的维度和各维度大小都相等，
        /// 且每个元素都对应相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="NotSupportedException">
        /// <paramref name="source"/> 的最内层元素为指针。</exception>
        public static bool ArrayEquals(this Array source, Array other,
            IEqualityComparer comparer = null)
        {
            // 引用比较。
            if (object.ReferenceEquals(source, other)) { return true; }
            if ((source is null) ^ (other is null)) { return false; }
            // 类型不同。
            if (source.GetType() != other.GetType()) { return false; }
            // 大小不等。
            if (source.Rank != other.Rank) { return false; }
            if (source.LongLength != other.LongLength) { return false; }
            for (int i = 0; i < source.Rank; i++)
            {
                if (source.GetLength(i) != other.GetLength(i))
                {
                    return false;
                }
            }

            // 通常情况。
            comparer = comparer ?? EqualityComparer<object>.Default;
            bool isMultiDim = source.Rank > 1;
            bool isJagged = source.GetType().GetElementType().IsArray;
            for (long i = 0L; i < source.LongLength; i++)
            {
                var xi = isMultiDim ? source.GetValue(source.OffsetToIndices(i)) : source.GetValue(i);
                var yi = isMultiDim ? other.GetValue(other.OffsetToIndices(i)) : other.GetValue(i);

                if (isJagged)
                {
                    if (!ArrayExtensions.ArrayEquals((Array)xi, (Array)yi))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!comparer.Equals(xi, yi))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 枚举数组中的每个元素。对于交错数组，将递归枚举至元素的声明类型不为数组。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <returns>数组元素的公开枚举数 <see cref="IEnumerable"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="source"/> 的最内层元素为指针。</exception>
        public static IEnumerable EnumerateArray(this Array source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (source.GetType().GetElementType().IsArray)
            {
                foreach (Array array in source)
                {
                    foreach (var item in array.EnumerateArray())
                    {
                        yield return item;
                    }
                }
            }
            else
            {
                foreach (object item in source)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// 返回一个指定大小的数组，其每个元素都由当前数组的最内层元素按顺序复制得到。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <param name="lengths">新数组的每个维度的大小。</param>
        /// <returns>一个大小等于 <paramref name="lengths"/> 的数组，
        /// 其每个元素都由 <paramref name="source"/> 的最内层元素按顺序复制得到。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="lengths"/> 所表示的数组元素数量小于 0。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="lengths"/>
        /// 所表示的数组元素数量与 <paramref name="source"/> 中的元素数量不一致。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="source"/> 的最内层元素为指针。</exception>
        public static Array Reshape(this Array source, params int[] lengths)
        {
            var itemType = source.GetElementTypeRecursive();
            if (itemType.IsPointer)
            {
                throw new NotSupportedException();
            }

            var result = Array.CreateInstance(itemType, lengths);
            var items = source.EnumerateArray().GetEnumerator();

            bool isMultiDim = lengths.Length > 1;
            for (long i = 0L; i < result.LongLength; i++)
            {
                if (items.MoveNext())
                {
                    if (isMultiDim)
                    {
                        var a = result.OffsetToIndices(i);
                        result.SetValue(items.Current, a);
                    }
                    else
                    {
                        result.SetValue(items.Current, i);
                    }
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(lengths));
                }
            }
            if (items.MoveNext())
            {
                throw new ArgumentOutOfRangeException(nameof(lengths));
            }

            return result;
        }

        /// <summary>
        /// 返回一个指定大小的交错数组，其每个元素都由当前数组的最内层元素按顺序复制得到。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <param name="lengths">新数组的每个维度的大小。</param>
        /// <returns>一个大小等于 <paramref name="lengths"/> 的交错数组，
        /// 其每个元素都由 <paramref name="source"/> 的最内层元素按顺序复制得到。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="lengths"/> 所表示的数组元素数量小于 0。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="lengths"/>
        /// 所表示的数组元素数量与 <paramref name="source"/> 中的元素数量不一致。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="source"/> 的最内层元素为指针。</exception>
        public static Array ReshapeAsJagged(this Array source, params int[] lengths)
        {
            var itemType = source.GetElementTypeRecursive();
            if (itemType.IsPointer)
            {
                throw new NotSupportedException();
            }

            if (lengths.Length == 0)
            {
                return Array.CreateInstance(itemType, lengths);
            }
            else if (lengths.Length == 1)
            {
                return source.Reshape(lengths);
            }
            else
            {
                long Product(params int[] values)
                {
                    long product = 1L;
                    foreach (var value in values)
                    {
                        product *= value;
                    }
                    return product;
                }

                var result = (Array)null;
                var items = source.EnumerateArray().GetEnumerator();
                var lastLengths = lengths;
                var restLengths = new int[lastLengths.Length - 1];
                Array.ConstrainedCopy(lastLengths, 0, restLengths, 0, restLengths.Length);

                while (restLengths.Length > 0)
                {
                    result = Array.CreateInstance(itemType.MakeArrayType(), Product(restLengths));
                    for (long i = 0L; i < result.LongLength; i++)
                    {
                        var array = Array.CreateInstance(itemType, lastLengths[lastLengths.Length - 1]);
                        for (long j = 0L; j < array.LongLength; j++)
                        {
                            if (items.MoveNext())
                            {
                                array.SetValue(items.Current, j);
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(nameof(lengths));
                            }
                        }
                        result.SetValue(array, i);
                    }
                    if (items.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException(nameof(lengths));
                    }

                    items = result.GetEnumerator();
                    itemType = itemType.MakeArrayType();
                    lastLengths = restLengths;
                    restLengths = new int[lastLengths.Length - 1];
                    Array.ConstrainedCopy(lastLengths, 0, restLengths, 0, restLengths.Length);
                }
                return result;
            }
        }

        /// <summary>
        /// 返回一个新数组，此数组为当前数组和指定数组连接后的结果。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 中元素的类型。</typeparam>
        /// <param name="source">一个包含 <typeparamref name="T"/> 类型元素的数组。</param>
        /// <param name="other">要于当前数组连接的数组。</param>
        /// <returns>一个新数组，此数组为当前数组和指定数组连接后的结果。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="other"/> 为 <see langword="null"/>。</exception>
        public static T[] Concat<T>(this T[] source, T[] other)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (other is null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            var result = new T[source.Length + other.Length];
            Array.ConstrainedCopy(source, 0, result, 0, source.Length);
            Array.ConstrainedCopy(other, 0, result, source.Length, other.Length);
            return result;
        }

        /// <summary>
        /// 返回一个新数组，此数组为当前交错数组中包含的数组顺序连接降阶后的结果。
        /// </summary>
        /// <typeparam name="T"><paramref name="source"/> 包含的数组中元素的类型。</typeparam>
        /// <param name="source">一个包含 <typeparamref name="T"/> 类型元素数组的数组。</param>
        /// <returns>一个新数组，此数组为指定交错数组中包含的数组顺序连接后的结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 包含为 <see langword="null"/> 的数组。</exception>
        public static T[] ReduceRank<T>(this T[][] source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            int length = 0;
            for (int i = 0; i < source.Length; i++)
            {
                var array = source[i];
                if (array is null)
                {
                    throw new ArgumentException(
                        new ArgumentException().Message, nameof(source));
                }
                length += array.Length;
            }

            var result = new T[length];
            int index = 0;
            for (int i = 0; i < source.Length; i++)
            {
                var array = source[i];
                Array.ConstrainedCopy(array, 0, result, index, array.Length);
                index += array.Length;
            }
            return result;
        }

        /// <summary>
        /// 返回一个长度等于当前 32 位有符号整数的指定类型的数组，
        /// 数组的每个元素由 <see cref="Converter{TInput, TOutput}"/> 转换索引得到。
        /// </summary>
        /// <typeparam name="T">要创建的数组的元素的类型。</typeparam>
        /// <param name="source">一个 32 位有符号整数。</param>
        /// <param name="indexMap">用于将索引转换到数组元素的转换器。</param>
        /// <returns>长度为 <paramref name="source"/> 的数组，
        /// 其中的每个元素由 <paramref name="indexMap"/> 转换索引得到。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="indexMap"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> 小于 0。</exception>
        public static T[] RangeMapToArray<T>(this int source, Converter<int, T> indexMap)
        {
            if (source < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(source));
            }
            if (indexMap is null)
            {
                throw new ArgumentNullException(nameof(indexMap));
            }

            var result = new T[source];
            for (int i = 0; i < source; i++)
            {
                result[i] = indexMap(i);
            }
            return result;
        }

        /// <summary>
        /// 返回一个大小等于当前 32 位有符号整数数组的指定类型的多维数组，
        /// 数组的每个元素由 <see cref="Converter{TInput, TOutput}"/> 转换索引得到。
        /// </summary>
        /// <typeparam name="T">要创建的数组的元素的类型。</typeparam>
        /// <param name="source">一个 32 位有符号整数的数组。</param>
        /// <param name="indicesMap">用于将索引转换到数组元素的转换器。</param>
        /// <returns>大小为 <paramref name="source"/> 的多维数组，
        /// 其中的每个元素由 <paramref name="indicesMap"/> 转换索引得到。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 或
        /// <paramref name="indicesMap"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="source"/> 中没有任何元素。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="source"/> 中包含小于 0 的整数。</exception>
        public static Array RangesMapToArray<T>(this int[] source, Converter<int[], T> indicesMap)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (indicesMap is null)
            {
                throw new ArgumentNullException(nameof(indicesMap));
            }

            if (source.Length == 0)
            {
                return Array.CreateInstance(typeof(T), source);
            }
            else if (source.Length == 1)
            {
                return source[0].RangeMapToArray(index => indicesMap(new[] { index }));
            }
            else
            {
                var result = Array.CreateInstance(typeof(T), source);
                for (long i = 0; i < result.LongLength; i++)
                {
                    var a = result.OffsetToIndices(i);
                    result.SetValue(indicesMap(a), a);
                }
                return result;
            }
        }
    }
}
