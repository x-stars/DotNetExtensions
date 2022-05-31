using System;
using System.Collections;

namespace XNetEx
{
    static partial class ArrayExtensions
    {
        /// <summary>
        /// 枚举数组中的每个元素。对于交错数组，将递归枚举至元素的声明类型不为数组。
        /// </summary>
        /// <param name="array">要枚举元素数组。</param>
        /// <returns>数组元素的公开枚举数 <see cref="IEnumerable"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 为 <see langword="null"/>，
        /// 或 <paramref name="array"/> 的内层数组为 <see langword="null"/>。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="array"/> 的最内层元素为指针。</exception>
        public static IEnumerable RecursiveEnumerate(this Array array)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            static IEnumerable EnumerateCore(Array array)
            {
                if (array.GetType().GetElementType()!.IsArray)
                {
                    foreach (var item in array)
                    {
                        var innerItems = ((Array)item!).RecursiveEnumerate();
                        foreach (var innerItem in innerItems)
                        {
                            yield return innerItem;
                        }
                    }
                }
                else
                {
                    foreach (var item in array)
                    {
                        yield return item;
                    }
                }
            }

            return EnumerateCore(array);
        }

        /// <summary>
        /// 返回一个指定大小的数组，其每个元素都由当前数组的最内层元素按顺序复制得到。
        /// </summary>
        /// <param name="array">作为数据来源的数组。</param>
        /// <param name="lengths">新数组的每个维度的大小。</param>
        /// <returns>一个大小等于 <paramref name="lengths"/> 的数组，
        /// 其每个元素都由 <paramref name="array"/> 的最内层元素按顺序复制得到。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 或 <paramref name="lengths"/> 为 <see langword="null"/>，
        /// 或 <paramref name="array"/> 的内层数组为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="lengths"/> 中没有任何元素。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="lengths"/>
        /// 所表示的数组元素数量与 <paramref name="array"/> 中的元素数量不一致。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="array"/> 的最内层元素为指针。</exception>
        public static Array Reshape(this Array array, params int[] lengths)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (lengths is null)
            {
                throw new ArgumentNullException(nameof(lengths));
            }

            var itemType = array.GetType().GetElementType()!;
            while (itemType.IsArray)
            {
                itemType = itemType.GetElementType()!;
            }

            if (itemType.IsPointer)
            {
                throw new NotSupportedException();
            }

            var result = Array.CreateInstance(itemType, lengths);
            var items = array.RecursiveEnumerate().GetEnumerator();

            bool isMultiDim = lengths.Length > 1;
            foreach (var offset in ..result.Length)
            {
                if (items.MoveNext())
                {
                    if (isMultiDim)
                    {
                        var indices = result.OffsetToIndices(offset);
                        result.SetValue(items.Current, indices);
                    }
                    else
                    {
                        result.SetValue(items.Current, offset);
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
        /// <param name="array">作为数据来源的数组。</param>
        /// <param name="lengths">新数组的每个维度的大小。</param>
        /// <returns>一个大小等于 <paramref name="lengths"/> 的交错数组，
        /// 其每个元素都由 <paramref name="array"/> 的最内层元素按顺序复制得到。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 或 <paramref name="lengths"/> 为 <see langword="null"/>，
        /// 或 <paramref name="array"/> 的内层数组为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="lengths"/> 中没有任何元素。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="lengths"/>
        /// 所表示的数组元素数量与 <paramref name="array"/> 中的元素数量不一致。</exception>
        /// <exception cref="NotSupportedException">
        /// <paramref name="array"/> 的最内层元素为指针。</exception>
        public static Array ReshapeJagged(this Array array, params int[] lengths)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (lengths is null)
            {
                throw new ArgumentNullException(nameof(lengths));
            }

            var itemType = array.GetType().GetElementType()!;
            while (itemType.IsArray)
            {
                itemType = itemType.GetElementType()!;
            }

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
                return array.Reshape(lengths);
            }
            else
            {
                static int ProductOf(params int[] values)
                {
                    int product = 1;
                    foreach (var value in values)
                    {
                        product *= value;
                    }
                    return product;
                }

                var result = Array.CreateInstance(itemType, 0);
                var items = array.RecursiveEnumerate().GetEnumerator();
                var lastLengths = lengths;
                var restLengths = new int[lastLengths.Length - 1];
                Array.Copy(lastLengths, 0, restLengths, 0, restLengths.Length);

                while (restLengths.Length > 0)
                {
                    result = Array.CreateInstance(itemType.MakeArrayType(), ProductOf(restLengths));
                    foreach (var index in ..result.Length)
                    {
                        var innerArray = Array.CreateInstance(itemType, lastLengths[^1]);
                        foreach (var innerIndex in ..innerArray.Length)
                        {
                            if (items.MoveNext())
                            {
                                innerArray.SetValue(items.Current, innerIndex);
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(nameof(lengths));
                            }
                        }
                        result.SetValue(innerArray, index);
                    }
                    if (items.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException(nameof(lengths));
                    }

                    items = result.GetEnumerator();
                    itemType = itemType.MakeArrayType();
                    lastLengths = restLengths;
                    restLengths = new int[lastLengths.Length - 1];
                    Array.Copy(lastLengths, 0, restLengths, 0, restLengths.Length);
                }
                return result;
            }
        }

        /// <summary>
        /// 将指定的二维数组转换为大小相等、元素相同的二维交错数组。
        /// </summary>
        /// <typeparam name="T">多维数组中的元素的类型。</typeparam>
        /// <param name="array">要转换为二维交错数组的二维数组。</param>
        /// <returns>与 <paramref name="array"/> 大小相等、元素相同的二维交错数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
        public static T[][] ToJaggedArray<T>(this T[,] array)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var length1 = array.GetLength(0);
            var length2 = array.GetLength(1);

            var result = new T[length1][];
            for (int index1 = 0; index1 < length1; index1++)
            {
                var result2 = new T[length2];
                for (int index2 = 0; index2 < length2; index2++)
                {
                    result2[index2] = array[index1, index2];
                }
                result[index1] = result2;
            }
            return result;
        }

        /// <summary>
        /// 将指定的三维数组转换为大小相等、元素相同的三维交错数组。
        /// </summary>
        /// <typeparam name="T">三维数组中的元素的类型。</typeparam>
        /// <param name="array">要转换为三维交错数组的三维数组。</param>
        /// <returns>与 <paramref name="array"/> 大小相等、元素相同的三维交错数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 为 <see langword="null"/>。</exception>
        public static T[][][] ToJaggedArray<T>(this T[,,] array)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var length1 = array.GetLength(0);
            var length2 = array.GetLength(1);
            var length3 = array.GetLength(2);

            var result = new T[length1][][];
            for (int index1 = 0; index1 < length1; index1++)
            {
                var result2 = new T[length2][];
                for (int index2 = 0; index2 < length2; index2++)
                {
                    var result3 = new T[length3];
                    for (int index3 = 0; index3 < length3; index3++)
                    {
                        result3[index3] = array[index1, index2, index3];
                    }
                    result2[index2] = result3;
                }
                result[index1] = result2;
            }
            return result;
        }

        /// <summary>
        /// 将指定的二维交错数组转换为大小相等、元素相同的二维数组。
        /// </summary>
        /// <typeparam name="T">二维交错数组中的元素的类型。</typeparam>
        /// <param name="array">要转换为二维数组的二维交错数组。</param>
        /// <returns>与 <paramref name="array"/> 大小相等、元素相同的二维数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 为 <see langword="null"/>，
        /// 或 <paramref name="array"/> 的内层数组为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="array"/> 的内层数组的长度不相等。</exception>
        public static T[,] ToRank2Array<T>(this T[][] array)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length == 0)
            {
                return new T[0, 0];
            }
            if (array[0] is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var length1 = array.Length;
            var length2 = array[0].Length;

            var result = new T[length1, length2];
            for (int index1 = 0; index1 < length1; index1++)
            {
                var array2 = array[index1];
                if (array2.Length != length2)
                {
                    throw new ArgumentOutOfRangeException(nameof(array));
                }

                for (int index2 = 0; index2 < length2; index2++)
                {
                    result[index1, index2] = array2[index2];
                }
            }
            return result;
        }

        /// <summary>
        /// 将指定的三维交错数组转换为大小相等、元素相同的三维数组。
        /// </summary>
        /// <typeparam name="T">交错数组中的元素的类型。</typeparam>
        /// <param name="array">要转换为三维数组的三维交错数组。</param>
        /// <returns>与 <paramref name="array"/> 大小相等、元素相同的三维数组。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="array"/> 为 <see langword="null"/>，
        /// 或 <paramref name="array"/> 的内层数组为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="array"/> 的内层数组的长度不相等。</exception>
        public static T[,,] ToRank3Array<T>(this T[][][] array)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            if (array.Length == 0)
            {
                return new T[0, 0, 0];
            }
            if (array[0] is null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            if (array[0].Length == 0)
            {
                return new T[0, 0, 0];
            }
            if (array[0][0] is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var length1 = array.Length;
            var length2 = array[0].Length;
            var length3 = array[0][0].Length;

            var result = new T[length1, length2, length3];
            for (int index1 = 0; index1 < length1; index1++)
            {
                var array2 = array[index1];
                if (array2.Length != length2)
                {
                    throw new ArgumentOutOfRangeException(nameof(array));
                }

                for (int index2 = 0; index2 < length2; index2++)
                {
                    var array3 = array2[index2];
                    if (array3.Length != length3)
                    {
                        throw new ArgumentOutOfRangeException(nameof(array));
                    }

                    for (int index3 = 0; index3 < length3; index3++)
                    {
                        result[index1, index2, index3] = array3[index3];
                    }
                }
            }
            return result;
        }
    }
}
