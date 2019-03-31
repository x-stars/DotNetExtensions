using System;
using System.Collections.Concurrent;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从指定原型类型构造代理派生类型及其实例的工厂方法和基类实现。
    /// </summary>
    public partial class ObjectProxyBuilder
    {
        /// <summary>
        /// <see cref="ObjectProxyBuilder"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<ObjectProxyBuilder>>
            OfTypes = new ConcurrentDictionary<Type, Lazy<ObjectProxyBuilder>>();

        /// <summary>
        /// 获取以指定原型类型为基础的 <see cref="ObjectProxyBuilder"/> 类的实例。
        /// </summary>
        /// <param name="type">代理类型的原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="type"/> 的
        /// <see cref="ObjectProxyBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static ObjectProxyBuilder OfType(Type type) =>
            ObjectProxyBuilder.OfTypes.GetOrAdd(type,
                newType => new Lazy<ObjectProxyBuilder>(
                    () => ObjectProxyBuilder.Create(newType))).Value;

        /// <summary>
        /// 以指定类型为原型类型创建一个 <see cref="ObjectProxyBuilder"/> 类的实例。
        /// </summary>
        /// <param name="type">代理类型的原型类型的 <see cref="Type"/> 对象。</param>
        /// <returns>一个原型类型为 <paramref name="type"/> 的
        /// <see cref="ObjectProxyBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        private static ObjectProxyBuilder Create(Type type) => new ObjectProxyBuilder(type);
    }
}
