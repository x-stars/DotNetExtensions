using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从原型类型构造用于代理派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">代理类型的原型类型，应为接口或非密封类。</typeparam>
    public class ProxyBuilder<T> : ProxyBuilderBase<T> where T : class
    {
        /// <summary>
        /// <see cref="ProxyBuilder{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ProxyBuilder<T>> LazyDefault =
            new Lazy<ProxyBuilder<T>>(() => new ProxyBuilder<T>());

        /// <summary>
        /// 初始化 <see cref="ProxyBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal ProxyBuilder()
        {
            var type = typeof(T);
            this.PrototypeType = type;
            this.InternalBuilder = ProxyBuilder.Default(type);
        }

        /// <summary>
        /// 代理类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 用于构造代理类型的 <see cref="ProxyBuilder"/> 对象。
        /// </summary>
        internal ProxyBuilder InternalBuilder { get; }

        /// <summary>
        /// 返回一个默认的 <see cref="ProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyBuilder<T> Default => ProxyBuilder<T>.LazyDefault.Value;

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的派生类型。</returns>
        protected override Type BuildProxyType() => this.InternalBuilder.ProxyType;
    }
}
