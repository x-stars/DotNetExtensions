using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型类型构造用于数据绑定的派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型类型，应为接口或非密封类。</typeparam>
    public interface IBindingBuilder<out T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        bool BindableOnly { get; }

        /// <summary>
        /// 用于数据绑定的派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        Type BindableType { get; }

        /// <summary>
        /// 使用默认构造函数来创建用于数据绑定的类型的实例。
        /// </summary>
        /// <returns>一个用于数据绑定的类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="T"/> 不为接口且不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <typeparamref name="T"/> 的无参构造函数访问级别过低。</exception>
        T CreateInstance();

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建用于数据绑定的类型的实例。
        /// </summary>
        /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>一个用于数据绑定的类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException"><typeparamref name="T"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException"><typeparamref name="T"/>
        /// 与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        T CreateInstance(params object[] arguments);
    }
}
