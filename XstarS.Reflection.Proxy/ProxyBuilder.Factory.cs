using System;

namespace XstarS.Reflection
{
    public partial class ProxyBuilder<T>
    {
        /// <summary>
        /// <see cref="ProxyBuilder{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ProxyBuilder<T>> LazyDefault =
            new Lazy<ProxyBuilder<T>>(ProxyBuilder<T>.Create);

        /// <summary>
        /// 返回一个默认的 <see cref="ProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyBuilder<T> Default => ProxyBuilder<T>.LazyDefault.Value;

        /// <summary>
        /// 以 <typeparamref name="T"/> 为原型类型创建一个 <see cref="ProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private static ProxyBuilder<T> Create() => new ProxyBuilder<T>();
    }
}
