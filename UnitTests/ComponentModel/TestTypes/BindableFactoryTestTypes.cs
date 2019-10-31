using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace XstarS.ComponentModel.TestTypes
{
    public interface IDisposableNotifyPropertyChanged : IDisposable, INotifyPropertyChanged { }

    public interface IDisposableBindingValue<T> : IDisposableNotifyPropertyChanged
    {
        T Value { get; set; }
    }

    internal interface IInternalDisposableBindingValue<T> : IDisposableBindingValue<T> { }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
    public abstract class DisposableBindingValueBase<T> : IDisposableBindingValue<T>
    {
        public DisposableBindingValueBase() { }

        public DisposableBindingValueBase(T value)
        {
            this.Value = value;
        }

        public abstract T Value { get; set; }

        public abstract event PropertyChangedEventHandler PropertyChanged;

        public void Dispose()
        {
            this.Value = default;
        }

        public abstract void Load<TCollection>(TCollection collection)
            where TCollection : List<T>, ICloneable;
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors")]
    public abstract class IndexedDisposableBindingValueBase<T> : DisposableBindingValueBase<T>
    {
        public IndexedDisposableBindingValueBase() { }

        public virtual T this[int index]
        {
            get => this.Value;
            set => this.Value = value;
        }
    }

    public abstract class BadDisposableBindingBase<T> : DisposableBindingValueBase<T>
    {
#pragma warning disable CS0067
        public sealed override event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
    }

    public class DisposableBindingValue<T> : DisposableBindingValueBase<T>
    {
        public DisposableBindingValue() { }

        public DisposableBindingValue(T value) : base(value) { }

        public override T Value { get; set; }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override void Load<TCollection>(TCollection collection)
        {
            this.Value = collection[0];
        }

        public virtual void OnPropertyChanged(string propertyName) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    internal class InternalDisposableBindingValue<T> : DisposableBindingValue<T> { }

    public sealed class SealedDisposableBindingValue<T> : DisposableBindingValue<T> { }

    public class DisposableSealedBindingValue<T> : DisposableBindingValue<T>
    {
        public DisposableSealedBindingValue() { }

        public DisposableSealedBindingValue(T value) : base(value) { }

        public sealed override T Value { get; set; }
    }

    internal class CloneableList<T> : List<T>, ICloneable
    {
        public CloneableList() : base() { }

        public CloneableList(int capacity) : base(capacity) { }

        public CloneableList(IEnumerable<T> collection) : base(collection) { }

        public object Clone() => new CloneableList<T>(this);
    }
}
