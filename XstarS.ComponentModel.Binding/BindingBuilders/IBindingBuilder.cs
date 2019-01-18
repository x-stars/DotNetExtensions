using System;
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
        /// 使用默认构造函数来创建用于数据绑定的类型的实例。
        /// </summary>
        /// <returns>一个用于数据绑定的类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="T"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <typeparamref name="T"/> 的无参构造函数访问级别过低。</exception>
        T CreateInstance();

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建用于数据绑定的类型的实例。
        /// </summary>
        /// <param name="args">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>一个用于数据绑定的类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="args"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="T"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <typeparamref name="T"/> 的无参构造函数访问级别过低。</exception>
        T CreateInstance(params object[] args);
    }
}
