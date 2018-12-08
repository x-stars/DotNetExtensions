using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型构造用于数据绑定的实例的内部基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的实例的原型类型。</typeparam>
    internal abstract class InternalBindingBuilder<T> : BindingBuilder<T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 初始化 <see cref="InterfaceBindingBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        protected InternalBindingBuilder(bool bindableOnly)
        {
            this.BindableOnly = bindableOnly;
            this.BindableType = this.BuildType();
        }

        /// <summary>
        /// 指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        public override bool BindableOnly { get; }

        /// <summary>
        /// 用于数据绑定的动态类型的 <see cref="Type"/> 对象。
        /// </summary>
        public override Type BindableType { get; }

        /// <summary>
        /// 在派生类中重写时，构造用于数据绑定的动态类型。
        /// </summary>
        /// <returns>构造完成的用于数据绑定的动态类型。</returns>
        protected abstract Type BuildType();
    }
}
