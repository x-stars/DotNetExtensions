using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

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
        /// <param name="source">一个 <see cref="ILGenerator"/> 类的对象。</param>
        /// <param name="position">要加载到计算堆栈的参数的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static void EmitLdarg(this ILGenerator source, int position)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            switch (position)
            {
                case 0: source.Emit(OpCodes.Ldarg_0); break;
                case 1: source.Emit(OpCodes.Ldarg_1); break;
                case 2: source.Emit(OpCodes.Ldarg_2); break;
                case 3: source.Emit(OpCodes.Ldarg_3); break;
                default:
                    source.Emit((position <= byte.MaxValue) ?
                        OpCodes.Ldarg_S : OpCodes.Ldarg, position);
                    break;
            }
        }

        /// <summary>
        /// 发出将指定 32 位有符号整数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="source">一个 <see cref="ILGenerator"/> 类的对象。</param>
        /// <param name="value">要加载到计算堆栈的 32 位有符号整数的值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为 <see langword="null"/>。</exception>
        internal static void EmitLdcI4(this ILGenerator source, int value)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            switch (value)
            {
                case 0: source.Emit(OpCodes.Ldc_I4_0); break;
                case 1: source.Emit(OpCodes.Ldc_I4_1); break;
                case 2: source.Emit(OpCodes.Ldc_I4_2); break;
                case 3: source.Emit(OpCodes.Ldc_I4_3); break;
                case 4: source.Emit(OpCodes.Ldc_I4_4); break;
                case 5: source.Emit(OpCodes.Ldc_I4_5); break;
                case 6: source.Emit(OpCodes.Ldc_I4_6); break;
                case 7: source.Emit(OpCodes.Ldc_I4_7); break;
                case 8: source.Emit(OpCodes.Ldc_I4_8); break;
                case -1: source.Emit(OpCodes.Ldc_I4_M1); break;
                default:
                    source.Emit((value <= byte.MaxValue) ?
                        OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, value);
                    break;
            }
        }

        /// <summary>
        /// 以指定的构造函数为基础，定义仅调用指定构造函数的构造函数，并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseConstructor">作为基础的构造函数的定义。</param>
        /// <returns>定义完成的构造函数，仅调用 <paramref name="baseConstructor"/> 构造函数。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseConstructor"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
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
            if (!baseConstructor.IsPublic && !baseConstructor.IsFamily)
            {
                throw new ArgumentException(
                    new ArgumentException().Message, nameof(baseConstructor));
            }

            // 定义构造函数。
            var baseParameters = baseConstructor.GetParameters();
            var baseAttributes = baseConstructor.Attributes;
            var constructor = source.DefineConstructor(
                baseAttributes, baseConstructor.CallingConvention,
                Array.ConvertAll(baseParameters, param => param.ParameterType));
            {
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    var constructorParameter = baseParameters[i];
                    var parameter = constructor.DefineParameter(
                        i + 1, constructorParameter.Attributes, constructorParameter.Name);
                    if (constructorParameter.HasDefaultValue)
                    {
                        parameter.SetConstant(constructorParameter.DefaultValue);
                    }
                }
                var ilGen = constructor.GetILGenerator();
                ilGen.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    ilGen.EmitLdarg(i + 1);
                }
                ilGen.Emit(OpCodes.Call, baseConstructor);
                ilGen.Emit(OpCodes.Ret);
            }

            return constructor;
        }

        /// <summary>
        /// 以指定的基类方法为基础，定义仅调用基类方法的新方法（签名不同），并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseMethod">作为基础的基类方法的定义。</param>
        /// <returns>定义完成的签名不同的新方法，仅调用 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的访问级别不为公共或保护。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineBaseAccessMethod(
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

            // 定义方法。
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var accessMethod = source.DefineMethod(
                $"<{baseMethod.DeclaringType.ToString()}>{baseMethod.Name}",
                MethodAttributes.Family | MethodAttributes.HideBySig,
                baseReturnParam.ParameterType,
                Array.ConvertAll(baseParameters, param => param.ParameterType));
            // 泛型参数。
            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                accessMethod.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var baseGenericParam = baseGenericParams[i];
                var genericConstraints = baseGenericParam.GetGenericParameterConstraints();
                var baseTypeConstraint = genericConstraints.Where(
                    genericConstraint => genericConstraint.IsClass).SingleOrDefault();
                var interfaceConstraints = genericConstraints.Where(
                    genericConstraint => genericConstraint.IsInterface).ToArray();
                var genericParam = genericParams[i];
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
            // 普通参数。
            var returnParam = accessMethod.DefineParameter(0, baseReturnParam.Attributes, null);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = accessMethod.DefineParameter(
                    i + 1, baseParameter.Attributes, baseParameter.Name);
                if (baseParameter.HasDefaultValue)
                {
                    parameter.SetConstant(baseParameter.DefaultValue);
                }
            }
            // 生成 IL 代码。
            var ilGen = accessMethod.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    ilGen.EmitLdarg(i + 1);
                }
                if (baseMethod.IsAbstract)
                {
                    ilGen.Emit(OpCodes.Newobj,
                        typeof(NotImplementedException).GetConstructor(Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Throw);
                }
                else
                {
                    ilGen.Emit(OpCodes.Call,
                        (baseGenericParams.Length == 0) ? baseMethod :
                        baseMethod.MakeGenericMethod(accessMethod.GetGenericArguments()));
                    ilGen.Emit(OpCodes.Ret);
                }
            }

            return accessMethod;
        }

        /// <summary>
        /// 以指定的基类方法为基础，定义仅调用基类方法的重写方法，并添加到当前类型。
        /// </summary>
        /// <param name="source">一个 <see cref="TypeBuilder"/> 类型的对象。</param>
        /// <param name="baseMethod">作为基础的基类方法的定义。</param>
        /// <returns>定义完成的重写方法，仅调用 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的访问级别不为公共或保护，或不可重写。</exception>
        internal static MethodBuilder DefineDefaultOverrideMethod(
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

            bool newSlot = baseMethod.DeclaringType.IsInterface;

            // 定义方法。
            var baseAttributes = baseMethod.Attributes;
            var baseGenericParams = baseMethod.GetGenericArguments();
            var baseReturnParam = baseMethod.ReturnParameter;
            var baseParameters = baseMethod.GetParameters();
            var attributes = baseAttributes & ~MethodAttributes.Abstract;
            if (!newSlot) { attributes &= ~MethodAttributes.NewSlot; }
            var overrideMethod = source.DefineMethod(baseMethod.Name,
                attributes, baseReturnParam.ParameterType,
                Array.ConvertAll(baseParameters, param => param.ParameterType));
            // 泛型参数。
            var genericParams = (baseGenericParams.Length == 0) ?
                Array.Empty<GenericTypeParameterBuilder>() :
                overrideMethod.DefineGenericParameters(
                    Array.ConvertAll(baseGenericParams, param => param.Name));
            for (int i = 0; i < baseGenericParams.Length; i++)
            {
                var baseGenericParam = baseGenericParams[i];
                var genericConstraints = baseGenericParam.GetGenericParameterConstraints();
                var baseTypeConstraint = genericConstraints.Where(
                    genericConstraint => genericConstraint.IsClass).SingleOrDefault();
                var interfaceConstraints = genericConstraints.Where(
                    genericConstraint => genericConstraint.IsInterface).ToArray();
                var genericParam = genericParams[i];
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
            // 普通参数。
            var returnParam = overrideMethod.DefineParameter(0, baseReturnParam.Attributes, null);
            for (int i = 0; i < baseParameters.Length; i++)
            {
                var baseParameter = baseParameters[i];
                var parameter = overrideMethod.DefineParameter(
                    i + 1, baseParameter.Attributes, baseParameter.Name);
                if (baseParameter.HasDefaultValue)
                {
                    parameter.SetConstant(baseParameter.DefaultValue);
                }
            }
            // 生成 IL 代码。
            var ilGen = overrideMethod.GetILGenerator();
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                for (int i = 0; i < baseParameters.Length; i++)
                {
                    ilGen.EmitLdarg(i + 1);
                }
                if (baseMethod.IsAbstract)
                {
                    ilGen.Emit(OpCodes.Newobj,
                        typeof(NotImplementedException).GetConstructor(Type.EmptyTypes));
                    ilGen.Emit(OpCodes.Throw);
                }
                else
                {
                    ilGen.Emit(OpCodes.Call,
                        (baseGenericParams.Length == 0) ? baseMethod :
                        baseMethod.MakeGenericMethod(baseMethod.GetGenericArguments()));
                    ilGen.Emit(OpCodes.Ret);
                }
            }

            return overrideMethod;
        }
    }
}
