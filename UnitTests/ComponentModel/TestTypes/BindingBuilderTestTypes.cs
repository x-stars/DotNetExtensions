using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XstarS.ComponentModel.TestTypes
{
    public interface IDisposableNotifyPropertyChanged : IDisposable, INotifyPropertyChanged { }

    public interface IDisposableBinding<T> : IDisposableNotifyPropertyChanged
    {
        T Value { get; set; }
    }

    internal interface IInternalDisposableBinding<T> : IDisposableBinding<T> { }

    public abstract class DisposableBindingBase<T> : IDisposableBinding<T>
    {
        protected DisposableBindingBase() { }

        protected DisposableBindingBase(T value, T bindableValue)
        {
            this.Value = value;
            this.BindableValue = bindableValue;
        }

        public abstract T Value { get; set; }

        [Bindable(true)]
        public abstract T BindableValue { get; set; }

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public void Dispose() => this.Value = default(T);

        public abstract TResult Convert<TResult>()
            where TResult : DisposableBindingBase<T>, IEnumerable<T>;
    }

    public abstract class BadDisposableBindingBase<T> : DisposableBindingBase<T>
    {
#pragma warning disable CS0067
        public sealed override event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
    }

    public class DisposableBinding<T> : DisposableBindingBase<T>
    {
        public DisposableBinding() { }

        public DisposableBinding(T value, T bindableValue)
            : base(value, bindableValue) { }

        public override T Value { get; set; }

        [Bindable(true)]
        public override T BindableValue { get; set; }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override TResult Convert<TResult>() => this.Value as TResult;

        public virtual void OnPropertyChanged(string propertyName) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class InternalDisposableBinding<T> : DisposableBinding<T> { }

    public sealed class SealedDisposableBinding<T> : DisposableBinding<T> { }

    public class NonOverridableDisposableBinding<T> : DisposableBinding<T>
    {
        public NonOverridableDisposableBinding(T value, T bindableValue)
            : base(value, bindableValue) { }

        public sealed override T Value { get; set; }

        [Bindable(true)]
        public sealed override T BindableValue { get; set; }
    }
}
