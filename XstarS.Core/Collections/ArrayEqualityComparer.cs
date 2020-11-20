using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供数组的元素序列的相等比较的方法。
    /// </summary>
    [Serializable]
    public class ArrayEqualityComparer : EqualityComparer<Array>
    {
        /// <summary>
        /// 初始化 <see cref="ArrayEqualityComparer"/> 类的新实例。
        /// </summary>
        public ArrayEqualityComparer() : this(recurse: false, null) { }

        /// <summary>
        /// 以比较元素时要使用的比较器初始化 <see cref="ArrayEqualityComparer"/> 类的新实例，
        /// 并指定是否对内层声明数组进行递归比较。
        /// </summary>
        /// <param name="recurse">指定是否对内层声明数组进行递归比较。</param>
        /// <param name="comparer">比较数组中的元素时要使用的比较器。</param>
        public ArrayEqualityComparer(
            bool recurse = false, IEqualityComparer comparer = null)
        {
            this.Recurse = recurse;
            this.ItemComparer = comparer ?? EqualityComparer<object>.Default;
        }

        /// <summary>
        /// 获取默认的 <see cref="ArrayEqualityComparer"/> 实例。
        /// </summary>
        public static new ArrayEqualityComparer Default { get; } = new ArrayEqualityComparer();

        /// <summary>
        /// 获取当前比较器是否对内层声明数组进行递归比较。
        /// </summary>
        /// <returns>若 <see cref="ArrayEqualityComparer"/> 对内层声明数组进行递归比较，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Recurse { get; }

        /// <summary>
        /// 获取比较数组中的元素时使用的比较器。
        /// </summary>
        /// <returns>比较数组中的元素时使用的比较器。</returns>
        protected IEqualityComparer ItemComparer { get; }

        /// <summary>
        /// 确定两个指定的数组中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个数组。</param>
        /// <param name="y">要比较的第二个数组。</param>
        /// <returns>如果两个数组中的所有元素对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(Array x, Array y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            var recurse = this.Recurse;
            var comparer = this.ItemComparer;

            if (x.GetType() != y.GetType()) { return false; }

            if (x.Rank != y.Rank) { return false; }
            if (x.Length != y.Length) { return false; }
            for (int i = 0; i < x.Rank; i++)
            {
                if (x.GetLength(i) != y.GetLength(i))
                {
                    return false;
                }
            }

            var length = x.Length;
            var isMultiDim = x.Rank > 1;
            var isJagged = x.GetType().GetElementType().IsArray;

            for (int i = 0; i < length; i++)
            {
                var xItem = isMultiDim ? x.GetValue(x.OffsetToIndices(i)) : x.GetValue(i);
                var yItem = isMultiDim ? y.GetValue(y.OffsetToIndices(i)) : y.GetValue(i);

                if (recurse && isJagged)
                {
                    if (!this.Equals((Array)xItem, (Array)yItem))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!comparer.Equals(xItem, yItem))
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
        /// <returns>数组遍历元素得到的哈希代码。</returns>
        public override int GetHashCode(Array obj)
        {
            if (obj is null) { return 0; }

            var recurse = this.Recurse;
            var comparer = this.ItemComparer;

            var length = obj.Length;
            var isMultiDim = obj.Rank > 1;
            var isJagged = obj.GetType().GetElementType().IsArray;

            var hashCode = obj.GetType().GetHashCode();
            for (int i = 0; i < length; i++)
            {
                var item = isMultiDim ? obj.GetValue(obj.OffsetToIndices(i)) : obj.GetValue(i);

                var nextHashCode = (recurse && isJagged) ?
                    this.GetHashCode((Array)item) : comparer.GetHashCode(item);
                hashCode = hashCode * -1521134295 + nextHashCode;
            }
            return hashCode;
        }
    }
}
