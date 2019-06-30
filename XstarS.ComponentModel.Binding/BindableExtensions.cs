using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供通过 <see cref="INotifyPropertyChanged"/> 接口实现的数据绑定的扩展方法。
    /// </summary>
    public static class BindableExtensions
    {
        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件委托字段的缓存。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, FieldInfo> PropertyChangedFields =
            new ConcurrentDictionary<Type, FieldInfo>();

        /// <summary>
        /// 搜寻当前类型的 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件委托的字段。
        /// </summary>
        /// <param name="bindableType">一个实现 <see cref="INotifyPropertyChanged"/> 接口的类型。</param>
        /// <returns><paramref name="bindableType"/> 及其所有基类型中第一个名称为
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 且类型为
        /// <see cref="ProgressChangedEventHandler"/> 的字段；若不存在，
        /// 则为第一个类型为 <see cref="ProgressChangedEventHandler"/> 的字段。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bindableType"/> 为 <see langword="null"/>。</exception>
        private static FieldInfo FindPropertyChangedField(Type bindableType)
        {
            if (bindableType is null)
            {
                throw new ArgumentNullException(nameof(bindableType));
            }

            var fieldsPropertyChanged = Enumerable.Empty<FieldInfo>();
            for (var type = bindableType; !(type is null); type = type.BaseType)
            {
                fieldsPropertyChanged = fieldsPropertyChanged.Concat(type.GetFields(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(
                    field => field.FieldType == typeof(PropertyChangedEventHandler)));
            }
            return fieldsPropertyChanged.FirstOrDefault(
                field => field.Name == nameof(INotifyPropertyChanged.PropertyChanged)) ??
                fieldsPropertyChanged.FirstOrDefault();
        }

        /// <summary>
        /// 获取当前对象的 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件委托。
        /// </summary>
        /// <param name="bindable">要获取事件委托的 <see cref="INotifyPropertyChanged"/> 对象。</param>
        /// <returns><paramref name="bindable"/> 的类型及其所有基类型中第一个名称为
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 且类型为
        /// <see cref="ProgressChangedEventHandler"/> 的字段的值；若不存在，
        /// 则为第一个类型为 <see cref="ProgressChangedEventHandler"/> 的字段的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bindable"/> 为 <see langword="null"/>。</exception>
        private static PropertyChangedEventHandler GetPropertyChangedDelegate(
            this INotifyPropertyChanged bindable)
        {
            if (bindable is null)
            {
                throw new ArgumentNullException(nameof(bindable));
            }

            var type = bindable.GetType();
            var fieldPropertyChanged =
                BindableExtensions.PropertyChangedFields.GetOrAdd(
                    type, BindableExtensions.FindPropertyChangedField);
            return fieldPropertyChanged?.GetValue(bindable) as PropertyChangedEventHandler;
        }

        /// <summary>
        /// 触发属性改变事件。基于反射调用，可能存在性能问题。
        /// </summary>
        /// <remarks><para>
        /// 将会向基类方向搜索类型为 <see cref="PropertyChangedEventHandler"/> 的实例字段，
        /// 并将第一个名为 <see cref="INotifyPropertyChanged.PropertyChanged"/> 的字段将其作为事件委托；
        /// 若不存在此名称的字段，则会将搜寻到的第一个字段作为事件委托。
        /// </para></remarks>
        /// <param name="bindable">要触发事件的 <see cref="INotifyPropertyChanged"/> 对象。</param>
        /// <param name="propertyName">已更改属性的名称。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bindable"/> 为 <see langword="null"/>。</exception>
        public static void OnPropertyChanged(this INotifyPropertyChanged bindable,
            [CallerMemberName] string propertyName = null)
        {
            if (bindable is null)
            {
                throw new ArgumentNullException(nameof(bindable));
            }

            bindable.GetPropertyChangedDelegate()?.Invoke(
                bindable, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 更改属性的值，并通知客户端属性发生更改。基于反射调用，可能存在性能问题。
        /// </summary>
        /// <remarks><para>
        /// 请在属性的 <see langword="set"/> 处调用此方法，
        /// 在更改属性值的同时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。
        /// </para><para>
        /// 当前对象最好应该直接实现 <see cref="INotifyPropertyChanged"/> 接口，
        /// 便于搜寻 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的委托。
        /// 若当前对象的类型显式实现 <see cref="INotifyPropertyChanged"/> 接口，或接口实现在基类中，
        /// 则会循环向下搜索第一个类型为 <see cref="PropertyChangedEventHandler"/> 的实例字段，将其作为事件委托。
        /// </para></remarks>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="bindable">要更改属性的 <see cref="INotifyPropertyChanged"/> 对象。</param>
        /// <param name="item">属性对应的字段。</param>
        /// <param name="value">属性的新值，一般为 <see langword="value"/> 关键字。</param>
        /// <param name="propertyName">属性的名称，由编译器自动获取。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bindable"/> 为 <see langword="null"/>。</exception>
        public static void SetProperty<T>(this INotifyPropertyChanged bindable,
            ref T item, T value, [CallerMemberName] string propertyName = null)
        {
            if (bindable is null)
            {
                throw new ArgumentNullException(nameof(bindable));
            }

            if (!EqualityComparer<T>.Default.Equals(item, value))
            {
                item = value;
                bindable.OnPropertyChanged(propertyName);
            }
        }
    }
}
