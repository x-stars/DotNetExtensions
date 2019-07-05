using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示一个任意类型的用于数据绑定的值。
    /// </summary>
    public interface IBindingValue : IDisposable
    {
        /// <summary>
        /// 用于数据绑定的值。
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// 在用于数据绑定的值更改时发生。
        /// </summary>
        event EventHandler ValueChanged;
    }

    /// <summary>
    /// 表示一个用于数据绑定的值。
    /// </summary>
    /// <typeparam name="T">用于数据绑定的值的类型。</typeparam>
    public interface IBindingValue<T> : IDisposable
    {
        /// <summary>
        /// 用于数据绑定的值。
        /// </summary>
        T Value { get; set; }

        /// <summary>
        /// 在用于数据绑定的值更改时发生。
        /// </summary>
        event EventHandler ValueChanged;
    }
}
