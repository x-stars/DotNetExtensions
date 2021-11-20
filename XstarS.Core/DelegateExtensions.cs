using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.Serialization;
using XstarS.Reflection;

namespace XstarS
{
    /// <summary>
    /// 提供委托 <see cref="Delegate"/> 的扩展方法。
    /// </summary>
    public static class DelegateExtensions
    {
        /// <summary>
        /// 表示委托 <see cref="Delegate"/> 的所有实例字段。
        /// </summary>
        private static readonly FieldInfo[] DelegateFields =
            typeof(MulticastDelegate).GetFields(
                BindingFlags.Instance | BindingFlags.NonPublic);

        /// <summary>
        /// 表示委托类型对应的动态调用委托。
        /// </summary>
        private static readonly ConcurrentDictionary<Type, Func<object?, object?[], object?>> DynamicDelegates =
            new ConcurrentDictionary<Type, Func<object?, object?[], object?>>();

        /// <summary>
        /// 确定当前委托是否是否能转换为另一类型的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要转换到的委托的类型。</typeparam>
        /// <param name="delegate">要确定是否能转换的委托。</param>
        /// <returns>若 <paramref name="delegate"/> 的参数列表和返回值与
        /// <typeparamref name="TDelegate"/> 的相同，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static bool CanConvertTo<TDelegate>(this Delegate @delegate)
            where TDelegate : Delegate
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            var comparer = MethodSignatureEqualityComparer.Default;
            var sourceInvoke = @delegate.GetType().GetMethod("Invoke");
            var targetInvoke = typeof(TDelegate).GetMethod("Invoke");
            return comparer.Equals(sourceInvoke, targetInvoke);
        }

        /// <summary>
        /// 将当前委托转换为另一种类型的委托。此方法并不会检查委托的适配性，使用
        /// <see cref="DelegateExtensions.CanConvertTo{TDelegate}(Delegate)"/>
        /// 方法检查当前委托是否能够转换为 <typeparamref name="TDelegate"/> 类型。
        /// </summary>
        /// <typeparam name="TDelegate">要转换到的委托类型。</typeparam>
        /// <param name="delegate">要转换为另一类型的委托。</param>
        /// <returns>转换为 <typeparamref name="TDelegate"/>
        /// 类型的 <paramref name="delegate"/> 委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static TDelegate ChangeType<TDelegate>(this Delegate @delegate)
            where TDelegate : Delegate
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            var result = FormatterServices.GetUninitializedObject(typeof(TDelegate));
            var fields = DelegateExtensions.DelegateFields;
            var data = FormatterServices.GetObjectData(@delegate, fields);
            FormatterServices.PopulateObjectMembers(result, fields, data);
            return (TDelegate)result;
        }

        /// <summary>
        /// 以构造的动态调用委托调用由当前委托所表示的方法。
        /// </summary>
        /// <param name="delegate">要进行动态调用的 <see cref="Delegate"/> 对象。</param>
        /// <param name="arguments">作为自变量传递给当前委托所表示的方法的对象数组。</param>
        /// <returns><paramref name="delegate"/> 所表示的方法返回的对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static object? DynamicInvokeFast(this Delegate @delegate, params object?[]? arguments)
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }
            arguments ??= Array.Empty<object>();

            var dynamicDelegate = DelegateExtensions.DynamicDelegates.GetOrAdd(@delegate.GetType(),
                newDelegateType => newDelegateType.GetMethod(nameof(Action.Invoke))!.CreateDynamicDelegate());
            return dynamicDelegate.Invoke(@delegate, arguments);
        }

        /// <summary>
        /// 创建当前委托对应的动态调用委托。
        /// </summary>
        /// <param name="delegate">要创建动态调用委托的 <see cref="Delegate"/> 对象。</param>
        /// <returns><paramref name="delegate"/> 的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="delegate"/> 为 <see langword="null"/>。</exception>
        public static Func<object?[], object?> ToDynamicDelegate(this Delegate @delegate)
        {
            if (@delegate is null)
            {
                throw new ArgumentNullException(nameof(@delegate));
            }

            return @delegate.GetType().GetMethod(nameof(Action.Invoke))!.CreateDynamicDelegate(@delegate);
        }
    }
}
