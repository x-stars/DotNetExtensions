namespace XstarS.Diagnostics
{
    /// <summary>
    /// 表示能够将自身表示为字符串的对象。
    /// </summary>
    public interface IRepresentable
    {
        /// <summary>
        /// 将当前对象表示为字符串。
        /// </summary>
        /// <returns>表示当前对象的字符串。</returns>
        string Represent();
    }
}
