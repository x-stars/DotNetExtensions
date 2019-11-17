using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供基于 <see cref="INotifyPropertyChanged"/> 的值更改通知封装，在值更改时通知客户端。
    /// </summary>
    /// <typeparam name="T"><see cref="Observable{T}"/> 的值的类型。</typeparam>
    [Serializable]
    public class Observable<T> : ObservableObject
    {
        /// <summary>
        /// <see cref="Observable{T}.Value"/> 的值。
        /// </summary>
        private T InternalValue;

        /// <summary>
        /// 初始化 <see cref="Observable{T}"/> 类的新实例。
        /// </summary>
        public Observable() { }

        /// <summary>
        /// 初始化 <see cref="Observable{T}"/> 类的新实例，并将其值设置为指定的值。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的值。</param>
        public Observable(T value) { this.Value = value; }

        /// <summary>
        /// 获取或设置当前 <see cref="Observable{T}"/> 的值。
        /// 更改此属性的值将引发 <see cref="ObservableBase.PropertyChanged"/> 事件。
        /// </summary>
        public T Value
        {
            get => this.InternalValue;
            set => this.SetProperty(ref this.InternalValue, value);
        }

        /// <summary>
        /// 返回表示当前 <see cref="Observable{T}"/> 的值的字符串。
        /// </summary>
        /// <returns><see cref="Observable{T}.Value"/> 的等效字符串表达形式。</returns>
        public override string ToString() => this.Value?.ToString();

        /// <summary>
        /// 创建一个新的 <see cref="Observable{T}"/>，并将其初始化为指定的值。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的值。</param>
        /// <returns>值为 <paramref name="value"/> 的 <see cref="Observable{T}"/>。</returns>
        public static implicit operator Observable<T>(T value) => new Observable<T>(value);

        /// <summary>
        /// 返回指定 <see cref="Observable{T}"/> 的值。
        /// </summary>
        /// <param name="observable">要获取值的 <see cref="Observable{T}"/>。</param>
        /// <returns><paramref name="observable"/> 的值。</returns>
        public static implicit operator T(Observable<T> observable) => observable.Value;
    }
}
