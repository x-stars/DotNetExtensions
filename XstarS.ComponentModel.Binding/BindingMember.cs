using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个用于数据绑定的对象成员或数组元素。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的成员或元素的类型。</typeparam>
    public abstract class BindingMember<T> : BindingValueBase<T>
    {
        /// <summary>
        /// <see cref="BindingMember{T}.Instance"/> 的值。
        /// </summary>
        private readonly object InternalInstance;

        /// <summary>
        /// <see cref="BindingMember{T}.PropertyName"/> 的值。
        /// </summary>
        private readonly string InternalPropertyName;

        /// <summary>
        /// 使用包含数据绑定值的对象和应绑定到的属性名称初始化 <see cref="BindingMember{T}"/> 类的新实例。
        /// </summary>
        /// <param name="instance">一个包含数据绑定值的对象。
        /// 若用于双向数据绑定，应实现 <see cref="INotifyPropertyChanged"/> 接口。</param>
        /// <param name="propertyName">设置数据绑定时应绑定到的属性名称。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public BindingMember(object instance, string propertyName)
        {
            this.InternalInstance = instance ?? throw new ArgumentNullException(nameof(instance));
            this.InternalPropertyName = propertyName ?? throw new ArgumentNullException(nameof(instance));

            if (!(this.Bindable is null))
            {
                this.Bindable.PropertyChanged += this.Bindable_PropertyChanged;
            }
        }

        /// <summary>
        /// 当前实例的 <see cref="IDisposable"/> 检查对象。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前实例已经被释放。</exception>
        protected new BindingMember<T> Disposable => (BindingMember<T>)base.Disposable;

        /// <summary>
        /// 包含数据绑定值的对象。
        /// </summary>
        public object Instance => this.Disposable.InternalInstance;

        /// <summary>
        /// 设置数据绑定时应绑定到的属性名称。
        /// </summary>
        public string PropertyName => this.Disposable.InternalPropertyName;

        /// <summary>
        /// 包含数据绑定值的对象对应的 <see cref="INotifyPropertyChanged"/> 对象。
        /// 若包含数据绑定值的对象不实现此接口，则为 <see langword="null"/>。
        /// </summary>
        protected INotifyPropertyChanged Bindable => this.Instance as INotifyPropertyChanged;

        /// <summary>
        /// 释放此实例占用的非托管资源。并根据指示释放托管资源。
        /// 将取消当前实例对 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的订阅。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected override void Dispose(bool disposing)
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

                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// <see cref="BindingMember{T}.Bindable"/> 的
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
