using System;
using System.Collections.Generic;
using XstarS.Collections.Generic;

namespace XstarS
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 提供数组基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="TItem">数组中的元素的类型。</typeparam>
    [Serializable]
    internal sealed class SZArrayEqualityComparer<TItem> : StructuralEqualityComparer<TItem[]>
    {
        /// <summary>
        /// 初始化 <see cref="SZArrayEqualityComparer{TItem}"/> 类的新实例。
        /// </summary>
        public SZArrayEqualityComparer() { }

        /// <summary>
        /// 确定两个指定数组中的元素是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个数组。</param>
        /// <param name="y">要比较的第二个数组。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected override bool EqualsCore(TItem[] x, TItem[] y, ISet<ObjectPair> compared)
        {
            if (x.Length != y.Length) { return false; }

            var length = x.Length;
            foreach (var index in ..length)
            {
                if (x[index]?.GetType() != y[index]?.GetType()) { return false; }

                var comparer = StructuralEqualityComparer.OfType(x[index]?.GetType());
                if (!comparer.Equals(x[index], y[index], compared)) { return false; }
            }
            return true;
        }

        /// <summary>
        /// 获取指定的数组中的元素的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的数组。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 中的元素的哈希代码。</returns>
        protected override int GetHashCodeCore(TItem[] obj, ISet<object> computed)
        {
            var length = obj.Length;
            var hashCode = obj.GetType().GetHashCode();
            foreach (var index in ..length)
            {
                var comparer = StructuralEqualityComparer.OfType(obj[index]?.GetType());
                var nextHashCode = comparer.GetHashCode(obj[index]!);
                hashCode = this.CombineHashCode(hashCode, nextHashCode);
            }
            return hashCode;
        }
    }
}
