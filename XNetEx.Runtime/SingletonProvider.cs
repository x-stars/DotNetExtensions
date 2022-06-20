using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XNetEx;

/// <summary>
/// 为引用类型提供简单的获取单例的方法。
/// </summary>
public static class SingletonProvider
{
    /// <summary>
    /// 提供指定类型的单例存储以及初始化方法。
    /// </summary>
    /// <typeparam name="T">单例的类型。</typeparam>
    private static class Storage<T> where T : class
    {
        /// <summary>
        /// 表示当前引用类型的单例存储。
        /// </summary>
        internal static volatile T? Value;

        /// <summary>
        /// 初始化当前类型的单例，并返回单例的值。
        /// </summary>
        /// <returns><typeparamref name="T"/> 类型的单例。</returns>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="T"/> 不包含无参构造函数；或
        /// <typeparamref name="T"/> 为抽象类型，但不包含无参数且返回值为
        /// <typeparamref name="T"/> 类型的 <c>Create</c> 方法。</exception>
        /// <exception cref="InvalidCastException">
        /// <typeparamref name="T"/> 的 <c>Create</c>
        /// 方法返回 <see langword="null"/>。</exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        internal static T InitializeValue()
        {
            return SingletonProvider.Storage<T>.Value ??=
                SingletonProvider.CreateInstance<T>();
        }
    }

    /// <summary>
    /// 获取指定引用类型的单例。
    /// </summary>
    /// <typeparam name="T">单例的类型。</typeparam>
    /// <returns><typeparamref name="T"/> 类型的单例。</returns>
    /// <exception cref="MissingMethodException">
    /// <typeparamref name="T"/> 不包含无参构造函数；或
    /// <typeparamref name="T"/> 为抽象类型，但不包含无参数且返回值为
    /// <typeparamref name="T"/> 类型的 <c>Create</c> 方法。</exception>
    /// <exception cref="InvalidCastException">
    /// <typeparamref name="T"/> 的 <c>Create</c>
    /// 方法返回 <see langword="null"/>。</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T GetInstance<T>() where T : class
    {
        return SingletonProvider.Storage<T>.Value ??
            SingletonProvider.Storage<T>.InitializeValue();
    }

    /// <summary>
    /// 创建指定引用类型的实例。
    /// </summary>
    /// <typeparam name="T">要创建实例的类型。</typeparam>
    /// <returns><typeparamref name="T"/> 类型的实例。</returns>
    /// <exception cref="MissingMethodException">
    /// <typeparamref name="T"/> 不包含无参构造函数；或
    /// <typeparamref name="T"/> 为抽象类型，但不包含无参数且返回值为
    /// <typeparamref name="T"/> 类型的 <c>Create</c> 方法。</exception>
    /// <exception cref="InvalidCastException">
    /// <typeparamref name="T"/> 的 <c>Create</c>
    /// 方法返回 <see langword="null"/>。</exception>
    private static T CreateInstance<T>() where T : class
    {
        static T? CreateByCreateMethod()
        {
            var createMethod = typeof(T).GetMethod("Create",
                BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                default(Binder), Type.EmptyTypes, Array.Empty<ParameterModifier>());
            if ((createMethod is null) || (createMethod.ReturnType != typeof(T)))
            {
                throw new MissingMethodException(typeof(T).ToString(), "Create");
            }
            return (T?)createMethod.Invoke(null, Array.Empty<object>());
        }

        var instance = typeof(T).IsAbstract ? CreateByCreateMethod() :
            (T?)Activator.CreateInstance(typeof(T), nonPublic: true);
        return instance ?? throw new InvalidCastException();
    }
}
