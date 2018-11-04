using System;
using System.Collections.Generic;
using System.Linq;

namespace XstarS
{
    /// <summary>
    /// 提供类型声明 <see cref="Type"/> 的扩展方法。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 可以表示为字面常量的类型的集合。
        /// </summary>
        private static readonly ICollection<Type> LiteralTypesField;

        /// <summary>
        /// 初始化 <see cref="TypeExtensions"/> 类的静态成员。
        /// </summary>
        static TypeExtensions()
        {
            TypeExtensions.LiteralTypesField = new HashSet<Type>()
            {
                typeof(bool), typeof(byte), typeof(char), typeof(decimal), typeof(double),
                typeof(float), typeof(int), typeof(long), typeof(sbyte), typeof(short),
                typeof(string), typeof(uint), typeof(ushort), typeof(ulong)
            };
        }

        /// <summary>
        /// 可以表示为字面常量的类型的集合。
        /// </summary>
        public static ICollection<Type> LiteralTypes =>
            new HashSet<Type>(TypeExtensions.LiteralTypesField);

        /// <summary>
        /// 指示当前 <see cref="Type"/> 对象表示的类型是否为可以表示为字面常量的类型。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <returns>若 <paramref name="source"/> 表示的类型为可以表示为字面常量的类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsLiteral(this Type source) =>
            TypeExtensions.LiteralTypesField.Contains(source);
    }
}
