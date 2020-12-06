using System;

namespace XstarS
{
    /// <summary>
    /// 提供可克隆对象 <see cref="ICloneable"/> 的扩展方法。
    /// </summary>
    public static class CloneableExtensions
    {
        /// <summary>
        /// 创建当前可克隆对象的副本。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="value">要创建副本的对象。</param>
        /// <returns><paramref name="value"/> 的副本。</returns>
        public static T TypedClone<T>(this T value) where T : ICloneable => (T)value?.Clone();
    }
}
