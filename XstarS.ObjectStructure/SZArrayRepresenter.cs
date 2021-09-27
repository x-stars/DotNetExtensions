using System;
using System.Collections.Generic;
using XstarS.Diagnostics;

namespace XstarS
{
    /// <summary>
    /// 提供将数组中的元素的表示为字符串的方法。
    /// </summary>
    /// <typeparam name="TItem">数组中的元素的类型。</typeparam>
    [Serializable]
    internal sealed class SZArrayRepresenter<TItem> : StructuralRepresenterBase<TItem[]>
    {
        /// <summary>
        /// 初始化 <see cref="SZArrayRepresenter{TItem}"/> 类的新实例。
        /// </summary>
        public SZArrayRepresenter() { }

        /// <summary>
        /// 将指定数组中的元素表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的数组。</param>
        /// <param name="represented">已经在路径中访问过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 中的元素的字符串。</returns>
        protected override string RepresentCore(TItem[] value, ISet<object> represented)
        {
            var represents = new List<string>();
            foreach (var item in value)
            {
                var representer = StructuralRepresenter.OfType(item?.GetType());
                represents.Add(representer.Represent(item, represented));
            }
            return $"{value.GetType().ToString()} {{ {string.Join(", ", represents)} }}";
        }
    }
}
