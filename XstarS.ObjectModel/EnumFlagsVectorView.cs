using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 为位域枚举的布尔值向量视图提供抽象基类。
    /// </summary>
    /// <typeparam name="TEnum">位域枚举的类型。</typeparam>
    [Serializable]
    public abstract class EnumFlagsVectorView<TEnum> : EnumVectorView<TEnum>
        where TEnum : struct, Enum
    {
        /// <summary>
        /// 初始化 <see cref="EnumFlagsVectorView{TEnum}"/> 类的新实例。
        /// </summary>
        protected EnumFlagsVectorView() { }

        /// <summary>
        /// 获取当前视图表示的枚举值是否包含指定的枚举位域。
        /// </summary>
        /// <param name="flagName">要确定的枚举位域的名称。</param>
        /// <returns>当前视图表示的枚举值是否包含指定的枚举位域值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="flagName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="flagName"/> 不为有效的枚举值名称。</exception>
        protected bool HasFlag(
            [CallerMemberName] string flagName = null)
        {
            return this.Value.HasFlag((TEnum)Enum.Parse(typeof(TEnum), flagName));
        }

        /// <summary>
        /// 根据指示设置当前视图表示的枚举值的指定的枚举位域。
        /// </summary>
        /// <param name="flagName">要设置的枚举位域的名称。</param>
        /// <param name="value">指示是添加位域还是移除位域。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="flagName"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="flagName"/> 不为有效的枚举值名称。</exception>
        protected void SetFlag(bool value,
            [CallerMemberName] string flagName = null)
        {
            var enumNames = new HashSet<string>(this.Value.ToString().Split(
                new[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
            enumNames.IntersectWith(Enum.GetNames(typeof(TEnum)));
            var flagValue = (TEnum)Enum.Parse(typeof(TEnum), flagName);
            var flagNames = new HashSet<string>(flagValue.ToString().Split(
                new[] { ", " }, StringSplitOptions.RemoveEmptyEntries));
            if (value) { enumNames.UnionWith(flagNames); }
            else { enumNames.ExceptWith(flagNames); }
            var enumName = string.Join(", ", enumNames);
            this.Value = (TEnum)Enum.Parse(typeof(TEnum), enumName);
            new List<string>(flagNames).ForEach(this.NotifyPropertyChanged);
        }
    }
}
