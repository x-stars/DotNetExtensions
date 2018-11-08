using System;
using System.Collections.Generic;

namespace XstarS
{
    /// <summary>
    /// 提供读写详情自动输出的类。当变量的值发生读写时均会输出信息到控制台。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Verbose<T> : IEquatable<Verbose<T>>
    {
        /// <summary>
        /// 未设置变量名称时使用的默认名称。
        /// </summary>
        protected static string DefaultName = "(NoName)";

        /// <summary>
        /// 变量的值。
        /// </summary>
        private T value;
        /// <summary>
        /// 变量的名称。
        /// </summary>
        private string name;

        /// <summary>
        /// 使用默认值初始化 <see cref="Verbose{T}"/> 类的实例，
        /// 并生成默认的 <see cref="Verbose{T}.OnValueRead"/> 和 <see cref="Verbose{T}.OnValueWrite"/>。
        /// </summary>
        public Verbose()
        {
            this.OnValueRead = (value, name) => Console.WriteLine(
                $"{typeof(T)} {name}: {value}");
            this.OnValueWrite = (oldValue, newValue, name) => Console.WriteLine(
                $"{typeof(T)} {name}: {oldValue} -> {newValue}");
        }

        /// <summary>
        /// 使用变量的值和名称初始化 <see cref="Verbose{T}"/> 类的实例，
        /// 并生成默认的 <see cref="Verbose{T}.OnValueRead"/> 和 <see cref="Verbose{T}.OnValueWrite"/>。
        /// </summary>
        /// <param name="value">变量的值。</param>
        /// <param name="name">变量的名称。</param>
        public Verbose(T value, string name = null) : this()
        {
            this.Value = value;
            this.Name = name;
        }

        /// <summary>
        /// 变量的值。
        /// 当读取此属性时会执行 <see cref="Verbose{T}.OnValueRead"/>，
        /// 当写入此属性时会执行 <see cref="Verbose{T}.OnValueWrite"/>。
        /// </summary>
        public T Value
        {
            get { this.OnValueRead?.Invoke(this.value, this.Name); return this.value; }
            set { this.OnValueWrite?.Invoke(this.value, value, this.Name); this.value = value; }
        }

        /// <summary>
        /// 变量的名称。若未设定，则返回 "(NoName)"。
        /// </summary>
        public string Name
        {
            get => this.name ?? Verbose<T>.DefaultName;
            set => this.name = value;
        }

        /// <summary>
        /// 当读取当前实例的 <see cref="Verbose{T}.Value"/> 时发生。
        /// 其中输入参数依次为变量的值和名称。
        /// 默认将变量的类型、名称和值依次输出到控制台。
        /// </summary>
        public Action<T, string> OnValueRead { get; set; }

        /// <summary>
        /// 当写入当前实例 <see cref="Verbose{T}.Value"/> 时发生。
        /// 其中输入参数依次为变量的旧值、新值和名称。
        /// 默认将变量的类型、名称、旧值和新值依次输出到控制台。
        /// </summary>
        public Action<T, T, string> OnValueWrite { get; set; }

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的对象是否表示相同的值。
        /// </summary>
        /// <param name="obj">要与此实例比较的对象。</param>
        /// <returns>
        /// 如果 <paramref name="obj"/> 是 <see cref="Verbose{T}"/> 的实例，
        /// 且 <see cref="Verbose{T}.Value"/> 和 <see cref="Verbose{T}.Name"/> 都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public override bool Equals(object obj) => this.Equals(obj as Verbose<T>);

        /// <summary>
        /// 返回一个值，该值指示此实例和指定的 <see cref="Verbose{T}"/> 对象是否表示相同的值。
        /// </summary>
        /// <param name="other">要与此实例比较的 <see cref="Verbose{T}"/> 对象。</param>
        /// <returns>
        /// 如果此实例和 <paramref name="other"/> 的
        /// <see cref="Verbose{T}.Value"/> 和 <see cref="Verbose{T}.Name"/> 都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public bool Equals(Verbose<T> other) =>
            !(other is null) &&
            EqualityComparer<T>.Default.Equals(this.Value, other.Value) &&
            this.Name == other.Name;

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            var hashCode = -1670801664;
            hashCode = hashCode * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.Value);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.Name);
            return hashCode;
        }

        /// <summary>
        /// 返回表示当前 <see cref="Verbose{T}"/> 实例的字符串。
        /// </summary>
        /// <returns>当前 <see cref="Verbose{T}"/> 实例的名称和值的等效字符串表达形式。</returns>
        public override string ToString() => $"{this.Name}: {this.Value}";

        /// <summary>
        /// 指示两 <see cref="Verbose{T}"/> 对象是否相等。
        /// </summary>
        /// <param name="verbose1">第一个对象。</param>
        /// <param name="verbose2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="verbose1"/> 和 <paramref name="verbose2"/> 的
        /// <see cref="Verbose{T}.Value"/> 和 <see cref="Verbose{T}.Name"/> 都相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator ==(Verbose<T> verbose1, Verbose<T> verbose2) =>
            EqualityComparer<Verbose<T>>.Default.Equals(verbose1, verbose2);

        /// <summary>
        /// 指示两 <see cref="Verbose{T}"/> 对象是否不等。
        /// </summary>
        /// <param name="verbose1">第一个对象。</param>
        /// <param name="verbose2">第二个对象。</param>
        /// <returns>
        /// 如果 <paramref name="verbose1"/> 和 <paramref name="verbose2"/> 的
        /// <see cref="Verbose{T}.Value"/> 和 <see cref="Verbose{T}.Name"/> 不完全相等，
        /// 则为 <see langword="true"/>；否则为 <see langword="false"/>。
        /// </returns>
        public static bool operator !=(Verbose<T> verbose1, Verbose<T> verbose2) =>
            !(verbose1 == verbose2);

        /// <summary>
        /// 创建一个新的 <see cref="Verbose{T}"/> 对象，并将其初始化为指定的值。
        /// </summary>
        /// <param name="value">一个 <typeparamref name="T"/> 类型的对象。</param>
        /// <returns>使用 <paramref name="value"/> 初始化的 <see cref="Verbose{T}"/> 对象。</returns>
        public static implicit operator Verbose<T>(T value) => new Verbose<T>(value);

        /// <summary>
        /// 返回指定 <see cref="Verbose{T}"/> 对象的值。
        /// </summary>
        /// <param name="verbose">一个 <see cref="Verbose{T}"/> 对象。</param>
        /// <returns><paramref name="verbose"/> 的值。</returns>
        public static implicit operator T(Verbose<T> verbose) => verbose.Value;
    }
}
