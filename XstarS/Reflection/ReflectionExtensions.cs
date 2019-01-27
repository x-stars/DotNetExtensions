using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XstarS.Collections.Generic;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供反射相关的扩展方法。
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// 使用指定绑定约束，搜索当前类型能够直接访问的所有字段。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的匹配绑定约束 <paramref name="bindingAttr"/>
        /// 的所有字段的 <see cref="FieldInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static ICollection<FieldInfo> GetAccessibleFields(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<FieldInfo>(new ToStringComparer<FieldInfo>());
            result.AddRange(source.GetFields(bindingAttr));
            for (var t_base = source.BaseType; !(t_base is null); t_base = t_base.BaseType)
            {
                if (t_base.IsPublic || (t_base.Assembly == source.Assembly))
                {
                    var base_Members =
                        from base_Member in t_base.GetFields(bindingAttr)
                        where base_Member.IsPublic || base_Member.IsFamily
                        || ((t_base.Assembly == source.Assembly)
                        && (base_Member.IsAssembly || base_Member.IsFamilyAndAssembly))
                        select base_Member;
                    result.AddRange(base_Members);
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型能够直接访问的字段。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的字段的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有字段的 <see cref="FieldInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的字段。</exception>
        public static FieldInfo GetAccessibleField(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetAccessibleFields(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }

        /// <summary>
        /// 使用指定绑定约束，搜索当前类型能够直接访问的所有属性。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的匹配绑定约束 <paramref name="bindingAttr"/>
        /// 的所有属性的 <see cref="PropertyInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static ICollection<PropertyInfo> GetAccessibleProperties(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<PropertyInfo>(new ToStringComparer<PropertyInfo>());
            result.AddRange(source.GetProperties(bindingAttr));
            if (source.IsInterface)
            {
                foreach (var t_interface in source.GetInterfaces())
                {
                    if (t_interface.IsPublic || (t_interface.Assembly == source.Assembly))
                    {
                        result.AddRange(t_interface.GetAccessibleProperties(bindingAttr));
                    }
                }
            }
            else
            {
                for (var t_base = source.BaseType; !(t_base is null); t_base = t_base.BaseType)
                {
                    if (t_base.IsPublic || (t_base.Assembly == source.Assembly))
                    {
                        var base_Members =
                            from base_Member in t_base.GetProperties(bindingAttr)
                            where (base_Member.CanRead
                            && (base_Member.GetMethod.IsPublic || base_Member.GetMethod.IsFamily)
                            || ((t_base.Assembly == source.Assembly)
                            && (base_Member.GetMethod.IsAssembly || base_Member.GetMethod.IsFamilyAndAssembly)))
                            || (base_Member.CanWrite
                            && (base_Member.SetMethod.IsPublic || base_Member.SetMethod.IsFamily)
                            || ((t_base.Assembly == source.Assembly)
                            && (base_Member.SetMethod.IsAssembly || base_Member.SetMethod.IsFamilyAndAssembly)))
                            select base_Member;
                        result.AddRange(base_Members);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型能够直接访问的属性。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的属性的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有属性的 <see cref="PropertyInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的属性。</exception>
        public static PropertyInfo GetAccessibleProperty(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetAccessibleProperties(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }

        /// <summary>
        /// 使用指定绑定约束，搜索当前类型能够直接访问的所有事件。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的匹配绑定约束 <paramref name="bindingAttr"/>
        /// 的所有事件的 <see cref="EventInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static ICollection<EventInfo> GetAccessibleEvents(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<EventInfo>(new ToStringComparer<EventInfo>());
            result.AddRange(source.GetEvents(bindingAttr));
            if (source.IsInterface)
            {
                foreach (var t_interface in source.GetInterfaces())
                {
                    if (t_interface.IsPublic || (t_interface.Assembly == source.Assembly))
                    {
                        result.AddRange(t_interface.GetAccessibleEvents(bindingAttr));
                    }
                }
            }
            else
            {
                for (var t_base = source.BaseType; !(t_base is null); t_base = t_base.BaseType)
                {
                    if (t_base.IsPublic || (t_base.Assembly == source.Assembly))
                    {
                        var base_Members =
                            from base_Member in t_base.GetEvents(bindingAttr)
                            where base_Member.AddMethod.IsPublic || base_Member.AddMethod.IsFamily
                            || ((t_base.Assembly == source.Assembly)
                            && (base_Member.AddMethod.IsAssembly || base_Member.AddMethod.IsFamilyAndAssembly))
                            select base_Member;
                        result.AddRange(base_Members);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型能够直接访问的事件。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的事件的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有事件的 <see cref="EventInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的事件。</exception>
        public static EventInfo GetAccessibleEvent(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetAccessibleEvents(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }

        /// <summary>
        /// 使用指定绑定约束，搜索当前类型能够直接访问的所有方法。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的匹配绑定约束 <paramref name="bindingAttr"/>
        /// 的所有方法的 <see cref="MethodInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        public static ICollection<MethodInfo> GetAccessibleMethods(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<MethodInfo>(new ToStringComparer<MethodInfo>());
            result.AddRange(source.GetMethods(bindingAttr));
            if (source.IsInterface)
            {
                foreach (var t_interface in source.GetInterfaces())
                {
                    if (t_interface.IsPublic || (t_interface.Assembly == source.Assembly))
                    {
                        result.AddRange(t_interface.GetAccessibleMethods(bindingAttr));
                    }
                }
            }
            else
            {
                for (var t_base = source.BaseType; !(t_base is null); t_base = t_base.BaseType)
                {
                    if (t_base.IsPublic || (t_base.Assembly == source.Assembly))
                    {
                        var base_Members =
                            from base_Member in t_base.GetMethods(bindingAttr)
                            where base_Member.IsPublic || base_Member.IsFamily
                            || ((t_base.Assembly == source.Assembly)
                            && (base_Member.IsAssembly || base_Member.IsFamilyAndAssembly))
                            select base_Member;
                        result.AddRange(base_Members);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型能够直接访问的方法。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的方法的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有方法的 <see cref="MethodInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的方法。</exception>
        public static MethodInfo GetAccessibleMethod(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetAccessibleMethods(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }
    }
}
