using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法调用时所用的代理。
    /// </summary>
    /// <param name="instance">方法的实例参数。</param>
    /// <param name="method">当前调用的方法。</param>
    /// <param name="delegate">调用当前方法的委托。</param>
    /// <param name="arguments">方法的参数的值。</param>
    /// <returns>调用方法的 <see cref="MethodDelegate"/> 委托得到返回值。</returns>
    public delegate object? MethodInvokeHandler(
        object instance, MethodInfo method, MethodDelegate @delegate, object?[] arguments);
}
