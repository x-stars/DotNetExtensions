using System;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection.Emit
{
    /// <summary>
    /// 提供 IL 指令生成相关的扩展方法。
    /// </summary>
    internal static class ILGenerationExtensions
    {
        /// <summary>
        /// 提供当前类型使用的方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        private static class MethodInfoCache
        {
            /// <summary>
            /// 表示 <see cref="Pointer.Box(void*, Type)"/> 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo PointerBox =
                typeof(Pointer).GetMethod(nameof(Pointer.Box))!;

            /// <summary>
            /// 表示 <see cref="Pointer.Unbox(object)"/> 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo PointerUnbox =
                typeof(Pointer).GetMethod(nameof(Pointer.Unbox))!;

            /// <summary>
            /// 表示 <see cref="Type.GetTypeFromHandle(RuntimeTypeHandle)"/>
            /// 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo TypeFromHandle =
                typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!;
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
        /// 发出将指定索引处的参数加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="index">要加载到计算堆栈的参数的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        public static void EmitLdarg(this ILGenerator ilGen, int index)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }

            switch (index)
            {
                case 0: ilGen.Emit(OpCodes.Ldarg_0); break;
                case 1: ilGen.Emit(OpCodes.Ldarg_1); break;
                case 2: ilGen.Emit(OpCodes.Ldarg_2); break;
                case 3: ilGen.Emit(OpCodes.Ldarg_3); break;
                default:
                    ilGen.Emit((byte)index == index ?
                        OpCodes.Ldarg_S : OpCodes.Ldarg, index);
                    break;
            }
        }

        /// <summary>
        /// 发出将指定索引处的局部变量加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="index">要加载到计算堆栈的局部变量的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        public static void EmitLdloc(this ILGenerator ilGen, int index)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }

            switch (index)
            {
                case 0: ilGen.Emit(OpCodes.Ldloc_0); break;
                case 1: ilGen.Emit(OpCodes.Ldloc_1); break;
                case 2: ilGen.Emit(OpCodes.Ldloc_2); break;
                case 3: ilGen.Emit(OpCodes.Ldloc_3); break;
                default:
                    ilGen.Emit((byte)index == index ?
                        OpCodes.Ldloc_S : OpCodes.Ldloc, index);
                    break;
            }
        }

        /// <summary>
        /// 发出将计算堆栈上的值弹出并存储到指定索引处的局部变量的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="index">要存储计算堆栈上的值的局部变量的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        public static void EmitStloc(this ILGenerator ilGen, int index)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }

            switch (index)
            {
                case 0: ilGen.Emit(OpCodes.Stloc_0); break;
                case 1: ilGen.Emit(OpCodes.Stloc_1); break;
                case 2: ilGen.Emit(OpCodes.Stloc_2); break;
                case 3: ilGen.Emit(OpCodes.Stloc_3); break;
                default:
                    ilGen.Emit((byte)index == index ?
                        OpCodes.Stloc_S : OpCodes.Stloc, index);
                    break;
            }
        }

        /// <summary>
        /// 发出将指定类型的数组元素加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="type">数组中的元素的类型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="ilGen"/>
        /// 或 <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static void EmitLdelem(this ILGenerator ilGen, Type type)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsEnum) { type = type.GetEnumUnderlyingType(); }

            var opcode = type.IsGenericParameter ? OpCodes.Ldelem :
                (type.IsByRef || type.IsPointer) ? OpCodes.Ldelem_I :
                !type.IsValueType ? OpCodes.Ldelem_Ref :
                !type.IsPrimitive ? OpCodes.Ldelem :
                (type == typeof(nint) || type == typeof(nuint)) ? OpCodes.Ldelem_I :
                type == typeof(sbyte) ? OpCodes.Ldelem_I1 :
                (type == typeof(byte) || type == typeof(bool)) ? OpCodes.Ldelem_U1 :
                type == typeof(short) ? OpCodes.Ldelem_I2 :
                (type == typeof(ushort) || type == typeof(char)) ? OpCodes.Ldelem_U2 :
                type == typeof(int) ? OpCodes.Ldelem_I4 :
                type == typeof(uint) ? OpCodes.Ldelem_U4 :
                (type == typeof(long) || type == typeof(ulong)) ? OpCodes.Ldelem_I8 :
                type == typeof(float) ? OpCodes.Ldelem_R4 :
                type == typeof(double) ? OpCodes.Ldelem_R8 :
                throw new InvalidProgramException();

            if (opcode == OpCodes.Ldelem)
            {
                ilGen.Emit(opcode, type);
            }
            else
            {
                ilGen.Emit(opcode);
            }
        }

        /// <summary>
        /// 发出将计算堆栈上的值弹出并存储到指定类型的数组元素的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="type">数组中的元素的类型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="ilGen"/>
        /// 或 <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static void EmitStelem(this ILGenerator ilGen, Type type)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsEnum) { type = type.GetEnumUnderlyingType(); }

            var opcode = type.IsGenericParameter ? OpCodes.Stelem :
                (type.IsByRef || type.IsPointer) ? OpCodes.Stelem_I :
                !type.IsValueType ? OpCodes.Stelem_Ref :
                !type.IsPrimitive ? OpCodes.Stelem :
                (type == typeof(nint) || type == typeof(nuint)) ? OpCodes.Stelem_I :
                type == typeof(sbyte) ? OpCodes.Stelem_I1 :
                (type == typeof(byte) || type == typeof(bool)) ? OpCodes.Stelem_I1 :
                type == typeof(short) ? OpCodes.Stelem_I2 :
                (type == typeof(ushort) || type == typeof(char)) ? OpCodes.Stelem_I2 :
                type == typeof(int) ? OpCodes.Stelem_I4 :
                type == typeof(uint) ? OpCodes.Stelem_I4 :
                (type == typeof(long) || type == typeof(ulong)) ? OpCodes.Stelem_I8 :
                type == typeof(float) ? OpCodes.Stelem_R4 :
                type == typeof(double) ? OpCodes.Stelem_R8 :
                throw new InvalidProgramException();

            if (opcode == OpCodes.Stelem)
            {
                ilGen.Emit(opcode, type);
            }
            else
            {
                ilGen.Emit(opcode);
            }
        }

        /// <summary>
        /// 发出将指定类型的值间接加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="type">以地址间接引用的值的类型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="ilGen"/>
        /// 或 <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static void EmitLdind(this ILGenerator ilGen, Type type)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsEnum) { type = type.GetEnumUnderlyingType(); }

            var opcode = type.IsGenericParameter ? OpCodes.Ldobj :
                (type.IsByRef || type.IsPointer) ? OpCodes.Ldind_I :
                !type.IsValueType ? OpCodes.Ldind_Ref :
                !type.IsPrimitive ? OpCodes.Ldobj :
                (type == typeof(nint) || type == typeof(nuint)) ? OpCodes.Ldind_I :
                type == typeof(sbyte) ? OpCodes.Ldind_I1 :
                (type == typeof(byte) || type == typeof(bool)) ? OpCodes.Ldind_U1 :
                type == typeof(short) ? OpCodes.Ldind_I2 :
                (type == typeof(ushort) || type == typeof(char)) ? OpCodes.Ldind_U2 :
                type == typeof(int) ? OpCodes.Ldind_I4 :
                type == typeof(uint) ? OpCodes.Ldind_U4 :
                (type == typeof(long) || type == typeof(ulong)) ? OpCodes.Ldind_I8 :
                type == typeof(float) ? OpCodes.Ldind_R4 :
                type == typeof(double) ? OpCodes.Ldind_R8 :
                throw new InvalidProgramException();

            if (opcode == OpCodes.Ldobj)
            {
                ilGen.Emit(opcode, type);
            }
            else
            {
                ilGen.Emit(opcode);
            }
        }

        /// <summary>
        /// 发出将计算堆栈上的值弹出并间接存储到指定类型的值的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="type">以地址间接引用的值的类型。</param>
        /// <exception cref="ArgumentNullException"><paramref name="ilGen"/>
        /// 或 <paramref name="type"/> 为 <see langword="null"/>。</exception>
        public static void EmitStind(this ILGenerator ilGen, Type type)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (type.IsEnum) { type = type.GetEnumUnderlyingType(); }

            var opcode = type.IsGenericParameter ? OpCodes.Stobj :
                (type.IsByRef || type.IsPointer) ? OpCodes.Stind_I :
                !type.IsValueType ? OpCodes.Stind_Ref :
                !type.IsPrimitive ? OpCodes.Stobj :
                (type == typeof(nint) || type == typeof(nuint)) ? OpCodes.Stind_I :
                type == typeof(sbyte) ? OpCodes.Stind_I1 :
                (type == typeof(byte) || type == typeof(bool)) ? OpCodes.Stind_I1 :
                type == typeof(short) ? OpCodes.Stind_I2 :
                (type == typeof(ushort) || type == typeof(char)) ? OpCodes.Stind_I2 :
                type == typeof(int) ? OpCodes.Stind_I4 :
                type == typeof(uint) ? OpCodes.Stind_I4 :
                (type == typeof(long) || type == typeof(ulong)) ? OpCodes.Stind_I8 :
                type == typeof(float) ? OpCodes.Stind_R4 :
                type == typeof(double) ? OpCodes.Stind_R8 :
                throw new InvalidProgramException();

            if (opcode == OpCodes.Stobj)
            {
                ilGen.Emit(opcode, type);
            }
            else
            {
                ilGen.Emit(opcode);
            }
        }

        /// <summary>
        /// 发出将计算堆栈上的指定类型的值转换为 <see cref="object"/> 的指令，并放到当前指令流中。
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
            else if (type.IsByRef)
            {
                var refType = type.GetElementType()!;
                ilGen.EmitLdind(refType);
                ilGen.EmitBox(refType);
            }
            else if (type.IsPointer)
            {
                ilGen.Emit(OpCodes.Ldtoken, type);
                ilGen.Emit(OpCodes.Call, MethodInfoCache.TypeFromHandle);
                ilGen.Emit(OpCodes.Call, MethodInfoCache.PointerBox);
            }
            else if (type.IsValueType)
            {
                ilGen.Emit(OpCodes.Box, type);
            }
        }

        /// <summary>
        /// 发出将计算堆栈上的 <see cref="object"/> 转换为指定类型的值的指令，并放到当前指令流中。
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
            else if (type.IsByRef)
            {
                var refType = type.GetElementType()!;
                ilGen.EmitUnbox(refType);
            }
            else if (type.IsPointer)
            {
                ilGen.Emit(OpCodes.Call, MethodInfoCache.PointerUnbox);
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
