using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using XstarS.Collections.Specialized;

namespace XstarS
{
    /// <summary>
    /// 提供获取指定对象基于值的哈希代码的方法。
    /// </summary>
    [Serializable]
    internal sealed class ValueHashableObject
    {
        /// <summary>
        /// 要获取基于值的哈希代码的对象。
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// 基于值的哈希代码的引用包装。
        /// </summary>
        [NonSerialized]
        private StrongBox<int> HashCodeBox;

        /// <summary>
        /// 已经计算过哈希代码的对象。
        /// </summary>
        [NonSerialized]
        private HashSet<object> Computed;

        /// <summary>
        /// 使用要获取基于值的哈希代码的对象初始化 <see cref="ValueHashableObject"/> 类的新实例。
        /// </summary>
        /// <param name="value">要获取基于值的哈希代码的对象。</param>
        public ValueHashableObject(object value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 获取当前实例包含的对象基于值的哈希代码。
        /// 将递归计算至对象的字段（数组的元素）为 .NET 基元类型 (<see cref="Type.IsPrimitive"/>)、
        /// 字符串 <see cref="string"/> 或指针类型 (<see cref="Type.IsPointer"/>)。
        /// </summary>
        /// <returns><see cref="ValueHashableObject.Value"/> 基于值的哈希代码。</returns>
        public int GetValueHashCode()
        {
            this.HashCodeBox = new StrongBox<int>();

            this.AppendValueHashCode();
            int hashCode = this.HashCodeBox.Value;

            this.HashCodeBox = null;
            return hashCode;
        }

        /// <summary>
        /// 将指定哈希代码附加到当前哈希代码中。
        /// </summary>
        /// <param name="hashCode">要附加到当前哈希代码的哈希代码。</param>
        private void AppendHashCode(int hashCode) =>
            this.HashCodeBox.Value = this.HashCodeBox.Value * -1521134295 + hashCode;

        /// <summary>
        /// 将当前实例包含的对象基于值的哈希代码附加到当前的哈希代码中。
        /// </summary>
        private void AppendValueHashCode()
        {
            this.Computed = this.Computed ??
                new HashSet<object>(
                    ReferenceEqualityComparer<object>.Default);

            this.AppendTypedValueHashCode();

            this.Computed = null;
        }

        /// <summary>
        /// 将指定对象基于值的哈希代码附加到当前的哈希代码中。
        /// </summary>
        /// <param name="value">要将其基于值的哈希代码追加到当前哈希代码的对象。</param>
        private void AppendValueHashCode(object value) =>
            new ValueHashableObject(value) { HashCodeBox = this.HashCodeBox,
                Computed = this.Computed }.AppendValueHashCode();

        /// <summary>
        /// 根据类型将当前实例包含的对象基于值的哈希代码附加到当前的哈希代码中。
        /// </summary>
        private void AppendTypedValueHashCode()
        {
            var value = this.Value;

            // 空引用。
            if (value is null) { this.AppendEmptyValueHashCode(); return; }
            // 已计算过的对象。
            if (!this.Computed.Add(value)) { this.AppendEmptyValueHashCode(); return; }

            // 根据类型附加哈希代码。
            var type = value.GetType();
            if (type.IsPrimitive) { this.AppendPrimitiveValueHashCode(); }
            else if (type == typeof(string)) { this.AppendStringValueHashCode(); }
            else if (type == typeof(Pointer)) { this.AppendPointerValueHashCode(); }
            else if (type.IsArray) { this.AppendArrayValueHashCode(); }
            else { this.AppendObjectValueHashCode(); }
        }

        /// <summary>
        /// 将一个空哈希代码附加到当前的哈希代码中。
        /// </summary>
        private void AppendEmptyValueHashCode() =>
            this.AppendHashCode(0);

        /// <summary>
        /// 将当前实例包含的基元类型对象 (<see cref="Type.IsPrimitive"/>) 基于值的哈希代码附加到当前的哈希代码中。
        /// </summary>
        private void AppendPrimitiveValueHashCode() =>
            this.AppendHashCode(this.Value.GetHashCode());

        /// <summary>
        /// 将当前实例包含的字符串 <see cref="string"/> 基于值的哈希代码附加到当前的哈希代码中。
        /// </summary>
        private void AppendStringValueHashCode() =>
            this.AppendHashCode(((string)this.Value).GetHashCode());

        /// <summary>
        /// 将当前实例包含的指针包装 <see cref="Pointer"/> 基于值的哈希代码附加到当前的哈希代码中。
        /// </summary>
        private unsafe void AppendPointerValueHashCode() =>
            this.AppendHashCode(((IntPtr)Pointer.Unbox((Pointer)this.Value)).GetHashCode());

        /// <summary>
        /// 将当前实例包含的数组 <see cref="Array"/> 基于所有元素的值的哈希代码附加到当前的哈希代码中。
        /// 将递归计算至元素为 .NET 基元类型、字符串或指针类型。
        /// </summary>
        private void AppendArrayValueHashCode()
        {
            var value = (Array)this.Value;

            this.AppendEmptyValueHashCode();

            var typeArray = value.GetType();
            // 指针数组，反射调用无法访问的 Get 方法。
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int i = 0; i < value.Length; i++)
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
                for (int i = 0; i < value.Length; i++)
                {
                    this.AppendValueHashCode(isMultiDim ?
                        value.GetValue(value.OffsetToIndices(i)) : value.GetValue(i));
                }
            }
        }

        /// <summary>
        /// 将当前实例包含的对象基于所有字段的值的哈希代码附加到当前的哈希代码中。
        /// 将递归计算至字段为 .NET 基元类型、字符串或指针类型。
        /// </summary>
        private void AppendObjectValueHashCode()
        {
            var value = this.Value;

            this.AppendEmptyValueHashCode();

            // 循环获取基类。
            for (var type = value.GetType(); !(type is null); type = type.BaseType)
            {
                // 获取每个实例字段。
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                // 依次递归获取每个字段的哈希代码。
                foreach (var field in fields)
                {
                    this.AppendValueHashCode(field.GetValue(value));
                }
            }
        }
    }
}
