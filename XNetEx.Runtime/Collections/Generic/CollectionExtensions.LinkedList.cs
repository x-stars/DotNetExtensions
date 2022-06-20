using System;
using System.Collections.Generic;

namespace XNetEx.Collections.Generic;

static partial class CollectionExtensions
{
    /// <summary>
    /// 返回位于 <see cref="LinkedList{T}"/> 开始处的对象但不将其移除。
    /// </summary>
    /// <typeparam name="T"><see cref="LinkedList{T}"/> 中的元素的类型。</typeparam>
    /// <param name="linkedList">要获取头部值的 <see cref="LinkedList{T}"/> 对象。</param>
    /// <returns><paramref name="linkedList"/> 开始处的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="linkedList"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="linkedList"/> 为空。</exception>
    public static T PeekFirst<T>(this LinkedList<T> linkedList)
    {
        if (linkedList is null)
        {
            throw new ArgumentNullException(nameof(linkedList));
        }

        var firstNode = linkedList.First;
        linkedList.RemoveFirst();
        linkedList.AddFirst(firstNode!);
        return firstNode!.Value;
    }

    /// <summary>
    /// 返回位于 <see cref="LinkedList{T}"/> 结尾处的对象但不将其移除。
    /// </summary>
    /// <typeparam name="T"><see cref="LinkedList{T}"/> 中的元素的类型。</typeparam>
    /// <param name="linkedList">要获取尾部值的 <see cref="LinkedList{T}"/> 对象。</param>
    /// <returns><paramref name="linkedList"/> 结尾处的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="linkedList"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="linkedList"/> 为空。</exception>
    public static T PeekLast<T>(this LinkedList<T> linkedList)
    {
        if (linkedList is null)
        {
            throw new ArgumentNullException(nameof(linkedList));
        }

        var lastNode = linkedList.Last;
        linkedList.RemoveLast();
        linkedList.AddLast(lastNode!);
        return lastNode!.Value;
    }

    /// <summary>
    /// 移除并返回位于 <see cref="LinkedList{T}"/> 开始处的对象。
    /// </summary>
    /// <typeparam name="T"><see cref="LinkedList{T}"/> 中的元素的类型。</typeparam>
    /// <param name="linkedList">要移除头部值的 <see cref="LinkedList{T}"/> 对象。</param>
    /// <returns>从 <paramref name="linkedList"/> 的开始处移除的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="linkedList"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="linkedList"/> 为空。</exception>
    public static T PopFirst<T>(this LinkedList<T> linkedList)
    {
        if (linkedList is null)
        {
            throw new ArgumentNullException(nameof(linkedList));
        }

        var firstNode = linkedList.First;
        linkedList.RemoveFirst();
        return firstNode!.Value;
    }

    /// <summary>
    /// 移除并返回位于 <see cref="LinkedList{T}"/> 结尾处的对象。
    /// </summary>
    /// <typeparam name="T"><see cref="LinkedList{T}"/> 中的元素的类型。</typeparam>
    /// <param name="linkedList">要移除尾部值的 <see cref="LinkedList{T}"/> 对象。</param>
    /// <returns>从 <paramref name="linkedList"/> 的结尾处移除的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="linkedList"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="linkedList"/> 为空。</exception>
    public static T PopLast<T>(this LinkedList<T> linkedList)
    {
        if (linkedList is null)
        {
            throw new ArgumentNullException(nameof(linkedList));
        }

        var lastNode = linkedList.Last;
        linkedList.RemoveLast();
        return lastNode!.Value;
    }
}
