using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace XstarS.Reflection.Emit
{
    /// <summary>
    /// 提供动态生成程序集相关的扩展方法。
    /// </summary>
    public static class EmitExtensions
    {
        /// <summary>
        /// 将新的属性的定义、字段、获取方法和设置方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="propertyName">属性的名称。</param>
        /// <param name="propertyType">属性的类型。</param>
        /// <param name="canRead">指定属性是否可以获取值。</param>
        /// <param name="canWrite">指定属性是否可以设置值。</param>
        /// <param name="propertyAttributes">属性的修饰符。</param>
        /// <param name="getMethodAttributes">属性的 <see langword="get"/> 方法的修饰符。</param>
        /// <param name="setMethodAttributes">属性的 <see langword="set"/> 方法的修饰符。</param>
        /// <returns>一个四元组，依次为属性的定义、字段、获取方法和设置方法。</returns>
        public static (PropertyBuilder Property, FieldBuilder Field, MethodBuilder GetMethod, MethodBuilder SetMethod)
            DefineDefaultProperty(this TypeBuilder source, string propertyName, Type propertyType,
            bool canRead = true, bool canWrite = true,
            PropertyAttributes propertyAttributes = PropertyAttributes.None,
            MethodAttributes getMethodAttributes = MethodAttributes.Public |
            MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.SpecialName,
            MethodAttributes setMethodAttributes = MethodAttributes.Public |
            MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.SpecialName)
        {
            // 定义属性和字段。
            var t_Property = propertyType;
            var ip_Property = source.DefineProperty(propertyName,
                propertyAttributes, t_Property, Type.EmptyTypes);
            var if_Property = source.DefineField($"<{propertyName}>__k_BackingField",
                t_Property, FieldAttributes.Private);

            // 定义属性的 get。
            var im_get_Property = (MethodBuilder)null;
            if (canRead)
            {
                im_get_Property = source.DefineMethod($"get_{propertyName}",
                    getMethodAttributes, t_Property, Type.EmptyTypes);
                {
                    var ilGen = im_get_Property.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, if_Property);
                    ilGen.Emit(OpCodes.Ret);
                    ip_Property.SetGetMethod(im_get_Property);
                }
            }

            // 定义属性的 set。
            var im_set_Property = (MethodBuilder)null;
            if (canWrite)
            {
                im_set_Property = source.DefineMethod($"set_{propertyName}",
                    setMethodAttributes, typeof(void), new[] { t_Property });
                {
                    im_set_Property.DefineParameter(1, ParameterAttributes.None, "value");
                    var ilGen = im_set_Property.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Stfld, if_Property);
                    ilGen.Emit(OpCodes.Ret);
                    ip_Property.SetSetMethod(im_set_Property);
                }
            }

            return (ip_Property, if_Property, im_get_Property, im_set_Property);
        }

        /// <summary>
        /// 将新的事件的定义、委托、订阅方法和取消订阅方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="eventName">事件的名称。</param>
        /// <param name="eventType">事件的类型。</param>
        /// <param name="eventAttributes">事件的修饰符。</param>
        /// <param name="addMethodAttributes">事件的 <see langword="add"/> 方法的修饰符。</param>
        /// <param name="removeMethodAttributes">事件的 <see langword="remove"/> 方法的修饰符。</param>
        /// <returns>一个四元组，依次为事件定义、事件委托、订阅方法和取消订阅方法。</returns>
        public static (EventBuilder Event, FieldBuilder Field, MethodBuilder AddOnMethod, MethodBuilder RemoveOnMethod)
            DefineDefaultEvent(this TypeBuilder source, string eventName, Type eventType,
            EventAttributes eventAttributes = EventAttributes.None,
            MethodAttributes addMethodAttributes = MethodAttributes.Public |
            MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.SpecialName,
            MethodAttributes removeMethodAttributes = MethodAttributes.Public |
            MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.SpecialName)
        {
            // 定义事件和委托。
            var t_Event = eventType;
            var ie_Event = source.DefineEvent(eventName, eventAttributes, t_Event);
            var if_Event = source.DefineField(eventName, t_Event, FieldAttributes.Private);

            // 定义事件的 add。
            var im_add_Event = source.DefineMethod($"add_{eventName}",
                addMethodAttributes, typeof(void), new[] { t_Event });
            {
                im_add_Event.DefineParameter(1, ParameterAttributes.None, "value");
                var ilGen = im_add_Event.GetILGenerator();
                var v_0 = ilGen.DeclareLocal(t_Event);
                var v_1 = ilGen.DeclareLocal(t_Event);
                var v_2 = ilGen.DeclareLocal(t_Event);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, if_Event);
                ilGen.Emit(OpCodes.Stloc_0);
                var startLoc = ilGen.DefineLabel();
                ilGen.MarkLabel(startLoc);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Stloc_1);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Call, typeof(Delegate).GetMethod(
                    nameof(Delegate.Combine),
                    new[] { typeof(Delegate), typeof(Delegate) }));
                ilGen.Emit(OpCodes.Castclass, t_Event);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, if_Event);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call, (
                    from method in typeof(Interlocked).GetMethods()
                    where method.Name == nameof(Interlocked.CompareExchange)
                    && method.IsGenericMethod
                    select method).First().MakeGenericMethod(t_Event));
                ilGen.Emit(OpCodes.Stloc_0);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Bne_Un_S, startLoc);
                ilGen.Emit(OpCodes.Ret);
                ie_Event.SetAddOnMethod(im_add_Event);
            }

            // 定义事件的 remove。
            var im_remove_Event = source.DefineMethod($"remove_{eventName}",
                removeMethodAttributes, typeof(void), new[] { t_Event });
            {
                im_remove_Event.DefineParameter(1, ParameterAttributes.None, "value");
                var ilGen = im_remove_Event.GetILGenerator();
                var v_0 = ilGen.DeclareLocal(t_Event);
                var v_1 = ilGen.DeclareLocal(t_Event);
                var v_2 = ilGen.DeclareLocal(t_Event);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, if_Event);
                ilGen.Emit(OpCodes.Stloc_0);
                var startLoc = ilGen.DefineLabel();
                ilGen.MarkLabel(startLoc);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Stloc_1);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Call, typeof(Delegate).GetMethod(
                    nameof(Delegate.Remove),
                    new[] { typeof(Delegate), typeof(Delegate) }));
                ilGen.Emit(OpCodes.Castclass, t_Event);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, if_Event);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call, (
                    from method in typeof(Interlocked).GetMethods()
                    where method.Name == nameof(Interlocked.CompareExchange)
                    && method.IsGenericMethod
                    select method).First().MakeGenericMethod(t_Event));
                ilGen.Emit(OpCodes.Stloc_0);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Bne_Un_S, startLoc);
                ilGen.Emit(OpCodes.Ret);
                ie_Event.SetRemoveOnMethod(im_remove_Event);
            }

            return (ie_Event, if_Event, im_add_Event, im_remove_Event);
        }

        /// <summary>
        /// 将新的属性的定义、字段、获取方法和设置方法添加到当前类型，并实现抽象基类或接口中定义的指定属性。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="property">抽象基类或接口中属性的定义。</param>
        /// <param name="newSlot">指定属性的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <param name="final">指定事件的相关方法是否不可重写，即是否有 <see langword="sealed"/> 修饰符。</param>
        /// <returns>一个四元组，依次为属性的定义、字段、获取方法和设置方法。</returns>
        public static (PropertyBuilder Property, FieldBuilder Field, MethodBuilder GetMethod, MethodBuilder SetMethod)
            DefineDefaultPropertyImplement(this TypeBuilder source, PropertyInfo property, bool newSlot, bool final)
        {
            // 定义属性和字段。
            var t_Property = property.PropertyType;
            var ip_Property = source.DefineProperty(property.Name,
                property.Attributes, t_Property, Type.EmptyTypes);
            var if_Property = source.DefineField($"<{property.Name}>__k_BackingField",
                t_Property, FieldAttributes.Private);

            // 定义属性的 get。
            var im_get_Property = (MethodBuilder)null;
            if (property.CanRead)
            {
                var getMethodAttributes = property.GetMethod.Attributes;
                getMethodAttributes &= ~MethodAttributes.Abstract;
                if (final) { getMethodAttributes |= MethodAttributes.Final; }
                if (!newSlot) { getMethodAttributes &= ~MethodAttributes.NewSlot; }
                im_get_Property = source.DefineMethod(property.GetMethod.Name,
                    getMethodAttributes, t_Property, Type.EmptyTypes);
                {
                    var ilGen = im_get_Property.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, if_Property);
                    ilGen.Emit(OpCodes.Ret);
                    ip_Property.SetGetMethod(im_get_Property);
                }
            }

            // 定义属性的 set。
            var im_set_Property = (MethodBuilder)null;
            if (property.CanWrite)
            {
                var setMethodAttributes = property.GetMethod.Attributes;
                setMethodAttributes &= ~MethodAttributes.Abstract;
                if (final) { setMethodAttributes |= MethodAttributes.Final; }
                if (!newSlot) { setMethodAttributes &= ~MethodAttributes.NewSlot; }
                im_set_Property = source.DefineMethod(property.SetMethod.Name,
                    setMethodAttributes, typeof(void), new[] { t_Property });
                {
                    im_set_Property.DefineParameter(1, ParameterAttributes.None, "value");
                    var ilGen = im_set_Property.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Stfld, if_Property);
                    ilGen.Emit(OpCodes.Ret);
                    ip_Property.SetSetMethod(im_set_Property);
                }
            }

            return (ip_Property, if_Property, im_get_Property, im_set_Property);
        }

        /// <summary>
        /// 将新的事件的定义、委托、订阅方法和取消订阅方法添加到当前类型，并实现抽象基类或接口中定义的指定事件。
        /// </summary>
        /// <remarks>
        /// 当此事件在基类中有非抽象定义时，不应调用此方法。此时将无法重写事件相关方法，会导致其仅能动态调用。
        /// </remarks>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="event">抽象基类或接口中事件的定义。</param>
        /// <param name="newSlot">指定事件的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <param name="final">指定事件的相关方法是否不可重写，即是否有 <see langword="sealed"/> 修饰符。</param>
        /// <returns>一个四元组，依次为事件定义、事件委托、订阅方法和取消订阅方法。</returns>
        public static (EventBuilder Event, FieldBuilder Field, MethodBuilder AddOnMethod, MethodBuilder RemoveOnMethod)
            DefineDefaultEventImplement(this TypeBuilder source, EventInfo @event, bool newSlot, bool final)
        {
            // 定义事件和委托。
            var t_Event = @event.EventHandlerType;
            var ie_Event = source.DefineEvent(@event.Name, @event.Attributes, t_Event);
            var if_Event = source.DefineField(@event.Name, t_Event, FieldAttributes.Private);

            // 定义事件的 add。
            var addMethodAttributes = @event.AddMethod.Attributes;
            addMethodAttributes &= ~MethodAttributes.Abstract;
            if (final) { addMethodAttributes |= MethodAttributes.Final; }
            if (!newSlot) { addMethodAttributes &= ~MethodAttributes.NewSlot; }
            var im_add_Event = source.DefineMethod(@event.AddMethod.Name,
                addMethodAttributes, typeof(void), new[] { t_Event });
            {
                im_add_Event.DefineParameter(1, ParameterAttributes.None, "value");
                var ilGen = im_add_Event.GetILGenerator();
                var v_0 = ilGen.DeclareLocal(t_Event);
                var v_1 = ilGen.DeclareLocal(t_Event);
                var v_2 = ilGen.DeclareLocal(t_Event);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, if_Event);
                ilGen.Emit(OpCodes.Stloc_0);
                var startLoc = ilGen.DefineLabel();
                ilGen.MarkLabel(startLoc);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Stloc_1);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Call, typeof(Delegate).GetMethod(
                    nameof(Delegate.Combine),
                    new[] { typeof(Delegate), typeof(Delegate) }));
                ilGen.Emit(OpCodes.Castclass, t_Event);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, if_Event);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call, (
                    from method in typeof(Interlocked).GetMethods()
                    where method.Name == nameof(Interlocked.CompareExchange)
                    && method.IsGenericMethod
                    select method).First().MakeGenericMethod(t_Event));
                ilGen.Emit(OpCodes.Stloc_0);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Bne_Un_S, startLoc);
                ilGen.Emit(OpCodes.Ret);
                ie_Event.SetAddOnMethod(im_add_Event);
            }

            // 定义事件的 remove。
            var removeMethodAttributes = @event.AddMethod.Attributes;
            removeMethodAttributes &= ~MethodAttributes.Abstract;
            if (final) { removeMethodAttributes |= MethodAttributes.Final; }
            if (!newSlot) { addMethodAttributes &= ~MethodAttributes.NewSlot; }
            var im_remove_Event = source.DefineMethod(@event.RemoveMethod.Name,
                removeMethodAttributes, typeof(void), new[] { t_Event });
            {
                im_remove_Event.DefineParameter(1, ParameterAttributes.None, "value");
                var ilGen = im_remove_Event.GetILGenerator();
                var v_0 = ilGen.DeclareLocal(t_Event);
                var v_1 = ilGen.DeclareLocal(t_Event);
                var v_2 = ilGen.DeclareLocal(t_Event);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, if_Event);
                ilGen.Emit(OpCodes.Stloc_0);
                var startLoc = ilGen.DefineLabel();
                ilGen.MarkLabel(startLoc);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Stloc_1);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Call, typeof(Delegate).GetMethod(
                    nameof(Delegate.Remove),
                    new[] { typeof(Delegate), typeof(Delegate) }));
                ilGen.Emit(OpCodes.Castclass, t_Event);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, if_Event);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call, (
                    from method in typeof(Interlocked).GetMethods()
                    where method.Name == nameof(Interlocked.CompareExchange)
                    && method.IsGenericMethod
                    select method).First().MakeGenericMethod(t_Event));
                ilGen.Emit(OpCodes.Stloc_0);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Bne_Un_S, startLoc);
                ilGen.Emit(OpCodes.Ret);
                ie_Event.SetRemoveOnMethod(im_remove_Event);
            }

            return (ie_Event, if_Event, im_add_Event, im_remove_Event);
        }
    }
}
