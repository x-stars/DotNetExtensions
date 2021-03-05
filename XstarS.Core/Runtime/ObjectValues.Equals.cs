using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using XstarS.Collections.Specialized;

namespace XstarS.Runtime
{
    using ObjectPair = KeyValuePair<object, object>;

    partial class ObjectValues
    {
        /// <summary>
        /// 确定指定的两个对象包含的值是否递归相等。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 包含的值递归相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        public static bool RecursiveEquals(object value, object other)
        {
            var comparer = PairReferenceEqualityComparer.Default;
            var compared = new HashSet<ObjectPair>(comparer);
            return ObjectValues.RecursiveEquals(value, other, compared);
        }

        /// <summary>
        /// 确定指定的两个对象包含的值是否递归相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <param name="compared">当前已经比较过的对象的键值对。</param>
        /// <returns>若 <paramref name="value"/> 与 <paramref name="other"/> 包含的值递归相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static bool RecursiveEquals(
            object value, object other, HashSet<ObjectPair> compared)
        {
            if (RuntimeHelpers.Equals(value, other)) { return true; }
            if ((value is null) ^ (other is null)) { return false; }
            if (value.GetType() != other.GetType()) { return false; }

            var pair = new ObjectPair(value, other);
            if (!compared.Add(pair)) { return true; }

            var type = value.GetType();
            if (type.IsPrimitive)
            {
                return ObjectValues.PrimitiveEquals(value, other);
            }
            if (type == typeof(string))
            {
                return ObjectValues.PrimitiveEquals(value, other);
            }
            else if (type.IsArray)
            {
                return ObjectValues.ArrayRecursiveEquals((Array)value, (Array)other, compared);
            }
            else
            {
                return ObjectValues.ObjectRecursiveEquals(value, other, compared);
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
        /// 确定指定的两个以 <see cref="Pointer"/> 包装的指针是否相等。
        /// </summary>
        /// <param name="value">要进行相等比较的第一个包装的指针。</param>
        /// <param name="other">要进行相等比较的第二个包装的指针。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/> 包装的指针相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private static unsafe bool BoxedPointerEquals(object value, object other)
        {
            return Pointer.Unbox(value) == Pointer.Unbox(other);
        }

        /// <summary>
        /// 确定指定的两个数组 <see cref="Array"/> 的所有元素的值是否递归相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个数组。</param>
        /// <param name="other">要进行值相等比较的第二个数组。</param>
        /// <param name="compared">当前已经比较过的对象的键值对。</param>
        /// <returns>若 <paramref name="value"/> 和 <paramref name="other"/>
        /// 均为数组 <see cref="Array"/>，且类型和尺寸相同，每个元素的值递归相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        private static bool ArrayRecursiveEquals(
            Array value, Array other, HashSet<ObjectPair> compared)
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
                    var valueItem = methodGet.Invoke(value, value.OffsetToIndices(index).Box());
                    var otherItem = methodGet.Invoke(other, other.OffsetToIndices(index).Box());
                    if (!ObjectValues.BoxedPointerEquals(valueItem, otherItem))
                    {
                        return false;
                    }
                }
            }
            else
            {
                bool isSZArray = value.IsSZArray();
                for (int index = 0; index < value.Length; index++)
                {
                    var valueItem = isSZArray ?
                        value.GetValue(index) : value.GetValue(value.OffsetToIndices(index));
                    var otherItem = isSZArray ?
                        other.GetValue(index) : other.GetValue(other.OffsetToIndices(index));
                    if (!ObjectValues.RecursiveEquals(valueItem, otherItem, compared))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 确定指定的两个对象的所有字段的值是否递归相等。
        /// </summary>
        /// <param name="value">要进行值相等比较的第一个对象。</param>
        /// <param name="other">要进行值相等比较的第二个对象。</param>
        /// <param name="compared">当前已经比较过的对象的键值对。</param>
        /// <returns>若 <paramref name="value"/> 和
        /// <paramref name="other"/> 的类型相同，且每个实例字段的值递归相等，
        /// 则为 <see langword="true"/>，否则为 <see langword="false"/>。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static bool ObjectRecursiveEquals(
            object value, object other, HashSet<ObjectPair> compared)
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
                        if (!ObjectValues.BoxedPointerEquals(valueMember, otherMember))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (!ObjectValues.RecursiveEquals(valueMember, otherMember, compared))
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
