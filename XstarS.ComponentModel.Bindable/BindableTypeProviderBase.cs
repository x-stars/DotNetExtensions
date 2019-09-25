using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 为可绑定派生类型提供对象 <see cref="IBindableTypeProvider{T}"/> 提供抽象基类实现。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public abstract class BindableTypeProviderBase<T> : IBindableTypeProvider<T> where T : class
    {
        /// <summary>
        /// <see cref="BindableTypeProviderBase{T}.BindableType"/> 的延迟初始化对象。
        /// </summary>
        private readonly Lazy<Type> LazyBindableType;

        /// <summary>
        /// 初始化 <see cref="BindableTypeProviderBase{T}"/> 类的新实例。
        /// </summary>
        protected BindableTypeProviderBase()
        {
            this.LazyBindableType = new Lazy<Type>(this.CreateBindableType);
        }

        /// <summary>
        /// 可绑定派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        public Type BindableType => this.LazyBindableType.Value;

        /// <summary>
        /// 使用默认构造函数创建可绑定派生类型的实例。
        /// </summary>
        /// <returns>一个不使用参数创建的可绑定派生类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="BindableTypeProviderBase{T}.BindableType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="BindableTypeProviderBase{T}.BindableType"/> 的无参构造函数访问级别过低。</exception>
        public T CreateInstance() =>
            (T)Activator.CreateInstance(this.BindableType);

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建可绑定派生类型的实例。
        /// </summary>
        /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>一个使用指定参数创建的可绑定派生类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="BindableTypeProviderBase{T}.BindableType"/> 
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="BindableTypeProviderBase{T}.BindableType"/> 
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public T CreateInstance(params object[] arguments) =>
            (T)Activator.CreateInstance(this.BindableType, arguments);

        /// <summary>
        /// 返回表示可绑定派生类型的字符串。
        /// </summary>
        /// <returns><see cref="BindableTypeProviderBase{T}.BindableType"/> 的字符串表达形式。</returns>
        public override string ToString() => this.BindableType.ToString();

        /// <summary>
        /// 在派生类中重写时，创建可绑定派生类型。
        /// </summary>
        /// <returns>创建的可绑定派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected abstract Type CreateBindableType();
    }
}
