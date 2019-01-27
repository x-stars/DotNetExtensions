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

        public void Dispose()
        {
            this.Value = default;
            this.BindableValue = default;
        }

        public abstract void Load<TCollection>(TCollection collection)
            where TCollection : List<T>, ICloneable;
    }

    public abstract class IndexedDisposableBindingBase<T> : DisposableBindingBase<T>
    {
        public abstract T this[int index] { get; set; }
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

        public override void Load<TCollection>(TCollection collection)
        {
            this.Value = collection[0];
            this.BindableValue = collection[1];
        }

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

    internal class CloneableList<T> : List<T>, ICloneable
    {
        public CloneableList() :base() { }

        public CloneableList(int capacity) : base(capacity) { }

        public CloneableList(IEnumerable<T> collection) : base(collection) { }

        public object Clone() => new CloneableList<T>(this);
    }
}
