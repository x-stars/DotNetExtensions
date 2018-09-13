using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 表示能够通过索引访问并支持相等比较的双重链接列表。
    /// </summary>
    public class IndexedLinkedList<T> : LinkedList<T>,
        IList, IList<T>, IReadOnlyList<T>, IEquatable<IndexedLinkedList<T>>
    {
        /// <summary>
        /// 初始化为空的 <see cref="IndexedLinkedList{T}"/> 类的新实例。
        /// </summary>
        public IndexedLinkedList() : base() { }

        /// <summary>
        /// 初始化 <see cref="IndexedLinkedList{T}"/> 类的新实例，
        /// 该实例包含从指定的 <see cref="IEnumerable{T}"/> 中复制的元素并且其容量足以容纳所复制的元素数。
        /// </summary>
        /// <param name="collection"><see cref="IEnumerable{T}"/>，
        /// 它的元素被复制到新 <see cref="IndexedLinkedList{T}"/>。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public IndexedLinkedList(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        /// 用序列化数据初始化 <see cref="IndexedLinkedList{T}"/> 类的新实例。
        /// </summary>
        /// <param name="info">
        /// 一个 <see cref="SerializationInfo"/> 对象
        /// 包含序列化 <see cref="IndexedLinkedList{T}"/> 所需的信息。</param>
        /// <param name="context">
        /// 一个 <see cref="StreamingContext"/> 结构
        /// 包含与 <see cref="IndexedLinkedList{T}"/> 关联的序列化流的源和目标。</param>
        protected IndexedLinkedList(SerializationInfo info, StreamingContext context) :
            base(info, context) { }

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获取或设置的元素的从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/> 大于或等于 <see cref="LinkedList{T}.Count"/>。</exception>
        public T this[int index]
        {
            get
            {
                if ((index < 0) || (index > this.Count - 1))
                { throw new ArgumentOutOfRangeException(nameof(index)); }

                if (index < this.Count / 2)
                {
                    var node = this.First;
                    for (int i = 0; i < index; i++)
                    { node = node.Next; }
                    return node.Value;
                }
                else
                {
                    var node = this.Last;
                    for (int i = this.Count - 1; i > index; i--)
                    { node = node.Previous; }
                    return node.Value;
                }
            }
            set
            {
                if ((index < 0) || (index > this.Count - 1))
                { throw new ArgumentOutOfRangeException(nameof(index)); }

                if (index < this.Count / 2)
                {
                    var node = this.First;
                    for (int i = 0; i < index; i++)
                    { node = node.Next; }
                    node.Value = value;
                }
                else
                {
                    var node = this.Last;
                    for (int i = this.Count - 1; i > index; i--)
                    { node = node.Previous; }
                    node.Value = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获取或设置的元素的从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 不是 <see cref="IList"/> 中的有效索引。</exception>
        /// <exception cref="InvalidCastException">
        /// 无法将 <see langword="value"/> 转换为 <typeparamref name="T"/> 类型的对象。</exception>
        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = (T)value;
        }

        /// <summary>
        /// 获取一个值，该值指示 <see cref="IList"/> 是否为只读。
        /// </summary>
        /// <returns><see cref="IndexedLinkedList{T}"/> 不为只读，
        /// 总是返回 <see langword="false"/>。</returns>
        bool IList.IsReadOnly => false;

        /// <summary>
        /// 获取一个值，该值指示 <see cref="IList"/> 是否具有固定大小。
        /// </summary>
        /// <returns><see cref="IndexedLinkedList{T}"/> 不为固定大小，
        /// 总是返回 <see langword="false"/>。</returns>
        bool IList.IsFixedSize => false;

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="IndexedLinkedList{T}"/> 对象是否表示相同的值。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="IndexedLinkedList{T}"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的所有元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(IndexedLinkedList<T> other)
        {
            if (other is null) { return false; }
            if (this.Count != other.Count) { return false; }

            using (IEnumerator<T>
                thisIter = this.GetEnumerator(),
                otherIter = other.GetEnumerator())
            {
                var comparer = EqualityComparer<T>.Default;
                while (thisIter.MoveNext() & otherIter.MoveNext())
                {
                    if (!comparer.Equals(thisIter.Current, otherIter.Current))
                    { return false; }
                }
                return true;
            }
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否表示相同的值。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="IndexedLinkedList{T}"/> 的实例，
        /// 且所有元素均相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as IndexedLinkedList<T>);
        }

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            int hashCode = 583264110;
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in this)
            { hashCode = hashCode * -1521134295 + comparer.GetHashCode(item); }
            return hashCode;
        }

        /// <summary>
        /// 返回 <see cref="IndexedLinkedList{T}"/> 的字符串表示形式。
        /// </summary>
        /// <returns><see cref="IndexedLinkedList{T}"/> 的字符串表示形式。</returns>
        public override string ToString()
        {
            var collectionStringBuilder = new StringBuilder("{ ");
            foreach (var item in this)
            { collectionStringBuilder.Append($"{item.ToString()}, "); }
            collectionStringBuilder.Append("}");
            return collectionStringBuilder.ToString();
        }

        /// <summary>
        /// 将对象添加到 <see cref="IndexedLinkedList{T}"/> 的结尾处。
        /// </summary>
        /// <param name="item">要添加到 <see cref="IndexedLinkedList{T}"/> 末尾的对象。
        /// 对于引用类型，该值可以为 <see langword="null"/>。</param>
        public void Add(T item)
        {
            this.AddLast(item);
        }

        /// <summary>
        /// 搜索指定的对象，并返回整个 <see cref="IndexedLinkedList{T}"/> 中第一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="IndexedLinkedList{T}"/> 中定位的对象。
        /// 对于引用类型，该值可以为 <see langword="null"/>。</param>
        /// <returns>如果在整个 <paramref name="item"/> 中找到 <see cref="IndexedLinkedList{T}"/> 的匹配项，
        /// 则为第一个匹配项的从零开始的索引；否则为 -1。</returns>
        public int IndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            var node = this.First;
            int index = 0;
            while (!(node is null))
            {
                if (comparer.Equals(node.Value, item))
                { return index; }
                node = node.Next;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// 将元素插入 <see cref="IndexedLinkedList{T}"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应插入 <paramref name="item"/> 的从零开始的索引。</param>
        /// <param name="item">要插入的对象。 对于引用类型，该值可以为 <see langword="null"/>。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/>> 大于 <see cref="LinkedList{T}.Count"/>。</exception>
        public void Insert(int index, T item)
        {
            if ((index < 0) || (index > this.Count))
            { throw new ArgumentOutOfRangeException(nameof(index)); }

            if (index == this.Count)
            {
                this.AddLast(item);
            }
            else if (index < this.Count / 2)
            {
                var node = this.First;
                for (int i = 0; i < index; i++)
                { node = node.Next; }
                this.AddBefore(node, item);
            }
            else
            {
                var node = this.Last;
                for (int i = this.Count - 1; i > index; i--)
                { node = node.Previous; }
                this.AddBefore(node, item);
            }
        }

        /// <summary>
        /// 搜索指定的对象，并返回整个 <see cref="IndexedLinkedList{T}"/> 中最后一个匹配项的从零开始的索引。
        /// </summary>
        /// <param name="item">要在 <see cref="IndexedLinkedList{T}"/> 中定位的对象。
        /// 对于引用类型，该值可以为 <see langword="null"/>。</param>
        /// <returns>如果在整个 <paramref name="item"/> 中找到 <see cref="IndexedLinkedList{T}"/> 的匹配项，
        /// 则为最后一个匹配项的从零开始的索引；否则为 -1。</returns>
        public int LastIndexOf(T item)
        {
            var comparer = EqualityComparer<T>.Default;
            var node = this.Last;
            int index = this.Count - 1;
            while (!(node is null))
            {
                if (comparer.Equals(node.Value, item))
                { return index; }
                node = node.Previous;
                index--;
            }
            return -1;
        }

        /// <summary>
        /// 移除 <see cref="IndexedLinkedList{T}"/> 的指定索引处的元素。
        /// </summary>
        /// <param name="index">要移除的元素的从零开始的索引。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/>> 大于 <see cref="LinkedList{T}.Count"/>。</exception>
        public void RemoveAt(int index)
        {
            if ((index < 0) || (index > this.Count - 1))
            { throw new ArgumentOutOfRangeException(nameof(index)); }

            if (index < this.Count / 2)
            {
                var node = this.First;
                for (int i = 0; i < index; i++)
                { node = node.Next; }
                this.Remove(node);
            }
            else
            {
                var node = this.Last;
                for (int i = this.Count - 1; i > index; i--)
                { node = node.Previous; }
                this.Remove(node);
            }
        }

        /// <summary>
        /// 将某项添加到 <see cref="IList"/> 中。
        /// </summary>
        /// <param name="value">要添加到 <see cref="IList"/> 的对象。</param>
        /// <returns>插入了新元素的位置，-1 指示该项未插入到集合中。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法将 <paramref name="value"/> 转换为 <typeparamref name="T"/> 类型的对象。</exception>
        int IList.Add(object value) { this.Add((T)value); return this.Count; }

        /// <summary>
        /// 从 <see cref="IList"/> 中移除所有项。
        /// </summary>
        void IList.Clear() => this.Clear();

        /// <summary>
        ///  确定 <see cref="IList"/> 是否包含特定值。
        /// </summary>
        /// <param name="value">要在 <see cref="IList"/> 中定位的对象。</param>
        /// <returns> 如果在 <see cref="IList"/> 中找到了 <paramref name="value"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        bool IList.Contains(object value) => (value is T item) ? this.Contains(item) : false;

        /// <summary>
        /// 确定 <see cref="IList"/> 中特定项的索引。
        /// </summary>
        /// <param name="value">要在 <see cref="IList"/> 中定位的对象。</param>
        /// <returns>如果在 <see cref="IList"/> 中找到，
        /// 则为 <paramref name="value"/> 的索引；否则为 -1。</returns>
        int IList.IndexOf(object value) => (value is T item) ? this.IndexOf(item) : -1;

        /// <summary>
        /// 在 <see cref="IList"/> 中的指定索引处插入一个项。
        /// </summary>
        /// <param name="index">应插入 <paramref name="value"/> 的从零开始的索引。</param>
        /// <param name="value">要插入到 <see cref="IList"/> 中的对象。</param>
        /// <exception cref="InvalidCastException">
        /// 无法将 <paramref name="value"/> 转换为 <typeparamref name="T"/> 类型的对象。</exception>
        void IList.Insert(int index, object value) => this.Insert(index, (T)value);

        /// <summary>
        /// 从 <see cref="IList"/> 中移除特定对象的第一个匹配项。
        /// </summary>
        /// <param name="value">要从 <see cref="IList"/> 中删除的对象。</param>
        /// <exception cref="InvalidCastException">
        /// 无法将 <paramref name="value"/> 转换为 <typeparamref name="T"/> 类型的对象。</exception>
        void IList.Remove(object value) => this.Remove((T)value);

        /// <summary>
        /// 从 <see cref="IList"/> 中移除位于指定索引处的项。
        /// </summary>
        /// <param name="index">要移除的项的从零开始的索引。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 不是 <see cref="IList"/> 中的有效索引。</exception>
        void IList.RemoveAt(int index) => this.RemoveAt(index);

        /// <summary>
        /// 指示两 <see cref="IndexedLinkedList{T}"/> 对象是否相等。
        /// </summary>
        /// <param name="list1">第一个对象。</param>
        /// <param name="list2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="list1"/> 与 <paramref name="list2"/> 的对应元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(IndexedLinkedList<T> list1, IndexedLinkedList<T> list2) =>
            EqualityComparer<IndexedLinkedList<T>>.Default.Equals(list1, list2);

        /// <summary>
        /// 指示两 <see cref="IndexedLinkedList{T}"/> 对象是否不相等。
        /// </summary>
        /// <param name="list1">第一个对象。</param>
        /// <param name="list2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="list1"/> 与 <paramref name="list2"/> 的存在不相等的对应元素，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(IndexedLinkedList<T> list1, IndexedLinkedList<T> list2) =>
            !(list1 == list2);
    }
}
