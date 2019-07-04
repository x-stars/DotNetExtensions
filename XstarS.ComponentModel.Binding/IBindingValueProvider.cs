namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个可提供用于数据绑定的值 <see cref="IBindingValue"/> 的对象。
    /// </summary>
    internal interface IBindingValueProvider
    {
        /// <summary>
        /// 根据包含数据绑定值的实例获取对应的 <see cref="IBindingValue"/> 对象。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的实例。</param>
        /// <returns><paramref name="instance"/> 对应的 <see cref="IBindingValue"/> 对象。</returns>
        IBindingValue GetBindingValue(object instance);
    }
}
