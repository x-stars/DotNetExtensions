using System;
using System.Collections;
using System.Collections.Generic;
using XstarS.Collections.Generic;

namespace XstarS.Collections
{
    using ObjectPair = KeyValuePair<object, object>;

    /// <summary>
    /// 提供 <see cref="IEnumerable"/> 中的元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">实现了 <see cref="IEnumerable"/> 接口的集合类型。</typeparam>
    [Serializable]
    internal sealed class EnumerableEqualityComparer<T> : StructuralEqualityComparerBase<T>
        where T : IEnumerable
    {
        /// <summary>
        /// 初始化 <see cref="EnumerableEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public EnumerableEqualityComparer() { }

        /// <summary>
        /// 确定两个指定的 <see cref="IEnumerable"/> 中的元素是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="compared">已经比较过的对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected override bool EqualsCore(T x, T y, ISet<ObjectPair> compared)
        {
            if ((x is ICollection<T> xCollection) &&
                (y is ICollection<T> yCollection))
            {
                if (xCollection.Count != yCollection.Count) { return false; }
            }

            var xEnumerator = x.GetEnumerator();
            var yEnumerator = y.GetEnumerator();
            try
            {
                bool xHasNext, yHasNext;
                while ((xHasNext = xEnumerator.MoveNext()) &
                    (yHasNext = yEnumerator.MoveNext()))
                {
                    var xItem = xEnumerator.Current;
                    var yItem = yEnumerator.Current;

                    if (xItem?.GetType() != yItem?.GetType()) { return false; }

                    var comparer = StructuralEqualityComparer.OfType(xItem?.GetType());
                    if (!comparer.Equals(xItem, yItem, compared)) { return false; }
                }
                return xHasNext == yHasNext;
            }
            finally
            {
                (xEnumerator as IDisposable)?.Dispose();
                (yEnumerator as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// 获取指定的 <see cref="IEnumerable"/> 中的元素的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="obj"/> 中的元素的哈希代码。</returns>
        protected override int GetHashCodeCore(T obj, ISet<object> computed)
        {
            var hashCode = obj.GetType().GetHashCode();
            foreach (var item in obj)
            {
                var comparer = StructuralEqualityComparer.OfType(item?.GetType());
                var nextHashCode = comparer.GetHashCode(item, computed);
                hashCode = this.CombineHashCode(hashCode, nextHashCode);
            }
            return hashCode;
        }
    }
}
