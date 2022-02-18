namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法的动态调用委托。
    /// </summary>
    /// <param name="instance">方法的实例参数。对于静态方法，应为 <see langword="null"/>。</param>
    /// <param name="arguments">方法的所有参数构成的 <see cref="object"/> 类型的数组。</param>
    /// <returns>方法的返回值。对于无返回值的方法，则总是 <see langword="null"/>。</returns>
    public delegate object? MethodDelegate(object? instance, object?[]? arguments);
}
