using System.Collections;
using System.Collections.Generic;
using XNetEx.Diagnostics;

namespace XNetEx.Collections;

/// <summary>
/// 提供将 <see cref="DictionaryEntry"/> 中的键值表示为字符串的方法。
/// </summary>
internal sealed class DictionaryEntryRepresenter : StructuralRepresenter<DictionaryEntry>
{
    /// <summary>
    /// 初始化 <see cref="DictionaryEntry"/> 类的新实例。
    /// </summary>
    public DictionaryEntryRepresenter() { }

    /// <summary>
    /// 将 <see cref="DictionaryEntry"/> 中的键值表示为字符串。
    /// </summary>
    /// <param name="value">要表示为字符串的 <see cref="DictionaryEntry"/>。</param>
    /// <param name="represented">已经在路径中表示过的对象。</param>
    /// <returns>表示 <paramref name="value"/> 中的键值的字符串。</returns>
    protected override string RepresentCore(DictionaryEntry value, ISet<object> represented)
    {
        var keyRepresenter = StructuralRepresenter.OfType(value.Key?.GetType());
        var valueRepresenter = StructuralRepresenter.OfType(value.Value?.GetType());
        var keyRepresent = keyRepresenter.Represent(value.Key, represented);
        var valueRepresent = valueRepresenter.Represent(value.Value, represented);
        return $"[{keyRepresent}] = {valueRepresent}";
    }
}
