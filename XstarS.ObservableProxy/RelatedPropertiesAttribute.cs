using System;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 指示与当前属性的值相关的其他属性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public sealed class RelatedPropertiesAttribute : Attribute
    {
        /// <summary>
        /// 使用与当前属性相关的属性的名称初始化 <see cref="RelatedPropertiesAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="propertyNames">与当前属性相关的属性的名称，使用半角逗号分隔。</param>
        public RelatedPropertiesAttribute(string propertyNames)
        {
            this.PropertyNames = Array.ConvertAll((propertyNames ?? "").Split(
                new[] { ',' }, StringSplitOptions.RemoveEmptyEntries), name => name.Trim());
        }

        /// <summary>
        /// 与当前属性相关的属性的名称。
        /// </summary>
        public string[] PropertyNames { get; }
    }
}
