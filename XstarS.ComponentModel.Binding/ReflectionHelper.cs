using System;
using System.Collections.Generic;
using System.Reflection;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供反射相关的帮助方法。
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// 确定当前方法或构造函数是否为程序集外部可继承的实例方法。
        /// </summary>
        /// <param name="source">一个 <see cref="MethodInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 是一个程序集外部可继承的实例方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static bool IsInheritableInstance(this MethodBase source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return !source.IsStatic &&
                (source.IsPublic || source.IsFamily || source.IsFamilyOrAssembly);
        }

        /// <summary>
        /// 确定当前方法是否可以在程序集外部重写。
        /// </summary>
        /// <param name="source">一个 <see cref="MethodInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 可以在程序集外部重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static bool IsOverridable(this MethodInfo source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.IsInheritableInstance() && (source.IsVirtual && !source.IsFinal);
        }

        /// <summary>
        /// 检索当前类型可以访问的所有指定类型的成员的集合。
        /// </summary>
        /// <typeparam name="TMemberInfo">要检索的成员的类型。</typeparam>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <param name="memberFinder">检索指定类型的成员的 <see cref="Func{T, TResult}"/> 委托。</param>
        /// <returns><paramref name="source"/> 可以访问的所有指定类型的成员的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<TMemberInfo> GetAccessibleMembers<TMemberInfo>(
            this Type source, Func<Type, IEnumerable<TMemberInfo>> memberFinder)
            where TMemberInfo : MemberInfo
        {
            var result = new List<TMemberInfo>(memberFinder(source));
            if (source.IsInterface)
            {
                foreach (var type in source.GetInterfaces())
                {
                    if (type.IsVisible)
                    {
                        result.AddRange(memberFinder(type));
                    }
                }
                result.AddRange(memberFinder(typeof(object)));
            }
            return result.ToArray();
        }

        /// <summary>
        /// 检索当前类型可以访问的所有事件的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有事件的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<EventInfo> GetAccessibleEvents(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeEvents);

        /// <summary>
        /// 检索当前类型可以访问的所有字段的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有字段的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<FieldInfo> GetAccessibleFields(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeFields);

        /// <summary>
        /// 检索当前类型可以访问的所有方法的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有方法的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<MethodInfo> GetAccessibleMethods(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeMethods);

        /// <summary>
        /// 检索当前类型可以访问的所有属性的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有属性的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<PropertyInfo> GetAccessibleProperties(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeProperties);
    }
}
