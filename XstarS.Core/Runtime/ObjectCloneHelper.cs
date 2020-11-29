using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using XstarS.Collections.Specialized;

namespace XstarS.Runtime
{
    /// <summary>
    /// 提供创建对象副本的方法。
    /// </summary>
    internal static class ObjectCloneHelper
    {
        /// <summary>
        /// 表示 <see cref="object.MemberwiseClone()"/> 方法的静态调用委托。
        /// </summary>
        private static readonly Converter<object, object> MemberwiseCloneDelegate =
            typeof(object).GetMethod(nameof(ObjectCloneHelper.MemberwiseClone),
                BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(
                    typeof(Converter<object, object>)) as Converter<object, object>;

        /// <summary>
        /// 表示用于二进制序列化和反序列化对象的 <see cref="BinaryFormatter"/> 对象。
        /// </summary>
        private static readonly BinaryFormatter BinarySerializer = new BinaryFormatter();

        /// <summary>
        /// 获取指定对象的浅表副本。
        /// </summary>
        /// <param name="value">要获取浅表副本的对象。</param>
        /// <returns><paramref name="value"/> 的浅表副本。</returns>
        public static object ObjectClone(object value)
        {
            return (value is null) ? null : ObjectCloneHelper.MemberwiseCloneDelegate.Invoke(value);
        }

        /// <summary>
        /// 创建指定对象的深度副本。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <param name="value">要获取深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        /// <exception cref="MemberAccessException">调用方没有权限来访问对象的成员。</exception>
        public static object ObjectRecurseClone(object value)
        {
            if (value is null) { return null; }
            var comparer = ReferenceEqualityComparer<object>.Default;
            var cloned = new Dictionary<object, object>(comparer);
            return (value is null) ? null : ObjectCloneHelper.ObjectRecurseClone(value, cloned);
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
        public static object SerializationClone(object value)
        {
            if (value is null) { return null; }
            using (var stream = new MemoryStream())
            {
                ObjectCloneHelper.BinarySerializer.Serialize(stream, value);
                stream.Position = 0L;
                return ObjectCloneHelper.BinarySerializer.Deserialize(stream);
            }
        }

        /// <summary>
        /// 根据类型获取指定对象的深度副本。
        /// </summary>
        /// <param name="value">要获取深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        /// <param name="cloned">已经创建副本的对象及其对应的副本。</param>
        private static object ObjectRecurseClone(object value, Dictionary<object, object> cloned)
        {
            if (value is null) { return null; }

            if (cloned.ContainsKey(value)) { return cloned[value]; }

            var clone = ObjectCloneHelper.ObjectClone(value);
            cloned[value] = clone;

            var type = clone.GetType();
            if (type.IsArray)
            {
                ObjectCloneHelper.ArrayElementsRecurseClone((Array)clone, cloned);
            }
            else if (!type.IsPrimitive)
            {
                ObjectCloneHelper.ObjectMembersRecurseClone(clone, cloned);
            }

            return clone;
        }

        /// <summary>
        /// 将指定数组的每个元素替换为其深度副本。
        /// </summary>
        /// <param name="value">要将元素替换为其深度副本的数组。</param>
        /// <param name="cloned">已经创建副本的对象及其对应的副本。</param>
        private static void ArrayElementsRecurseClone(Array value, Dictionary<object, object> cloned)
        {
            if (!value.GetType().GetElementType().IsPointer)
            {
                if (value.Rank == 1)
                {
                    for (int index = 0; index < value.Length; index++)
                    {
                        var item = value.GetValue(index);
                        var clone = ObjectCloneHelper.ObjectRecurseClone(item, cloned);
                        value.SetValue(clone, index);
                    }
                }
                else
                {
                    for (int offset = 0; offset < value.Length; offset++)
                    {
                        var indices = value.OffsetToIndices(offset);
                        var item = value.GetValue(indices);
                        var clone = ObjectCloneHelper.ObjectRecurseClone(item, cloned);
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
        private static void ObjectMembersRecurseClone(object value, Dictionary<object, object> cloned)
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
                        var clone = ObjectCloneHelper.ObjectRecurseClone(member, cloned);
                        field.SetValue(value, clone);
                    }
                }
            }
        }
    }
}
