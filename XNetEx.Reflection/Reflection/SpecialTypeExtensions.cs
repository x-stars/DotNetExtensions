using System;
using System.Collections.Generic;
using System.Linq;

namespace XNetEx.Reflection;

/// <summary>
/// 提供特殊类型相关的扩展方法。
/// </summary>
public static class SpecialTypeExtensions
{
    /// <summary>
    /// 与可变参数列表方法相关的类型的 <see cref="HashSet{T}"/>。
    /// </summary>
    private static readonly HashSet<Type> VarArgTypes = new HashSet<Type>(
        new[] { "System.ArgIterator", "System.RuntimeArgumentHandle",
            "System.TypedReference" }.Select(Type.GetType).OfType<Type>());

    /// <summary>
    /// 确定当前 <see cref="Type"/> 是否是与可变参数列表方法相关的类型。
    /// </summary>
    /// <param name="type">要确定是否相关的 <see cref="Type"/> 对象。</param>
    /// <returns>若 <paramref name="type"/> 是与可变参数列表方法相关的类型，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
    public static bool IsVarArgType(this Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return SpecialTypeExtensions.VarArgTypes.Contains(type);
    }

    /// <summary>
    /// 确定当前 <see cref="Type"/> 的实例是否不能转换或包装为 <see cref="object"/>。
    /// </summary>
    /// <param name="type">要确定是否转换或包装的 <see cref="Type"/> 对象。</param>
    /// <returns>若 <paramref name="type"/> 的实例不能转换或包装为 <see cref="object"/>，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
    public static bool IsNotBoxable(this Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return type.IsByRef ||
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            (!type.IsGenericParameter && type.IsByRefLike) ||
#endif
            type.IsVarArgType();
    }

    /// <summary>
    /// 确定当前 <see cref="Type"/> 的实例是否不能直接转换为 <see cref="object"/>。
    /// </summary>
    /// <param name="type">要确定是否不能直接转换的 <see cref="Type"/> 对象。</param>
    /// <returns>若 <paramref name="type"/> 的实例不能直接转换为 <see cref="object"/>，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
    public static bool IsNotObject(this Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return type.IsPointer || type.IsNotBoxable();
    }

    /// <summary>
    /// 尝试获取当前 <see cref="Type"/> 引用的对象的 <see cref="Type"/>。
    /// </summary>
    /// <param name="type">要获取引用的类型的 <see cref="Type"/> 对象。</param>
    /// <returns>若 <paramref name="type"/> 表示一个引用类型，则为其引用的对象的
    /// <see cref="Type"/>；否则为 <paramref name="type"/> 本身。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
    public static Type TryDerefType(this Type type)
    {
        if (type is null)
        {
            throw new ArgumentNullException(nameof(type));
        }

        return type.IsByRef ? type.GetElementType()! : type;
    }
}
