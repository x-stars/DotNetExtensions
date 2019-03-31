using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    public partial class BindingBuilder<T>
    {
        /// <summary>
        /// <see cref="BindingBuilder{T}.Default"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<BindingBuilder<T>> LazyDefault =
            new Lazy<BindingBuilder<T>>(() => BindingBuilder<T>.Create(false));

        /// <summary>
        /// <see cref="BindingBuilder{T}.BindableOnly"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<BindingBuilder<T>> LazyBindableOnly =
            new Lazy<BindingBuilder<T>>(() => BindingBuilder<T>.Create(true));

        /// <summary>
        /// 对所有属性设置数据绑定的
        /// <see cref="BindingBuilder{T}"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<BindingBuilder<T>>>
            DefaultOfTypes = new ConcurrentDictionary<Type, Lazy<BindingBuilder<T>>>();

        /// <summary>
        /// 仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定的
        /// <see cref="BindingBuilder{T}"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<BindingBuilder<T>>>
            BindableOnlyOfTypes = new ConcurrentDictionary<Type, Lazy<BindingBuilder<T>>>();

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定对所有属性设置数据绑定。
        /// </summary>
        /// <returns>一个对所有属性设置数据绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> Default => BindingBuilder<T>.LazyDefault.Value;

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <returns>一个仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> BindableOnly => BindingBuilder<T>.LazyBindableOnly.Value;

        /// <summary>
        /// 返回一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> Of(bool bindableOnly) => bindableOnly ?
            BindingBuilder<T>.BindableOnly : BindingBuilder<T>.Default;

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定对所有属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <returns>一个对所有属性设置数据绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> DefaultOfType(Type type) =>
            BindingBuilder<T>.DefaultOfTypes.GetOrAdd(type,
                newType => new Lazy<BindingBuilder<T>>(
                    () => BindingBuilder<T>.Create(newType, false))).Value;

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <returns>一个仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> BindableOnlyOfType(Type type) =>
            BindingBuilder<T>.BindableOnlyOfTypes.GetOrAdd(type,
                newType => new Lazy<BindingBuilder<T>>(
                    () => BindingBuilder<T>.Create(newType, false))).Value;

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindingBuilder<T> OfType(Type type, bool bindableOnly) => bindableOnly ?
            BindingBuilder<T>.BindableOnlyOfType(type) : BindingBuilder<T>.DefaultOfType(type);

        /// <summary>
        /// 创建一个以 <typeparamref name="T"/> 为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <typeparamref name="T"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        private static BindingBuilder<T> Create(bool bindableOnly) =>
            new BindingBuilder<T>(bindableOnly);

        /// <summary>
        /// 创建一个以指定类型为原型类型的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindingBuilder{T}"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindingBuilder{T}"/> 类的实例。</returns>
        /// <exception cref="InvalidCastException">
        /// <paramref name="type"/> 类型的实例不能转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        private static BindingBuilder<T> Create(Type type, bool bindableOnly) =>
            new BindingBuilder<T>(type, bindableOnly);
    }
}
