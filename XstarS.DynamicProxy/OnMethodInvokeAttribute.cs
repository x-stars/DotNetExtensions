using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法调用时所用的代理。仅当其用于可重写的方法时有效。
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public abstract class OnMethodInvokeAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="OnMethodInvokeAttribute"/> 类的新实例。
        /// </summary>
        protected OnMethodInvokeAttribute() { }

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
        public virtual object Invoke(MethodInvoker invoker, object target,
            MethodInfo method, Type[] genericArguments, object[] arguments)
        {
            return invoker(target, arguments);
        }
    }
}
