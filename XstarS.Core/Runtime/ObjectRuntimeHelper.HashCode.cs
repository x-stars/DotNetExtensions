using System;
using System.Collections.Generic;
using System.Reflection;
using XstarS.Collections.Specialized;

namespace XstarS.Runtime
{
    partial class ObjectRuntimeHelper
    {
        /// <summary>
        /// 获取指定对象基于值的哈希代码。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <param name="value">要获取基于值的哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 基于值的哈希代码。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        public static int GetValueHashCode(object value)
        {
            var comparer = ReferenceEqualityComparer.Default;
            var computed = new HashSet<object>(comparer);
            return ObjectRuntimeHelper.GetValueHashCode(value, computed);
        }

        /// <summary>
        /// 获取指定以 <see cref="Pointer"/> 包装的指针的哈希代码。
        /// </summary>
        /// <param name="value">要获取哈希代码的包装的指针。</param>
        /// <returns><paramref name="value"/> 中包装的指针的哈希代码。</returns>
        internal static unsafe int GetBoxedPointerHashCode(object value)
        {
            return ((IntPtr)Pointer.Unbox(value)).GetHashCode();
        }

        /// <summary>
        /// 组合当前哈希代码和新的哈希代码。
        /// </summary>
        /// <param name="hashCode">当前哈希代码。</param>
        /// <param name="nextHashCode">新的哈希代码。</param>
        /// <returns><paramref name="hashCode"/> 与
        /// <paramref name="nextHashCode"/> 组合得到的哈希代码。</returns>
        private static int CombineHashCode(int hashCode, int nextHashCode)
        {
            return hashCode * -1521134295 + nextHashCode;
        }

        /// <summary>
        /// 获取指定对象基于值的哈希代码。
        /// </summary>
        /// <param name="value">要获取基于值的哈希代码的对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 基于值的哈希代码。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static int GetValueHashCode(object value, HashSet<object> computed)
        {
            if (value is null) { return 0; }

            if (!computed.Add(value)) { return 0; }

            var type = value.GetType();
            if (type.IsPrimitive)
            {
                return ObjectRuntimeHelper.GetPrimitiveHashCode(value);
            }
            else if (type.IsArray)
            {
                return ObjectRuntimeHelper.GetArrayValueHashCode((Array)value, computed);
            }
            else
            {
                return ObjectRuntimeHelper.GetObjectValueHashCode(value, computed);
            }
        }

        /// <summary>
        /// 获取指定基元类型对象 (<see cref="Type.IsPrimitive"/>) 的哈希代码。
        /// </summary>
        /// <param name="value">要获取哈希代码的基元类型对象。</param>
        /// <returns><paramref name="value"/> 的哈希代码。</returns>
        private static int GetPrimitiveHashCode(object value)
        {
            return value.GetHashCode();
        }

        /// <summary>
        /// 获取指定数组中所有元素的基于值的哈希代码。
        /// </summary>
        /// <param name="value">要获取基于值的哈希代码的数组。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 中所有元素的基于值的哈希代码。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static int GetArrayValueHashCode(Array value, HashSet<object> computed)
        {
            var hashCode = value.GetType().GetHashCode();

            var typeArray = value.GetType();
            if (typeArray.GetElementType().IsPointer)
            {
                var methodGet = typeArray.GetMethod("Get");
                for (int index = 0; index < value.Length; index++)
                {
                    var item = methodGet.Invoke(value, value.OffsetToIndices(index).Box());
                    hashCode = ObjectRuntimeHelper.CombineHashCode(
                        hashCode, ObjectRuntimeHelper.GetBoxedPointerHashCode(item));
                }
            }
            else
            {
                bool isMultiDim = value.Rank > 1;
                for (int index = 0; index < value.Length; index++)
                {
                    var item = isMultiDim ?
                        value.GetValue(value.OffsetToIndices(index)) : value.GetValue(index);
                    hashCode = ObjectRuntimeHelper.CombineHashCode(
                        hashCode, ObjectRuntimeHelper.GetValueHashCode(item, computed));
                }
            }

            return hashCode;
        }

        /// <summary>
        /// 获取指定对象中所有字段的基于值的哈希代码。
        /// </summary>
        /// <param name="value">要获取基于值的哈希代码的对象。</param>
        /// <param name="computed">已经计算过哈希代码的对象。</param>
        /// <returns><paramref name="value"/> 中所有字段的基于值的哈希代码。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static int GetObjectValueHashCode(object value, HashSet<object> computed)
        {
            var hashCode = value.GetType().GetHashCode();

            for (var type = value.GetType(); !(type is null); type = type.BaseType)
            {
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    var member = field.GetValue(value);
                    if (field.FieldType.IsPointer)
                    {
                        hashCode = ObjectRuntimeHelper.CombineHashCode(
                            hashCode, ObjectRuntimeHelper.GetBoxedPointerHashCode(member));
                    }
                    else
                    {
                        hashCode = ObjectRuntimeHelper.CombineHashCode(
                            hashCode, ObjectRuntimeHelper.GetValueHashCode(member, computed));
                    }
                }
            }

            return hashCode;
        }
    }
}
