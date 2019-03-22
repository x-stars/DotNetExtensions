using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XstarS
{
    /// <summary>
    /// 提供两个对象的值相等比较的方法。
    /// </summary>
    [Serializable]
    internal sealed class ValueEquatablePair : IEquatable<ValueEquatablePair>
    {
        /// <summary>
        /// <see cref="Pointer"/> 的 <code>GetPointerType</code> 方法的静态委托调用。
        /// </summary>
        private static readonly Func<Pointer, Type> StaticGetPointerType =
            typeof(Pointer).GetMethod("GetPointerType",
                BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(
                    typeof(Func<Pointer, Type>)) as Func<Pointer, Type>;

        /// <summary>
        /// 要进行值相等比较的第一个对象。
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// 要进行值相等比较的第二个对象。
        /// </summary>
        public readonly object Other;

        /// <summary>
        /// 已经比较过的 <see cref="ValueEquatablePair"/> 对象。
        /// </summary>
        [NonSerialized]
        private ISet<ValueEquatablePair> Compared;

        /// <summary>
        /// 使用要进行值相等比较的两个对象初始化 <see cref="ValueEquatablePair"/> 类的新实例。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        public ValueEquatablePair(object value, object other)
        {
            this.Value = value;
            this.Other = other;
        }

        /// <summary>
        /// 使用要进行值相等比较的两个对象和已经比较过的对象初始化 <see cref="ValueEquatablePair"/> 类的新实例。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <param name="compared">已经比较过的 <see cref="ValueEquatablePair"/> 对象。</param>
        private ValueEquatablePair(object value, object other,
            ISet<ValueEquatablePair> compared) : this(value, other)
        {
            this.Compared = compared;
        }

        /// <summary>
        /// 确定当前实例包含的两个对象的所有字段的值（对数组则是所有元素的值）是否相等。
        /// 将递归比较至字段（元素）为 .NET 基元类型（<see cref="Type.IsPrimitive"/>）或指针类型。
        /// </summary>
        public bool ValueEquals
        {
            get
            {
                var value = this.Value;
                var other = this.Other;
                this.Compared = this.Compared ?? new HashSet<ValueEquatablePair>();

                // 引用比较。
                if (object.ReferenceEquals(value, other)) { return true; }
                if ((value is null) ^ (other is null)) { return false; }
                // 类型不同。
                if (value.GetType() != other.GetType()) { return false; }

                // 根据类型取得对应的结果。
                var type = value.GetType();
                bool result = !this.Compared.Add(this) ? true :
                    type.IsPrimitive ? this.PrimitiveValueEquals :
                    type == typeof(string) ? this.StringValueEquals :
                    type == typeof(Pointer) ? this.PointerValueEquals :
                    type.IsArray ? this.ArrayValueEquals :
                    this.ObjectValueEquals;

                this.Compared = null;
                return result;
            }
        }

        /// <summary>
        /// 确定当前实例包含的两个基元类型对象 (<see cref="Type.IsPrimitive"/>) 的值是否相等。
        /// </summary>
        private bool PrimitiveValueEquals =>
            object.Equals(this.Value, this.Other);

        /// <summary>
        /// 确定当前实例包含的两个字符串 <see cref="string"/> 的值是否相等。
        /// </summary>
        private bool StringValueEquals =>
            string.Equals((string)this.Value, (string)this.Other);

        /// <summary>
        /// 确定当前实例包含的两个指针包装 <see cref="Pointer"/> 的值是否相等。将比较指针的值和指针的类型。
        /// </summary>
        private unsafe bool PointerValueEquals
        {
            get
            {
                var value = (Pointer)this.Value;
                var other = (Pointer)this.Other;
                var compared = this.Compared;

                return (Pointer.Unbox(value) == Pointer.Unbox(other)) ?
                    new ValueEquatablePair(
                        ValueEquatablePair.StaticGetPointerType(value),
                        ValueEquatablePair.StaticGetPointerType(other),
                        compared).ValueEquals : false;
            }
        }

        /// <summary>
        /// 确定当前实例包含的两个数组 <see cref="Array"/> 的所有元素的值是否相等。
        /// 对每个元素将递归比较至字段（元素）为 .NET 基元类型或指针类型。
        /// </summary>
        private bool ArrayValueEquals
        {
            get
            {
                var value = (Array)this.Value;
                var other = (Array)this.Other;
                var compared = this.Compared;

                // 大小不等。
                if (value.Rank != other.Rank) { return false; }
                if (value.LongLength != other.LongLength) { return false; }
                for (int i = 0; i < value.Rank; i++)
                {
                    if (value.GetLength(i) != other.GetLength(i))
                    {
                        return false;
                    }
                }

                var typeArray = value.GetType();
                // 指针数组，反射调用无法访问的 Get 方法。
                if (typeArray.GetElementType().IsPointer)
                {
                    var methodGet = typeArray.GetMethod("Get");
                    for (long i = 0; i < value.LongLength; i++)
                    {
                        if (!new ValueEquatablePair(
                            methodGet.Invoke(value, Array.ConvertAll(
                                value.OffsetToIndices(i), index => (object)index)),
                            methodGet.Invoke(other, Array.ConvertAll(
                                other.OffsetToIndices(i), index => (object)index)),
                            compared).ValueEquals)
                        {
                            return false;
                        }
                    }
                }
                // 一般数组。
                else
                {
                    bool isMultiDim = value.Rank > 1;
                    for (long i = 0; i < value.LongLength; i++)
                    {
                        if (!new ValueEquatablePair(
                            isMultiDim ? value.GetValue(value.OffsetToIndices(i)) : value.GetValue(i),
                            isMultiDim ? other.GetValue(other.OffsetToIndices(i)) : other.GetValue(i),
                            compared).ValueEquals)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 确定当前实例包含的两个对象的所有字段的值是否相等。
        /// 对每个字段将递归比较至字段（元素）为 .NET 基元类型或指针类型。
        /// </summary>
        private bool ObjectValueEquals
        {
            get
            {
                var value = this.Value;
                var other = this.Other;
                var compared = this.Compared;

                // 类型不同。
                if (value.GetType() != other.GetType()) { return false; }

                var type = value.GetType();
                // 获取每个实例字段。
                var fields = type.GetFields(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                // 依次递归比较每个字段。
                foreach (var field in fields)
                {
                    if (!new ValueEquatablePair(field.GetValue(value),
                        field.GetValue(other), compared).ValueEquals)
                    {
                        return false;
                    }
                }

                // 递归进行基类的比较。
                for (var baseType = type.BaseType;
                    !(baseType is null);
                    baseType = baseType.BaseType)
                {
                    // 获取每个实例字段。
                    var baseFields = baseType.GetFields(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    // 依次递归比较每个字段。
                    foreach (var baseField in baseFields)
                    {
                        if (!new ValueEquatablePair(baseField.GetValue(value),
                            baseField.GetValue(other), compared).ValueEquals)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
        }

        /// <summary>
        /// 确定当前实例与指定的 <see cref="ValueEquatablePair"/> 实例中包含的两个对象的引用是否相等。
        /// </summary>
        /// <param name="other">要与当前实例进行比较的 <see cref="ValueEquatablePair"/> 类的实例。</param>
        /// <returns>若 <paramref name="other"/> 不为 <see langword="null"/>，
        /// 且 <see cref="ValueEquatablePair.Value"/> 和 <see cref="ValueEquatablePair.Other"/> 的引用都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public bool Equals(ValueEquatablePair other) =>
            !(other is null) &&
            object.ReferenceEquals(this.Value, other.Value) &&
            object.ReferenceEquals(this.Other, other.Other);

        /// <summary>
        /// 确定当前实例与指定的对象是否相等。
        /// </summary>
        /// <param name="obj">要与当前实例进行比较的对象。</param>
        /// <returns>若 <paramref name="obj"/> 是一个 <see cref="ValueEquatablePair"/> 类的实例，
        /// 且 <see cref="ValueEquatablePair.Value"/> 和 <see cref="ValueEquatablePair.Other"/> 的引用都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public override bool Equals(object obj) =>
            (obj is ValueEquatablePair other) && this.Equals(other);

        /// <summary>
        /// 获取以当前实例包含的两个对象的引用为基础的哈希函数。
        /// </summary>
        /// <returns>以当前实例的 <see cref="ValueEquatablePair.Value"/> 和
        /// <see cref="ValueEquatablePair.Other"/> 的引用为基础的哈希函数。</returns>
        public override int GetHashCode() =>
            RuntimeHelpers.GetHashCode(this.Value) ^
            RuntimeHelpers.GetHashCode(this.Other);
    }
}
