using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace XstarS.Collections
{
    /// <summary>
    /// 提供非泛型键值对集合的泛型包装。
    /// </summary>
    /// <typeparam name="TKey">字典中键的类型。</typeparam>
    /// <typeparam name="TValue">字典中值的类型。</typeparam>
    [Serializable]
    [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
    public sealed class GenericDictionary<TKey, TValue>
        : IDictionary, IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        /// <summary>
        /// 表示当前实例包装的字典。
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private readonly IDictionary Dictionary;

        /// <summary>
        /// 表示实例包装的字典的键的集合的泛型包装。
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly KeyCollection GenericKeys;

        /// <summary>
        /// 表示实例包装的字典的值的集合的泛型包装。
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ValueCollection GenericValues;

        /// <summary>
        /// 将 <see cref="GenericDictionary{TKey, TValue}"/> 类的新实例初始化为指定字典的包装。
        /// </summary>
        /// <param name="dictionary">要包装的字典。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        public GenericDictionary(IDictionary dictionary)
        {
            this.Dictionary = dictionary ??
                throw new ArgumentNullException(nameof(dictionary));
            this.GenericKeys = new KeyCollection(this.Dictionary);
            this.GenericValues = new ValueCollection(this.Dictionary);
        }

        /// <inheritdoc/>
        public TValue this[TKey key]
        {
            get => (TValue)this.Dictionary[key];
            set => this.Dictionary[key] = value;
        }

        /// <inheritdoc/>
        object IDictionary.this[object key]
        {
            get => this.Dictionary[key];
            set => this.Dictionary[key] = value;
        }

        /// <inheritdoc/>
        public int Count => this.Dictionary.Count;

        /// <summary>
        /// 获取包含字典的键的集合。
        /// </summary>
        /// <returns>包含字典的键的 <see cref="KeyCollection"/>。</returns>
        public KeyCollection Keys => this.GenericKeys;

        /// <summary>
        /// 获取包含字典的值的集合。
        /// </summary>
        /// <returns>包含字典的值的 <see cref="ValueCollection"/>。</returns>
        public ValueCollection Values => this.GenericValues;

        /// <inheritdoc/>
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Keys;

        /// <inheritdoc/>
        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.Values;

        /// <inheritdoc/>
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => this.Keys;

        /// <inheritdoc/>
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => this.Values;

        /// <inheritdoc/>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => this.Dictionary.IsReadOnly;

        /// <inheritdoc/>
        ICollection IDictionary.Keys => this.Dictionary.Keys;

        /// <inheritdoc/>
        ICollection IDictionary.Values => this.Dictionary.Values;

        /// <inheritdoc/>
        bool IDictionary.IsReadOnly => this.Dictionary.IsReadOnly;

        /// <inheritdoc/>
        bool IDictionary.IsFixedSize => this.Dictionary.IsFixedSize;

        /// <inheritdoc/>
        object ICollection.SyncRoot => this.Dictionary.SyncRoot;

        /// <inheritdoc/>
        bool ICollection.IsSynchronized => this.Dictionary.IsSynchronized;

        /// <inheritdoc/>
        public void Add(TKey key, TValue value) => this.Dictionary.Add(key, value);

        /// <inheritdoc/>
        public void Add(KeyValuePair<TKey, TValue> item) => this.Add(item.Key, item.Value);

        /// <inheritdoc/>
        public void Clear() => this.Dictionary.Clear();

        /// <inheritdoc/>
        public bool Contains(KeyValuePair<TKey, TValue> item) =>
            this.ContainsKey(item.Key) &&
            ValueCollection.Comparer.Equals(this[item.Key], item.Value);

        /// <inheritdoc/>
        public bool ContainsKey(TKey key) => this.Dictionary.Contains(key);

        /// <inheritdoc/>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            var entries = new DictionaryEntry[array.Length];
            this.Dictionary.CopyTo(entries, index);
            for (int offset = index; offset < array.Length; offset++)
            {
                array[offset] = new KeyValuePair<TKey, TValue>(
                    (TKey)entries[offset].Key, (TValue)entries[offset].Value);
            }
        }

        /// <summary>
        /// 返回一个循环访问集合的枚举器。
        /// </summary>
        /// <returns>用于循环访问集合的 <see cref="GenericDictionaryEnumerator{TKey, TValue}"/>。</returns>
        public GenericDictionaryEnumerator<TKey, TValue> GetEnumerator() =>
            new GenericDictionaryEnumerator<TKey, TValue>(this.Dictionary.GetEnumerator());

        /// <inheritdoc/>
        public bool Remove(TKey key)
        {
            var contains = this.ContainsKey(key);
            if (contains) { this.Dictionary.Remove(key); }
            return contains;
        }

        /// <inheritdoc/>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var contains = this.Contains(item);
            if (contains) { this.Dictionary.Remove(item.Key); }
            return contains;
        }

        /// <inheritdoc/>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.ContainsKey(key)) { value = this[key]; return true; }
            else { value = default(TValue); return false; }
        }

        /// <inheritdoc/>
        void IDictionary.Add(object key, object value) => this.Dictionary.Add(key, value);

        /// <inheritdoc/>
        bool IDictionary.Contains(object key) => this.Dictionary.Contains(key);

        /// <inheritdoc/>
        void ICollection.CopyTo(Array array, int index) => this.Dictionary.CopyTo(array, index);

        /// <inheritdoc/>
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() =>
            this.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => this.Dictionary.GetEnumerator();

        /// <inheritdoc/>
        IDictionaryEnumerator IDictionary.GetEnumerator() => this.Dictionary.GetEnumerator();

        /// <inheritdoc/>
        void IDictionary.Remove(object key) => this.Dictionary.Remove(key);

        /// <summary>
        /// 提供非泛型字典的键的集合的泛型包装。
        /// </summary>
        [Serializable]
        [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public sealed class KeyCollection : ICollection, ICollection<TKey>, IReadOnlyCollection<TKey>
        {
            /// <summary>
            /// 表示当前实例包装的字典。
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly IDictionary Dictionary;

            /// <summary>
            /// 表示当前实例包装的字典的键的集合。
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            private readonly ICollection DictionaryKeys;

            /// <summary>
            /// 将 <see cref="ValueCollection"/> 类的新实例初始化为指定字典的值的集合的包装。
            /// </summary>
            /// <param name="dictionary">要包装键的集合的字典。</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
            internal KeyCollection(IDictionary dictionary)
            {
                this.Dictionary = dictionary ??
                    throw new ArgumentNullException(nameof(dictionary));
                this.DictionaryKeys = dictionary.Keys;
            }

            /// <inheritdoc/>
            public int Count => this.Dictionary.Count;

            /// <inheritdoc/>
            bool ICollection<TKey>.IsReadOnly => true;

            /// <inheritdoc/>
            object ICollection.SyncRoot => this.DictionaryKeys.SyncRoot;

            /// <inheritdoc/>
            bool ICollection.IsSynchronized => this.DictionaryKeys.IsSynchronized;

            /// <inheritdoc/>
            public bool Contains(TKey item) => this.Dictionary.Contains(item);

            /// <inheritdoc/>
            public void CopyTo(TKey[] array, int index) => this.DictionaryKeys.CopyTo(array, index);

            /// <summary>
            /// 返回循环访问字典的键的集合的枚举数。
            /// </summary>
            /// <returns>用于循环访问字典的键的集合的 <see cref="GenericEnumerator{T}"/>。</returns>
            public GenericEnumerator<TKey> GetEnumerator() =>
                new GenericEnumerator<TKey>(this.DictionaryKeys.GetEnumerator());

            /// <inheritdoc/>
            void ICollection<TKey>.Add(TKey item) => throw new NotSupportedException();

            /// <inheritdoc/>
            void ICollection<TKey>.Clear() => throw new NotSupportedException();

            /// <inheritdoc/>
            bool ICollection<TKey>.Remove(TKey item) => throw new NotSupportedException();

            /// <inheritdoc/>
            bool ICollection<TKey>.Contains(TKey item) => this.Dictionary.Contains(item);

            /// <inheritdoc/>
            void ICollection.CopyTo(Array array, int index) => this.DictionaryKeys.CopyTo(array, index);

            /// <inheritdoc/>
            IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => this.GetEnumerator();

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator() => this.DictionaryKeys.GetEnumerator();
        }

        /// <summary>
        /// 提供非泛型字典的值的集合的泛型包装。
        /// </summary>
        [Serializable]
        [DebuggerDisplay("Count = {" + nameof(Count) + "}")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public sealed class ValueCollection : ICollection, ICollection<TValue>, IReadOnlyCollection<TValue>
        {
            /// <summary>
            /// 表示用于比较字典中值的比较器。
            /// </summary>
            internal static readonly EqualityComparer<TValue> Comparer =
                EqualityComparer<TValue>.Default;

            /// <summary>
            /// 表示当前实例包装的字典。
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private readonly IDictionary Dictionary;

            /// <summary>
            /// 表示当前实例包装的字典的值的集合。
            /// </summary>
            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            private readonly ICollection DictionaryValues;

            /// <summary>
            /// 将 <see cref="ValueCollection"/> 类的新实例初始化为指定字典的值的集合的包装。
            /// </summary>
            /// <param name="dictionary">要包装值的集合的字典。</param>
            /// <exception cref="ArgumentNullException">
            /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
            internal ValueCollection(IDictionary dictionary)
            {
                this.Dictionary = dictionary ??
                    throw new ArgumentNullException(nameof(dictionary));
                this.DictionaryValues = dictionary.Values;
            }

            /// <inheritdoc/>
            public int Count => this.Dictionary.Count;

            /// <inheritdoc/>
            bool ICollection<TValue>.IsReadOnly => true;

            /// <inheritdoc/>
            object ICollection.SyncRoot => this.DictionaryValues.SyncRoot;

            /// <inheritdoc/>
            bool ICollection.IsSynchronized => this.DictionaryValues.IsSynchronized;

            /// <inheritdoc/>
            public void CopyTo(TValue[] array, int index) => this.DictionaryValues.CopyTo(array, index);

            /// <summary>
            /// 返回循环访问字典的值的集合的枚举数。
            /// </summary>
            /// <returns>用于循环访问字典的值的集合的 <see cref="GenericEnumerator{T}"/>。</returns>
            public GenericEnumerator<TValue> GetEnumerator() =>
                new GenericEnumerator<TValue>(this.DictionaryValues.GetEnumerator());

            /// <inheritdoc/>
            void ICollection<TValue>.Add(TValue item) => throw new NotSupportedException();

            /// <inheritdoc/>
            void ICollection<TValue>.Clear() => throw new NotSupportedException();

            /// <inheritdoc/>
            bool ICollection<TValue>.Contains(TValue item)
            {
                var comparer = ValueCollection.Comparer;
                foreach (var value in this)
                {
                    if (comparer.Equals(value, item))
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <inheritdoc/>
            bool ICollection<TValue>.Remove(TValue item) => throw new NotSupportedException();

            /// <inheritdoc/>
            void ICollection.CopyTo(Array array, int index) => this.DictionaryValues.CopyTo(array, index);

            /// <inheritdoc/>
            IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => this.GetEnumerator();

            /// <inheritdoc/>
            IEnumerator IEnumerable.GetEnumerator() => this.DictionaryValues.GetEnumerator();
        }
    }
}
