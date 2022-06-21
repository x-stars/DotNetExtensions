using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace XNetEx.Reflection;

/// <summary>
/// 提供通过反射访问对象的公共属性的方法。
/// </summary>
/// <typeparam name="T">对象的类型。</typeparam>
internal static class SimplePropertyAccessor<T> where T : class
{
    /// <summary>
    /// 表示当前类型的所有公共实例属性的 <see langword="get"/> 访问器方法的元数据对象。
    /// </summary>
    private static readonly ConcurrentDictionary<string, MethodInfo> GetMethods =
        new ConcurrentDictionary<string, MethodInfo>();

    /// <summary>
    /// 表示当前类型的所有公共实例属性的 <see langword="set"/> 访问器方法的元数据对象。
    /// </summary>
    private static readonly ConcurrentDictionary<string, MethodInfo> SetMethods =
        new ConcurrentDictionary<string, MethodInfo>();

    /// <summary>
    /// 获取指定对象的指定公共属性的值。
    /// </summary>
    /// <param name="instance">要获取属性的对象。</param>
    /// <param name="propertyName">要获取值的属性的名称。</param>
    /// <returns><paramref name="instance"/>
    /// 中名为 <paramref name="propertyName"/> 的属性的值。</returns>
    /// <exception cref="MissingMemberException">
    /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="get"/> 属性。</exception>
    public static object? GetValue(T instance, string propertyName)
    {
        var getMethod = SimplePropertyAccessor<T>.GetMethods.GetOrAdd(
            propertyName, SimplePropertyAccessor<T>.FindGetMethod);
        return getMethod.Invoke(instance, Array.Empty<object?>());
    }

    /// <summary>
    /// 设置指定对象的指定公共属性的值。
    /// </summary>
    /// <param name="instance">要设置属性的对象。</param>
    /// <param name="propertyName">要设置值的属性的名称。</param>
    /// <param name="value">属性的新值。</param>
    /// <exception cref="InvalidCastException">
    /// <paramref name="value"/> 无法转换为指定属性的类型。</exception>
    /// <exception cref="MissingMemberException">
    /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="set"/> 属性。</exception>
    public static void SetValue(T instance, string propertyName, object? value)
    {
        var setMethod = SimplePropertyAccessor<T>.SetMethods.GetOrAdd(
            propertyName, SimplePropertyAccessor<T>.FindSetMethod);
        _ = setMethod.Invoke(instance, new object?[] { value });
    }

    /// <summary>
    /// 查找当前类型的指定公共实例属性的 <see langword="get"/> 访问器方法的元数据对象。
    /// </summary>
    /// <param name="propertyName">要查找访问器方法的属性的名称。</param>
    /// <returns><typeparamref name="T"/> 类型中名为 <paramref name="propertyName"/>
    /// 的属性的 <see langword="get"/> 访问器方法的元数据对象。</returns>
    /// <exception cref="MissingMemberException">
    /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="get"/> 属性。</exception>
    private static MethodInfo FindGetMethod(string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName, Type.EmptyTypes);
        if (property?.GetMethod is null) { throw new MissingMemberException(); }
        if (property.GetMethod.IsStatic) { throw new MissingMemberException(); }
        if (!property.GetMethod.IsPublic) { throw new MissingMemberException(); }
        return property.GetMethod;
    }

    /// <summary>
    /// 查找当前类型的指定公共实例属性的 <see langword="set"/> 访问器方法的元数据对象。
    /// </summary>
    /// <param name="propertyName">要查找访问器方法的属性的名称。</param>
    /// <returns><typeparamref name="T"/> 类型中名为 <paramref name="propertyName"/>
    /// 的属性的 <see langword="set"/> 访问器方法的元数据对象。</returns>
    /// <exception cref="MissingMemberException">
    /// 无法访问名为 <paramref name="propertyName"/> 的 <see langword="set"/> 属性。</exception>
    private static MethodInfo FindSetMethod(string propertyName)
    {
        var property = typeof(T).GetProperty(propertyName, Type.EmptyTypes);
        if (property?.SetMethod is null) { throw new MissingMemberException(); }
        if (property.SetMethod.IsStatic) { throw new MissingMemberException(); }
        if (!property.SetMethod.IsPublic) { throw new MissingMemberException(); }
        return property.SetMethod;
    }
}
