using System;
using System.ComponentModel;

namespace XstarS.ComponentModel.TestTypes
{
    public class BindingValue<T> : INotifyPropertyChanged
    {
        private T value;

        public T Value
        {
            get => this.value;
            set => this.SetProperty(ref this.value, value);
        }

#pragma warning disable CS0067
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067
    }

    public class ExplicitBindingValue<T> : INotifyPropertyChanged
    {
        private T value;
        private PropertyChangedEventHandler explicit_PropertyChanged;

        public T Value
        {
            get => this.value;
            set => this.SetProperty(ref this.value, value);
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => this.explicit_PropertyChanged += value;
            remove => this.explicit_PropertyChanged += value;
        }
    }

    public class DerivedBindingValue<T> : ExplicitBindingValue<T>
    {
        private T derivedValue;

        public T DerivedValue
        {
            get => this.derivedValue;
            set => this.SetProperty(ref this.derivedValue, value);
        }
    }
}
