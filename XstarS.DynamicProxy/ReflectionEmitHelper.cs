using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供反射发出相关的帮助方法。
    /// </summary>
    internal static class ReflectionEmitHelper
    {
        /// <summary>
        /// 发出将指定索引处的参数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="il">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="position">要加载到计算堆栈的参数的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="il"/> 为 <see langword="null"/>。</exception>
        internal static void EmitLdarg(this ILGenerator il, int position)
        {
            if (il is null)
            {
                throw new ArgumentNullException(nameof(il));
            }

            switch (position)
            {
                case 0: il.Emit(OpCodes.Ldarg_0); break;
                case 1: il.Emit(OpCodes.Ldarg_1); break;
                case 2: il.Emit(OpCodes.Ldarg_2); break;
                case 3: il.Emit(OpCodes.Ldarg_3); break;
                default:
                    il.Emit((position <= byte.MaxValue) ?
                        OpCodes.Ldarg_S : OpCodes.Ldarg, position);
                    break;
            }
        }

        /// <summary>
        /// 发出将指定 32 位有符号整数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="il">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="value">要加载到计算堆栈的 32 位有符号整数的值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="il"/> 为 <see langword="null"/>。</exception>
        internal static void EmitLdcI4(this ILGenerator il, int value)
        {
            if (il is null)
            {
                throw new ArgumentNullException(nameof(il));
            }

            switch (value)
            {
                case -1: il.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: il.Emit(OpCodes.Ldc_I4_0); break;
                case 1: il.Emit(OpCodes.Ldc_I4_1); break;
                case 2: il.Emit(OpCodes.Ldc_I4_2); break;
                case 3: il.Emit(OpCodes.Ldc_I4_3); break;
                case 4: il.Emit(OpCodes.Ldc_I4_4); break;
                case 5: il.Emit(OpCodes.Ldc_I4_5); break;
                case 6: il.Emit(OpCodes.Ldc_I4_6); break;
                case 7: il.Emit(OpCodes.Ldc_I4_7); break;
                case 8: il.Emit(OpCodes.Ldc_I4_8); break;
                default:
                    il.Emit((value <= byte.MaxValue) ?
                        OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, value);
                    break;
            }
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
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseGenericParam));
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
        /// 以指定的方法为基础，定义调用基类方法的新方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <returns>定义的新方法，调用 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineBaseInvokeMethod(
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
            if (!baseMethod.IsInheritableInstance())
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseMethod));
            }

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var method = type.DefineMethod($"@{baseMethod.Name}",
                MethodAttributes.Assembly | MethodAttributes.HideBySig,
                baseReturnParam.ParameterType,
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

            var il = method.GetILGenerator();
            if (!baseMethod.IsAbstract)
            {
                il.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    il.EmitLdarg(i + 1);
                }
                il.Emit(OpCodes.Call,
                    (baseGenericParams.Length == 0) ? baseMethod :
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
        /// <returns>定义的基类方法的 <see cref="MethodInfo"/>
        /// 和 <see cref="MethodDelegate"/> 字段</returns>
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
            if (!baseMethod.IsOverridable())
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseMethod));
            }

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var nestedType = type.DefineNestedType(
                $"@{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                TypeAttributes.Class | TypeAttributes.NestedAssembly |
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
                il.Emit(OpCodes.Castclass, type);
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
                    il.Emit(OpCodes.Unbox_Any, parameterType);
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
                    il.Emit(OpCodes.Box, returnType);
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

        /// <summary>
        /// 以指定的方法为基础，定义调用指定代理委托的方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="baseMethodInfoField">基础方法的 <see cref="MethodInfo"/> 字段。</param>
        /// <param name="baseMethodDelegateField">基础方法的 <see cref="MethodDelegate"/> 字段。</param>
        /// <param name="methodInvokeHandlerField">当前类型的代理委托字段。</param>
        /// <returns>定义的方法，调用 <paramref name="methodInvokeHandlerField"/> 字段的代理委托。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 无法在程序集外部重写。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineProxyMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod,
            FieldInfo baseMethodInfoField, FieldInfo baseMethodDelegateField,
            FieldInfo methodInvokeHandlerField)
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
                var methodGenericParam = baseGenericParams[i];
                var genericParam = genericParams[i];
                genericParam.SetGenericParameterAttributes(
                    methodGenericParam.GenericParameterAttributes);
            }

            if (baseMethod.IsGenericMethod)
            {
                baseMethodDelegateField = TypeBuilder.GetField(
                    baseMethodDelegateField.DeclaringType.MakeGenericType(
                        method.GetGenericArguments()), baseMethodDelegateField);
                baseMethodInfoField = TypeBuilder.GetField(
                    baseMethodInfoField.DeclaringType.MakeGenericType(
                        method.GetGenericArguments()), baseMethodInfoField);
            }

            var returnParam = method.DefineParameter(0,
                baseReturnParam.Attributes, baseReturnParam.Name);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = method.DefineParameter(i + 1,
                    baseParameter.Attributes, baseParameter.Name);
            }

            var il = method.GetILGenerator();
            var invokeBaseLabel = il.DefineLabel();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, methodInvokeHandlerField);
            il.Emit(OpCodes.Brfalse_S, invokeBaseLabel);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, methodInvokeHandlerField);
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Castclass, typeof(object));
            il.Emit(OpCodes.Ldsfld, baseMethodInfoField);
            il.EmitLdcI4(baseParameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                il.Emit(OpCodes.Dup);
                il.EmitLdcI4(i);
                il.EmitLdarg(i + 1);
                il.Emit(OpCodes.Box, baseParameter.ParameterType);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Ldsfld, baseMethodDelegateField);
            il.Emit(OpCodes.Call,
                typeof(MethodInvokeHandler).GetMethod(nameof(MethodInvokeHandler.Invoke)));
            var returnLabel = il.DefineLabel();
            il.Emit(OpCodes.Br_S, returnLabel);
            il.MarkLabel(invokeBaseLabel);
            il.Emit(OpCodes.Ldsfld, baseMethodDelegateField);
            il.Emit(OpCodes.Ldarg_0);
            il.EmitLdcI4(baseParameters.Length);
            il.Emit(OpCodes.Newarr, typeof(object));
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                il.Emit(OpCodes.Dup);
                il.EmitLdcI4(i);
                il.EmitLdarg(i + 1);
                il.Emit(OpCodes.Box, baseParameter.ParameterType);
                il.Emit(OpCodes.Stelem_Ref);
            }
            il.Emit(OpCodes.Call,
                typeof(MethodDelegate).GetMethod(nameof(MethodDelegate.Invoke)));
            il.MarkLabel(returnLabel);
            if (baseMethod.ReturnType != typeof(void))
            {
                il.Emit(OpCodes.Unbox_Any, baseMethod.ReturnType);
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
