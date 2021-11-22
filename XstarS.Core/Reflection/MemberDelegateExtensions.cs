﻿using System;
using System.Reflection;
using System.Reflection.Emit;
using XstarS.Reflection.Emit;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供创建类型成员调用委托的扩展方法。
    /// </summary>
    public static partial class MemberDelegateExtensions
    {
        /// <summary>
        /// 从当前构造函数创建指定类型的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要创建的委托的类型。</typeparam>
        /// <param name="constructor">要创建委托的 <see cref="ConstructorInfo"/>。</param>
        /// <returns><paramref name="constructor"/> 构造函数的委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="constructor"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="constructor"/> 表示一个类构造函数（或称类初始化器）。</exception>
        public static TDelegate CreateDelegate<TDelegate>(this ConstructorInfo constructor)
            where TDelegate : Delegate
        {
            return (TDelegate)constructor.CreateDelegate(typeof(TDelegate));
        }

        /// <summary>
        /// 从当前构造函数创建指定类型的委托。
        /// </summary>
        /// <param name="constructor">要创建委托的 <see cref="ConstructorInfo"/>。</param>
        /// <param name="delegateType">要创建的委托的类型。</param>
        /// <returns><paramref name="constructor"/> 构造函数的委托。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="constructor"/>
        /// 或 <paramref name="delegateType"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="constructor"/> 表示一个类构造函数（或称类初始化器）；或
        /// <paramref name="delegateType"/> 表示的类型不从 <see cref="Delegate"/> 派生。</exception>
        public static Delegate CreateDelegate(this ConstructorInfo constructor, Type delegateType)
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
            if (delegateType is null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
            {
                var inner = new InvalidCastException();
                throw new ArgumentException(inner.Message, nameof(delegateType), inner);
            }

            var paramInfos = constructor.GetParameters();
            var paramTypes = Array.ConvertAll(paramInfos, param => param.ParameterType);
            var createMethod = new DynamicMethod(
                "CreateInstance", constructor.DeclaringType, paramTypes,
                restrictedSkipVisibility: true);
            var ilGen = createMethod.GetILGenerator();
            for (int index = 0; index < paramInfos.Length; index++)
            {
                var param = paramInfos[index];
                createMethod.DefineParameter(index, param.Attributes, param.Name);
                ilGen.EmitLdarg(index);
            }
            ilGen.Emit(OpCodes.Newobj, constructor);
            ilGen.Emit(OpCodes.Ret);
            return createMethod.CreateDelegate(delegateType);
        }

        /// <summary>
        /// 从当前字段创建指定类型的获取值的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要创建的委托的类型。</typeparam>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static TDelegate CreateGetDelegate<TDelegate>(this FieldInfo field)
            where TDelegate : Delegate
        {
            return (TDelegate)field.CreateGetDelegate(typeof(TDelegate));
        }

        /// <summary>
        /// 从当前字段创建指定类型的获取值的委托。
        /// </summary>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="delegateType">要创建的委托的类型。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="field"/>
        /// 或 <paramref name="delegateType"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="delegateType"/> 表示的类型不从 <see cref="Delegate"/> 派生。</exception>
        public static Delegate CreateGetDelegate(this FieldInfo field, Type delegateType)
        {
            return field.CreateGetDelegate(delegateType, target: null);
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的获取值的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要创建的委托的类型。</typeparam>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static TDelegate CreateGetDelegate<TDelegate>(this FieldInfo field, object? target)
            where TDelegate : Delegate
        {
            return (TDelegate)field.CreateGetDelegate(typeof(TDelegate), target);
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的获取值的委托。
        /// </summary>
        /// <param name="field">要创建获取值委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="delegateType">要创建的委托的类型。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>获取 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="field"/>
        /// 或 <paramref name="delegateType"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="delegateType"/> 表示的类型不从 <see cref="Delegate"/> 派生。</exception>
        public static Delegate CreateGetDelegate(this FieldInfo field, Type delegateType, object? target)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if (delegateType is null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
            {
                var inner = new InvalidCastException();
                throw new ArgumentException(inner.Message, nameof(delegateType), inner);
            }

            var paramTypes = field.IsStatic ?
                Type.EmptyTypes : new[] { field.DeclaringType };
            var getMethod = new DynamicMethod(
                "GetValue", field.FieldType, paramTypes,
                restrictedSkipVisibility: true);
            var ilGen = getMethod.GetILGenerator();
            if (field.IsStatic)
            {
                ilGen.Emit(OpCodes.Ldsfld, field);
            }
            else
            {
                var paramAttr = ParameterAttributes.None;
                getMethod.DefineParameter(0, paramAttr, "instance");
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Ldfld, field);
            }
            ilGen.Emit(OpCodes.Ret);
            return getMethod.CreateDelegate(delegateType, target);
        }

        /// <summary>
        /// 从当前字段创建指定类型的设置值的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要创建的委托的类型。</typeparam>
        /// <param name="field">要创建设置值委托的 <see cref="FieldInfo"/>。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static TDelegate CreateSetDelegate<TDelegate>(this FieldInfo field)
            where TDelegate : Delegate
        {
            return (TDelegate)field.CreateSetDelegate(typeof(TDelegate));
        }

        /// <summary>
        /// 从当前字段创建指定类型的设置值的委托。
        /// </summary>
        /// <param name="field">要创建设置值委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="delegateType">要创建的委托的类型。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="field"/>
        /// 或 <paramref name="delegateType"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="delegateType"/> 表示的类型不从 <see cref="Delegate"/> 派生。</exception>
        public static Delegate CreateSetDelegate(this FieldInfo field, Type delegateType)
        {
            return field.CreateSetDelegate(delegateType, target: null);
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的设置值的委托。
        /// </summary>
        /// <typeparam name="TDelegate">要创建的委托的类型。</typeparam>
        /// <param name="field">要创建委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="field"/> 为 <see langword="null"/>。</exception>
        public static TDelegate CreateSetDelegate<TDelegate>(this FieldInfo field, object? target)
            where TDelegate : Delegate
        {
            return (TDelegate)field.CreateSetDelegate(typeof(TDelegate), target);
        }

        /// <summary>
        /// 从当前字段创建具有指定目标的指定类型的设置值的委托。
        /// </summary>
        /// <param name="field">要创建设置值委托的 <see cref="FieldInfo"/>。</param>
        /// <param name="delegateType">要创建的委托的类型。</param>
        /// <param name="target">由委托将其作为目标的对象。</param>
        /// <returns>设置 <paramref name="field"/> 字段值的委托。</returns>
        /// <exception cref="ArgumentNullException"><paramref name="field"/>
        /// 或 <paramref name="delegateType"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="delegateType"/> 表示的类型不从 <see cref="Delegate"/> 派生。</exception>
        public static Delegate CreateSetDelegate(this FieldInfo field, Type delegateType, object? target)
        {
            if (field is null)
            {
                throw new ArgumentNullException(nameof(field));
            }
            if (delegateType is null)
            {
                throw new ArgumentNullException(nameof(delegateType));
            }
            if (!typeof(Delegate).IsAssignableFrom(delegateType))
            {
                var inner = new InvalidCastException();
                throw new ArgumentException(inner.Message, nameof(delegateType), inner);
            }

            var paramTypes = field.IsStatic ?
                new[] { field.FieldType } :
                new[] { field.DeclaringType, field.FieldType };
            var setMethod = new DynamicMethod(
                "SetValue", typeof(void), paramTypes,
                restrictedSkipVisibility: true);
            var ilGen = setMethod.GetILGenerator();
            if (field.IsStatic)
            {
                var paramAttr = ParameterAttributes.None;
                setMethod.DefineParameter(0, paramAttr, "value");
                ilGen.Emit(OpCodes.Ldarg_0);
                ilGen.Emit(OpCodes.Stsfld, field);
            }
            else
            {
                var paramAttr = ParameterAttributes.None;
                setMethod.DefineParameter(0, paramAttr, "instance");
                ilGen.Emit(OpCodes.Ldarg_0);
                setMethod.DefineParameter(1, paramAttr, "value");
                ilGen.Emit(OpCodes.Ldarg_1);
                ilGen.Emit(OpCodes.Stfld, field);
            }
            ilGen.Emit(OpCodes.Ret);
            return setMethod.CreateDelegate(delegateType, target);
        }

#if !NET5_0_OR_GREATER
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
        public static TDelegate CreateDelegate<TDelegate>(this MethodInfo method, object? target)
            where TDelegate : Delegate
        {
            if (method is null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            return (TDelegate)method.CreateDelegate(typeof(TDelegate), target);
        }
#endif
    }
}
