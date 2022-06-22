using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using XNetEx.Collections;
using XNetEx.Collections.Generic;
using XNetEx.Reflection;
#if NET471_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace XNetEx.Diagnostics;

/// <summary>
/// 提供将结构化对象表示为字符串的方法。
/// </summary>
public static class StructuralRepresenter
{
    /// <summary>
    /// 表示指定类型的 <see cref="StructuralRepresenter{T}"/> 类的默认实例。
    /// </summary>
    private static readonly ConcurrentDictionary<Type, IAcyclicRepresenter> Defaults =
        new ConcurrentDictionary<Type, IAcyclicRepresenter>();

    /// <summary>
    /// 将指定对象表示为字符串。
    /// </summary>
    /// <param name="value">要表示为字符串的对象。</param>
    /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
    public static string Represent(object? value)
    {
        return StructuralRepresenter.OfType(value?.GetType()).Represent(value);
    }

    /// <summary>
    /// 获取指定类型的 <see cref="StructuralRepresenter{T}"/> 类的默认实例。
    /// </summary>
    /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
    /// <returns>类型参数为 <paramref name="type"/> 的
    /// <see cref="StructuralRepresenter{T}"/> 类的默认实例。</returns>
    internal static IAcyclicRepresenter OfType(Type? type)
    {
        return (type is null) ? StructuralRepresenter<object>.Default :
            StructuralRepresenter.Defaults.GetOrAdd(type, StructuralRepresenter.GetDefault);
    }

    /// <summary>
    /// 获取指定类型的 <see cref="StructuralRepresenter{T}"/> 类的默认实例。
    /// </summary>
    /// <param name="type">要表示的类型 <see cref="Type"/> 对象。</param>
    /// <returns>类型参数为 <paramref name="type"/> 的
    /// <see cref="StructuralRepresenter{T}"/> 类的默认实例。</returns>
    private static IAcyclicRepresenter GetDefault(Type type)
    {
        var typeRepresenter = typeof(StructuralRepresenter<>).MakeGenericType(type);
        var nameDefualt = nameof(StructuralRepresenter<object>.Default);
        var propertyDefault = typeRepresenter.GetProperty(nameDefualt)!;
        return (IAcyclicRepresenter)propertyDefault.GetValue(null)!;
    }
}

/// <summary>
/// 为结构化对象的字符串表示对象提供抽象基类。
/// </summary>
/// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
internal abstract class StructuralRepresenter<T> : AcyclicRepresenter<T>
{
    /// <summary>
    /// 表示 <see cref="StructuralRepresenter{T}.Default"/> 的延迟初始化值。
    /// </summary>
    private static readonly Lazy<StructuralRepresenter<T>> LazyDefault =
        new Lazy<StructuralRepresenter<T>>(StructuralRepresenter<T>.CreateDefault);

    /// <summary>
    /// 初始化 <see cref="StructuralRepresenter{T}"/> 类的新实例。
    /// </summary>
    protected StructuralRepresenter() { }

    /// <summary>
    /// 获取 <see cref="StructuralRepresenter{T}"/> 类的默认实例。
    /// </summary>
    /// <returns><see cref="StructuralRepresenter{T}"/> 类的默认实例。</returns>
    public new static StructuralRepresenter<T> Default =>
        StructuralRepresenter<T>.LazyDefault.Value;

    /// <summary>
    /// 获取当前类型是否应用了 <see cref="DebuggerDisplayAttribute"/> 特性。
    /// </summary>
    /// <returns>若当前类型应用了 <see cref="DebuggerDisplayAttribute"/> 特性，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    internal static bool HasDebuggerDisplay =>
        typeof(T).IsDefined(typeof(DebuggerDisplayAttribute), inherit: true);

    /// <summary>
    /// 判断当前类型的 <see cref="object.ToString"/> 方法是否为默认定义。
    /// </summary>
    /// <returns>若当前类型的 <see cref="object.ToString"/> 方法定义于
    /// <see cref="object"/> 或 <see cref="ValueType"/> 类，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    internal static bool IsDefaultToString
    {
        get
        {
            var method = typeof(T).GetMethod(nameof(object.ToString), Type.EmptyTypes)!;
            return (method.DeclaringType == typeof(object)) ||
                (method.DeclaringType == typeof(ValueType));
        }
    }

    /// <summary>
    /// 创建 <see cref="StructuralRepresenter{T}"/> 类的默认实例。
    /// </summary>
    /// <returns><see cref="StructuralRepresenter{T}"/> 类的默认实例。</returns>
    private static StructuralRepresenter<T> CreateDefault()
    {
        var type = typeof(T);
        if (type.IsArray)
        {
            var itemType = type.GetElementType();
            if (itemType!.IsPointer)
            {
                return new PointerArrayRepresenter<T>();
            }
            if (itemType.MakeArrayType() == type)
            {
                return (StructuralRepresenter<T>)Activator.CreateInstance(
                    typeof(SZArrayRepresenter<>).MakeGenericType(itemType))!;
            }
            else
            {
                return new ArrayRepresenter<T>();
            }
        }
        else if (type == typeof(string))
        {
            return new PlainRepresenter<T>();
        }
        else if (typeof(IEnumerable).IsAssignableFrom(type))
        {
            return (StructuralRepresenter<T>)Activator.CreateInstance(
                typeof(EnumerableRepresenter<>).MakeGenericType(type))!;
        }
#if NET471_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        else if (typeof(ITuple).IsAssignableFrom(type))
        {
            return (StructuralRepresenter<T>)Activator.CreateInstance(
                typeof(TupleRepresenter<>).MakeGenericType(type))!;
        }
#endif
        else if (type == typeof(DictionaryEntry))
        {
            return (StructuralRepresenter<T>)(object)new DictionaryEntryRepresenter();
        }
        else if (type.IsGenericType &&
            (type.GetGenericTypeDefinition() == typeof(KeyValuePair<,>)))
        {
            var keyValueTypes = type.GetGenericArguments();
            return (StructuralRepresenter<T>)Activator.CreateInstance(
                typeof(KeyValuePairRepresenter<,>).MakeGenericType(keyValueTypes))!;
        }
        else if (StructuralRepresenter<T>.HasDebuggerDisplay)
        {
            return new PlainRepresenter<T>();
        }
        else if (StructuralRepresenter<T>.IsDefaultToString)
        {
            return new MemberRepresenter<T>();
        }
        else
        {
            return new PlainRepresenter<T>();
        }
    }
}
