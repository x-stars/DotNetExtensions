using System;

namespace XstarS
{
    /// <summary>
    /// 提供参数的验证和对应异常的抛出所需的数据。
    /// </summary>
    /// <typeparam name="T">待验证的参数的类型。</typeparam>
    public interface IValidate<out T>
    {
        /// <summary>
        /// 待验证的参数的值。
        /// </summary>
        T Value { get; }

        /// <summary>
        /// 待验证的参数的名称。
        /// </summary>
        string Name { get; }
    }
}
