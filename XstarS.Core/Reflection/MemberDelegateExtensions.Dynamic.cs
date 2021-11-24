using System;
using System.Reflection;
using System.Reflection.Emit;
using XstarS.Reflection.Emit;

namespace XstarS.Reflection
{
    public static partial class MemberDelegateExtensions
    {
        /// <summary>
        /// 从当前构造函数创建动态调用委托。
        /// </summary>
        /// <param name="constructor">要创建委托的 <see cref="ConstructorInfo"/>。</param>
        /// <returns><paramref name="constructor"/> 构造函数的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="constructor"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="constructor"/> 表示一个类构造函数（或称类初始化器）。</exception>
        public static Func<object[], object> CreateDynamicDelegate(this ConstructorInfo constructor)
        {
            if (constructor is null)
            {
                throw new ArgumentNullException(nameof(constructor));
            }
            if (constructor.IsStatic)
            {
                var inner = new InvalidOperationException();
                throw new ArgumentException(inner.Message, nameof(constructor), inner);
            }

            var paramInfos = constructor.GetParameters();
            var createMethod = new DynamicMethod(
                "CreateInstance", typeof(object), new[] { typeof(object[]) },
                restrictedSkipVisibility: true);
            createMethod.DefineParameter(1, ParameterAttributes.None, "arguments");
            var ilGen = createMethod.GetILGenerator();
            for (int index = 0; index < paramInfos.Length; index++)
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.EmitLdcI4(index);
                ilGen.Emit(OpCodes.Ldelem_Ref);
                var param = paramInfos[index];
                ilGen.EmitUnbox(param.ParameterType);
            }
            ilGen.Emit(OpCodes.Newobj, constructor);
            ilGen.EmitBox(constructor.DeclaringType);
            ilGen.Emit(OpCodes.Ret);
            return createMethod.CreateDelegate<Func<object[], object>>();
        }

        /// <summary>
        /// 从当前字段创建指定类型的获取值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Func<object, object> CreateDynamicGetDelegate(this FieldInfo field)
        {
            return field.CreateDynamicGetMethod().CreateDelegate<Func<object, object>>();
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的获取值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Func<object> CreateDynamicGetDelegate(this FieldInfo field, object target)
        {
            return field.CreateDynamicGetMethod().CreateDelegate<Func<object>>(target);
        }

        /// <summary>
        /// 从当前字段创建获取值的动态调用方法。
        /// 方法签名类似于 <see cref="FieldInfo.GetValue(object)"/>。
        /// </summary>
        /// <param name="field">要创建获取值方法的 <see cref="FieldInfo"/>。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的动态调用方法。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        private static DynamicMethod CreateDynamicGetMethod(this FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var getMethod = new DynamicMethod(
                "GetValue", typeof(object), new[] { typeof(object) },
                restrictedSkipVisibility: true);
            getMethod.DefineParameter(1, ParameterAttributes.None, "instance");
            var ilGen = getMethod.GetILGenerator();
            if (!field.IsStatic)
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.EmitUnbox(field.DeclaringType);
            }
            ilGen.Emit(field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field);
            ilGen.EmitBox(field.FieldType);
            ilGen.Emit(OpCodes.Ret);
            return getMethod;
        }

        /// <summary>
        /// 从当前字段创建指定类型的设置值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建设置值委托的 <see cref="FieldInfo"/>。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Action<object, object> CreateDynamicSetDelegate(this FieldInfo field)
        {
            return field.CreateDynamicSetMethod().CreateDelegate<Action<object, object>>();
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的设置值的动态调用委托。
        /// </summary>
        /// <param name="field">要创建委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的动态调用委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static Action<object> CreateDynamicSetDelegate(this FieldInfo field, object target)
        {
            return field.CreateDynamicSetMethod().CreateDelegate<Action<object>>(target);
        }

        /// <summary>
        /// 从当前字段创建设置值的动态调用方法。
        /// 方法签名类似于 <see cref="FieldInfo.SetValue(object, object)"/>。
        /// </summary>
        /// <param name="field">要创建设置值方法的 <see cref="FieldInfo"/>。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的动态调用方法。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        private static DynamicMethod CreateDynamicSetMethod(this FieldInfo field)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }

            var setMethod = new DynamicMethod(
                "SetValue", typeof(void), new[] { typeof(object), typeof(object) },
                restrictedSkipVisibility: true);
            setMethod.DefineParameter(1, ParameterAttributes.None, "instance");
            setMethod.DefineParameter(2, ParameterAttributes.None, "value");
            var ilGen = setMethod.GetILGenerator();
            if (!field.IsStatic)
            {
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.EmitUnbox(field.DeclaringType);
            }
            ilGen.Emit(OpCodes.Ldarg_1);
            ilGen.EmitUnbox(field.FieldType);
            ilGen.Emit(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);
            ilGen.Emit(OpCodes.Ret);
            return setMethod;
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
