using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供代理派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class ProxyTypeProvider<T> : ProxyTypeProviderBase<T> where T : class
    {
        /// <summary>
        /// <see cref="ProxyTypeProvider{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ProxyTypeProvider<T>> LazyDefault =
            new Lazy<ProxyTypeProvider<T>>(() => new ProxyTypeProvider<T>());

        /// <summary>
        /// 初始化 <see cref="ProxyTypeProvider{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private ProxyTypeProvider()
        {
            var type = typeof(T);
            this.PrototypeType = type;
            this.InternalProvider = ProxyTypeProvider.Default(type);
        }

        /// <summary>
        /// 原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 提供代理派生类型的 <see cref="ProxyTypeProvider"/> 对象。
        /// </summary>
        private ProxyTypeProvider InternalProvider { get; }

        /// <summary>
        /// 返回一个默认的 <see cref="ProxyTypeProvider{T}"/> 类的实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyTypeProvider<T> Default => ProxyTypeProvider<T>.LazyDefault.Value;

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的代理派生类型。</returns>
        protected override Type BuildProxyType() => this.InternalProvider.ProxyType;
    }
}
