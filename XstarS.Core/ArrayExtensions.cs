using System;

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
        public static object[] Box<T>(this T?[] array) where T : struct
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
            for (int index = 0; index < arrays.Length; index++)
            {
                var array = arrays[index] ?? Array.Empty<T>();
                length += array.Length;
            }

            var result = new T[length];
            int offset = 0;
            for (int index = 0; index < arrays.Length; index++)
            {
                var array = arrays[index] ?? Array.Empty<T>();
                Array.Copy(array, 0, result, offset, array.Length);
                offset += array.Length;
            }
            return result;
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
            for (int index = 0; index < length; index++)
            {
                result[index] = indexMap(index);
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
            for (int offset = 0; offset < result.Length; offset++)
            {
                var indices = result.OffsetToIndices(offset);
                result.SetValue(indicesMap(indices), indices);
            }
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
            var itemType = type.GetElementType();
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

            var result = new int[array.Rank];
            var scale = array.Length;
            for (int rank = 0; rank < array.Rank; rank++)
            {
                var start = array.GetLowerBound(rank);
                scale /= array.GetLength(rank);
                result[rank] = start + offset / scale;
                offset %= scale;
            }
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

        /// <summary>
        /// 将当前字节数组转换为其用 Base64 数字编码的等效字符串表示形式。
        /// </summary>
        /// <param name="bytes">要进行转换的字节数组。</param>
        /// <returns>当前字节数组的 Base64 数字编码的等效字符串表示形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public static string ToBase64String(this byte[] bytes) => Convert.ToBase64String(bytes);

        /// <summary>
        /// 将当前字节数组转换为十六进制数字的等效字符串表示形式。
        /// </summary>
        /// <param name="bytes">要进行转换的字节数组。</param>
        /// <returns>当前字节数组的十六进制数字的等效字符串表示形式。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> 为 <see langword="null"/>。</exception>
        public static string ToHexString(this byte[] bytes)
        {
            if (bytes is null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var hexes = Array.ConvertAll(bytes, @byte => @byte.ToString("X2"));
            return string.Join(string.Empty, hexes);
        }
    }
}
