using System;
using System.Collections;
using XstarS.Collections.Generic;
using XstarS.Runtime;

namespace XstarS
{
    /// <summary>
    /// 提供数组基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="TArray">数组的类型。</typeparam>
    [Serializable]
    internal sealed class ArrayEqualityComparer<TArray> : CollectionEqualityComparer<TArray>
    {
        /// <summary>
        /// 用于比较数组中元素的 <see cref="IEqualityComparer"/>。
        /// </summary>
        private readonly IEqualityComparer ItemsComparer;

        /// <summary>
        /// 初始化 <see cref="ArrayEqualityComparer{TArray}"/> 类的新实例。
        /// </summary>
        public ArrayEqualityComparer()
        {
            this.ItemsComparer = ArrayEqualityComparer<TArray>.GetItemComparer();
        }

        /// <summary>
        /// 获取当前数组中元素的比较器。
        /// </summary>
        /// <returns><typeparamref name="TArray"/> 中元素的比较器。</returns>
        private static IEqualityComparer GetItemComparer()
        {
            var itemType = typeof(TArray).GetElementType();
            var comparerType = typeof(CollectionEqualityComparer<>).MakeGenericType(itemType);
            var defaultProperty = comparerType.GetProperty("Default");
            return (IEqualityComparer)defaultProperty.GetValue(null);
        }

        /// <summary>
        /// 确定两个指定数组中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个数组。</param>
        /// <param name="y">要比较的第二个数组。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 的类型相同且所包含的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(TArray x, TArray y)
        {
            var arrayX = (Array)(object)x;
            var arrayY = (Array)(object)y;

            if (arrayX.Rank != arrayY.Rank) { return false; }
            if (arrayX.Length != arrayY.Length) { return false; }
            for (int i = 0; i < arrayX.Rank; i++)
            {
                if (arrayX.GetLength(i) != arrayY.GetLength(i))
                {
                    return false;
                }
            }

            var typeArray = arrayX.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < arrayX.Length; index++)
                {
                    var valueItem = methodGet.Invoke(arrayX, Array.ConvertAll(
                        arrayX.OffsetToIndices(index), ObjectRuntimeHelper.BoxIndex));
                    var otherItem = methodGet.Invoke(arrayY, Array.ConvertAll(
                        arrayY.OffsetToIndices(index), ObjectRuntimeHelper.BoxIndex));
                    if (!ObjectRuntimeHelper.BoxedPointerEquals(valueItem, otherItem))
                    {
                        return false;
                    }
                }
            }
            else
            {
                for (int index = 0; index < arrayX.Length; index++)
                {
                    var valueItem = arrayX.GetValue(arrayX.OffsetToIndices(index));
                    var otherItem = arrayY.GetValue(arrayY.OffsetToIndices(index));
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
        public override int GetHashCode(TArray obj)
        {
            var array = (Array)(object)obj;

            var hashCode = 0;

            var typeArray = array.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < array.Length; index++)
                {
                    var item = methodGet.Invoke(array, Array.ConvertAll(
                        array.OffsetToIndices(index), ObjectRuntimeHelper.BoxIndex));
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
