using System;
using System.Collections.Generic;
using System.Reflection;
using XstarS.Collections.Specialized;

namespace XstarS.Runtime
{
    partial class ObjectRuntimeHelper
    {
        /// <summary>
        /// 确定指定的两个对象的值是否相等。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        public static bool ValueEquals(object value, object other)
        {
            var comparer = PairReferenceEqualityComparer.Default;
            var compared = new HashSet<KeyValuePair<object, object>>(comparer);
            return ObjectRuntimeHelper.ValueEquals(value, other, compared);
        }

        /// <summary>
        /// 确定指定的两个以 <see cref="Pointer"/> 包装的指针是否相等。
        /// </summary>
        /// <param name="value">要进行相等比较的第一个包装的指针。</param>
        /// <param name="other">要进行相等比较的第二个包装的指针。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/> 包装的指针相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        internal static unsafe bool BoxedPointerEquals(object value, object other)
        {
            return Pointer.Unbox(value) == Pointer.Unbox(other);
        }

        /// <summary>
        /// 确定指定的两个对象的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <param name="compared">当前已经比较过的对象的键值对。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 的值相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static bool ValueEquals(object value, object other,
            HashSet<KeyValuePair<object, object>> compared)
        {
            var pair = new KeyValuePair<object, object>(value, other);
            if (!compared.Add(pair)) { return true; }

            if (object.ReferenceEquals(value, other)) { return true; }
            if ((value is null) ^ (other is null)) { return false; }

            if (value.GetType() != other.GetType()) { return false; }

            var type = value.GetType();
            if (type.IsPrimitive)
            {
                return ObjectRuntimeHelper.PrimitiveEquals(value, other);
            }
            else if (type.IsArray)
            {
                return ObjectRuntimeHelper.ArrayValueEquals(
                    (Array)value, (Array)other, compared);
            }
            else
            {
                return ObjectRuntimeHelper.ObjectValueEquals(value, other, compared);
            }
        }

        /// <summary>
        /// 确定指定的两个基元类型对象 (<see cref="Type.IsPrimitive"/>) 是否相等。
        /// </summary>
        /// <param name="value">要进行相等比较的第一个基元类型对象。</param>
        /// <param name="other">要进行相等比较的第二个基元类型对象。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/> 的类型相同且相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private static bool PrimitiveEquals(object value, object other)
        {
            return object.Equals(value, other);
        }

        /// <summary>
        /// 确定指定的两个数组 <see cref="Array"/> 的所有元素的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个数组。</param>
        /// <param name="other">要进行值相等比较的第二个数组。</param>
        /// <param name="compared">当前已经比较过的对象的键值对。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/>
        /// 均为数组 <see cref="Array"/>，且类型和尺寸相同，每个元素的值相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private static bool ArrayValueEquals(Array value, Array other,
            HashSet<KeyValuePair<object, object>> compared)
        {
            if (value.Rank != other.Rank) { return false; }
            if (value.Length != other.Length) { return false; }
            for (int rank = 0; rank < value.Rank; rank++)
            {
                if (value.GetLength(rank) != other.GetLength(rank))
                {
                    return false;
                }
            }

            var typeArray = value.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < value.Length; index++)
                {
                    var valueItem = methodGet.Invoke(value, Array.ConvertAll(
                        value.OffsetToIndices(index), ObjectRuntimeHelper.BoxIndex));
                    var otherItem = methodGet.Invoke(other, Array.ConvertAll(
                        other.OffsetToIndices(index), ObjectRuntimeHelper.BoxIndex));
                    if (!ObjectRuntimeHelper.BoxedPointerEquals(valueItem, otherItem))
                    {
                        return false;
                    }
                }
            }
            else
            {
                bool isMultiDim = value.Rank > 1;
                for (int index = 0; index < value.Length; index++)
                {
                    var valueItem = isMultiDim ?
                        value.GetValue(value.OffsetToIndices(index)) : value.GetValue(index);
                    var otherItem = isMultiDim ?
                        other.GetValue(other.OffsetToIndices(index)) : other.GetValue(index);
                    if (!ObjectRuntimeHelper.ValueEquals(valueItem, otherItem, compared))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 确定指定的两个对象的所有字段的值是否相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <param name="compared">当前已经比较过的对象的键值对。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/>
        /// 的类型相同，且每个实例字段的值相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static bool ObjectValueEquals(object value, object other,
            HashSet<KeyValuePair<object, object>> compared)
        {
            for (var type = value.GetType(); !(type is null); type = type.BaseType)
            {
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    var valueMember = field.GetValue(value);
                    var otherMember = field.GetValue(other);
                    if (field.FieldType.IsPointer)
                    {
                        if (!ObjectRuntimeHelper.BoxedPointerEquals(valueMember, otherMember))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!ObjectRuntimeHelper.ValueEquals(valueMember, otherMember, compared))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
