using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供基于代理委托 <see cref="MethodInvokeHandler"/> 的代理类型，并提供创建此代理类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class ProxyFactory<T> where T : class
    {
        /// <summary>
        /// 表示 <see cref="ProxyFactory{T}.Default"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<ProxyFactory<T>> LazyDefault =
            new Lazy<ProxyFactory<T>>(() => new ProxyFactory<T>());

        /// <summary>
        /// 表示 <see cref="ProxyFactory{T}.Handler"/> 的默认值。
        /// </summary>
        private static readonly MethodInvokeHandler DefaultHandler =
            (instance, method, arguments, @delegate) => @delegate.Invoke(instance, arguments);

        /// <summary>
        /// 表示提供代理类型的 <see cref="ProxyTypeProvider"/> 对象。
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
            this.Handler = ProxyFactory<T>.DefaultHandler;
        }

        /// <summary>
        /// 初始化 <see cref="ProxyFactory{T}"/> 类的新实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        private ProxyFactory(MethodInvokeHandler handler)
        {
            this.TypeProvider = ProxyTypeProvider.OfType(typeof(T));
            this.Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// 获取默认的 <see cref="ProxyFactory{T}"/> 类的实例。
        /// </summary>
        /// <returns>默认的 <see cref="ProxyFactory{T}"/> 类的实例。</returns>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyFactory<T> Default => ProxyFactory<T>.LazyDefault.Value;

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
        /// 以指定的方法调用所用的代理创建一个 <see cref="ProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <param name="handler">方法调用所用的代理。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        public static ProxyFactory<T> WithHandler(MethodInvokeHandler handler) =>
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
        /// 初始化代理类型的实例的代理委托字段。
        /// </summary>
        /// <param name="proxy">要初始化代理委托字段的代理类型的实例。</param>
        /// <returns>完成代理委托字段初始化的 <paramref name="proxy"/>。</returns>
        private T InitializeHandler(T proxy)
        {
            var handlerField = this.TypeProvider.HandlerField;
            handlerField.SetValue(proxy, this.Handler);
            return proxy;
        }
    }
}
