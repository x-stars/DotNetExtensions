using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace XNetEx.Reflection;

using MethodDynamicDelegate = Func<object?, object?[]?, object?>;

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
        (instance, method, arguments) => method.GetDynamicDelegate().Invoke(instance, arguments);

    /// <summary>
    /// 表示类型对应的代理创建方法的委托。
    /// </summary>
    private static readonly ConcurrentDictionary<Type, Func<object?[]?, object>> ProxyCreateDelegates =
        new ConcurrentDictionary<Type, Func<object?[]?, object>>();

    /// <summary>
    /// 表示方法对应的动态调用委托。
    /// </summary>
    private static readonly ConcurrentDictionary<MethodInfo, MethodDynamicDelegate> MethodDelegates =
        new ConcurrentDictionary<MethodInfo, MethodDynamicDelegate>();

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
        var createDelegate = DispatchProxyServices.ProxyCreateDelegates.GetOrAdd(interfaceType,
            newInterfaceType => typeof(DispatchProxy<>).MakeGenericType(newInterfaceType).GetMethod(
                nameof(DispatchProxy<object>.Create), new[] { newInterfaceType, typeof(InvocationHandler) }
                )!.CreateDynamicDelegate(target: null)!);
        return createDelegate.Invoke(new[] { instance, handler });
    }

    /// <summary>
    /// 获取当前方法的动态调用委托。
    /// </summary>
    /// <param name="method">要获取动态调用委托的 <see cref="MethodInfo"/>。</param>
    /// <returns><paramref name="method"/> 方法的动态调用委托。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
    public static MethodDynamicDelegate GetDynamicDelegate(this MethodInfo method)
    {
        if (method is null)
        {
            throw new ArgumentNullException(nameof(method));
        }

        return DispatchProxyServices.MethodDelegates.GetOrAdd(
            method, newMethod => newMethod.CreateDynamicDelegate());
    }
}
