using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型类型构造用于数据绑定的派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型类型，应为接口或非密封类。</typeparam>
    public class BindableBuilder<T> : BindableBuilderBase<T> where T : class
    {
        /// <summary>
        /// <see cref="BindableBuilder{T}.Default"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<BindableBuilder<T>> LazyDefault =
            new Lazy<BindableBuilder<T>>(() => new BindableBuilder<T>(false));

        /// <summary>
        /// <see cref="BindableBuilder{T}.BindableOnly"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<BindableBuilder<T>> LazyBindableOnly =
            new Lazy<BindableBuilder<T>>(() => new BindableBuilder<T>(true));

        /// <summary>
        /// 初始化 <see cref="BindableBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindableBuilder() : this(false) { }

        /// <summary>
        /// 初始化 <see cref="BindableBuilder{T}"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindableBuilder(bool bindableOnly)
        {
            var type = typeof(T);
            this.PrototypeType = type;
            this.InternalBuilder = bindableOnly ?
                BindableBuilder.BindableOnly(type) : BindableBuilder.Default(type);
        }

        /// <summary>
        /// 用于数据绑定的类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 用于构造数据绑定的派生类型的 <see cref="BindableBuilder"/> 对象。
        /// </summary>
        internal BindableBuilder InternalBuilder { get; }

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableBuilder{T}"/> 类的实例，
        /// 并指定对所有属性设置数据绑定。
        /// </summary>
        /// <returns>一个对所有属性设置数据绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder<T> Default => BindableBuilder<T>.LazyDefault.Value;

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableBuilder{T}"/> 类的实例，
        /// 并指定仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <returns>一个仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder<T> BindableOnly => BindableBuilder<T>.LazyBindableOnly.Value;

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
