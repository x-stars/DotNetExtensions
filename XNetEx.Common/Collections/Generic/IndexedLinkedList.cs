using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace XNetEx.Collections.Generic;

/// <summary>
/// 表示能够通过索引访问的双重链接列表。
/// </summary>
/// <typeparam name="T">指定链接列表的元素类型。</typeparam>
[Serializable]
public class IndexedLinkedList<T> : LinkedList<T>, IList, IList<T>, IReadOnlyList<T>
{
    /// <summary>
    /// 初始化为空的 <see cref="IndexedLinkedList{T}"/> 类的新实例。
    /// </summary>
    public IndexedLinkedList()
    {
    }

    /// <summary>
    /// 初始化 <see cref="IndexedLinkedList{T}"/> 类的新实例，
    /// 该实例包含从指定的 <see cref="IEnumerable{T}"/> 中复制的元素并且其容量足以容纳所复制的元素数。
    /// </summary>
    /// <param name="collection"><see cref="IEnumerable{T}"/>，
    /// 它的元素被复制到新 <see cref="IndexedLinkedList{T}"/>。</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
    public IndexedLinkedList(IEnumerable<T> collection)
        : base(collection)
    {
    }

    /// <summary>
    /// 初始化 <see cref="IndexedLinkedList{T}"/> 类的新实例，
    /// 该实例可使用指定的 <see cref="SerializationInfo"/> 和 <see cref="StreamingContext"/> 进行序列化。
    /// </summary>
    /// <param name="info">一个 <see cref="SerializationInfo"/> 对象，
    /// 包含序列化 <see cref="IndexedLinkedList{T}"/> 所需的信息。</param>
    /// <param name="context">一个 <see cref="StreamingContext"/> 结构，
    /// 包含与 <see cref="IndexedLinkedList{T}"/> 关联的序列化流的源和目标。</param>
    protected IndexedLinkedList(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

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

    /// <inheritdoc/>
    object? IList.this[int index]
    {
        get => this[index];
        set => this[index] = (T)value!;
    }

    /// <inheritdoc/>
    bool IList.IsReadOnly => false;

    /// <inheritdoc/>
    bool IList.IsFixedSize => false;

    /// <summary>
    /// 将对象添加到 <see cref="IndexedLinkedList{T}"/> 的结尾处。
    /// </summary>
    /// <param name="item">要添加到 <see cref="IndexedLinkedList{T}"/> 末尾的对象。
    /// 对于引用类型，该值可以为 <see langword="null"/>。</param>
    public void Add(T item) => this.AddLast(item);

    /// <summary>
    /// 搜索指定的对象，并返回整个 <see cref="IndexedLinkedList{T}"/> 中第一个匹配项的从零开始的索引。
    /// </summary>
    /// <param name="item">要在 <see cref="IndexedLinkedList{T}"/> 中定位的对象。
    /// 对于引用类型，该值可以为 <see langword="null"/>。</param>
    /// <returns>如果在整个 <paramref name="item"/> 中找到 <see cref="IndexedLinkedList{T}"/> 的匹配项，
    /// 则为第一个匹配项的从零开始的索引；否则为 -1。</returns>
    public int IndexOf(T item)
    {
        var index = 0;
        var node = this.First;
        var comparer = EqualityComparer<T>.Default;
        while (node is not null)
        {
            if (comparer.Equals(node.Value, item))
            {
                return index;
            }
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
    /// 搜索指定的对象，并返回整个 <see cref="IndexedLinkedList{T}"/> 中最后一个匹配项的从零开始的索引。
    /// </summary>
    /// <param name="item">要在 <see cref="IndexedLinkedList{T}"/> 中定位的对象。
    /// 对于引用类型，该值可以为 <see langword="null"/>。</param>
    /// <returns>如果在整个 <paramref name="item"/> 中找到 <see cref="IndexedLinkedList{T}"/> 的匹配项，
    /// 则为最后一个匹配项的从零开始的索引；否则为 -1。</returns>
    public int LastIndexOf(T item)
    {
        var index = this.Count - 1;
        var node = this.Last;
        var comparer = EqualityComparer<T>.Default;
        while (node is not null)
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
            for (int front = 0; front < index; front++)
            {
                node = node!.Next;
            }
            return node!;
        }
        else
        {
            var node = this.Last;
            for (int back = 1; back < this.Count - index; back++)
            {
                node = node!.Previous;
            }
            return node!;
        }
    }

    /// <summary>
    /// 移除 <see cref="IndexedLinkedList{T}"/> 的指定索引处的元素。
    /// </summary>
    /// <param name="index">要移除的元素的从零开始的索引。</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> 小于 0，
    /// 或 <paramref name="index"/> 大于 <see cref="LinkedList{T}.Count"/>。</exception>
    public void RemoveAt(int index)
    {
        if ((index < 0) || (index > this.Count - 1))
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        this.Remove(this.NodeAt(index)!);
    }

    /// <inheritdoc/>
    int IList.Add(object? value) { this.Add((T)value!); return this.Count; }

    /// <inheritdoc/>
    void IList.Clear() => this.Clear();

    /// <inheritdoc/>
    bool IList.Contains(object? value) => ((IList)this).IndexOf(value) >= 0;

    /// <inheritdoc/>
    int IList.IndexOf(object? value)
    {
        var index = 0;
        foreach (var item in this)
        {
            if (object.Equals(item, value))
            {
                return index;
            }
            index++;
        }
        return -1;
    }

    /// <inheritdoc/>
    void IList.Insert(int index, object? value) => this.Insert(index, (T)value!);

    /// <inheritdoc/>
    void IList.Remove(object? value) => this.Remove((T)value!);

    /// <inheritdoc/>
    void IList.RemoveAt(int index) => this.RemoveAt(index);
}
