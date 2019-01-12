using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供从原型构造用于数据绑定的实例的帮助方法。
    /// </summary>
    internal static class BindingBuildHelper
    {
        /// <summary>
        /// 获取一个值，该值指示当前属性是否可以被重写。
        /// </summary>
        /// <param name="source">一个 <see cref="PropertyInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 可以被重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool IsOverridable(this PropertyInfo source) =>
            source.CanRead ? (source.GetMethod.IsVirtual && !source.GetMethod.IsFinal) : (
            source.CanWrite ? (source.SetMethod.IsVirtual && !source.SetMethod.IsFinal) : 
            throw new InvalidProgramException());

        /// <summary>
        /// 将新的自动属性的定义、字段、获取方法和设置方法添加到当前类型，并实现抽象基类或接口中定义的指定属性。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="property">抽象基类或接口中属性的定义。</param>
        /// <param name="newSlot">指定属性的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>一个四元组，依次为属性的定义、字段、获取方法和设置方法。</returns>
        public static (PropertyBuilder Property, FieldBuilder Field, MethodBuilder GetMethod, MethodBuilder SetMethod)
            DefineDefaultProperty(this TypeBuilder source, PropertyInfo property, bool newSlot)
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
                getMethodAttributes |= MethodAttributes.Final;
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
                var setMethodAttributes = property.SetMethod.Attributes;
                setMethodAttributes &= ~MethodAttributes.Abstract;
                setMethodAttributes |= MethodAttributes.Final;
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
        /// 将新的可绑定属性的定义、字段、获取方法和设置方法添加到当前类型，并实现抽象基类或接口中定义的指定属性。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="property">抽象基类或接口中属性的定义。</param>
        /// <param name="methodSetProperty">定义完成的 <code>SetProperty(ref T, T, string)</code> 方法。</param>
        /// <param name="newSlot">指定属性的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>一个四元组，依次为属性的定义、字段、获取方法和设置方法。</returns>
        public static (PropertyBuilder Property, FieldBuilder Field, MethodBuilder GetMethod, MethodBuilder SetMethod)
            DefineBindableProperty(this TypeBuilder source, PropertyInfo property, MethodInfo methodSetProperty, bool newSlot)
        {
            // 定义属性和字段。
            var t_Property = property.PropertyType;
            var ip_Property = source.DefineProperty(property.Name,
                property.Attributes, t_Property, Type.EmptyTypes);
            var if_Property = source.DefineField($"<{property.Name}>k__BackingField",
                t_Property, FieldAttributes.Private);

            // 定义属性的 get。
            var im_get_Property = (MethodBuilder)null;
            if (property.CanRead)
            {
                var getMethodAttributes = property.GetMethod.Attributes;
                getMethodAttributes &= ~MethodAttributes.Abstract;
                getMethodAttributes |= MethodAttributes.Final;
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
                setMethodAttributes |= MethodAttributes.Final;
                if (!newSlot) { setMethodAttributes &= ~MethodAttributes.NewSlot; }
                im_set_Property = source.DefineMethod(property.SetMethod.Name,
                    setMethodAttributes, typeof(void), new[] { t_Property });
                {
                    im_set_Property.DefineParameter(1, ParameterAttributes.None, "value");
                    var ilGen = im_set_Property.GetILGenerator();
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldflda, if_Property);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Ldstr, property.Name);
                    ilGen.Emit(OpCodes.Call,
                        methodSetProperty.MakeGenericMethod(t_Property));
                    ilGen.Emit(OpCodes.Ret);
                    ip_Property.SetSetMethod(im_set_Property);
                }
            }

            return (ip_Property, if_Property, im_get_Property, im_set_Property);
        }

        /// <summary>
        /// 将 <see cref="INotifyPropertyChanged.PropertyChanged"/>
        /// 的默认事件的定义、委托、订阅方法、取消订阅方法和其它相关方法添加到当前类型，
        /// 并实现抽象基类或接口中的定义。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="newSlot">指定事件的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>一个六元组，依次为事件定义、事件委托、订阅方法、取消订阅方法、触发方法和
        /// <code>SetProperty(ref T, T, string)</code> 方法。</returns>
        public static (EventBuilder Event, FieldBuilder Field, MethodBuilder AddOnMethod,
            MethodBuilder RemoveOnMethod, MethodBuilder RaiseMethod, MethodBuilder SetPropertyMethod)
            DefinePropertyChangedEvent(this TypeBuilder source, bool newSlot)
        {
            // 定义事件及其相关方法。
            var @event = typeof(INotifyPropertyChanged).GetEvent(
                nameof(INotifyPropertyChanged.PropertyChanged));
            var t_PropertyChanged = @event.EventHandlerType;
            var (ie_PropertyChanged, if_PropertyChanged, im_add_PropertyChanged, im_remove_PropertyChanged) =
                source.DefineDefaultEvent(@event, newSlot);

            // 定义 OnPropertyChanged(string) 方法。
            var im_OnPropertyChanged = source.DefineMethod(
                $"On{nameof(INotifyPropertyChanged.PropertyChanged)}",
                MethodAttributes.Family | MethodAttributes.HideBySig |
                MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(void), new[] { typeof(string) });
            {
                im_OnPropertyChanged.DefineParameter(1, ParameterAttributes.None, "propertyName");
                var ilGen = im_OnPropertyChanged.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, if_PropertyChanged);
                ilGen.Emit(OpCodes.Dup);
                var invkLoc = ilGen.DefineLabel();
                ilGen.Emit(OpCodes.Brtrue_S, invkLoc);
                ilGen.Emit(OpCodes.Pop);
                ilGen.Emit(OpCodes.Ret);
                ilGen.MarkLabel(invkLoc);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Newobj,
                    typeof(PropertyChangedEventArgs).GetConstructor(new[] { typeof(string) }));
                ilGen.Emit(OpCodes.Callvirt, typeof(PropertyChangedEventHandler).GetMethod(
                    nameof(PropertyChangedEventHandler.Invoke),
                    new[] { typeof(object), typeof(PropertyChangedEventArgs) }));
                ilGen.Emit(OpCodes.Ret);
            }

            // 定义 SetProperty(ref T, T, string) 方法。
            var im_SetProperty = source.DefineMethod("SetProperty",
                MethodAttributes.Family | MethodAttributes.HideBySig);
            {
                var t_T = im_SetProperty.DefineGenericParameters("T")[0];
                im_SetProperty.SetParameters(t_T.MakeByRefType(), t_T, typeof(string));
                im_SetProperty.SetReturnType(typeof(void));
                im_SetProperty.DefineParameter(1, ParameterAttributes.None, "item");
                im_SetProperty.DefineParameter(2, ParameterAttributes.None, "value");
                im_SetProperty.DefineParameter(3, ParameterAttributes.None, "propertyName");
                var ilGen = im_SetProperty.GetILGenerator();
                ilGen.Emit(OpCodes.Call, typeof(EqualityComparer<object>).GetMethod(
                    $"get_{nameof(EqualityComparer<object>.Default)}", Type.EmptyTypes));
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Ldobj, t_T);
                ilGen.Emit(OpCodes.Box, t_T);
                ilGen.Emit(OpCodes.Ldarg_2);
                ilGen.Emit(OpCodes.Box, t_T);
                ilGen.Emit(OpCodes.Callvirt, typeof(EqualityComparer<object>).GetMethod(
                    nameof(EqualityComparer<object>.Equals), new[] { typeof(object), typeof(object) }));
                var endLoc = ilGen.DefineLabel();
                ilGen.Emit(OpCodes.Brtrue_S, endLoc);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Ldarg_2);
                ilGen.Emit(OpCodes.Stobj, t_T);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldarg_3);
                ilGen.Emit(OpCodes.Callvirt, im_OnPropertyChanged);
                ilGen.MarkLabel(endLoc);
                ilGen.Emit(OpCodes.Ret);
            }

            return (ie_PropertyChanged, if_PropertyChanged,
                im_add_PropertyChanged, im_remove_PropertyChanged,
                im_OnPropertyChanged, im_SetProperty);
        }

        /// <summary>
        /// 使用已有的 <code>OnPropertyChanged(string)</code> 方法，
        /// 定义 <code>SetProperty(ref T, T, string)</code> 方法。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="methodOnPropertyChanged"><code>OnPropertyChanged(string)</code> 在基类中的实现。</param>
        /// <returns>定义完成的 <code>SetProperty(ref T, T, string)</code> 方法。</returns>
        public static MethodBuilder DefineSetPropertyMethod(this TypeBuilder source, MethodInfo methodOnPropertyChanged)
        {
            var im_SetProperty = source.DefineMethod("SetProperty",
                MethodAttributes.Family | MethodAttributes.HideBySig);
            {
                var t_T = im_SetProperty.DefineGenericParameters("T")[0];
                im_SetProperty.SetParameters(t_T.MakeByRefType(), t_T, typeof(string));
                im_SetProperty.SetReturnType(typeof(void));
                im_SetProperty.DefineParameter(1, ParameterAttributes.None, "item");
                im_SetProperty.DefineParameter(2, ParameterAttributes.None, "value");
                im_SetProperty.DefineParameter(3, ParameterAttributes.None, "propertyName");
                var ilGen = im_SetProperty.GetILGenerator();
                ilGen.Emit(OpCodes.Call, typeof(EqualityComparer<object>).GetMethod(
                    $"get_{nameof(EqualityComparer<object>.Default)}", Type.EmptyTypes));
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Ldobj, t_T);
                ilGen.Emit(OpCodes.Box, t_T);
                ilGen.Emit(OpCodes.Ldarg_2);
                ilGen.Emit(OpCodes.Box, t_T);
                ilGen.Emit(OpCodes.Callvirt, typeof(EqualityComparer<object>).GetMethod(
                    nameof(EqualityComparer<object>.Equals), new[] { typeof(object), typeof(object) }));
                var endLoc = ilGen.DefineLabel();
                ilGen.Emit(OpCodes.Brtrue_S, endLoc);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Ldarg_2);
                ilGen.Emit(OpCodes.Stobj, t_T);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldarg_3);
                ilGen.Emit(OpCodes.Callvirt, methodOnPropertyChanged);
                ilGen.MarkLabel(endLoc);
                ilGen.Emit(OpCodes.Ret);
            }

            return im_SetProperty;
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
        /// <returns>一个四元组，依次为事件定义、事件委托、订阅方法和取消订阅方法。</returns>
        public static (EventBuilder Event, FieldBuilder Field, MethodBuilder AddOnMethod, MethodBuilder RemoveOnMethod)
            DefineDefaultEvent(this TypeBuilder source, EventInfo @event, bool newSlot)
        {
            // 定义事件和委托。
            var t_Event = @event.EventHandlerType;
            var ie_Event = source.DefineEvent(@event.Name, @event.Attributes, t_Event);
            var if_Event = source.DefineField(@event.Name, t_Event, FieldAttributes.Private);

            // 定义事件的 add。
            var addMethodAttributes = @event.AddMethod.Attributes;
            addMethodAttributes &= ~MethodAttributes.Abstract;
            addMethodAttributes |= MethodAttributes.Final;
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
            removeMethodAttributes |= MethodAttributes.Final;
            if (!newSlot) { removeMethodAttributes &= ~MethodAttributes.NewSlot; }
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

        /// <summary>
        /// 定义仅抛出 <see cref="NotImplementedException"/> 异常的方法，并实现抽象基类或接口中定义的指定方法。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类的对象。</param>
        /// <param name="method">抽象基类或接口中方法的定义。</param>
        /// <param name="newSlot">指定方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>定义完成的方法，仅抛出 <see cref="NotImplementedException"/> 异常。</returns>
        public static MethodBuilder DefineNotImplementedMethod(this TypeBuilder source, MethodInfo method, bool newSlot)
        {
            var parameters = method.GetParameters();
            var methodAttributes = method.Attributes;
            methodAttributes &= ~MethodAttributes.Abstract;
            methodAttributes |= MethodAttributes.Final;
            if (!newSlot) { methodAttributes &= ~MethodAttributes.NewSlot; }
            var im_Method = source.DefineMethod(method.Name,
                methodAttributes, method.ReturnType,
                parameters.ToList().ConvertAll(param => param.ParameterType).ToArray());
            {

                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    im_Method.DefineParameter(i + 1, parameter.Attributes, parameter.Name);
                }
                var ilGen = im_Method.GetILGenerator();
                ilGen.Emit(OpCodes.Newobj,
                    typeof(NotImplementedException).GetConstructor(Type.EmptyTypes));
                ilGen.Emit(OpCodes.Throw);
            }

            return im_Method;
        }
    }
}
