using System;
using System.Collections;

namespace XstarS
{
    partial class ArrayExtensions
    {
        /// <summary>
        /// 枚举数组中的每个元素。对于交错数组，将递归枚举至元素的声明类型不为数组。
        /// </summary>
        /// <param name="array">要枚举元素数组。</param>
        /// <returns>数组元素的公开枚举数 <see cref="IEnumerable"/> 对象。</returns>
        /// <exception cref="NotSupportedException">
        /// <paramref name="array"/> 的最内层元素为指针。</exception>
        public static IEnumerable RecurseEnumerate(this Array array)
        {
            if (array is null)
            {
                yield break;
            }

            if (array.GetType().GetElementType().IsArray)
            {
                foreach (var item in array)
                {
                    if (item is Array innerArray)
                    {
                        foreach (var innerItem in innerArray.RecurseEnumerate())
                        {
                            yield return innerItem;
                        }
                    }
                    else
                    {
                        yield return item;
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

        /// <summary>
        /// 返回一个指定大小的数组，其每个元素都由当前数组的最内层元素按顺序复制得到。
        /// </summary>
        /// <param name="array">作为数据来源的数组。</param>
        /// <param name="lengths">新数组的每个维度的大小。</param>
        /// <returns>一个大小等于 <paramref name="lengths"/> 的数组，
        /// 其每个元素都由 <paramref name="array"/> 的最内层元素按顺序复制得到。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 或
        /// <paramref name="lengths"/> 为 <see langword="null"/>。</exception>
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

            var itemType = array.GetType().GetElementType();
            while (itemType.IsArray)
            {
                itemType = itemType.GetElementType();
            }

            if (itemType.IsPointer)
            {
                throw new NotSupportedException();
            }

            var result = Array.CreateInstance(itemType, lengths);
            var items = array.RecurseEnumerate().GetEnumerator();

            bool isMultiDim = lengths.Length > 1;
            for (int i = 0; i < result.Length; i++)
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
        /// <param name="array">作为数据来源的数组。</param>
        /// <param name="lengths">新数组的每个维度的大小。</param>
        /// <returns>一个大小等于 <paramref name="lengths"/> 的交错数组，
        /// 其每个元素都由 <paramref name="array"/> 的最内层元素按顺序复制得到。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> 或
        /// <paramref name="lengths"/> 为 <see langword="null"/>。</exception>
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

            var itemType = array.GetType().GetElementType();
            while (itemType.IsArray)
            {
                itemType = itemType.GetElementType();
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
                int ProductOf(params int[] values)
                {
                    int product = 1;
                    foreach (var value in values)
                    {
                        product *= value;
                    }
                    return product;
                }

                var result = default(Array);
                var items = array.RecurseEnumerate().GetEnumerator();
                var lastLengths = lengths;
                var restLengths = new int[lastLengths.Length - 1];
                Array.Copy(lastLengths, 0, restLengths, 0, restLengths.Length);

                while (restLengths.Length > 0)
                {
                    result = Array.CreateInstance(itemType.MakeArrayType(), ProductOf(restLengths));
                    for (int i = 0; i < result.Length; i++)
                    {
                        var innerArray = Array.CreateInstance(
                            itemType, lastLengths[lastLengths.Length - 1]);
                        for (int j = 0; j < innerArray.Length; j++)
                        {
                            if (items.MoveNext())
                            {
                                innerArray.SetValue(items.Current, j);
                            }
                            else
                            {
                                throw new ArgumentOutOfRangeException(nameof(lengths));
                            }
                        }
                        result.SetValue(innerArray, i);
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
    }
}
