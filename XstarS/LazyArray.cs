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
    /// <typeparam name="T">延迟初始化的对象的类型。</typeparam>
    public class LazyArray<T> : IReadOnlyList<T>, IDisposable
    {
        /// <summary>
        /// 延迟初始化对象 <see cref="Lazy{T}"/> 的数组。
        /// </summary>
        private readonly Lazy<T>[] lazyObjects;

        /// <summary>
        /// 初始化 <see cref="LazyArray{T}"/> 类的新实例。
        /// 出现迟缓初始化时，将使用指定的初始化函数。
        /// </summary>
        /// <param name="valueFactories">
        /// 需要生成延迟初始化值时调用的的委托的列表。</param>
        public LazyArray(params Func<T>[] valueFactories)
        {
            this.lazyObjects = new Lazy<T>[valueFactories.Length];
            for (int i = 0; i < valueFactories.Length; i++)
            {
                this.lazyObjects[i] = new Lazy<T>(valueFactories[i]);
            }
        }

        /// <summary>
        /// 获取 <see cref="LazyArray{T}"/> 实例的指定位置的延迟初始化值。
        /// </summary>
        /// <param name="index">获取延迟初始化值的位置。</param>
        public T this[int index] => this.lazyObjects[index].Value;

        /// <summary>
        /// 获取 <see cref="LazyArray{T}"/> 中的元素数。
        /// </summary>
        public int Length => this.lazyObjects.Length;

        /// <summary>
        /// 获取 <see cref="IReadOnlyCollection{T}"/> 中的元素数。
        /// </summary>
        int IReadOnlyCollection<T>.Count => this.Length;

        /// <summary>
        /// 指示 <see cref="LazyArray{T}"/> 实例的指定位置的值是否已经被创建。
        /// </summary>
        /// <param name="index">要判断是否被创建的值的位置。</param>
        /// <returns>若位于 <paramref name="index"/> 位置的值已经被创建，
        /// 则为 <see langword="true"/>，否则·为 <see langword="false"/>。</returns>
        public bool IsValueCreated(int index) => this.lazyObjects[index].IsValueCreated;

        /// <summary>
        /// 释放指定位置的延迟初始化值。
        /// </summary>
        /// <param name="index">要释放延迟初始化值的位置。</param>
        public void Dispose(int index) =>
            this.lazyObjects[index] = new Lazy<T>(() =>
            throw new ObjectDisposedException($"{nameof(this.lazyObjects)}[{index}]"));

        /// <summary>
        /// 释放所有的延迟初始化值。
        /// </summary>
        public void Dispose()
        {
            for (int i = 0; i < this.Length; i++)
            {
                this.Dispose(i);
            }
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>用于循环访问集合的枚举数。</returns>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (var lazyObject in this.lazyObjects)
            {
                yield return lazyObject.Value;
            }
        }

        /// <summary>
        /// 返回循环访问集合的枚举数。
        /// </summary>
        /// <returns>一个可用于循环访问集合的对象。</returns>
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
