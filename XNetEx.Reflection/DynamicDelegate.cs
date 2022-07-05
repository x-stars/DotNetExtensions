namespace XNetEx;

/// <summary>
/// 表示委托的动态调用委托。
/// </summary>
/// <param name="arguments">委托的所有参数构成的 <see cref="object"/> 类型的数组。</param>
/// <returns>委托的返回值。对于无返回值的委托，则总是 <see langword="null"/>。</returns>
public delegate object? DynamicDelegate(params object?[]? arguments);
