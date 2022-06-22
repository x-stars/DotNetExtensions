#if NET471_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using XNetEx.Diagnostics;

namespace XNetEx;

/// <summary>
/// 提供将元组中的元素的表示为字符串的方法。
/// </summary>
/// <typeparam name="T">实现了 <see cref="ITuple"/> 接口的元组的类型。</typeparam>
internal sealed class TupleRepresenter<T> : StructuralRepresenter<T>
    where T : ITuple
{
    /// <summary>
    /// 初始化 <see cref="TupleRepresenter{T}"/> 类的新实例。
    /// </summary>
    public TupleRepresenter() { }

    /// <summary>
    /// 将指定元组中的元素表示为字符串。
    /// </summary>
    /// <param name="value">要表示为字符串的元组。</param>
    /// <param name="represented">已经在路径中访问过的对象。</param>
    /// <returns>表示 <paramref name="value"/> 中的元素的字符串。</returns>
    protected override string RepresentCore(T value, ISet<object> represented)
    {
        var represents = new List<string>();
        foreach (var index in ..value.Length)
        {
            var item = value[index];
            var representer = StructuralRepresenter.OfType(item?.GetType());
            represents.Add(representer.Represent(item, represented));
        }
        return "(" + string.Join(", ", represents) + ")";
    }
}
#endif
