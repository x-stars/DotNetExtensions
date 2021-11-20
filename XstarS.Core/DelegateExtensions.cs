﻿using System;
using System.Reflection;
using System.Runtime.Serialization;
using XstarS.Reflection;

namespace XstarS
{
    /// <summary>
    /// 提供委托 <see cref="Delegate"/> 的扩展方法。
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// 表示委托 <see cref="Delegate"/> 的所有实例字段。
        /// </summary>
        private static readonly FieldInfo[] DelegateFields =
            typeof(MulticastDelegate).GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// 确定当前委托是否是否能转换为另一类型的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要转换到的委托的类型。</typeparam>
        /// <param name="delegate">要确定是否能转换的委托。</param>
        /// <returns>若 <paramref name="delegate"/> 的参数列表和返回值与
        /// <typeparamref name="TDelegate"/> 的相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static bool CanConvertTo<TDelegate>(this Delegate @delegate)
            where TDelegate : Delegate
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            var comparer = MethodSignatureEqualityComparer.Default;
            var sourceInvoke = @delegate.GetType().GetMethod("Invoke");
            var targetInvoke = typeof(TDelegate).GetMethod("Invoke");
            return comparer.Equals(sourceInvoke, targetInvoke);
        }

        /// <summary>
        /// 将当前委托转换为另一种类型的委托。此方法并不会检查委托的适配性，使用
        /// <see cref="DelegateExtensions.CanConvertTo{TDelegate}(Delegate)"/>
        /// 方法检查当前委托是否能够转换为 <typeparamref name="TDelegate"/> 类型。
        /// </summary>
        /// <typeparam name="TDelegate">要转换到的委托类型。</typeparam>
        /// <param name="delegate">要转换为另一类型的委托。</param>
        /// <returns>转换为 <typeparamref name="TDelegate"/>
        /// 类型的 <paramref name="delegate"/> 委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static TDelegate ChangeType<TDelegate>(this Delegate @delegate)
            where TDelegate : Delegate
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            var result = FormatterServices.GetUninitializedObject(typeof(TDelegate));
            var fields = DelegateExtensions.DelegateFields;
            var data = FormatterServices.GetObjectData(@delegate, fields);
            FormatterServices.PopulateObjectMembers(result, fields, data);
            return (TDelegate)result;
        }
    }
}
