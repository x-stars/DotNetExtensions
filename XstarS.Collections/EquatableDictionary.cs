using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace XstarS.Collections.Generic
{
    /// <summary>
    /// 可进行相等比较的键和值的集合。
    /// </summary>
    /// <typeparam name="TKey">字典中的键的类型。</typeparam>
    /// <typeparam name="TValue">字典中的值的类型。</typeparam>
    public class EquatableDictionary<TKey, TValue> : Dictionary<TKey, TValue>,
        IEquatable<EquatableDictionary<TKey, TValue>>
    {
        /// <summary>
        /// 初始化 <see cref="EquatableDictionary{TKey, TValue}"/> 类的新实例，
        /// 该实例为空，具有默认的初始容量并为键类型使用默认的相等比较器。
        /// </summary>
        public EquatableDictionary() : base() { }

        /// <summary>
        /// 初始化 <see cref="EquatableDictionary{TKey, TValue}"/> 类的新实例，
        /// 该实例为空，具有指定的初始容量并为键类型使用默认的相等比较器。
        /// </summary>
        /// <param name="capacity">
        /// <see cref="EquatableDictionary{TKey, TValue}"/> 可包含的初始元素数。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> 小于 0。</exception>
        public EquatableDictionary(int capacity) :
            base(capacity) { }

        /// <summary>
        /// 初始化 <see cref="EquatableDictionary{TKey, TValue}"/> 类的新实例，
        /// 该实例为空，具有默认的初始容量并使用指定的 <see cref="IEqualityComparer{T}"/>。
        /// </summary>
        /// <param name="comparer"></param>
        public EquatableDictionary(IEqualityComparer<TKey> comparer) :
            base(comparer) { }

        /// <summary>
        /// 初始化 <see cref="EquatableDictionary{TKey, TValue}"/> 类的新实例，
        /// 该实例包含从指定的 <see cref="IDictionary{TKey, TValue}"/> 复制的元素并为键类型使用默认的相等比较器。
        /// </summary>
        /// <param name="dictionary"><see cref="IDictionary{TKey, TValue}"/>，
        /// 它的元素被复制到新 <see cref="EquatableDictionary{TKey, TValue}"/>。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="dictionary"/> 包含一个或多个重复键。</exception>
        public EquatableDictionary(IDictionary<TKey, TValue> dictionary) :
            base(dictionary) { }

        /// <summary>
        /// 初始化 <see cref="EquatableDictionary{TKey, TValue}"/> 类的新实例，
        /// 该实例为空，具有指定的初始容量并使用指定的 <see cref="IEqualityComparer{T}"/>。
        /// </summary>
        /// <param name="capacity">
        /// <see cref="EquatableDictionary{TKey, TValue}"/> 可包含的初始元素数。</param>
        /// <param name="comparer">
        /// 比较键时要使用的 <see cref="IEqualityComparer{T}"/> 实现，
        /// 或者为 <see langword="null"/>，
        /// 以便为键类型使用默认的 <see cref="IEqualityComparer{T}"/>。</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="capacity"/> 小于 0。</exception>
        public EquatableDictionary(int capacity, IEqualityComparer<TKey> comparer) :
            base(capacity, comparer) { }

        /// <summary>
        /// 初始化 <see cref="EquatableDictionary{TKey, TValue}"/> 类的新实例，
        /// 该实例包含从指定的 <see cref="IDictionary{TKey, TValue}"/> 中复制的元素
        /// 并使用指定的 <see cref="IEqualityComparer{T}"/>。
        /// </summary>
        /// <param name="dictionary"><see cref="IDictionary{TKey, TValue}"/>，
        /// 它的元素被复制到新 <see cref="EquatableDictionary{TKey, TValue}"/>。</param>
        /// <param name="comparer">
        /// 比较键时要使用的 <see cref="IEqualityComparer{T}"/> 实现，
        /// 或者为 <see langword="null"/>，
        /// 以便为键类型使用默认的 <see cref="IEqualityComparer{T}"/>。</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dictionary"/> 为 <see langword="null"/>。</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="dictionary"/> 包含一个或多个重复键。</exception>
        public EquatableDictionary(
            IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) :
            base(dictionary, comparer) { }

        /// <summary>
        /// 用序列化数据初始化 <see cref="EquatableDictionary{TKey, TValue}"/> 类的新实例。
        /// </summary>
        /// <param name="info">
        /// 一个 <see cref="SerializationInfo"/> 对象
        /// 包含序列化 <see cref="EquatableDictionary{TKey, TValue}"/> 所需的信息。</param>
        /// <param name="context">
        /// 一个 <see cref="StreamingContext"/> 结构
        /// 包含与 <see cref="EquatableDictionary{TKey, TValue}"/> 关联的序列化流的源和目标。</param>
        protected EquatableDictionary(SerializationInfo info, StreamingContext context) :
            base(info, context) { }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="EquatableDictionary{TKey, TValue}"/> 对象是否表示相同的值。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="EquatableDictionary{TKey, TValue}"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的所有元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(EquatableDictionary<TKey, TValue> other)
        {
            if (other is null) { return false; }
            if (this.Count != other.Count) { return false; }

            using (IEnumerator<KeyValuePair<TKey, TValue>>
                thisIter = this.GetEnumerator(),
                otherIter = other.GetEnumerator())
            {
                var keyComparer = EqualityComparer<TKey>.Default;
                var valueComparer = EqualityComparer<TValue>.Default;
                while (thisIter.MoveNext() & otherIter.MoveNext())
                {
                    if (!keyComparer.Equals(thisIter.Current.Key, otherIter.Current.Key) ||
                        !valueComparer.Equals(thisIter.Current.Value, otherIter.Current.Value))
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
        /// 如果 <paramref name="obj"/> 是 <see cref="EquatableDictionary{TKey, TValue}"/> 的实例，
        /// 且所有元素均相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as EquatableDictionary<TKey, TValue>);
        }

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            int hashCode = 1520531305;
            var keyComparer = EqualityComparer<TKey>.Default;
            var valueComparer = EqualityComparer<TValue>.Default;
            foreach (var item in this)
            {
                hashCode = hashCode * -1521134295 + keyComparer.GetHashCode(item.Key);
                hashCode = hashCode * -1521134295 + valueComparer.GetHashCode(item.Value);
            }
            return hashCode;
        }

        /// <summary>
        /// 返回 <see cref="EquatableDictionary{TKey, TValue}"/> 的字符串表示形式。
        /// </summary>
        /// <returns><see cref="EquatableDictionary{TKey, TValue}"/> 的字符串表示形式。</returns>
        public override string ToString()
        {
            var collectionStringBuilder = new StringBuilder("{ ");
            foreach (var item in this)
            { collectionStringBuilder.Append($"{item.ToString()}, "); }
            collectionStringBuilder.Append("}");
            return collectionStringBuilder.ToString();
        }

        /// <summary>
        /// 指示两 <see cref="EquatableDictionary{TKey, TValue}"/> 对象是否相等。
        /// </summary>
        /// <param name="dictionary1">第一个对象。</param>
        /// <param name="dictionary2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="dictionary1"/> 与 <paramref name="dictionary2"/> 的对应元素均相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(
            EquatableDictionary<TKey, TValue> dictionary1, EquatableDictionary<TKey, TValue> dictionary2) =>
            EqualityComparer<EquatableDictionary<TKey, TValue>>.Default.Equals(dictionary1, dictionary2);

        /// <summary>
        /// 指示两 <see cref="EquatableDictionary{TKey, TValue}"/> 对象是否不相等。
        /// </summary>
        /// <param name="dictionary1">第一个对象。</param>
        /// <param name="dictionary2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="dictionary1"/> 与 <paramref name="dictionary2"/> 的存在不相等的对应元素，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(
            EquatableDictionary<TKey, TValue> dictionary1, EquatableDictionary<TKey, TValue> dictionary2) =>
            !(dictionary1 == dictionary2);
    }
}
