using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供反射相关的扩展方法。
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 用于存储指定类型对象的默认值。
        /// </summary>
        /// <typeparam name="T">默认值的类型。</typeparam>
        private static class Default<T>
        {
            /// <summary>
            /// <typeparamref name="T"/> 类型的默认值。
            /// </summary>
#pragma warning disable CS0649
            public static readonly T Value;
#pragma warning restore CS0649
        }

        /// <summary>
        /// <see cref="ReflectionExtensions.DefaultValue(Type)"/> 的延迟初始化值。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Lazy<object>>
            LazyDefaultValues = new ConcurrentDictionary<Type, Lazy<object>>();

        /// <summary>
        /// 获取当前类型的默认值，即通过默认值表达式 <see langword="default"/> 获得的值。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 类型对应的默认值。</returns>
        public static object DefaultValue(this Type source) =>
            ReflectionExtensions.LazyDefaultValues.GetOrAdd(source,
                newType => new Lazy<object>(
                    () => typeof(Default<>).MakeGenericType(newType).GetField(
                        nameof(Default<object>.Value)).GetValue(null))).Value;

        /// <summary>
        /// 检索当前类型可以访问的所有事件的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有事件的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<EventInfo> GetAccessibleEvents(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeEvents);

        /// <summary>
        /// 检索当前类型可以访问的所有字段的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有字段的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<FieldInfo> GetAccessibleFields(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeFields);

        /// <summary>
        /// 检索当前类型可以访问的所有方法的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有方法的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<MethodInfo> GetAccessibleMethods(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeMethods);

        /// <summary>
        /// 检索当前类型可以访问的所有属性的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有属性的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<PropertyInfo> GetAccessibleProperties(this Type source) =>
            source.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeProperties);

        /// <summary>
        /// 检索当前类型可以访问的所有指定类型的成员的集合。
        /// </summary>
        /// <typeparam name="TMemberInfo">要检索的成员的类型。</typeparam>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <param name="memberFinder">检索指定类型的成员的 <see cref="Func{T, TResult}"/> 委托。</param>
        /// <returns><paramref name="source"/> 可以访问的所有指定类型的成员的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        private static IEnumerable<TMemberInfo> GetAccessibleMembers<TMemberInfo>(
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
    }
}
