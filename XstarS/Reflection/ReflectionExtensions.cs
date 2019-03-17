using System;
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
        /// 检索当前类型可以访问的所有事件的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有事件的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<EventInfo> GetAccessibleEvents(this Type source)
        {
            var result = new List<EventInfo>(source.GetRuntimeEvents());
            if (source.IsInterface)
            {
                foreach (var type in source.GetInterfaces())
                {
                    if (type.IsPublic)
                    {
                        result.AddRange(type.GetRuntimeEvents());
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 检索当前类型可以访问的所有字段的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有字段的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<FieldInfo> GetAccessibleFields(this Type source)
        {
            return source.GetRuntimeFields();
        }

        /// <summary>
        /// 检索当前类型可以访问的所有方法的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有方法的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<MethodInfo> GetAccessibleMethods(this Type source)
        {
            var result = new List<MethodInfo>(source.GetRuntimeMethods());
            if (source.IsInterface)
            {
                foreach (var type in source.GetInterfaces())
                {
                    if (type.IsPublic)
                    {
                        result.AddRange(type.GetRuntimeMethods());
                    }
                }
            }
            return result.ToArray();
        }

        /// <summary>
        /// 检索当前类型可以访问的所有属性的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有属性的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static IEnumerable<PropertyInfo> GetAccessibleProperties(this Type source)
        {
            var result = new List<PropertyInfo>(source.GetRuntimeProperties());
            if (source.IsInterface)
            {
                foreach (var type in source.GetInterfaces())
                {
                    if (type.IsPublic)
                    {
                        result.AddRange(type.GetRuntimeProperties());
                    }
                }
            }
            return result.ToArray();
        }
    }
}
