namespace XstarS
{
    /// <summary>
    /// 表示单输入单输出的映射关系。
    /// </summary>
    /// <typeparam name="TKey">映射的输入值的类型。</typeparam>
    /// <typeparam name="TValue">映射的输出值的类型。</typeparam>
    /// <param name="key">映射的输入值。</param>
    /// <returns><paramref name="key"/> 对应的输出值。</returns>
    public delegate TValue Map<in TKey, out TValue>(TKey key);
}
