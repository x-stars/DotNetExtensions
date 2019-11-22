using System;
using System.Reflection.Emit;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 提供 IL 指令生成相关的扩展方法。
    /// </summary>
    internal static class ILGenerationExtensions
    {
        /// <summary>
        /// 发出将指定索引处的参数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="il">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="position">要加载到计算堆栈的参数的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="il"/> 为 <see langword="null"/>。</exception>
        public static void EmitLdarg(this ILGenerator il, int position)
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
        public static void EmitLdcI4(this ILGenerator il, int value)
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
    }
}
