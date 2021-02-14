using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供枚举的视图的抽象基类。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    public abstract class EnumViewBase<TEnum> : ObservableDataObject
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        protected EnumViewBase() { }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值。
        /// </summary>
        /// <returns>当前视图表示的枚举值。</returns>
        public TEnum Value
        {
            get => this.GetProperty<TEnum>();
            set => this.SetEnumValue(value);
        }

        /// <summary>
        /// 设置当前视图表示的枚举值。
        /// </summary>
        /// <param name="enumValue">要设置的枚举值。</param>
        protected virtual void SetEnumValue(TEnum enumValue)
        {
            var thisValue = this.Value;
            this.SetProperty(enumValue, nameof(this.Value));
            var valueChanged = !object.Equals(thisValue, enumValue);
            if (valueChanged) { this.NotifyEnumPropertiesChanged(); }
        }

        /// <summary>
        /// 通知所有枚举值对应的属性的值已更改。
        /// </summary>
        protected void NotifyEnumPropertiesChanged()
        {
            var enumNames = Enum.GetNames(typeof(TEnum));
            Array.ForEach(enumNames, this.NotifyPropertyChanged);
            this.NotifyPropertyChanged(ObservableDataObject.IndexerName);
        }

        /// <summary>
        /// 将当前视图表示的枚举值转换为其等效的字符串表示形式。
        /// </summary>
        /// <returns>当前视图表示的枚举值的字符串表示形式。</returns>
        public override string ToString() => this.Value.ToString();
    }
}
