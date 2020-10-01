﻿using System;
using System.Collections.Generic;
using System.Reflection;
using XstarS.Collections.Specialized;

namespace XstarS
{
    /// <summary>
    /// 提供创建对象的浅表副本和深层副本的方法。
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
    internal sealed class CloneableObject
    {
        /// <summary>
        /// <see cref="object.MemberwiseClone()"/> 方法的静态委托调用。
        /// </summary>
        private static readonly Converter<object, object> StaticMemberwiseClone =
            typeof(object).GetMethod(nameof(MemberwiseClone),
                BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(
                    typeof(Converter<object, object>)) as Converter<object, object>;

        /// <summary>
        /// 表示已经创建副本的对象及其对应的副本。
        /// </summary>
        private Dictionary<object, object> Cloned;

        /// <summary>
        /// 使用要创建副本的对象初始化 <see cref="CloneableObject"/> 类的新实例。
        /// </summary>
        /// <param name="value">要创建副本的对象。</param>
        public CloneableObject(object value)
        {
            this.Value = value;
        }

        /// <summary>
        /// 获取要为其创建副本的对象。
        /// </summary>
        /// <returns>要为其创建副本的对象。</returns>
        public object Value { get; }

        /// <summary>
        /// 创建当前对象的浅表副本。
        /// </summary>
        /// <returns><see cref="CloneableObject.Value"/> 的浅表副本。</returns>
        public object ShallowClone()
        {
            return this.ShallowClone(this.Value);
        }

        /// <summary>
        /// 创建当前对象的深度副本。
        /// </summary>
        /// <returns><see cref="CloneableObject.Value"/> 的深度副本。</returns>
        public object DeepClone()
        {
            this.Cloned = new Dictionary<object, object>(
                ReferenceEqualityComparer<object>.Default);

            var clone = this.DeepClone(this.Value);

            this.Cloned = null;
            return clone;
        }

        /// <summary>
        /// 获取指定对象的浅表副本。
        /// </summary>
        /// <param name="value">要获取浅表副本的对象。</param>
        /// <returns><paramref name="value"/> 的浅表副本。</returns>
        private object ShallowClone(object value)
        {
            return (value is null) ? null :
                CloneableObject.StaticMemberwiseClone(value);
        }

        /// <summary>
        /// 根据类型获取指定对象的深度副本。
        /// </summary>
        /// <param name="value">要获取深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        private object DeepClone(object value)
        {
            if (value is null) { return null; }

            if (this.Cloned.ContainsKey(value)) { return this.Cloned[value]; }

            var clone = this.ShallowClone(value);
            this.Cloned[value] = clone;

            var type = clone.GetType();
            if (type.IsPrimitive) { }
            else if (type == typeof(string)) { }
            else if (type.IsArray) { this.ArrayElementsDeepClone((Array)clone); }
            else { this.ObjectMembersDeepClone(clone); }

            return clone;
        }

        /// <summary>
        /// 将指定数组的每个元素替换为其深度副本。
        /// </summary>
        /// <param name="value">要将元素替换为其深度副本的数组。</param>
        private void ArrayElementsDeepClone(Array value)
        {
            if (!value.GetType().GetElementType().IsPointer)
            {
                if (value.Rank == 1)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        value.SetValue(this.DeepClone(value.GetValue(i)), i);
                    }
                }
                else
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        var a = value.OffsetToIndices(i);
                        value.SetValue(this.DeepClone(value.GetValue(a)), a);
                    }
                }
            }
        }

        /// <summary>
        /// 将指定对象的每个实例字段替换为其深度副本。
        /// </summary>
        /// <param name="value">要将成员替换为其深度副本的对象。</param>
        private void ObjectMembersDeepClone(object value)
        {
            for (var type = value.GetType(); !(type is null); type = type.BaseType)
            {
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var field in fields)
                {
                    if (!field.FieldType.IsPointer)
                    {
                        field.SetValue(value, this.DeepClone(field.GetValue(value)));
                    }
                }
            }
        }
    }
}
