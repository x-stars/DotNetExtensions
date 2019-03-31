using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法调用时所用的代理。
    /// </summary>
    /// <param name="target">实例方法的实例参数的值。</param>
    /// <param name="method">方法的 <see cref="MethodInfo"/> 对象。</param>
    /// <param name="invoker">方法的 <see cref="MethodInvoker"/> 委托。</param>
    /// <param name="genericArguments">方法的泛型参数的值（如果有）。</param>
    /// <param name="arguments">方法的参数的值。</param>
    /// <returns>调用方法的 <see cref="MethodInvoker"/> 委托得到返回值。</returns>
    public delegate object OnInvokeHandler(object target, MethodInfo method,
        MethodInvoker invoker, Type[] genericArguments, object[] arguments);
}
