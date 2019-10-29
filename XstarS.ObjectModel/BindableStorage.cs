using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供可绑定类型 <see cref="INotifyPropertyChanged"/> 基于数据结构的抽象基类。
    /// </summary>
    [Serializable]
    public abstract class BindableStorage : BindableBase
    {
        /// <summary>
        /// 所有属性的数据存储。
        /// </summary>
        private readonly ConcurrentDictionary<string, object> PropertyStorage;

        /// <summary>
        /// 初始化 <see cref="BindableStorage"/> 类的新实例。
        /// </summary>
        protected BindableStorage()
        {
            this.PropertyStorage = new ConcurrentDictionary<string, object>();
        }

        /// <summary>
        /// 获取或设置指定属性的值。
        /// </summary>
        /// <param name="propertyName">属性的名称。</param>
        /// <returns>指定属性的值；若不存在，则为 <see langword="null"/>。</returns>
        protected object this[string propertyName]
        {
            get
            {
                return this.PropertyStorage.ContainsKey(propertyName) ?
                    this.PropertyStorage[propertyName] : null;
            }

            set
            {
                if (!object.Equals(this[propertyName], value))
                {
                    this.PropertyStorage[propertyName] = value;
                    this.OnPropertyChanged(propertyName);
                }
            }
        }

        /// <summary>
        /// 获取指定属性的值；若不存在或类型不匹配，则返回 <see langword="default"/>。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="propertyName">属性的名称，可由编译器自动获取。</param>
        /// <returns>指定属性的值；若不存在或类型不匹配，则为 <see langword="default"/>。</returns>
        protected T GetProperty<T>(
            [CallerMemberName] string propertyName = null)
        {
            return (this[propertyName] is T value) ? value : default(T);
        }

        /// <summary>
        /// 设置指定属性的值，并引发 <see cref="BindableBase.PropertyChanged"/> 事件。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="value">属性的新值，一般为 <see langword="value"/>。</param>
        /// <param name="propertyName">属性的名称，可由编译器自动获取。</param>
        protected void SetProperty<T>(T value,
            [CallerMemberName] string propertyName = null)
        {
            this[propertyName] = (object)value;
        }
    }
}
