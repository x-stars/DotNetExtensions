using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using ReferenceEqualityComparer =
    XstarS.Collections.Specialized.ReferenceEqualityComparer;

namespace XstarS.Runtime
{
    partial class ObjectValues
    {
        /// <summary>
        /// 表示 <see cref="object.MemberwiseClone()"/> 方法的静态调用委托。
        /// </summary>
        private static readonly Converter<object, object> CloneDelegate =
            (typeof(object).GetMethod(nameof(ObjectValues.MemberwiseClone),
                BindingFlags.Instance | BindingFlags.NonPublic)!.CreateDelegate(
                    typeof(Converter<object, object>)) as Converter<object, object>)!;

        /// <summary>
        /// 表示用于二进制序列化和反序列化对象的 <see cref="BinaryFormatter"/> 对象。
        /// </summary>
#if NET5_0_OR_GREATER
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used.")]
#endif
        private static readonly BinaryFormatter Serializer = new BinaryFormatter();

        /// <summary>
        /// 获取指定对象的浅表副本。
        /// </summary>
        /// <param name="value">要获取浅表副本的对象。</param>
        /// <returns><paramref name="value"/> 的浅表副本。</returns>
        [return: NotNullIfNotNull("value")]
        public static object? Clone(object? value)
        {
            return (value is null) ? null :
                ObjectValues.CloneDelegate.Invoke(value);
        }

        /// <summary>
        /// 创建指定对象的深度副本。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <param name="value">要获取深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        [return: NotNullIfNotNull("value")]
        public static object? RecursiveClone(object? value)
        {
            if (value is null) { return null; }
            var comparer = ReferenceEqualityComparer.Default;
            var cloned = new Dictionary<object, object>(comparer);
            return ObjectValues.RecursiveClone(value, cloned);
        }

        /// <summary>
        /// 创建指定对象的序列化副本。
        /// </summary>
        /// <remarks>基于对象序列化，可能存在性能问题。</remarks>
        /// <param name="value">要获取序列化副本的对象。</param>
        /// <returns><paramref name="value"/> 的序列化副本。</returns>
        /// <exception cref="SerializationException">
        /// <paramref name="value"/> 中的某个对象未标记为可序列化。</exception>
        /// <exception cref="SecurityException">调用方没有所要求的权限。</exception>
#if NET5_0_OR_GREATER
        [Obsolete("BinaryFormatter serialization is obsolete and should not be used.")]
#endif
        [return: NotNullIfNotNull("value")]
        public static object? SerializationClone(object? value)
        {
            if (value is null) { return null; }
            using var stream = new MemoryStream();
            var serializer = ObjectValues.Serializer;
            serializer.Serialize(stream, value);
            stream.Position = 0L;
            return serializer.Deserialize(stream);
        }

        /// <summary>
        /// 根据类型获取指定对象的深度副本。
        /// </summary>
        /// <param name="value">要获取深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        /// <param name="cloned">已经创建副本的对象及其对应的副本。</param>
        [return: NotNullIfNotNull("value")]
        private static object? RecursiveClone(object? value, Dictionary<object, object> cloned)
        {
            if (value is null) { return null; }
            if (cloned.ContainsKey(value)) { return cloned[value]; }

            var clone = ObjectValues.Clone(value);
            cloned[value] = clone;

            var type = clone.GetType();
            if (type.IsArray)
            {
                ObjectValues.ArrayRecursiveClone((Array)clone, cloned);
            }
            else if (!type.IsPrimitive)
            {
                ObjectValues.ObjectRecursiveClone(clone, cloned);
            }

            return clone;
        }

        /// <summary>
        /// 将指定数组的每个元素替换为其深度副本。
        /// </summary>
        /// <param name="value">要将元素替换为其深度副本的数组。</param>
        /// <param name="cloned">已经创建副本的对象及其对应的副本。</param>
        private static void ArrayRecursiveClone(Array value, Dictionary<object, object> cloned)
        {
            if (!value.GetType().GetElementType()!.IsPointer)
            {
                if (value.IsSZArray())
                {
                    for (int index = 0; index < value.Length; index++)
                    {
                        var item = value.GetValue(index);
                        var clone = ObjectValues.RecursiveClone(item, cloned);
                        value.SetValue(clone, index);
                    }
                }
                else
                {
                    for (int index = 0; index < value.Length; index++)
                    {
                        var indices = value.OffsetToIndices(index);
                        var item = value.GetValue(indices);
                        var clone = ObjectValues.RecursiveClone(item, cloned);
                        value.SetValue(clone, indices);
                    }
                }
            }
        }

        /// <summary>
        /// 将指定对象的每个成员替换为其深度副本。
        /// </summary>
        /// <param name="value">要将成员替换为其深度副本的对象。</param>
        /// <param name="cloned">已经创建副本的对象及其对应的副本。</param>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        private static void ObjectRecursiveClone(object value, Dictionary<object, object> cloned)
        {
            for (var type = value.GetType(); !(type is null); type = type.BaseType)
            {
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    if (!field.FieldType.IsPointer)
                    {
                        var member = field.GetValue(value);
                        var clone = ObjectValues.RecursiveClone(member, cloned);
                        field.SetValue(value, clone);
                    }
                }
            }
        }
    }
}
