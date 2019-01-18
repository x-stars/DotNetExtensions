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
        /// 确定当前数组与指定数组的元素是否对应相等。
        /// 对于嵌套数组会递归进行元素相等比较。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <param name="other">要与当前数组进行相等比较的数组。</param>
        /// <param name="comparer">用于比较数组元素的比较器。</param>
        /// <returns>若 <paramref name="source"/> 与 <paramref name="other"/> 的维度和各维度大小都相等，
        /// 且每个元素都对应相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ArrayEquals(this Array source, Array other,
            IEqualityComparer comparer = null)
        {
            // 空引用。
            if ((source is null) && (other is null)) { return true; }
            if ((source is null) ^ (other is null)) { return false; }
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
            var t_source = source.GetType();
            bool isMultiDim = source.Rank > 1;
            bool isNested = t_source.GetElementType().IsArray;
            for (long i = 0L; i < source.LongLength; i++)
            {
                var xi = isMultiDim ? source.GetValue(source.IndexV2T(i)) : source.GetValue(i);
                var yi = isMultiDim ? other.GetValue(other.IndexV2T(i)) : other.GetValue(i);

                if (isNested)
                {
                    if (!ArrayExtensions.ArrayEquals(xi as Array, yi as Array))
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
        /// 将当前数组的一维连续索引转换为多维索引。
        /// </summary>
        /// <param name="source">一个数组。</param>
        /// <param name="index">要获取对应多维索引的一维连续索引。</param>
        /// <returns><paramref name="source"/> 中第
        /// <paramref name="index"/> 个元素对应的多维索引。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 不在 0 和
        /// <paramref name="source"/> 的 <see cref="Array.LongLength"/> 之间。</exception>
        public static int[] IndexV2T(this Array source, long index)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if ((index < 0L) || (index >= source.LongLength))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            var result = new int[source.Rank];
            for (int i = 0; i < source.Rank; i++)
            {
                long scale = 1L;
                for (int j = source.Rank - 1; j >= i + 1; j--)
                {
                    scale *= source.GetLength(j);
                }
                result[i] = (int)(index / scale);
                index %= scale;
            }
            return result;
        }
    }
}
