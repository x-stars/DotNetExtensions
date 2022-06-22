using System;
using System.Collections;
using System.Collections.Generic;
using XNetEx.Collections.Generic;

namespace XNetEx.Collections;

using ObjectPair = KeyValuePair<object, object>;

/// <summary>
/// 提供 <see cref="IStructuralEquatable"/> 中的成员的相等比较的方法。
/// </summary>
/// <typeparam name="T">实现了 <see cref="IStructuralEquatable"/> 接口的结构化对象类型。</typeparam>
[Serializable]
internal sealed class StructuralEquatableEqualityComparer<T> : StructuralEqualityComparer<T>
    where T : IStructuralEquatable
{
    /// <summary>
    /// 初始化 <see cref="StructuralEquatableEqualityComparer{T}"/> 类的新实例。
    /// </summary>
    public StructuralEquatableEqualityComparer() { }

    /// <summary>
    /// 确定两个指定的 <see cref="IStructuralEquatable"/> 中的成员是否相等。
    /// </summary>
    /// <param name="x">要比较的第一个 <see cref="IStructuralEquatable"/> 对象。</param>
    /// <param name="y">要比较的第二个 <see cref="IStructuralEquatable"/> 对象。</param>
    /// <param name="compared">已经比较过的对象。</param>
    /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的成员相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    protected override bool EqualsCore(T x, T y, ISet<ObjectPair> compared)
    {
        var comparer = StructuralEqualityComparer.OfType(x.GetType());
        return x.Equals(y, comparer);
    }

    /// <summary>
    /// 获取指定的 <see cref="IStructuralEquatable"/> 中的成员的哈希代码。
    /// </summary>
    /// <param name="obj">要获取哈希代码的 <see cref="IStructuralEquatable"/> 对象。</param>
    /// <param name="computed">已经计算过哈希代码的对象。</param>
    /// <returns><paramref name="obj"/> 中的成员的哈希代码。</returns>
    protected override int GetHashCodeCore(T obj, ISet<object> computed)
    {
        var comparer = StructuralEqualityComparer.OfType(obj.GetType());
        return obj.GetHashCode(comparer);
    }
}
