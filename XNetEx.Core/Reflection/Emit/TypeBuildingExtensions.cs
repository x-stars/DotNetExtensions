using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace XNetEx.Reflection.Emit
{
    /// <summary>
    /// 提供运行时类型生成相关的扩展方法。
    /// </summary>
    public static class TypeBuildingExtensions
    {
        /// <summary>
        /// 提供当前类型使用的反射元数据的 <see cref="MemberInfo"/> 对象。
        /// </summary>
        private static class ReflectionData
        {
            /// <summary>
            /// 表示 <see cref="Delegate.Combine(Delegate, Delegate)"/>
            /// 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo DelegateCombineMethod =
                typeof(Delegate).GetMethod(nameof(Delegate.Combine),
                    new[] { typeof(Delegate), typeof(Delegate) })!;

            /// <summary>
            /// 表示 <see cref="Delegate.Remove(Delegate, Delegate)"/>
            /// 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo DelegateRemoveMethod =
                typeof(Delegate).GetMethod(nameof(Delegate.Remove),
                    new[] { typeof(Delegate), typeof(Delegate) })!;

            /// <summary>
            /// 表示 <see cref="Interlocked.CompareExchange{T}(ref T, T, T)"/>
            /// 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo CompareExchangeMethod =
                typeof(Interlocked).GetMethods().Where(
                    method => method.Name == nameof(Interlocked.CompareExchange) &&
                    method.IsGenericMethod).Single();
        }

        /// <summary>
        /// 确定当前 <see cref="MethodBase"/> 是否为程序集外部可继承的实例方法。
        /// </summary>
        /// <param name="method">要进行检查的 <see cref="MethodBase"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 是程序集外部可继承的实例方法，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static bool IsInheritable(this MethodBase method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.DeclaringType!.IsVisible && !method.DeclaringType.IsSealed &&
                !method.IsStatic && (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly);
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

            return method.IsInheritable() && (method.IsVirtual && !method.IsFinal);
        }

        /// <summary>
        /// 以指定的构造函数为基础，定义构造函数，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义构造函数的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseConstructor">作为基础的构造函数。</param>
        /// <returns>定义的构造函数，仅包括构造函数定义，不包括任何实现。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseConstructor"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static ConstructorBuilder DefineConstructorLike(
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
            if (!baseConstructor.IsInheritable())
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseConstructor), inner);
            }

            var baseParameters = baseConstructor.GetParameters();

            var constructor = type.DefineConstructor(
                baseConstructor.Attributes, baseConstructor.CallingConvention,
                Array.ConvertAll(baseParameters, param => param.ParameterType),
                Array.ConvertAll(baseParameters, param => param.GetRequiredCustomModifiers()),
                Array.ConvertAll(baseParameters, param => param.GetOptionalCustomModifiers()));

            foreach (var index in ..baseParameters.Length)
            {
                var baseParameter = baseParameters[index];
                var parameter = constructor.DefineParameter(index + 1,
                    baseParameter.Attributes, baseParameter.Name);
            }

            return constructor;
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
        public static ConstructorBuilder DefineBaseInvokeConstructorLike(
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
            if (!baseConstructor.IsInheritable())
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseConstructor), inner);
            }

            var constructor = type.DefineConstructorLike(baseConstructor);

            var ilGen = constructor.GetILGenerator();
            ilGen.Emit(OpCodes.Ldarg_0);
            foreach (var index in ..baseConstructor.GetParameters().Length)
            {
                ilGen.EmitLdarg(index + 1);
            }
            ilGen.Emit(OpCodes.Call, baseConstructor);
            ilGen.Emit(OpCodes.Ret);

            return constructor;
        }

        /// <summary>
        /// 以指定的方法为基础，定义重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的方法，仅包括方法定义，不包括任何实现。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static MethodBuilder DefineMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod, bool explicitOverride = false)
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
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseMethod), inner);
            }

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var methodName = baseMethod.Name;
            if (explicitOverride)
            {
                var baseHandle = baseMethod.MethodHandle;
                methodName += $"#{baseHandle.Value.ToString()}";
            }
            var attributes = baseMethod.Attributes;
            attributes &= ~MethodAttributes.Abstract;
            if (!baseMethod.DeclaringType!.IsInterface)
            {
                attributes &= ~MethodAttributes.NewSlot;
            }
            if (explicitOverride)
            {
                attributes &= ~MethodAttributes.MemberAccessMask;
                attributes |= MethodAttributes.Private;
            }

            var method = type.DefineMethod(methodName,
                attributes, baseMethod.CallingConvention,
                baseReturnParam.ParameterType,
                baseReturnParam.GetRequiredCustomModifiers(),
                baseReturnParam.GetOptionalCustomModifiers(),
                Array.ConvertAll(baseParameters, param => param.ParameterType),
                Array.ConvertAll(baseParameters, param => param.GetRequiredCustomModifiers()),
                Array.ConvertAll(baseParameters, param => param.GetOptionalCustomModifiers()));

            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                method.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));

            var returnParam = method.DefineParameter(0,
                baseReturnParam.Attributes, baseReturnParam.Name);
            foreach (var index in ..baseParameters.Length)
            {
                var baseParameter = baseParameters[index];
                var parameter = method.DefineParameter(index + 1,
                    baseParameter.Attributes, baseParameter.Name);
            }

            if (explicitOverride)
            {
                type.DefineMethodOverride(method, baseMethod);
            }

            return method;
        }

        /// <summary>
        /// 以指定的方法为基础，定义抛出未实现异常的重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的方法，抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static MethodBuilder DefineNotImplementedMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod, bool explicitOverride = false)
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
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseMethod), inner);
            }

            var method = type.DefineMethodOverride(baseMethod, explicitOverride);

            var ilGen = method.GetILGenerator();
            ilGen.EmitNotImplemented();

            return method;
        }

        /// <summary>
        /// 以指定的属性为基础，定义重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义完成的属性，仅包括属性定义，不包括任何访问器及其实现。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 的方法无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static PropertyBuilder DefinePropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty, bool explicitOverride = false)
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
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }

            var propertyName = baseProperty.Name;
            var baseParameters = baseProperty.GetIndexParameters();

            if (explicitOverride)
            {
                var baseHandle = baseProperty.GetAccessors().First().MethodHandle;
                propertyName += $"#{baseHandle.Value.ToString()}";
            }

            var property = type.DefineProperty(
                propertyName, baseProperty.Attributes, baseProperty.PropertyType,
                baseProperty.GetRequiredCustomModifiers(), baseProperty.GetOptionalCustomModifiers(),
                Array.ConvertAll(baseParameters, param => param.ParameterType),
                Array.ConvertAll(baseParameters, param => param.GetRequiredCustomModifiers()),
                Array.ConvertAll(baseParameters, param => param.GetOptionalCustomModifiers()));

            return property;
        }

        /// <summary>
        /// 以指定的属性为基础，定义抛出未实现异常的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义完成的属性，抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 的方法无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static PropertyBuilder DefineNotImplementedPropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty, bool explicitOverride = false)
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
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }

            var property = type.DefinePropertyOverride(baseProperty, explicitOverride);

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod!;
                var method = type.DefineNotImplementedMethodOverride(baseMethod, explicitOverride);
                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod!;
                var method = type.DefineNotImplementedMethodOverride(baseMethod, explicitOverride);
                property.SetSetMethod(method);
            }

            return property;
        }

        /// <summary>
        /// 以指定的属性为基础，定义以自动属性模式实现的重写属性，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义属性的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseProperty">作为基础的属性。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的自动属性与其对应的字段。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseProperty"/> 是索引属性或无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static (PropertyBuilder Property, FieldBuilder Field) DefineAutoPropertyOverride(
            this TypeBuilder type, PropertyInfo baseProperty, bool explicitOverride = false)
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
                var inner = new TargetParameterCountException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }
            if (!baseProperty.GetAccessors().All(accessor => accessor.IsOverridable()))
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseProperty), inner);
            }

            var property = type.DefinePropertyOverride(baseProperty, explicitOverride);

            var field = type.DefineField(
                property.Name, baseProperty.PropertyType, FieldAttributes.Private);

            if (baseProperty.CanRead)
            {
                var baseMethod = baseProperty.GetMethod!;

                var method = type.DefineMethodOverride(baseMethod, explicitOverride);

                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Ret);

                property.SetGetMethod(method);
            }

            if (baseProperty.CanWrite)
            {
                var baseMethod = baseProperty.SetMethod!;

                var method = type.DefineMethodOverride(baseMethod, explicitOverride);

                var ilGen = method.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Stfld, field);
                ilGen.Emit(OpCodes.Ret);

                property.SetSetMethod(method);
            }

            return (property, field);
        }

        /// <summary>
        /// 以指定的事件为基础，定义抛出未实现异常的重写事件，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义事件的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseEvent">作为基础的事件。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的事件，抛出 <see cref="NotImplementedException"/> 异常。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseEvent"/> 的方法无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static EventBuilder DefineNotImplementedEventOverride(
            this TypeBuilder type, EventInfo baseEvent, bool explicitOverride = false)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseEvent is null)
            {
                throw new ArgumentNullException(nameof(baseEvent));
            }
            if (!baseEvent.AddMethod!.IsOverridable())
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseEvent), inner);
            }

            var eventName = baseEvent.Name;
            if (explicitOverride)
            {
                var baseHandle = baseEvent.AddMethod!.MethodHandle;
                eventName += $"#{baseHandle.Value.ToString()}";
            }

            var @event = type.DefineEvent(
                eventName, baseEvent.Attributes, baseEvent.EventHandlerType!);

            {
                var baseMethod = baseEvent.AddMethod!;
                var method = type.DefineNotImplementedMethodOverride(baseMethod, explicitOverride);
                @event.SetAddOnMethod(method);
            }

            {
                var baseMethod = baseEvent.RemoveMethod!;
                var method = type.DefineNotImplementedMethodOverride(baseMethod, explicitOverride);
                @event.SetRemoveOnMethod(method);
            }

            return @event;
        }

        /// <summary>
        /// 以指定的事件为基础，定义以默认事件模式实现的重写事件，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义事件的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseEvent">作为基础的事件。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的默认模式的事件与其对应的事件委托。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseEvent"/> 的方法无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        public static (EventBuilder Event, FieldBuilder Field) DefineDefaultEventOverride(
            this TypeBuilder type, EventInfo baseEvent, bool explicitOverride = false)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseEvent is null)
            {
                throw new ArgumentNullException(nameof(baseEvent));
            }
            if (!baseEvent.AddMethod!.IsOverridable())
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseEvent), inner);
            }

            var eventName = baseEvent.Name;
            if (explicitOverride)
            {
                var baseHandle = baseEvent.AddMethod!.MethodHandle;
                eventName += $"#{baseHandle.Value.ToString()}";
            }

            var @event = type.DefineEvent(
                eventName, baseEvent.Attributes, baseEvent.EventHandlerType!);

            var field = type.DefineField(
                eventName, baseEvent.EventHandlerType!, FieldAttributes.Private);

            {
                var baseMethod = baseEvent.AddMethod!;

                var method = type.DefineMethodOverride(baseMethod, explicitOverride);

                var ilGen = method.GetILGenerator();
                var eventType = baseEvent.EventHandlerType!;
                var local0 = ilGen.DeclareLocal(eventType);
                var local1 = ilGen.DeclareLocal(eventType);
                var local2 = ilGen.DeclareLocal(eventType);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Stloc_0);
                var startLabel = ilGen.DefineLabel();
                ilGen.MarkLabel(startLabel);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Stloc_1);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Call, ReflectionData.DelegateCombineMethod);
                ilGen.Emit(OpCodes.Castclass, eventType);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, field);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call,
                    ReflectionData.CompareExchangeMethod.MakeGenericMethod(eventType));
                ilGen.Emit(OpCodes.Stloc_0);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Bne_Un_S, startLabel);
                ilGen.Emit(OpCodes.Ret);

                @event.SetAddOnMethod(method);
            }

            {
                var baseMethod = baseEvent.RemoveMethod!;

                var method = type.DefineMethodOverride(baseMethod, explicitOverride);

                var ilGen = method.GetILGenerator();
                var eventType = baseEvent.EventHandlerType!;
                var local0 = ilGen.DeclareLocal(eventType);
                var local1 = ilGen.DeclareLocal(eventType);
                var local2 = ilGen.DeclareLocal(eventType);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
                ilGen.Emit(OpCodes.Stloc_0);
                var startLabel = ilGen.DefineLabel();
                ilGen.MarkLabel(startLabel);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Stloc_1);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Call, ReflectionData.DelegateRemoveMethod);
                ilGen.Emit(OpCodes.Castclass, eventType);
                ilGen.Emit(OpCodes.Stloc_2);
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldflda, field);
                ilGen.Emit(OpCodes.Ldloc_2);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Call,
                    ReflectionData.CompareExchangeMethod.MakeGenericMethod(eventType));
                ilGen.Emit(OpCodes.Stloc_0);
                ilGen.Emit(OpCodes.Ldloc_0);
                ilGen.Emit(OpCodes.Ldloc_1);
                ilGen.Emit(OpCodes.Bne_Un_S, startLabel);
                ilGen.Emit(OpCodes.Ret);

                @event.SetRemoveOnMethod(method);
            }

            return (@event, field);
        }
    }
}
