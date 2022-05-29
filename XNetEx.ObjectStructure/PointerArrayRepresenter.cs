using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using XNetEx.Diagnostics;

namespace XNetEx
{
    /// <summary>
    /// 提供指针将数组中的元素表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">指针数组的类型。</typeparam>
    internal sealed class PointerArrayRepresenter<T> : StructuralRepresenter<T>
    {
        /// <summary>
        /// 表示数组根据索引获取元素的方法的 <see cref="MethodInfo"/> 对象。
        /// </summary>
        private static readonly MethodInfo GetMethod = typeof(T).GetMethod("Get")!;

        /// <summary>
        /// 初始化 <see cref="PointerArrayRepresenter{T}"/> 类的新实例。
        /// </summary>
        public PointerArrayRepresenter() { }

        /// <summary>
        /// 将指定指针数组中的元素表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的指针数组。</param>
        /// <param name="represented">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 中的元素的字符串。</returns>
        protected override string RepresentCore([DisallowNull] T value, ISet<object> represented)
        {
            return value.GetType().ToString() + " " +
                this.RepresentArray((Array)(object)value, Array.Empty<int>(), represented);
        }

        /// <summary>
        /// 将指定指针数组中指定索引处的元素表示为字符串。
        /// </summary>
        /// <param name="array">要将元素表示为字符串的指针数组。</param>
        /// <param name="indices">要表示为字符串的元素的索引。</param>
        /// <param name="represented">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="array"/> 中索引为
        /// <paramref name="indices"/> 的元素的字符串。</returns>
        private string RepresentArray(Array array, int[] indices, ISet<object> represented)
        {
            if (indices.Length == array.Rank)
            {
                var item = PointerArrayRepresenter<T>.GetMethod.Invoke(array, indices.Box())!;
                unsafe { return ((nint)Pointer.Unbox(item)).ToString(); }
            }
            else
            {
                var represents = new List<string>();
                var length = array.GetLength(indices.Length);
                foreach (var index in ..length)
                {
                    var nextIndices = indices.Append(index);
                    represents.Add(this.RepresentArray(array, nextIndices, represented));
                }
                return $"{{ {string.Join(", ", represents)} }}";
            }
        }
    }
}
