using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从原型类型构造代理派生类型及其实例的基类实现。
    /// </summary>
    /// <typeparam name="T">代理类型的原型类型，应为非抽象非密封类。</typeparam>
    public abstract class ProxyBuilderBase<T> : IProxyBuilder<T> where T : class
    {
        /// <summary>
        /// 代理派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        private readonly Lazy<Type> LazyProxyType;

        /// <summary>
        /// 初始化 <see cref="ProxyBuilder{T}"/> 类的新实例。
        /// </summary>
        protected ProxyBuilderBase()
        {
            this.LazyProxyType = new Lazy<Type>(this.BuildProxyType);
        }

        /// <summary>
        /// 代理派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type ProxyType => this.LazyProxyType.Value;

        /// <summary>
        /// 使用默认构造函数来创建代理类型的实例。
        /// </summary>
        /// <returns>一个代理类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 的无参构造函数访问级别过低。</exception>
        public T CreateInstance() =>
            (T)Activator.CreateInstance(this.ProxyType);

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建代理类型的实例。
        /// </summary>
        /// <returns>一个代理类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException"><see cref="ProxyBuilderBase{T}.ProxyType"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException"><see cref="ProxyBuilderBase{T}.ProxyType"/>
        /// 与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public T CreateInstance(params object[] arguments) =>
            (T)Activator.CreateInstance(this.ProxyType, arguments);

        /// <summary>
        /// 在派生类中重写时，构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的代理派生类型。</returns>
        protected abstract Type BuildProxyType();
    }
}
