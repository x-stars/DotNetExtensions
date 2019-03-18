﻿using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型类型构造用于数据绑定的派生类型及其实例的基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的类型的原型类型，应为接口或非密封类。</typeparam>
    [Serializable]
    public abstract class BindingBuilder<T> : IBindingBuilder<T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 对所有属性设置数据绑定的 <see cref="BindingBuilder{T}"/> 类的实例。
        /// </summary>
        private static BindingBuilder<T> DefaultBuilder;

        /// <summary>
        /// 仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定的
        /// <see cref="BindingBuilder{T}"/> 类的实例。
        /// </summary>
        private static BindingBuilder<T> BindableBuilder;

        /// <summary>
        /// 用于数据绑定的派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        [NonSerialized]
        private Type BindableDerivedType;

        /// <summary>
        /// 初始化 <see cref="BindingBuilder{T}"/> 类的新实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        protected BindingBuilder(bool bindableOnly)
        {
            this.BindableOnly = bindableOnly;
        }

        /// <summary>
        /// 返回一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定对所有属性设置数据绑定。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> Default => BindingBuilder<T>.DefaultBuilder ??
            (BindingBuilder<T>.DefaultBuilder = BindingBuilder<T>.Create(false));

        /// <summary>
        /// 返回一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> Bindable => BindingBuilder<T>.BindableBuilder ??
            (BindingBuilder<T>.BindableBuilder = BindingBuilder<T>.Create(true));

        /// <summary>
        /// 指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        public bool BindableOnly { get; }

        /// <summary>
        /// 用于数据绑定的派生类型的 <see cref="Type"/> 对象。
        /// </summary>
        /// <exception cref="MissingMethodException">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件已经实现，
        /// 但未定义公共或保护级别的 <code>void OnPropertyChanged(string)</code> 方法。</exception>
        public Type BindableType => this.BindableDerivedType ??
            (this.BindableDerivedType = this.BuildType());

        /// <summary>
        /// 创建一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 其 <see cref="BindingBuilder{T}.BindableOnly"/>
        /// 属性被指定为 <paramref name="bindableOnly"/>。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private static BindingBuilder<T> Create(bool bindableOnly)
        {
            if (typeof(T).IsInterface)
            {
                return new InterfaceBindingBuilder<T>(bindableOnly);
            }
            else if (typeof(T).IsClass)
            {
                return new ClassBindingBuilder<T>(bindableOnly);
            }
            else
            {
                throw new TypeAccessException();
            }
        }

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
        protected abstract Type BuildType();
    }
}
