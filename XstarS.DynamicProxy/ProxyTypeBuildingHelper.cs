using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供代理类型运行时类型生成相关的帮助方法。
    /// </summary>
    internal static class ProxyTypeBuildingHelper
    {
        /// <summary>
        /// 确定当前 <see cref="MemberInfo"/> 是否应由 <see cref="ProxyTypeProvider"/> 按照代理模式重写。
        /// </summary>
        /// <param name="method">要确定是否按照代理模式重写的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 应由 <see cref="ProxyTypeProvider"/> 按照代理模式重写，
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
                (!method.IsGenericMethod || (method.IsGenericMethod &&
                Array.TrueForAll(
                    method.GetGenericArguments(),
                    type => type.GetGenericParameterConstraints().Length == 0))) &&
                !method.ReturnParameter.ParameterType.IsNotILBoxable() &&
                Array.TrueForAll(
                    Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                    type => !type.IsNotILBoxable());
        }

        /// <summary>
        /// 确定当前 <see cref="MemberInfo"/> 是否应由 <see cref="ProxyTypeProvider"/> 按照非代理模式重写。
        /// </summary>
        /// <param name="method">要确定是否按照非代理模式重写的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 应由 <see cref="ProxyTypeProvider"/> 按照非代理模式重写，
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
        /// 以指定的泛型参数为基础，设定当前泛型参数的泛型约束。
        /// </summary>
        /// <param name="genericParam">
        /// 要设定泛型约束的 <see cref="GenericTypeParameterBuilder"/> 对象。</param>
        /// <param name="baseGenericParam">作为基础的泛型参数。</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseGenericParam"/> 不为泛型参数。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static void SetGenericConstraintsAs(
            this GenericTypeParameterBuilder genericParam, Type baseGenericParam)
        {
            if (genericParam is null)
            {
                throw new ArgumentNullException(nameof(genericParam));
            }
            if (baseGenericParam is null)
            {
                throw new ArgumentNullException(nameof(baseGenericParam));
            }
            if (!baseGenericParam.IsGenericParameter)
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseGenericParam));
            }

            var baseGenericConstraints = baseGenericParam.GetGenericParameterConstraints();
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

        /// <summary>
        /// 以指定的方法为基础，定义新方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <returns>定义的方法，仅包括方法定义，不包括任何实现。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineMethodLike(
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
            if (!baseMethod.IsInheritable())
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseMethod));
            }

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var method = type.DefineMethod(
                $"@{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                MethodAttributes.Private | MethodAttributes.HideBySig,
                baseMethod.CallingConvention, baseReturnParam.ParameterType,
                Array.ConvertAll(baseParameters, param => param.ParameterType));

            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                method.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var baseGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                genericParam.SetGenericConstraintsAs(baseGenericParam);
            }

            var returnParam = method.DefineParameter(0,
                baseReturnParam.Attributes, baseReturnParam.Name);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = method.DefineParameter(i + 1,
                    baseParameter.Attributes, baseParameter.Name);
            }

            return method;
        }

        /// <summary>
        /// 以指定的方法为基础，定义调用基类方法的新方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <returns>定义的方法，调用 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineBaseInvokeMethodLike(
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
            if (!baseMethod.IsProxyOverride())
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseMethod));
            }

            var method = type.DefineMethodLike(baseMethod);

            var il = method.GetILGenerator();
            if (!baseMethod.IsAbstract)
            {
                il.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseMethod.GetParameters().Length; i++)
                {
                    il.EmitLdarg(i + 1);
                }
                il.Emit(OpCodes.Call,
                    (baseMethod.GetGenericArguments().Length == 0) ? baseMethod :
                    baseMethod.MakeGenericMethod(method.GetGenericArguments()));
                il.Emit(OpCodes.Ret);
            }
            else
            {
                il.Emit(OpCodes.Newobj,
                    typeof(NotImplementedException).GetConstructor(Type.EmptyTypes));
                il.Emit(OpCodes.Throw);
            }

            return method;
        }

        /// <summary>
        /// 以指定的方法为基础，定义基类方法的 <see cref="MethodInfo"/>
        /// 和 <see cref="MethodDelegate"/> 字段，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="baseInvokeMethod">调用基础方法的当前类型的方法。</param>
        /// <returns>定义的基类方法的 <see cref="MethodInfo"/> 和 <see cref="MethodDelegate"/> 字段</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static KeyValuePair<FieldBuilder, FieldBuilder> DefineMethodInfoAndDelegateField(
            this TypeBuilder type, MethodInfo baseMethod, MethodInfo baseInvokeMethod)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (baseInvokeMethod is null)
            {
                throw new ArgumentNullException(nameof(baseInvokeMethod));
            }
            if (!baseMethod.IsProxyOverride())
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseMethod));
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
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var baseGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                genericParam.SetGenericConstraintsAs(baseGenericParam);
            }

            var delegateMethod = nestedType.DefineMethod(nameof(MethodDelegate.Invoke),
                MethodAttributes.Assembly | MethodAttributes.Static | MethodAttributes.HideBySig,
                typeof(object), new[] { typeof(object), typeof(object[]) });
            {
                delegateMethod.DefineParameter(1, ParameterAttributes.None, "instance");
                delegateMethod.DefineParameter(2, ParameterAttributes.None, "arguments");

                var il = delegateMethod.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.EmitUnbox(type);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var baseParameter = baseParameters[i];
                    int index = Array.IndexOf(
                        baseGenericParams, baseParameter.ParameterType);
                    var parameterType = (index == -1) ?
                        baseParameter.ParameterType : genericParams[index];
                    il.Emit(OpCodes.Ldarg_1);
                    il.EmitLdcI4(i);
                    il.Emit(OpCodes.Ldelem_Ref);
                    il.EmitUnbox(parameterType);
                }
                il.Emit(OpCodes.Call,
                    !baseInvokeMethod.IsGenericMethod ? baseInvokeMethod :
                    baseInvokeMethod.MakeGenericMethod(nestedType.GetGenericArguments()));
                if (baseMethod.ReturnType != typeof(void))
                {
                    int index = Array.IndexOf(
                        baseGenericParams, baseReturnParam.ParameterType);
                    var returnType = (index == -1) ?
                        baseReturnParam.ParameterType : genericParams[index];
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
                il.Emit(OpCodes.Ldtoken, baseMethod.DeclaringType);
                il.Emit(OpCodes.Call, typeof(MethodBase).GetMethod(
                    nameof(MethodBase.GetMethodFromHandle),
                    new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) }));
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
        /// <param name="baseMethodInfoField">基础方法的 <see cref="MethodInfo"/> 字段。</param>
        /// <param name="baseMethodDelegateField">基础方法的 <see cref="MethodDelegate"/> 字段。</param>
        /// <param name="methodInvokeHandlerField">当前类型的代理委托字段。</param>
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的方法，调用 <paramref name="methodInvokeHandlerField"/> 字段的代理委托。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineProxyMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod,
            FieldInfo baseMethodInfoField, FieldInfo baseMethodDelegateField,
            FieldInfo methodInvokeHandlerField, bool explicitOverride = false)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
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
                throw new ArgumentException(new ArgumentException().Message, nameof(baseMethod));
            }

            var method = type.DefineMethodOverride(baseMethod, explicitOverride);

            if (baseMethod.IsGenericMethod)
            {
                baseMethodDelegateField = TypeBuilder.GetField(
                    baseMethodDelegateField.DeclaringType.MakeGenericType(
                        method.GetGenericArguments()), baseMethodDelegateField);
                baseMethodInfoField = TypeBuilder.GetField(
                    baseMethodInfoField.DeclaringType.MakeGenericType(
                        method.GetGenericArguments()), baseMethodInfoField);
            }

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, methodInvokeHandlerField);
            il.Emit(OpCodes.Ldarg_0);
            il.EmitBox(type);
            il.Emit(OpCodes.Ldsfld, baseMethodInfoField);
            var baseParameters = baseMethod.GetParameters();
            il.EmitLdcI4(baseParameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                il.Emit(OpCodes.Dup);
                il.EmitLdcI4(i);
                il.EmitLdarg(i + 1);
                il.EmitBox(baseParameter.ParameterType);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Ldsfld, baseMethodDelegateField);
            il.Emit(OpCodes.Call,
                typeof(MethodInvokeHandler).GetMethod(nameof(MethodInvokeHandler.Invoke)));
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
    }
}
