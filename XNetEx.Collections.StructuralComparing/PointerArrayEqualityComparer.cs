﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using XNetEx.Collections.Generic;
using XNetEx.Reflection;

namespace XNetEx;

using ObjectPair = KeyValuePair<object, object>;

/// <summary>
/// 提供指针数组基于元素的相等比较的方法。
/// </summary>
/// <typeparam name="T">指针数组的类型。</typeparam>
[Serializable]
internal sealed class PointerArrayEqualityComparer<T> : StructuralEqualityComparer<T>
{
    /// <summary>
    /// 表示数组根据索引获取元素的方法的 <see cref="MethodInfo"/> 对象。
    /// </summary>
    private static readonly MethodInfo GetMethod = typeof(T).GetMethod("Get")!;

    /// <summary>
    /// 初始化 <see cref="PointerArrayEqualityComparer{T}"/> 类的新实例。
    /// </summary>
    public PointerArrayEqualityComparer() { }

    /// <summary>
    /// 确定两个指定指针数组中所包含的元素是否相等。
    /// </summary>
    /// <param name="x">要比较的第一个指针数组。</param>
    /// <param name="y">要比较的第二个指针数组。</param>
    /// <param name="compared">已经比较过的对象。</param>
    /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的元素相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    protected override bool EqualsCore(
        [DisallowNull] T x, [DisallowNull] T y, ISet<ObjectPair> compared)
    {
        var xArray = (Array)(object)x;
        var yArray = (Array)(object)y;
        if (xArray.Rank != yArray.Rank) { return false; }
        if (xArray.Length != yArray.Length) { return false; }
        foreach (var rank in ..xArray.Rank)
        {
            if ((xArray.GetLowerBound(rank) != yArray.GetLowerBound(rank)) ||
                (xArray.GetLength(rank) != yArray.GetLength(rank)))
            {
                return false;
            }
        }

        var methodGet = PointerArrayEqualityComparer<T>.GetMethod;
        foreach (var indices in xArray.GetIndicesSequence(reuseIndices: true))
        {
            var boxedIndices = indices.Box();
            var xItem = methodGet.Invoke(xArray, boxedIndices);
            var yItem = methodGet.Invoke(yArray, boxedIndices);
            if (!PointerEqualityComparer.Equals(xItem, yItem)) { return false; }
        }
        return true;
    }

    /// <summary>
    /// 获取指定的指针数组中的元素的哈希代码。
    /// </summary>
    /// <param name="obj">要获取哈希代码的指针数组。</param>
    /// <param name="computed">已经计算过哈希代码的对象。</param>
    /// <returns><paramref name="obj"/> 中的元素的哈希代码。</returns>
    protected override int GetHashCodeCore([DisallowNull] T obj, ISet<object> computed)
    {
        var array = (Array)(object)obj;

        var hashCode = array.GetType().GetHashCode();
        var methodGet = PointerArrayEqualityComparer<T>.GetMethod;
        foreach (var indices in array.GetIndicesSequence(reuseIndices: true))
        {
            var item = methodGet.Invoke(array, indices.Box());
            var nextHashCode = PointerEqualityComparer.GetHashCode(item);
            hashCode = this.CombineHashCode(hashCode, nextHashCode);
        }
        return hashCode;
    }
}
