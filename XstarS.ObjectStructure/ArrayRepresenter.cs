using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using XstarS.Diagnostics;

namespace XstarS
{
    /// <summary>
    /// 提供将数组中的元素表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">数组的类型。</typeparam>
    internal sealed class ArrayRepresenter<T> : InternalStructuralRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="ArrayRepresenter{T}"/> 类的新实例。
        /// </summary>
        public ArrayRepresenter() { }

        /// <summary>
        /// 将指定数组中的元素表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的数组。</param>
        /// <param name="represented">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 中的元素的字符串。</returns>
        protected override string RepresentCore([DisallowNull] T value, ISet<object> represented)
        {
            return value.GetType().ToString() + " " +
                this.RepresentArray((Array)(object)value, Array.Empty<int>(), represented);
        }

        /// <summary>
        /// 将指定数组中指定索引处的元素表示为字符串。
        /// </summary>
        /// <param name="array">要将元素表示为字符串的数组。</param>
        /// <param name="indices">要表示为字符串的元素的索引。</param>
        /// <param name="represented">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="array"/> 中索引为
        /// <paramref name="indices"/> 的元素的字符串。</returns>
        private string RepresentArray(Array array, int[] indices, ISet<object> represented)
        {
            if (indices.Length == array.Rank)
            {
                var item = array.GetValue(indices);
                var representer = StructuralRepresenter.OfType(item?.GetType());
                return representer.Represent(item, represented);
            }
            else
            {
                var represents = new List<string>();
                var length = array.GetLength(indices.Length);
                for (int index = 0; index < length; index++)
                {
                    var nextIndices = indices.Append(index);
                    represents.Add(this.RepresentArray(array, nextIndices, represented));
                }
                return $"{{ {string.Join(", ", represents)} }}";
            }
        }
    }
}
