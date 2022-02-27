using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace XstarS.Reflection
{
    using DynamicGetDelegate = Func<object?, object?>;
    using DynamicSetDelegate = Action<object?, object?>;
    using DynamicCreateDelegate = Func<object?[]?, object>;
    using DynamicInvokeDelegate = Func<object?, object?[]?, object?>;

    /// <summary>
    /// 提供以委托快速调用类型成员的扩展方法。
    /// </summary>
    public static class MemberFastInvokeExtensions
    {
        /// <summary>
        /// 表示字段对应的获取值的动态调用委托。
        /// </summary>
        private readonly static ConcurrentDictionary<FieldInfo, DynamicGetDelegate> GetDelegates =
            new ConcurrentDictionary<FieldInfo, DynamicGetDelegate>();

        /// <summary>
        /// 表示字段对应的设置值的动态调用委托。
        /// </summary>
        private readonly static ConcurrentDictionary<FieldInfo, DynamicSetDelegate> SetDelegates =
            new ConcurrentDictionary<FieldInfo, DynamicSetDelegate>();

        /// <summary>
        /// 表示构造函数对应的动态调用委托。
        /// </summary>
        private readonly static ConcurrentDictionary<ConstructorInfo, DynamicCreateDelegate> CreateDelegates =
            new ConcurrentDictionary<ConstructorInfo, DynamicCreateDelegate>();

        /// <summary>
        /// 表示方法对应的动态调用委托。
        /// </summary>
        private readonly static ConcurrentDictionary<MethodInfo, DynamicInvokeDelegate> InvokeDelegates =
            new ConcurrentDictionary<MethodInfo, DynamicInvokeDelegate>();

        /// <summary>
        /// 快速获取指定对象的字段的值。
        /// 以委托实现，较 <see cref="FieldInfo.GetValue(object)"/> 更快。
        /// </summary>
        /// <param name="field">要获取值的 <see cref="FieldInfo"/>。</param>
        /// <param name="instance">要获取字段的值的对象。</param>
        /// <returns><paramref name="instance"/> 的字段的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static object? GetValueFast(this FieldInfo field, object? instance)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var getDelegate = MemberFastInvokeExtensions.GetDelegates.GetOrAdd(
                field, newField => newField.CreateDynamicGetDelegate());
            return getDelegate.Invoke(instance);
        }

        /// <summary>
        /// 快速设置指定对象的字段的值。
        /// 以委托实现，较 <see cref="FieldInfo.SetValue(object, object)"/> 更快。
        /// </summary>
        /// <param name="field">要设置值的 <see cref="FieldInfo"/>。</param>
        /// <param name="instance">要设置字段的值的对象。</param>
        /// <param name="value">要分配给字段的值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static void SetValueFast(this FieldInfo field, object? instance, object? value)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var setDelegate = MemberFastInvokeExtensions.SetDelegates.GetOrAdd(
                field, newField => newField.CreateDynamicSetDelegate());
            setDelegate.Invoke(instance, value);
        }

        /// <summary>
        /// 使用指定的参数快速调用构造函数以创建对象。
        /// 以委托实现，较 <see cref="ConstructorInfo.Invoke(object[])"/> 更快。
        /// </summary>
        /// <param name="constructor">要调用的 <see cref="ConstructorInfo"/>。</param>
        /// <param name="arguments">调用构造函数的参数列表。</param>
        /// <returns>创建的与构造函数关联的类的实例。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="constructor"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="constructor"/> 表示一个类构造函数（或称类初始化器）。</exception>
        public static object InvokeFast(this ConstructorInfo constructor, object?[]? arguments)
        {
            if (constructor is null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }

            var createDelegate = MemberFastInvokeExtensions.CreateDelegates.GetOrAdd(
                constructor, newConstructor => newConstructor.CreateDynamicDelegate());
            return createDelegate.Invoke(arguments);
        }

        /// <summary>
        /// 使用指定的实例和参数快速调用方法。
        /// 以委托实现，较 <see cref="MethodBase.Invoke(object, object[])"/> 更快。
        /// </summary>
        /// <param name="method">要调用的 <see cref="MethodInfo"/>。</param>
        /// <param name="instance">在其上调用方法的对象。</param>
        /// <param name="arguments">调用方法的参数列表。</param>
        /// <returns>调用方法得到的的返回值；若方法无返回值，则为 <see langword="null"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static object? InvokeFast(this MethodInfo method, object? instance, object?[]? arguments)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var invokeDelegate = MemberFastInvokeExtensions.InvokeDelegates.GetOrAdd(
                method, newMethod => newMethod.CreateDynamicDelegate());
            return invokeDelegate.Invoke(instance, arguments);
        }

        /// <summary>
        /// 快速获取指定对象的属性的值。
        /// 以委托实现，较 <see cref="PropertyInfo.GetValue(object)"/> 更快。
        /// </summary>
        /// <param name="property">要获取值的 <see cref="PropertyInfo"/>。</param>
        /// <param name="instance">要获取属性的值的对象。</param>
        /// <returns><paramref name="instance"/> 的属性的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="property"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="property"/> 没有 <see langword="get"/> 访问器。</exception>
        public static object? GetValueFast(this PropertyInfo property, object? instance)
        {
            return property.GetValueFast(instance, Array.Empty<object>());
        }

        /// <summary>
        /// 使用指定的索引参数快速获取指定对象的索引化属性的值。
        /// 以委托实现，较 <see cref="PropertyInfo.GetValue(object, object[])"/> 更快。
        /// </summary>
        /// <param name="property">要获取值的 <see cref="PropertyInfo"/>。</param>
        /// <param name="instance">要获取属性的值的对象。</param>
        /// <param name="indices">索引化属性的可选索引值。</param>
        /// <returns><paramref name="instance"/> 的属性的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="property"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="property"/> 没有 <see langword="get"/> 访问器。</exception>
        public static object? GetValueFast(
            this PropertyInfo property, object? instance, object?[]? indices)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.GetMethod is null)
            {
                var inner = new MissingMethodException();
                throw new ArgumentException(inner.Message, nameof(property), inner);
            }

            var getMethod = property.GetMethod;
            return getMethod.InvokeFast(instance, indices);
        }

        /// <summary>
        /// 快速设置指定对象的属性的值。
        /// 以委托实现，较 <see cref="PropertyInfo.SetValue(object, object)"/> 更快。
        /// </summary>
        /// <param name="property">要设置值的 <see cref="PropertyInfo"/>。</param>
        /// <param name="instance">要设置属性的值的对象。</param>
        /// <param name="value">新的属性值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="property"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="property"/> 没有 <see langword="set"/> 访问器。</exception>
        public static void SetValueFast(this PropertyInfo property, object? instance, object? value)
        {
            property.SetValueFast(instance, value, Array.Empty<object>());
        }

        /// <summary>
        /// 快速设置指定对象的索引化属性的值。
        /// 以委托实现，较 <see cref="PropertyInfo.SetValue(object, object)"/> 更快。
        /// </summary>
        /// <param name="property">要设置值的 <see cref="PropertyInfo"/>。</param>
        /// <param name="instance">要设置属性的值的对象。</param>
        /// <param name="value">新的属性值。</param>
        /// <param name="indices">索引化属性的可选索引值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="property"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="property"/> 没有 <see langword="set"/> 访问器。</exception>
        public static void SetValueFast(
            this PropertyInfo property, object? instance, object? value, object?[]? indices)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }
            if (property.SetMethod is null)
            {
                var inner = new MissingMethodException();
                throw new ArgumentException(inner.Message, nameof(property), inner);
            }

            var setMethod = property.SetMethod;
            var arguments = (indices ?? Array.Empty<object>()).Append(value);
            _ = setMethod.InvokeFast(instance, arguments);
        }

        /// <summary>
        /// 将指定事件处理程序快速添加到事件源。
        /// 以委托实现，较 <see cref="EventInfo.AddEventHandler(object, Delegate)"/> 更快。
        /// </summary>
        /// <param name="event">要添加事件处理程序的 <see cref="EventInfo"/>。</param>
        /// <param name="instance">要添加事件处理程序的事件源。</param>
        /// <param name="handler">要添加到的事件处理委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="event"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="event"/> 没有 <see langword="add"/> 访问器。</exception>
        public static void AddEventHandlerFast(
            this EventInfo @event, object? instance, Delegate? handler)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            if (@event.AddMethod is null)
            {
                var inner = new MissingMethodException();
                throw new ArgumentException(inner.Message, nameof(@event), inner);
            }

            var addMethod = @event.AddMethod;
            _ = addMethod.InvokeFast(instance, new[] { handler });
        }

        /// <summary>
        /// 将指定事件处理程序快速快速从事件源移除。
        /// 以委托实现，较 <see cref="EventInfo.RemoveEventHandler(object, Delegate)"/> 更快。
        /// </summary>
        /// <param name="event">要移除事件处理程序的 <see cref="EventInfo"/>。</param>
        /// <param name="instance">要移除事件处理程序的事件源。</param>
        /// <param name="handler">要从事件源中移除的事件处理委托。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="event"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="event"/> 没有 <see langword="remove"/> 访问器。</exception>
        public static void RemoveEventHandlerFast(
            this EventInfo @event, object? instance, Delegate? handler)
        {
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            if (@event.RemoveMethod is null)
            {
                var inner = new MissingMethodException();
                throw new ArgumentException(inner.Message, nameof(@event), inner);
            }

            var removeMethod = @event.RemoveMethod;
            _ = removeMethod.InvokeFast(instance, new[] { handler });
        }
    }
}
