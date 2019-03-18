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
        /// 确定当前属性是否为抽象。
        /// </summary>
        /// <param name="source">一个 <see cref="PropertyInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 为抽象，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static bool IsAbstract(this PropertyInfo source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.CanRead ? source.GetMethod.IsAbstract : (
                source.CanWrite ? source.SetMethod.IsAbstract :
                throw new InvalidProgramException());
        }

        /// <summary>
        /// 确定当前属性是否可以被重写。
        /// </summary>
        /// <param name="source">一个 <see cref="PropertyInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 可以被重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static bool IsOverridable(this PropertyInfo source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return
                source.CanRead ? (source.GetMethod.IsVirtual && !source.GetMethod.IsFinal) : (
                source.CanWrite ? (source.SetMethod.IsVirtual && !source.SetMethod.IsFinal) :
                throw new InvalidProgramException());
        }

        /// <summary>
        /// 确定当前方法是否为外部可继承的实例方法。
        /// </summary>
        /// <param name="source">一个 <see cref="MethodInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 是一个外部可继承的实例方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static bool IsInheritableInstance(this MethodInfo source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return !source.IsStatic && (source.IsPublic || source.IsFamily);
        }

        /// <summary>
        /// 检索当前类型可以访问的所有事件的集合。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类的对象。</param>
        /// <returns><paramref name="source"/> 可以访问的所有事件的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<EventInfo> GetAccessibleEvents(this Type source)
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
        internal static IEnumerable<FieldInfo> GetAccessibleFields(this Type source)
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
        internal static IEnumerable<MethodInfo> GetAccessibleMethods(this Type source)
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
        internal static IEnumerable<PropertyInfo> GetAccessibleProperties(this Type source)
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
