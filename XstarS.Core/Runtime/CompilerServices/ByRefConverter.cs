using System;
using System.Reflection;
using System.Reflection.Emit;
using XstarS.Reflection.Emit;

namespace XstarS.Runtime.CompilerServices
{
    /// <summary>
    /// 提供转换引用传递 <see langword="ref"/> 类型的方法。
    /// </summary>
    public abstract unsafe class ByRefConverter
    {
        /// <summary>
        /// 表示提供 <see cref="ByRefConverter"/> 方法实现的延迟初始化对象。
        /// </summary>
        private static readonly Lazy<ByRefConverter> LazyImplementation =
            new Lazy<ByRefConverter>(ByRefConverter.CreateImplementation);

        /// <summary>
        /// 初始化 <see cref="ByRefConverter"/> 类型的新实例。
        /// 仅用于内部实现，用户不应继承此类型。
        /// </summary>
        [Obsolete("This constructor is for internal usages only.", error: true)]
        protected ByRefConverter() { }

        /// <summary>
        /// 表示提供 <see cref="ByRefConverter"/> 方法实现的对象。
        /// </summary>
        private static ByRefConverter Implementation =>
            ByRefConverter.LazyImplementation.Value;

        /// <summary>
        /// 将指定的 <see cref="IntPtr"/> 转换为等效的引用传递 <see langword="ref"/>。
        /// </summary>
        /// <typeparam name="T">按引用转递的值的类型。</typeparam>
        /// <param name="pointer">引用值的 <see cref="IntPtr"/>。</param>
        /// <returns><paramref name="pointer"/> 对应的引用转递 <see langword="ref"/>。</returns>
        public static ref T FromIntPtr<T>(IntPtr pointer) =>
            ref ByRefConverter.Implementation.ConvertFromIntPtr<T>(pointer);

        /// <summary>
        /// 将指定的指针转换为对应类型的引用传递 <see langword="ref"/>。
        /// </summary>
        /// <typeparam name="T">指针指向的值的类型。</typeparam>
        /// <param name="pointer">指向指定类型值的指针。</param>
        /// <returns><paramref name="pointer"/> 对应的引用转递 <see langword="ref"/>。</returns>
        [CLSCompliant(false)]
        public static ref T FromPointer<T>(T* pointer) where T : unmanaged =>
            ref ByRefConverter.FromIntPtr<T>((IntPtr)pointer);

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/>。
        /// </summary>
        /// <typeparam name="T">按引用传递的值的类型。</typeparam>
        /// <param name="reference">要进行转换的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="reference"/> 转换得到的 <see cref="IntPtr"/>。</returns>
        public static IntPtr ToIntPtr<T>(ref T reference) =>
            ByRefConverter.Implementation.ConvertToIntPtr<T>(ref reference);

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为对应类型的指针。
        /// </summary>
        /// <typeparam name="T">按引用传递的值的类型。</typeparam>
        /// <param name="reference">要进行转换的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="reference"/> 转换得到的对应类型的指针。</returns>
        [CLSCompliant(false)]
        public static T* ToPointer<T>(ref T reference) where T : unmanaged =>
            (T*)ByRefConverter.ToIntPtr<T>(ref reference);

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为指定的另一类型的引用传递。
        /// </summary>
        /// <typeparam name="TInput">按引用传递的值的类型。</typeparam>
        /// <typeparam name="TOutput">要转换到的按引用传递的值的类型。</typeparam>
        /// <param name="value">要进行类型转换的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="value"/> 转换到另一类型的引用传递。</returns>
        public static ref TOutput ChangeType<TInput, TOutput>(ref TInput value) =>
            ref ByRefConverter.Implementation.ChangeRefType<TInput, TOutput>(ref value);

        /// <summary>
        /// 将指定的 <see cref="IntPtr"/> 转换为等效的引用传递 <see langword="ref"/>。
        /// </summary>
        /// <typeparam name="T">按引用转递的值的类型。</typeparam>
        /// <param name="pointer">引用值的 <see cref="IntPtr"/>。</param>
        /// <returns><paramref name="pointer"/> 对应的引用转递 <see langword="ref"/>。</returns>
        public abstract ref T ConvertFromIntPtr<T>(IntPtr pointer);

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/>。
        /// </summary>
        /// <typeparam name="T">按引用传递的值的类型。</typeparam>
        /// <param name="reference">要进行转换的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="reference"/> 转换得到的 <see cref="IntPtr"/>。</returns>
        public abstract IntPtr ConvertToIntPtr<T>(ref T reference);

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为指定的另一类型的引用传递。
        /// </summary>
        /// <typeparam name="TInput">按引用传递的值的类型。</typeparam>
        /// <typeparam name="TOutput">要转换到的按引用传递的值的类型。</typeparam>
        /// <param name="value">要进行类型转换的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="value"/> 转换到另一类型的引用传递。</returns>
        public abstract ref TOutput ChangeRefType<TInput, TOutput>(ref TInput value);

        /// <summary>
        /// 创建提供 <see cref="ByRefConverter"/> 方法实现的对象。
        /// </summary>
        /// <returns>提供 <see cref="ByRefConverter"/> 方法实现的对象。</returns>
        private static ByRefConverter CreateImplementation()
        {
            var baseType = typeof(ByRefConverter);
            var typeName = baseType.ToString();
            var assembly = AssemblyBuilder.DefineDynamicAssembly(
                new AssemblyName(typeName), AssemblyBuilderAccess.Run);
            var module = assembly.DefineDynamicModule($"{typeName}.dll");
            var type = module.DefineType(typeName, TypeAttributes.Public |
                TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit, baseType);

            var constructor = type.DefineDefaultConstructor(MethodAttributes.Public);
            var baseMethods = baseType.GetMethods(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (var baseMethod in baseMethods)
            {
                var method = type.DefineMethodOverride(baseMethod);
                var il = method.GetILGenerator();
                il.Emit(OpCodes.Ldarg_1);
                il.Emit(OpCodes.Ret);
            }

            return (ByRefConverter)Activator.CreateInstance(type.CreateTypeInfo());
        }
    }
}
