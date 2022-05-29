using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using ReferenceEqualityComparer =
    XNetEx.Collections.Specialized.ReferenceEqualityComparer;

namespace XNetEx.Diagnostics
{
    /// <summary>
    /// 为无环字符串表示对象 <see cref="IAcyclicRepresenter{T}"/> 提供抽象基类。
    /// </summary>
    /// <typeparam name="T">要无环地表示为字符串的对象的类型。</typeparam>
    public abstract class AcyclicRepresenter<T>
        : Representer<T>, IAcyclicRepresenter, IAcyclicRepresenter<T>
    {
        /// <summary>
        /// 表示已经在路径中表示过的对象的字符串表示。
        /// </summary>
        protected const string RepresentedString = "{ ... }";

        /// <summary>
        /// 初始化 <see cref="AcyclicRepresenter{T}"/> 类的新实例。
        /// </summary>
        protected AcyclicRepresenter() { }

        /// <summary>
        /// 将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        public sealed override string Represent(T? value)
        {
            var comparer = ReferenceEqualityComparer.Default;
            var represented = new HashSet<object>(comparer);
            return this.Represent(value, represented);
        }

        /// <summary>
        /// 将指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        protected string Represent(T? value, ISet<object> represented)
        {
            if (value is null) { return Representer<T>.NullRefString; }

            if (!represented.Add(value))
            {
                return AcyclicRepresenter<T>.RepresentedString;
            }

            var represent = this.RepresentCore(value, represented);
            represented.Remove(value);
            return represent;
        }

        /// <summary>
        /// 在派生类中重写，将指定对象表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        protected abstract string RepresentCore([DisallowNull] T value, ISet<object> represented);

        /// <summary>
        /// 将指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        string IAcyclicRepresenter<T>.Represent(T? value, ISet<object> represented)
        {
            return this.Represent(value, represented);
        }

        /// <summary>
        /// 将访指定对象无环地表示为字符串。
        /// </summary>
        /// <param name="value">要表示为字符串的对象。</param>
        /// <param name="represented">已经在路径中表示过的对象。</param>
        /// <returns>表示 <paramref name="value"/> 的字符串。</returns>
        /// <exception cref="InvalidCastException">
        /// 无法强制转换 <paramref name="value"/> 到 <typeparamref name="T"/> 类型。</exception>
        string IAcyclicRepresenter.Represent(object? value, ISet<object> represented)
        {
            if (value is null) { return Representer<T>.NullRefString; }

            return this.Represent((T)value, represented);
        }
    }
}
