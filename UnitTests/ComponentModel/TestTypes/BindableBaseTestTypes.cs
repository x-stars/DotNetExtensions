namespace XstarS.ComponentModel.TestTypes
{
    public class BindableBox : BindableObject
    {
        public BindableBox() { }

        private int _Length;
        public int Length
        {
            get => this._Length;
            set
            {
                this.SetProperty(ref this._Length, value);
                this.OnPropertyChanged(nameof(this.Size));
            }
        }

        private int _Width;
        public int Width
        {
            get => this._Width;
            set
            {
                this.SetProperty(ref this._Width, value);
                this.OnPropertyChanged(nameof(this.Size));
            }
        }

        private int _Height;
        public int Height
        {
            get => this._Height;
            set
            {
                this.SetProperty(ref this._Height, value);
                this.OnPropertyChanged(nameof(this.Size));
            }
        }

        public int Size => this.Length * this.Width * this.Height;
    }

    public class BindablePersonName : BindableStorage
    {
        public BindablePersonName() { }

        public string FamilyName
        {
            get => this.GetProperty<string>();
            set
            {
                this.SetProperty(value);
                this.OnPropertyChanged(nameof(this.FullName));
            }
        }

        public string GivenName
        {
            get => this.GetProperty<string>();
            set
            {
                this.SetProperty(value);
                this.OnPropertyChanged(nameof(this.FullName));
            }
        }

        public bool IsEasternStyle
        {
            get => this.GetProperty<bool>();
            set
            {
                this.SetProperty(value);
                this.OnPropertyChanged(nameof(this.FullName));
            }
        }

        public string FullName => this.IsEasternStyle ?
            $"{this.FamilyName}{this.GivenName}" : $"{this.GivenName} {this.FamilyName}";
    }
}
