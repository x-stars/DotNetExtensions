using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供特殊类型相关的扩展方法。
    /// </summary>
    public static class SpecialTypeExtensions
    {
        /// <summary>
        /// 与可变参数列表方法相关的类型的 <see cref="HashSet{T}"/>。
        /// </summary>
        private static readonly HashSet<Type> VarArgTypes = new HashSet<Type>(
            new[] { "System.ArgIterator", "System.RuntimeArgumentHandle",
                "System.TypedReference" }.Select(Type.GetType).OfType<Type>());

        /// <summary>
        /// 确定类型是否是类引用传递结构类型的方法的委托。
        /// </summary>
        private static readonly Func<Type, bool> IsByRefLikeDelegate =
            (typeof(Type).GetProperty("IsByRefLike")?.GetMethod?.CreateDelegate(
                typeof(Func<Type, bool>)) as Func<Type, bool>) ?? (type => false);

        /// <summary>
        /// 确定当前 <see cref="Type"/> 是否是与可变参数列表方法相关的类型。
        /// </summary>
        /// <param name="type">要确定是否相关的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 是与可变参数列表方法相关的类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static bool IsVarArgType(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return SpecialTypeExtensions.VarArgTypes.Contains(type);
        }

        /// <summary>
        /// 确定当前 <see cref="Type"/> 的实例是否仅能分配于计算堆栈上。
        /// </summary>
        /// <param name="type">要确定是否仅能分配于堆栈的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 的实例仅能分配于计算堆栈上，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static bool IsStackOnly(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsByRef || type.IsVarArgType() ||
                SpecialTypeExtensions.IsByRefLikeDelegate.Invoke(type);
        }

        /// <summary>
        /// 确定当前 <see cref="Type"/> 的实例是否不能转换为 <see cref="object"/>。
        /// </summary>
        /// <param name="type">要确定是否不能转换的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 的实例不能转换为 <see cref="object"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static bool IsNotBoxable(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsPointer || type.IsStackOnly();
        }
    }
}
