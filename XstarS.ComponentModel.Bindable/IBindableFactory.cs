using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供可绑定派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public interface IBindableFactory<out T> where T : class
    {
        /// <summary>
        /// 可绑定派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        Type BindableType { get; }

        /// <summary>
        /// 使用默认构造函数来创建可绑定派生类型的实例。
        /// </summary>
        /// <returns>一个可绑定派生类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="IBindableFactory{T}.BindableType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="IBindableFactory{T}.BindableType"/> 的无参构造函数访问级别过低。</exception>
        T CreateInstance();

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建可绑定派生类型的实例。
        /// </summary>
        /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>一个使用指定参数创建的可绑定派生类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="IBindableFactory{T}.BindableType"/>
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="IBindableFactory{T}.BindableType"/>
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        T CreateInstance(params object[] arguments);
    }
}
