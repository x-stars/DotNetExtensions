using System;
using System.ComponentModel;
using System.Reflection;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供可绑定派生类型，并提供创建此派生类型的实例的方法。
    /// </summary>
    /// <typeparam name="T">原型类型，应为接口或非密封类。</typeparam>
    public sealed class BindableTypeProvider<T> : BindableTypeProviderBase<T> where T : class
    {
        /// <summary>
        /// <see cref="BindableTypeProvider{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<BindableTypeProvider<T>> LazyDefault =
            new Lazy<BindableTypeProvider<T>>(() => new BindableTypeProvider<T>());

        /// <summary>
        /// 初始化 <see cref="BindableTypeProvider{T}"/> 类的新实例，
        /// 并指定将所有可重写属性设置为可绑定属性。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableTypeProvider()
        {
            this.InternalProvider = BindableTypeProvider.Default(typeof(T));
        }

        /// <summary>
        /// 初始化 <see cref="BindableTypeProvider{T}"/> 类的新实例，
        /// 并指定将符合指定条件的可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableTypeProvider(Predicate<PropertyInfo> isBindable)
        {
            this.InternalProvider = BindableTypeProvider.Custom(typeof(T), isBindable);
        }

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableTypeProvider{T}"/> 类的实例，
        /// 并指定将所有可重写属性设置为可绑定属性。
        /// </summary>
        /// <returns>一个将所有可重写属性设置为可绑定属性的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableTypeProvider{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableTypeProvider<T> Default => BindableTypeProvider<T>.LazyDefault.Value;

        /// <summary>
        /// 原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType => this.InternalProvider.PrototypeType;

        /// <summary>
        /// 提供可绑定派生类型的 <see cref="BindableTypeProvider"/> 对象。
        /// </summary>
        private BindableTypeProvider InternalProvider { get; }

        /// <summary>
        /// 创建一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableTypeProvider"/> 类的实例，
        /// 并指定将符合指定条件的可重写属性设置为可绑定属性。
        /// </summary>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>一个将符合 <paramref name="isBindable"/> 条件的可重写属性设置为可绑定属性的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableTypeProvider"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableTypeProvider<T> Custom(Predicate<PropertyInfo> isBindable) =>
            new BindableTypeProvider<T>(isBindable);

        /// <summary>
        /// 构造可绑定派生类型。
        /// </summary>
        /// <returns>构造完成的可绑定派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected override Type BuildBindableType() => this.InternalProvider.BindableType;
    }
}
