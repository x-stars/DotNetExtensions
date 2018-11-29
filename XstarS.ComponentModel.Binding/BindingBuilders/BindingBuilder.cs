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
        /// 返回一个默认的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定对所有属性设置数据绑定。
        /// </summary>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> 不是接口，
        /// 也不是含有 <see langword="public"/> 或 <see langword="protected"/>
        /// 访问级别的无参构造函数的非密封类。</exception>
        public static BindingBuilder<T> Default
        {
            get
            {
                if (typeof(T).IsInterface)
                {
                    return new InterfaceBindingBuilder<T>();
                }
                else if (typeof(T).IsClass && !typeof(T).IsSealed &&
                    typeof(T).GetConstructors(BindingFlags.Instance |
                    BindingFlags.Public | BindingFlags.NonPublic).Any(ctor =>
                    (ctor.GetParameters().Length == 0) && (ctor.IsPublic || ctor.IsFamily)))
                {
                    return new ClassBindingBuilder<T>();
                }
                else
                {
                    throw new ArgumentException(new ArgumentException().Message, nameof(T));
                }
            }
        }

        /// <summary>
        /// 返回一个默认的 <see cref="BindingBuilder{T}"/> 类的实例，
        /// 并指定仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        /// <exception cref="ArgumentException"><typeparamref name="T"/> 不是接口，
        /// 也不是含有 <see langword="public"/> 或 <see langword="protected"/>
        /// 访问级别的无参构造函数的非密封类。</exception>
        public static BindingBuilder<T> DefaultBindableOnly
        {
            get
            {
                var builder = BindingBuilder<T>.Default;
                builder.BindableOnly = true;
                return builder;
            }
        }

        /// <summary>
        /// 初始化 <see cref="BindingBuilder{T}"/> 类的新实例。
        /// </summary>
        protected BindingBuilder() { }

        /// <summary>
        /// 在派生类中重写时，指示在构建用于数据绑定的动态类型时，
        /// 是否仅对有 <see cref="BindableAttribute"/> 特性的属性设定数据绑定。
        /// </summary>
        public abstract bool BindableOnly { get; set; }

        /// <summary>
        /// 在派生类中重写时，表示用于数据绑定的动态类型的 <see cref="Type"/> 对象。
        /// </summary>
        public abstract Type BindableType { get; }

        /// <summary>
        /// 返回一个用于数据绑定的类型的实例。
        /// </summary>
        /// <returns>一个用于数据绑定的类型的实例。</returns>
        public virtual T CreateInstance() =>
            Activator.CreateInstance(this.BindableType) as T;
    }
}
