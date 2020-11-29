using System;
using System.Collections.Generic;
using XstarS.Collections.Generic;

namespace XstarS
{
    /// <summary>
    /// 提供数组基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="TItem">数组中元素的类型。</typeparam>
    [Serializable]
    internal sealed class SZArrayEqualityComparer<TItem> : CollectionEqualityComparer<TItem[]>
    {
        /// <summary>
        /// 用于比较数组中元素的 <see cref="IEqualityComparer{T}"/>。
        /// </summary>
        private readonly IEqualityComparer<TItem> ItemComparer;

        /// <summary>
        /// 初始化 <see cref="SZArrayEqualityComparer{TItem}"/> 类的新实例。
        /// </summary>
        public SZArrayEqualityComparer()
        {
            this.ItemComparer = CollectionEqualityComparer<TItem>.Default;
        }

        /// <summary>
        /// 确定两个指定数组中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个数组。</param>
        /// <param name="y">要比较的第二个数组。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 的类型相同且所包含的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(TItem[] x, TItem[] y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            var comparer = this.ItemComparer;

            if (x.GetType() != y.GetType()) { return false; }

            if (x.Length != y.Length) { return false; }

            var length = x.Length;
            for (int index = 0; index < length; index++)
            {
                if (!comparer.Equals(x[index], y[index]))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取指定的数组遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的数组。</param>
        /// <returns>遍历 <paramref name="obj"/> 中元素得到的哈希代码。</returns>
        public override int GetHashCode(TItem[] obj)
        {
            if (obj is null) { return 0; }

            var comparer = this.ItemComparer;

            var length = obj.Length;
            var hashCode = obj.GetType().GetHashCode();
            for (int index = 0; index < length; index++)
            {
                var nextHashCode = comparer.GetHashCode(obj[index]);
                hashCode = this.CombineHashCode(hashCode, nextHashCode);
            }
            return hashCode;
        }
    }
}
