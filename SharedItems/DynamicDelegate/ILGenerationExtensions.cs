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
        /// 提供当前类型使用的反射元数据的 <see cref="MemberInfo"/> 对象。
        /// </summary>
        private static class ReflectionData
        {
            /// <summary>
            /// 表示 <see cref="Pointer.Box(void*, Type)"/> 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo PointerBoxMethod =
                typeof(Pointer).GetMethod(nameof(Pointer.Box))!;

            /// <summary>
            /// 表示 <see cref="Pointer.Unbox(object)"/> 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo PointerUnboxMethod =
                typeof(Pointer).GetMethod(nameof(Pointer.Unbox))!;

            /// <summary>
            /// 表示 <see cref="Type.GetTypeFromHandle(RuntimeTypeHandle)"/>
            /// 方法的 <see cref="MethodInfo"/> 对象。
            /// </summary>
            internal static readonly MethodInfo TypeFromHandleMethod =
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
                case int when (sbyte)value == value:
                    ilGen.Emit(OpCodes.Ldc_I4_S, (sbyte)value); break;
                default:
                    ilGen.Emit(OpCodes.Ldc_I4, value); break;
            }
        }

        /// <summary>
        /// 发出将指定索引处的局部变量加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="index">要加载到计算堆栈的局部变量的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 超出 16 位无符号整数能表示的范围。</exception>
        public static void EmitLdloc(this ILGenerator ilGen, int index)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if ((ushort)index != index)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            switch (index)
            {
                case 0: ilGen.Emit(OpCodes.Ldloc_0); break;
                case 1: ilGen.Emit(OpCodes.Ldloc_1); break;
                case 2: ilGen.Emit(OpCodes.Ldloc_2); break;
                case 3: ilGen.Emit(OpCodes.Ldloc_3); break;
                case int when (byte)index == index:
                    ilGen.Emit(OpCodes.Ldloc_S, (byte)index); break;
                default:
                    ilGen.Emit(OpCodes.Ldloc, (short)index); break;
            }
        }

        /// <summary>
        /// 发出将计算堆栈上的值弹出并存储到指定索引处的局部变量的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="index">要存储计算堆栈上的值的局部变量的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 超出 16 位无符号整数能表示的范围。</exception>
        public static void EmitStloc(this ILGenerator ilGen, int index)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if ((ushort)index != index)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            switch (index)
            {
                case 0: ilGen.Emit(OpCodes.Stloc_0); break;
                case 1: ilGen.Emit(OpCodes.Stloc_1); break;
                case 2: ilGen.Emit(OpCodes.Stloc_2); break;
                case 3: ilGen.Emit(OpCodes.Stloc_3); break;
                case int when (byte)index == index:
                    ilGen.Emit(OpCodes.Stloc_S, (byte)index); break;
                default:
                    ilGen.Emit(OpCodes.Stloc, (short)index); break;
            }
        }

        /// <summary>
        /// 发出将指定索引处的局部变量的地址加载到计算堆栈上的指令，并放到当前指令流中。
        /// </summary>
        /// <param name="ilGen">要发出指令的 <see cref="ILGenerator"/> 对象。</param>
        /// <param name="index">要加载地址到计算堆栈的局部变量的索引。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ilGen"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="index"/> 超出 16 位无符号整数能表示的范围。</exception>
        public static void EmitLdloca(this ILGenerator ilGen, int index)
        {
            if (ilGen is null)
            {
                throw new ArgumentNullException(nameof(ilGen));
            }
            if ((ushort)index != index)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if ((byte)index == index)
            {
                ilGen.Emit(OpCodes.Ldloca_S, (byte)index);
            }
            else
            {
                ilGen.Emit(OpCodes.Ldloca, (short)index);
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
                (type == typeof(sbyte)) ? OpCodes.Ldind_I1 :
                (type == typeof(byte) || type == typeof(bool)) ? OpCodes.Ldind_U1 :
                (type == typeof(short)) ? OpCodes.Ldind_I2 :
                (type == typeof(ushort) || type == typeof(char)) ? OpCodes.Ldind_U2 :
                (type == typeof(int)) ? OpCodes.Ldind_I4 :
                (type == typeof(uint)) ? OpCodes.Ldind_U4 :
                (type == typeof(long) || type == typeof(ulong)) ? OpCodes.Ldind_I8 :
                (type == typeof(float)) ? OpCodes.Ldind_R4 :
                (type == typeof(double)) ? OpCodes.Ldind_R8 :
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
                ilGen.Emit(OpCodes.Call, ReflectionData.TypeFromHandleMethod);
                ilGen.Emit(OpCodes.Call, ReflectionData.PointerBoxMethod);
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
                ilGen.Emit(OpCodes.Call, ReflectionData.PointerUnboxMethod);
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
