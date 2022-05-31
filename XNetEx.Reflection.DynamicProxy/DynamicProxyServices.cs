using System;

namespace XNetEx.Reflection
{
    /// <summary>
    /// 提供动态代理类型相关的服务方法。
    /// </summary>
    public static class DynamicProxyServices
    {
        /// <summary>
        /// 表示默认的 <see cref="MethodInvokeHandler"/> 代理委托。
        /// 此代理委托仅调用被代理方法并返回。
        /// </summary>
        public static readonly MethodInvokeHandler DefaultHandler =
            (instance, method, @delegate, arguments) => @delegate.Invoke(instance, arguments);

        /// <summary>
        /// 使用默认构造函数创建指定类型的直接代理类型的实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <returns>一个不使用参数创建并使用指定代理委托的代理类型的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="T"/> 类型不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <typeparamref name="T"/> 类型的无参构造函数访问级别过低。</exception>
        public static T CreateDirectProxy<T>(MethodInvokeHandler? handler = null)
            where T : class
        {
            return (T)DynamicProxyServices.CreateDirectProxy(typeof(T), handler);
        }

        /// <summary>
        /// 使用默认构造函数创建指定类型的直接代理类型的实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="baseType">原型类型，应为接口或非密封类。</param>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <returns>一个不使用参数创建并使用指定代理委托的代理类型的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="MissingMethodException">
        /// <paramref name="baseType"/> 类型不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="baseType"/> 类型的无参构造函数访问级别过低。</exception>
        public static object CreateDirectProxy(Type baseType, MethodInvokeHandler? handler = null)
        {
            if (baseType is null) { throw new ArgumentNullException(nameof(baseType)); }
            handler ??= DynamicProxyServices.DefaultHandler;
            var provider = DirectProxyTypeProvider.OfType(baseType);
            return provider.CreateProxyInstance(Array.Empty<object>(), handler);
        }

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建指定类型的直接代理类型的实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
        /// <param name="arguments">用于创建代理对象的参数数组。</param>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <returns>一个不使用参数创建并使用指定代理委托的代理类型的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="MissingMethodException"><typeparamref name="T"/>
        /// 类型不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException"><typeparamref name="T"/>
        /// 类型中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public static T CreateDirectProxy<T>(object?[]? arguments, MethodInvokeHandler? handler = null)
            where T : class
        {
            return (T)DynamicProxyServices.CreateDirectProxy(typeof(T), arguments, handler);
        }

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建指定类型的直接代理类型的实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="baseType">原型类型，应为接口或非密封类。</param>
        /// <param name="arguments">用于创建代理对象的参数数组。</param>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <returns>一个不使用参数创建并使用指定代理委托的代理类型的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseType"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="MissingMethodException"><paramref name="baseType"/>
        /// 类型不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException"><paramref name="baseType"/>
        /// 类型中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public static object CreateDirectProxy(
            Type baseType, object?[]? arguments, MethodInvokeHandler? handler = null)
        {
            if (baseType is null) { throw new ArgumentNullException(nameof(baseType)); }
            handler ??= DynamicProxyServices.DefaultHandler;
            var provider = DirectProxyTypeProvider.OfType(baseType);
            return provider.CreateProxyInstance(arguments, handler);
        }

        /// <summary>
        /// 使用指定的代理对象创建代理类型的实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <typeparam name="T">原型类型，应为接口。</typeparam>
        /// <param name="instance">要为其提供代理的对象。</param>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <returns>一个为指定对象提供以指定委托定义的代理的代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
        public static T CreateWrapProxy<T>(T instance, MethodInvokeHandler? handler = null)
            where T : class
        {
            return (T)DynamicProxyServices.CreateWrapProxy(typeof(T), instance, handler);
        }

        /// <summary>
        /// 使用指定的代理对象创建代理类型的实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="baseType">原型类型，应为接口。</param>
        /// <param name="instance">要为其提供代理的对象。</param>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <returns>一个为指定对象提供以指定委托定义的代理的代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="baseType"/>
        /// 或 <paramref name="instance"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException"><paramref name="instance"/>
        /// 无法转换为 <see cref="WrapProxyTypeProvider.BaseType"/> 类型。</exception>
        public static object CreateWrapProxy(
            Type baseType, object instance, MethodInvokeHandler? handler = null)
        {
            if (baseType is null) { throw new ArgumentNullException(nameof(baseType)); }
            handler ??= DynamicProxyServices.DefaultHandler;
            var provider = WrapProxyTypeProvider.OfType(baseType);
            return provider.CreateProxyInstance(instance, handler);
        }
    }
}
