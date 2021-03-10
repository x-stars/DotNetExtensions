using System;

namespace XstarS.Text
{
    /// <summary>
    /// 表示原样返回的输入字符串的字符串解析对象。
    /// </summary>
    [Serializable]
    internal sealed class StringValueParser : SimpleValueParser<string>
    {
        /// <summary>
        /// 初始化 <see cref="ValueParser"/> 类的新实例。
        /// </summary>
        public StringValueParser() { }

        /// <summary>
        /// 将输入的字符串原样返回。
        /// </summary>
        /// <param name="text">要解析的字符串。</param>
        /// <returns><paramref name="text"/> 本身。</returns>
        public override string Parse(string text) => text;
    }
}
