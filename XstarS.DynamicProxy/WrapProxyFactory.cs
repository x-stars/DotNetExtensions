using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供基于代理委托 <see cref="MethodInvokeHandler"/> 的包装代理类型，并提供创建此代理类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口。</typeparam>
    public sealed class WrapProxyFactory<T> where T : class
    {
        /// <summary>
        /// 表示 <see cref="WrapProxyFactory{T}.Default"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<WrapProxyFactory<T>> LazyDefault =
            new Lazy<WrapProxyFactory<T>>(() => new WrapProxyFactory<T>());

        /// <summary>
        /// 表示 <see cref="WrapProxyFactory{T}.Handler"/> 的默认值。
        /// </summary>
        private static readonly MethodInvokeHandler DefaultHandler =
            (instance, method, @delegate, arguments) => @delegate.Invoke(instance, arguments);

        /// <summary>
        /// 表示提供代理类型的 <see cref="WrapProxyTypeProvider"/> 对象。
        /// </summary>
        private readonly WrapProxyTypeProvider TypeProvider;

        /// <summary>
        /// 初始化 <see cref="WrapProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口。</exception>
        private WrapProxyFactory()
        {
            this.TypeProvider = WrapProxyTypeProvider.OfType(typeof(T));
            this.Handler = WrapProxyFactory<T>.DefaultHandler;
        }

        /// <summary>
        /// 初始化 <see cref="WrapProxyFactory{T}"/> 类的新实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        private WrapProxyFactory(MethodInvokeHandler handler)
        {
            this.TypeProvider = WrapProxyTypeProvider.OfType(typeof(T));
            this.Handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        /// <summary>
        /// 获取默认的 <see cref="WrapProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static WrapProxyFactory<T> Default => WrapProxyFactory<T>.LazyDefault.Value;

        /// <summary>
        /// 获取原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>原型类型的 <see cref="Type"/> 对象。</returns>
        public Type BaseType => this.TypeProvider.BaseType;

        /// <summary>
        /// 获取代理类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <returns>原型类型的 <see cref="Type"/> 对象。</returns>
        public Type ProxyType => this.TypeProvider.ProxyType;

        /// <summary>
        /// 获取方法调用时所用的代理委托。
        /// </summary>
        /// <returns>原型类型的 <see cref="Type"/> 对象。</returns>
        public MethodInvokeHandler Handler { get; }

        /// <summary>
        /// 以指定的方法调用所用的代理创建一个 <see cref="WrapProxyFactory{T}"/> 类的新实例。
        /// </summary>
        /// <param name="handler">方法调用所用的代理。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        public static WrapProxyFactory<T> WithHandler(MethodInvokeHandler handler) =>
            new WrapProxyFactory<T>(handler);

        /// <summary>
        /// 使用指定的代理对象创建代理类型的实例。
        /// </summary>
        /// <param name="instance">要为其提供代理的对象。</param>
        /// <returns>一个为指定对象提供代理的代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
        public T CreateInstance(T instance) =>
            this.InitializeHandler(this.InitializeInstance(
                (T)Activator.CreateInstance(this.ProxyType)!, instance));

        /// <summary>
        /// 初始化代理类型的实例的代理对象字段。
        /// </summary>
        /// <param name="proxy">要初始化代理对象字段的代理类型的实例。</param>
        /// <param name="instance">要为其提供代理的对象。</param>
        /// <returns>完成代理对象字段初始化的 <paramref name="proxy"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
        private T InitializeInstance(T proxy, T instance)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var instanceField = this.TypeProvider.InstanceField;
            instanceField.SetValue(proxy, instance);
            return proxy;
        }

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
