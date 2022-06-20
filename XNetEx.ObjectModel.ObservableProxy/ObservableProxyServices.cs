using System;

namespace XNetEx.ComponentModel;

/// <summary>
/// 提供属性更改通知代理类型相关的服务方法。
/// </summary>
public static class ObservableProxyServices
{
    /// <summary>
    /// 使用默认构造函数创建指定原型类型的属性更改通知类型的实例。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    /// <returns>一个不使用参数创建的属性更改通知类型的实例。</returns>
    /// <exception cref="MissingMethodException">
    /// <typeparamref name="T"/> 类型不包含无参构造函数。</exception>
    /// <exception cref="MethodAccessException">
    /// <typeparamref name="T"/> 类型的无参构造函数访问级别过低。</exception>
    public static T CreateObservable<T>() where T : class
    {
        return (T)ObservableProxyServices.CreateObservable(typeof(T));
    }

    /// <summary>
    /// 使用默认构造函数创建指定原型类型的属性更改通知类型的实例。
    /// </summary>
    /// <param name="baseType">原型类型，应为接口或非密封类。</param>
    /// <returns>一个不使用参数创建的属性更改通知类型的实例。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="MissingMethodException">
    /// <paramref name="baseType"/> 不包含无参构造函数。</exception>
    /// <exception cref="MethodAccessException">
    /// <paramref name="baseType"/> 的无参构造函数访问级别过低。</exception>
    public static object CreateObservable(Type baseType)
    {
        if (baseType is null) { throw new ArgumentNullException(nameof(baseType)); }
        var provider = ObservableTypeProvider.OfType(baseType);
        return provider.CreateObservableInstance(Array.Empty<object>());
    }

    /// <summary>
    /// 使用与指定参数匹配程度最高的构造函数创建指定原型类型的属性更改通知类型的实例。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
    /// <returns>一个使用指定参数创建的属性更改通知类型的实例。</returns>
    /// <exception cref="MissingMethodException"><typeparamref name="T"/> 类型
    /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
    /// <exception cref="MethodAccessException"><typeparamref name="T"/> 类型
    /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
    public static T CreateObservable<T>(params object?[]? arguments) where T : class
    {
        return (T)ObservableProxyServices.CreateObservable(typeof(T), arguments);
    }

    /// <summary>
    /// 使用与指定参数匹配程度最高的构造函数创建指定原型类型的属性更改通知类型的实例。
    /// </summary>
    /// <param name="baseType">原型类型，应为接口或非密封类。</param>
    /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
    /// <returns>一个使用指定参数创建的属性更改通知类型的实例。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="baseType"/> 为 <see langword="null"/>。</exception>
    /// <exception cref="MissingMethodException"><paramref name="baseType"/>
    /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
    /// <exception cref="MethodAccessException"><paramref name="baseType"/>
    /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
    public static object CreateObservable(Type baseType, params object?[]? arguments)
    {
        if (baseType is null) { throw new ArgumentNullException(nameof(baseType)); }
        var provider = ObservableTypeProvider.OfType(baseType);
        return provider.CreateObservableInstance(arguments);
    }
}
