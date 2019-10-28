namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法的通用静态调用方法。
    /// </summary>
    /// <param name="target">方法的实例参数，对于静态方法应为 <see langword="null"/>。</param>
    /// <param name="arguments">方法的所有参数构成的 <see cref="object"/> 类型的数组。</param>
    /// <returns>方法的返回值。对于无返回值的方法，则总是 <see langword="null"/>。</returns>
    public delegate object MethodInvoker(object target, object[] arguments);
}
