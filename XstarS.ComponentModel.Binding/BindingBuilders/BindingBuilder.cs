using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型构造用于数据绑定的实例的基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的实例的原型类型，应为非密封类或接口。</typeparam>
    public abstract class BindingBuilder<T> : IBindingBuilder<T>
        where T : class, INotifyPropertyChanged
    {
        /// <summary>
        /// 初始化 <see cref="BindingBuilder{T}"/> 类的静态成员。
        /// </summary>
        static BindingBuilder()
        {
            BindingBuilder<T>.Default = BindingBuilder<T>.Create(false);
            BindingBuilder<T>.Bindable = BindingBuilder<T>.Create(true);
        }

        /// <summary>
        /// 初始化 <see cref="BindingBuilder{T}"/> 类的新实例。
        /// </summary>
        protected BindingBuilder() { }

        /// <summary>
        /// 返回一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定对所有属性设置数据绑定。
        /// </summary>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> 不是接口，
        /// 也不是含有 <see langword="public"/> 或 <see langword="protected"/>
        /// 访问级别的无参构造函数的非密封类。</exception>
        public static BindingBuilder<T> Default { get; }

        /// <summary>
        /// 创建一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> 不是接口，
        /// 也不是含有 <see langword="public"/> 或 <see langword="protected"/>
        /// 访问级别的无参构造函数的非密封类。</exception>
        public static BindingBuilder<T> Bindable { get; }

        /// <summary>
        /// 在派生类中重写时，指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        public abstract bool BindableOnly { get; }

        /// <summary>
        /// 在派生类中重写时，表示用于数据绑定的动态类型的 <see cref="Type"/> 对象。
        /// </summary>
        public abstract Type BindableType { get; }

        /// <summary>
        /// 创建一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定是否仅对有 <see cref="BindableAttribute"/> 特性的属性设置数据绑定。
        /// </summary>
        /// <param name="bindableOnly">指定是否仅对有
        /// <see cref="BindableAttribute"/> 特性的属性设置数据绑定。</param>
        /// <returns>一个 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 其 <see cref="BindingBuilder{T}.BindableOnly"/>
        /// 属性被指定为 <paramref name="bindableOnly"/>。</returns>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> 不是接口，
        /// 也不是含有 <see langword="public"/> 或 <see langword="protected"/>
        /// 访问级别的无参构造函数的非密封类。</exception>
        private static BindingBuilder<T> Create(bool bindableOnly)
        {
            if (typeof(T).IsInterface)
            {
                return new InterfaceBindingBuilder<T>(bindableOnly);
            }
            else if (typeof(T).IsClass && !typeof(T).IsSealed)
            {
                return new ClassBindingBuilder<T>(bindableOnly);
            }
            else
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(T));
            }
        }

        /// <summary>
        /// 返回一个用于数据绑定的类型的实例。
        /// </summary>
        /// <returns>一个用于数据绑定的类型的实例。</returns>
        public virtual T CreateInstance() =>
            Activator.CreateInstance(this.BindableType) as T;
    }
}
