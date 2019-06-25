using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供调用代理方法所需的信息。
    /// </summary>
    public sealed class ProxyInvokeInfo
    {
        /// <summary>
        /// 初始化 <see cref="ProxyInvokeInfo"/> 类的新实例。
        /// </summary>
        /// <param name="target">实例方法的实例参数的值。</param>
        /// <param name="method">方法的 <see cref="MethodInfo"/> 对象。</param>
        /// <param name="invoker">方法的 <see cref="MethodInvoker"/> 委托。</param>
        /// <param name="genericArguments">方法的泛型参数的值（如果有）。</param>
        /// <param name="arguments">方法的参数的值。</param>
        public ProxyInvokeInfo(object target, MethodInfo method,
            MethodInvoker invoker, Type[] genericArguments, object[] arguments)
        {
            this.Target = target ?? throw new ArgumentNullException(nameof(target));
            this.Method = method ?? throw new ArgumentNullException(nameof(method));
            this.Invoker = invoker ?? throw new ArgumentNullException(nameof(invoker));
            this.GenericArguments = genericArguments ?? throw new ArgumentNullException(nameof(genericArguments));
            this.Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// 实例方法的实例参数的值。
        /// </summary>
        public object Target { get; }

        /// <summary>
        /// 方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// 方法的 <see cref="MethodInvoker"/> 委托。
        /// </summary>
        public MethodInvoker Invoker { get; }

        /// <summary>
        /// 方法的泛型参数的值（如果有）。
        /// </summary>
        public Type[] GenericArguments { get; }

        /// <summary>
        /// 方法的参数的值。
        /// </summary>
        public object[] Arguments { get; }
    }
}
