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
        /// 获取一个值，该值指示当前属性是否为抽象。
        /// </summary>
        /// <param name="source">一个 <see cref="PropertyInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 为抽象，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static bool IsAbstract(this PropertyInfo source)
        {
            if (source is null) { throw new ArgumentNullException(nameof(source)); }

            return
                source.CanRead ? source.GetMethod.IsAbstract : (
                source.CanWrite ? source.SetMethod.IsAbstract :
                throw new InvalidProgramException());
        }

        /// <summary>
        /// 获取一个值，该值指示当前属性是否可以被重写。
        /// </summary>
        /// <param name="source">一个 <see cref="PropertyInfo"/> 类的对象。</param>
        /// <returns>若 <paramref name="source"/> 可以被重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static bool IsOverridable(this PropertyInfo source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }

            return
                source.CanRead ? (source.GetMethod.IsVirtual && !source.GetMethod.IsFinal) : (
                source.CanWrite ? (source.SetMethod.IsVirtual && !source.SetMethod.IsFinal) :
                throw new InvalidProgramException());
        }

        /// <summary>
        /// 以指定的构造函数为基础，定义仅调用指定构造函数的构造函数，并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="constructor">作为基础的构造函数的定义。</param>
        /// <param name="asPublic">指定是否将构造函数的访问级别定义为公共。</param>
        /// <returns>定义完成的构造函数，仅调用 <paramref name="constructor"/> 构造函数。</returns>
        /// <exception cref="ArgumentException"><paramref name="constructor"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static ConstructorBuilder DefineDefaultConstructor(this TypeBuilder source,
            ConstructorInfo constructor, bool asPublic = false)
        {
            // 参数检查。
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (constructor is null) { throw new ArgumentNullException(nameof(constructor)); }
            if (!constructor.IsPublic && !constructor.IsFamily)
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(constructor));
            }

            // 定义构造函数。
            var constructorParameters = constructor.GetParameters();
            var constructorAttributes = constructor.Attributes;
            if (asPublic)
            {
                constructorAttributes &= ~MethodAttributes.MemberAccessMask;
                constructorAttributes |= MethodAttributes.Public;
            }
            var ic_Constructor = source.DefineConstructor(
                constructorAttributes, constructor.CallingConvention,
                Array.ConvertAll(constructorParameters, param => param.ParameterType));
            {
                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    var constructorParameter = constructorParameters[i];
                    var parameter = ic_Constructor.DefineParameter(
                        i + 1, constructorParameter.Attributes, constructorParameter.Name);
                    if (constructorParameter.HasDefaultValue)
                    {
                        parameter.SetConstant(constructorParameter.DefaultValue);
                    }
                }
                var ilGen = ic_Constructor.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    switch (i + 1)
                    {
                        case 1: ilGen.Emit(OpCodes.Ldarg_1); break;
                        case 2: ilGen.Emit(OpCodes.Ldarg_2); break;
                        case 3: ilGen.Emit(OpCodes.Ldarg_3); break;
                        default: ilGen.Emit(OpCodes.Ldarg_S, i + 1); break;
                    }
                }
                ilGen.Emit(OpCodes.Call, constructor);
                ilGen.Emit(OpCodes.Ret);
            }

            return ic_Constructor;
        }

        /// <summary>
        /// 以指定属性或索引器的定义为基础，将新的属性的定义、获取方法和设置方法添加到当前类型。
        /// 并设定其仅抛出 <see cref="NotImplementedException"/> 异常。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="property">作为基础的属性或索引器的定义。</param>
        /// <param name="newSlot">指定属性或索引器的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>一个三元组，依次为属性或索引器的定义、获取方法和设置方法。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static (PropertyBuilder Property, MethodBuilder GetMethod, MethodBuilder SetMethod)
            DefineNotImplementedPropertyOrIndexer(this TypeBuilder source, PropertyInfo property, bool newSlot)
        {
            // 参数检查。
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (property is null) { throw new ArgumentNullException(nameof(property)); }

            // 定义属性或构造器。
            var ip_Property = source.DefineProperty(property.Name, property.Attributes, property.PropertyType,
                Array.ConvertAll(property.GetIndexParameters(), param => param.ParameterType));
            var im_get_Property = property.CanRead ?
                source.DefineNotImplementedMethod(property.GetMethod, newSlot) : null;
            var im_set_Property = property.CanWrite ?
                source.DefineNotImplementedMethod(property.SetMethod, newSlot) : null;

            return (ip_Property, im_get_Property, im_set_Property);
        }

        /// <summary>
        /// 以指定属性的定义为基础，将新的自动属性的定义、字段、获取方法和设置方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="property">作为基础的属性的定义。</param>
        /// <param name="newSlot">指定属性的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>一个四元组，依次为属性的定义、字段、获取方法和设置方法。</returns>
        /// <exception cref="ArgumentException"><paramref name="property"/> 是一个索引器。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static (PropertyBuilder Property, FieldBuilder Field, MethodBuilder GetMethod, MethodBuilder SetMethod)
            DefineDefaultProperty(this TypeBuilder source, PropertyInfo property, bool newSlot)
        {
            // 参数检查。
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (property is null) { throw new ArgumentNullException(nameof(property)); }
            if (property.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(property));
            }

            // 定义属性和字段。
            var propertyType = property.PropertyType;
            var ip_Property = source.DefineProperty(property.Name, property.Attributes, propertyType,
                Array.ConvertAll(property.GetIndexParameters(), param => param.ParameterType));
            var if_Property = source.DefineField($"<{property.Name}>__k_BackingField",
                propertyType, FieldAttributes.Private);

            // 定义属性的 get。
            var im_get_Property = (MethodBuilder)null;
            if (property.CanRead)
            {
                var getMethod = property.GetMethod;
                var getMethodAttributes = getMethod.Attributes;
                var getMethodReturnParam = getMethod.ReturnParameter;
                var getMethodParameters = getMethod.GetParameters();
                getMethodAttributes &= ~MethodAttributes.Abstract;
                getMethodAttributes |= MethodAttributes.Final;
                if (!newSlot) { getMethodAttributes &= ~MethodAttributes.NewSlot; }
                im_get_Property = source.DefineMethod(getMethod.Name,
                    getMethodAttributes, getMethodReturnParam.ParameterType,
                    Array.ConvertAll(getMethodParameters, param => param.ParameterType));
                {
                    for (int i = 0; i < getMethodParameters.Length; i++)
                    {
                        var getMethodParameter = getMethodParameters[i];
                        var parameter = im_get_Property.DefineParameter(i + 1,
                            getMethodParameters[i].Attributes, getMethodParameters[i].Name);
                    }
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
                var setMethod = property.SetMethod;
                var setMethodAttributes = setMethod.Attributes;
                var setMethodReturnParam = setMethod.ReturnParameter;
                var setMethodParameters = setMethod.GetParameters();
                setMethodAttributes &= ~MethodAttributes.Abstract;
                setMethodAttributes |= MethodAttributes.Final;
                if (!newSlot) { setMethodAttributes &= ~MethodAttributes.NewSlot; }
                im_set_Property = source.DefineMethod(setMethod.Name,
                    setMethodAttributes, setMethodReturnParam.ParameterType,
                    Array.ConvertAll(setMethodParameters, param => param.ParameterType));
                {
                    for (int i = 0; i < setMethodParameters.Length; i++)
                    {
                        var setMethodParameter = setMethodParameters[i];
                        var parameter = im_set_Property.DefineParameter(i + 1,
                            setMethodParameters[i].Attributes, setMethodParameters[i].Name);
                    }
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
        /// 以指定属性的定义为基础，将新的可绑定属性的定义、字段、获取方法和设置方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="property">作为哦基础的属性的定义。</param>
        /// <param name="methodOnPropertyChanged">定义完成的 <code>SetProperty(ref T, T, string)</code> 方法。</param>
        /// <param name="newSlot">指定属性的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>一个四元组，依次为属性的定义、字段、获取方法和设置方法。</returns>
        /// <exception cref="ArgumentException"><paramref name="property"/> 是一个索引器。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static (PropertyBuilder Property, FieldBuilder Field, MethodBuilder GetMethod, MethodBuilder SetMethod)
            DefineBindableProperty(this TypeBuilder source, PropertyInfo property, MethodInfo methodOnPropertyChanged, bool newSlot)
        {
            // 参数检查。
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (property is null) { throw new ArgumentNullException(nameof(property)); }
            if (methodOnPropertyChanged is null) { throw new ArgumentNullException(nameof(methodOnPropertyChanged)); }
            if (property.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(property));
            }

            // 定义属性和字段。
            var propertyType = property.PropertyType;
            var ip_Property = source.DefineProperty(property.Name, property.Attributes, propertyType,
                Array.ConvertAll(property.GetIndexParameters(), param => param.ParameterType));
            var if_Property = source.DefineField($"<{property.Name}>__k_BackingField",
                propertyType, FieldAttributes.Private);

            // 定义属性的 get。
            var im_get_Property = (MethodBuilder)null;
            if (property.CanRead)
            {
                var getMethod = property.GetMethod;
                var getMethodAttributes = getMethod.Attributes;
                var getMethodReturnParam = getMethod.ReturnParameter;
                var getMethodParameters = getMethod.GetParameters();
                getMethodAttributes &= ~MethodAttributes.Abstract;
                getMethodAttributes |= MethodAttributes.Final;
                if (!newSlot) { getMethodAttributes &= ~MethodAttributes.NewSlot; }
                im_get_Property = source.DefineMethod(getMethod.Name,
                    getMethodAttributes, getMethodReturnParam.ParameterType,
                    Array.ConvertAll(getMethod.GetParameters(), param => param.ParameterType));
                {
                    for (int i = 0; i < getMethodParameters.Length; i++)
                    {
                        var getMethodParameter = getMethodParameters[i];
                        var parameter = im_get_Property.DefineParameter(i + 1,
                            getMethodParameters[i].Attributes, getMethodParameters[i].Name);
                    }
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
                var setMethod = property.SetMethod;
                var setMethodAttributes = setMethod.Attributes;
                var setMethodReturnParam = setMethod.ReturnParameter;
                var setMethodParameters = setMethod.GetParameters();
                setMethodAttributes &= ~MethodAttributes.Abstract;
                setMethodAttributes |= MethodAttributes.Final;
                if (!newSlot) { setMethodAttributes &= ~MethodAttributes.NewSlot; }
                im_set_Property = source.DefineMethod(setMethod.Name,
                    setMethodAttributes, setMethodReturnParam.ParameterType,
                    Array.ConvertAll(setMethodParameters, param => param.ParameterType));
                {
                    for (int i = 0; i < setMethodParameters.Length; i++)
                    {
                        var setMethodParameter = setMethodParameters[i];
                        var parameter = im_set_Property.DefineParameter(i + 1,
                            setMethodParameters[i].Attributes, setMethodParameters[i].Name);
                    }
                    var ilGen = im_set_Property.GetILGenerator();
                    var t_Comparer = typeof(EqualityComparer<>).MakeGenericType(propertyType);
                    ilGen.Emit(OpCodes.Call, t_Comparer.GetMethod(
                        $"get_{nameof(EqualityComparer<object>.Default)}", Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, if_Property);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Callvirt, t_Comparer.GetMethod(
                        nameof(EqualityComparer<object>.Equals), new[] { propertyType, propertyType }));
                    var endLoc = ilGen.DefineLabel();
                    ilGen.Emit(OpCodes.Brtrue_S, endLoc);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Stfld, if_Property);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldstr, property.Name);
                    ilGen.Emit(methodOnPropertyChanged.IsVirtual ?
                        OpCodes.Callvirt : OpCodes.Call, methodOnPropertyChanged);
                    ilGen.MarkLabel(endLoc);
                    ilGen.Emit(OpCodes.Ret);
                    ip_Property.SetSetMethod(im_set_Property);
                }
            }

            return (ip_Property, if_Property, im_get_Property, im_set_Property);
        }

        /// <summary>
        /// 以指定事件的定义为基础，将新的事件的定义、委托、订阅方法和取消订阅方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="event">作为基础的事件的定义。</param>
        /// <param name="newSlot">指定事件的相关方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>一个四元组，依次为事件定义、事件委托、订阅方法和取消订阅方法。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static (EventBuilder Event, FieldBuilder Field, MethodBuilder AddOnMethod, MethodBuilder RemoveOnMethod)
            DefineDefaultEvent(this TypeBuilder source, EventInfo @event, bool newSlot)
        {
            // 参数检查。
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (@event is null) { throw new ArgumentNullException(nameof(@event)); }

            // 定义事件和委托。
            var eventType = @event.EventHandlerType;
            var ie_Event = source.DefineEvent(@event.Name, @event.Attributes, eventType);
            var if_Event = source.DefineField(@event.Name, eventType, FieldAttributes.Private);

            // 定义事件的 add。
            var addMethod = @event.AddMethod;
            var addMethodAttributes = addMethod.Attributes;
            var addMethodReturnParam = addMethod.ReturnParameter;
            var addMethodParameters = addMethod.GetParameters();
            addMethodAttributes &= ~MethodAttributes.Abstract;
            addMethodAttributes |= MethodAttributes.Final;
            if (!newSlot) { addMethodAttributes &= ~MethodAttributes.NewSlot; }
            var im_add_Event = source.DefineMethod(addMethod.Name,
                addMethodAttributes, addMethodReturnParam.ParameterType,
                Array.ConvertAll(addMethodParameters, param => param.ParameterType));
            {
                for (int i = 0; i < addMethodParameters.Length; i++)
                {
                    var addMethodParameter = addMethodParameters[i];
                    var parameter = im_add_Event.DefineParameter(i + 1,
                        addMethodParameter.Attributes, addMethodParameter.Name);
                }
                var ilGen = im_add_Event.GetILGenerator();
                var v_0 = ilGen.DeclareLocal(eventType);
                var v_1 = ilGen.DeclareLocal(eventType);
                var v_2 = ilGen.DeclareLocal(eventType);
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
                ilGen.Emit(OpCodes.Castclass, eventType);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, if_Event);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call, (
                    from method in typeof(Interlocked).GetMethods()
                    where method.Name == nameof(Interlocked.CompareExchange)
                    && method.IsGenericMethod
                    select method).First().MakeGenericMethod(eventType));
                ilGen.Emit(OpCodes.Stloc_0);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Bne_Un_S, startLoc);
                ilGen.Emit(OpCodes.Ret);
                ie_Event.SetAddOnMethod(im_add_Event);
            }

            // 定义事件的 remove。
            var removeMethod = @event.RemoveMethod;
            var removeMethodAttributes = removeMethod.Attributes;
            var removeMethodReturnParam = removeMethod.ReturnParameter;
            var removeMethodParameters = removeMethod.GetParameters();
            removeMethodAttributes &= ~MethodAttributes.Abstract;
            removeMethodAttributes |= MethodAttributes.Final;
            if (!newSlot) { removeMethodAttributes &= ~MethodAttributes.NewSlot; }
            var im_remove_Event = source.DefineMethod(removeMethod.Name,
                removeMethodAttributes, removeMethodReturnParam.ParameterType,
                Array.ConvertAll(removeMethodParameters, param => param.ParameterType));
            {
                for (int i = 0; i < removeMethodParameters.Length; i++)
                {
                    var removeMethodParameter = removeMethodParameters[i];
                    var parameter = im_remove_Event.DefineParameter(i + 1,
                        removeMethodParameters[i].Attributes, removeMethodParameters[i].Name);
                }
                var ilGen = im_remove_Event.GetILGenerator();
                var v_0 = ilGen.DeclareLocal(eventType);
                var v_1 = ilGen.DeclareLocal(eventType);
                var v_2 = ilGen.DeclareLocal(eventType);
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
                ilGen.Emit(OpCodes.Castclass, eventType);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, if_Event);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call, (
                    from method in typeof(Interlocked).GetMethods()
                    where method.Name == nameof(Interlocked.CompareExchange)
                    && method.IsGenericMethod
                    select method).First().MakeGenericMethod(eventType));
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
        /// 使用已有的 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的委托，
        /// 定义 <code>OnPropertyChanged(string)</code> 方法，并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="fieldPropertyChanged"><see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的委托。</param>
        /// <returns>定义完成的 <code>OnPropertyChanged(string)</code> 方法。</returns>
        /// <exception cref="ArgumentException"><paramref name="fieldPropertyChanged"/>
        /// 不为 <see cref="PropertyChangedEventHandler"/> 类型。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineOnPropertyChangedMethod(this TypeBuilder source, FieldInfo fieldPropertyChanged)
        {
            // 参数检查。
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (fieldPropertyChanged is null) { throw new ArgumentNullException(nameof(fieldPropertyChanged)); }
            if (fieldPropertyChanged.FieldType != typeof(PropertyChangedEventHandler))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(fieldPropertyChanged));
            }

            // 定义方法。
            var im_OnPropertyChanged = source.DefineMethod(
                $"On{nameof(INotifyPropertyChanged.PropertyChanged)}",
                MethodAttributes.Family | MethodAttributes.HideBySig |
                MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(void), new[] { typeof(string) });
            {
                im_OnPropertyChanged.DefineParameter(1, ParameterAttributes.None, "propertyName");
                var ilGen = im_OnPropertyChanged.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, fieldPropertyChanged);
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

            return im_OnPropertyChanged;
        }

        /// <summary>
        /// 以指定方法的定义为基础，定义仅抛出 <see cref="NotImplementedException"/> 异常的方法，并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="method">作为基础的方法的定义。</param>
        /// <param name="newSlot">指定方法是否应获取在 vtable 中获取一个新槽。
        /// 当基类为接口时应为 <see langword="true"/>；否则为 <see langword="false"/>。</param>
        /// <returns>定义完成的方法，仅抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineNotImplementedMethod(this TypeBuilder source, MethodInfo method, bool newSlot)
        {
            // 参数检查。
            if (source is null) { throw new ArgumentNullException(nameof(source)); }
            if (method is null) { throw new ArgumentNullException(nameof(method)); }

            // 定义方法。
            var methodAttributes = method.Attributes;
            var methodGenericParams = method.GetGenericArguments();
            var methodReturnParam = method.ReturnParameter;
            var methodParameters = method.GetParameters();
            methodAttributes &= ~MethodAttributes.Abstract;
            methodAttributes |= MethodAttributes.Final;
            if (!newSlot) { methodAttributes &= ~MethodAttributes.NewSlot; }
            var im_Method = source.DefineMethod(method.Name,
                methodAttributes, methodReturnParam.ParameterType,
                Array.ConvertAll(methodParameters, param => param.ParameterType));
            {
                // 泛型参数。
                if (methodGenericParams.Length != 0)
                {
                    var genericParams = im_Method.DefineGenericParameters(
                        Array.ConvertAll(methodGenericParams, param => param.Name));
                    for (int i = 0; i < methodGenericParams.Length; i++)
                    {
                        var methodGenericParam = methodGenericParams[i];
                        var genericParam = genericParams[i];
                        genericParam.SetGenericParameterAttributes(
                            methodGenericParam.GenericParameterAttributes);
                    }
                }
                // 普通参数。
                for (int i = 0; i < methodParameters.Length; i++)
                {
                    var methodParameter = methodParameters[i];
                    var parameter = im_Method.DefineParameter(
                        i + 1, methodParameter.Attributes, methodParameter.Name);
                    if (methodParameter.HasDefaultValue)
                    {
                        parameter.SetConstant(methodParameter.DefaultValue);
                    }
                }
                // 生成 IL 代码。
                var ilGen = im_Method.GetILGenerator();
                ilGen.Emit(OpCodes.Newobj,
                    typeof(NotImplementedException).GetConstructor(Type.EmptyTypes));
                ilGen.Emit(OpCodes.Throw);
            }

            return im_Method;
        }
   }
}
