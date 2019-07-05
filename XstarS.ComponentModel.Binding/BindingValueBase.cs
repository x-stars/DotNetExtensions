using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 为用于数据绑定的值提供基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的值的类型。</typeparam>
    public abstract class BindingValueBase<T> : IBindingValue, IBindingValue<T>
    {
        /// <summary>
        /// <see cref="BindingValueBase{T}.GetValue"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<Func<T>> LazyGetValue;

        /// <summary>
        /// <see cref="BindingValueBase{T}.SetValue"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<Action<T>> LazySetValue;

        /// <summary>
        /// 初始化 <see cref="BindingValueBase{T}"/> 类的新实例。
        /// </summary>
        public BindingValueBase()
        {
            this.IsDisposed = false;
            this.LazyGetValue = new Lazy<Func<T>>(this.BuildGetValue);
            this.LazySetValue = new Lazy<Action<T>>(this.BuildSetValue);
        }

        /// <summary>
        /// 指示当前实例占用的资源是否已经被释放。
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// 当前实例的 <see cref="IDisposable"/> 检查对象。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前实例已经被释放。</exception>
        protected BindingValueBase<T> Disposable =>
            this.IsDisposed ? throw new ObjectDisposedException(this.GetType().ToString()) : this;

        /// <summary>
        /// 用于数据绑定的值。
        /// </summary>
        /// <exception cref="MissingMemberException">调用了未能正确构造数据绑定值访问器。</exception>
        public T Value { get => this.GetValue(); set => this.SetValue(value); }

        /// <summary>
        /// 用于获取数据绑定值的委托。
        /// </summary>
        /// <exception cref="MissingMemberException">无法正确构造获取数据绑定值的委托。</exception>
        protected Func<T> GetValue => this.Disposable.LazyGetValue.Value;

        /// <summary>
        /// 用于设置数据绑定值的委托。
        /// </summary>
        /// <exception cref="MissingMemberException">无法正确构造设置数据绑定值的委托。</exception>
        protected Action<T> SetValue => this.Disposable.LazySetValue.Value;

        /// <summary>
        /// 用于数据绑定的值。
        /// </summary>
        /// <exception cref="MissingMemberException">调用了未能正确构造数据绑定值访问器。</exception>
        object IBindingValue.Value
        {
            get => this.Value;
            set => this.Value = (value is T newValue) ? newValue : default;
        }

        /// <summary>
        /// 在数据绑定值更改时发生。
        /// </summary>
        public event EventHandler ValueChanged;

        /// <summary>
        /// 释放此实例占用的资源。
        /// 将取消当前实例对 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的订阅。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 在派生类中重写，用于释放此实例占用的非托管资源。并根据指示释放托管资源。
        /// 应取消当前实例对 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的订阅。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                this.IsDisposed = true;
            }
        }

        /// <summary>
        /// 触发 <see cref="BindingValueBase{T}.ValueChanged"/> 事件。
        /// </summary>
        protected virtual void OnValueChanged()
        {
            this.ValueChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 在派生类中重写，用于构造获取数据绑定值的委托。
        /// </summary>
        /// <returns>构造完成的获取数据绑定值的委托。</returns>
        /// <exception cref="MissingMemberException">无法正确构造获取数据绑定值的委托。</exception>
        protected abstract Func<T> BuildGetValue();

        /// <summary>
        /// 在派生类中重写，用于构造设置数据绑定值的委托。
        /// </summary>
        /// <returns>构造完成的设置数据绑定值的委托。</returns>
        /// <exception cref="MissingMemberException">无法正确构造设置数据绑定值的委托。</exception>
        protected abstract Action<T> BuildSetValue();
    }
}
