using System;
using System.Reflection;

namespace XNetEx.Dynamic
{
    /// <summary>
    /// 提供动态调用对象或类型成员的扩展方法。
    /// </summary>
    public static class MemberInvokeExtensions
    {
        /// <summary>
        /// 表示实例成员访问修饰符的 <see cref="BindingFlags"/> 的位域组合。
        /// </summary>
        private const BindingFlags InstanceBindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// 表示实例成员访问修饰符的 <see cref="BindingFlags"/> 的位域组合。
        /// </summary>
        private const BindingFlags StaticBindingFlags =
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// 调用与给定参数匹配的构造函数创建当前类型的实例。
        /// </summary>
        /// <param name="type">要创建实例的类型。</param>
        /// <param name="arguments">与要调用构造函数的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>新创建的 <paramref name="type"/> 类型对象的引用。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// 找不到与 <paramref name="arguments"/> 中的参数匹配的构造函数。</exception>
        /// <exception cref="AmbiguousMatchException">多个构造函数与绑定条件匹配。</exception>
        public static object? CreateInstance(this Type type, params object?[]? arguments)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            return type.InvokeMember(null!, BindingFlags.CreateInstance |
                MemberInvokeExtensions.InstanceBindingFlags, null, null, arguments);
        }

        /// <summary>
        /// 调用当前对象与给定参数匹配的指定名称的实例方法。
        /// </summary>
        /// <param name="instance">要调用实例方法的对象。</param>
        /// <param name="name">要调用的实例方法的名称。</param>
        /// <param name="arguments">与要调用实例方法的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>调用 <paramref name="instance"/> 中名为 <paramref name="name"/> 的实例方法的返回值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// 找不到与 <paramref name="arguments"/> 中参数匹配的名为 <paramref name="name"/> 的实例方法。</exception>
        /// <exception cref="AmbiguousMatchException">多个实例方法与绑定条件匹配。</exception>
        public static object? InvokeMethod(this object instance, string name, params object?[]? arguments)
        {
            if (instance is null) { throw new ArgumentNullException(nameof(instance)); }
            return instance.GetType().InvokeMember(name, BindingFlags.InvokeMethod |
                MemberInvokeExtensions.InstanceBindingFlags, null, instance, arguments);
        }

        /// <summary>
        /// 调用当前对象与给定参数匹配的指定名称的静态方法。
        /// </summary>
        /// <param name="type">要调用静态方法的类型。</param>
        /// <param name="name">要调用的静态方法的名称。</param>
        /// <param name="arguments">与要调用静态方法的参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns>调用 <paramref name="type"/> 中名为 <paramref name="name"/> 的静态方法的返回值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingMethodException">
        /// 找不到与 <paramref name="arguments"/> 中参数匹配的名为 <paramref name="name"/> 的静态方法。</exception>
        /// <exception cref="AmbiguousMatchException">多个静态方法与绑定条件匹配。</exception>
        public static object? InvokeStaticMethod(this Type type, string name, params object?[]? arguments)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            return type.InvokeMember(name, BindingFlags.InvokeMethod |
                MemberInvokeExtensions.StaticBindingFlags, null, null, arguments);
        }

        /// <summary>
        /// 获取当前对象指定名称的实例字段的值。
        /// </summary>
        /// <param name="instance">要获取实例字段的值的对象。</param>
        /// <param name="name">要获取值的实例字段的名称。</param>
        /// <returns><paramref name="instance"/> 中名为 <paramref name="name"/> 的实例字段的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的实例字段。</exception>
        public static object? GetField(this object instance, string name)
        {
            if (instance is null) { throw new ArgumentNullException(nameof(instance)); }
            return instance.GetType().InvokeMember(name, BindingFlags.GetField |
                MemberInvokeExtensions.InstanceBindingFlags, null, instance, null);
        }

        /// <summary>
        /// 设置当前对象指定名称的实例字段的值。
        /// </summary>
        /// <param name="instance">要设置实例字段的值的对象。</param>
        /// <param name="name">要设置值的实例字段的名称。</param>
        /// <param name="value">要设置为实例字段的值的对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的实例字段。</exception>
        public static void SetField(this object instance, string name, object? value)
        {
            if (instance is null) { throw new ArgumentNullException(nameof(instance)); }
            instance.GetType().InvokeMember(name, BindingFlags.SetField |
                MemberInvokeExtensions.InstanceBindingFlags, null, instance, new[] { value });
        }

        /// <summary>
        /// 获取当前对象指定名称的静态字段的值。
        /// </summary>
        /// <param name="type">要获取静态字段的值的对象。</param>
        /// <param name="name">要获取值的静态字段的名称。</param>
        /// <returns><paramref name="type"/> 中名为 <paramref name="name"/> 的静态字段的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的静态字段。</exception>
        public static object? GetStaticField(this Type type, string name)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            return type.InvokeMember(name, BindingFlags.GetField |
                MemberInvokeExtensions.StaticBindingFlags, null, null, null);
        }

        /// <summary>
        /// 设置当前对象指定名称的静态字段的值。
        /// </summary>
        /// <param name="type">要设置静态字段的值的对象。</param>
        /// <param name="name">要设置值的静态字段的名称。</param>
        /// <param name="value">要设置为静态字段的值的对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="FieldAccessException"><paramref name="name"/> 表示的字段为常量字段。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的静态字段。</exception>
        public static void SetStaticField(this Type type, string name, object? value)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            type.InvokeMember(name, BindingFlags.SetField |
                MemberInvokeExtensions.StaticBindingFlags, null, null, new[] { value });
        }

        /// <summary>
        /// 获取当前对象与给定索引参数匹配的指定名称的实例属性的值。
        /// </summary>
        /// <param name="instance">要获取实例属性的值的对象。</param>
        /// <param name="name">要获取值的实例属性的名称。</param>
        /// <param name="indices">与要调用实例属性的索引参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns><paramref name="instance"/> 中名为 <paramref name="name"/> 的实例属性的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的实例属性。</exception>
        /// <exception cref="MissingMethodException">
        /// 找不到与 <paramref name="indices"/> 中索引参数匹配的名为 <paramref name="name"/> 的实例属性。</exception>
        /// <exception cref="AmbiguousMatchException">多个实例属性与绑定条件匹配。</exception>
        public static object? GetProperty(this object instance, string name, params object?[]? indices)
        {
            if (instance is null) { throw new ArgumentNullException(nameof(instance)); }
            return instance.GetType().InvokeMember(name, BindingFlags.GetProperty |
                MemberInvokeExtensions.InstanceBindingFlags, null, instance, indices);
        }

        /// <summary>
        /// 设置当前对象与给定索引参数匹配的指定名称的实例属性的值。
        /// </summary>
        /// <param name="instance">要设置实例属性的值的对象。</param>
        /// <param name="name">要设置值的实例属性的名称。</param>
        /// <param name="value">要设置为实例属性的值的对象。</param>
        /// <param name="indices">与要调用实例属性的索引参数数量、顺序和类型匹配的参数数组。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="instance"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的实例属性。</exception>
        /// <exception cref="MissingMethodException">
        /// 找不到与 <paramref name="indices"/> 中索引参数匹配的名为 <paramref name="name"/> 的实例属性。</exception>
        /// <exception cref="AmbiguousMatchException">多个实例属性与绑定条件匹配。</exception>
        public static void SetProperty(this object instance, string name, object? value, params object?[]? indices)
        {
            if (instance is null) { throw new ArgumentNullException(nameof(instance)); }
            var arguments = (indices ?? Array.Empty<object?>()).Append(value);
            instance.GetType().InvokeMember(name, BindingFlags.SetProperty |
                MemberInvokeExtensions.InstanceBindingFlags, null, instance, arguments);
        }

        /// <summary>
        /// 获取当前类型与给定索引参数匹配的指定名称的静态属性的值。
        /// </summary>
        /// <param name="type">要获取静态属性的值的对象。</param>
        /// <param name="name">要获取值的静态属性的名称。</param>
        /// <param name="indices">与要调用静态属性的索引参数数量、顺序和类型匹配的参数数组。</param>
        /// <returns><paramref name="type"/> 中名为 <paramref name="name"/> 的静态属性的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的静态属性。</exception>
        /// <exception cref="MissingMethodException">
        /// 找不到与 <paramref name="indices"/> 中索引参数匹配的名为 <paramref name="name"/> 的静态属性。</exception>
        /// <exception cref="AmbiguousMatchException">多个静态属性与绑定条件匹配。</exception>
        public static object? GetStaticProperty(this Type type, string name, params object?[]? indices)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            return type.InvokeMember(name, BindingFlags.GetProperty |
                MemberInvokeExtensions.StaticBindingFlags, null, null, indices);
        }

        /// <summary>
        /// 设置当前类型与给定索引参数匹配的指定名称的静态属性的值。
        /// </summary>
        /// <param name="type">要设置静态属性的值的对象。</param>
        /// <param name="name">要设置值的静态属性的名称。</param>
        /// <param name="value">要设置为静态属性的值的对象。</param>
        /// <param name="indices">与要调用静态属性的索引参数数量、顺序和类型匹配的参数数组。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 或 <paramref name="name"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="MissingFieldException">找不到名为 <paramref name="name"/> 的静态属性。</exception>
        /// <exception cref="MissingMethodException">
        /// 找不到与 <paramref name="indices"/> 中索引参数匹配的名为 <paramref name="name"/> 的静态属性。</exception>
        /// <exception cref="AmbiguousMatchException">多个静态属性与绑定条件匹配。</exception>
        public static void SetStaticProperty(this Type type, string name, object? value, params object?[]? indices)
        {
            if (type is null) { throw new ArgumentNullException(nameof(type)); }
            var arguments = (indices ?? Array.Empty<object?>()).Append(value);
            type.InvokeMember(name, BindingFlags.SetProperty |
                MemberInvokeExtensions.StaticBindingFlags, null, null, arguments);
        }
    }
}
