using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    public partial class BindableBuilder<T>
    {
        /// <summary>
        /// <see cref="BindableBuilder{T}.Default"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<BindableBuilder<T>> LazyDefault =
            new Lazy<BindableBuilder<T>>(() => BindableBuilder<T>.Create(false));

        /// <summary>
        /// <see cref="BindableBuilder{T}.BindableOnly"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<BindableBuilder<T>> LazyBindableOnly =
            new Lazy<BindableBuilder<T>>(() => BindableBuilder<T>.Create(true));

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
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder<T> Of(bool bindableOnly) => bindableOnly ?
            BindableBuilder<T>.BindableOnly : BindableBuilder<T>.Default;

        /// <summary>
        /// 创建一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindableBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindableBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private static BindableBuilder<T> Create(bool bindableOnly) =>
            new BindableBuilder<T>(bindableOnly);
    }
}
