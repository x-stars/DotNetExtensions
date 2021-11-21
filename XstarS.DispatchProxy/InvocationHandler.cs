using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示调用代理类型方法时所用的代理委托。
    /// </summary>
    /// <param name="instance">代理类型包装的代理对象。</param>
    /// <param name="method">调用方调用的方法。</param>
    /// <param name="arguments">调用方传递给方法的参数。</param>
    /// <returns>要返回给调用方的对象。或者对于
    /// <see langword="void"/> 方法，应为 <see langword="null"/>。</returns>
    public delegate object InvocationHandler(object instance, MethodInfo method, object[] arguments);
}
