﻿using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供观察指定名称属性的方法，并在属性发生更改时发出通知。
    /// </summary>
    internal sealed class SimplePropertyObserver : IDisposable
    {
        /// <summary>
        /// 使用要观察的对象和属性的名称初始化 <see cref="SimplePropertyObserver"/> 类的新实例。
        /// </summary>
        /// <param name="source">属性更改通知的事件源对象。</param>
        /// <param name="propertyName">要接收更改通知的属性的名称。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public SimplePropertyObserver(INotifyPropertyChanged source, string propertyName)
        {
            this.Source = source ??
                throw new ArgumentNullException(nameof(source));
            this.PropertyName = propertyName;
            this.Source.PropertyChanged += this.OnSourcePropertyChanged;
        }

        /// <summary>
        /// 获取属性更改通知的事件源对象。
        /// </summary>
        /// <returns>属性更改通知的事件源对象。</returns>
        public INotifyPropertyChanged Source { get; }

        /// <summary>
        /// 获取要接收更改通知的属性的名称。
        /// </summary>
        /// <returns>要接收更改通知的属性的名称。</returns>
        public string PropertyName { get; }

        /// <summary>
        /// 在当前正在观察的属性更改时发生。
        /// </summary>
        public event EventHandler ObservingPropertyChanged;

        /// <summary>
        /// 释放当前对象占用的资源。
        /// </summary>
        public void Dispose()
        {
            this.Source.PropertyChanged -= this.OnSourcePropertyChanged;
        }

        /// <summary>
        /// 使用指定的事件数据引发 <see cref="SimplePropertyObserver.ObservingPropertyChanged"/> 事件。
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="EventArgs"/>。</param>
        private void OnObservingPropertyChanged(EventArgs e)
        {
            this.ObservingPropertyChanged?.Invoke(this, e);
        }

        /// <summary>
        /// 当指定名称的属性发生更改时，引发 <see cref="SimplePropertyObserver.ObservingPropertyChanged"/> 事件。
        /// </summary>
        /// <param name="sender">属性更改通知的事件源。</param>
        /// <param name="e">提供属性更改通知的事件数据。</param>
        private void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.PropertyName == e.PropertyName)
            {
                this.OnObservingPropertyChanged(EventArgs.Empty);
            }
        }
    }
}
