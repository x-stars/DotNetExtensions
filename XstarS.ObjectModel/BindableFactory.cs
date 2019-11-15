using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供基于 <see cref="INotifyPropertyChanged"/> 的可绑定类型，
    /// 并提供创建此可绑定类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class BindableFactory<T> where T : class
    {
        /// <summary>
        /// <see cref="BindableFactory{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<BindableFactory<T>> LazyDefault =
            new Lazy<BindableFactory<T>>(() => new BindableFactory<T>());

        /// <summary>
        /// 提供可绑定类型的 <see cref="BindableTypeProvider"/> 对象。
        /// </summary>
        private readonly BindableTypeProvider TypeProvider;

        /// <summary>
        /// 初始化 <see cref="BindableFactory{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableFactory()
        {
            this.TypeProvider = BindableTypeProvider.OfType(typeof(T));
        }

        /// <summary>
        /// 获取默认的 <see cref="BindableFactory{T}"/> 类的实例。
        /// </summary>
        public static BindableFactory<T> Default =>
            BindableFactory<T>.LazyDefault.Value;

        /// <summary>
        /// 获取原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type BaseType => this.TypeProvider.BaseType;

        /// <summary>
        /// 获取可绑定类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <c>void OnPropertyChanged(string)</c> 方法。</exception>
        public Type BindableType => this.TypeProvider.BindableType;

        /// <summary>
        /// 使用默认构造函数创建可绑定类型的实例。
        /// </summary>
        /// <returns>一个不使用参数创建的可绑定类型的实例。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="BindableFactory{T}.BindableType"/> 不包含无参构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="BindableFactory{T}.BindableType"/> 的无参构造函数访问级别过低。</exception>
        public T CreateInstance() =>
            (T)Activator.CreateInstance(this.BindableType);

        /// <summary>
        /// 使用与指定参数匹配程度最高的构造函数创建可绑定类型的实例。
        /// </summary>
        /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>一个使用指定参数创建的可绑定类型的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="arguments"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// <see cref="BindableFactory{T}.BindableType"/> 
        /// 不包含与 <paramref name="arguments"/> 相匹配的构造函数。</exception>
        /// <exception cref="MethodAccessException">
        /// <see cref="BindableFactory{T}.BindableType"/> 
        /// 中与 <paramref name="arguments"/> 相匹配的构造函数的访问级别过低。</exception>
        public T CreateInstance(params object[] arguments) =>
            (T)Activator.CreateInstance(this.BindableType, arguments);
    }
}
