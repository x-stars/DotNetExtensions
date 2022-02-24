using System;
using System.Collections.Concurrent;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供通过反射访问对象的公共属性的方法。
    /// </summary>
    /// <typeparam name="TObject">对象的类型。</typeparam>
    internal static class SimplePropertyAccessor<TObject>
    {
        /// <summary>
        /// 表示当前类型的所有公共实例属性的 <see langword="get"/> 访问器的委托。
        /// </summary>
        private static readonly ConcurrentDictionary<string, Delegate> GetValueDelegates =
            new ConcurrentDictionary<string, Delegate>();

        /// <summary>
        /// 表示当前类型的所有公共实例属性的 <see langword="set"/> 访问器的委托。
        /// </summary>
        private static readonly ConcurrentDictionary<string, Delegate> SetValueDelegates =
            new ConcurrentDictionary<string, Delegate>();

        /// <summary>
        /// 获取指定对象的指定公共属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="instance">要获取属性的对象。</param>
        /// <param name="propertyName">要获取值的属性的名称。</param>
        /// <returns><paramref name="instance"/>
        /// 中名为 <paramref name="propertyName"/> 的属性的值。</returns>
        /// <exception cref="InvalidCastException">
        /// 指定属性的值无法转换为 <typeparamref name="T"/> 类型。</exception>
        /// <exception cref="MissingMemberException">
        /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="get"/> 属性。</exception>
        public static T? GetValue<T>(TObject instance, string propertyName)
        {
            var getValueDelegate = SimplePropertyAccessor<TObject>.GetValueDelegates.GetOrAdd(
                propertyName, SimplePropertyAccessor<TObject>.CreateGetValueDelegate);
            return ((Func<TObject, T?>)getValueDelegate).Invoke(instance);
        }

        /// <summary>
        /// 设置指定对象的指定公共属性的值。
        /// </summary>
        /// <typeparam name="T">属性的类型。</typeparam>
        /// <param name="instance">要设置属性的对象。</param>
        /// <param name="propertyName">要设置值的属性的名称。</param>
        /// <param name="value">属性的新值。</param>
        /// <exception cref="InvalidCastException">
        /// <typeparamref name="T"/> 类型的值无法转换为指定属性的类型。</exception>
        /// <exception cref="MissingMemberException">
        /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="set"/> 属性。</exception>
        public static void SetValue<T>(TObject instance, string propertyName, T? value)
        {
            var setValueDelegate = SimplePropertyAccessor<TObject>.SetValueDelegates.GetOrAdd(
                propertyName, SimplePropertyAccessor<TObject>.CreateSetValueDelegate);
            ((Action<TObject, T?>)setValueDelegate).Invoke(instance, value);
        }

        /// <summary>
        /// 创建当前类型的指定公共实例属性的 <see langword="get"/> 访问器的委托。
        /// </summary>
        /// <param name="propertyName">要创建委托的属性的名称。</param>
        /// <returns><typeparamref name="TObject"/> 类型中名为
        /// <paramref name="propertyName"/> 的属性的 <see langword="get"/> 访问器的委托。</returns>
        /// <exception cref="MissingMemberException">
        /// 无法找到名为 <paramref name="propertyName"/> 的 <see langword="get"/> 属性。</exception>
        private static Delegate CreateGetValueDelegate(string propertyName)
        {
            var property = typeof(TObject).GetProperty(propertyName, Type.EmptyTypes);
            if (property?.GetMethod is null) { throw new MissingMemberException(); }
            if (property.GetMethod.IsStatic) { throw new MissingMemberException(); }
            if (!property.GetMethod.IsPublic) { throw new MissingMemberException(); }
            var typeParameters = new[] { typeof(TObject), property.PropertyType };
            var delegateType = typeof(Func<,>).MakeGenericType(typeParameters);
            return property.GetMethod.CreateDelegate(delegateType);
        }

        /// <summary>
        /// 创建当前类型的指定公共实例属性的 <see langword="set"/> 访问器的委托。
        /// </summary>
        /// <param name="propertyName">要创建委托的属性的名称。</param>
        /// <returns><typeparamref name="TObject"/> 类型中名为
        /// <paramref name="propertyName"/> 的属性的 <see langword="set"/> 访问器的委托。</returns>
        /// <exception cref="MissingMemberException">
        /// 无法访问名为 <paramref name="propertyName"/> 的 <see langword="set"/> 属性。</exception>
        private static Delegate CreateSetValueDelegate(string propertyName)
        {
            var property = typeof(TObject).GetProperty(propertyName, Type.EmptyTypes);
            if (property?.SetMethod is null) { throw new MissingMemberException(); }
            if (property.SetMethod.IsStatic) { throw new MissingMemberException(); }
            if (!property.SetMethod.IsPublic) { throw new MissingMemberException(); }
            var typeParameters = new[] { typeof(TObject), property.PropertyType };
            var delegateType = typeof(Action<,>).MakeGenericType(typeParameters);
            return property.SetMethod.CreateDelegate(delegateType);
        }
    }
}
