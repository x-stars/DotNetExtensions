using System;
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
        /// 以指定的基类方法为基础，定义调用基类方法的新方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的基类方法。</param>
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

            var method = type.DefineMethod($"<Base>{baseMethod.ToNameHandleString()}",
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
    }
}
