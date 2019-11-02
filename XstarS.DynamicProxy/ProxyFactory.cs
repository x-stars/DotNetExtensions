using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供基于代理委托 <see cref="ProxyInvokeHandler"/> 的代理类型，并提供创建此代理类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class ProxyFactory<T> where T : class
    {
        /// <summary>
        /// <see cref="ProxyFactory{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ProxyFactory<T>> LazyDefault =
            new Lazy<ProxyFactory<T>>(() => new ProxyFactory<T>());

        /// <summary>
        /// 提供代理类型的 <see cref="ProxyTypeProvider"/> 对象。
        /// </summary>
        private readonly ProxyTypeProvider TypeProvider;

        /// <summary>
        /// 初始化 <see cref="ProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private ProxyFactory()
        {
            this.TypeProvider = ProxyTypeProvider.OfType(typeof(T));
        }

        /// <summary>
        /// 以指定的代理委托初始化 <see cref="ProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <param name="handler">方法调用所用的代理。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        private ProxyFactory(ProxyInvokeHandler handler)
        {
            if (handler is null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            this.TypeProvider = ProxyTypeProvider.OfType(typeof(T));
            this.Handler = handler;
        }

        /// <summary>
        /// 返回一个默认的 <see cref="ProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyFactory<T> Default => ProxyFactory<T>.LazyDefault.Value;

        /// <summary>
        /// 原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType => this.TypeProvider.BaseType;

        /// <summary>
        /// 代理类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type ProxyType => this.TypeProvider.ProxyType;

        /// <summary>
        /// 方法调用时所用的代理委托。
        /// </summary>
        public ProxyInvokeHandler Handler { get; }

        /// <summary>
        /// 以指定的方法调用所用的代理创建一个 <see cref="ProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <param name="handler">方法调用所用的代理。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        public static ProxyFactory<T> WithHandler(ProxyInvokeHandler handler) =>
            new ProxyFactory<T>(handler);

        /// <summary>
        /// 使用默认构造函数创建代理类型的实例。
        /// </summary>
        /// <returns>一个不使用参数创建的代理类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="ProxyFactory{T}.ProxyType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ProxyFactory{T}.ProxyType"/> 的无参构造函数访问级别过低。</exception>
        public T CreateInstance() =>
            this.InitializeHandler((T)Activator.CreateInstance(this.ProxyType));

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建代理类型的实例。
        /// </summary>
        /// <returns>一个使用指定参数创建的代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="ProxyFactory{T}.ProxyType"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ProxyFactory{T}.ProxyType"/>
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public T CreateInstance(params object[] arguments) =>
            this.InitializeHandler((T)Activator.CreateInstance(this.ProxyType, arguments));

        /// <summary>
        /// 初始化代理类型的实例的 <see cref="ProxyInvokeHandler"/> 字段。
        /// </summary>
        /// <param name="proxyInstance">要初始化代理字段的代理类型的实例。</param>
        /// <returns>完成代理字段初始化的 <paramref name="proxyInstance"/>。</returns>
        private T InitializeHandler(T proxyInstance)
        {
            var handlerField = this.TypeProvider.HandlerField;
            handlerField.SetValue(proxyInstance, this.Handler);
            return proxyInstance;
        }
    }
}
