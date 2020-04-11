using System;
using System.Runtime.CompilerServices;

namespace XstarS.ComponentModel.TestTypes
{
    public class ObservablePersonName : ObservableDataObject
    {
        public ObservablePersonName() { }

        public string FamilyName
        {
            get => this.GetProperty<string>();
            set
            {
                this.SetProperty(value);
                this.NotifyPropertyChanged(nameof(this.FullName));
            }
        }

        public string GivenName
        {
            get => this.GetProperty<string>();
            set
            {
                this.SetProperty(value);
                this.NotifyPropertyChanged(nameof(this.FullName));
            }
        }

        public bool IsEasternStyle
        {
            get => this.GetProperty<bool>();
            set
            {
                this.SetProperty(value);
                this.NotifyPropertyChanged(nameof(this.FullName));
            }
        }

        public string FullName => this.IsEasternStyle ?
            $"{this.FamilyName}{this.GivenName}" : $"{this.GivenName} {this.FamilyName}";
    }

    public class ObservableBox : ObservableValidDataObject
    {
        public ObservableBox() { }

        public int Length
        {
            get => this.GetProperty<int>();
            set
            {
                this.SetProperty(value);
                this.NotifyPropertyChanged(nameof(this.Size));
            }
        }

        public int Width
        {
            get => this.GetProperty<int>();
            set
            {
                this.SetProperty(value);
                this.NotifyPropertyChanged(nameof(this.Size));
            }
        }

        public int Height
        {
            get => this.GetProperty<int>();
            set
            {
                this.SetProperty(value);
                this.NotifyPropertyChanged(nameof(this.Size));
            }
        }

        protected override void ValidateProperty(
            [CallerMemberName] string propertyName = null)
        {
            base.ValidateProperty(propertyName);
            var errors = (this.GetProperty<int>(propertyName) < 0) ?
                new[] { new ArgumentOutOfRangeException().Message } : null;
            this.SetErrors(errors, propertyName);
        }

        public int Size => this.Length * this.Width * this.Height;
    }
}
