using System;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection.Emit
{
    /// <summary>
    /// 提供包装代理类型运行时类型生成相关的帮助方法。
    /// </summary>
    internal static class WrapProxyTypeBuildingHelpers
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

            return (method.DeclaringType != typeof(IWrapProxy)) &&
                ((method.DeclaringType == typeof(object)) || method.DeclaringType!.IsInterface) &&
                method.IsPublic && method.IsOverridable() &&
                !method.ReturnParameter.ParameterType.IsNotILBoxable() &&
                Array.TrueForAll(
                    Array.ConvertAll(method.GetParameters(), param => param.ParameterType),
                    type => !type.IsNotILBoxable());
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

            return (method.DeclaringType != typeof(IWrapProxy)) &&
                ((method.DeclaringType == typeof(object)) || method.DeclaringType!.IsInterface) &&
                method.IsPublic && method.IsOverridable() && !method.IsWrapProxyOverride();
        }

        /// <summary>
        /// 以指定的方法为基础，定义调用代理对象方法的新方法，并添加到当前类型。
        /// </summary>
        /// <param name="type">要定义方法的 <see cref="TypeBuilder"/> 对象。</param>
        /// <param name="baseMethod">作为基础的方法。</param>
        /// <param name="baseType">定义基础方法的类型，若为泛型类型，则应为构造泛型类型。</param>
        /// <param name="instanceField">代理对象的字段。</param>
        /// <returns>定义的方法，调用代理对象的 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的声明类型不为接口。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineWrapBaseInvokeMethodLike(
            this TypeBuilder type, MethodInfo baseMethod, Type baseType, FieldInfo instanceField)
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
            if (!(baseMethod.IsPublic && baseMethod.IsInheritable()))
            {
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseMethod), inner);
            }

            var method = type.DefineMethodLike(baseMethod, baseType);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, instanceField);
            for (int index = 0; index < baseMethod.GetParameters().Length; index++)
            {
                il.EmitLdarg(index + 1);
            }
            il.Emit(OpCodes.Callvirt,
                (baseMethod.GetGenericArguments().Length == 0) ? baseMethod :
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
        /// <param name="explicitOverride">指定是否以显式方式重写。</param>
        /// <returns>定义的方法，调用代理对象的 <paramref name="baseMethod"/> 方法。</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="baseMethod"/> 的声明类型不为接口。</exception>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        internal static MethodBuilder DefineWrapBaseInvokeMethodOverride(
            this TypeBuilder type, MethodInfo baseMethod,
            FieldInfo instanceField, bool explicitOverride = false)
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
                var inner = new MemberAccessException();
                throw new ArgumentException(inner.Message, nameof(baseMethod), inner);
            }

            var method = type.DefineMethodOverride(baseMethod, explicitOverride);

            var il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, instanceField);
            for (int index = 0; index < baseMethod.GetParameters().Length; index++)
            {
                il.EmitLdarg(index + 1);
            }
            il.Emit(OpCodes.Callvirt,
                (baseMethod.GetGenericArguments().Length == 0) ? baseMethod :
                baseMethod.MakeGenericMethod(method.GetGenericArguments()));
            il.Emit(OpCodes.Ret);

            type.DefineMethodOverride(method, baseMethod);

            return method;
        }
    }
}
