using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供数据绑定相关的扩展方法。
    /// </summary>
    public static class BindingExtensions
    {
        /// <summary>
        /// 触发属性改变事件。
        /// </summary>
        /// <remarks>
        /// 基于反射调用，可能存在性能问题。
        /// </remarks>
        /// <param name="source">一个实现 <see cref="INotifyPropertyChanged"/> 接口的对象。</param>
        /// <param name="propertyName">已更改属性的名称。</param>
        internal static void OnPropertyChanged(this INotifyPropertyChanged source, string propertyName)
        {
            var t_source_if_PropertyChanged = source.GetType().GetField(
                nameof(INotifyPropertyChanged.PropertyChanged), BindingFlags.Instance | BindingFlags.NonPublic);
            var source_PropertyChanged = t_source_if_PropertyChanged.GetValue(source) as PropertyChangedEventHandler;
            var t_PropertyChangedEventHandler_im_Invoke =
                typeof(PropertyChangedEventHandler).GetMethod(nameof(PropertyChangedEventHandler.Invoke));
            source_PropertyChanged?.Invoke(source, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 更改属性的值，并通知客户端属性发生更改。
        /// </summary>
        /// <remarks><para>
        /// 请在属性的 <see langword="set"/> 处调用此方法，
        /// 在更改属性值的同时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。
        /// </para><para>
        /// 基于反射调用，可能存在性能问题。
        /// </para></remarks>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="source">一个 <see cref="INotifyPropertyChanged"/> 接口的对象。</param>
        /// <param name="item">属性对应的字段。</param>
        /// <param name="value">属性的新值，一般为 <see langword="value"/> 关键字。</param>
        /// <param name="propertyName">属性的名称，由编译器自动获取。</param>
        public static void SetProperty<T>(this INotifyPropertyChanged source, ref T item, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                source.OnPropertyChanged(propertyName);
            }
        }
    }
}
