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
    public delegate object? InvocationHandler(object instance, MethodInfo method, object?[]? arguments);
}
