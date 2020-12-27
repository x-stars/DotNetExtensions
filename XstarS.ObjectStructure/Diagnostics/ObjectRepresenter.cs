using System;

namespace XstarS.Diagnostics
{
    /// <summary>
    /// 提供字符串表示对象 <see cref="IObjectRepresenter{T}"/> 的抽象基类。
    /// </summary>
    /// <typeparam name="T">要表示为字符串的对象的类型。</typeparam>
    [Serializable]
    public abstract class ObjectRepresenter<T> : IObjectRepresenter, IObjectRepresenter<T>
    {
        /// <summary>
        /// 初始化 <see cref="ObjectRepresenter{T}"/> 类的新实例。
        /// </summary>
        protected ObjectRepresenter() { }

        /// <summary>
        /// 获取默认的 <see cref="ObjectRepresenter{T}"/> 类的实例。
        /// </summary>
        /// <returns>默认的 <see cref="ObjectRepresenter{T}"/> 类的实例。</returns>
        public static ObjectRepresenter<T> Default { get; } = ObjectRepresenter<T>.CreateDefault();

        /// <summary>
        /// 创建 <see cref="ObjectRepresenter{T}"/> 类的默认实例。
        /// </summary>
        /// <returns><see cref="ObjectRepresenter{T}"/> 类的默认实例。</returns>
        private static ObjectRepresenter<T> CreateDefault()
        {
            if (typeof(IRepresentable).IsAssignableFrom(typeof(T)))
            {
                return (ObjectRepresenter<T>)Activator.CreateInstance(
                    typeof(RepresentableRepresenter<>).MakeGenericType(typeof(T)));
            }
            else
            {
                return new ObjectToStringRepresenter<T>();
            }
        }

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public abstract string Represent(T value);

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IObjectRepresenter.Represent(object value) => this.Represent((T)value);
    }
}
