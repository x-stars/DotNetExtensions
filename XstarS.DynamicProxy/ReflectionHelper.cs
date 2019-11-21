using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供反射相关的帮助方法。
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// 与 vararg 方法相关的类型的 <see cref="HashSet{T}"/>。
        /// </summary>
        private static readonly HashSet<Type> VarArgTypes = new HashSet<Type>(
            new[] { "System.ArgIterator", "System.RuntimeArgumentHandle",
                "System.TypedReference" }.Select(Type.GetType).OfType<Type>());

        /// <summary>
        /// 确定类型是否是 byref-like 结构类型的方法的委托。
        /// </summary>
        private static readonly Func<Type, bool> IsByRefLikeDelegate =
            (typeof(Type).GetProperty("IsByRefLike")?.GetMethod?.CreateDelegate(
                typeof(Func<Type, bool>)) as Func<Type, bool>) ?? (type => false);

        /// <summary>
        /// 确定当前 <see cref="Type"/> 是否是与 vararg 方法相关的类型。
        /// </summary>
        /// <param name="type">要确定是否与 vararg 方法相关的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 是与 vararg 方法相关的类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        private static bool IsVarArgType(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return ReflectionHelper.VarArgTypes.Contains(type);
        }

        /// <summary>
        /// 确定当前 <see cref="Type"/> 是否是 byref-like 结构类型。
        /// </summary>
        /// <param name="type">要确定是否是 byref-like 结构的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 是 byref-like 结构类型，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        private static bool IsByRefLike(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return ReflectionHelper.IsByRefLikeDelegate.Invoke(type);
        }

        /// <summary>
        /// 确定当前 <see cref="Type"/> 的实例是否仅能分配于计算堆栈上。
        /// </summary>
        /// <param name="type">要确定是否仅能分配于堆栈的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 的实例仅能分配于计算堆栈上，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        internal static bool IsStackOnly(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsByRef || type.IsByRefLike() || type.IsVarArgType();
        }

        /// <summary>
        /// 确定当前 <see cref="Type"/> 的实例是否不能进行装箱操作。
        /// </summary>
        /// <param name="type">要确定是否不能装箱的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 的实例不能进行装箱操作，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        internal static bool IsNotBoxable(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsPointer || type.IsStackOnly();
        }

        /// <summary>
        /// 确定当前 <see cref="MethodBase"/> 是否为程序集外部可继承的实例方法。
        /// </summary>
        /// <param name="method">要进行检查的 <see cref="MethodBase"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 是程序集外部可继承的实例方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        internal static bool IsInheritableInstance(this MethodBase method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return !method.IsStatic &&
                (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly);
        }

        /// <summary>
        /// 确定当前 <see cref="MethodInfo"/> 是否为程序集外部可重写的方法。
        /// </summary>
        /// <param name="method">要进行检查的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 是程序集外部可重写的方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        internal static bool IsOverridable(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.IsInheritableInstance() && (method.IsVirtual && !method.IsFinal);
        }

        /// <summary>
        /// 确定当前 <see cref="MemberInfo"/> 是否可被 <see cref="MethodInvokeHandler"/> 代理。
        /// </summary>
        /// <param name="method">要确定是否可被代理的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 可被 <see cref="MethodInvokeHandler"/> 代理，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        internal static bool IsProxySupported(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.IsOverridable() &&
                Array.TrueForAll(
                    Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                    type => !type.IsNotBoxable()) &&
                (!method.IsGenericMethod || (method.IsGenericMethod &&
                Array.TrueForAll(
                    method.GetGenericArguments(),
                    type => type.GetGenericParameterConstraints().Length == 0)));
        }

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

            var result = new List<TMemberInfo>(memberFinder(type));
            if (type.IsInterface)
            {
                foreach (var iType in type.GetInterfaces())
                {
                    if (iType.IsVisible)
                    {
                        result.AddRange(memberFinder(iType));
                    }
                }
                result.AddRange(memberFinder(typeof(object)));
            }
            return result.ToArray();
        }

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有事件的集合。
        /// </summary>
        /// <param name="type">要检索事件的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有事件的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<EventInfo> GetAccessibleEvents(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeEvents);

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有字段的集合。
        /// </summary>
        /// <param name="type">要检索字段的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有字段的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<FieldInfo> GetAccessibleFields(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeFields);

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有方法的集合。
        /// </summary>
        /// <param name="type">要检索方法的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有方法的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<MethodInfo> GetAccessibleMethods(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeMethods);

        /// <summary>
        /// 检索当前 <see cref="Type"/> 可以访问的所有属性的集合。
        /// </summary>
        /// <param name="type">要检索属性的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 可以访问的所有属性的集合。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        internal static IEnumerable<PropertyInfo> GetAccessibleProperties(this Type type) =>
            type.GetAccessibleMembers(RuntimeReflectionExtensions.GetRuntimeProperties);
    }
}
