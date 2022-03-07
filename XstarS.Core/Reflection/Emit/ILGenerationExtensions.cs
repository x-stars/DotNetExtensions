﻿using System;
using System.Reflection.Emit;

namespace XstarS.Reflection.Emit
{
    /// <summary>
    /// 提供 IL 指令生成相关的扩展方法。
    /// </summary>
    public static class ILGenerationExtensions
    {
        /// <summary>
        /// 确定当前 <see cref="Type"/> 的实例是否不能由 IL 指令转换为 <see cref="object"/>。
        /// </summary>
        /// <param name="type">要确定是否不能转换的 <see cref="Type"/> 对象。</param>
        /// <returns>若 <paramref name="type"/> 的实例不能由 IL 指令转换为 <see cref="object"/>，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static bool IsNotILBoxable(this Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.IsNotBoxable() && !(type.IsByRef || type.IsPointer);
        }

        /// <summary>
        /// 发出将指定索引处的参数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="position">要加载到计算堆栈的参数的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        public static void EmitLdarg(this ILGenerator ilGen, int position)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }

            switch (position)
            {
                case 0: ilGen.Emit(OpCodes.Ldarg_0); break;
                case 1: ilGen.Emit(OpCodes.Ldarg_1); break;
                case 2: ilGen.Emit(OpCodes.Ldarg_2); break;
                case 3: ilGen.Emit(OpCodes.Ldarg_3); break;
                default:
                    ilGen.Emit((byte)position == position ?
                        OpCodes.Ldarg_S : OpCodes.Ldarg, position);
                    break;
            }
        }

        /// <summary>
        /// 发出将指定 32 位有符号整数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="value">要加载到计算堆栈的 32 位有符号整数的值。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        public static void EmitLdcI4(this ILGenerator ilGen, int value)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }

            switch (value)
            {
                case -1: ilGen.Emit(OpCodes.Ldc_I4_M1); break;
                case 0: ilGen.Emit(OpCodes.Ldc_I4_0); break;
                case 1: ilGen.Emit(OpCodes.Ldc_I4_1); break;
                case 2: ilGen.Emit(OpCodes.Ldc_I4_2); break;
                case 3: ilGen.Emit(OpCodes.Ldc_I4_3); break;
                case 4: ilGen.Emit(OpCodes.Ldc_I4_4); break;
                case 5: ilGen.Emit(OpCodes.Ldc_I4_5); break;
                case 6: ilGen.Emit(OpCodes.Ldc_I4_6); break;
                case 7: ilGen.Emit(OpCodes.Ldc_I4_7); break;
                case 8: ilGen.Emit(OpCodes.Ldc_I4_8); break;
                default:
                    ilGen.Emit((sbyte)value == value ?
                        OpCodes.Ldc_I4_S : OpCodes.Ldc_I4, value);
                    break;
            }
        }

        /// <summary>
        /// 发出将指定类型的值转换为 <see cref="object"/> 的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="type">要转换为 <see cref="object"/> 的值的类型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="ilGen"/>
        /// 或 <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static void EmitBox(this ILGenerator ilGen, Type type)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsGenericParameter)
            {
                ilGen.Emit(OpCodes.Box, type);
            }
            else if (type.IsByRef || type.IsPointer)
            {
                ilGen.Emit(OpCodes.Box, typeof(nint));
            }
            else if (type.IsValueType)
            {
                ilGen.Emit(OpCodes.Box, type);
            }
        }

        /// <summary>
        /// 发出将 <see cref="object"/> 转换为指定类型的值的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="type">要由 <see cref="object"/> 转换到的值的类型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="ilGen"/>
        /// 或 <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static void EmitUnbox(this ILGenerator ilGen, Type type)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsGenericParameter)
            {
                ilGen.Emit(OpCodes.Unbox_Any, type);
            }
            else if (type.IsByRef || type.IsPointer)
            {
                ilGen.Emit(OpCodes.Unbox_Any, typeof(nint));
            }
            else if (type.IsValueType)
            {
                ilGen.Emit(OpCodes.Unbox_Any, type);
            }
            else if (type != typeof(object))
            {
                ilGen.Emit(OpCodes.Castclass, type);
            }
        }
    }
}
