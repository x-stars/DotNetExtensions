using System;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供泛型集合 <see cref="IEnumerable{T}"/> 的元素序列的相等比较的方法。
    /// </summary>
    /// <typeparam name="T">集合中的元素的类型。</typeparam>
    [Serializable]
    public class SequenceEqualityComparer<T> : EqualityComparer<IEnumerable<T>>
    {
        /// <summary>
        /// 比较集合中的元素时使用的比较器。
        /// </summary>
        private readonly IEqualityComparer<T> ItemComparer;

        /// <summary>
        /// 初始化 <see cref="SequenceEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public SequenceEqualityComparer() : this(false, null) { }

        /// <summary>
        /// 初始化 <see cref="SequenceEqualityComparer{T}"/> 类的新实例，
        /// 并指定进行比较时是否忽略集合的类型和比较元素时要使用的比较器。
        /// </summary>
        /// <param name="ignoreType">指定进行比较时是否忽略集合的类型。</param>
        /// <param name="comparer">指定比较元素时要使用的比较器。</param>
        public SequenceEqualityComparer(
            bool ignoreType = false, IEqualityComparer<T> comparer = null)
        {
            this.IgnoreType = ignoreType;
            this.ItemComparer = comparer ?? EqualityComparer<T>.Default;
        }

        /// <summary>
        /// 返回一个默认的 <see cref="SequenceEqualityComparer{T}"/> 实例。
        /// </summary>
        public static new SequenceEqualityComparer<T> Default { get; } =
            new SequenceEqualityComparer<T>();

        /// <summary>
        /// 指示此比较器在进行比较时是否忽略集合本身的类型。
        /// </summary>
        public bool IgnoreType { get; }

        /// <summary>
        /// 确定两个 <see cref="IEnumerable{T}"/> 对象中所包含的元素是否均相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>
        /// 如果两个集合的所有元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            if (!this.IgnoreType && (x.GetType() != y.GetType())) { return false; }
            if ((x is ICollection<T> xc) && (y is ICollection<T> yc) &&
                (xc.Count != yc.Count)) { return false; }

            using (IEnumerator<T>
                xEtor = x.GetEnumerator(),
                yEtor = y.GetEnumerator())
            {
                bool xHasNext, yHasNext;
                var comparer = this.ItemComparer;
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
        }

        /// <summary>
        /// 获取 <see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns><see cref="IEnumerable{T}"/> 对象遍历元素得到的哈希代码。</returns>
        public override int GetHashCode(IEnumerable<T> obj)
        {
            if (obj is null) { return 0; }

            int hashCode = this.IgnoreType ?
                this.GetType().GetHashCode() : obj.GetType().GetHashCode();
            var comparer = this.ItemComparer;
            foreach (var item in obj)
            {
                hashCode = hashCode * -1521134295 + comparer.GetHashCode(item);
            }
            return hashCode;
        }
    }
}
