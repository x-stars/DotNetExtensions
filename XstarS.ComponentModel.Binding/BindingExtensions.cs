using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        /// <remarks><para>
        /// 基于反射调用，可能存在性能问题。
        /// </para><para>
        /// 当前对象最好应该直接实现 <see cref="INotifyPropertyChanged"/> 接口，
        /// 便于搜寻 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的委托。
        /// 若当前对象的类型显式实现 <see cref="INotifyPropertyChanged"/> 接口，或接口实现在基类中，
        /// 则会循环向下搜索第一个类型为 <see cref="PropertyChangedEventHandler"/> 的实例字段，将其作为事件委托。
        /// </para></remarks>
        /// <param name="source">一个实现 <see cref="INotifyPropertyChanged"/> 接口的对象。</param>
        /// <param name="propertyName">已更改属性的名称。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static void OnPropertyChanged(this INotifyPropertyChanged source, string propertyName)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }

            // 搜寻当前类型中名为的 PropertyChanged 且类型为 PropertyChangedEventHandler 的字段。
            var t_source = source.GetType();
            var t_source_if_PropertyChanged = t_source.GetField(
                nameof(INotifyPropertyChanged.PropertyChanged),
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            t_source_if_PropertyChanged =
                t_source_if_PropertyChanged?.FieldType == typeof(PropertyChangedEventHandler) ?
                t_source_if_PropertyChanged : null;

            // 搜索失败则从当前类型开始向基类方向逐层搜索类型为 PropertyChangedEventHandler 的字段。
            if (t_source_if_PropertyChanged is null)
            {
                for (var t_base = t_source; !(t_base is null); t_base = t_base.BaseType)
                {
                    t_source_if_PropertyChanged = (
                        from field in t_base.GetFields(
                            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                        where field.FieldType == typeof(PropertyChangedEventHandler)
                        select field).FirstOrDefault();
                    if (!(t_source_if_PropertyChanged is null)) { break; }
                }
            }

            // 获取事件委托字段的值，并调用委托。
            var source_PropertyChanged =
                t_source_if_PropertyChanged?.GetValue(source) as PropertyChangedEventHandler;
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
        /// </para><para>
        /// 当前对象最好应该直接实现 <see cref="INotifyPropertyChanged"/> 接口，
        /// 便于搜寻 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的委托。
        /// 若当前对象的类型显式实现 <see cref="INotifyPropertyChanged"/> 接口，或接口实现在基类中，
        /// 则会循环向下搜索第一个类型为 <see cref="PropertyChangedEventHandler"/> 的实例字段，将其作为事件委托。
        /// </para></remarks>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="source">一个 <see cref="INotifyPropertyChanged"/> 接口的对象。</param>
        /// <param name="item">属性对应的字段。</param>
        /// <param name="value">属性的新值，一般为 <see langword="value"/> 关键字。</param>
        /// <param name="propertyName">属性的名称，由编译器自动获取。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static void SetProperty<T>(this INotifyPropertyChanged source, ref T item, T value,
            [CallerMemberName] string propertyName = null)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }

            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                source.OnPropertyChanged(propertyName);
            }
        }
    }
}
