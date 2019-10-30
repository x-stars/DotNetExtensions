using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供可绑定类型 <see cref="INotifyPropertyChanged"/> 基于字段的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class BindableObject : BindableBase
    {
        /// <summary>
        /// 初始化 <see cref="BindableObject"/> 类的新实例。
        /// </summary>
        protected BindableObject() { }

        /// <summary>
        /// 更改属性的值，并引发 <see cref="BindableBase.PropertyChanged"/> 事件。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="field">属性对应的字段。</param>
        /// <param name="value">属性的新值，一般为 <see langword="value"/>。</param>
        /// <param name="propertyName">属性的名称，可由编译器自动获取。</param>
        protected void SetProperty<T>(ref T field, T value,
            [CallerMemberName] string propertyName = null)
        {
            field = value;
            this.OnPropertyChanged(propertyName);
        }
    }
}
