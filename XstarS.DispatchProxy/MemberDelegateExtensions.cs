using System;
using System.Reflection;
using System.Reflection.Emit;
using XstarS.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供创建类型成员调用委托的扩展方法。
    /// </summary>
    internal static class MemberDelegateExtensions
    {
        /// <summary>
        /// 从当前方法创建指定类型的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要创建的委托的类型。</typeparam>
        /// <param name="method">要创建委托的 <see cref="MethodInfo"/>。</param>
        /// <returns><paramref name="method"/> 方法的委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method)
            where TDelegate : Delegate
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return (TDelegate)method.CreateDelegate(typeof(TDelegate));
        }

        /// <summary>
        /// 从当前方法创建具有指定目标的指定类型的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要创建的委托的类型。</typeparam>
        /// <param name="method">要创建委托的 <see cref="MethodInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns><paramref name="method"/> 方法的委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method, object target)
            where TDelegate : Delegate
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return (TDelegate)method.CreateDelegate(typeof(TDelegate), target);
        }

        /// <summary>
        /// 从当前方法创建指定类型的动态调用委托。
        /// </summary>
        /// <param name="method">要创建委托的 <see cref="MethodInfo"/>。</param>
        /// <returns><paramref name="method"/> 方法的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static Func<object, object[], object> CreateDynamicDelegate(this MethodInfo method)
        {
            return method.CreateDynamicMethod().CreateDelegate<Func<object, object[], object>>();
        }

        /// <summary>
        /// 从当前方法创建具有指定目标的指定类型的动态调用委托。
        /// </summary>
        /// <param name="method">要创建委托的 <see cref="MethodInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns><paramref name="method"/> 方法的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        public static Func<object[], object> CreateDynamicDelegate(this MethodInfo method, object target)
        {
            return method.CreateDynamicMethod().CreateDelegate<Func<object[], object>>(target);
        }

        /// <summary>
        /// 从当前方法创建指定类型的动态调用方法。
        /// 方法签名类似于 <see cref="MethodBase.Invoke(object, object[])"/>。
        /// </summary>
        /// <param name="method">要创建方法的 <see cref="MethodInfo"/>。</param>
        /// <returns><paramref name="method"/> 方法的动态调用方法。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
        private static DynamicMethod CreateDynamicMethod(this MethodInfo method)
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            var paramInfos = method.GetParameters();
            var invokeMethod = new DynamicMethod(
                "Invoke", typeof(object), new[] { typeof(object), typeof(object[]) },
                restrictedSkipVisibility: true);
            invokeMethod.DefineParameter(1, ParameterAttributes.None, "instance");
            invokeMethod.DefineParameter(2, ParameterAttributes.None, "arguments");
            var ilGen = invokeMethod.GetILGenerator();
            if (!method.IsStatic)
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.EmitUnbox(method.DeclaringType);
            }
            for (int index = 0; index < paramInfos.Length; index++)
            {
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.EmitLdcI4(index);
                ilGen.Emit(OpCodes.Ldelem_Ref);
                var param = paramInfos[index];
                ilGen.EmitUnbox(param.ParameterType);
            }
            ilGen.Emit(method.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, method);
            if (method.ReturnType == typeof(void))
            {
                ilGen.Emit(OpCodes.Ldnull);
            }
            else
            {
                ilGen.EmitBox(method.ReturnType);
            }
            ilGen.Emit(OpCodes.Ret);
            return invokeMethod;
        }
    }
}
