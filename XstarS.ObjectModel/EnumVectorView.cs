using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供枚举的布尔值向量视图的抽象基类。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    public abstract class EnumVectorView<TEnum> : ObservableDataObject
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        protected EnumVectorView() { }

        /// <summary>
        /// 获取当前视图表示的枚举值。
        /// </summary>
        public TEnum Value
        {
            get => this.GetProperty<TEnum>();
            protected set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取当前视图表示的枚举值是否为指定的枚举值。
        /// </summary>
        /// <param name="enumName">要确定的枚举值的名称。</param>
        /// <returns>当前视图表示的枚举值是否为指定的枚举值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected bool IsEnum(
            [CallerMemberName] string enumName = null)
        {
            var enumValue = (TEnum)Enum.Parse(typeof(TEnum), enumName);
            return this.Value.ToString() == enumValue.ToString();
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值。
        /// </summary>
        /// <param name="value">指示是否设置枚举值。</param>
        /// <param name="enumName">要设置的枚举值的名称。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected void SetEnum(bool value,
            [CallerMemberName] string enumName = null)
        {
            if (value)
            {
                this.Value = (TEnum)Enum.Parse(typeof(TEnum), enumName);
                var enumNames = Enum.GetNames(typeof(TEnum));
                new List<string>(enumNames).ForEach(this.NotifyPropertyChanged);
            }
        }
    }
}
