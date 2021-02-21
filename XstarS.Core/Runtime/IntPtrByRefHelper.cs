using System;
using System.Reflection;
using System.Reflection.Emit;

namespace XstarS.Runtime
{
    /// <summary>
    /// 提供以 <see cref="IntPtr"/> 表示引用传递 <see langword="ref"/> 的帮助方法。
    /// </summary>
    public static class IntPtrByRefHelper
    {
        /// <summary>
        /// <see cref="IntPtrByRefHelper.Implementation"/> 的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<Type> LazyImplementation =
            new Lazy<Type>(IntPtrByRefHelper.CreateImplementation);

        /// <summary>
        /// 获取提供 <see cref="IntPtrByRefHelper"/> 方法实现的类型的 <see cref="Type"/> 对象。
        /// </summary>
        private static Type Implementation => IntPtrByRefHelper.LazyImplementation.Value;

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/>。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="value">按引用转递的值。</param>
        /// <returns><paramref name="value"/> 的引用转换得到的 <see cref="IntPtr"/>。</returns>
        public static IntPtr ToIntPtr<T>(ref T value)
        {
            return IntPtrByRefHelper.Delegates<T>.ToIntPtr.Invoke(ref value);
        }

        /// <summary>
        /// 获取当前 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/> 所引用的值。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="reference">
        /// 以 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="reference"/>
        /// 表示的引用传递 <see langword="ref"/> 所引用的值。</returns>
        public static T GetRefValue<T>(this IntPtr reference)
        {
            return IntPtrByRefHelper.Delegates<T>.GetValue.Invoke(reference);
        }

        /// <summary>
        /// 设置当前 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/> 所引用的值。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="reference">
        /// 以 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/>。</param>
        /// <param name="value">要设置为 <see cref="IntPtr"/>
        /// 表示的引用传递 <see langword="ref"/> 所引用的值。</param>
        public static void SetRefValue<T>(this IntPtr reference, T value)
        {
            IntPtrByRefHelper.Delegates<T>.SetValue.Invoke(reference, value);
        }

        /// <summary>
        /// 创建提供 <see cref="IntPtrByRefHelper"/> 方法实现的类型。
        /// </summary>
        /// <returns>提供 <see cref="IntPtrByRefHelper"/> 方法实现的类型。</returns>
        private static Type CreateImplementation()
        {
            var typeName = typeof(IntPtrByRefHelper).ToString();
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(typeName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{typeName}.dll");
            var type = module.DefineType(typeName,
                TypeAttributes.Public | TypeAttributes.Abstract |
                TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);

            {
                var method = type.DefineMethod(
                    nameof(IntPtrByRefHelper.ToIntPtr), MethodAttributes.Public |
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
                    nameof(IntPtrByRefHelper.GetRefValue), MethodAttributes.Public |
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
                    nameof(IntPtrByRefHelper.SetRefValue), MethodAttributes.Public |
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

        /// <summary>
        /// 封装将引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/> 的方法。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="value">按引用转递的值。</param>
        /// <returns>引用传递 <see langword="ref"/> 转换得到的 <see cref="IntPtr"/>。</returns>
        private delegate IntPtr Converter<T>(ref T value);

        /// <summary>
        /// 提供 <see cref="IntPtrByRefHelper.Implementation"/> 中各方法的委托。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        private static class Delegates<T>
        {
            /// <summary>
            /// 表示 <see cref="IntPtrByRefHelper.Implementation"/> 中
            /// <see cref="IntPtrByRefHelper.ToIntPtr{T}(ref T)"/> 方法的委托。
            /// </summary>
            internal static readonly Converter<T> ToIntPtr =
                (Converter<T>)IntPtrByRefHelper.Implementation.GetMethod(
                    nameof(IntPtrByRefHelper.ToIntPtr)).MakeGenericMethod(
                        typeof(T)).CreateDelegate(typeof(Converter<T>));

            /// <summary>
            /// 表示 <see cref="IntPtrByRefHelper.Implementation"/> 中
            /// <see cref="IntPtrByRefHelper.GetRefValue{T}(IntPtr)"/> 方法的委托。
            /// </summary>
            internal static readonly Func<IntPtr, T> GetValue =
                (Func<IntPtr, T>)IntPtrByRefHelper.Implementation.GetMethod(
                    nameof(IntPtrByRefHelper.GetRefValue)).MakeGenericMethod(
                        typeof(T)).CreateDelegate(typeof(Func<IntPtr, T>));

            /// <summary>
            /// 表示 <see cref="IntPtrByRefHelper.Implementation"/> 中
            /// <see cref="IntPtrByRefHelper.SetRefValue{T}(IntPtr, T)"/> 方法的委托。
            /// </summary>
            internal static readonly Action<IntPtr, T> SetValue =
                (Action<IntPtr, T>)IntPtrByRefHelper.Implementation.GetMethod(
                    nameof(IntPtrByRefHelper.SetRefValue)).MakeGenericMethod(
                        typeof(T)).CreateDelegate(typeof(Action<IntPtr, T>));
        }
    }
}
