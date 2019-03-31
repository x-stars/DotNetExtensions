using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型类型构造用于数据绑定的派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型类型，应为接口或非密封类。</typeparam>
    public partial class BindingBuilder<T> : BindingBuilderBase<T> where T : class
    {
        /// <summary>
        /// 初始化 <see cref="BindingBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindingBuilder() : this(false) { }

        /// <summary>
        /// 初始化 <see cref="BindingBuilder{T}"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindingBuilder(bool bindableOnly) : this(typeof(T), bindableOnly) { }

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="BindingBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindingBuilder(Type type) : this(type, false) { }

        /// <summary>
        /// 以指定类型为原型类型初始化 <see cref="BindingBuilder{T}"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        internal BindingBuilder(Type type, bool bindableOnly) : base(bindableOnly)
        {
            if (!typeof(T).IsAssignableFrom(type))
            {
                throw new InvalidCastException();
            }
            this.PrototypeType = type;
            this.InternalBuilder = ObjectBindingBuilder.OfType(type, bindableOnly);
        }

        /// <summary>
        /// 用于数据绑定的类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 用于构造数据绑定的派生类型的 <see cref="ObjectBindingBuilder"/> 对象。
        /// </summary>
        internal ObjectBindingBuilder InternalBuilder { get; }

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
