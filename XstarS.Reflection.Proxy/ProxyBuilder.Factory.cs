using System;
using System.Collections.Concurrent;

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
        /// <see cref="ProxyBuilder{T}"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<ProxyBuilder<T>>>
            OfTypes = new ConcurrentDictionary<Type, Lazy<ProxyBuilder<T>>>();

        /// <summary>
        /// 返回一个默认的 <see cref="ProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyBuilder<T> Default => ProxyBuilder<T>.LazyDefault.Value;

        /// <summary>
        /// 获取以指定原型类型为基础的 <see cref="ProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <param name="type">代理类型的原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="type"/> 的
        /// <see cref="ProxyBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyBuilder<T> OfType(Type type) =>
            ProxyBuilder<T>.OfTypes.GetOrAdd(type,
                newType => new Lazy<ProxyBuilder<T>>(
                    () => ProxyBuilder<T>.Create(newType))).Value;

        /// <summary>
        /// 以 <typeparamref name="T"/> 为原型类型创建一个 <see cref="ProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private static ProxyBuilder<T> Create() => new ProxyBuilder<T>();

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="ProxyBuilder{T}"/> 类的实例。
        /// </summary>
        /// <param name="type">代理类型的原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="type"/> 的
        /// <see cref="ProxyBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private static ProxyBuilder<T> Create(Type type) => new ProxyBuilder<T>(type);
    }
}
