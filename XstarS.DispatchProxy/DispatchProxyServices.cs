using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供 <see cref="DispatchProxy{TInterface}"/> 相关的服务方法。
    /// </summary>
    public static class DispatchProxyServices
    {
        /// <summary>
        /// 表示默认的 <see cref="InvocationHandler"/> 代理委托。
        /// 此代理委托仅调用被代理方法并返回。
        /// </summary>
        public static readonly InvocationHandler DefaultHandler =
            (instance, method, arguments) => method.GetDynamicDelegate().Invoke(instance, arguments);

        /// <summary>
        /// 表示方法对应的动态调用委托。
        /// </summary>
        private static readonly ConcurrentDictionary<MethodInfo, Func<object, object[], object>> DynamicDelegates =
            new ConcurrentDictionary<MethodInfo, Func<object, object[], object>>();

        /// <summary>
        /// 获取当前方法的动态调用委托。
        /// </summary>
        /// <param name="method">要获取动态调用委托的 <see cref="MethodInfo"/>。</param>
        /// <returns><paramref name="method"/> 方法的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static Func<object, object[], object> GetDynamicDelegate(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return DispatchProxyServices.DynamicDelegates.GetOrAdd(method,
                newMethod => newMethod.CreateDynamicDelegate());
        }
    }
}
