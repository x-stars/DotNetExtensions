using System;
using System.Runtime.CompilerServices;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供转换引用传递 <see langword="ref"/> 类型的方法。
    /// </summary>
    public static unsafe class ByRefConverter
    {
        /// <summary>
        /// 将指定的 <see cref="IntPtr"/> 转换为等效的引用传递 <see langword="ref"/>。
        /// </summary>
        /// <typeparam name="T">按引用转递的值的类型。</typeparam>
        /// <param name="pointer">引用值的 <see cref="IntPtr"/>。</param>
        /// <returns><paramref name="pointer"/> 对应的引用转递 <see langword="ref"/>。</returns>
        public static ref T ToByRef<T>(nint pointer) => ref Unsafe.AsRef<T>((void*)pointer);

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/>。
        /// </summary>
        /// <typeparam name="T">按引用传递的值的类型。</typeparam>
        /// <param name="reference">要进行转换的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="reference"/> 转换得到的 <see cref="IntPtr"/>。</returns>
        public static nint ToIntPtr<T>(ref T reference) => (nint)Unsafe.AsPointer(ref reference);
    }
}
