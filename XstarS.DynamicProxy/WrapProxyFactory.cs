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
        /// 表示提供代理类型的 <see cref="WrapProxyTypeProvider"/> 对象。
        /// </summary>
        private readonly WrapProxyTypeProvider TypeProvider;

        /// <summary>
        /// 初始化 <see cref="WrapProxyFactory{T}"/> 类的新实例，并将其代理委托设定为指定的委托。
        /// </summary>
        /// <param name="handler">方法调用所用的代理委托。</param>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口。</exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        public WrapProxyFactory(MethodInvokeHandler handler)
        {
            this.TypeProvider = WrapProxyTypeProvider.OfType(typeof(T));
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
        /// <returns>原型类型的 <see cref="Type"/> 对象。</returns>
        public MethodInvokeHandler Handler { get; }

        /// <summary>
        /// 使用指定的代理对象创建代理类型的实例。
        /// </summary>
        /// <param name="instance">要为其提供代理的对象。</param>
        /// <returns>一个为指定对象提供代理的代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
        public T CreateInstance(T instance) =>
            (T)this.TypeProvider.CreateProxyInstance(instance, this.Handler);
    }
}
