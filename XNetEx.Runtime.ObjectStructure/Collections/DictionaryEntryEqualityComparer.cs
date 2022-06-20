using System;
using System.Collections;
using System.Collections.Generic;
using XNetEx.Collections.Generic;

namespace XNetEx.Collections;

using ObjectPair = KeyValuePair<object, object>;

/// <summary>
/// 提供 <see cref="DictionaryBase"/> 中的键值的相对比较方法。
/// </summary>
[Serializable]
internal sealed class DictionaryEntryEqualityComparer : StructuralEqualityComparer<DictionaryEntry>
{
    /// <summary>
    /// 初始化 <see cref="DictionaryEntryEqualityComparer"/> 类的新实例。
    /// </summary>
    public DictionaryEntryEqualityComparer() { }

    /// <summary>
    /// 确定两个指定的 <see cref="DictionaryEntry"/> 中的键值是否相等。
    /// </summary>
    /// <param name="x">要比较的第一个 <see cref="IEnumerable"/> 对象。</param>
    /// <param name="y">要比较的第二个 <see cref="IEnumerable"/> 对象。</param>
    /// <param name="compared">已经比较过的对象。</param>
    /// <returns>如果 <paramref name="x"/> 和 <paramref name="y"/> 中的键值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    protected override bool EqualsCore(
        DictionaryEntry x, DictionaryEntry y, ISet<ObjectPair> compared)
    {
        if (x.Key?.GetType() != y.Key?.GetType()) { return false; }
        if (x.Value?.GetType() != y.Value?.GetType()) { return false; }

        var keyComparer = StructuralEqualityComparer.OfType(x.Key?.GetType());
        var valueComparer = StructuralEqualityComparer.OfType(x.Value?.GetType());
        return keyComparer.Equals(x.Key, y.Key, compared) &&
            valueComparer.Equals(x.Value, y.Value, compared);
    }

    /// <summary>
    /// 获取指定的 <see cref="DictionaryEntry"/> 中的键值的哈希代码。
    /// </summary>
    /// <param name="obj">要为其获取哈希代码的 <see cref="IEnumerable"/> 对象。</param>
    /// <param name="computed">已经计算过哈希代码的对象。</param>
    /// <returns><paramref name="obj"/> 中的键值的哈希代码。</returns>
    protected override int GetHashCodeCore(DictionaryEntry obj, ISet<object> computed)
    {
        var keyComparer = StructuralEqualityComparer.OfType(obj.Key?.GetType());
        var valueComparer = StructuralEqualityComparer.OfType(obj.Value?.GetType());
        return this.CombineHashCode(
            keyComparer.GetHashCode(obj, computed),
            valueComparer.GetHashCode(obj, computed));
    }
}
