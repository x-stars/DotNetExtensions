using System;
using System.Collections.Concurrent;

namespace XstarS.Reflection
{
    public partial class ProxyBuilder
    {
        /// <summary>
        /// <see cref="ProxyBuilder"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<ProxyBuilder>>
            OfTypes = new ConcurrentDictionary<Type, Lazy<ProxyBuilder>>();

        /// <summary>
        /// 获取以指定原型类型为基础的 <see cref="ProxyBuilder"/> 类的实例。
        /// </summary>
        /// <param name="type">代理类型的原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="type"/> 的
        /// <see cref="ProxyBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ProxyBuilder OfType(Type type) =>
            ProxyBuilder.OfTypes.GetOrAdd(type,
                newType => new Lazy<ProxyBuilder>(
                    () => ProxyBuilder.Create(newType))).Value;

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="ProxyBuilder"/> 类的实例。
        /// </summary>
        /// <param name="type">代理类型的原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="type"/> 的
        /// <see cref="ProxyBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        private static ProxyBuilder Create(Type type) => new ProxyBuilder(type);
    }
}
