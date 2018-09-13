using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供泛型集合 <see cref="IEnumerable{T}"/> 的相等比较的方法，通过遍历每个元素进行比较。
    /// </summary>
    /// <typeparam name="T">集合中的元素的类型。</typeparam>
    public class CollectionEqualityComparer<T> : EqualityComparer<IEnumerable<T>>
    {
        /// <summary>
        /// 初始化 <see cref="CollectionEqualityComparer{T}"/> 类的新实例。
        /// </summary>
        public CollectionEqualityComparer() { }

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
            if ((x is null) && (y is null)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            using (IEnumerator<T>
                xIter = x.GetEnumerator(),
                yIter = y.GetEnumerator())
            {
                var comparer = EqualityComparer<T>.Default;
                while (xIter.MoveNext() & yIter.MoveNext())
                {
                    if (!comparer.Equals(xIter.Current, yIter.Current))
                    { return false; }
                }
                return true;
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

            int hashCode = 161269689;
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in obj)
            { hashCode = hashCode * -1521134295 + comparer.GetHashCode(item); }
            return hashCode;
        }
    }
}
