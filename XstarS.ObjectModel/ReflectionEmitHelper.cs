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
        /// 发出将指定索引处的参数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="position">要加载到计算堆栈的参数的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="position"/> 小于 0。</exception>
        internal static void EmitLdarg(this ILGenerator ilGen, int position)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if (position < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(position));
            }

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

        /// <summary>
        /// 以指定的构造函数为基础，定义仅调用此构造函数的构造函数，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义构造函数的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseConstructor">作为基础的构造函数。</param>
        /// <returns>定义的构造函数，调用 <paramref name="baseConstructor"/> 构造函数。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseConstructor"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static ConstructorBuilder DefineBaseInvokeConstructor(
            this TypeBuilder type, ConstructorInfo baseConstructor)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseConstructor is null)
            {
                throw new ArgumentNullException(nameof(baseConstructor));
            }
            if (!baseConstructor.IsInheritableInstance())
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseConstructor));
            }

            var baseParameters = baseConstructor.GetParameters();
            var baseAttributes = baseConstructor.Attributes;
            var constructor = type.DefineConstructor(
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
        /// 以指定的属性为基础，定义抛出未实现异常的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <returns>定义完成的属性，抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 的方法无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static PropertyBuilder DefineNotImplementedPropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new ArgumentException();
            }

            var property = type.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));
            if (baseProperty.CanRead)
            {
                var method = type.DefineNotImplementedMethodOverride(baseProperty.GetMethod);
                property.SetGetMethod(method);
            }
            if (baseProperty.CanWrite)
            {
                var method = type.DefineNotImplementedMethodOverride(baseProperty.SetMethod);
                property.SetSetMethod(method);
            }

            return property;
        }

        /// <summary>
        /// 以指定的属性为基础，定义调用此属性并触发属性更改事件的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="methodOnPropertyChanged"><c>void OnPropertyChanged(string)</c> 方法。</param>
        /// <returns>定义的可绑定属性，调用 <paramref name="baseProperty"/> 属性，
        /// 并在属性更改时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是抽象属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static PropertyBuilder DefineBaseBindablePropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty, MethodInfo methodOnPropertyChanged)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (methodOnPropertyChanged is null)
            {
                throw new ArgumentNullException(nameof(methodOnPropertyChanged));
            }
            if (baseProperty.GetAccessors().All(accessor => accessor.IsAbstract))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }

            var baseInInterface = baseProperty.DeclaringType.IsInterface;
            var property = type.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseMethod.GetParameters(), param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = method.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    for (int i = 0; i < baseParameters.Length; i++)
                    {
                        ilGen.EmitLdarg(i + 1);
                    }
                    ilGen.Emit(OpCodes.Call, baseProperty.GetMethod);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = method.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    for (int i = 0; i < baseParameters.Length; i++)
                    {
                        ilGen.EmitLdarg(i + 1);
                    }
                    ilGen.Emit(OpCodes.Call, baseProperty.SetMethod);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    var propertyName = baseProperty.Name;
                    if (baseProperty.IsIndexProperty()) { propertyName += "[]"; }
                    ilGen.Emit(OpCodes.Ldstr, propertyName);
                    ilGen.Emit(OpCodes.Callvirt, methodOnPropertyChanged);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetSetMethod(method);
            }

            return property;
        }

        /// <summary>
        /// 以指定的属性为基础，定义以自动属性模式实现的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <returns>定义的自动属性与其对应的字段。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是索引属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static KeyValuePair<PropertyBuilder, FieldBuilder> DefineAutoPropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (baseProperty.IsIndexProperty())
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }

            var baseInInterface = baseProperty.DeclaringType.IsInterface;
            var property = type.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));
            var field = type.DefineField(
                baseProperty.Name, baseProperty.PropertyType, FieldAttributes.Private);

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = method.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, field);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = method.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Stfld, field);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetSetMethod(method);
            }

            return new KeyValuePair<PropertyBuilder, FieldBuilder>(property, field);
        }

        /// <summary>
        /// 以指定的属性为基础，定义以可绑定自动属性模式实现的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="methodOnPropertyChanged"><c>void OnProperty(string)</c> 方法。</param>
        /// <returns>定义的可绑定自动属性及其对应的字段，
        /// 在属性更改时触发 <see cref="INotifyPropertyChanged.PropertyChanged"/> 事件。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是索引属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static KeyValuePair<PropertyBuilder, FieldBuilder> DefineAutoBindablePropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty, MethodInfo methodOnPropertyChanged)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseProperty is null)
            {
                throw new ArgumentNullException(nameof(baseProperty));
            }
            if (methodOnPropertyChanged is null)
            {
                throw new ArgumentNullException(nameof(methodOnPropertyChanged));
            }
            if (baseProperty.IsIndexProperty())
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }

            var baseInInterface = baseProperty.DeclaringType.IsInterface;
            var property = type.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));
            var field = type.DefineField(
                baseProperty.Name, baseProperty.PropertyType, FieldAttributes.Private);

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseMethod.GetParameters(), param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = method.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldfld, field);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = method.GetILGenerator();
                {
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldarg_1);
                    ilGen.Emit(OpCodes.Stfld, field);
                    ilGen.Emit(OpCodes.Ldarg_0);
                    ilGen.Emit(OpCodes.Ldstr, baseProperty.Name);
                    ilGen.Emit(OpCodes.Callvirt, methodOnPropertyChanged);
                    ilGen.Emit(OpCodes.Ret);
                }
                property.SetSetMethod(method);
            }

            return new KeyValuePair<PropertyBuilder, FieldBuilder>(property, field);
        }

        /// <summary>
        /// 以指定的事件为基础，定义以默认事件模式实现的重写事件，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义事件的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseEvent">作为基础的事件。</param>
        /// <returns>定义的默认模式的事件与其对应的事件委托。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseEvent"/> 的方法无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static KeyValuePair<EventBuilder, FieldBuilder> DefineDefaultEventOverride(
            this TypeBuilder type, EventInfo baseEvent)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseEvent is null)
            {
                throw new ArgumentNullException(nameof(baseEvent));
            }
            if (!baseEvent.AddMethod.IsOverridable())
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseEvent));
            }

            var baseInInterface = baseEvent.DeclaringType.IsInterface;
            var eventType = baseEvent.EventHandlerType;
            var @event = type.DefineEvent(baseEvent.Name, baseEvent.Attributes, eventType);
            var field = type.DefineField(baseEvent.Name, eventType, FieldAttributes.Private);

            {
                var baseMethod = baseEvent.AddMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }
                var ilGen = method.GetILGenerator();
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
                    @event.SetAddOnMethod(method);
                }
            }

            {
                var baseMethod = baseEvent.RemoveMethod;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameters[i].Attributes, baseParameters[i].Name);
                }
                var ilGen = method.GetILGenerator();
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
                    @event.SetRemoveOnMethod(method);
                }
            }

            return new KeyValuePair<EventBuilder, FieldBuilder>(@event, field);
        }

        /// <summary>
        /// 以指定的方法为基础，定义抛出未实现异常的重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <returns>定义的方法，抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineNotImplementedMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (!baseMethod.IsOverridable())
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseMethod));
            }

            var baseInInterface = baseMethod.DeclaringType.IsInterface;
            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract;
            if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }
            var method = type.DefineMethod(baseMethod.Name,
                attributes, baseReturnParam.ParameterType,
                Array.ConvertAll(baseParameters, param => param.ParameterType));

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
