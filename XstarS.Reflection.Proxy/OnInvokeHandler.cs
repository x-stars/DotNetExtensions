using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 表示方法调用时所用的代理。
    /// </summary>
    /// <param name="invokeInfo">提供调用代理方法所用的信息。</param>
    /// <returns>调用方法的 <see cref="MethodInvoker"/> 委托得到返回值。</returns>
    public delegate object OnInvokeHandler(ProxyInvokeInfo invokeInfo);
}
