using System;
using System.Collections.Concurrent;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    public partial class BindableBuilder
    {
        /// <summary>
        /// 对所有属性设置数据绑定的
        /// <see cref="BindableBuilder"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<BindableBuilder>>
            DefaultOfTypes = new ConcurrentDictionary<Type, Lazy<BindableBuilder>>();

        /// <summary>
        /// 仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定的
        /// <see cref="BindableBuilder"/> 类的实例的存储对象。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<BindableBuilder>>
            BindableOnlyOfTypes = new ConcurrentDictionary<Type, Lazy<BindableBuilder>>();

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindableBuilder"/> 类的实例，
        /// 并指定对所有属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <returns>一个对所有属性设置数据绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder DefaultOfType(Type type) =>
            BindableBuilder.DefaultOfTypes.GetOrAdd(type,
                newType => new Lazy<BindableBuilder>(
                    () => BindableBuilder.Create(newType, false))).Value;

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindableBuilder"/> 类的实例，
        /// 并指定仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <returns>一个仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder BindableOnlyOfType(Type type) =>
            BindableBuilder.BindableOnlyOfTypes.GetOrAdd(type,
                newType => new Lazy<BindableBuilder>(
                    () => BindableBuilder.Create(newType, true))).Value;

        /// <summary>
        /// 返回一个以指定类型为原型类型的 <see cref="BindableBuilder"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        public static BindableBuilder OfType(Type type, bool bindableOnly) => bindableOnly ?
            BindableBuilder.BindableOnlyOfType(type) : BindableBuilder.DefaultOfType(type);

        /// <summary>
        /// 创建一个以指定类型为原型类型的 <see cref="BindableBuilder"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="type">用于数据绑定的类型的原型类型。</param>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个根据 <paramref name="bindableOnly"/> 的指示设定属性的绑定的原型类型为
        /// <paramref name="type"/> 的 <see cref="BindableBuilder"/> 类的实例。</returns>
        /// <exception cref="TypeAccessException">
        /// <paramref name="type"/> 不是公共接口，也不是公共非密封类。</exception>
        private static BindableBuilder Create(Type type, bool bindableOnly) =>
            new BindableBuilder(type, bindableOnly);
    }
}
