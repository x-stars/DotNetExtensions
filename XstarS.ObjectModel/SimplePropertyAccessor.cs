using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace XstarS.Reflection
{
    using DynamicMethodDelegate = Func<object?, object?[]?, object?>;

    /// <summary>
    /// 提供通过反射访问对象的公共属性的方法。
    /// </summary>
    /// <typeparam name="T">对象的类型。</typeparam>
    internal static class SimplePropertyAccessor<T>
    {
        /// <summary>
        /// 表示当前类型的所有公共实例属性的 <see langword="get"/> 访问器的动态调用委托。
        /// </summary>
        private static readonly ConcurrentDictionary<string, DynamicMethodDelegate> GetDelegates =
            new ConcurrentDictionary<string, DynamicMethodDelegate>();

        /// <summary>
        /// 表示当前类型的所有公共实例属性的 <see langword="set"/> 访问器的动态调用委托。
        /// </summary>
        private static readonly ConcurrentDictionary<string, DynamicMethodDelegate> SetDelegates =
            new ConcurrentDictionary<string, DynamicMethodDelegate>();

        /// <summary>
        /// 获取指定对象的指定公共属性的值。
        /// </summary>
        /// <param name="instance">要获取属性的对象。</param>
        /// <param name="propertyName">要获取值的属性的名称。</param>
        /// <returns><paramref name="instance"/>
        /// 中名为 <paramref name="propertyName"/> 的属性的值。</returns>
        /// <exception cref="MissingMemberException">
        /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="get"/> 属性。</exception>
        public static object? GetValue([DisallowNull] T instance, string propertyName)
        {
            var getDelegate = SimplePropertyAccessor<T>.GetDelegates.GetOrAdd(
                propertyName, SimplePropertyAccessor<T>.CreateGetDelegate);
            return getDelegate.Invoke(instance, Array.Empty<object?>());
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
        public static void SetValue([DisallowNull] T instance, string propertyName, object? value)
        {
            var setDelegate = SimplePropertyAccessor<T>.SetDelegates.GetOrAdd(
                propertyName, SimplePropertyAccessor<T>.CreateSetDelegate);
            _ = setDelegate.Invoke(instance, new object?[] { value });
        }

        /// <summary>
        /// 创建当前类型的指定公共实例属性的 <see langword="get"/> 访问器的动态调用委托。
        /// </summary>
        /// <param name="propertyName">要创建委托的属性的名称。</param>
        /// <returns><typeparamref name="T"/> 类型中名为 <paramref name="propertyName"/>
        /// 的属性的 <see langword="get"/> 访问器的动态调用委托。</returns>
        /// <exception cref="MissingMemberException">
        /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="get"/> 属性。</exception>
        private static DynamicMethodDelegate CreateGetDelegate(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName, Type.EmptyTypes);
            if (property?.GetMethod is null) { throw new MissingMemberException(); }
            if (property.GetMethod.IsStatic) { throw new MissingMemberException(); }
            if (!property.GetMethod.IsPublic) { throw new MissingMemberException(); }
            return property.GetMethod.CreateDynamicDelegate();
        }

        /// <summary>
        /// 创建当前类型的指定公共实例属性的 <see langword="set"/> 访问器的动态调用委托。
        /// </summary>
        /// <param name="propertyName">要创建委托的属性的名称。</param>
        /// <returns><typeparamref name="T"/> 类型中名为 <paramref name="propertyName"/>
        /// 的属性的 <see langword="set"/> 访问器的动态调用委托。</returns>
        /// <exception cref="MissingMemberException">
        /// 无法访问名为 <paramref name="propertyName"/> 的 <see langword="set"/> 属性。</exception>
        private static DynamicMethodDelegate CreateSetDelegate(string propertyName)
        {
            var property = typeof(T).GetProperty(propertyName, Type.EmptyTypes);
            if (property?.SetMethod is null) { throw new MissingMemberException(); }
            if (property.SetMethod.IsStatic) { throw new MissingMemberException(); }
            if (!property.SetMethod.IsPublic) { throw new MissingMemberException(); }
            return property.SetMethod.CreateDynamicDelegate();
        }
    }
}
