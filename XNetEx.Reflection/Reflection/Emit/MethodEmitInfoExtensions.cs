using System;
using System.Reflection;

namespace XNetEx.Reflection.Emit;

/// <summary>
/// 提供 <see cref="MethodInfo"/> 与反射发出相关的元数据的扩展方法。
/// </summary>
public static class MethodEmitInfoExtensions
{
    /// <summary>
    /// 确定当前 <see cref="MethodBase"/> 是否为程序集外部可继承的实例方法。
    /// </summary>
    /// <param name="method">要进行检查的 <see cref="MethodBase"/> 对象。</param>
    /// <returns>若 <paramref name="method"/> 是程序集外部可继承的实例方法，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
    public static bool IsInheritable(this MethodBase method)
    {
        if (method is null)
        {
            throw new ArgumentNullException(nameof(method));
        }

        return method.DeclaringType!.IsVisible && !method.DeclaringType.IsSealed &&
            !method.IsStatic && (method.IsPublic || method.IsFamily || method.IsFamilyOrAssembly);
    }

    /// <summary>
    /// 确定当前 <see cref="MethodInfo"/> 是否为程序集外部可重写的方法。
    /// </summary>
    /// <param name="method">要进行检查的 <see cref="MethodInfo"/> 对象。</param>
    /// <returns>若 <paramref name="method"/> 是程序集外部可重写的方法，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="method"/> 为 <see langword="null"/>。</exception>
    public static bool IsOverridable(this MethodInfo method)
    {
        if (method is null)
        {
            throw new ArgumentNullException(nameof(method));
        }

        return method.IsInheritable() && (method.IsVirtual && !method.IsFinal);
    }

    /// <summary>
    /// 确定当前 <see cref="MethodInfo"/> 与另一方法的方法签名是否相等。
    /// </summary>
    /// <param name="value">要进行比较的 <see cref="MethodInfo"/>。</param>
    /// <param name="other">要与当前方法进行比较的 <see cref="MethodInfo"/>。</param>
    /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的方法签名相等，
    /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool SignatureEquals(MethodInfo? value, MethodInfo? other)
    {
        if (object.ReferenceEquals(value, other)) { return true; }
        if ((value is null) || (other is null)) { return false; }

        if (value.Name != other.Name) { return false; }
        if (value.CallingConvention != other.CallingConvention) { return false; }

        if (value.IsGenericMethod ^ other.IsGenericMethod) { return false; }
        if (value.IsGenericMethod) { value = value.GetGenericMethodDefinition(); }
        if (other.IsGenericMethod) { other = other.GetGenericMethodDefinition(); }
        var xGTypes = value.IsGenericMethod ? value.GetGenericArguments() : Array.Empty<Type>();
        var yGTypes = other.IsGenericMethod ? other.GetGenericArguments() : Array.Empty<Type>();
        if (xGTypes.Length != yGTypes.Length) { return false; }

        static bool TypeEquals(Type xType, Type yType)
        {
            if (xType.IsGenericParameter ^ yType.IsGenericParameter) { return false; }
            if (xType.IsGenericParameter && yType.IsGenericParameter)
            {
                return xType.GenericParameterPosition == yType.GenericParameterPosition;
            }

            if (xType.IsGenericType ^ yType.IsGenericType) { return false; }
            if (!xType.IsGenericType && !yType.IsGenericType) { return xType == yType; }

            var xTypeDType = xType.GetGenericTypeDefinition();
            var yTypeDType = yType.GetGenericTypeDefinition();
            if (xTypeDType != yTypeDType) { return false; }

            var xTypeGTypes = xType.GetGenericArguments();
            var yTypeGTypes = yType.GetGenericArguments();
            if (xTypeGTypes.Length != yTypeGTypes.Length) { return false; }
            foreach (var index in ..xTypeGTypes.Length)
            {
                if (!TypeEquals(xTypeGTypes[index], yTypeGTypes[index])) { return false; }
            }

            return true;
        }

        var xRType = value.ReturnParameter.ParameterType;
        var yRType = other.ReturnParameter.ParameterType;
        if (!TypeEquals(xRType, yRType)) { return false; }

        var xPTypes = Array.ConvertAll(value.GetParameters(), param => param.ParameterType);
        var yPTypes = Array.ConvertAll(other.GetParameters(), param => param.ParameterType);
        if (xPTypes.Length != yPTypes.Length) { return false; }
        foreach (var index in ..xPTypes.Length)
        {
            if (!TypeEquals(xPTypes[index], yPTypes[index])) { return false; }
        }

        return true;
    }
}
