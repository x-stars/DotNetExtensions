namespace XstarS
{
    /// <summary>
    /// 提供对象的值的验证和对应异常的抛出所需的数据。
    /// </summary>
    /// <typeparam name="T">待验证的对象的类型。</typeparam>
    public interface IValueInfo<out T>
    {
        /// <summary>
        /// 获取待验证的对象的值。
        /// </summary>
        /// <returns>待验证的对象的值。</returns>
        T Value { get; }

        /// <summary>
        /// 获取待验证的对象的名称。
        /// </summary>
        /// <returns>待验证的对象的名称。</returns>
        string? Name { get; }
    }
}
