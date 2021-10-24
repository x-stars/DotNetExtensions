using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using XstarS.Diagnostics;

namespace XstarS.Reflection
{
    using NamedValue = KeyValuePair<string, object?>;
    using NamedValues = Dictionary<string, object?>;

    /// <summary>
    /// 提供将对象中的公共成员表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    internal sealed class MemberRepresenter<T> : InternalStructuralRepresenter<T>
    {
        /// <summary>
        /// 表示当前类型的所有公共实例字段的 <see cref="FieldInfo"/> 对象。
        /// </summary>
        private static readonly FieldInfo[] PublicFields =
            MemberRepresenter<T>.GetPublicFields();

        /// <summary>
        /// 表示当前类型的所有公共实例可读属性的 <see cref="PropertyInfo"/> 对象。
        /// </summary>
        private static readonly PropertyInfo[] PublicProperties =
            MemberRepresenter<T>.GetPublicProperties();

        /// <summary>
        /// 初始化 <see cref="MemberRepresenter{T}"/> 类的新实例。
        /// </summary>
        public MemberRepresenter() { }

        /// <summary>
        /// 获取当前类型的所有公共实例字段的 <see cref="FieldInfo"/> 对象。
        /// </summary>
        /// <returns>当前类型的所有公共实例字段的 <see cref="FieldInfo"/> 对象。</returns>
        private static FieldInfo[] GetPublicFields()
        {
            return typeof(T).GetFields(BindingFlags.Instance | BindingFlags.Public);
        }

        /// <summary>
        /// 获取当前类型的所有公共实例可读属性的 <see cref="PropertyInfo"/> 对象。
        /// </summary>
        /// <returns>当前类型的所有公共实例可读属性的 <see cref="PropertyInfo"/> 对象。</returns>
        private static PropertyInfo[] GetPublicProperties()
        {
            return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(
                property => property.CanRead && property.GetMethod!.IsPublic).ToArray();
        }

        /// <summary>
        /// 获取指定对象的所有公共实例成员的名称和对应的值。
        /// </summary>
        /// <param name="value">要获取公共实例成员的值的对象。</param>
        /// <returns><paramref name="value"/> 的所有公共实例成员的名称和对应的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为 <see langword="null"/>。</exception>
        private NamedValues GetPublicValues([DisallowNull] T value)
        {
            if (value is null) { throw new ArgumentNullException(nameof(value)); }
            var namedFields = MemberRepresenter<T>.PublicFields.Select(
                field => new NamedValue(field.Name, field.GetValue(value)));
            var namedProperties = MemberRepresenter<T>.PublicProperties.Select(
                property => new NamedValue(property.Name, property.GetValue(value)));
            return Enumerable.Concat(namedFields, namedProperties).ToDictionary(
                namedValue => namedValue.Key, namedValues => namedValues.Value);
        }

        /// <summary>
        /// 将指定对象中的公共实例成员表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 中的成员的字符串。</returns>
        protected override string RepresentCore([DisallowNull] T value, ISet<object> represented)
        {
            var represents = this.GetPublicValues(value).Select(namedValue =>
            {
                var valueType = namedValue.Value?.GetType();
                var representer = StructuralRepresenter.OfType(valueType);
                var represent = representer.Represent(namedValue.Value, represented);
                return $"{namedValue.Key} = {represent}";
            });
            return $"{typeof(T).ToString()} {{ {string.Join(", ", represents)} }}";
        }
    }
}
