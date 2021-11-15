using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法的静态调用委托。
    /// </summary>
    /// <param name="instance">方法的实例参数。</param>
    /// <param name="arguments">方法的所有参数构成的 <see cref="object"/> 类型的数组。
    /// 对于指针和引用传递 <see langword="ref"/>，则已经转换为 <see cref="IntPtr"/> 并装箱；
    /// 使用 <see cref="ProxyServices"/> 类提供的方法读写按引用传递的参数。</param>
    /// <returns>方法的返回值。对于无返回值的方法，则总是 <see langword="null"/>。
    /// 对于指针和引用传递 <see langword="ref"/>，则已经转换为 <see cref="IntPtr"/> 并装箱；
    /// 使用 <see cref="ProxyServices"/> 类提供的方法读写按引用传递的返回值。</returns>
    public delegate object MethodDelegate(object instance, object[] arguments);
}
