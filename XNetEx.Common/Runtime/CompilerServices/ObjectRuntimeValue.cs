using System.Runtime.CompilerServices;

namespace XNetEx.Runtime.CompilerServices;

/// <summary>
/// 提供对象在运行时包含的值相关的帮助方法。
/// </summary>
public static partial class ObjectRuntimeValue
{
    /// <summary>
    /// 确定当前对象与指定对象的引用是否相等。
    /// </summary>
    /// <typeparam name="T">对象的类型。</typeparam>
    /// <param name="value">要进行引用相等比较的对象。</param>
    /// <param name="other">要与当前对象进行比较的对象。</param>
    /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的引用相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ReferenceEquals<T>(this T? value, T? other)
        where T : class
    {
        return object.ReferenceEquals(value, other);
    }

    /// <summary>
    /// 获取当前对象基于引用的哈希代码。
    /// </summary>
    /// <typeparam name="T">对象的类型。</typeparam>
    /// <param name="value">要获取基于引用的哈希代码的对象。</param>
    /// <returns><paramref name="value"/> 基于引用的哈希代码。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetReferenceHashCode<T>(this T? value)
        where T : class
    {
        return RuntimeHelpers.GetHashCode(value!);
    }

    /// <summary>
    /// 确定当前对象与指定对象的计算堆栈值是否相等。
    /// </summary>
    /// <remarks>对于引用类型，计算堆栈值为对象的引用；
    /// 对于值类型，计算堆栈值为对象所有字段的序列。</remarks>
    /// <typeparam name="T">对象的类型。</typeparam>
    /// <param name="value">要进行计算堆栈值相等比较的对象。</param>
    /// <param name="other">要与当前对象进行比较的对象。</param>
    /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的计算堆栈值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe bool RuntimeEquals<T>(this T? value, T? other)
    {
        return RuntimeHelpers.Equals(value, other);
    }
}
