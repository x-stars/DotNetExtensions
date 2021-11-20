using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法调用时所用的代理。
    /// </summary>
    /// <param name="instance">方法的实例参数。</param>
    /// <param name="method">当前调用的方法。</param>
    /// <param name="delegate">调用当前方法的委托。</param>
    /// <param name="arguments">方法的所有参数构成的 <see cref="object"/> 类型的数组。
    /// 对于指针和引用传递 <see langword="ref"/>，则已经转换为 <see cref="IntPtr"/> 并装箱；
    /// 使用 <see cref="DynamicProxyServices"/> 类提供的方法读写按引用传递的参数。</param>
    /// <returns>调用方法的 <see cref="MethodDelegate"/> 委托得到返回值。
    /// 对于指针和引用传递 <see langword="ref"/>，则已经转换为 <see cref="IntPtr"/> 并装箱；
    /// 使用 <see cref="DynamicProxyServices"/> 类提供的方法读写按引用传递的返回值。</returns>
    public delegate object? MethodInvokeHandler(
        object instance, MethodInfo method, MethodDelegate @delegate, object?[] arguments);
}
