using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using XstarS.Reflection;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供属性更改通知类型 <see cref="INotifyPropertyChanged"/> 的包装抽象基类。
    /// </summary>
    /// <typeparam name="TData">包装的数据对象的类型。</typeparam>
    [Serializable]
    public abstract class ObservableDataWrapper<TData> : ObservableDataObject
        where TData : class
    {
        /// <summary>
        /// 表示当前包装的数据对象。
        /// </summary>
        private readonly TData DataObject;

        /// <summary>
        /// 初始化 <see cref="ObservableDataWrapper{TData}"/> 类的新实例，
        /// 并使用默认构造函数初始化包装对象的值。
        /// </summary>
        protected ObservableDataWrapper()
        {
            this.DataObject = Activator.CreateInstance<TData>();
        }

        /// <summary>
        /// 初始化 <see cref="ObservableDataWrapper{TData}"/> 类的新实例，
        /// 并将包装对象的值初始化为指定的值。
        /// </summary>
        /// <param name="dataObject">要包装的数据对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dataObject"/> 为 <see langword="null"/>。</exception>
        protected ObservableDataWrapper(TData dataObject)
        {
            this.DataObject = dataObject ??
                throw new ArgumentNullException(nameof(dataObject));
        }

        /// <summary>
        /// 获取当前包装的数据对象。
        /// </summary>
        /// <returns>当前包装的数据对象。</returns>
        public TData GetWrappedObject() => this.DataObject;

        /// <summary>
        /// 获取包装对象的指定属性或实体的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">要获取值的属性的名称；
        /// 如果要获取当前实体的值，则为 <see langword="null"/> 或空字符串。</param>
        /// <returns>包装对象中名为 <paramref name="propertyName"/> 的属性或实体的值。</returns>
        /// <exception cref="InvalidCastException">
        /// 指定属性或实体的值无法转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="MissingMemberException">
        /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="get"/> 属性。</exception>
        [return: MaybeNull]
        protected override T GetProperty<T>([CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { return (T)(object)this.DataObject; }
            return SimplePropertyAccessor<TData>.GetValue<T>(this.DataObject, propertyName);
        }

        /// <summary>
        /// 设置包装对象的指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="value">属性的新值。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="propertyName"/> 为 <see langword="null"/> 或空字符串。</exception>
        /// <exception cref="MissingMemberException">
        /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="set"/> 属性。</exception>
        protected override void SetProperty<T>(
            [AllowNull] T value, [CallerMemberName] string? propertyName = null)
        {
            if (this.IsEntityName(propertyName)) { throw new InvalidOperationException(); }
            SimplePropertyAccessor<TData>.SetValue(this.DataObject, propertyName, value);
        }
    }
}
