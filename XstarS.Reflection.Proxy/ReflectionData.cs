using System;
using System.Reflection;

namespace XstarS.Reflection
{
    /// <summary>
    /// 用于存储部分成员反射元数据信息。
    /// </summary>
    internal static class ReflectionData
    {
        /// <summary>
        /// <see cref="Type.GetTypeFromHandle(RuntimeTypeHandle)"/> 静态方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        internal static readonly MethodInfo T_Type_SM_GetTypeFromHandle =
            typeof(Type).GetMethod(
                nameof(Type.GetTypeFromHandle), new[] { typeof(RuntimeTypeHandle) });

        /// <summary>
        /// <see cref="MethodBase.GetMethodFromHandle(RuntimeMethodHandle, RuntimeTypeHandle)"/>
        /// 静态方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        internal static readonly MethodInfo T_MethodBase_SM_GetMethodFromHandle_2 =
            typeof(MethodBase).GetMethod(
                nameof(MethodBase.GetMethodFromHandle),
                new[] { typeof(RuntimeMethodHandle), typeof(RuntimeTypeHandle) });

        /// <summary>
        /// <see cref="MemberInfo.GetCustomAttributes(Type, bool)"/> 实例方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        internal static readonly MethodInfo T_MemberInfo_IM_GetCustomAttributes_2 =
            typeof(MemberInfo).GetMethod(
                nameof(MemberInfo.GetCustomAttributes), new[] { typeof(Type), typeof(bool) });

        /// <summary>
        /// <see cref="MethodInvoker"/> 类型的 <see cref="Type"/> 对象。
        /// </summary>
        internal static readonly Type T_MethodInvoker = typeof(MethodInvoker);

        /// <summary>
        /// <see cref="OnMemberInvokeAttribute"/> 类型的 <see cref="Type"/> 对象。
        /// </summary>
        internal static readonly Type T_OnMemberInvokeAttribute = typeof(OnMemberInvokeAttribute);

        /// <summary>
        /// <see cref="OnMethodInvokeAttribute"/> 类型的 <see cref="Type"/> 对象。
        /// </summary>
        internal static readonly Type T_OnMethodInvokeAttribute = typeof(OnMethodInvokeAttribute);

        /// <summary>
        /// <see cref="HandleOnMethodInvokeAttribute"/> 类型的 <see cref="Type"/> 对象。
        /// </summary>
        internal static readonly Type T_HandleOnMethodInvokeAttribute =
            typeof(HandleOnMethodInvokeAttribute);

        /// <summary>
        /// <see cref="MethodInvoker.MethodInvoker(object, IntPtr)"/>
        /// 实例构造函数的 <see cref="ConstructorInfo"/> 对象。
        /// </summary>
        internal static readonly ConstructorInfo T_MethodInvoker_IC_ctor =
            ReflectionData.T_MethodInvoker.GetConstructors()[0];

        /// <summary>
        /// <see cref="HandleOnMethodInvokeAttribute.HandleOnMethodInvokeAttribute(string)"/>
        /// 实例构造函数的 <see cref="ConstructorInfo"/> 对象。
        /// </summary>
        internal static readonly ConstructorInfo T_HandleOnMethodInvokeAttribute_IC_ctor =
            ReflectionData.T_HandleOnMethodInvokeAttribute.GetConstructor(new[] { typeof(string) });

        /// <summary>
        /// <see cref="MethodInvoker.Invoke(object, object[])"/> 实例方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        internal static readonly MethodInfo T_MethodInvoker_IM_Invoke =
            ReflectionData.T_MethodInvoker.GetMethod(nameof(MethodInvoker.Invoke));

        /// <summary>
        /// <see cref="OnMemberInvokeAttribute.Invoke(MethodInvoker, object, MethodInfo, Type[], object[])"/>
        /// 实例方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        internal static readonly MethodInfo T_OnMemberInvokeAttribute_IM_Invoke =
            ReflectionData.T_OnMemberInvokeAttribute.GetMethod(nameof(OnMemberInvokeAttribute.Invoke));

        /// <summary>
        /// <see cref="OnMethodInvokeAttribute.Invoke(MethodInvoker, object, MethodInfo, Type[], object[])"/>
        /// 实例方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        internal static readonly MethodInfo T_OnMethodInvokeAttribute_IM_Invoke =
            ReflectionData.T_OnMethodInvokeAttribute.GetMethod(nameof(OnMethodInvokeAttribute.Invoke));
    }
}
