using System.Collections;

namespace XNetEx.Collections;

/// <summary>
/// 提供 <see cref="StructuralComparisons"/> 的扩展方法包装。
/// </summary>
public static class StructuralComparingExtensions
{
    /// <summary>
    /// 确定当前集合对象在排序顺序中的位置是位于另一个对象之前、之后还是与其位置相同。
    /// </summary>
    /// <typeparam name="T">集合对象的类型。</typeparam>
    /// <param name="value">当前 <see cref="IStructuralComparable"/> 对象。</param>
    /// <param name="other">要与当前实例进行比较的对象。</param>
    /// <returns>若 <paramref name="value"/> 小于 <paramref name="other"/>，则小于零；
    /// 若 <paramref name="value"/> 大于 <paramref name="other"/>，则大于零；
    /// 若 <paramref name="value"/> 等于 <paramref name="other"/>，则为零。</returns>
    public static int StructuralCompareTo<T>(this T? value, T? other)
        where T : IStructuralComparable
    {
        var comparer = StructuralComparisons.StructuralComparer;
        return comparer.Compare(value, other);
    }

    /// <summary>
    /// 确定当前实例是否在结构上等于指定对象。
    /// </summary>
    /// <typeparam name="T">集合对象的类型。</typeparam>
    /// <param name="value">当前 <see cref="IStructuralEquatable"/> 对象。</param>
    /// <param name="other">要与当前实例进行比较的对象。</param>
    /// <returns>如果 <paramref name="value"/> 与 <paramref name="other"/> 相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool StructuralEquals<T>(this T? value, T? other)
        where T : IStructuralEquatable
    {
        var comparer = StructuralComparisons.StructuralEqualityComparer;
        return comparer.Equals(value, other);
    }

    /// <summary>
    /// 返回当前实例的结构化哈希代码。
    /// </summary>
    /// <typeparam name="T">集合对象的类型。</typeparam>
    /// <param name="value">当前 <see cref="IStructuralEquatable"/> 对象。</param>
    /// <returns><paramref name="value"/> 的结构化哈希代码。</returns>
    public static int GetStructuralHashCode<T>(this T value)
        where T : IStructuralEquatable
    {
        var comparer = StructuralComparisons.StructuralEqualityComparer;
        return comparer.GetHashCode(value);
    }
}
