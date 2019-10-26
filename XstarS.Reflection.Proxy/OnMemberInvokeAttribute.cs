using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示类型的成员方法调用时所用的代理。仅当其用于可继承的类型时有效，且仅能代理可重写的实例成员方法。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface,
        AllowMultiple = true, Inherited = false)]
    public abstract class OnMemberInvokeAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="OnMemberInvokeAttribute"/> 类的新实例。
        /// </summary>
        protected OnMemberInvokeAttribute() { }

        /// <summary>
        /// 在派生类中重写时，表示用于筛选要代理的成员方法的方法。
        /// </summary>
        /// <param name="method">要确定是否应用代理的成员方法。</param>
        /// <returns>若要对 <paramref name="method"/> 应用当前代理，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public virtual bool FilterMethod(MethodInfo method) => true;

        /// <summary>
        /// 在派生类中重写时，表示类型的成员方法调用时所用的代理。
        /// 默认实现为仅调用成员方法的 <see cref="MethodInvoker"/> 委托并返回结果。
        /// </summary>
        /// <param name="invoker">方法的 <see cref="MethodInvoker"/> 委托。</param>
        /// <param name="target">实例方法的实例参数的值。</param>
        /// <param name="method">方法的 <see cref="MethodInfo"/> 对象。</param>
        /// <param name="genericArguments">方法的泛型参数的值（如果有）。</param>
        /// <param name="arguments">方法的参数的值。</param>
        /// <returns>调用方法的 <see cref="MethodInvoker"/> 委托得到返回值。</returns>
        public virtual object Invoke(MethodInvoker invoker, object target,
            MethodInfo method, Type[] genericArguments, object[] arguments)
        {
            return invoker(target, arguments);
        }
    }
}
