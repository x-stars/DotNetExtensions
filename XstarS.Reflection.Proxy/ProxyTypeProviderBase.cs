using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 为代理派生类型提供对象 <see cref="IProxyTypeProvider{T}"/> 提供抽象基类实现。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public abstract class ProxyTypeProviderBase<T> : IProxyTypeProvider<T> where T : class
    {
        /// <summary>
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<Type> LazyProxyType;

        /// <summary>
        /// 初始化 <see cref="ProxyTypeProviderBase{T}"/> 类的新实例。
        /// </summary>
        protected ProxyTypeProviderBase()
        {
            this.LazyProxyType = new Lazy<Type>(this.CreateProxyType);
        }

        /// <summary>
        /// 代理派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type ProxyType => this.LazyProxyType.Value;

        /// <summary>
        /// 使用默认构造函数创建代理派生类型的实例。
        /// </summary>
        /// <returns>一个不使用参数创建的代理派生类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/> 的无参构造函数访问级别过低。</exception>
        public T CreateInstance() =>
            (T)Activator.CreateInstance(this.ProxyType);

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建代理派生类型的实例。
        /// </summary>
        /// <returns>一个使用指定参数创建的代理派生类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ProxyTypeProviderBase{T}.ProxyType"/>
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public T CreateInstance(params object[] arguments) =>
            (T)Activator.CreateInstance(this.ProxyType, arguments);

        /// <summary>
        /// 返回表示代理派生类型的字符串。
        /// </summary>
        /// <returns><see cref="ProxyTypeProviderBase{T}.ProxyType"/> 的字符串表达形式。</returns>
        public override string ToString() => this.ProxyType.ToString();

        /// <summary>
        /// 在派生类中重写时，创建代理派生类型。
        /// </summary>
        /// <returns>创建的代理派生类型。</returns>
        protected abstract Type CreateProxyType();
    }
}
