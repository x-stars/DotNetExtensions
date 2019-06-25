using System;
using System.Collections.Generic;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供从原型类型和代理委托构造代理派生类型及其实例的方法。
    /// </summary>
    /// <typeparam name="T">代理类型的原型类型，应为接口或非密封类。</typeparam>
    public sealed class CustomProxyBuilder<T> : ProxyBuilderBase<T> where T : class
    {
        /// <summary>
        /// 初始化 <see cref="CustomProxyBuilder{T}"/> 类的新实例。
        /// </summary>
        /// <exception cref="TypeAccessException">
        /// <typeparamref name="T"/> 不是公共接口，也不是公共非密封类。</exception>
        public CustomProxyBuilder()
        {
            var type = typeof(T);
            this.PrototypeType = type;
            this.InternalBuilder = new CustomProxyBuilder(type);
        }

        /// <summary>
        /// 代理类型的原型类型的 <see cref="Type"/> 对象。
        /// </summary>
        public Type PrototypeType { get; }

        /// <summary>
        /// 用于构造代理类型的 <see cref="CustomProxyBuilder"/> 对象。
        /// </summary>
        private CustomProxyBuilder InternalBuilder { get; }

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="method">要添加代理委托的可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="method"/> 不为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, MethodInfo method) =>
            this.InternalBuilder.AddOnInvoke(handler, method);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, params MethodInfo[] methods) =>
            this.InternalBuilder.AddOnInvoke(handler, methods);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到指定的多个可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methods">要添加代理委托的多个可重写方法的 <see cref="MemberInfo"/> 对象。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="MethodAccessException">
        /// <paramref name="methods"/> 不全为原型类型中的可重写方法。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, IEnumerable<MethodInfo> methods) =>
            this.InternalBuilder.AddOnInvoke(handler, methods);

        /// <summary>
        /// 根据指定规则将指定 <see cref="OnInvokeHandler"/> 代理委托添加到可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <param name="methodFilter">筛选要添加代理委托的方法的 <see cref="Predicate{T}"/> 委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler, Predicate<MethodInfo> methodFilter) =>
            this.InternalBuilder.AddOnInvoke(handler, methodFilter);

        /// <summary>
        /// 将指定 <see cref="OnInvokeHandler"/> 代理委托添加到所有可重写方法。
        /// </summary>
        /// <param name="handler">要添加到方法的 <see cref="OnInvokeHandler"/> 代理委托。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="NotSupportedException">
        /// <see cref="ProxyBuilderBase{T}.ProxyType"/> 已经创建，无法再添加新的代理委托。</exception>
        public void AddOnInvoke(OnInvokeHandler handler) =>
            this.InternalBuilder.AddOnInvoke(handler);

        /// <summary>
        /// 构造代理派生类型。
        /// </summary>
        /// <returns>构造完成的派生类型。</returns>
        protected override Type BuildProxyType() => this.InternalBuilder.ProxyType;
    }
}
