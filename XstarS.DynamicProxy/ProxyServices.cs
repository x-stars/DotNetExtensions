using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供动态代理相关的服务方法。
    /// </summary>
    public static class ProxyServices
    {
        /// <summary>
        /// 将当前 <see cref="IntPtr"/> 转换为等效的引用传递 <see langword="ref"/>。
        /// </summary>
        /// <typeparam name="T">按引用转递的值的类型。</typeparam>
        /// <param name="pointer">引用值的 <see cref="IntPtr"/>。</param>
        /// <returns><paramref name="pointer"/> 对应的引用转递 <see langword="ref"/>。</returns>
        public static ref T ToByRef<T>(this nint pointer) => ref ByRefConverter.ToByRef<T>(pointer);

        /// <summary>
        /// 将指定的引用传递 <see langword="ref"/> 转换为等效的 <see cref="IntPtr"/>。
        /// </summary>
        /// <typeparam name="T">按引用转递的值的类型。</typeparam>
        /// <param name="value">按引用转递的值。</param>
        /// <returns><paramref name="value"/> 的引用转换得到的 <see cref="IntPtr"/>。</returns>
        public static nint ToIntPtr<T>(ref T value) => ByRefConverter.ToIntPtr(ref value);
    }
}
