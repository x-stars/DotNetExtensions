using System;

namespace XNetEx.Collections.Generic;

/// <summary>
/// 为无参数的无环相等比较器 <see cref="IAcyclicEqualityComparer{T}"/> 提供抽象基类。
/// </summary>
/// <typeparam name="T">要比较的对象的类型。</typeparam>
[Serializable]
internal abstract class SimpleAcyclicEqualityComparer<T> : AcyclicEqualityComparer<T>
{
    /// <summary>
    /// 初始化 <see cref="SimpleAcyclicEqualityComparer{T}"/> 类的新实例。
    /// </summary>
    protected SimpleAcyclicEqualityComparer() { }

    /// <summary>
    /// 确定指定的对象是否等于当前对象。
    /// </summary>
    /// <param name="obj">要与当前对象进行比较的对象。</param>
    /// <returns>如果指定的对象等于当前对象，
    /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
    public sealed override bool Equals(object? obj) =>
        this.GetType() == obj?.GetType();

    /// <summary>
    /// 获取当前对象的哈希代码。
    /// </summary>
    /// <returns>当前对象的哈希代码。</returns>
    public sealed override int GetHashCode() =>
        this.GetType().GetHashCode() * -1521134295 +
        typeof(SimpleAcyclicEqualityComparer<T>).GetHashCode();
}
