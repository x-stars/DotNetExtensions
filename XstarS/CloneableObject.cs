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
        private IDictionary<object, object> Cloned;

        /// <summary>
        /// 使用要创建副本的对象初始化 <see cref="CloneableObject"/> 类的新实例。
        /// </summary>
        /// <param name="value">要创建副本的对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为 <see langword="null"/>。</exception>
        public CloneableObject(object value)
        {
            this.Value = value ??
                throw new ArgumentNullException(nameof(value));
        }

        /// <summary>
        /// 使用要创建副本的对象和已经创建副本的对象初始化 <see cref="CloneableObject"/> 类的新实例。
        /// </summary>
        /// <param name="value">要创建副本的对象。</param>
        /// <param name="cloned">已经创建副本的对象及其副本的
        /// <see cref="IDictionary{TKey, TValue}"/> 对象。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> 为 <see langword="null"/>。</exception>
        private CloneableObject(object value,
            IDictionary<object, object> cloned) : this(value)
        {
            this.Cloned = cloned;
        }

        /// <summary>
        /// 创建当前对象的浅表副本。
        /// </summary>
        /// <returns><see cref="CloneableObject.Value"/> 的浅表副本。</returns>
        public object ShallowClone()
        {
            return CloneableObject.StaticMemberwiseClone(this.Value);
        }

        /// <summary>
        /// 创建当前对象的深度副本。
        /// </summary>
        /// <returns><see cref="CloneableObject.Value"/> 的深度副本。</returns>
        public object DeepClone()
        {
            // 创建当前对象的浅表副本。
            var value = this.Value;
            var clone = this.ShallowClone();
            this.Cloned = this.Cloned ??
                new Dictionary<object, object>(ReferenceEqualityComparer.Default);
            var cloned = this.Cloned;
            cloned.Add(value, clone);

            // 根据当前对象的类型确定创建成员副本的方法。
            var type = this.Value.GetType();
            // 基元类型，不创建成员的副本。
            if (type.IsPrimitive) { }
            // 数组类型，调用对应方法以创建元素的副本。
            else if (type.IsArray)
            {
                this.DeepCloneArrayElements((Array)clone);
            }
            // 其他类型，调用对应方法以创建字段的副本。
            else
            {
                this.DeepCloneObjectMembers(clone);
            }

            // 返回当前对象的深层副本。
            this.Cloned = null;
            return clone;
        }

        /// <summary>
        /// 获取指定对象的深层副本。
        /// </summary>
        /// <param name="value">要获取副本的对象。</param>
        /// <returns><paramref name="value"/> 的副本。</returns>
        private object GetDeepCloneOf(object value)
        {
            if (value is null) { return null; }
            var cloned = this.Cloned;
            return cloned.ContainsKey(value) ? cloned[value] :
                (cloned[value] = new CloneableObject(value, cloned).DeepClone());
        }

        /// <summary>
        /// 将数组的每个元素替换为其深层副本。
        /// </summary>
        /// <param name="array">要对每个元素创建深层副本的数组。</param>
        private void DeepCloneArrayElements(Array array)
        {
            // 仅创建非指针元素的副本。
            if (!array.GetType().GetElementType().IsPointer)
            {
                // 一维数组，将每个元素替换为其副本。
                if (array.Rank == 1)
                {
                    for (long i = 0; i < array.LongLength; i++)
                    {
                        array.SetValue(this.GetDeepCloneOf(array.GetValue(i)), i);
                    }
                }
                // 多维数组，将其映射为一维数组，并将每个元素替换为其副本。
                else
                {
                    for (long i = 0; i < array.LongLength; i++)
                    {
                        var a = array.OffsetToIndices(i);
                        array.SetValue(this.GetDeepCloneOf(array.GetValue(a)), a);
                    }
                }
            }
        }

        /// <summary>
        /// 将对象的每个实例字段替换为其深层副本。
        /// </summary>
        /// <param name="object">要对实例字段创建深层副本的对象。</param>
        private void DeepCloneObjectMembers(object @object)
        {
            for (var type = @object.GetType(); !(type is null); type = type.BaseType)
            {
                // 将每个非指针实例字段的值替换为其副本。
                var fields = type.GetFields(BindingFlags.DeclaredOnly |
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var field in fields)
                {
                    if (!field.FieldType.IsPointer)
                    {
                        field.SetValue(@object, this.GetDeepCloneOf(field.GetValue(@object)));
                    }
                }
            }
        }
    }
}
