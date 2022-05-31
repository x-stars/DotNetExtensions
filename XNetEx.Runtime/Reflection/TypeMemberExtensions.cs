using System;
using System.Collections.Generic;
using System.Reflection;

namespace XNetEx.Reflection
{
    /// <summary>
    /// 提供类型成员相关的扩展方法。
    /// </summary>
    public static class TypeMemberExtensions
    {
        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有事件的集合。
        /// </summary>
        /// <param name="type">要检索事件的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有事件的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<EventInfo> GetAccessibleEvents(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeEvents);

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有字段的集合。
        /// </summary>
        /// <param name="type">要检索字段的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有字段的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<FieldInfo> GetAccessibleFields(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeFields);

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有方法的集合。
        /// </summary>
        /// <param name="type">要检索方法的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有方法的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<MethodInfo> GetAccessibleMethods(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeMethods);

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有属性的集合。
        /// </summary>
        /// <param name="type">要检索属性的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有属性的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<PropertyInfo> GetAccessibleProperties(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeProperties);

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有指定类型的成员的集合。
        /// </summary>
        /// <typeparam name="TMemberInfo">要检索的成员的类型。</typeparam>
        /// <param name="type">要检索成员的 <see cref="Type"/> 对象。</param>
        /// <param name="memberFinder">检索指定类型的成员的 <see cref="Func{T, TResult}"/> 委托。</param>
        /// <returns><paramref name="type"/> 可以访问的所有指定类型的成员的集合。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/>
        /// 或 <paramref name="memberFinder"/> 为 <see langword="null"/>。</exception>
        private static IEnumerable<TMemberInfo> GetAccessibleMembers<TMemberInfo>(
            this Type type, Func<Type, IEnumerable<TMemberInfo>> memberFinder)
            where TMemberInfo : MemberInfo
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (memberFinder is null)
            {
                throw new ArgumentNullException(nameof(memberFinder));
            }

            var result = new List<TMemberInfo>(memberFinder.Invoke(type));
            if (type.IsInterface)
            {
                foreach (var iType in type.GetInterfaces())
                {
                    if (iType.IsVisible)
                    {
                        result.AddRange(memberFinder.Invoke(iType));
                    }
                }
                result.AddRange(memberFinder.Invoke(typeof(object)));
            }
            return result.ToArray();
        }
    }
}
