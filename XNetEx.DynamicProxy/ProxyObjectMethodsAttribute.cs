using System;

namespace XstarS.Reflection
{
    /// <summary>
    /// 用于标记类型的动态代理类型应代理在 <see cref="object"/> 类型中定义的基础方法。
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, Inherited = false)]
    public sealed class ProxyObjectMethodsAttribute : Attribute
    {
        /// <summary>
        /// 初始化 <see cref="ProxyObjectMethodsAttribute"/> 类的新实例。
        /// </summary>
        public ProxyObjectMethodsAttribute() { }
    }
}
