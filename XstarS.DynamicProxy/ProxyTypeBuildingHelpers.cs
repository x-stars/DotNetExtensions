using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection.Emit
{
    /// <summary>
    /// 提供直接代理类型运行时类型生成相关的帮助方法。
    /// </summary>
    internal static class ProxyTypeBuildingHelpers
    {
        /// <summary>
        /// 确定当前 <see cref="MemberInfo"/> 是否应按照代理模式重写。
        /// </summary>
        /// <param name="method">要确定是否按照代理模式重写的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 应按照代理模式重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        internal static bool IsProxyOverride(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.IsOverridable() &&
                !method.ReturnParameter.ParameterType.IsNotILBoxable() &&
                Array.TrueForAll(
                    Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                    type => !type.IsNotILBoxable());
        }

        /// <summary>
        /// 确定当前 <see cref="MemberInfo"/> 是否应按照非代理模式重写。
        /// </summary>
        /// <param name="method">要确定是否按照非代理模式重写的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 应按照非代理模式重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        internal static bool IsNonProxyOverride(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.IsOverridable() && method.IsAbstract && !method.IsProxyOverride();
        }

        /// <summary>
        /// 以指定的泛型参数列表为基础，设定当前泛型参数列表的泛型约束。
        /// </summary>
        /// <param name="genericParams">
        /// 要设定泛型约束的 <see cref="GenericTypeParameterBuilder"/> 数组。</param>
        /// <param name="baseGenericParams">作为基础的泛型参数列表。</param>
        /// <param name="baseTypeGenericArgs">定义基础泛型参数列表的方法的构造泛型类型的泛型参数列表。</param>
        /// <exception cref="ArgumentException"><paramref name="baseGenericParams"/>
        /// 不为泛型参数列表，或与 <paramref name="genericParams"/> 的长度不等。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static void SetGenericConstraintsLike(
            this GenericTypeParameterBuilder[] genericParams,
            Type[] baseGenericParams, Type[] baseTypeGenericArgs)
        {
            if (genericParams is null)
            {
                throw new ArgumentNullException(nameof(genericParams));
            }
            if (baseGenericParams is null)
            {
                throw new ArgumentNullException(nameof(baseGenericParams));
            }
            if (baseTypeGenericArgs is null)
            {
                throw new ArgumentNullException(nameof(baseTypeGenericArgs));
            }
            if (baseGenericParams.Length != genericParams.Length)
            {
                var inner = new TargetParameterCountException();
                throw new ArgumentException(inner.Message, nameof(baseGenericParams), inner);
            }
            if (baseGenericParams.Any(type => !type.IsGenericParameter))
            {
                var inner = new InvalidOperationException();
                throw new ArgumentException(inner.Message, nameof(baseGenericParams), inner);
            }

            Type MakeConstraint(Type constraintType)
            {
                if (constraintType.IsGenericParameter)
                {
                    var position = constraintType.GenericParameterPosition;
                    var isInBase = Array.IndexOf(baseGenericParams, constraintType) != -1;
                    return isInBase ? genericParams[position] : baseTypeGenericArgs[position];
                }
                else if (constraintType.IsGenericType)
                {
                    var typeGenericDefinition = constraintType.GetGenericTypeDefinition();
                    var typeGenericArguments = constraintType.GetGenericArguments();
                    for (int index = 0; index < typeGenericArguments.Length; index++)
                    {
                        typeGenericArguments[index] = MakeConstraint(typeGenericArguments[index]);
                    }
                    return typeGenericDefinition.MakeGenericType(typeGenericArguments);
                }
                else
                {
                    return constraintType;
                }
            }

            for (int index = 0; index < genericParams.Length; index++)
            {
                var genericParam = genericParams[index];
                var baseGenericParam = baseGenericParams[index];

                var baseGenericConstraints = baseGenericParam.GetGenericParameterConstraints();

                baseGenericConstraints = Array.ConvertAll(baseGenericConstraints, MakeConstraint);

                var baseTypeConstraint = baseGenericConstraints.Where(
                    genericConstraint => genericConstraint.IsClass).SingleOrDefault();
                var interfaceConstraints = baseGenericConstraints.Where(
                    genericConstraint => genericConstraint.IsInterface).ToArray();

                genericParam.SetGenericParameterAttributes(
                    baseGenericParam.GenericParameterAttributes);
                if (!(baseTypeConstraint is null))
                {
                    genericParam.SetBaseTypeConstraint(baseTypeConstraint);
                }
                if (interfaceConstraints.Length != 0)
                {
                    genericParam.SetInterfaceConstraints(interfaceConstraints);
                }
            }
        }

        /// <summary>
        /// 以指定的方法为基础，定义基类方法的 <see cref="MethodInfo"/>
        /// 和 <see cref="MethodDelegate"/> 字段，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="baseType">定义基础方法的类型；若为泛型类型，则应为构造泛型类型。</param>
        /// <param name="instanceField">代理对象的字段；<see langword="null"/> 表示代理对象为当前实例。</param>
        /// <returns>定义的基类方法的 <see cref="MethodInfo"/> 和 <see cref="MethodDelegate"/> 字段</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static KeyValuePair<FieldBuilder, FieldBuilder> DefineBaseMethodInfoAndDelegateField(
            this TypeBuilder type, MethodInfo baseMethod, Type baseType, FieldInfo instanceField = null)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (baseType is null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }
            if (!baseMethod.IsProxyOverride())
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseMethod), inner);
            }

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var nestedType = type.DefineNestedType(
                $"@{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                TypeAttributes.Class | TypeAttributes.NestedPrivate |
                TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                nestedType.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));
            genericParams.SetGenericConstraintsLike(baseGenericParams, baseType.GetGenericArguments());

            var delegateMethod = nestedType.DefineMethod(nameof(MethodDelegate.Invoke),
                MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(object), new[] { typeof(object), typeof(object[]) });
            {
                delegateMethod.DefineParameter(1, ParameterAttributes.None, "instance");
                delegateMethod.DefineParameter(2, ParameterAttributes.None, "arguments");

                var il = delegateMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.EmitUnbox(baseType);
                for (int pIndex = 0; pIndex < baseParameters.Length; pIndex++)
                {
                    var baseParameter = baseParameters[pIndex];
                    int gIndex = Array.IndexOf(
                        baseGenericParams, baseParameter.ParameterType);
                    var parameterType = (gIndex == -1) ?
                        baseParameter.ParameterType : genericParams[gIndex];
                    il.Emit(OpCodes.Ldarg_1);
                    il.EmitLdcI4(pIndex);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.EmitUnbox(parameterType);
                }
                il.Emit((instanceField is null) ? OpCodes.Call : OpCodes.Callvirt,
                    !baseMethod.IsGenericMethod ? baseMethod :
                        baseMethod.MakeGenericMethod(nestedType.GetGenericArguments()));
                if (baseMethod.ReturnType != typeof(void))
                {
                    int gIndex = Array.IndexOf(
                        baseGenericParams, baseReturnParam.ParameterType);
                    var returnType = (gIndex == -1) ?
                        baseReturnParam.ParameterType : genericParams[gIndex];
                    il.EmitBox(returnType);
                }
                else
                {
                    il.Emit(OpCodes.Ldnull);
                }
                il.Emit(OpCodes.Ret);
            }

            var infoField = nestedType.DefineField(nameof(MethodInfo), typeof(MethodInfo),
                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);

            var delegateField = nestedType.DefineField(nameof(MethodDelegate), typeof(MethodDelegate),
                FieldAttributes.Assembly | FieldAttributes.Static | FieldAttributes.InitOnly);

            var constructor = nestedType.DefineTypeInitializer();
            {
                var il = constructor.GetILGenerator();
                il.Emit(OpCodes.Ldtoken,
                    !baseMethod.IsGenericMethod ? baseMethod :
                        baseMethod.MakeGenericMethod(nestedType.GetGenericArguments()));
                il.Emit(OpCodes.Ldtoken, baseMethod.DeclaringType!);
                il.Emit(OpCodes.Call,
                    typeof(MethodBase).GetMethod(
                        nameof(MethodBase.GetMethodFromHandle),
                        new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) })!);
                il.Emit(OpCodes.Castclass, typeof(MethodInfo));
                il.Emit(OpCodes.Stsfld, infoField);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Ldftn, delegateMethod);
                il.Emit(OpCodes.Newobj, typeof(MethodDelegate).GetConstructors()[0]);
                il.Emit(OpCodes.Stsfld, delegateField);
                il.Emit(OpCodes.Ret);
            }

            nestedType.CreateTypeInfo();

            return new KeyValuePair<FieldBuilder, FieldBuilder>(infoField, delegateField);
        }

        /// <summary>
        /// 以指定的方法为基础，定义调用指定代理委托的重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="baseType">定义基础方法的类型；若为泛型类型，则应为构造泛型类型。</param>
        /// <param name="baseMethodInfoField">基础方法的 <see cref="MethodInfo"/> 字段。</param>
        /// <param name="baseMethodDelegateField">基础方法的 <see cref="MethodDelegate"/> 字段。</param>
        /// <param name="methodInvokeHandlerField">当前类型的代理委托字段。</param>
        /// <param name="instanceField">代理对象的字段；<see langword="null"/> 表示代理对象为当前实例。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的方法，调用 <paramref name="methodInvokeHandlerField"/> 字段的代理委托。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineProxyMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod, Type baseType,
            FieldInfo baseMethodInfoField, FieldInfo baseMethodDelegateField,
            FieldInfo methodInvokeHandlerField,
            FieldInfo instanceField = null, bool explicitOverride = false)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (baseType is null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }
            if (baseMethodInfoField is null)
            {
                throw new ArgumentNullException(nameof(baseMethodInfoField));
            }
            if (baseMethodDelegateField is null)
            {
                throw new ArgumentNullException(nameof(baseMethodDelegateField));
            }
            if (methodInvokeHandlerField is null)
            {
                throw new ArgumentNullException(nameof(methodInvokeHandlerField));
            }
            if (!baseMethod.IsProxyOverride())
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseMethod), inner);
            }

            var method = type.DefineMethodOverride(baseMethod, explicitOverride);

            if (baseMethod.IsGenericMethod)
            {
                baseMethodDelegateField = TypeBuilder.GetField(
                    baseMethodDelegateField.DeclaringType!.MakeGenericType(
                        method.GetGenericArguments()), baseMethodDelegateField);
                baseMethodInfoField = TypeBuilder.GetField(
                    baseMethodInfoField.DeclaringType!.MakeGenericType(
                        method.GetGenericArguments()), baseMethodInfoField);
            }

            var baseHasGenericConstraints = baseMethod.GetGenericArguments().Any(
                param => param.GetGenericParameterConstraints().Length != 0);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, methodInvokeHandlerField);
            il.Emit(OpCodes.Ldarg_0);
            if (!(instanceField is null))
            {
                il.Emit(OpCodes.Ldfld, instanceField);
            }
            il.EmitBox(baseType);
            if (baseHasGenericConstraints)
            {
                il.Emit(OpCodes.Ldtoken, baseMethodInfoField);
                il.Emit(OpCodes.Ldtoken, baseMethodInfoField.DeclaringType!);
                il.Emit(OpCodes.Call,
                    typeof(FieldInfo).GetMethod(
                        nameof(FieldInfo.GetFieldFromHandle),
                        new[] { typeof(RuntimeFieldHandle), typeof(RuntimeTypeHandle) })!);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetValue))!);
            }
            else
            {
                il.Emit(OpCodes.Ldsfld, baseMethodInfoField);
            }
            if (baseHasGenericConstraints)
            {
                il.Emit(OpCodes.Ldtoken, baseMethodDelegateField);
                il.Emit(OpCodes.Ldtoken, baseMethodDelegateField.DeclaringType!);
                il.Emit(OpCodes.Call,
                    typeof(FieldInfo).GetMethod(
                        nameof(FieldInfo.GetFieldFromHandle),
                        new[] { typeof(RuntimeFieldHandle), typeof(RuntimeTypeHandle) })!);
                il.Emit(OpCodes.Ldnull);
                il.Emit(OpCodes.Callvirt, typeof(FieldInfo).GetMethod(nameof(FieldInfo.GetValue))!);
            }
            else
            {
                il.Emit(OpCodes.Ldsfld, baseMethodDelegateField);
            }
            var baseParameters = baseMethod.GetParameters();
            il.EmitLdcI4(baseParameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            for (int index = 0; index < baseParameters.Length; index++)
            {
                var baseParameter = baseParameters[index];
                il.Emit(OpCodes.Dup);
                il.EmitLdcI4(index);
                il.EmitLdarg(index + 1);
                il.EmitBox(baseParameter.ParameterType);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Callvirt,
                typeof(MethodInvokeHandler).GetMethod(nameof(MethodInvokeHandler.Invoke))!);
            if (baseMethod.ReturnType != typeof(void))
            {
                il.EmitUnbox(baseMethod.ReturnType);
            }
            else
            {
                il.Emit(OpCodes.Pop);
            }
            il.Emit(OpCodes.Ret);

            return method;
        }

        /// <summary>
        /// 以指定的方法为基础，定义调用代理对象方法的重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="instanceField">代理对象的字段；<see langword="null"/> 表示代理对象为当前实例。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的方法，调用代理对象的 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的声明类型不为接口。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineBaseInvokeMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod,
            FieldInfo instanceField = null, bool explicitOverride = false)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (!baseMethod.IsProxyOverride())
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseMethod), inner);
            }

            var method = type.DefineMethodOverride(baseMethod, explicitOverride);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            if (!(instanceField is null))
            {
                il.Emit(OpCodes.Ldfld, instanceField);
            }
            for (int index = 0; index < baseMethod.GetParameters().Length; index++)
            {
                il.EmitLdarg(index + 1);
            }
            il.Emit((instanceField is null) ? OpCodes.Call : OpCodes.Callvirt,
                (baseMethod.GetGenericArguments().Length == 0) ? baseMethod :
                    baseMethod.MakeGenericMethod(method.GetGenericArguments()));
            il.Emit(OpCodes.Ret);

            type.DefineMethodOverride(method, baseMethod);

            return method;
        }
    }
}
