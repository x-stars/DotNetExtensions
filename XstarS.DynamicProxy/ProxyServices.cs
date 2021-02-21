using System;
using XstarS.Runtime;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供动态代理相关的服务方法。
    /// </summary>
    public static class ProxyServices
    {
        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/>。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="value">按引用转递的值。</param>
        /// <returns><paramref name="value"/> 的引用转换得到的 <see cref="IntPtr"/>。</returns>
        public static IntPtr ByRefToIntPtr<T>(ref T value) =>
            IntPtrByRefHelper.ToIntPtr(ref value);

        /// <summary>
        /// 获取当前 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/> 所引用的值。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="reference">
        /// 以 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/>。</param>
        /// <returns><paramref name="reference"/>
        /// 表示的引用传递 <see langword="ref"/> 所引用的值。</returns>
        public static T GetRefValue<T>(this IntPtr reference) =>
            IntPtrByRefHelper.GetRefValue<T>(reference);

        /// <summary>
        /// 设置当前 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/> 所引用的值。
        /// </summary>
        /// <typeparam name="T">引用传递 <see langword="ref"/> 的类型。</typeparam>
        /// <param name="reference">
        /// 以 <see cref="IntPtr"/> 表示的引用传递 <see langword="ref"/>。</param>
        /// <param name="value">要设置为 <see cref="IntPtr"/>
        /// 表示的引用传递 <see langword="ref"/> 所引用的值。</param>
        public static void SetRefValue<T>(this IntPtr reference, T value) =>
            IntPtrByRefHelper.SetRefValue(reference, value);
    }
}
