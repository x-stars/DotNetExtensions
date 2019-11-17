using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供基于 <see cref="INotifyPropertyChanged"/> 的属性更改通知类型，
    /// 并提供创建此属性更改通知类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class ObservableFactory<T> where T : class
    {
        /// <summary>
        /// <see cref="ObservableFactory{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<ObservableFactory<T>> LazyDefault =
            new Lazy<ObservableFactory<T>>(() => new ObservableFactory<T>());

        /// <summary>
        /// 提供属性更改通知类型的 <see cref="ObservableTypeProvider"/> 对象。
        /// </summary>
        private readonly ObservableTypeProvider TypeProvider;

        /// <summary>
        /// 初始化 <see cref="ObservableFactory{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private ObservableFactory()
        {
            this.TypeProvider = ObservableTypeProvider.OfType(typeof(T));
        }

        /// <summary>
        /// 获取默认的 <see cref="ObservableFactory{T}"/> 类的实例。
        /// </summary>
        public static ObservableFactory<T> Default =>
            ObservableFactory<T>.LazyDefault.Value;

        /// <summary>
        /// 获取原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType => this.TypeProvider.BaseType;

        /// <summary>
        /// 获取属性更改通知类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <c>void OnPropertyChanged(string)</c> 方法。</exception>
        public Type ObservableType => this.TypeProvider.ObservableType;

        /// <summary>
        /// 使用默认构造函数创建属性更改通知类型的实例。
        /// </summary>
        /// <returns>一个不使用参数创建的属性更改通知类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="ObservableFactory{T}.ObservableType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ObservableFactory{T}.ObservableType"/> 的无参构造函数访问级别过低。</exception>
        public T CreateInstance() =>
            (T)Activator.CreateInstance(this.ObservableType);

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建属性更改通知类型的实例。
        /// </summary>
        /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>一个使用指定参数创建的属性更改通知类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="ObservableFactory{T}.ObservableType"/> 
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="ObservableFactory{T}.ObservableType"/> 
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public T CreateInstance(params object[] arguments) =>
            (T)Activator.CreateInstance(this.ObservableType, arguments);
    }
}
