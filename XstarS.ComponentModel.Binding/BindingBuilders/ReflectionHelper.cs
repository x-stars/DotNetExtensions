using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供反射相关的帮助方法。
    /// </summary>
    internal static class ReflectionHelper
    {
        /// <summary>
        /// 使用指定绑定约束，搜索当前类型的外部派生类型能够直接访问的所有字段。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有字段的 <see cref="FieldInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static ICollection<FieldInfo> GetDerivedAccessibleFields(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<FieldInfo>(new ToStringComparer<FieldInfo>());
            for (var t_base = source; !(t_base is null); t_base = t_base.BaseType)
            {
                if (t_base.IsPublic)
                {
                    var base_Members =
                        from base_Member in t_base.GetFields(bindingAttr)
                        where base_Member.IsPublic || base_Member.IsFamily
                        select base_Member;
                    result.AddRange(base_Members);
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型的外部派生类型能够直接访问的字段。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的字段的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有字段的 <see cref="FieldInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的字段。</exception>
        internal static FieldInfo GetDerivedAccessibleField(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetDerivedAccessibleFields(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }

        /// <summary>
        /// 使用指定绑定约束，搜索当前类型的外部派生类型能够直接访问的所有属性。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有属性的 <see cref="PropertyInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static ICollection<PropertyInfo> GetDerivedAccessibleProperties(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<PropertyInfo>(new ToStringComparer<PropertyInfo>());
            if (source.IsInterface)
            {
                result.AddRange(source.GetProperties(bindingAttr));
                foreach (var t_interface in source.GetInterfaces())
                {
                    if (t_interface.IsPublic)
                    {
                        result.AddRange(t_interface.GetDerivedAccessibleProperties(bindingAttr));
                    }
                }
            }
            else
            {
                for (var t_base = source; !(t_base is null); t_base = t_base.BaseType)
                {
                    if (t_base.IsPublic)
                    {
                        var base_Members =
                            from base_Member in t_base.GetProperties(bindingAttr)
                            where (base_Member.CanRead
                            && (base_Member.GetMethod.IsPublic || base_Member.GetMethod.IsFamily))
                            || (base_Member.CanWrite
                            && (base_Member.SetMethod.IsPublic || base_Member.SetMethod.IsFamily))
                            select base_Member;
                        result.AddRange(base_Members);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型的外部派生类型能够直接访问的属性。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的属性的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有属性的 <see cref="PropertyInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的属性。</exception>
        internal static PropertyInfo GetDerivedAccessibleProperty(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetDerivedAccessibleProperties(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }

        /// <summary>
        /// 使用指定绑定约束，搜索当前类型的外部派生类型能够直接访问的所有事件。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有事件的 <see cref="EventInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static ICollection<EventInfo> GetDerivedAccessibleEvents(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<EventInfo>(new ToStringComparer<EventInfo>());
            if (source.IsInterface)
            {
                result.AddRange(source.GetEvents(bindingAttr));
                foreach (var t_interface in source.GetInterfaces())
                {
                    if (t_interface.IsPublic)
                    {
                        result.AddRange(t_interface.GetDerivedAccessibleEvents(bindingAttr));
                    }
                }
            }
            else
            {
                for (var t_base = source; !(t_base is null); t_base = t_base.BaseType)
                {
                    if (t_base.IsPublic)
                    {
                        var base_Members =
                            from base_Member in t_base.GetEvents(bindingAttr)
                            where base_Member.AddMethod.IsPublic || base_Member.AddMethod.IsFamily
                            select base_Member;
                        result.AddRange(base_Members);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型的外部派生类型能够直接访问的事件。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的事件的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有事件的 <see cref="EventInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的事件。</exception>
        internal static EventInfo GetDerivedAccessibleEvent(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetDerivedAccessibleEvents(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }

        /// <summary>
        /// 使用指定绑定约束，搜索当前类型的外部派生类型能够直接访问的所有方法。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有方法的 <see cref="MethodInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static ICollection<MethodInfo> GetDerivedAccessibleMethods(this Type source,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            var result = new HashSet<MethodInfo>(new ToStringComparer<MethodInfo>());
            if (source.IsInterface)
            {
                result.AddRange(source.GetMethods(bindingAttr));
                foreach (var t_interface in source.GetInterfaces())
                {
                    if (t_interface.IsPublic)
                    {
                        result.AddRange(t_interface.GetDerivedAccessibleMethods(bindingAttr));
                    }
                }
            }
            else
            {
                for (var t_base = source; !(t_base is null); t_base = t_base.BaseType)
                {
                    if (t_base.IsPublic)
                    {
                        var base_Members =
                            from base_Member in t_base.GetMethods(bindingAttr)
                            where base_Member.IsPublic || base_Member.IsFamily
                            select base_Member;
                        result.AddRange(base_Members);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 使用指定名称和绑定约束，搜索当前类型的外部派生类型能够直接访问的方法。
        /// </summary>
        /// <param name="source">一个 <see cref="Type"/> 类型的对象。</param>
        /// <param name="name">要获取的方法的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，
        /// 由一个或多个指定搜索执行方式的 <see cref="BindingFlags"/> 组成。</param>
        /// <returns>表示当前类型的外部派生类型能够直接访问的名为 <paramref name="name"/> 且匹配绑定约束
        /// <paramref name="bindingAttr"/> 的所有方法的 <see cref="MethodInfo"/> 对象。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="AmbiguousMatchException">找到了多个满足条件的方法。</exception>
        internal static MethodInfo GetDerivedAccessibleEventMethod(this Type source, string name,
            BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            try
            {
                return (
                    from member in source.GetDerivedAccessibleMethods(bindingAttr)
                    where member.Name == name
                    select member).SingleOrDefault();
            }
            catch (InvalidOperationException)
            {
                throw new AmbiguousMatchException();
            }
        }

        /// <summary>
        /// 将指定集合的元素添加到 <see cref="ICollection{T}"/> 中。
        /// </summary>
        /// <typeparam name="T"><see cref="ICollection{T}"/> 中的元素的类型。</typeparam>
        /// <param name="source">一个 <see cref="ICollection{T}"/> 对象。</param>
        /// <param name="collection">应将其元素添加到 <see cref="ICollection{T}"/> 中的集合。
        /// 集合自身不能为 <see langword="null"/>，但它可以包含为 <see langword="null"/> 的元素。</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>
        /// 或 <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        private static void AddRange<T>(this ICollection<T> source, IEnumerable<T> collection)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (collection is null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            foreach (var item in collection)
            {
                source.Add(item);
            }
        }

        /// <summary>
        /// 使用对象的 <see cref="object.ToString()"/> 方法实现的类型无关的通用比较器。
        /// </summary>
        /// <typeparam name="T">要进行比较的对象的类型。</typeparam>
        [Serializable]
        private class ToStringComparer<T> : IEqualityComparer, IComparer, IEqualityComparer<T>, IComparer<T>
        {
            /// <summary>
            /// 用于进行字符串比较的比较器。
            /// </summary>
            private readonly StringComparer stringComparer;

            /// <summary>
            /// 初始化 <see cref="ToStringComparer{T}"/> 类的新实例。
            /// </summary>
            public ToStringComparer() : this(false) { }

            /// <summary>
            /// 初始化 <see cref="ToStringComparer{T}"/> 类的新实例，并指定进行字符串比较是是否忽略大小写。
            /// </summary>
            /// <param name="ignoreCase">指定进行字符串比较是是否忽略大小写。</param>
            public ToStringComparer(bool ignoreCase) => this.stringComparer = ignoreCase ?
                StringComparer.InvariantCultureIgnoreCase : StringComparer.InvariantCulture;

            /// <summary>
            /// 初始化 <see cref="ToStringComparer{T}"/> 类的新实例，并指定进行字符串比较时使用的比较器。
            /// </summary>
            /// <param name="comparer">进行字符串比较时使用的比较器。</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="comparer"/> 为 <see langword="null"/>。</exception>
            public ToStringComparer(StringComparer comparer) => this.stringComparer = comparer;

            /// <summary>
            /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值进行比较，
            /// 并返回一个值，指示一个对象是小于、 等于还是大于另一个对象。
            /// </summary>
            /// <param name="x">要比较的第一个对象。</param>
            /// <param name="y">要比较的第二个对象。</param>
            /// <returns>一个指示 <paramref name="x"/> 与 <paramref name="y"/> 的大小关系的有符号整数，
            /// 大于为正，等于为零，小于为负。</returns>
            public int Compare(T x, T y) => this.stringComparer.Compare(x?.ToString(), y?.ToString());

            /// <summary>
            /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值确定两个对象是否相等。
            /// </summary>
            /// <param name="x">要比较的第一个对象。</param>
            /// <param name="y">要比较的第二个对象。</param>
            /// <returns>
            /// 如果两个对象的 <see cref="object.ToString()"/> 方法返回的值相等，
            /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
            /// </returns>
            public bool Equals(T x, T y) => this.stringComparer.Equals(x?.ToString(), y?.ToString());

            /// <summary>
            /// 获取对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。
            /// </summary>
            /// <param name="obj">要为其获取哈希代码的对象。</param>
            /// <returns>对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。</returns>
            public int GetHashCode(T obj) => this.stringComparer.GetHashCode(obj?.ToString());

            /// <summary>
            /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值进行比较，
            /// 并返回一个值，指示一个对象是小于、 等于还是大于另一个对象。
            /// </summary>
            /// <param name="x">要比较的第一个对象。</param>
            /// <param name="y">要比较的第二个对象。</param>
            /// <returns>一个指示 <paramref name="x"/> 与 <paramref name="y"/> 的大小关系的有符号整数，
            /// 大于为正，等于为零，小于为负。</returns>
            /// <exception cref="InvalidCastException"><paramref name="x"/>
            /// 或 <paramref name="y"/> 无法转换为 <typeparamref name="T"/> 类型的对象。</exception>
            int IComparer.Compare(object x, object y) => this.Compare((T)x, (T)y);

            /// <summary>
            /// 使用两个对象的 <see cref="object.ToString()"/> 方法返回的值确定两个对象是否相等。
            /// </summary>
            /// <param name="x">要比较的第一个对象。</param>
            /// <param name="y">要比较的第二个对象。</param>
            /// <returns>
            /// 如果两个对象均为 <typeparamref name="T"/> 类型的对象，
            /// 且 <see cref="object.ToString()"/> 方法返回的值相等，
            /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
            /// </returns>
            /// <exception cref="InvalidCastException"><paramref name="x"/>
            /// 或 <paramref name="y"/> 无法转换为 <typeparamref name="T"/> 类型的对象。</exception>
            bool IEqualityComparer.Equals(object x, object y) => this.Equals((T)x, (T)y);

            /// <summary>
            /// 获取对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。
            /// </summary>
            /// <param name="obj">要为其获取哈希代码的对象。</param>
            /// <returns>对象的 <see cref="object.ToString()"/> 方法返回的值的哈希代码。</returns>
            /// <exception cref="InvalidCastException">
            /// <paramref name="obj"/> 无法转换为 <typeparamref name="T"/> 类型的对象。</exception>
            int IEqualityComparer.GetHashCode(object obj) => this.GetHashCode((T)obj);
        }
    }
}
