using System;
using System.Collections;
using XstarS.Collections.Generic;
using XstarS.Runtime;

namespace XstarS
{
    /// <summary>
    /// 提供数组基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">数组的类型。</typeparam>
    [Serializable]
    internal sealed class ArrayEqualityComparer<T> : StructuralEqualityComparer<T>
    {
        /// <summary>
        /// 表示用于比较数组中元素的 <see cref="IEqualityComparer"/>。
        /// </summary>
        private readonly IEqualityComparer ItemsComparer;

        /// <summary>
        /// 初始化 <see cref="ArrayEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public ArrayEqualityComparer()
        {
            this.ItemsComparer =
                StructuralEqualityComparer.OfType(typeof(T).GetElementType());
        }

        /// <summary>
        /// 确定两个指定数组中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个数组。</param>
        /// <param name="y">要比较的第二个数组。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 的类型相同且所包含的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(T x, T y)
        {
            var xArray = (Array)(object)x;
            var yArray = (Array)(object)y;

            if (xArray.Rank != yArray.Rank) { return false; }
            if (xArray.Length != yArray.Length) { return false; }
            for (int rank = 0; rank < xArray.Rank; rank++)
            {
                if (xArray.GetLength(rank) != yArray.GetLength(rank))
                {
                    return false;
                }
            }

            var typeArray = xArray.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < xArray.Length; index++)
                {
                    var valueItem = methodGet.Invoke(xArray, xArray.OffsetToIndices(index).Box());
                    var otherItem = methodGet.Invoke(yArray, yArray.OffsetToIndices(index).Box());
                    if (!ObjectRuntimeHelper.BoxedPointerEquals(valueItem, otherItem))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int index = 0; index < xArray.Length; index++)
                {
                    var valueItem = xArray.GetValue(xArray.OffsetToIndices(index));
                    var otherItem = yArray.GetValue(yArray.OffsetToIndices(index));
                    if (!this.ItemsComparer.Equals(valueItem, otherItem))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定的数组遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的数组。</param>
        /// <returns>遍历 <paramref name="obj"/> 中元素得到的哈希代码。</returns>
        public override int GetHashCode(T obj)
        {
            var array = (Array)(object)obj;

            var hashCode = 0;

            var typeArray = array.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < array.Length; index++)
                {
                    var item = methodGet.Invoke(array, array.OffsetToIndices(index).Box());
                    hashCode = this.CombineHashCode(
                        hashCode, ObjectRuntimeHelper.GetBoxedPointerHashCode(item));
                }
            }
            else
            {
                for (int index = 0; index < array.Length; index++)
                {
                    var item = array.GetValue(array.OffsetToIndices(index));
                    hashCode = this.CombineHashCode(
                        hashCode, this.ItemsComparer.GetHashCode(item));
                }
            }

            return hashCode;
        }
    }
}
