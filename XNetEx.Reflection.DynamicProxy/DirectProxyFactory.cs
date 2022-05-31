using System;

namespace XNetEx.Reflection
{
    /// <summary>
    /// 提供基于代理委托 <see cref="MethodInvokeHandler"/> 的直接代理类型，并提供创建此代理类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class DirectProxyFactory<T> where T : class
    {
        /// <summary>
        /// 表示提供代理类型的 <see cref="DirectProxyTypeProvider"/> 对象。
        /// </summary>
        private readonly DirectProxyTypeProvider TypeProvider;

        /// <summary>
        /// 初始化 <see cref="DirectProxyFactory{T}"/> 类的新实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        public DirectProxyFactory(MethodInvokeHandler handler)
        {
            this.TypeProvider = DirectProxyTypeProvider.OfType(typeof(T));
            this.Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// 获取原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>原型类型的 <see cref="Type"/> 对象。</returns>
        public Type BaseType => this.TypeProvider.BaseType;

        /// <summary>
        /// 获取代理类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>代理类型的 <see cref="Type"/> 对象。</returns>
        public Type ProxyType => this.TypeProvider.ProxyType;

        /// <summary>
        /// 获取方法调用时所用的代理委托。
        /// </summary>
        /// <returns>方法调用时所用的代理委托。</returns>
        public MethodInvokeHandler Handler { get; }

        /// <summary>
        /// 使用默认构造函数创建代理类型的实例。
        /// </summary>
        /// <returns>一个不使用参数创建的代理类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="T"/> 类型不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <typeparamref name="T"/> 类型的无参构造函数访问级别过低。</exception>
        public T CreateInstance() => this.CreateInstance(Array.Empty<object>());

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建代理类型的实例。
        /// </summary>
        /// <param name="arguments">用于创建代理对象的参数数组。</param>
        /// <returns>一个使用指定参数创建的代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException"><typeparamref name="T"/>
        /// 类型不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException"><typeparamref name="T"/>
        /// 类型中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public T CreateInstance(params object?[]? arguments) =>
            (T)this.TypeProvider.CreateProxyInstance(arguments, this.Handler);
    }
}
