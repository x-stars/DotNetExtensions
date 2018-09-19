using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XstarS
{
    /// <summary>
    /// 延迟初始化对象 <see cref="Lazy{T}"/> 的索引访问支持。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class LazyArray<T> : IReadOnlyList<T>, IEquatable<LazyArray<T>>, IDisposable
    {
        /// <summary>
        /// 初始化对象 <see cref="Lazy{T}"/> 的数组。
        /// </summary>
        private readonly Lazy<T>[] lazyObjects;
        /// <summary>
        /// 调用用于在需要时生成延迟初始化值的委托的列表。
        /// </summary>
        private readonly Func<T>[] valueFactories;

        /// <summary>
        /// 初始化 <see cref="LazyArray{T}"/> 类的新实例。
        /// 出现迟缓初始化时，将使用指定的初始化函数。
        /// </summary>
        /// <param name="valueFactories">
        /// 调用用于在需要时生成延迟初始化值的委托的列表。</param>
        public LazyArray(Func<T>[] valueFactories)
        {
            this.valueFactories = valueFactories;
            this.lazyObjects = new Lazy<T>[this.valueFactories.Length];
            for (int i = 0; i < this.valueFactories.Length; i++)
            { this.lazyObjects[i] = new Lazy<T>(this.valueFactories[i]); }
        }

        /// <summary>
        /// 获取 <see cref="LazyArray{T}"/> 实例的指定位置的延迟初始化值。
        /// </summary>
        /// <param name="index">获取延迟初始化值的位置。</param>
        /// <returns><see cref="LazyArray{T}"/> 实例的指定位置的延迟初始化值。</returns>
        public T this[int index] => this.lazyObjects[index].Value;
        
        /// <summary>
        /// 获取 <see cref="LazyArray{T}"/> 中的元素数。
        /// </summary>
        /// <returns>集合中的元素数。</returns>
        public int Count => this.lazyObjects.Length;

        /// <summary>
        /// 释放指定位置的延迟初始化值。
        /// </summary>
        /// <param name="index">要释放延迟初始化值的位置。</param>
        public void Dispose(int index) =>
            this.lazyObjects[index] = new Lazy<T>(this.valueFactories[index]);

        /// <summary>
        /// 释放所有的延迟初始化值。
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < this.valueFactories.Length; i++)
            { this.Dispose(i); }
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="LazyArray{T}"/> 对象是否表示相同的值。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="LazyArray{T}"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的所有元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(LazyArray<T> other)
        {
            if (other is null) { return false; }
            if (this.Count != other.Count) { return false; }

            using (IEnumerator<T>
                thisIter = this.GetEnumerator(),
                otherIter = other.GetEnumerator())
            {
                var comparer = EqualityComparer<T>.Default;
                while (thisIter.MoveNext() & otherIter.MoveNext())
                {
                    if (!comparer.Equals(thisIter.Current, otherIter.Current))
                    { return false; }
                }
                return true;
            }
        }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否表示相同的值。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="LazyArray{T}"/> 的实例，
        /// 且所有元素均相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as LazyArray<T>);
        }

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            int hashCode = 879525830;
            var comparer = EqualityComparer<T>.Default;
            foreach (var item in this)
            { hashCode = hashCode * -1521134295 + comparer.GetHashCode(item); }
            return hashCode;
        }

        /// <summary>
        /// 返回 <see cref="LazyArray{T}"/> 的字符串表示形式。
        /// </summary>
        /// <returns><see cref="LazyArray{T}"/> 的字符串表示形式。</returns>
        public override string ToString()
        {
            var collectionStringBuilder = new StringBuilder("{ ");
            foreach (var item in this)
            { collectionStringBuilder.Append($"{item.ToString()}, "); }
            collectionStringBuilder.Append("}");
            return collectionStringBuilder.ToString();
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>用于循环访问集合的枚举数。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var lazyObject in this.lazyObjects)
            { yield return lazyObject.Value; }
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>一个可用于循环访问集合的对象。</returns>
        IEnumerator IEnumerable.GetEnumerator() =>
            ((IEnumerable<T>)this).GetEnumerator();

        /// <summary>
        /// 指示两 <see cref="LazyArray{T}"/> 对象是否相等。
        /// </summary>
        /// <param name="array1">第一个对象。</param>
        /// <param name="array2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="array1"/> 与 <paramref name="array2"/> 的对应元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(LazyArray<T> array1, LazyArray<T> array2) =>
            EqualityComparer<LazyArray<T>>.Default.Equals(array1, array2);

        /// <summary>
        /// 指示两 <see cref="LazyArray{T}"/> 对象是否不相等。
        /// </summary>
        /// <param name="array1">第一个对象。</param>
        /// <param name="array2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="array1"/> 与 <paramref name="array2"/> 的存在不相等的对应元素，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(LazyArray<T> array1, LazyArray<T> array2) =>
            !(array1 == array2);
    }
}
