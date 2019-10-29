using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供可绑定类型 <see cref="INotifyPropertyChanged"/> 的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        /// <summary>
        /// 初始化 <see cref="BindableBase"/> 类的新实例。
        /// </summary>
        protected BindableBase() { }

        /// <summary>
        /// 在属性值更改时发生。
        /// </summary>
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 引发 <see cref="BindableBase.PropertyChanged"/> 事件。
        /// </summary>
        /// <param name="propertyName">已更改属性的名称，可由编译器自动获取。</param>
        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
