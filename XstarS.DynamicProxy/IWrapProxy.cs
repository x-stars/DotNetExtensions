namespace XstarS.Reflection
{
    /// <summary>
    /// 表示包装代理类型。
    /// </summary>
    public interface IWrapProxy
    {
        /// <summary>
        /// 获取当前包装的代理对象。
        /// </summary>
        /// <returns>当前包装的代理对象。</returns>
        object GetInstance();
    }
}
