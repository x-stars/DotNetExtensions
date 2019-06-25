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
    /// 提供反射发出相关的帮助方法。
    /// </summary>
    internal static class ReflectionEmitHelper
    {
        /// <summary>
        /// 以指定的构造函数为基础，定义仅调用指定构造函数的构造函数，并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseConstructor">作为基础的构造函数的定义。</param>
        /// <returns>定义完成的构造函数，仅调用 <paramref name="baseConstructor"/> 构造函数。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="baseConstructor"/> 的访问级别不为公共或保护。</exception>
        internal static ConstructorBuilder DefineDefaultConstructor(
            this TypeBuilder source, ConstructorInfo baseConstructor)
        {
            // 参数检查。
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (baseConstructor is null)
            {
                throw new ArgumentNullException(nameof(baseConstructor));
            }
            if (!baseConstructor.IsInheritableInstance())
            {
                throw new MethodAccessException();
            }

            // 定义构造函数。
            var baseParameters = baseConstructor.GetParameters();
            var baseAttributes = baseConstructor.Attributes;
            var constructor = source.DefineConstructor(
                baseAttributes, baseConstructor.CallingConvention,
                Array.ConvertAll(baseParameters, param => param.ParameterType));
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = constructor.DefineParameter(
                    i + 1, baseParameter.Attributes, baseParameter.Name);
                if (baseParameter.HasDefaultValue)
                {
                    parameter.SetConstant(baseParameter.DefaultValue);
                }
            }
            var ilGen = constructor.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    int position = i + 1;
                    switch (position)
                    {
                        case 0: ilGen.Emit(OpCodes.Ldarg_0); break;
                        case 1: ilGen.Emit(OpCodes.Ldarg_1); break;
                        case 2: ilGen.Emit(OpCodes.Ldarg_2); break;
                        case 3: ilGen.Emit(OpCodes.Ldarg_3); break;
                        default:
                            ilGen.Emit((position <= byte.MaxValue) ?
                                OpCodes.Ldarg_S : OpCodes.Ldarg, position);
                            break;
                    }
                }
                ilGen.Emit(OpCodes.Call, baseConstructor);
                ilGen.Emit(OpCodes.Ret);
            }

            return constructor;
        }

        /// <summary>
        /// 以指定属性的定义为基础，将新的属性的定义、获取方法和设置方法添加到当前类型。
        /// 并设定其仅抛出 <see cref="NotImplementedException"/> 异常。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseProperty">作为基础的属性的定义。</param>
        /// <returns>定义完成的属性，仅抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="baseProperty"/> 的方法无法在程序集外部重写。</exception>
        internal static PropertyBuilder DefineNotImplementedProperty(
            this TypeBuilder source, PropertyInfo baseProperty)
        {
            // 参数检查。
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new MethodAccessException();
            }

            // 定义属性或构造器。
            var property = source.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));
            if (baseProperty.CanRead)
            {
                var methodGet = source.DefineNotImplementedMethod(baseProperty.GetMethod);
                property.SetGetMethod(methodGet);
            }
            if (baseProperty.CanWrite)
            {
                var methodSet = source.DefineNotImplementedMethod(baseProperty.SetMethod);
                property.SetSetMethod(methodSet);
            }

            return property;
        }

        /// <summary>
        /// 以指定属性的定义为基础，将新的自动属性的定义、字段、获取方法和设置方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseProperty">作为基础的属性的定义。</param>
        /// <returns>一个键值对，键为自动属性的定义，值为其对应的字段。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentException"><paramref name="baseProperty"/> 是一个索引器。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="baseProperty"/> 的方法无法在程序集外部重写。</exception>
        internal static KeyValuePair<PropertyBuilder, FieldBuilder> DefineDefaultProperty(
            this TypeBuilder source, PropertyInfo baseProperty)
        {
            // 参数检查。
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (baseProperty.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new MethodAccessException();
            }

            var newSlot = baseProperty.DeclaringType.IsInterface;

            // 定义属性和字段。
            var propertyType = baseProperty.PropertyType;
            var property = source.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, propertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));
            var field = source.DefineField($"<{baseProperty.Name}>__k_BackingField",
                propertyType, FieldAttributes.Private);

            // 定义属性的 get。
            if (baseProperty.CanRead)
            {
                var baseMethodGet = baseProperty.GetMethod;
                var baseAttributes = baseMethodGet.Attributes;
                var baseReturnParam = baseMethodGet.ReturnParameter;
                var baseParameters = baseMethodGet.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
                var methodGet = source.DefineMethod(baseMethodGet.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = methodGet.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = methodGet.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, field);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetGetMethod(methodGet);
            }

            // 定义属性的 set。
            if (baseProperty.CanWrite)
            {
                var baseMethodSet = baseProperty.SetMethod;
                var baseAttributes = baseMethodSet.Attributes;
                var baseReturnParam = baseMethodSet.ReturnParameter;
                var baseParameters = baseMethodSet.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
                var methodSet = source.DefineMethod(baseMethodSet.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = methodSet.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = methodSet.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Stfld, field);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetSetMethod(methodSet);
            }

            return new KeyValuePair<PropertyBuilder, FieldBuilder>(property, field);
        }

        /// <summary>
        /// 以指定属性的定义为基础，将新的可绑定属性的定义、字段、获取方法和设置方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseProperty">作为基础的属性的定义。</param>
        /// <param name="methodOnPropertyChanged"><code>void OnProperty(string)</code> 方法。</param>
        /// <returns>一个键值对，键为可绑定属性的定义，值为其对应的字段。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentException"><paramref name="baseProperty"/> 是一个索引器。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="baseProperty"/> 的方法无法在程序集外部重写。</exception>
        internal static KeyValuePair<PropertyBuilder, FieldBuilder> DefineBindableProperty(
            this TypeBuilder source, PropertyInfo baseProperty, MethodInfo methodOnPropertyChanged)
        {
            // 参数检查。
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (methodOnPropertyChanged is null)
            {
                throw new ArgumentNullException(nameof(methodOnPropertyChanged));
            }
            if (baseProperty.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new MethodAccessException();
            }

            var newSlot = baseProperty.DeclaringType.IsInterface;

            // 定义属性和字段。
            var propertyType = baseProperty.PropertyType;
            var property = source.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, propertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));
            var field = source.DefineField($"<{baseProperty.Name}>__k_BackingField",
                propertyType, FieldAttributes.Private);

            // 定义属性的 get。
            if (baseProperty.CanRead)
            {
                var baseMethodGet = baseProperty.GetMethod;
                var baseAttributes = baseMethodGet.Attributes;
                var baseReturnParam = baseMethodGet.ReturnParameter;
                var baseParameters = baseMethodGet.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
                var methodGet = source.DefineMethod(baseMethodGet.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseMethodGet.GetParameters(), param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var getMethodParameter = baseParameters[i];
                    var parameter = methodGet.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = methodGet.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, field);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetGetMethod(methodGet);
            }

            // 定义属性的 set。
            if (baseProperty.CanWrite)
            {
                var baseMethodSet = baseProperty.SetMethod;
                var baseAttributes = baseMethodSet.Attributes;
                var baseReturnParam = baseMethodSet.ReturnParameter;
                var baseParameters = baseMethodSet.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
                var methodSet = source.DefineMethod(baseMethodSet.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = methodSet.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = methodSet.GetILGenerator();
                {
                    var typeEqualityComparer = typeof(EqualityComparer<>).MakeGenericType(propertyType);
                    ilGen.Emit(OpCodes.Call, typeEqualityComparer.GetProperty(
                        nameof(EqualityComparer<object>.Default), Type.EmptyTypes).GetMethod);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, field);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Callvirt, typeEqualityComparer.GetMethod(
                        nameof(EqualityComparer<object>.Equals), new[] { propertyType, propertyType }));
                    var endLoc = ilGen.DefineLabel();
                    ilGen.Emit(OpCodes.Brtrue_S, endLoc);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Stfld, field);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldstr, baseProperty.Name);
                    ilGen.Emit(methodOnPropertyChanged.IsVirtual ?
                        OpCodes.Callvirt : OpCodes.Call, methodOnPropertyChanged);
                    ilGen.MarkLabel(endLoc);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetSetMethod(methodSet);
            }

            return new KeyValuePair<PropertyBuilder, FieldBuilder>(property, field);
        }

        /// <summary>
        /// 以指定事件的定义为基础，将新的事件的定义、委托、订阅方法和取消订阅方法添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseEvent">作为基础的事件的定义。</param>
        /// <returns>一个键值对，键为事件的定义，值为其对应的事件委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// 存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="baseEvent"/> 的方法无法在程序集外部重写。</exception>
        internal static KeyValuePair<EventBuilder, FieldBuilder> DefineDefaultEvent(
            this TypeBuilder source, EventInfo baseEvent)
        {
            // 参数检查。
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (baseEvent is null)
            {
                throw new ArgumentNullException(nameof(baseEvent));
            }
            if (!baseEvent.AddMethod.IsOverridable())
            {
                throw new MethodAccessException();
            }

            var newSlot = baseEvent.DeclaringType.IsInterface;

            // 定义事件和委托。
            var eventType = baseEvent.EventHandlerType;
            var @event = source.DefineEvent(baseEvent.Name, baseEvent.Attributes, eventType);
            var field = source.DefineField(baseEvent.Name, eventType, FieldAttributes.Private);

            // 定义事件的 add。
            {
                var baseMethodAdd = baseEvent.AddMethod;
                var baseAttributes = baseMethodAdd.Attributes;
                var baseReturnParam = baseMethodAdd.ReturnParameter;
                var baseParameters = baseMethodAdd.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
                var methodAdd = source.DefineMethod(baseMethodAdd.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = methodAdd.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }
                var ilGen = methodAdd.GetILGenerator();
                {
                    var local0 = ilGen.DeclareLocal(eventType);
                    var local1 = ilGen.DeclareLocal(eventType);
                    var local2 = ilGen.DeclareLocal(eventType);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, field);
                    ilGen.Emit(OpCodes.Stloc_0);
                    var labelStart = ilGen.DefineLabel();
                    ilGen.MarkLabel(labelStart);
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
                    ilGen.Emit(OpCodes.Ldflda, field);
                    ilGen.Emit(OpCodes.Ldloc_2);
                    ilGen.Emit(OpCodes.Ldloc_1);
                    ilGen.Emit(OpCodes.Call, typeof(Interlocked).GetMethods().Where(
                        method => method.Name == nameof(Interlocked.CompareExchange) &&
                        method.IsGenericMethod).First().MakeGenericMethod(eventType));
                    ilGen.Emit(OpCodes.Stloc_0);
                    ilGen.Emit(OpCodes.Ldloc_0);
                    ilGen.Emit(OpCodes.Ldloc_1);
                    ilGen.Emit(OpCodes.Bne_Un_S, labelStart);
                    ilGen.Emit(OpCodes.Ret);
                    @event.SetAddOnMethod(methodAdd);
                }
            }

            // 定义事件的 remove。
            {
                var baseMethodRemove = baseEvent.RemoveMethod;
                var baseAttributes = baseMethodRemove.Attributes;
                var baseReturnParam = baseMethodRemove.ReturnParameter;
                var baseParameters = baseMethodRemove.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
                var methodRemove = source.DefineMethod(baseMethodRemove.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = methodRemove.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = methodRemove.GetILGenerator();
                {
                    var local0 = ilGen.DeclareLocal(eventType);
                    var local1 = ilGen.DeclareLocal(eventType);
                    var local2 = ilGen.DeclareLocal(eventType);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, field);
                    ilGen.Emit(OpCodes.Stloc_0);
                    var labelStart = ilGen.DefineLabel();
                    ilGen.MarkLabel(labelStart);
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
                    ilGen.Emit(OpCodes.Ldflda, field);
                    ilGen.Emit(OpCodes.Ldloc_2);
                    ilGen.Emit(OpCodes.Ldloc_1);
                    ilGen.Emit(OpCodes.Call, typeof(Interlocked).GetMethods().Where(
                        method => method.Name == nameof(Interlocked.CompareExchange) &&
                        method.IsGenericMethod).First().MakeGenericMethod(eventType));
                    ilGen.Emit(OpCodes.Stloc_0);
                    ilGen.Emit(OpCodes.Ldloc_0);
                    ilGen.Emit(OpCodes.Ldloc_1);
                    ilGen.Emit(OpCodes.Bne_Un_S, labelStart);
                    ilGen.Emit(OpCodes.Ret);
                    @event.SetRemoveOnMethod(methodRemove);
                }
            }

            return new KeyValuePair<EventBuilder, FieldBuilder>(@event, field);
        }

        /// <summary>
        /// 使用已有的 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的委托，
        /// 定义 <code>OnPropertyChanged(string)</code> 方法，并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="fieldPropertyChanged">
        /// <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件的委托。</param>
        /// <returns>定义完成的 <code>void OnPropertyChanged(string)</code> 方法。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="FieldAccessException"><paramref name="fieldPropertyChanged"/>
        /// 不为 <see cref="PropertyChangedEventHandler"/> 类型。</exception>
        internal static MethodBuilder DefineOnPropertyChangedMethod(
            this TypeBuilder source, FieldInfo fieldPropertyChanged)
        {
            // 参数检查。
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (fieldPropertyChanged is null)
            {
                throw new ArgumentNullException(nameof(fieldPropertyChanged));
            }
            if (fieldPropertyChanged.FieldType != typeof(PropertyChangedEventHandler))
            {
                throw new FieldAccessException();
            }

            // 定义方法。
            var methodOnPropertyChanged = source.DefineMethod(
                $"On{nameof(INotifyPropertyChanged.PropertyChanged)}",
                MethodAttributes.Family | MethodAttributes.HideBySig |
                MethodAttributes.NewSlot | MethodAttributes.Virtual,
                typeof(void), new[] { typeof(string) });
            methodOnPropertyChanged.DefineParameter(1, ParameterAttributes.None, "propertyName");
            var ilGen = methodOnPropertyChanged.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, fieldPropertyChanged);
                ilGen.Emit(OpCodes.Dup);
                var labelInvoke = ilGen.DefineLabel();
                ilGen.Emit(OpCodes.Brtrue_S, labelInvoke);
                ilGen.Emit(OpCodes.Pop);
                ilGen.Emit(OpCodes.Ret);
                ilGen.MarkLabel(labelInvoke);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Newobj,
                    typeof(PropertyChangedEventArgs).GetConstructor(new[] { typeof(string) }));
                ilGen.Emit(OpCodes.Callvirt, typeof(PropertyChangedEventHandler).GetMethod(
                    nameof(PropertyChangedEventHandler.Invoke),
                    new[] { typeof(object), typeof(PropertyChangedEventArgs) }));
                ilGen.Emit(OpCodes.Ret);
            }

            return methodOnPropertyChanged;
        }

        /// <summary>
        /// 以指定方法的定义为基础，将新的方法添加到当前类型，
        /// 并设定其仅抛出 <see cref="NotImplementedException"/> 异常。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseMethod">作为基础的方法的定义。</param>
        /// <returns>定义完成的方法，仅抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        internal static MethodBuilder DefineNotImplementedMethod(
            this TypeBuilder source, MethodInfo baseMethod)
        {
            // 参数检查。
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (!baseMethod.IsOverridable())
            {
                throw new MethodAccessException();
            }

            var newSlot = baseMethod.DeclaringType.IsInterface;

            // 定义方法。
            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract;
            if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
            var method = source.DefineMethod(baseMethod.Name,
                attributes, baseReturnParam.ParameterType,
                Array.ConvertAll(baseParameters, param => param.ParameterType));
            // 泛型参数。
            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                method.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var baseGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                genericParam.SetGenericParameterAttributes(
                    baseGenericParam.GenericParameterAttributes);
            }
            // 普通参数。
            var returnParam = method.DefineParameter(0, baseReturnParam.Attributes, null);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = method.DefineParameter(
                    i + 1, baseParameter.Attributes, baseParameter.Name);
                if (baseParameter.HasDefaultValue)
                {
                    parameter.SetConstant(baseParameter.DefaultValue);
                }
            }
            // 生成 IL 代码。
            var ilGen = method.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Newobj,
                    typeof(NotImplementedException).GetConstructor(Type.EmptyTypes));
                ilGen.Emit(OpCodes.Throw);
            }

            return method;
        }
    }
}
