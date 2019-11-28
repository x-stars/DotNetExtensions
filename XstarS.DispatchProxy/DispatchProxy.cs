using System;
using System.ComponentModel;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法调用时所用的代理委托。
    /// </summary>
    /// <param name="instance">方法的实例参数。</param>
    /// <param name="method">当前调用的方法。</param>
    /// <param name="arguments">方法的参数列表。</param>
    /// <returns>方法的返回值。若无返回值，应为 <see langword="null"/>。</returns>
    public delegate object InvokeHandler(object instance, MethodInfo method, object[] arguments);

    /// <summary>
    /// 为代理类型 <see cref="DispatchProxy"/> 提供基于 <see cref="InvokeHandler"/> 委托的实现。
    /// </summary>
    /// <typeparam name="TInterface">要代理的类型，应为接口。</typeparam>
    public class DispatchProxy<TInterface> : DispatchProxy where TInterface : class
    {
        /// <summary>
        /// 表示默认的 <see cref="InvokeHandler"/> 代理委托，调用方法并返回。
        /// </summary>
        public static readonly InvokeHandler DefaultHandler =
            (instance, method, arguments) => method.Invoke(instance, arguments);

        /// <summary>
        /// 当前 <see cref="DispatchProxy{TInterface}"/> 的代理对象。
        /// </summary>
        protected TInterface Instance;

        /// <summary>
        /// 当前 <see cref="DispatchProxy{TInterface}"/> 的代理委托。
        /// </summary>
        protected InvokeHandler Handler;

        /// <summary>
        /// 初始化 <see cref="DispatchProxy{TInterface}"/> 类的新实例。用于内部实现，请勿调用。
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DispatchProxy() : base() { }

        /// <summary>
        /// 调用代理类型的方法时实际调用的方法，执行传入的 <see cref="InvokeHandler"/> 委托。
        /// </summary>
        /// <param name="method">当前调用的方法。</param>
        /// <param name="arguments">方法的参数列表。</param>
        /// <returns>方法的返回值。若无返回值，应为 <see langword="null"/>。</returns>
        protected override object Invoke(MethodInfo method, object[] arguments)
        {
            return this.Handler.Invoke(this.Instance, method, arguments);
        }

        /// <summary>
        /// 使用指定的代理对象和代理委托创建 <see cref="DispatchProxy{TInterface}"/> 类的实例。
        /// </summary>
        /// <param name="instance">要代理的 <typeparamref name="TInterface"/> 类型的对象。</param>
        /// <param name="handler">方法调用时所用的 <see cref="InvokeHandler"/> 代理委托。</param>
        /// <returns>以指定的参数初始化的 <see cref="DispatchProxy{TInterface}"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="instance"/>
        /// 或 <paramref name="handler"/> 为 <see langword="null"/>。</exception>
        public static TInterface Create(TInterface instance, InvokeHandler handler)
        {
            if (instance is null) { throw new ArgumentNullException(nameof(instance)); }
            if (handler is null) { throw new ArgumentNullException(nameof(handler)); }
            var proxy = DispatchProxy.Create<TInterface, DispatchProxy<TInterface>>();
            ((DispatchProxy<TInterface>)(object)proxy).Instance = instance;
            ((DispatchProxy<TInterface>)(object)proxy).Handler = handler;
            return proxy;
        }

        /// <summary>
        /// 创建一个指定类型的代理，其代理过程由指定的 <see cref="DispatchProxy"/> 类型定义。
        /// 不支持调用此方法，总是引发 <see cref="NotSupportedException"/> 异常。
        /// </summary>
        /// <typeparam name="T">要代理的类型，应为接口。</typeparam>
        /// <typeparam name="TProxy">定义代理过程的 <see cref="DispatchProxy"/> 类型。</typeparam>
        /// <returns>不支持调用此方法，总是引发 <see cref="NotSupportedException"/> 异常。</returns>
        /// <exception cref="NotSupportedException">不支持调用此方法。</exception>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static new T Create<T, TProxy>() => throw new NotSupportedException();
    }
}
