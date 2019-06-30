using System;
using System.Collections.Concurrent;

namespace XstarS.Reflection
{
    /// <summary>
    /// 用于获取指定类型的默认值。
    /// </summary>
    internal static class Default
    {
        /// <summary>
        /// 用于存储类型的默认值。
        /// </summary>
        /// <typeparam name="T">默认值的类型。</typeparam>
        private static class Storage<T>
        {
            /// <summary>
            /// <typeparamref name="T"/> 类型的默认值。
            /// </summary>
#pragma warning disable CS0649
            public static readonly T Value;
#pragma warning restore CS0649
        }

        /// <summary>
        /// 类型对应的默认值。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, object> Values =
            new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// 获取指定类型的默认值，即通过默认值表达式 <see langword="default"/> 获得的值。
        /// </summary>
        /// <param name="type">要获取默认值的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 类型对应的默认值。</returns>
        public static object Value(Type type) =>
            Default.Values.GetOrAdd(type, Default.GetValue);

        /// <summary>
        /// 通过反射获取指定类型的默认值。
        /// </summary>
        /// <param name="type">要获取默认值的 <see cref="Type"/> 对象。</param>
        /// <returns><paramref name="type"/> 类型对应的默认值。</returns>
        private static object GetValue(Type type) =>
            typeof(Default.Storage<>).MakeGenericType(type).GetField(
                nameof(Default.Storage<object>.Value)).GetValue(null);
    }
}
