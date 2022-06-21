using System.Collections.Generic;
using XNetEx.Diagnostics;

namespace XNetEx.Collections.Generic;

/// <summary>
/// 提供将 <see cref="KeyValuePair{TKey, TValue}"/> 中的键值表示为字符串的方法。
/// </summary>
/// <typeparam name="TKey"><see cref="KeyValuePair{TKey, TValue}"/> 中的键的类型。</typeparam>
/// <typeparam name="TValue"><see cref="KeyValuePair{TKey, TValue}"/> 中的值的类型。</typeparam>
internal sealed class KeyValuePairRepresenter<TKey, TValue>
    : StructuralRepresenter<KeyValuePair<TKey, TValue>>
{
    /// <summary>
    /// 初始化 <see cref="KeyValuePairRepresenter{TKey, TValue}"/> 类的新实例。
    /// </summary>
    public KeyValuePairRepresenter() { }

    /// <summary>
    /// 将 <see cref="KeyValuePair{TKey, TValue}"/> 中的键值表示为字符串。
    /// </summary>
    /// <param name="value">要表示为字符串的 <see cref="KeyValuePair{TKey, TValue}"/>。</param>
    /// <param name="represented">已经在路径中表示过的对象。</param>
    /// <returns>表示 <paramref name="value"/> 中的键值的字符串。</returns>
    protected override string RepresentCore(
        KeyValuePair<TKey, TValue> value, ISet<object> represented)
    {
        var keyRepresenter = StructuralRepresenter.OfType(value.Key?.GetType());
        var valueRepresenter = StructuralRepresenter.OfType(value.Value?.GetType());
        var keyRepresent = keyRepresenter.Represent(value.Key, represented);
        var valueRepresent = valueRepresenter.Represent(value.Value, represented);
        return $"[{keyRepresent}] = {valueRepresent}";
    }
}
