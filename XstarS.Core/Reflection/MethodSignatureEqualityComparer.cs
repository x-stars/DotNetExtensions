using System;
using System.Reflection;
using XstarS.Collections.Generic;

namespace XstarS.Reflection
{
    /// <summary>
    /// 提供用于比较方法的签名是否相等的方法。
    /// </summary>
    [Serializable]
    public sealed class MethodSignatureEqualityComparer : SimpleEqualityComparer<MethodInfo>
    {
        /// <summary>
        /// 初始化 <see cref="MethodSignatureEqualityComparer"/> 类的新实例。
        /// </summary>
        public MethodSignatureEqualityComparer() { }

        /// <summary>
        /// 获取 <see cref="MethodSignatureEqualityComparer"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="MethodSignatureEqualityComparer"/> 类的默认实例。</returns>
        public static new MethodSignatureEqualityComparer Default { get; } =
            new MethodSignatureEqualityComparer();

        /// <summary>
        /// 确定两个 <see cref="MethodInfo"/> 的方法签名是否相等。
        /// </summary>
        /// <param name="x">要比较的第一个 <see cref="MethodInfo"/>。</param>
        /// <param name="y">要比较的第二个 <see cref="MethodInfo"/>。</param>
        /// <returns>若 <paramref name="x"/> 与 <paramref name="y"/> 的方法签名相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(MethodInfo x, MethodInfo y)
        {
            if (object.ReferenceEquals(x, y)) { return true; }
            if ((x is null) ^ (y is null)) { return false; }

            if (x.Name != y.Name) { return false; }
            if (x.CallingConvention != y.CallingConvention) { return false; }

            if (x.IsGenericMethod ^ y.IsGenericMethod) { return false; }
            if (x.IsGenericMethod) { x = x.GetGenericMethodDefinition(); }
            if (y.IsGenericMethod) { y = y.GetGenericMethodDefinition(); }
            var xGTypes = x.IsGenericMethod ? x.GetGenericArguments() : Array.Empty<Type>();
            var yGTypes = y.IsGenericMethod ? y.GetGenericArguments() : Array.Empty<Type>();
            if (xGTypes.Length != yGTypes.Length) { return false; }

            bool TypeEquals(Type xType, Type yType)
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
                for (int index = 0; index < xTypeGTypes.Length; index++)
                {
                    if (!TypeEquals(xTypeGTypes[index], yTypeGTypes[index])) { return false; }
                }

                return true;
            }

            var xRType = x.ReturnParameter.ParameterType;
            var yRType = y.ReturnParameter.ParameterType;
            if (!TypeEquals(xRType, yRType)) { return false; }

            var xPTypes = Array.ConvertAll(x.GetParameters(), param => param.ParameterType);
            var yPTypes = Array.ConvertAll(y.GetParameters(), param => param.ParameterType);
            if (xPTypes.Length != yPTypes.Length) { return false; }
            for (int index = 0; index < xPTypes.Length; index++)
            {
                if (!TypeEquals(xPTypes[index], yPTypes[index])) { return false; }
            }

            return true;
        }

        /// <summary>
        /// 获取指定 <see cref="MethodInfo"/> 基于方法签名的哈希代码。
        /// </summary>
        /// <param name="obj">要获取哈希代码的 <see cref="MethodInfo"/>。</param>
        /// <returns><paramref name="obj"/> 基于方法签名的哈希代码。</returns>
        public override int GetHashCode(MethodInfo obj)
        {
            if (obj is null) { return 0; }

            var hashCode = 0;

            hashCode = this.CombineHasCode(hashCode, obj.Name.GetHashCode());
            hashCode = this.CombineHasCode(hashCode, obj.CallingConvention.GetHashCode());

            if (obj.IsGenericMethod) { obj = obj.GetGenericMethodDefinition(); }
            var gTypes = obj.IsGenericMethod ? obj.GetGenericArguments() : Array.Empty<Type>();
            for (int gIndex = 0; gIndex < gTypes.Length; gIndex++)
            {
                hashCode = this.CombineHasCode(hashCode, gIndex);
            }

            int GetTypeHashCode(Type type)
            {
                var tHashCode = 0;

                if (type.IsGenericParameter)
                {
                    tHashCode = this.CombineHasCode(tHashCode, type.GenericParameterPosition);
                }
                else if (!type.IsGenericType)
                {
                    tHashCode = this.CombineHasCode(tHashCode, type.GetHashCode());
                }
                else
                {
                    var typeDType = type.GetGenericTypeDefinition();
                    tHashCode = this.CombineHasCode(tHashCode, typeDType.GetHashCode());

                    var typeGTypes = type.GetGenericArguments();
                    foreach (var typeGType in typeGTypes)
                    {
                        tHashCode = this.CombineHasCode(tHashCode, GetTypeHashCode(typeGType));
                    }
                }

                return tHashCode;
            }

            var rType = obj.ReturnParameter.ParameterType;
            hashCode = this.CombineHasCode(hashCode, GetTypeHashCode(rType));

            var pTypes = Array.ConvertAll(obj.GetParameters(), param => param.ParameterType);
            foreach (var pType in pTypes)
            {
                hashCode = this.CombineHasCode(hashCode, GetTypeHashCode(pType));
            }

            return hashCode;
        }

        /// <summary>
        /// 组合当前哈希代码和新的哈希代码。
        /// </summary>
        /// <param name="hashCode">当前哈希代码。</param>
        /// <param name="nextHashCode">新的哈希代码。</param>
        /// <returns><paramref name="hashCode"/> 与
        /// <paramref name="nextHashCode"/> 组合得到的哈希代码。</returns>
        private int CombineHasCode(int hashCode, int nextHashCode)
        {
            return hashCode * -1521134295 + nextHashCode;
        }
    }
}
