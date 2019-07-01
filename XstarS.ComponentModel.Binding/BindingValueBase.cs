using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 为用于数据绑定的值提供基类实现。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的值的类型。</typeparam>
    public abstract class BindingValueBase<T> : IBindingValue<T>
    {
        /// <summary>
        /// 指示当前实例占用的资源是否已经被释放。
        /// </summary>
        private volatile bool IsDisposed = false;

        /// <summary>
        /// <see cref="BindingValueBase{T}.GetValue"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<Func<T>> LazyGetValue;

        /// <summary>
        /// <see cref="BindingValueBase{T}.SetValue"/> 的延迟初始化值。
        /// </summary>
        private readonly Lazy<Func<T, T>> LazySetValue;

        /// <summary>
        /// 使用包含数据绑定值的对象和应绑定到的属性名称初始化 <see cref="BindingValueBase{T}"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。
        /// 若用于双向数据绑定，应实现 <see cref="INotifyPropertyChanged"/> 接口。</param>
        /// <param name="propertyName">设置数据绑定时应绑定到的属性名称。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public BindingValueBase(object instance, string propertyName)
        {
            this.Instance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.PropertyName = propertyName ?? throw new ArgumentNullException(nameof(instance));
            this.LazyGetValue = new Lazy<Func<T>>(this.BuildGetValue);
            this.LazySetValue = new Lazy<Func<T, T>>(this.BuildSetValue);

            if (!(this.Bindable is null))
            {
                this.Bindable.PropertyChanged += this.Bindable_PropertyChanged;
            }
        }

        /// <summary>
        /// 用于数据绑定的值。
        /// </summary>
        /// <exception cref="ArgumentNullException">调用了未能成功构造的属性访问器。</exception>
        public T Value { get => this.GetValue(); set => this.SetValue(value); }

        /// <summary>
        /// 包含数据绑定值的对象。
        /// </summary>
        public object Instance { get; }

        /// <summary>
        /// 设置数据绑定时应绑定到的属性名称。
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 包含数据绑定值的对象对应的 <see cref="INotifyPropertyChanged"/> 对象。
        /// 若包含数据绑定值的对象不实现此接口，则为 <see langword="null"/>。
        /// </summary>
        protected INotifyPropertyChanged Bindable => this.Instance as INotifyPropertyChanged;

        /// <summary>
        /// 用于获取数据绑定值的委托。
        /// </summary>
        protected Func<T> GetValue => this.LazyGetValue.Value;

        /// <summary>
        /// 用于设置数据绑定值的委托。
        /// </summary>
        protected Func<T, T> SetValue => this.LazySetValue.Value;

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
        /// 释放此实例占用的非托管资源。并根据指示释放托管资源。
        /// 将取消当前实例对 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的订阅。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    if (!(this.Bindable is null))
                    {
                        this.Bindable.PropertyChanged -= this.Bindable_PropertyChanged;
                    }
                }

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
        /// 在派生类中重写，用于构造 <see cref="BindingValueBase{T}.GetValue"/> 委托。
        /// </summary>
        /// <returns>构造完成的 <see cref="BindingValueBase{T}.GetValue"/> 委托。</returns>
        protected abstract Func<T> BuildGetValue();

        /// <summary>
        /// 在派生类中重写，用于构造 <see cref="BindingValueBase{T}.SetValue"/> 委托。
        /// </summary>
        /// <returns>构造完成的 <see cref="BindingValueBase{T}.SetValue"/> 委托。</returns>
        protected abstract Func<T, T> BuildSetValue();

        /// <summary>
        /// <see cref="BindingValueBase{T}.Bindable"/> 的
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void Bindable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == this.PropertyName)
            {
                this.OnValueChanged();
            }
        }
    }
}
