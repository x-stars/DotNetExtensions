using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 表示能够通过索引访问的双重链接列表。
    /// </summary>
    public class IndexedLinkedList<T> : LinkedList<T>, IList, IList<T>, IReadOnlyList<T>
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
        protected IndexedLinkedList(SerializationInfo info, StreamingContext context) : base(info, context) { }

        /// <summary>
        /// 获取或设置指定索引处的元素。
        /// </summary>
        /// <param name="index">要获取或设置的元素的从零开始的索引。</param>
        /// <returns>指定索引处的元素。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/> 大于或等于 <see cref="LinkedList{T}.Count"/>。</exception>
        public T this[int index]
        {
            get => this.NodeAt(index).Value;
            set => this.NodeAt(index).Value = value;
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
        /// 将对象添加到 <see cref="IndexedLinkedList{T}"/> 的结尾处。
        /// </summary>
        /// <param name="item">要添加到 <see cref="IndexedLinkedList{T}"/> 末尾的对象。
        /// 对于引用类型，该值可以为 <see langword="null"/>。</param>
        public void Add(T item)
        {
            this.AddLast(item);
        }

        /// <summary>
        /// 将指定的新节点添加到 <see cref="IndexedLinkedList{T}"/> 的结尾处。
        /// </summary>
        /// <param name="node">要添加到 <see cref="IndexedLinkedList{T}"/> 末尾的链表节点。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="node"/> 为 <see langword="null"/>。</exception>
        public void Add(LinkedListNode<T> node)
        {
            this.AddLast(node);
        }

        /// <summary>
        /// 将指定链表中的所有元素添加到 <see cref="IList{T}"/> 的末尾。
        /// </summary>
        /// <param name="list">应将其元素添加到 <see cref="IList{T}"/> 的末尾的链表。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list"/> 为 <see langword="null"/>。</exception>
        public void AddRange(LinkedList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var node = list.First;
            while (!(node is null))
            {
                this.AddLast(node);
                node = node.Next;
            }
        }

        /// <summary>
        /// 将指定链表中的所有元素添加到 <see cref="IList{T}"/> 的末尾。
        /// </summary>
        /// <param name="collection">应将其元素添加到 <see cref="IList{T}"/> 的末尾的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public void AddRange(IEnumerable<T> collection)
        {
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                this.AddLast(item);
            }
        }

        /// <summary>
        /// 创建 <see cref="IndexedLinkedList{T}"/> 中指定元素范围的浅表复制。
        /// </summary>
        /// <param name="index">范围开始处的从零开始的索引。</param>
        /// <param name="count">范围中的元素数。</param>
        /// <returns><see cref="IndexedLinkedList{T}"/> 中指定元素范围的浅表复制。</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 和 <paramref name="count"/>
        /// 不表示 <see cref="IndexedLinkedList{T}"/> 中元素的有效范围。</exception>
        public IndexedLinkedList<T> GetRange(int index, int count)
        {
            if ((index < 0) || (index > this.Count - 1))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if ((count < 0) || (index + count > this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var list = new IndexedLinkedList<T>();
            var node = this.NodeAt(index);
            for (int i = 0; i < count; i++)
            {
                list.AddLast(node);
                node = node.Next;
            }
            return list;
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
                if (comparer.Equals(node.Value, item)) { return index; }
                node = node.Next;
                index++;
            }
            return -1;
        }

        /// <summary>
        /// 将元素插入 <see cref="IndexedLinkedList{T}"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应插入 <paramref name="item"/> 的从零开始的索引。</param>
        /// <param name="item">要插入的对象。对于引用类型，该值可以为 <see langword="null"/>。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/> 大于 <see cref="LinkedList{T}.Count"/>。</exception>
        public void Insert(int index, T item)
        {
            if ((index < 0) || (index > this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == this.Count)
            {
                this.AddLast(item);
            }
            else
            {
                this.AddBefore(this.NodeAt(index), item);
            }
        }

        /// <summary>
        /// 将指定链表中的所有节点插入 <see cref="IndexedLinkedList{T}"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应插入 <paramref name="list"/> 的从零开始的索引。</param>
        /// <param name="list">要插入的链表。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/> 大于 <see cref="LinkedList{T}.Count"/>。</exception>
        public void InsertRange(int index, LinkedList<T> list)
        {
            if ((index < 0) || (index > this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == this.Count)
            {
                var node = list.First;
                while (!(node is null))
                {
                    this.AddLast(node);
                    node = node.Next;
                }
            }
            else
            {
                var indexNode = this.NodeAt(index);
                var node = list.First;
                while (!(node is null))
                {
                    this.AddBefore(indexNode, node);
                    node = node.Next;
                }
            }
        }

        /// <summary>
        /// 将指定集合中的所有元素插入 <see cref="IndexedLinkedList{T}"/> 的指定索引处。
        /// </summary>
        /// <param name="index">应插入 <paramref name="collection"/> 的从零开始的索引。</param>
        /// <param name="collection">要插入的集合。</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/> 大于 <see cref="LinkedList{T}.Count"/>。</exception>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if ((index < 0) || (index > this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index == this.Count)
            {
                foreach (var item in collection)
                {
                    this.AddLast(item);
                }
            }
            else
            {
                var node = this.NodeAt(index);
                foreach (var item in collection)
                {
                    this.AddBefore(node, item);
                }
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
                {
                    return index;
                }
                node = node.Previous;
                index--;
            }
            return -1;
        }

        /// <summary>
        /// 获取或设置指定索引处的链表节点 <see cref="LinkedListNode{T}"/>。
        /// </summary>
        /// <param name="index">要获取的节点的从零开始的索引。</param>
        /// <returns>指定索引处的节点。</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
        /// 或 <paramref name="index"/> 大于或等于 <see cref="LinkedList{T}.Count"/>。</exception>
        public LinkedListNode<T> NodeAt(int index)
        {
            if ((index < 0) || (index > this.Count - 1))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (index < this.Count / 2)
            {
                var node = this.First;
                for (int i = 0; i < index; i++)
                {
                    node = node.Next;
                }
                return node;
            }
            else
            {
                var node = this.Last;
                for (int i = this.Count - 1; i > index; i--)
                {
                    node = node.Previous;
                }
                return node;
            }
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
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            this.Remove(this.NodeAt(index));
        }

        /// <summary>
        /// 从 <see cref="IndexedLinkedList{T}"/> 中移除一定范围的元素。
        /// </summary>
        /// <param name="index">要移除的元素范围的从零开始的起始索引。</param>
        /// <param name="count">要移除的元素数。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 和 <paramref name="count"/>
        /// 不表示 <see cref="IndexedLinkedList{T}"/> 中元素的有效范围。</exception>
        public void RemoveRange(int index, int count)
        {
            if ((index < 0) || (index > this.Count - 1))
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            if ((count < 0) || (index + count > this.Count))
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            var node = this.NodeAt(index);
            var nextNode = node.Next;
            for (int i = 0; i < count; i++)
            {
                this.Remove(node);
                node = nextNode;
                nextNode = node.Next;
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
    }
}
