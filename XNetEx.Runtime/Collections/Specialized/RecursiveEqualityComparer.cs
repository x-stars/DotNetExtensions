using System;
using XNetEx.Collections.Generic;
using XNetEx.Runtime.CompilerServices;

namespace XNetEx.Collections.Specialized;

/// <summary>
/// 提供用于比较对象的值是否递归相等的方法。
/// </summary>
/// <remarks>基于反射调用，可能存在性能问题。
/// 将递归比较至对象的字段（数组的元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
/// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。</remarks>
/// <typeparam name="T">要比较的对象的类型。</typeparam>
[Serializable]
public sealed class RecursiveEqualityComparer<T> : SimpleEqualityComparer<T>
{
    /// <summary>
    /// 初始化 <see cref="RecursiveEqualityComparer{T}"/> 类的新实例。
    /// </summary>
    private RecursiveEqualityComparer() { }

    /// <summary>
    /// 获取 <see cref="RecursiveEqualityComparer{T}"/> 类的默认实例。
    /// </summary>
    /// <returns><see cref="RecursiveEqualityComparer{T}"/> 类的默认实例。</returns>
    public static new RecursiveEqualityComparer<T> Default { get; } = new RecursiveEqualityComparer<T>();

    /// <summary>
    /// 确定两对象的值是否递归相等。
    /// </summary>
    /// <param name="x">要比较的第一个对象。</param>
    /// <param name="y">要比较的第二个对象。</param>
    /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的值相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public override bool Equals(T? x, T? y) => ObjectRuntimeValue.RecursiveEquals(x, y);

    /// <summary>
    /// 获取指定对象递归包含的值的哈希代码。
    /// </summary>
    /// <param name="obj">要获取哈希代码的对象。</param>
    /// <returns><paramref name="obj"/> 基于值的哈希代码。</returns>
    public override int GetHashCode(T? obj) => ObjectRuntimeValue.GetRecursiveHashCode(obj);
}
