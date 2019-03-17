using System;
using System.Collections.Generic;
using System.Reflection;

namespace XstarS
{
    /// <summary>
    /// 提供类型声明 <see cref="Type"/> 的扩展方法。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 可以表示为非 <see langword="null"/> 字面常量的类型的集合，不包括枚举类型。
        /// </summary>
        private static readonly ICollection<Type> InternalLiteralTypes = new HashSet<Type>()
        {
            typeof(Boolean), typeof(Byte), typeof(Char), typeof(Double), typeof(Int16),
            typeof(Int32), typeof(Int64), typeof(SByte), typeof(Single), typeof(String),
            typeof(UInt32), typeof(UInt16), typeof(UInt64)
        };

        /// <summary>
        /// .NET 原生类型的集合。在 IL 汇编中，这些类型被表示为关键字，是其它所有类型的基础。
        /// </summary>
        private static readonly ICollection<Type> InternalNativeTypes = new HashSet<Type>()
        {
            typeof(Boolean), typeof(Byte), typeof(Char), typeof(Double), typeof(Int16),
            typeof(Int32), typeof(Int64), typeof(IntPtr), typeof(Object), typeof(SByte),
            typeof(Single), typeof(String), typeof(UInt16), typeof(UInt32), typeof(UInt64),
            typeof(UIntPtr), typeof(void)
        };

        /// <summary>
        /// 可以表示为非 <see langword="null"/> 字面常量的类型的集合，不包括枚举类型。
        /// </summary>
        public static ICollection<Type> LiteralTypes =>
            new HashSet<Type>(TypeExtensions.InternalNativeTypes);

        /// <summary>
        /// .NET 原生类型的集合。在 IL 汇编中，这些类型被表示为关键字，是其它所有类型的基础。
        /// </summary>
        public static ICollection<Type> NativeTypes =>
            new HashSet<Type>(TypeExtensions.InternalNativeTypes);

        /// <summary>
        /// 指示当前 <see cref="Type"/> 对象表示的类型是否为可以表示为字面常量的类型。
        /// 空引用 <see langword="null"/> 可以作为所有引用类型的字面常量，此处不算在内。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <returns>若 <paramref name="source"/> 表示的类型为可以表示为字面常量的类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static bool IsLiteral(this Type source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return TypeExtensions.InternalLiteralTypes.Contains(source) || source.IsEnum;
        }

        /// <summary>
        /// 指示当前 <see cref="Type"/> 对象表示的类型是否为 .NET 原生类型。
        /// 原生类型在 IL 汇编中被表示为关键字，是其它所有类型的基础。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <returns>若 <paramref name="source"/> 表示的类型为 .NET 原生类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static bool IsNative(this Type source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return TypeExtensions.InternalNativeTypes.Contains(source);
        }
    }
}
