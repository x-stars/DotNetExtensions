using System;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供包装代理类型运行时类型生成相关的帮助方法。
    /// </summary>
    internal static class WrapProxyTypeBuildingHelper
    {
        /// <summary>
        /// 确定当前 <see cref="MemberInfo"/> 是否应由 <see cref="WrapProxyTypeProvider"/> 按照代理模式重写。
        /// </summary>
        /// <param name="method">要确定是否按代理模式重写的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 应由 <see cref="WrapProxyTypeProvider"/> 按照代理模式重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        internal static bool IsWrapProxyOverride(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.IsPublic && method.IsOverridable() &&
                Array.TrueForAll(
                    Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                    type => !type.IsNotBoxable()) &&
                (!method.IsGenericMethod || (method.IsGenericMethod &&
                Array.TrueForAll(
                    method.GetGenericArguments(),
                    type => type.GetGenericParameterConstraints().Length == 0)));
        }

        /// <summary>
        /// 确定当前 <see cref="MemberInfo"/> 是否应由 <see cref="WrapProxyTypeProvider"/> 按照非代理模式重写。
        /// </summary>
        /// <param name="method">要确定是否按非代理模式重写的 <see cref="MethodInfo"/> 对象。</param>
        /// <returns>若 <paramref name="method"/> 应由 <see cref="WrapProxyTypeProvider"/> 按照非代理模式重写，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        internal static bool IsNonWrapProxyOverride(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return method.IsPublic && method.IsOverridable() && !method.IsWrapProxyOverride();
        }

        /// <summary>
        /// 以指定的方法为基础，定义调用代理对象方法的新方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="instanceField">代理对象的字段。</param>
        /// <returns>定义的方法，调用代理对象的 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的声明类型不为接口。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineWrapBaseInvokeMethod(
            this TypeBuilder type, MethodInfo baseMethod, FieldInfo instanceField)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (!baseMethod.IsWrapProxyOverride())
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseMethod));
            }

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var method = type.DefineMethod(
                $"@{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
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
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, instanceField);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                il.EmitLdarg(i + 1);
            }
            il.Emit(OpCodes.Callvirt,
                (baseGenericParams.Length == 0) ? baseMethod :
                baseMethod.MakeGenericMethod(method.GetGenericArguments()));
            il.Emit(OpCodes.Ret);

            return method;
        }

        /// <summary>
        /// 以指定的方法为基础，定义调用调用代理对象方法的重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="instanceField">代理对象的字段。</param>
        /// <returns>定义的方法，调用代理对象的 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的声明类型不为接口。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineWrapBaseInvokeMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod, FieldInfo instanceField)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (baseMethod is null)
            {
                throw new ArgumentNullException(nameof(baseMethod));
            }
            if (!(baseMethod.IsPublic && baseMethod.IsOverridable()))
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseMethod));
            }

            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();

            var method = type.DefineMethod(
                $"{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
                MethodAttributes.Assembly | MethodAttributes.HideBySig,
                baseReturnParam.ParameterType,
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
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, instanceField);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                il.EmitLdarg(i + 1);
            }
            il.Emit(OpCodes.Callvirt,
                (baseGenericParams.Length == 0) ? baseMethod :
                baseMethod.MakeGenericMethod(method.GetGenericArguments()));
            il.Emit(OpCodes.Ret);

            type.DefineMethodOverride(method, baseMethod);

            return method;
        }

        /// <summary>
        /// 以指定的方法为基础，定义调用指定代理委托的重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="baseMethodInfoField">基础方法的 <see cref="MethodInfo"/> 字段。</param>
        /// <param name="baseMethodDelegateField">基础方法的 <see cref="MethodDelegate"/> 字段。</param>
        /// <param name="methodInvokeHandlerField">当前类型的代理委托字段。</param>
        /// <returns>定义的方法，调用 <paramref name="methodInvokeHandlerField"/> 字段的代理委托。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的声明类型不为接口。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineWrapProxyMethodOverride(
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
            if (!baseMethod.IsWrapProxyOverride())
            {
                throw new ArgumentException(new ArgumentException().Message, nameof(baseMethod));
            }

            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract & ~MethodAttributes.NewSlot;

            var method = type.DefineMethod(
                $"{baseMethod.Name}#{baseMethod.MethodHandle.Value.ToString()}",
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
            if (baseMethod.ReturnType != typeof(void))
            {
                il.Emit(OpCodes.Unbox_Any, baseMethod.ReturnType);
            }
            else
            {
                il.Emit(OpCodes.Pop);
            }
            il.Emit(OpCodes.Ret);

            type.DefineMethodOverride(method, baseMethod);

            return method;
        }
    }
}
