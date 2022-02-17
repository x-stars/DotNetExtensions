﻿using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供枚举的向量视图的基类。
    /// </summary>
    /// <typeparam name="TEnum">枚举的类型。</typeparam>
    [Serializable]
    [DebuggerDisplay("Value = {" + nameof(Value) + "}")]
    public class EnumVectorView<TEnum> : ObservableDataObject
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        protected EnumVectorView() { }

        /// <summary>
        /// 获取或设置当前视图表示的枚举值。
        /// </summary>
        /// <returns>当前视图表示的枚举值。</returns>
        public TEnum Value
        {
            get => this.GetProperty<TEnum>();
            set => this.SetProperty(value);
        }

        /// <summary>
        /// 获取或设置当前视图是否选中了指定的枚举值。
        /// </summary>
        /// <param name="enumValue">要获取或设置是否选中的枚举值。</param>
        /// <returns>若当前视图选中了 <paramref name="enumValue"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool this[TEnum enumValue]
        {
            get => this.IsSelected(enumValue);
            set => this.SelectEnum(enumValue, value);
        }

        /// <summary>
        /// 初始化 <see cref="EnumVectorView{TEnum}.Value"/> 属性的关联枚举名称属性。
        /// </summary>
        protected override void InitializeRelatedProperties()
        {
            base.InitializeRelatedProperties();
            var enumNames = Enum.GetNames(typeof(TEnum));
            var valueRelated = new string[enumNames.Length + 1];
            Array.Copy(enumNames, valueRelated, enumNames.Length);
            valueRelated[enumNames.Length] = ObservableDataObject.IndexerName;
            this.SetRelatedProperties(nameof(this.Value), valueRelated);
        }

        /// <summary>
        /// 获取当前视图是否选中了指定的枚举值。
        /// </summary>
        /// <param name="enumValue">要确定是否选中的枚举值。</param>
        /// <returns>若当前视图选中了 <paramref name="enumValue"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        protected virtual bool IsSelected(TEnum enumValue)
        {
            return this.Value.Equals(enumValue);
        }

        /// <summary>
        /// 根据指示在当前视图选中或取消选中指定的枚举值。
        /// </summary>
        /// <param name="enumValue">要选中或取消选中的枚举值。</param>
        /// <param name="select">若要在当前视图选中指定的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        protected virtual void SelectEnum(TEnum enumValue, bool select)
        {
            if (select) { this.Value = enumValue; }
        }

        /// <summary>
        /// 获取当前视图是否选中了具有指定名称的枚举值。
        /// </summary>
        /// <param name="enumName">要确定是否选中的枚举值的名称。</param>
        /// <returns>若当前视图选中了名为 <paramref name="enumName"/> 的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected bool IsSelected([CallerMemberName] string? enumName = null)
        {
            var enumValue = (TEnum)Enum.Parse(typeof(TEnum), enumName!);
            return this.IsSelected(enumValue);
        }

        /// <summary>
        /// 根据指示在当前视图选中或取消选中具有指定名称的枚举值。
        /// </summary>
        /// <param name="select">若要在当前视图选中指定名称的枚举值，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <param name="enumName">要选中或取消选中的枚举值的名称。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="enumName"/> 不为有效的枚举值名称。</exception>
        protected void SelectEnum(bool select, [CallerMemberName] string? enumName = null)
        {
            var enumValue = (TEnum)Enum.Parse(typeof(TEnum), enumName!);
            this.SelectEnum(enumValue, select);
        }
    }
}
