using System;
using System.Collections.Generic;
using System.Reflection;
using XstarS.Collections.Specialized;

namespace XstarS
{
    /// <summary>
    /// 提供创建对象的浅表副本和深层副本的方法。
    /// </summary>
    [Serializable]
    internal sealed class CloneableObject
    {
        /// <summary>
        /// <see cref="object.MemberwiseClone()"/> 方法的静态委托调用。
        /// </summary>
        private static readonly Func<object, object> StaticMemberwiseClone =
            typeof(object).GetMethod(nameof(MemberwiseClone),
                BindingFlags.Instance | BindingFlags.NonPublic).CreateDelegate(
                    typeof(Func<object, object>)) as Func<object, object>;

        /// <summary>
        /// 要创建副本的对象。
        /// </summary>
        public readonly object Value;

        /// <summary>
        /// 已经创建副本的对象及其对应的副本。
        /// </summary>
        [NonSerialized]
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
        /// 创建当前对象的浅表副本。
        /// </summary>
        /// <returns><see cref="CloneableObject.Value"/> 的浅表副本。</returns>
        public object ShallowClone()
        {
            var value = this.Value;

            return (value is null) ? null :
                CloneableObject.StaticMemberwiseClone(value);
        }

        /// <summary>
        /// 创建当前对象的深度副本。
        /// </summary>
        /// <returns><see cref="CloneableObject.Value"/> 的深度副本。</returns>
        public object DeepClone()
        {
            this.Cloned = this.Cloned ??
                new Dictionary<object, object>(
                    ReferenceEqualityComparer<object>.Default);

            var clone = this.TypedDeepClone();

            this.Cloned = null;
            return clone;
        }

        /// <summary>
        /// 获取指定对象的深度副本。
        /// </summary>
        /// <param name="value">要获取深度副本的对象。</param>
        /// <returns><paramref name="value"/> 的深度副本。</returns>
        private object DeepClone(object value) =>
            new CloneableObject(value) { Cloned = this.Cloned }.DeepClone();

        /// <summary>
        /// 根据类型创建当前对象的深度副本。
        /// </summary>
        /// <returns><see cref="CloneableObject.Value"/> 的深度副本。</returns>
        private object TypedDeepClone()
        {
            var value = this.Value;

            // 空引用。
            if (value is null) { return null; }
            // 已有副本的对象。
            if (this.Cloned.ContainsKey(value)) { return this.Cloned[value]; }

            // 创建当前对象的浅表副本。
            var clone = this.ShallowClone();
            this.Cloned[value] = clone;

            // 根据当前对象的类型确定创建成员副本的方法。
            var type = value.GetType();
            if (type.IsPrimitive) { }
            else if (type == typeof(string)) { }
            else if (type.IsArray) { this.ArrayElementsDeepClone(); }
            else { this.ObjectMembersDeepClone(); }

            return clone;
        }

        /// <summary>
        /// 将当前实例包含的数组的浅表副本的每个元素替换为其深度副本。
        /// </summary>
        private void ArrayElementsDeepClone()
        {
            var clone = (Array)this.Cloned[this.Value];

            // 仅创建非指针元素的副本。
            if (!clone.GetType().GetElementType().IsPointer)
            {
                // 将每个元素替换为其深度副本。
                if (clone.Rank == 1)
                {
                    for (long i = 0L; i < clone.LongLength; i++)
                    {
                        clone.SetValue(this.DeepClone(clone.GetValue(i)), i);
                    }
                }
                else
                {
                    for (long i = 0L; i < clone.LongLength; i++)
                    {
                        var a = clone.OffsetToIndices(i);
                        clone.SetValue(this.DeepClone(clone.GetValue(a)), a);
                    }
                }
            }
        }

        /// <summary>
        /// 将当前实例包含的对象的浅表副本的每个实例字段替换为其深度副本。
        /// </summary>
        private void ObjectMembersDeepClone()
        {
            var clone = this.Cloned[this.Value];

            for (var type = clone.GetType(); !(type is null); type = type.BaseType)
            {
                // 将每个非指针实例字段的值替换为其深度副本。
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var field in fields)
                {
                    if (!field.FieldType.IsPointer)
                    {
                        field.SetValue(clone, this.DeepClone(field.GetValue(clone)));
                    }
                }
            }
        }
    }
}
