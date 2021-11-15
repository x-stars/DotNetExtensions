using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供非泛型集合集合的泛型包装。
    /// </summary>
    /// <typeparam name="T">集合中元素的类型。</typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class GenericCollection<T> : IList, IList<T>, IReadOnlyList<T>
    {
        /// <summary>
        /// 表示当前实例包装的列表。
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IList Collection;

        /// <summary>
        /// 将 <see cref="GenericCollection{T}"/> 类的新实例初始化为指定列表的包装。
        /// </summary>
        /// <param name="collection">要包装的列表。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="collection"/> 为 <see langword="null"/>。</exception>
        public GenericCollection(IList collection)
        {
            this.Collection = collection ??
                throw new ArgumentNullException(nameof(collection));
        }

        /// <inheritdoc/>
        public T this[int index]
        {
            get => (T)this.Collection[index];
            set => this.Collection[index] = value;
        }

        /// <inheritdoc/>
        object IList.this[int index]
        {
            get => this.Collection[index];
            set => this.Collection[index] = value;
        }

        /// <inheritdoc/>
        public int Count => this.Collection.Count;

        /// <inheritdoc/>
        bool ICollection<T>.IsReadOnly => this.Collection.IsReadOnly;

        /// <inheritdoc/>
        bool IList.IsFixedSize => this.Collection.IsFixedSize;

        /// <inheritdoc/>
        bool IList.IsReadOnly => this.Collection.IsReadOnly;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this.Collection.SyncRoot;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => this.Collection.IsSynchronized;

        /// <inheritdoc/>
        public void Add(T item) => this.Collection.Add(item);

        /// <inheritdoc/>
        public void Clear() => this.Collection.Clear();

        /// <inheritdoc/>
        public bool Contains(T item) => this.Collection.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(T[] array, int index) => this.Collection.CopyTo(array, index);

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>用于循环访问集合的 <see cref="GenericEnumerator{T}"/>。</returns>
        public GenericEnumerator<T> GetEnumerator() =>
            new GenericEnumerator<T>(this.Collection.GetEnumerator());

        /// <inheritdoc/>
        public int IndexOf(T item) => this.Collection.IndexOf(item);

        /// <inheritdoc/>
        public void Insert(int index, T item) => this.Collection.Insert(index, item);

        /// <inheritdoc/>
        public bool Remove(T item)
        {
            var contains = this.Contains(item);
            this.Collection.Remove(item);
            return contains;
        }

        /// <inheritdoc/>
        public void RemoveAt(int index) => this.Collection.RemoveAt(index);

        /// <inheritdoc/>
        int IList.Add(object value) => this.Collection.Add(value);

        /// <inheritdoc/>
        bool IList.Contains(object value) => this.Collection.Contains(value);

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index) => this.Collection.CopyTo(array, index);

        /// <inheritdoc/>
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.Collection.GetEnumerator();

        /// <inheritdoc/>
        int IList.IndexOf(object value) => this.Collection.IndexOf(value);

        /// <inheritdoc/>
        void IList.Insert(int index, object value) => this.Collection.Insert(index, value);

        /// <inheritdoc/>
        void IList.Remove(object value) => this.Collection.Remove(value);
    }
}
