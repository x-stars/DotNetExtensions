using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性基于 <see cref="INotifyPropertyChanged"/> 的可绑定封装，在属性值更改时通知客户端。
    /// </summary>
    /// <typeparam name="T"><see cref="Bindable{T}"/> 的值的类型。</typeparam>
    [Serializable]
    public class Bindable<T> : BindableObject
    {
        /// <summary>
        /// <see cref="Bindable{T}.Value"/> 的值。
        /// </summary>
        private T InternalValue;

        /// <summary>
        /// 初始化 <see cref="Bindable{T}"/> 类的新实例。
        /// </summary>
        public Bindable() { }

        /// <summary>
        /// 初始化 <see cref="Bindable{T}"/> 类的新实例，并将其值设置为指定的值。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的值。</param>
        public Bindable(T value) { this.Value = value; }

        /// <summary>
        /// 获取或设置当前 <see cref="Bindable{T}"/> 的值。
        /// 更改此属性的值将引发 <see cref="BindableBase.PropertyChanged"/> 事件。
        /// </summary>
        public T Value
        {
            get => this.InternalValue;
            set => this.SetProperty(ref this.InternalValue, value);
        }

        /// <summary>
        /// 返回表示当前 <see cref="Bindable{T}"/> 的值的字符串。
        /// </summary>
        /// <returns><see cref="Bindable{T}.Value"/> 的等效字符串表达形式。</returns>
        public override string ToString() => this.Value?.ToString();

        /// <summary>
        /// 创建一个新的 <see cref="Bindable{T}"/> 对象，并将其初始化为指定的值。
        /// </summary>
        /// <param name="value">作为初始化值的 <typeparamref name="T"/> 类型的对象。</param>
        /// <returns>值为 <paramref name="value"/> 的 <see cref="Bindable{T}"/> 对象。</returns>
        public static implicit operator Bindable<T>(T value) => new Bindable<T>(value);

        /// <summary>
        /// 返回指定 <see cref="Bindable{T}"/> 对象的值。
        /// </summary>
        /// <param name="bindable">要获取值的 <see cref="Bindable{T}"/> 对象。</param>
        /// <returns><paramref name="bindable"/> 的值。</returns>
        public static implicit operator T(Bindable<T> bindable) => bindable.Value;
    }
}
