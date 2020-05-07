using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供非泛型集合 <see cref="IEnumerable"/> 的元素序列的相等比较的方法。
    /// </summary>
    [Serializable]
    public class CollectionEqualityComparer : EqualityComparer<IEnumerable>
    {
        /// <summary>
        /// 初始化 <see cref="CollectionEqualityComparer"/> 类的新实例。
        /// </summary>
        public CollectionEqualityComparer() : this(recurse: false, null) { }

        /// <summary>
        /// 以比较元素时要使用的比较器初始化 <see cref="CollectionEqualityComparer"/> 类的新实例，
        /// 并指定在是否对内层 <see cref="IEnumerable"/> 进行递归比较。
        /// </summary>
        /// <param name="recurse">指定是否对内层 <see cref="IEnumerable"/> 进行递归比较。</param>
        /// <param name="comparer">比较集合中的元素时要使用的比较器。</param>
        public CollectionEqualityComparer(
            bool recurse = false, IEqualityComparer comparer = null)
        {
            this.Recurse = recurse;
            this.ItemComparer = comparer ?? EqualityComparer<object>.Default;
        }

        /// <summary>
        /// 获取默认的 <see cref="CollectionEqualityComparer"/> 实例。
        /// </summary>
        public static new CollectionEqualityComparer Default { get; } = new CollectionEqualityComparer();

        /// <summary>
        /// 获取当前比较器是否对内层 <see cref="IEnumerable"/> 进行递归比较。
        /// </summary>
        public bool Recurse { get; }

        /// <summary>
        /// 获取比较集合中的元素时使用的比较器。
        /// </summary>
        protected IEqualityComparer ItemComparer { get; }

        /// <summary>
        /// 确定两个 <see cref="IEnumerable"/> 对象中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="IEnumerable"/> 对象。</param>
        /// <returns>如果两个集合的所有元素对应相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(IEnumerable x, IEnumerable y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            var recurse = this.Recurse;
            var comparer = this.ItemComparer;

            if (x.GetType() != y.GetType()) { return false; }

            if ((x is ICollection xCollection) && (y is ICollection yCollection))
            {
                if (xCollection.Count != yCollection.Count) { return false; }
            }

            var xEnumerator = x.GetEnumerator();
            var yEnumerator = y.GetEnumerator();
            bool xHasNext, yHasNext;
            while ((xHasNext = xEnumerator.MoveNext()) &
                (yHasNext = yEnumerator.MoveNext()))
            {
                var xItem = xEnumerator.Current;
                var yItem = yEnumerator.Current;
                if (recurse && (xItem is IEnumerable xInnerEnum) && (yItem is IEnumerable yInnerEnum))
                {
                    if (!this.Equals(xInnerEnum, yInnerEnum))
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
            (xEnumerator as IDisposable)?.Dispose();
            (yEnumerator as IDisposable)?.Dispose();
            return xHasNext == yHasNext;
        }

        /// <summary>
        /// 获取 <see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns><see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="obj"/> 不为 <see cref="IEnumerable"/> 接口的对象。</exception>
        public override int GetHashCode(IEnumerable obj)
        {
            if (obj is null) { return 0; }

            var recurse = this.Recurse;
            var comparer = this.ItemComparer;

            var hashCode = obj.GetType().GetHashCode();
            foreach (var item in obj)
            {
                var nextHashCode = (recurse && (item is IEnumerable innerEnum)) ?
                    this.GetHashCode(innerEnum) : comparer.GetHashCode(item);
                hashCode = hashCode * -1521134295 + nextHashCode;
            }
            return hashCode;
        }
    }
}
