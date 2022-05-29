using System;

namespace XNetEx.ComponentModel
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
        /// 使用与当前属性相关的属性的名称初始化 <see cref="RelatedPropertiesAttribute"/> 类的新实例。
        /// </summary>
        /// <param name="propertyNames">与当前属性相关的属性的名称。</param>
        [CLSCompliant(false)]
        public RelatedPropertiesAttribute(params string[] propertyNames)
        {
            this.PropertyNames = propertyNames ?? Array.Empty<string>();
        }

        /// <summary>
        /// 获取与当前属性相关的属性的名称。
        /// </summary>
        /// <returns>与当前属性相关的属性的名称。</returns>
        public string[] PropertyNames { get; }
    }
}
