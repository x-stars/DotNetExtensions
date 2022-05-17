using System;
using System.Runtime.CompilerServices;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供转换引用传递 <see langword="ref"/> 类型的方法。
    /// </summary>
    public static unsafe class ByRefUnsafeConverter
    {
        /// <summary>
        /// 将指定的 <see cref="IntPtr"/> 表示的地址转换为等效的 <typeparamref name="T"/> 类型的值的引用。
        /// </summary>
        /// <typeparam name="T">按引用传递的值的类型。</typeparam>
        /// <param name="pointer">指向 <typeparamref name="T"/> 类型的值的 <see cref="IntPtr"/>。</param>
        /// <returns>对 <paramref name="pointer"/> 指向的 <typeparamref name="T"/> 类型的值的引用。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T ToByRef<T>(nint pointer) => ref Unsafe.AsRef<T>((void*)pointer);

        /// <summary>
        /// 将指定的 <typeparamref name="T"/> 类型的值的引用转换为等效的 <see cref="IntPtr"/> 表示的地址。
        /// </summary>
        /// <typeparam name="T">按引用传递的值的类型。</typeparam>
        /// <param name="reference">要获取地址的 <typeparamref name="T"/> 类型的值的引用。</param>
        /// <returns>指向 <paramref name="reference"/> 引用的值并以 <see cref="IntPtr"/> 表示的地址。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static nint ToIntPtr<T>(ref T reference) => (nint)Unsafe.AsPointer(ref reference);

        /// <summary>
        /// 获取指定装箱值类型中已装箱的 <typeparamref name="T"/> 类型的值的引用。
        /// </summary>
        /// <typeparam name="T">已装箱的值的类型。</typeparam>
        /// <param name="box">已装箱为 <see cref="object"/> 的值类型。</param>
        /// <returns>对 <paramref name="box"/> 中已装箱的 <typeparamref name="T"/> 类型的值的引用。</returns>
        /// <exception cref="NullReferenceException">
        /// <paramref name="box"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="InvalidCastException">
        /// <paramref name="box"/> 不为装箱的 <typeparamref name="T"/> 类型的值。</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T RefBoxed<T>(object box) where T : struct => ref Unsafe.Unbox<T>(box);
    }
}
