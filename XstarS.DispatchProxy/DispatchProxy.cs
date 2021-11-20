using System;
using System.ComponentModel;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 为代理类型 <see cref="DispatchProxy"/> 提供基于代理委托的实现。
    /// </summary>
    /// <typeparam name="TInterface">要代理的类型，应为接口。</typeparam>
    public class DispatchProxy<TInterface> : DispatchProxy where TInterface : class
    {
        /// <summary>
        /// 表示当前 <see cref="DispatchProxy{TInterface}"/> 的代理对象。
        /// </summary>
        protected TInterface Instance;

        /// <summary>
        /// 表示当前 <see cref="DispatchProxy{TInterface}"/> 的代理委托。
        /// </summary>
        protected InvocationHandler Handler;

        /// <summary>
        /// 初始化 <see cref="DispatchProxy{TInterface}"/> 类的新实例。
        /// 用于内部实现，应使用 <see langword="protected"/> 构造函数实现继承。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This constructor is for internal usages only.", error: true)]
        public DispatchProxy() : base() { }

        /// <summary>
        /// 初始化 <see cref="DispatchProxy{TInterface}"/> 类的新实例。
        /// 应使用此构造函数实现继承，而非 <see langword="public"/> 构造函数。
        /// </summary>
        /// <param name="unused">不使用此参数，应为 <see langword="null"/>。</param>
        protected DispatchProxy(object unused) : base() { }

        /// <summary>
        /// 调用代理类型的方法时实际调用的方法，执行传入的 <see cref="Handler"/> 委托。
        /// </summary>
        /// <param name="method">当前调用的方法。</param>
        /// <param name="arguments">方法的参数列表。</param>
        /// <returns>方法的返回值。若无返回值，应为 <see langword="null"/>。</returns>
        protected override object Invoke(MethodInfo method, object[] arguments)
        {
            return (method is null) ? null :
                this.Handler!.Invoke(this.Instance!, method, arguments);
        }

        /// <summary>
        /// 不支持此方法，总是抛出 <see cref="NotSupportedException"/> 异常。
        /// </summary>
        /// <typeparam name="T">代理应实现的接口。</typeparam>
        /// <typeparam name="TProxy">要用于代理类的基类。</typeparam>
        /// <returns>总是抛出 <see cref="NotSupportedException"/> 异常。</returns>
        /// <exception cref="NotSupportedException">总是抛出此异常。</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Not supported, use non-generic method instead.", error: true)]
        public static new T Create<T, TProxy>() where TProxy : DispatchProxy =>
            throw new NotSupportedException();

        /// <summary>
        /// 使用指定的代理对象和默认的代理委托创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。
        /// </summary>
        /// <param name="instance">要代理的 <typeparamref name="TInterface"/> 类型的对象。</param>
        /// <returns>以指定的参数初始化的 <see cref="DispatchProxy{TInterface}"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 为 <see langword="null"/>。</exception>
        public static TInterface Create(TInterface instance)
        {
            return DispatchProxy<TInterface>.Create(instance, DispatchProxyServices.DefaultHandler);
        }

        /// <summary>
        /// 使用指定的代理对象和代理委托创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。
        /// </summary>
        /// <param name="instance">要代理的 <typeparamref name="TInterface"/> 类型的对象。</param>
        /// <param name="handler">方法调用时所用的 <see cref="Handler"/> 代理委托。</param>
        /// <returns>以指定的参数初始化的 <see cref="DispatchProxy{TInterface}"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/>
        /// 或 <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        public static TInterface Create(TInterface instance, InvocationHandler handler)
        {
            if (instance is null) { throw new ArgumentNullException(nameof(instance)); }
            if (handler is null) { throw new ArgumentNullException(nameof(handler)); }
            var proxy = DispatchProxy.Create<TInterface, DispatchProxy<TInterface>>();
            ((DispatchProxy<TInterface>)(object)proxy).Instance = instance;
            ((DispatchProxy<TInterface>)(object)proxy).Handler = handler;
            return proxy;
        }
    }
}
