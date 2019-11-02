using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供调用代理方法时所需的信息。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays")]
    public sealed class ProxyInvokeInfo
    {
        /// <summary>
        /// 初始化 <see cref="ProxyInvokeInfo"/> 类的新实例。
        /// </summary>
        /// <param name="instance">方法的实例参数。</param>
        /// <param name="method">当前调用的方法。</param>
        /// <param name="genericArguments">方法的泛型参数的值。</param>
        /// <param name="arguments">方法的参数的值。</param>
        /// <param name="baseDelegate">调用当前方法的委托。</param>
        public ProxyInvokeInfo(object instance, MethodInfo method,
            Type[] genericArguments, object[] arguments, MethodDelegate baseDelegate)
        {
            this.Instance = instance;
            this.Method = method ??
                throw new ArgumentNullException(nameof(method));
            this.GenericArguments = genericArguments ??
                throw new ArgumentNullException(nameof(genericArguments));
            this.Arguments = arguments ??
                throw new ArgumentNullException(nameof(arguments));
            this.BaseDelegate = baseDelegate ??
                throw new ArgumentNullException(nameof(baseDelegate));
        }

        /// <summary>
        /// 方法的实例参数。
        /// </summary>
        public object Instance { get; }

        /// <summary>
        /// 当前调用的方法。
        /// </summary>
        public MethodInfo Method { get; }

        /// <summary>
        /// 方法的泛型参数。
        /// </summary>
        public Type[] GenericArguments { get; }

        /// <summary>
        /// 方法的参数。
        /// </summary>
        public object[] Arguments { get; }

        /// <summary>
        /// 当前方法的委托调用。
        /// </summary>
        public MethodDelegate BaseDelegate { get; }
    }
}
