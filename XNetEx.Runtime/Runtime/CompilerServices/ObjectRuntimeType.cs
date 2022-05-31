using System;
using System.Runtime.CompilerServices;

namespace XNetEx.Runtime.CompilerServices
{
    /// <summary>
    /// 提供对象运行时类型相关的帮助方法。
    /// </summary>
    public static class ObjectRuntimeType
    {
        /// <summary>
        /// 获取当前对象对象的类型句柄 <see cref="IntPtr"/> 的引用。
        /// </summary>
        /// <param name="instance">要获取类型句柄引用的对象。</param>
        /// <returns>对 <paramref name="instance"/> 的类型句柄的引用。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ref IntPtr RefTypeHandle(this object instance)
        {
            return ref **(IntPtr**)Unsafe.AsPointer(ref instance);
        }

        /// <summary>
        /// 创建当前对象的浅表副本，并通过替换类型句柄将其强制转换为指定类型。
        /// </summary>
        /// <typeparam name="T">要强制转换为的类型。</typeparam>
        /// <param name="instance">要强制转换为另一类型的对象。</param>
        /// <returns>已转换为 <typeparamref name="T"/> 类型的
        /// <paramref name="instance"/> 的浅表副本。</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T UncheckedCast<T>(this object instance) where T : class
        {
            var casting = instance.MemberwiseClone();
            casting.RefTypeHandle() = typeof(T).TypeHandle.Value;
            return Unsafe.As<T>(casting);
        }
    }
}
