using System;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供以 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/> 的方法。
    /// </summary>
    public static class IntPtrByRef
    {
        /// <summary>
        /// <see cref="IntPtrByRef.RealType"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<Type> LazyRealType =
            new Lazy<Type>(IntPtrByRef.CreateRealType);

        /// <summary>
        /// 获取提供 <see cref="IntPtrByRef"/> 方法实现的类型的 <see cref="Type"/> 对象。
        /// </summary>
        private static Type RealType => IntPtrByRef.LazyRealType.Value;

        /// <summary>
        /// 封装将引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/> 的方法。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="value">按引用转递的值。</param>
        /// <returns>引用传递 <see langword="ref"/> 转换得到的 <see cref="IntPtr"/>。</returns>
        private delegate IntPtr ByRefBoxer<T>(ref T value);

        /// <summary>
        /// 提供 <see cref="IntPtrByRef.RealType"/> 中各方法的委托。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        private static class Delegates<T>
        {
            /// <summary>
            /// 表示 <see cref="IntPtrByRef.RealType"/> 中
            /// <see cref="IntPtrByRef.ToIntPtr{T}(ref T)"/> 方法的委托。
            /// </summary>
            internal static readonly ByRefBoxer<T> ToIntPtr =
                (ByRefBoxer<T>)IntPtrByRef.RealType.GetMethod(
                    nameof(IntPtrByRef.ToIntPtr)).MakeGenericMethod(
                        typeof(T)).CreateDelegate(typeof(ByRefBoxer<T>));

            /// <summary>
            /// 表示 <see cref="IntPtrByRef.RealType"/> 中
            /// <see cref="IntPtrByRef.GetValue{T}(IntPtr)"/> 方法的委托。
            /// </summary>
            internal static readonly Func<IntPtr, T> GetValue =
                (Func<IntPtr, T>)IntPtrByRef.RealType.GetMethod(
                    nameof(IntPtrByRef.GetValue)).MakeGenericMethod(
                        typeof(T)).CreateDelegate(typeof(Func<IntPtr, T>));

            /// <summary>
            /// 表示 <see cref="IntPtrByRef.RealType"/> 中
            /// <see cref="IntPtrByRef.SetValue{T}(IntPtr, T)"/> 方法的委托。
            /// </summary>
            internal static readonly Action<IntPtr, T> SetValue =
                (Action<IntPtr, T>)IntPtrByRef.RealType.GetMethod(
                    nameof(IntPtrByRef.SetValue)).MakeGenericMethod(
                        typeof(T)).CreateDelegate(typeof(Action<IntPtr, T>));
        }

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/>。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="value">按引用转递的值。</param>
        /// <returns><paramref name="value"/> 的引用转换得到的 <see cref="IntPtr"/>。</returns>
        public static IntPtr ToIntPtr<T>(ref T value)
        {
            return IntPtrByRef.Delegates<T>.ToIntPtr.Invoke(ref value);
        }

        /// <summary>
        /// 获取以指定 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/> 所引用的值。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="reference">
        /// 以 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="reference"/>
        /// 表示的引用传递 <see langword="ref"/> 所引用的值。</returns>
        public static T GetValue<T>(IntPtr reference)
        {
            return IntPtrByRef.Delegates<T>.GetValue.Invoke(reference);
        }

        /// <summary>
        /// 设置以指定 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/> 所引用的值。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="reference">
        /// 以 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/>。</param>
        /// <param name="value">以 <see cref="IntPtr"/> 表示的引用传递
        /// <see langword="ref"/> 所引用的新值。</param>
        public static void SetValue<T>(IntPtr reference, T value)
        {
            IntPtrByRef.Delegates<T>.SetValue.Invoke(reference, value);
        }

        /// <summary>
        /// 创建提供 <see cref="IntPtrByRef"/> 方法实现的类型。
        /// </summary>
        /// <returns>提供 <see cref="IntPtrByRef"/> 方法实现的类型。</returns>
        private static Type CreateRealType()
        {
            var typeName = typeof(IntPtrByRef).ToString();
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(typeName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{typeName}.dll");
            var type = module.DefineType(typeName,
                TypeAttributes.Public | TypeAttributes.Abstract |
                TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            {
                var method = type.DefineMethod(
                    nameof(IntPtrByRef.ToIntPtr), MethodAttributes.Public |
                    MethodAttributes.Static | MethodAttributes.HideBySig);
                var typeParam = method.DefineGenericParameters("T")[0];
                method.SetReturnType(typeof(IntPtr));
                method.SetParameters(typeParam.MakeByRefType());
                method.DefineParameter(1, ParameterAttributes.None, "value");
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ret);
            }

            {
                var method = type.DefineMethod(
                    nameof(IntPtrByRef.GetValue), MethodAttributes.Public |
                    MethodAttributes.Static | MethodAttributes.HideBySig);
                var typeParam = method.DefineGenericParameters("T")[0];
                method.SetReturnType(typeParam);
                method.SetParameters(typeof(IntPtr));
                method.DefineParameter(1, ParameterAttributes.None, "reference");
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldobj, typeParam);
                il.Emit(OpCodes.Ret);
            }

            {
                var method = type.DefineMethod(
                    nameof(IntPtrByRef.SetValue), MethodAttributes.Public |
                    MethodAttributes.Static | MethodAttributes.HideBySig);
                var typeParam = method.DefineGenericParameters("T")[0];
                method.SetReturnType(typeof(void));
                method.SetParameters(typeof(IntPtr), typeParam);
                method.DefineParameter(1, ParameterAttributes.None, "reference");
                method.DefineParameter(2, ParameterAttributes.None, "value");
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Stobj, typeParam);
                il.Emit(OpCodes.Ret);
            }

            return type.CreateTypeInfo();
        }
    }
}
