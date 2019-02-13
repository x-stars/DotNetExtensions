using System;

namespace XstarS
{
    /// <summary>
    /// 提供读写详情自动输出的类。当变量的值发生读写时均会输出信息到控制台。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Verbose<T>
    {
        /// <summary>
        /// 变量的值。
        /// </summary>
        private T value;
        /// <summary>
        /// 变量的名称。
        /// </summary>
        private string name;

        /// <summary>
        /// 使用默认值初始化 <see cref="Verbose{T}"/> 类的实例。
        /// </summary>
        public Verbose() { }

        /// <summary>
        /// 使用变量的值和名称初始化 <see cref="Verbose{T}"/> 类的实例。
        /// </summary>
        /// <param name="value">变量的值。</param>
        /// <param name="name">变量的名称。</param>
        public Verbose(T value, string name = null)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// 变量的值。
        /// 当读取此属性时会执行 <see cref="Verbose{T}.OnValueReading(T, string)"/>，
        /// 当写入此属性时会执行 <see cref="Verbose{T}.OnValueWriting(T, T, string)"/>。
        /// </summary>
        public T Value
        {
            get { this.OnValueReading(this.value, this.Name); return this.value; }
            set { this.OnValueWriting(this.value, value, this.Name); this.value = value; }
        }

        /// <summary>
        /// 变量的名称。若未设定，则返回 <see cref="Verbose{T}.GetDefaultName"/> 的返回值。
        /// </summary>
        public string Name
        {
            get => this.name ?? this.GetDefaultName();
            set => this.name = value;
        }

        /// <summary>
        /// 获取未设置变量名称时使用的默认名称。
        /// </summary>
        /// <returns>未设置变量名称时使用的默认名称。</returns>
        protected virtual string GetDefaultName() => "(NoName)";

        /// <summary>
        /// 当读取当前实例 <see cref="Verbose{T}.Value"/> 时发生。
        /// </summary>
        /// <param name="value"><see cref="Verbose{T}.Value"/> 的值。</param>
        /// <param name="name"><see cref="Verbose{T}.Name"/> 的值。</param>
        protected virtual void OnValueReading(T value, string name) =>
            Console.WriteLine($"{typeof(T)} {name}: {value}");

        /// <summary>
        /// 当写入当前实例 <see cref="Verbose{T}.Value"/> 时发生。
        /// </summary>
        /// <param name="oldValue"><see cref="Verbose{T}.Value"/> 的旧值。</param>
        /// <param name="newValue"><see cref="Verbose{T}.Value"/> 的新值。</param>
        /// <param name="name"><see cref="Verbose{T}.Name"/> 的值。</param>
        protected virtual void OnValueWriting(T oldValue, T newValue, string name) =>
            Console.WriteLine($"{typeof(T)} {name}: {oldValue} -> {newValue}");

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
