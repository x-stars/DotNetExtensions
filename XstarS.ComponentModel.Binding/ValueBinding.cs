using System;
using System.ComponentModel;

namespace XstarS.ComponentModel
{
    /// <summary>
    /// 表示两个不同类型的值之间的单向或双向的数据绑定。
    /// </summary>
    /// <typeparam name="TSource">数据绑定源值的类型。</typeparam>
    /// <typeparam name="TTarget">数据绑定目标值的类型。</typeparam>
    public class ValueBinding<TSource, TTarget> : IDisposable
    {
        /// <summary>
        /// <see cref="ValueBinding{TSource, TTarget}.Source"/> 的值。
        /// </summary>
        private readonly IBindingValue<TSource> InternalSource;

        /// <summary>
        /// <see cref="ValueBinding{TSource, TTarget}.Target"/> 的值。
        /// </summary>
        private readonly IBindingValue<TTarget> InternalTarget;

        /// <summary>
        /// <see cref="ValueBinding{TSource, TTarget}.Direction"/> 的值。
        /// </summary>
        private readonly BindingDirection InternalDirection;

        /// <summary>
        /// <see cref="ValueBinding{TSource, TTarget}.Convert"/> 的值。
        /// </summary>
        private readonly Converter<TSource, TTarget> InternalConvert;

        /// <summary>
        /// <see cref="ValueBinding{TSource, TTarget}.ConvertBack"/> 的值。
        /// </summary>
        private readonly Converter<TTarget, TSource> InternalConvertBack;

        /// <summary>
        /// 使用数据绑定源值、数据绑定目标值、绑定方向和类型转换方法初始化
        /// <see cref="ValueBinding{TSource, TTarget}"/> 类的新实例。
        /// </summary>
        /// <param name="source">数据绑定源值的 <see cref="IBindingValue{T}"/> 对象。</param>
        /// <param name="target">数据绑定目标值的 <see cref="IBindingValue{T}"/> 对象。</param>
        /// <param name="direction">数据绑定方向，可为从源到目标的单向绑定，或是双向绑定。</param>
        /// <param name="convert">从源值到目标值的类型转换方法。</param>
        /// <param name="convertBack">从目标值到源值的类型转换方法。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="direction"/> 的值不合法。</exception>
        public ValueBinding(
            IBindingValue<TSource> source, IBindingValue<TTarget> target, BindingDirection direction,
            Converter<TSource, TTarget> convert, Converter<TTarget, TSource> convertBack)
        {
            this.InternalSource = source ?? throw new ArgumentNullException(nameof(source));
            this.InternalTarget = target ?? throw new ArgumentNullException(nameof(target));
            this.InternalDirection = direction;

            switch (this.Direction)
            {
                case BindingDirection.OneWay:
                    this.InternalConvert = convert ??
                        throw new ArgumentNullException(nameof(convert));
                    this.InternalConvertBack = convertBack;
                    this.Source.ValueChanged += this.Source_ValueChanged;
                    break;
                case BindingDirection.TwoWay:
                    this.InternalConvert = convert ??
                        throw new ArgumentNullException(nameof(convert));
                    this.InternalConvertBack = convertBack ??
                        throw new ArgumentNullException(nameof(convertBack));
                    this.Source.ValueChanged += this.Source_ValueChanged;
                    this.Target.ValueChanged += this.Target_ValueChanged;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction));
            }
            this.Target.Value = this.Convert(this.Source.Value);
        }

        /// <summary>
        /// 指示当前实例占用的资源是否已经被释放。
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <summary>
        /// 当前实例的 <see cref="IDisposable"/> 检查对象。
        /// </summary>
        /// <exception cref="ObjectDisposedException">当前实例已经被释放。</exception>
        protected ValueBinding<TSource, TTarget> Disposable =>
            this.IsDisposed ? throw new ObjectDisposedException(this.GetType().ToString()) : this;

        /// <summary>
        /// 数据绑定源值的 <see cref="IBindingValue{T}"/> 对象。
        /// </summary>
        public IBindingValue<TSource> Source => this.Disposable.InternalSource;

        /// <summary>
        /// 数据绑定目标值的 <see cref="IBindingValue{T}"/> 对象。
        /// </summary>
        public IBindingValue<TTarget> Target => this.Disposable.InternalTarget;

        /// <summary>
        /// 数据绑定源值到数据绑定目标值的绑定方向。
        /// </summary>
        public BindingDirection Direction => this.Disposable.InternalDirection;

        /// <summary>
        /// 从源值到目标值的类型转换方法的 <see cref="Converter{TInput, TOutput}"/> 委托。
        /// </summary>
        protected Converter<TSource, TTarget> Convert => this.Disposable.InternalConvert;

        /// <summary>
        /// 从目标值到源值的类型转换方法的 <see cref="Converter{TInput, TOutput}"/> 委托。
        /// </summary>
        protected Converter<TTarget, TSource> ConvertBack => this.Disposable.InternalConvertBack;

        /// <summary>
        /// 释放此实例占用的资源。
        /// 将取消当前实例对 <see cref="IBindingValue{T}.ValueChanged"/> 事件的订阅。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放此实例占用的非托管资源。并根据指示释放托管资源。
        /// 将取消当前实例对 <see cref="IBindingValue{T}.ValueChanged"/> 事件的订阅。
        /// </summary>
        /// <param name="disposing">指示是否释放托管资源。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    this.Source.ValueChanged -= this.Source_ValueChanged;
                    this.Target.ValueChanged -= this.Target_ValueChanged;
                }

                IsDisposed = true;
            }
        }

        /// <summary>
        /// <see cref="ValueBinding{TSource, TTarget}.Source"/> 的
        /// <see cref="IBindingValue{T}.ValueChanged"/> 事件的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void Source_ValueChanged(object sender, EventArgs e)
        {
            try { this.Target.Value = this.Convert(this.Source.Value); }
            catch (Exception) { this.Target.Value = default; }
        }

        /// <summary>
        /// <see cref="ValueBinding{TSource, TTarget}.Target"/> 的
        /// <see cref="IBindingValue{T}.ValueChanged"/> 事件的事件处理。
        /// </summary>
        /// <param name="sender">事件源。</param>
        /// <param name="e">提供事件数据的对象。</param>
        private void Target_ValueChanged(object sender, EventArgs e)
        {
            try { this.Source.Value = this.ConvertBack(this.Target.Value); }
            catch (Exception) { this.Source.Value = default; }
        }
    }

    /// <summary>
    /// 表示两个相同类型的值之间的单向或双向的数据绑定。
    /// </summary>
    /// <typeparam name="T">数据绑定的值的类型。</typeparam>
    public class ValueBinding<T> : ValueBinding<T, T>
    {
        /// <summary>
        /// 使用数据绑定源值、数据绑定目标值和绑定方向初始化 <see cref="ValueBinding{TValue}"/> 类的新实例。
        /// </summary>
        /// <param name="source">数据绑定源值的 <see cref="IBindingValue{T}"/> 对象。</param>
        /// <param name="target">数据绑定目标值的 <see cref="IBindingValue{T}"/> 对象。</param>
        /// <param name="direction">数据绑定方向，可为从源到目标的单向绑定，或是双向绑定。</param>
        /// <exception cref="ArgumentNullException">存在为 <see langword="null"/> 的参数。</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="direction"/> 的值不合法。</exception>
        public ValueBinding(IBindingValue<T> source, IBindingValue<T> target, BindingDirection direction)
            : base(source, target, direction, ValueBinding<T>.DefaultConvert, ValueBinding<T>.DefaultConvert) { }

        /// <summary>
        /// 表示一个默认的类型转换方法，直接将输入值返回。
        /// </summary>
        /// <param name="value">要进行转换的值。</param>
        /// <returns>转换后得到的值。</returns>
        private static T DefaultConvert(T value) => value;
    }
}
