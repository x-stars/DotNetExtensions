using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型构造用于数据绑定的实例的方法。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的实例的原型类型。</typeparam>
    public interface IBindingBuilder<out T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        bool BindableOnly { get; }

        /// <summary>
        /// 返回一个构造完成的用于数据绑定的类型的实例。
        /// </summary>
        /// <returns>一个构造完成的用于数据绑定的类型的实例。</returns>
        T CreateInstance();
    }
}
