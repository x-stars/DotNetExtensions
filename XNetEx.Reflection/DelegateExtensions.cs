using System;
using System.Collections.Concurrent;
using XNetEx.Reflection;

namespace XNetEx;

using DynamicInvoker = Func<object?, object?[]?, object?>;

/// <summary>
/// 提供委托 <see cref="Delegate"/> 的扩展方法。
/// </summary>
public static class DelegateExtensions
{
    /// <summary>
    /// 表示委托类型对应的动态调用委托。
    /// </summary>
    private static readonly ConcurrentDictionary<Type, DynamicInvoker> InvokeDelegates =
        new ConcurrentDictionary<Type, DynamicInvoker>();

    /// <summary>
    /// 以动态调用委托快速动态调用（后期绑定）由当前委托所表示的方法。
    /// </summary>
    /// <param name="delegate">要进行动态调用的 <see cref="Delegate"/> 对象。</param>
    /// <param name="arguments">作为自变量传递给当前委托所表示的方法的对象数组。</param>
    /// <returns><paramref name="delegate"/> 所表示的方法返回的对象。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
    public static object? DynamicInvokeFast(this Delegate @delegate, params object?[]? arguments)
    {
        var invokeDelegate = @delegate.GetInvokeDynamicDelegate();
        return invokeDelegate.Invoke(@delegate, arguments);
    }

    /// <summary>
    /// 创建当前委托对应的动态调用委托。
    /// </summary>
    /// <param name="delegate">要创建动态调用委托的 <see cref="Delegate"/> 对象。</param>
    /// <returns><paramref name="delegate"/> 的动态调用委托。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
    public static DynamicDelegate ToDynamicDelegate(this Delegate @delegate)
    {
        var invokeDelegate = @delegate.GetInvokeDynamicDelegate();
        return arguments => invokeDelegate.Invoke(@delegate, arguments);
    }

    /// <summary>
    /// 获取当前委托的 <c>Invoke</c> 方法的动态调用委托。
    /// </summary>
    /// <param name="delegate">要创建动态调用委托的 <see cref="Delegate"/> 对象。</param>
    /// <returns><paramref name="delegate"/> 的 <c>Invoke</c> 方法动态调用委托。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
    private static DynamicInvoker GetInvokeDynamicDelegate(this Delegate @delegate)
    {
        if (@delegate is null)
        {
            throw new ArgumentNullException(nameof(@delegate));
        }

        return DelegateExtensions.InvokeDelegates.GetOrAdd(@delegate.GetType(),
            delegateType => delegateType.GetMethod("Invoke")!.CreateDynamicDelegate());
    }
}
