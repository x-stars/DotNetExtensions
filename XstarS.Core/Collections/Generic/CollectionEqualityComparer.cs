using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供泛型集合 <see cref="IEnumerable{T}"/> 的元素序列的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">集合中的元素的类型。</typeparam>
    [Serializable]
    public class CollectionEqualityComparer<T> : EqualityComparer<IEnumerable<T>>
    {
        /// <summary>
        /// 初始化 <see cref="CollectionEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public CollectionEqualityComparer() : this(null) { }

        /// <summary>
        /// 以比较元素时要使用的比较器初始化 <see cref="CollectionEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        /// <param name="comparer">比较集合中的元素时要使用的比较器。</param>
        public CollectionEqualityComparer(IEqualityComparer<T> comparer = null)
        {
            this.ItemComparer = comparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// 获取默认的 <see cref="CollectionEqualityComparer{T}"/> 实例。
        /// </summary>
        public static new CollectionEqualityComparer<T> Default { get; } = new CollectionEqualityComparer<T>();

        /// <summary>
        /// 获取比较集合中的元素时使用的比较器。
        /// </summary>
        protected IEqualityComparer<T> ItemComparer { get; }

        /// <summary>
        /// 确定两个 <see cref="IEnumerable{T}"/> 对象中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>如果两个集合的所有元素对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            var comparer = this.ItemComparer;

            if (x.GetType() != y.GetType()) { return false; }

            if ((x is ICollection<T> xCollection) && (y is ICollection<T> yCollection))
            {
                if (xCollection.Count != yCollection.Count) { return false; }
            }

            using (IEnumerator<T>
                xEnumerator = x.GetEnumerator(),
                yEnumerator = y.GetEnumerator())
            {
                bool xHasNext, yHasNext;
                while ((xHasNext = xEnumerator.MoveNext()) &
                    (yHasNext = yEnumerator.MoveNext()))
                {
                    var xItem = xEnumerator.Current;
                    var yItem = yEnumerator.Current;
                    if (!comparer.Equals(xItem, yItem))
                    {
                        return false;
                    }
                }
                return xHasNext == yHasNext;
            }
        }

        /// <summary>
        /// 获取 <see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns><see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。</returns>
        public override int GetHashCode(IEnumerable<T> obj)
        {
            if (obj is null) { return 0; }

            var comparer = this.ItemComparer;

            var hashCode = obj.GetType().GetHashCode();
            foreach (var item in obj)
            {
                var nextHashCode = comparer.GetHashCode(item);
                hashCode = hashCode * -1521134295 + nextHashCode;
            }
            return hashCode;
        }
    }
}
