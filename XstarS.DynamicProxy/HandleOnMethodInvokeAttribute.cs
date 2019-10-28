using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法调用时调用一个 <see cref="MethodInvokeHandler"/> 委托的代理。
    /// 此特性用于 <see cref="CustomProxyFactory"/> 的内部实现，请不要使用此特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class HandleOnMethodInvokeAttribute : OnMethodInvokeAttribute
    {
        /// <summary>
        /// 使用 <see cref="MethodInvokeHandler"/> 被分配的 <see cref="Guid"/>
        /// 初始化 <see cref="OnMethodInvokeAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="handlerGuid">
        /// <see cref="MethodInvokeHandler"/> 被分配的 <see cref="Guid"/>。</param>
        public HandleOnMethodInvokeAttribute(string handlerGuid)
        {
            this.Handler = CustomProxyFactory.GetHandler(Guid.Parse(handlerGuid));
        }

        /// <summary>
        /// 方法调用时调用的代理方法的 <see cref="MethodInvokeHandler"/> 委托。
        /// </summary>
        public MethodInvokeHandler Handler { get; }

        /// <summary>
        /// 在派生类中重写时，表示方法调用时所用的代理。
        /// 默认实现为仅调用方法的 <see cref="MethodInvoker"/> 委托并返回结果。
        /// </summary>
        /// <param name="invoker">方法的 <see cref="MethodInvoker"/> 委托。</param>
        /// <param name="target">实例方法的实例参数的值。</param>
        /// <param name="method">方法的 <see cref="MethodInfo"/> 对象。</param>
        /// <param name="genericArguments">方法的泛型参数的值（如果有）。</param>
        /// <param name="arguments">方法的参数的值。</param>
        /// <returns>调用方法的  <see cref="MethodInvoker"/> 委托得到返回值。</returns>
        public override object Invoke(MethodInvoker invoker, object target,
            MethodInfo method, Type[] genericArguments, object[] arguments)
        {
            return this.Handler(new MethodInvokeInfo(
                invoker, target, method, genericArguments, arguments));
        }
    }
}
