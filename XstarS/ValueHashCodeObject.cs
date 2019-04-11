using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using XstarS.Collections.Specialized;

namespace XstarS
{
    /// <summary>
    /// 提供获取指定对象基于值的哈希函数的方法。
    /// </summary>
    [Serializable]
    internal sealed class ValueHashCodeObject
    {
        /// <summary>
        /// 要获取基于值的哈希函数的对象。
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// 基于值的哈希函数的引用包装。
        /// </summary>
        [NonSerialized]
        private StrongBox<int> HashCode;

        /// <summary>
        /// 已经计算过哈希函数的对象。
        /// </summary>
        [NonSerialized]
        private HashSet<object> Computed;

        /// <summary>
        /// 使用要获取基于值的哈希函数的对象初始化 <see cref="ValueHashCodeObject"/> 类的新实例。
        /// </summary>
        /// <param name="value">要获取基于值的哈希函数的对象。</param>
        public ValueHashCodeObject(object value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 获取当前实例包含的对象基于所有字段的值（对数组则是所有元素的值）的哈希函数，
        /// 将递归计算至字段（元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
        /// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。
        /// </summary>
        /// <returns>递归获取 <see cref="ValueHashCodeObject.Value"/> 的所有字段（对数组则是所有元素），
        /// 直至其为 .NET 基元类型、字符串或指针类型得到的基于值的 32 位有符号整数哈希函数。</returns>
        public int GetValueHashCode()
        {
            this.HashCode = new StrongBox<int>();

            this.AppendValueHashCode();
            int hashCode = this.HashCode.Value;

            this.HashCode = null;
            return hashCode;
        }

        /// <summary>
        /// 将指定哈希函数附加到当前哈希函数中。
        /// </summary>
        /// <param name="hashCode">要附加到当前哈希函数的哈希函数。</param>
        private void AppendHashCode(int hashCode) =>
            this.HashCode.Value = this.HashCode.Value * -1521134295 + hashCode;

        /// <summary>
        /// 将当前实例包含的对象基于所有字段的值（对数组则是所有元素的值）的哈希函数附加到当前的哈希函数中。
        /// 将递归计算至字段（元素）为 .NET 基元类型、字符串或指针类型。
        /// </summary>
        private void AppendValueHashCode()
        {
            var value = this.Value;
            this.Computed = this.Computed ??
                new HashSet<object>(ReferenceEqualityComparer.Default);

            // 空引用。
            if (value is null) { return; }

            // 根据类型附加哈希函数。
            var type = value.GetType();
            if (!this.Computed.Add(value)) { }
            else if (type.IsPrimitive) { this.AppendPrimitiveValueHashCode(); }
            else if (type == typeof(string)) { this.AppendStringValueHashCode(); }
            else if (type == typeof(Pointer)) { this.AppendPointerValueHashCode(); }
            else if (type.IsArray) { this.AppendArrayValueHashCode(); }
            else { this.AppendObjectValueHashCode(); }

            this.Computed = null;
        }

        /// <summary>
        /// 将指定对象基于所有字段的值（对数组则是所有元素的值）的哈希函数附加到当前的哈希函数中。
        /// 将递归计算至字段（元素）为 .NET 基元类型、字符串或指针类型。
        /// </summary>
        /// <param name="value">要将其基于值的哈希函数追加到当前哈希函数的对象。</param>
        private void AppendValueHashCode(object value) =>
            new ValueHashCodeObject(value) { HashCode = this.HashCode,
                Computed = this.Computed }.AppendValueHashCode();

        /// <summary>
        /// 将当前实例包含的基元类型对象 (<see cref="Type.IsPrimitive"/>) 基于值的哈希函数附加到当前的哈希函数中。
        /// </summary>
        private void AppendPrimitiveValueHashCode() =>
            this.AppendHashCode((this.Value).GetHashCode());

        /// <summary>
        /// 将当前实例包含的字符串 <see cref="string"/> 基于值的哈希函数附加到当前的哈希函数中。
        /// </summary>
        private void AppendStringValueHashCode() =>
            this.AppendHashCode(((string)this.Value).GetHashCode());

        /// <summary>
        /// 将当前实例包含的指针包装 <see cref="Pointer"/> 基于值的哈希函数附加到当前的哈希函数中。
        /// </summary>
        private unsafe void AppendPointerValueHashCode() =>
            this.AppendHashCode(((IntPtr)Pointer.Unbox((Pointer)this.Value)).GetHashCode());

        /// <summary>
        /// 将当前实例包含的数组 <see cref="Array"/> 基于所有元素的值的哈希函数附加到当前的哈希函数中。
        /// 将递归计算至元素为 .NET 基元类型、字符串或指针类型。
        /// </summary>
        private void AppendArrayValueHashCode()
        {
            var value = (Array)this.Value;

            var typeArray = value.GetType();
            // 指针数组，反射调用无法访问的 Get 方法。
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (long i = 0; i < value.LongLength; i++)
                {
                    this.AppendValueHashCode(
                        methodGet.Invoke(value, Array.ConvertAll(
                            value.OffsetToIndices(i), index => (object)index)));
                }
            }
            // 一般数组。
            else
            {
                bool isMultiDim = value.Rank > 1;
                for (long i = 0; i < value.LongLength; i++)
                {
                    this.AppendValueHashCode(isMultiDim ?
                        value.GetValue(value.OffsetToIndices(i)) : value.GetValue(i));
                }
            }
        }

        /// <summary>
        /// 将当前实例包含的对象基于所有字段的值的哈希函数附加到当前的哈希函数中。
        /// 将递归计算至字段为 .NET 基元类型、字符串或指针类型。
        /// </summary>
        private void AppendObjectValueHashCode()
        {
            var value = this.Value;

            // 循环获取基类。
            for (var type = value.GetType(); !(type is null); type = type.BaseType)
            {
                // 获取每个实例字段。
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                // 依次递归获取每个字段的哈希函数。
                foreach (var field in fields)
                {
                    this.AppendValueHashCode(field.GetValue(value));
                }
            }
        }
    }
}
