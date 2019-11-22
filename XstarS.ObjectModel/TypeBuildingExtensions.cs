using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供运行时类型生成相关的扩展方法。
    /// </summary>
    internal static class TypeBuildingExtensions
    {
        /// <summary>
        /// 确定当前 <see cref="MethodBase"/> 是否为程序集外部可继承的实例方法。
        /// </summary>
        /// <param name="method">要进行检查的 <see cref="MethodBase"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 是程序集外部可继承的实例方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static bool IsInheritableInstance(this MethodBase method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return !method.IsStatic &&
                (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly);
        }

        /// <summary>
        /// 确定当前 <see cref="MethodInfo"/> 是否为程序集外部可重写的方法。
        /// </summary>
        /// <param name="method">要进行检查的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 是程序集外部可重写的方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static bool IsOverridable(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.IsInheritableInstance() && (method.IsVirtual && !method.IsFinal);
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
        public static ConstructorBuilder DefineBaseInvokeConstructor(
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

            var baseAttributes = baseConstructor.Attributes;
            var baseParameters = baseConstructor.GetParameters();

            var constructor = type.DefineConstructor(
                baseAttributes, baseConstructor.CallingConvention,
                Array.ConvertAll(baseParameters, param => param.ParameterType));

            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = constructor.DefineParameter(i + 1,
                    baseParameter.Attributes, baseParameter.Name);
            }

            var il = constructor.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                il.EmitLdarg(i + 1);
            }
            il.Emit(OpCodes.Call, baseConstructor);
            il.Emit(OpCodes.Ret);

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
        public static PropertyBuilder DefineNotImplementedPropertyOverride(
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
        /// 以指定的属性为基础，定义以自动属性模式实现的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <returns>定义的自动属性与其对应的字段。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是索引属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static KeyValuePair<PropertyBuilder, FieldBuilder> DefineAutoPropertyOverride(
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
            if (baseProperty.GetIndexParameters().Length != 0)
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseProperty));
            }

            var property = type.DefineProperty(
                baseProperty.Name, baseProperty.Attributes, baseProperty.PropertyType,
                Array.ConvertAll(baseProperty.GetIndexParameters(), param => param.ParameterType));

            var field = type.DefineField(
                baseProperty.Name, baseProperty.PropertyType, FieldAttributes.Private);

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
                il.Emit(OpCodes.Ret);

                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stfld, field);
                il.Emit(OpCodes.Ret);

                property.SetSetMethod(method);
            }

            return new KeyValuePair<PropertyBuilder, FieldBuilder>(property, field);
        }

        /// <summary>
        /// 以指定的事件为基础，定义抛出未实现异常的重写事件，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义事件的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseEvent">作为基础的事件。</param>
        /// <returns>定义的事件，抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseEvent"/> 的方法无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static EventBuilder DefineNotImplementedEventOverride(
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

            var @event = type.DefineEvent(
                baseEvent.Name, baseEvent.Attributes, baseEvent.EventHandlerType);

            {
                var baseMethod = baseEvent.AddMethod;
                var method = type.DefineNotImplementedMethodOverride(baseMethod);
                @event.SetAddOnMethod(method);
            }

            {
                var baseMethod = baseEvent.RemoveMethod;
                var method = type.DefineNotImplementedMethodOverride(baseMethod);
                @event.SetRemoveOnMethod(method);
            }

            return @event;
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
        public static KeyValuePair<EventBuilder, FieldBuilder> DefineDefaultEventOverride(
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

            var @event = type.DefineEvent(
                baseEvent.Name, baseEvent.Attributes, baseEvent.EventHandlerType);

            var field = type.DefineField(
                baseEvent.Name, baseEvent.EventHandlerType, FieldAttributes.Private);

            {
                var baseMethod = baseEvent.AddMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                var eventType = baseEvent.EventHandlerType;
                var local0 = il.DeclareLocal(eventType);
                var local1 = il.DeclareLocal(eventType);
                var local2 = il.DeclareLocal(eventType);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
                il.Emit(OpCodes.Stloc_0);
                var startLabel = il.DefineLabel();
                il.MarkLabel(startLabel);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Stloc_1);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Call, typeof(Delegate).GetMethod(
                    nameof(Delegate.Combine),
                    new[] { typeof(Delegate), typeof(Delegate) }));
                il.Emit(OpCodes.Castclass, eventType);
                il.Emit(OpCodes.Stloc_2);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldflda, field);
                il.Emit(OpCodes.Ldloc_2);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Call, typeof(Interlocked).GetMethods().Where(
                    method => method.Name == nameof(Interlocked.CompareExchange) &&
                    method.IsGenericMethod).First().MakeGenericMethod(eventType));
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Bne_Un_S, startLabel);
                il.Emit(OpCodes.Ret);

                @event.SetAddOnMethod(method);
            }

            {
                var baseMethod = baseEvent.RemoveMethod;
                var baseInInterface = baseMethod.DeclaringType.IsInterface;
                var baseAttributes = baseMethod.Attributes;
                var baseReturnParam = baseMethod.ReturnParameter;
                var baseParameters = baseMethod.GetParameters();
                var attributes = baseAttributes & ~MethodAttributes.Abstract;
                if (!baseInInterface) { attributes &= ~MethodAttributes.NewSlot; }

                var method = type.DefineMethod(baseMethod.Name,
                    attributes, baseReturnParam.ParameterType,
                    Array.ConvertAll(baseParameters, param => param.ParameterType));

                var returnParam = method.DefineParameter(0,
                    baseReturnParam.Attributes, baseReturnParam.Name);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    var parameter = method.DefineParameter(i + 1,
                        baseParameter.Attributes, baseParameter.Name);
                }

                var il = method.GetILGenerator();
                var eventType = baseEvent.EventHandlerType;
                var local0 = il.DeclareLocal(eventType);
                var local1 = il.DeclareLocal(eventType);
                var local2 = il.DeclareLocal(eventType);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldfld, field);
                il.Emit(OpCodes.Stloc_0);
                var startLabel = il.DefineLabel();
                il.MarkLabel(startLabel);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Stloc_1);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Call, typeof(Delegate).GetMethod(
                    nameof(Delegate.Remove),
                    new[] { typeof(Delegate), typeof(Delegate) }));
                il.Emit(OpCodes.Castclass, eventType);
                il.Emit(OpCodes.Stloc_2);
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldflda, field);
                il.Emit(OpCodes.Ldloc_2);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Call, typeof(Interlocked).GetMethods().Where(
                    method => method.Name == nameof(Interlocked.CompareExchange) &&
                    method.IsGenericMethod).First().MakeGenericMethod(eventType));
                il.Emit(OpCodes.Stloc_0);
                il.Emit(OpCodes.Ldloc_0);
                il.Emit(OpCodes.Ldloc_1);
                il.Emit(OpCodes.Bne_Un_S, startLabel);
                il.Emit(OpCodes.Ret);

                @event.SetRemoveOnMethod(method);
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
        public static MethodBuilder DefineNotImplementedMethodOverride(
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

            var returnParam = method.DefineParameter(0,
                baseReturnParam.Attributes, baseReturnParam.Name);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = method.DefineParameter(i + 1,
                    baseParameter.Attributes, baseParameter.Name);
            }

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Newobj,
                typeof(NotImplementedException).GetConstructor(Type.EmptyTypes));
            il.Emit(OpCodes.Throw);

            return method;
        }
    }
}
