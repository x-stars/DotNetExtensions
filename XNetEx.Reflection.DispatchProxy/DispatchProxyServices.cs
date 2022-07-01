using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace XNetEx.Reflection;

/// <summary>
/// 提供代理类型 <see cref="DispatchProxy{TInterface}"/> 相关的服务方法。
/// </summary>
public static class DispatchProxyServices
{
    /// <summary>
    /// 表示默认的 <see cref="InvocationHandler"/> 代理委托。
    /// 此代理委托仅调用被代理方法并返回。
    /// </summary>
    public static readonly InvocationHandler DefaultHandler =
        (instance, method, arguments) => method.Invoke(instance, arguments);

    /// <summary>
    /// 表示类型对应的代理创建方法的 <see cref="MethodInfo"/> 对象。
    /// </summary>
    private static readonly ConcurrentDictionary<Type, MethodInfo> ProxyCreateMethods =
        new ConcurrentDictionary<Type, MethodInfo>();

    /// <summary>
    /// 使用指定的代理对象和代理委托创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。
    /// </summary>
    /// <typeparam name="TInterface">代理应实现的接口类型。</typeparam>
    /// <param name="instance">要代理的 <typeparamref name="TInterface"/> 类型的对象。</param>
    /// <param name="handler">方法调用时所用的 <see cref="InvocationHandler"/> 代理委托。</param>
    /// <returns>实现 <typeparamref name="TInterface"/> 接口的代理类型的对象。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="instance"/>
    /// 或 <paramref name="handler"/> 为 <see langword="null"/>。</exception>
    public static TInterface CreateProxy<TInterface>(TInterface instance, InvocationHandler? handler = null)
        where TInterface : class
    {
        handler ??= DispatchProxyServices.DefaultHandler;
        return DispatchProxy<TInterface>.Create(instance, handler);
    }

    /// <summary>
    /// 使用指定的代理对象和代理委托创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。
    /// </summary>
    /// <param name="interfaceType">代理应实现的接口类型。</param>
    /// <param name="instance">要代理的 <paramref name="interfaceType"/> 类型的对象。</param>
    /// <param name="handler">方法调用时所用的 <see cref="InvocationHandler"/> 代理委托。</param>
    /// <returns>实现 <paramref name="interfaceType"/> 接口的代理类型的对象。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="instance"/>
    /// 或 <paramref name="handler"/> 为 <see langword="null"/>。</exception>
    public static object CreateProxy(Type interfaceType, object instance, InvocationHandler? handler = null)
    {
        if (interfaceType is null) { throw new ArgumentNullException(nameof(interfaceType)); }
        handler ??= DispatchProxyServices.DefaultHandler;
        var createDelegate = DispatchProxyServices.ProxyCreateMethods.GetOrAdd(interfaceType,
            newInterfaceType => typeof(DispatchProxy<>).MakeGenericType(newInterfaceType).GetMethod(
                nameof(DispatchProxy<object>.Create), new[] { newInterfaceType, typeof(InvocationHandler) })!);
        return createDelegate.Invoke(null, new[] { instance, handler })!;
    }
}
