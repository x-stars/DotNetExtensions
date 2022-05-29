using System.Collections;
using System.Collections.Generic;
using XstarS.Diagnostics;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供用于将 <see cref="IEnumerable"/> 中的元素表示为字符串的方法。
    /// </summary>
    /// <typeparam name="T">实现了 <see cref="IEnumerable"/> 接口的集合类型。</typeparam>
    internal sealed class EnumerableRepresenter<T> : StructuralRepresenter<T>
        where T : IEnumerable
    {
        /// <summary>
        /// 初始化 <see cref="EnumerableRepresenter{T}"/> 类的新实例。
        /// </summary>
        public EnumerableRepresenter() { }

        /// <summary>
        /// 将 <see cref="IEnumerable"/> 中的元素表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的 <see cref="IEnumerable"/>。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 中的元素的字符串。</returns>
        protected override string RepresentCore(T value, ISet<object> represented)
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
