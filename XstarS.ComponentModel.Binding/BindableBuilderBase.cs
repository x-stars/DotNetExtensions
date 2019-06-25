using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型类型构造用于数据绑定的派生类型及其实例的基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型类型，应为接口或非密封类。</typeparam>
    public abstract class BindableBuilderBase<T> : IBindableBuilder<T> where T : class
    {
        /// <summary>
        /// <see cref="BindableBuilderBase{T}.BindableType"/> 的延迟初始化对象。
        /// </summary>
        private readonly Lazy<Type> LazyBindableType;

        /// <summary>
        /// 初始化 <see cref="BindableBuilderBase{T}"/> 类的新实例。
        /// </summary>
        protected BindableBuilderBase()
        {
            this.LazyBindableType = new Lazy<Type>(this.BuildBindableType);
        }

        /// <summary>
        /// 用于数据绑定的派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        public Type BindableType => this.LazyBindableType.Value;

        /// <summary>
        /// 使用默认构造函数来创建用于数据绑定的类型的实例。
        /// </summary>
        /// <returns>一个用于数据绑定的类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <typeparamref name="T"/> 不为接口且不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <typeparamref name="T"/> 的无参构造函数访问级别过低。</exception>
        public T CreateInstance() =>
            (T)Activator.CreateInstance(this.BindableType);

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
        public T CreateInstance(params object[] arguments) =>
            (T)Activator.CreateInstance(this.BindableType, arguments);

        /// <summary>
        /// 在派生类中重写时，构造用于数据绑定的派生类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected abstract Type BuildBindableType();
    }
}
