using System;
using System.Collections;
using System.Collections.Generic;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 提供 <see cref="IEnumerable{T}"/> 基于元素的相等比较的方法。
    /// </summary>
    /// <typeparam name="TCollection">实现了 <see cref="IEnumerable{T}"/> 的集合类型。</typeparam>
    [Serializable]
    internal sealed class EnumerableEqualityComparer<TCollection>
        : CollectionEqualityComparer<TCollection>
    {
        /// <summary>
        /// 表示用于比较集合中元素的 <see cref="IEqualityComparer"/>。
        /// </summary>
        private readonly IEqualityComparer ItemComparer;

        /// <summary>
        /// 初始化 <see cref="EnumEqualityComparer{TCollection}"/> 类的新实例。
        /// </summary>
        public EnumerableEqualityComparer()
        {
            this.ItemComparer = EnumerableEqualityComparer<TCollection>.GetItemComparer();
        }

        /// <summary>
        /// 获取当前 <see cref="IEnumerable{T}"/> 中元素的类型。
        /// </summary>
        /// <returns><typeparamref name="TCollection"/> 中元素的类型。</returns>
        private static Type GetItemType()
        {
            var type = typeof(TCollection);
            foreach (var iType in type.GetInterfaces())
            {
                if (iType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return iType.GetGenericArguments()[0];
                }
            }
            return null;
        }

        /// <summary>
        /// 获取当前 <see cref="IEnumerable{T}"/> 中元素的比较器。
        /// </summary>
        /// <returns><typeparamref name="TCollection"/> 中元素的比较器。</returns>
        private static IEqualityComparer GetItemComparer()
        {
            var itemType = EnumerableEqualityComparer<TCollection>.GetItemType();
            var comparerType = typeof(CollectionEqualityComparer<>).MakeGenericType(itemType);
            var defaultProperty = comparerType.GetProperty("Default");
            return (IEqualityComparer)defaultProperty.GetValue(null);
        }

        /// <summary>
        /// 确定两个指定的 <see cref="IEnumerable{T}"/> 中所包含的元素是否对应相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <param name="y">要比较的第二个 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 的类型相同且所包含的元素相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(TCollection x, TCollection y)
        {
            var xEnumerable = (IEnumerable)x;
            var yEnumerable = (IEnumerable)y;

            if (object.ReferenceEquals(xEnumerable, yEnumerable)) { return true; }
            if ((xEnumerable is null) ^ (yEnumerable is null)) { return false; }

            var comparer = this.ItemComparer;

            if (xEnumerable.GetType() != yEnumerable.GetType()) { return false; }

            if ((xEnumerable is ICollection<TCollection> xCollection) &&
                (yEnumerable is ICollection<TCollection> yCollection))
            {
                if (xCollection.Count != yCollection.Count) { return false; }
            }

            var xEnumerator = xEnumerable.GetEnumerator();
            var yEnumerator = yEnumerable.GetEnumerator();
            try
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
            finally
            {
                (xEnumerator as IDisposable)?.Dispose();
                (yEnumerator as IDisposable)?.Dispose();
            }
        }

        /// <summary>
        /// 获取指定的 <see cref="IEnumerable{T}"/> 遍历元素得到的哈希代码。
        /// </summary>
        /// <param name="obj">要为其获取哈希代码的 <see cref="IEnumerable{T}"/> 对象。</param>
        /// <returns>遍历 <paramref name="obj"/> 中元素得到的哈希代码。</returns>
        public override int GetHashCode(TCollection obj)
        {
            var enumerable = (IEnumerable)obj;

            if (enumerable is null) { return 0; }

            var comparer = this.ItemComparer;

            var hashCode = enumerable.GetType().GetHashCode();
            foreach (var item in enumerable)
            {
                var nextHashCode = comparer.GetHashCode(item);
                hashCode = this.CombineHashCode(hashCode, nextHashCode);
            }
            return hashCode;
        }
    }
}
