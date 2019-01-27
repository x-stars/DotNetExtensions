using System;
using System.Reflection;

namespace XstarS
{
    /// <summary>
    /// 提供类型无关的通用扩展方法。
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 返回当前对象的字符串表达形式。
        /// 若当前对象为 <see langword="null"/>，或 <see cref="object.ToString()"/>
        /// 方法的返回值为 <see langword="null"/>，则返回 <see cref="string.Empty"/>。
        /// </summary>
        /// <param name="source">一个 <see cref="object"/> 类型的对象。</param>
        /// <returns><paramref name="source"/> 的字符串表达形式；
        /// 若 <paramref name="source"/> 为 <see langword="null"/>，
        /// 或 <paramref name="source"/> 的 <see cref="object.ToString()"/>
        /// 方法的返回值为 <see langword="null"/>，则为 <see cref="string.Empty"/>。</returns>
        public static string ToStringOrEmpty(this object source) =>
            source?.ToString() ?? string.Empty;

        /// <summary>
        /// 确定当前对象与指定对象的所有字段的值（对数组则是所有元素的值）是否相等。
        /// 将递归比较至字段（元素）为 .NET 原生类型（<see cref="TypeExtensions.NativeTypes"/>）或指针类型。
        /// </summary>
        /// <remarks>基于反射调用，可能存在性能问题。</remarks>
        /// <param name="source">一个 <see cref="object"/> 类型的对象。</param>
        /// <param name="other">要与当前对象进行比较的对象。</param>
        /// <returns>若 <paramref name="source"/> 与 <paramref name="other"/> 的所有实例字段都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
        public static bool ValueEquals(this object source, object other)
        {
            // 引用比较。
            if (object.ReferenceEquals(source, other)) { return true; }
            if ((source is null) ^ (other is null)) { return false; }
            // 类型不同。
            if (source.GetType() != other.GetType()) { return false; }

            #region 用于相等比较的函数。

            // 指针包装相等比较函数。
            unsafe bool PointerValueEquals(Pointer x, Pointer y)
            {
                // 引用比较。
                if (object.ReferenceEquals(x, y)) { return true; }
                if ((x is null) ^ (y is null)) { return false; }

                // 指针值比较。
                if (Pointer.Unbox(x) != Pointer.Unbox(y))
                {
                    return false;
                }
                // 指针类型比较。
                else
                {
                    var t_Pointer_im_GetPointerType = typeof(Pointer).GetMethod(
                        "GetPointerType", BindingFlags.Instance | BindingFlags.NonPublic);
                    var x_GetPointerType = t_Pointer_im_GetPointerType.Invoke(x, new object[0]);
                    var y_GetPointerType = t_Pointer_im_GetPointerType.Invoke(y, new object[0]);
                    return object.Equals(x_GetPointerType, y_GetPointerType);
                }
            }

            // 数组相等比较函数。
            bool ArrayValueEquals(Array x, Array y)
            {
                // 引用比较。
                if (object.ReferenceEquals(x, y)) { return true; }
                if ((x is null) ^ (y is null)) { return false; }
                // 类型不同。
                if (x.GetType() != y.GetType()) { return false; }
                // 大小不等。
                if (x.Rank != y.Rank) { return false; }
                if (x.LongLength != y.LongLength) { return false; }
                for (int i = 0; i < x.Rank; i++)
                {
                    if (x.GetLength(i) != y.GetLength(i))
                    {
                        return false;
                    }
                }

                var t_Array = x.GetType();
                bool isMultiDim = x.Rank > 1;
                // 指针数组。
                if (t_Array.GetElementType().IsPointer)
                {
                    var t_Array_im_Get = t_Array.GetMethod("Get");
                    for (long i = 0; i < x.LongLength; i++)
                    {
                        var xi = t_Array_im_Get.Invoke(x, isMultiDim ?
                            Array.ConvertAll(x.IndexV2T(i), item => (object)item) :
                            new object[] { i });
                        var yi = t_Array_im_Get.Invoke(y, isMultiDim ?
                            Array.ConvertAll(y.IndexV2T(i), item => (object)item) :
                            new object[] { i });

                        if (!ObjectExtensions.ValueEquals(xi, yi))
                        {
                            return false;
                        }
                    }
                }
                // 一般数组。
                else
                {
                    for (long i = 0; i < x.LongLength; i++)
                    {
                        var xi = isMultiDim ? x.GetValue(x.IndexV2T(i)) : x.GetValue(i);
                        var yi = isMultiDim ? y.GetValue(y.IndexV2T(i)) : y.GetValue(i);

                        if (!ObjectExtensions.ValueEquals(xi, yi))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            #endregion

            var t_source = source.GetType();
            // object 类型，直接认为相等。
            if (t_source == typeof(object))
            {
                return true;
            }
            // 原生类型，直接比较值。
            else if (t_source.IsNative())
            {
                if (!object.Equals(source, other))
                {
                    return false;
                }
            }
            // 指针包装类型，拆箱为 void* 进行比较。
            else if (t_source == typeof(Pointer))
            {
                if (!PointerValueEquals(source as Pointer, other as Pointer))
                {
                    return false;
                }
            }
            // 数组类型，依次比较每个值。
            else if (t_source.IsArray)
            {
                if (!ArrayValueEquals(source as Array, other as Array))
                {
                    return false;
                }
            }
            // 其他类型，对每个字段递归调用。
            else
            {
                // 获取每个实例字段。
                var t_source_if_Fields = t_source.GetFields(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                // 依次递归比较每个字段。
                foreach (var t_source_if_Field in t_source_if_Fields)
                {
                    var source_Field = t_source_if_Field.GetValue(source);
                    var other_Field = t_source_if_Field.GetValue(other);

                    if (!ObjectExtensions.ValueEquals(source_Field, other_Field))
                    {
                        return false;
                    }
                }

                // 递归进行基类的比较。
                for (var t_base = t_source.BaseType;
                    !(t_base is null);
                    t_base = t_base.BaseType)
                {
                    // 获取每个实例字段。
                    var t_base_if_Fields = t_base.GetFields(
                        BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    // 依次递归比较每个字段。
                    foreach (var t_base_if_Field in t_base_if_Fields)
                    {
                        var source_Field = t_base_if_Field.GetValue(source);
                        var other_Field = t_base_if_Field.GetValue(other);

                        if (!ObjectExtensions.ValueEquals(source_Field, other_Field))
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
