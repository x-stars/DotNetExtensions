using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 可进行相等比较的可通过索引访问的对象的强类型列表。
    /// </summary>
    /// <typeparam name="T">列表中元素的类型。</typeparam>
    public class EquatableList<T> : List<T>, IEquatable<EquatableList<T>>
    {
        /// <summary>
        /// 初始化 <see cref="EquatableList{T}"/> 类的新实例，该实例为空并且具有默认初始容量。
        /// </summary>
        public EquatableList() : base() { }

        /// <summary>
        /// 初始化 <see cref="EquatableList{T}"/> 类的新实例，该实例为空并且具有指定的初始容量。
        /// </summary>
        /// <param name="capacity">新列表最初可以存储的元素数。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> 小于 0。</exception>
        public EquatableList(int capacity) : base(capacity) { }

        /// <summary>
        /// 初始化 <see cref="EquatableList{T}"/> 类的新实例，
        /// 该实例包含从指定集合复制的元素并且具有足够的容量来容纳所复制的元素。
        /// </summary>
        /// <param name="collection">一个集合，其元素被复制到新列表中。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public EquatableList(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="EquatableList{T}"/> 对象是否表示相同的值。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="EquatableList{T}"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的所有元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(EquatableList<T> other)
        {
            if (other is null) { return false; }
            if (this.Count != other.Count) { return false; }

            bool isEqual = true;
            var comparer = EqualityComparer<T>.Default;
            var thisIter = this.GetEnumerator();
            var otherIter = other.GetEnumerator();
            while (thisIter.MoveNext() & otherIter.MoveNext())
            {
                if (!comparer.Equals(thisIter.Current, otherIter.Current))
                { isEqual = false; break; }
            }
            return isEqual;
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否表示相同的值。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="EquatableList{T}"/> 的实例，
        /// 且所有元素均相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as EquatableList<T>);
        }

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            int hashCode = 879525830;
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in this)
            { hashCode = hashCode * -1521134295 + comparer.GetHashCode(item); }
            return hashCode;
        }

        /// <summary>
        /// 返回 <see cref="EquatableList{T}"/> 的字符串表示形式。
        /// </summary>
        /// <returns><see cref="EquatableList{T}"/> 的字符串表示形式。</returns>
        public override string ToString()
        {
            var collectionStringBuilder = new StringBuilder("{ ");
            foreach (var item in this)
            { collectionStringBuilder.Append($"{item.ToString()}, "); }
            collectionStringBuilder.Append("}");
            return collectionStringBuilder.ToString();
        }

        /// <summary>
        /// 指示两 <see cref="EquatableList{T}"/> 对象是否相等。
        /// </summary>
        /// <param name="list1">第一个对象。</param>
        /// <param name="list2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="list1"/> 与 <paramref name="list2"/> 的对应元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(EquatableList<T> list1, EquatableList<T> list2) =>
            EqualityComparer<EquatableList<T>>.Default.Equals(list1, list2);

        /// <summary>
        /// 指示两 <see cref="EquatableList{T}"/> 对象是否不相等。
        /// </summary>
        /// <param name="list1">第一个对象。</param>
        /// <param name="list2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="list1"/> 与 <paramref name="list2"/> 的存在不相等的对应元素，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(EquatableList<T> list1, EquatableList<T> list2) =>
            !(list1 == list2);
    }
}
