using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XstarS
{
    /// <summary>
    /// 提供类型声明 <see cref="Type"/> 的扩展方法。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// <see cref="TypeExtensions.LiteralTypes"/> 的值。
        /// </summary>
        private static readonly ICollection<Type> LiteralTypesStorage;

        /// <summary>
        /// <see cref="TypeExtensions.NativeTypes"/> 的值。
        /// </summary>
        private static readonly ICollection<Type> NativeTypesStorage;

        /// <summary>
        /// 初始化 <see cref="TypeExtensions"/> 类的静态成员。
        /// </summary>
        static TypeExtensions()
        {
            TypeExtensions.LiteralTypesStorage = new HashSet<Type>()
            {
                typeof(Boolean), typeof(Byte), typeof(Char), typeof(Double), typeof(Int16),
                typeof(Int32), typeof(Int64), typeof(SByte), typeof(Single), typeof(String),
                typeof(UInt32), typeof(UInt16), typeof(UInt64)
            };
            TypeExtensions.NativeTypesStorage = new HashSet<Type>()
            {
                typeof(Boolean), typeof(Byte), typeof(Char), typeof(Double), typeof(Int16),
                typeof(Int32), typeof(Int64), typeof(IntPtr), typeof(Object), typeof(SByte),
                typeof(Single), typeof(String), typeof(UInt16), typeof(UInt32), typeof(UInt64),
                typeof(UIntPtr)
            };
        }

        /// <summary>
        /// 可以表示为字面常量的类型的集合。
        /// </summary>
        public static ICollection<Type> LiteralTypes =>
            new HashSet<Type>(TypeExtensions.LiteralTypesStorage);

        /// <summary>
        /// 所有 .NET 原生类型的集合。
        /// </summary>
        public static ICollection<Type> NativeTypes =>
            new HashSet<Type>(TypeExtensions.NativeTypesStorage);

        /// <summary>
        /// 指示当前 <see cref="Type"/> 对象表示的类型是否为可以表示为字面常量的类型。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <returns>若 <paramref name="source"/> 表示的类型为可以表示为字面常量的类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsLiteral(this Type source) =>
            TypeExtensions.LiteralTypesStorage.Contains(source);

        /// <summary>
        /// 指示当前 <see cref="Type"/> 对象表示的类型是否为 .NET 原生类型。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <returns>若 <paramref name="source"/> 表示的类型为 .NET 原生类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsNative(this Type source) =>
            TypeExtensions.NativeTypesStorage.Contains(source);
    }
}
