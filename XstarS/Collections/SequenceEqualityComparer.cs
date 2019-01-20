using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供公开枚举数 <see cref="IEnumerable"/> 的元素序列的相等比较的方法。
    /// </summary>
    public class SequenceEqualityComparer : IEqualityComparer
    {
        /// <summary>
        /// 比较集合中的元素时使用的比较器。
        /// </summary>
        private readonly IEqualityComparer comparer;

        /// <summary>
        /// 初始化 <see cref="SequenceEqualityComparer"/> 类的新实例。
        /// </summary>
        public SequenceEqualityComparer() : this(false, null) { }

        /// <summary>
        /// 初始化 <see cref="SequenceEqualityComparer"/> 类的新实例，
        /// 并指定进行比较时是否忽略集合的类型和比较元素时要使用的比较器。
        /// </summary>
        /// <param name="ignoreType">指定进行比较时是否忽略集合的类型。</param>
        /// <param name="comparer">指定比较元素时要使用的比较器。</param>
        public SequenceEqualityComparer(
            bool ignoreType = false, IEqualityComparer comparer = null)
        {
            this.IgnoreType = ignoreType;
            this.comparer = comparer ?? EqualityComparer<object>.Default;
        }

        /// <summary>
        /// 指示此比较器在进行比较时是否忽略集合本身的类型。
        /// </summary>
        public bool IgnoreType { get; }

        /// <summary>
        /// 确定两个 <see cref="IEnumerable"/> 对象中所包含的元素是否均相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="IEnumerable"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="IEnumerable"/> 对象。</param>
        /// <returns>
        /// 如果两个集合的所有元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="x"/> 或 <paramref name="y"/>
        /// 不为 <see cref="IEnumerable"/> 接口的对象。</exception>
        public new bool Equals(object x, object y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            if ((x is IEnumerable xe) && (y is IEnumerable ye))
            {
                if (!this.IgnoreType && (xe.GetType() != ye.GetType())) { return false; }
                if ((xe is ICollection xc) && (ye is ICollection yc) &&
                    (xc.Count != yc.Count)) { return false; }

                var xEtor = xe.GetEnumerator();
                var yEtor = ye.GetEnumerator();
                bool xHasNext, yHasNext;
                var comparer = this.comparer;
                while ((xHasNext = xEtor.MoveNext()) &
                    (yHasNext = yEtor.MoveNext()))
                {
                    if (!comparer.Equals(xEtor.Current, yEtor.Current))
                    {
                        return false;
                    }
                }
                return xHasNext == yHasNext;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// 获取 <see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns><see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="obj"/> 不为 <see cref="IEnumerable"/> 接口的对象。</exception>
        public int GetHashCode(object obj)
        {
            if (obj is null) { return 0; }

            if (obj is IEnumerable collection)
            {
                int hashCode = this.IgnoreType ?
                    this.GetType().GetHashCode() : obj.GetType().GetHashCode();
                var comparer = this.comparer;
                foreach (var item in collection)
                {
                    hashCode = hashCode * -1521134295 + comparer.GetHashCode(item);
                }
                return hashCode;
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
