using System;
using System.Collections.Concurrent;
using XstarS.Reflection;

namespace XstarS
{
    /// <summary>
    /// 提供委托 <see cref="Delegate"/> 的扩展方法。
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// 表示委托类型对应的动态调用委托。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Func<object, object[], object>> DynamicDelegates =
            new ConcurrentDictionary<Type, Func<object, object[], object>>();

        /// <summary>
        /// 以构造的动态调用委托调用由当前委托所表示的方法。
        /// </summary>
        /// <param name="delegate">要进行动态调用的 <see cref="Delegate"/> 对象。</param>
        /// <param name="arguments">作为自变量传递给当前委托所表示的方法的对象数组。</param>
        /// <returns><paramref name="delegate"/> 所表示的方法返回的对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static object DynamicInvokeFast(this Delegate @delegate, params object[] arguments)
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }
            arguments ??= Array.Empty<object>();

            var dynamicDelegate = DelegateExtensions.DynamicDelegates.GetOrAdd(@delegate.GetType(),
                newDelegateType => newDelegateType.GetMethod(nameof(Action.Invoke)).CreateDynamicDelegate());
            return dynamicDelegate.Invoke(@delegate, arguments);
        }

        /// <summary>
        /// 创建当前委托对应的动态调用委托。
        /// </summary>
        /// <param name="delegate">要创建动态调用委托的 <see cref="Delegate"/> 对象。</param>
        /// <returns><paramref name="delegate"/> 的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static Func<object[], object> ToDynamicDelegate(this Delegate @delegate)
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            return @delegate.GetType().GetMethod(nameof(Action.Invoke)).CreateDynamicDelegate(@delegate);
        }
    }
}
