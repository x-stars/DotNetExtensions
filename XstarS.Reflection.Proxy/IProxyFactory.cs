using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供代理派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public interface IProxyFactory<out T> where T : class
    {
        /// <summary>
        /// 代理派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不为非抽象非密封类。</exception>
        Type ProxyType { get; }

        /// <summary>
        /// 使用默认构造函数来创建代理派生类型的实例。
        /// </summary>
        /// <returns>一个不使用参数创建的代理派生类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="IProxyFactory{T}.ProxyType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="IProxyFactory{T}.ProxyType"/> 的无参构造函数访问级别过低。</exception>
        T CreateInstance();

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建代理派生类型的实例。
        /// </summary>
        /// <returns>一个使用指定参数创建的代理派生类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="IProxyFactory{T}.ProxyType"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="IProxyFactory{T}.ProxyType"/>
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        T CreateInstance(params object[] arguments);
    }
}
