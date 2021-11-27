using System;
using System.ComponentModel;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 为代理类型 <see cref="DispatchProxy"/> 提供基于代理委托的实现。
    /// </summary>
    /// <typeparam name="TInterface">代理应实现的接口类型。</typeparam>
    public class DispatchProxy<TInterface> : DispatchProxy where TInterface : class
    {
        /// <summary>
        /// 表示当前 <see cref="DispatchProxy{TInterface}"/> 的代理对象。
        /// </summary>
        protected TInterface? Instance;

        /// <summary>
        /// 表示当前 <see cref="DispatchProxy{TInterface}"/> 的代理委托。
        /// </summary>
        protected InvocationHandler? Handler;

        /// <summary>
        /// 初始化 <see cref="DispatchProxy{TInterface}"/> 类的新实例。用于内部实现，不应直接使用。
        /// </summary>
        /// <remarks>
        /// <para>使用 <see cref="DispatchProxy{TInterface}(object)"/> 构造函数实现继承。</para>
        /// <para>使用 <see cref="DispatchProxy{TInterface}.Create(TInterface, InvocationHandler)"/>
        /// 方法创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。</para>
        /// </remarks>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("For internal usages only, use Create method insted.", error: true)]
        public DispatchProxy() : base() { }

        /// <summary>
        /// 初始化 <see cref="DispatchProxy{TInterface}"/> 类的新实例。应使用此构造函数实现继承。
        /// </summary>
        /// <param name="unused">不使用此参数，应为 <see langword="null"/>。</param>
        protected DispatchProxy(object? unused) : base() { }

        /// <summary>
        /// 每当对生成的代理类型调用任何方法时，都会调用此方法来调度控件。
        /// 执行传入的 <see cref="DispatchProxy{TInterface}.Handler"/> 委托。
        /// </summary>
        /// <param name="method">调用方调用的方法。</param>
        /// <param name="arguments">调用方传递给方法的参数。</param>
        /// <returns>要返回给调用方的对象。或者对于
        /// <see langword="void"/> 方法，应为 <see langword="null"/>。</returns>
        protected override object? Invoke(MethodInfo? method, object?[]? arguments)
        {
            arguments ??= Array.Empty<object>();
            return (method is null) ? null :
                this.Handler!.Invoke(this.Instance!, method, arguments);
        }

        /// <summary>
        /// 不支持此方法，总是抛出 <see cref="NotSupportedException"/> 异常。
        /// </summary>
        /// <remarks>
        /// 使用 <see cref="DispatchProxy{TInterface}.Create(TInterface, InvocationHandler)"/>
        /// 方法创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。
        /// </remarks>
        /// <typeparam name="T">代理应实现的接口。</typeparam>
        /// <typeparam name="TProxy">要用于代理类的基类。</typeparam>
        /// <returns>总是抛出 <see cref="NotSupportedException"/> 异常。</returns>
        /// <exception cref="NotSupportedException">总是抛出此异常。</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Not supported, use non-generic Create method instead.", error: true)]
        public static new T Create<T, TProxy>() where TProxy : DispatchProxy
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// 使用指定的代理对象和代理委托创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。
        /// </summary>
        /// <param name="instance">要代理的 <typeparamref name="TInterface"/> 类型的对象。</param>
        /// <param name="handler">方法调用时所用的 <see cref="InvocationHandler"/> 代理委托。</param>
        /// <returns>实现 <typeparamref name="TInterface"/> 接口的代理类型的对象。</returns>
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
