using System;
using System.Collections.Generic;
using System.Reflection;
using XstarS.Diagnostics;

namespace XstarS
{
    /// <summary>
    /// 提供将数组中的元素表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">数组的类型。</typeparam>
    [Serializable]
    internal sealed class ArrayRepresenter<T> : ObjectRepresenter<T>
    {
        /// <summary>
        /// 表示当前数组类型的元素类型是否为指针类型。
        /// </summary>
        private static readonly bool ItemIsPointer = typeof(T).GetElementType().IsPointer;

        /// <summary>
        /// 表示获取指针数组指定索引处的元素的方法的 <see cref="MemberInfo"/> 对象。
        /// </summary>
        private static readonly MethodInfo PointerGetMethod =
            ArrayRepresenter<T>.ItemIsPointer ? typeof(T).GetMethod("Get") : null;

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
        protected override string RepresentCore(T value, ISet<object> represented)
        {
            return this.RepresentArray((Array)(object)value, Array.Empty<int>(), represented);
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
                if (ArrayRepresenter<T>.ItemIsPointer)
                {
                    var item = ArrayRepresenter<T>.PointerGetMethod.Invoke(array, indices.Box());
                    var representer = ObjectRepresenter.OfType(typeof(Pointer));
                    return representer.Represent(item, represented);
                }
                else
                {
                    var item = array.GetValue(indices);
                    var representer = ObjectRepresenter.OfType(item?.GetType());
                    return representer.Represent(item, represented);
                }
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
