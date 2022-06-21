using System.Runtime.CompilerServices;

namespace XNetEx;

/// <summary>
/// 提供对象的值的验证与对应异常的抛出的方法。
/// 应先调用 <see cref="Validate.Value{T}(T, string)"/> 方法，
/// 创建一个 <see cref="IValueInfo{T}"/> 接口的实例再进行值的验证。
/// </summary>
public static partial class Validate
{
    /// <summary>
    /// 使用值和名称创建一个 <see cref="IValueInfo{T}"/> 接口的新实例。
    /// </summary>
    /// <typeparam name="T">待验证的对象的类型。</typeparam>
    /// <param name="value">待验证的对象的值。</param>
    /// <param name="name">待验证的对象的名称。</param>
    /// <returns><see cref="IValueInfo{T}"/> 接口的新实例。</returns>
    public static IValueInfo<T> Value<T>(T value,
        [CallerArgumentExpression("value")] string? name = null)
    {
        return new ValueInfo<T>(value, name);
    }
}
