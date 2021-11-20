using System;
using System.Linq.Expressions;
using System.Reflection;

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
            var paramExprs = Array.ConvertAll(constructor.GetParameters(),
                parameter => Expression.Parameter(parameter.ParameterType, parameter.Name));
            var newExpr = Expression.New(constructor, paramExprs);
            var lambda = Expression.Lambda(delegateType, newExpr, paramExprs);
            return lambda.Compile();
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

            var instExpr = field.IsStatic ? null : Expression.Parameter(field.DeclaringType!, "instance");
            var fieldExpr = Expression.Field(instExpr, field);
            var paramExprs = (instExpr is null) ? Array.Empty<ParameterExpression>() : new[] { instExpr };
            var lambda = Expression.Lambda(delegateType, fieldExpr, paramExprs);
            return lambda.Compile();
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

            var instExpr = field.IsStatic ? null : Expression.Constant(target);
            var fieldExpr = Expression.Field(instExpr, field);
            var lambda = Expression.Lambda(delegateType, fieldExpr);
            return lambda.Compile();
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

            var instExpr = field.IsStatic ? null : Expression.Parameter(field.DeclaringType!, "instance");
            var valueExpr = Expression.Parameter(field.FieldType, "value");
            var fieldExpr = Expression.Field(instExpr, field);
            var assignExpr = Expression.Assign(fieldExpr, valueExpr);
            var paramExprs = (instExpr is null) ? new[] { valueExpr } : new[] { instExpr, valueExpr };
            var lambda = Expression.Lambda(delegateType, assignExpr, paramExprs);
            return lambda.Compile();
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

            var instExpr = field.IsStatic ? null : Expression.Constant(target);
            var valueExpr = Expression.Parameter(field.FieldType, "value");
            var fieldExpr = Expression.Field(instExpr, field);
            var assignExpr = Expression.Assign(fieldExpr, valueExpr);
            var lambda = Expression.Lambda(delegateType, assignExpr, valueExpr);
            return lambda.Compile();
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
            return (TDelegate)method.CreateDelegate(typeof(TDelegate), target);
        }
#endif
    }
}
