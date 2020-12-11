using System;

namespace XstarS
{
    /// <summary>
    /// 提供数组的扩展方法。
    /// </summary>
    internal static partial class ArrayExtensions
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
        /// 确定当前数组是否为一个下限为零的一维数组。
        /// </summary>
        /// <param name="array"></param>
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
            return itemType.MakeArrayType() == type;
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
            for (int rank = 0; rank < array.Rank; rank++)
            {
                scale /= array.GetLength(rank);
                var start = array.GetLowerBound(rank);
                result[rank] = offset / scale + start;
                offset %= scale;
            }
            return result;
        }
    }
}
