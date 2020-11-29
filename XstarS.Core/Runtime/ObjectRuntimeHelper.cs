namespace XstarS.Runtime
{
    /// <summary>
    /// 提供对象运行时级别的帮助方法。
    /// </summary>
    internal static partial class ObjectRuntimeHelper
    {
        /// <summary>
        /// 返回指定索引的装箱对象。
        /// </summary>
        /// <param name="index">要获取装箱对象的索引。</param>
        /// <returns><paramref name="index"/> 的装箱对象。</returns>
        private static object BoxIndex(int index) => index;
    }
}
