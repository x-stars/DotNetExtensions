using System;

namespace XNetEx.Text
{
    /// <summary>
    /// 表示将输入的 URI 字符串转换为其等效的 <see cref="Uri"/> 对象的字符串解析对象。
    /// </summary>
    internal sealed class UriStringParser : SimpleStringParser<Uri>
    {
        /// <summary>
        /// 初始化 <see cref="UriStringParser"/> 类的新实例。
        /// </summary>
        public UriStringParser() { }

        /// <summary>
        /// 将指定的 URI 字符串转换为其等效的 <see cref="Uri"/> 对象。
        /// </summary>
        /// <param name="text">表示 URI 的字符串。</param>
        /// <returns>URI 为 <paramref name="text"/> 的 <see cref="Uri"/> 对象。</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="text"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="UriFormatException">
        /// <paramref name="text"/> 不表示有效的 URI。</exception>
        public override Uri Parse(string text) => new Uri(text);
    }
}
