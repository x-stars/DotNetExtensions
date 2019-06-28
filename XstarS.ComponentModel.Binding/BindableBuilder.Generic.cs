using System;
using System.ComponentModel;
using System.Reflection;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型类型构造用于数据绑定的派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型类型，应为接口或非密封类。</typeparam>
    public sealed class BindableBuilder<T> : BindableBuilderBase<T> where T : class
    {
        /// <summary>
        /// <see cref="BindableBuilder{T}.Default"/> 的延迟初始化值。
        /// </summary>
        private static readonly Lazy<BindableBuilder<T>> LazyDefault =
            new Lazy<BindableBuilder<T>>(() => new BindableBuilder<T>());

        /// <summary>
        /// 初始化 <see cref="BindableBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableBuilder()
        {
            this.InternalBuilder = BindableBuilder.Default(typeof(T));
        }

        /// <summary>
        /// 以用于筛选可绑定属性的条件初始化 <see cref="BindableBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private BindableBuilder(Predicate<PropertyInfo> isBindable)
        {
            this.InternalBuilder = BindableBuilder.Custom(typeof(T), isBindable);
        }

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableBuilder{T}"/> 类的实例，
        /// 并指定对所有可重写属性设置数据绑定。
        /// </summary>
        /// <returns>一个对所有属性设置数据绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder<T> Default => BindableBuilder<T>.LazyDefault.Value;

        /// <summary>
        /// 用于数据绑定的类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType => this.InternalBuilder.PrototypeType;

        /// <summary>
        /// 指示当前 <see cref="BindableBuilder{T}"/> 是否用于构造默认的数据绑定派生类型。
        /// </summary>
        public bool IsDefault => this.InternalBuilder.IsDefault;

        /// <summary>
        /// 用于构造数据绑定的派生类型的 <see cref="BindableBuilder"/> 对象。
        /// </summary>
        private BindableBuilder InternalBuilder { get; }

        /// <summary>
        /// 创建一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableBuilder"/> 类的实例，
        /// 并指定对符合指定条件的可重写属性设置数据绑定。
        /// </summary>
        /// <param name="isBindable">用于筛选可绑定属性的 <see cref="Predicate{T}"/> 委托。</param>
        /// <returns>一个对符合 <paramref name="isBindable"/> 条件的可重写属性设置数据绑定的原型类型为
        ///  <typeparamref name="T"/>  的 <see cref="BindableBuilder"/> 类的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="isBindable"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="TypeAccessException">
        ///  <typeparamref name="T"/>  不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder<T> Custom(Predicate<PropertyInfo> isBindable) =>
            new BindableBuilder<T>(isBindable);

        /// <summary>
        /// 构造用于数据绑定的派生类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的派生类型。</returns>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        protected override Type BuildBindableType() => this.InternalBuilder.BindableType;
    }
}
