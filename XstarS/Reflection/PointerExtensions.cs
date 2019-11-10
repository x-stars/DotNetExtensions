using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供指针包装 <see cref="Pointer"/> 的扩展方法。
    /// </summary>
    [CLSCompliant(false)]
    public static class PointerExtensions
    {
        /// <summary>
        /// <see cref="Pointer"/> 类型的 <c>GetPointerValue()</c> 方法的委托调用。
        /// </summary>
        private static readonly Func<Pointer, IntPtr> GetPointerValueDelegate =
            typeof(Pointer).GetMethod("GetPointerValue",
                BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(
                    typeof(Func<Pointer, IntPtr>)) as Func<Pointer, IntPtr>;

        /// <summary>
        /// <see cref="Pointer"/> 类型的 <c>GetPointerType()</c> 方法的委托调用。
        /// </summary>
        private static readonly Func<Pointer, Type> GetPointerTypeDelegate =
            typeof(Pointer).GetMethod("GetPointerType",
                BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(
                    typeof(Func<Pointer, Type>)) as Func<Pointer, Type>;

        /// <summary>
        /// 获取当前 <see cref="Pointer"/> 的指针的值。
        /// </summary>
        /// <param name="pointer">要获取指针的值的 <see cref="Pointer"/> 对象。</param>
        /// <returns><paramref name="pointer"/> 的指针的值。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pointer"/> 为 <see langword="null"/>。</exception>
        public static IntPtr GetPointerValue(this Pointer pointer)
        {
            if (pointer is null)
            {
                throw new ArgumentNullException(nameof(pointer));
            }

            return PointerExtensions.GetPointerValueDelegate.Invoke(pointer);
        }

        /// <summary>
        /// 获取当前 <see cref="Pointer"/> 的指针类型。
        /// </summary>
        /// <param name="pointer">要获取指针类型的 <see cref="Pointer"/> 对象。</param>
        /// <returns><paramref name="pointer"/> 的指针类型。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pointer"/> 为 <see langword="null"/>。</exception>
        public static Type GetPointerType(this Pointer pointer)
        {
            if (pointer is null)
            {
                throw new ArgumentNullException(nameof(pointer));
            }

            return PointerExtensions.GetPointerTypeDelegate.Invoke(pointer);
        }
    }
}
