using System;
using System.Collections;
using System.Collections.Generic;
using XstarS.Collections;
using XstarS.Collections.Generic;
using XstarS.Collections.Specialized;

namespace XstarS
{
    /// <summary>
    /// 提供数组的扩展方法。
    /// </summary>
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// 返回一个新数组，此数组为当前数组追加指定元素后的结果。
        /// </summary>
        /// <typeparam name="T"><paramref name="array"/> 中元素的类型。</typeparam>
        /// <param name="array">要进行追加的数组。</param>
        /// <param name="item">要追加到当前数组的元素。</param>
        /// <returns>一个新数组，此数组为当前数组追加指定元素后的结果。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
        public static T[] Append<T>(this T[] array, T item)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var result = new T[array.Length + 1];
            Array.Copy(array, result, array.Length);
            result[array.Length] = item;
            return result;
        }

        /// <summary>
        /// 确定当前数组与指定数组的元素是否对应相等。
        /// </summary>
        /// <typeparam name="T">数组元素的类型。</typeparam>
        /// <param name="array">要进行相等比较的数组。</param>
        /// <param name="other">要与当前数组进行相等比较的数组。</param>
        /// <param name="comparer">用于比较数组元素的比较器。</param>
        /// <returns>若 <paramref name="array"/> 与
        /// <paramref name="other"/> 的每个元素都对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ArrayEquals<T>(this T[] array, T[] other,
            IEqualityComparer<T> comparer = null)
        {
            return new ArrayEqualityComparer<T>(comparer).Equals(array, other);
        }

        /// <summary>
        /// 确定当前数组与指定数组的元素是否对应相等。
        /// </summary>
        /// <param name="array">要进行相等比较的数组。</param>
        /// <param name="other">要与当前数组进行相等比较的数组。</param>
        /// <param name="recurse">指示是否对内层数组递归。</param>
        /// <param name="comparer">用于比较数组元素的比较器。</param>
        /// <returns>若 <paramref name="array"/> 与
        /// <paramref name="other"/> 的维度和各维度大小都相等，且每个元素都对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="NotSupportedException">
        /// <paramref name="array"/> 的最内层元素为指针。</exception>
        public static bool ArrayEquals(this Array array, Array other,
            bool recurse = false, IEqualityComparer comparer = null)
        {
            return new ArrayEqualityComparer(recurse, comparer).Equals(array, other);
        }

        /// <summary>
        /// 返回当前数组的所有元素的字符串表达形式。
        /// </summary>
        /// <param name="array">要获取字符串表达形式的数组。</param>
        /// <param name="recurse">指示是否对内层声明数组递归。</param>
        /// <returns><paramref name="array"/> 的所有元素的字符串表达形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
        public static string ArrayToString(this Array array,
            bool recurse = false)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var comparer = ReferenceEqualityComparer<Array>.Default;
            return array.ArrayToString(false, new HashSet<Array>(comparer));
        }

        /// <summary>
        /// 返回当前数组的所有元素的字符串表达形式。
        /// </summary>
        /// <param name="array">要获取字符串表达形式的数组。</param>
        /// <param name="recurse">指示是否对内层声明数组递归。</param>
        /// <param name="pathed">当前路径已经访问的数组。</param>
        /// <returns><paramref name="array"/> 的所有元素的字符串表达形式。</returns>
        private static string ArrayToString(this Array array,
            bool recurse, HashSet<Array> pathed)
        {
            if (!pathed.Add(array))
            {
                return "{ ... }";
            }

            var result = default(string);
            recurse &= array.GetType().GetElementType().IsArray;
            if (array.Rank == 1)
            {
                var sequence = new List<string>();
                foreach (var item in array)
                {
                    sequence.Add(recurse ?
                        (item as Array)?.ArrayToString(recurse, pathed) :
                        item?.ToString());
                }
                result = "{ " + string.Join(", ", sequence) + " }";
            }
            else
            {
                result = array.ArrayToString(recurse, Array.Empty<int>(), pathed);
            }

            pathed.Remove(array);
            return result;
        }

        /// <summary>
        /// 返回当前多维数组的所有元素的字符串表达形式。
        /// </summary>
        /// <param name="array">要获取字符串表达形式的数组。</param>
        /// <param name="recurse">指示是否对内层声明数组递归。</param>
        /// <param name="indices">当前多维数组的当前索引。</param>
        /// <param name="pathed">当前路径已经访问的数组。</param>
        /// <returns><paramref name="array"/> 的所有元素的字符串表达形式。</returns>
        private static string ArrayToString(this Array array,
            bool recurse, int[] indices, HashSet<Array> pathed)
        {
            if (indices.Length == array.Rank)
            {
                var item = array.GetValue(indices);
                return recurse ?
                    (item as Array)?.ArrayToString(recurse, pathed) :
                    item?.ToString();
            }
            else
            {
                var sequence = new List<string>();
                var length = array.GetLength(indices.Length);
                for (int i = 0; i < length; i++)
                {
                    indices = indices.Append(i);
                    sequence.Add(array.ArrayToString(recurse, indices, pathed));
                }
                return "{ " + string.Join(", ", sequence) + " }";
            }
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
            for (int i = 0; i < arrays.Length; i++)
            {
                var array = arrays[i] ?? Array.Empty<T>();
                length += array.Length;
            }

            var result = new T[length];
            int index = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                var array = arrays[i] ?? Array.Empty<T>();
                Array.Copy(array, 0, result, index, array.Length);
                index += array.Length;
            }
            return result;
        }

        /// <summary>
        /// 获取当前数组遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="array">要为其获取哈希代码的数组。</param>
        /// <param name="comparer">用于获取数组元素的哈希代码的比较器。</param>
        /// <returns>遍历 <paramref name="array"/> 的元素得到的哈希代码。</returns>
        public static int GetArrayHashCode<T>(this T[] array,
            IEqualityComparer<T> comparer = null)
        {
            return new ArrayEqualityComparer<T>(comparer).GetHashCode(array);
        }

        /// <summary>
        /// 获取当前数组遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="array">要为其获取哈希代码的数组。</param>
        /// <param name="recurse">指示是否对内层数组递归。</param>
        /// <param name="comparer">用于获取数组元素的哈希代码的比较器。</param>
        /// <returns>遍历 <paramref name="array"/> 的元素得到的哈希代码。</returns>
        public static int GetArrayHashCode(this Array array,
            bool recurse = false, IEqualityComparer comparer = null)
        {
            return new ArrayEqualityComparer(recurse, comparer).GetHashCode(array);
        }

        /// <summary>
        /// 返回一个长度等于当前 32 位有符号整数的指定类型的数组，
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
        public static T[] InitializeArray<T>(this int length, Func<int, T> indexMap)
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
            for (int i = 0; i < length; i++)
            {
                result[i] = indexMap(i);
            }
            return result;
        }

        /// <summary>
        /// 返回一个大小等于当前 32 位有符号整数二元组的指定类型的二维数组，
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
        public static T[,] InitializeArray<T>(
            this (int, int) lengths, Func<int, int, T> indicesMap)
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
            for (int i = 0; i < length1; i++)
            {
                for (int j = 0; j < length2; j++)
                {
                    result[i, j] = indicesMap(i, j);
                }
            }
            return result;
        }

        /// <summary>
        /// 返回一个大小等于当前 32 位有符号整数三元组的指定类型的三维数组，
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
        public static T[,,] InitializeArray<T>(
            this (int, int, int) lengths, Func<int, int, int, T> indicesMap)
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
            for (int i = 0; i < length1; i++)
            {
                for (int j = 0; j < length2; j++)
                {
                    for (int k = 0; k < length3; k++)
                    {
                        result[i, j, k] = indicesMap(i, j, k);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 返回一个大小等于当前 32 位有符号整数数组的指定类型的多维数组，
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
        public static Array InitializeArray<T>(this int[] lengths, Func<int[], T> indicesMap)
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
            for (int i = 0; i < result.Length; i++)
            {
                var a = result.OffsetToIndices(i);
                result.SetValue(indicesMap(a), a);
            }
            return result;
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

            var scale = array.Length;
            var result = new int[array.Rank];
            for (int i = 0; i < array.Rank; i++)
            {
                scale /= array.GetLength(i);
                result[i] = offset / scale;
                offset %= scale;
            }
            return result;
        }
    }
}
