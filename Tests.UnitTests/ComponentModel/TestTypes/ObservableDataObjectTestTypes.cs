using System.ComponentModel.DataAnnotations;

namespace XNetEx.ComponentModel.TestTypes
{
    public class ObservablePersonName : ObservableDataObject
    {
        public ObservablePersonName() { }

        protected override void InitializeRelatedProperties()
        {
            base.InitializeRelatedProperties();
            this.SetRelatedProperties(nameof(this.FamilyName), nameof(this.FullName));
            this.SetRelatedProperties(nameof(this.GivenName), nameof(this.FullName));
            this.SetRelatedProperties(nameof(this.IsEasternStyle), nameof(this.FullName));
        }

        public string? FamilyName
        {
            get => this.GetProperty<string>();
            set => this.SetProperty(value);
        }

        public string? GivenName
        {
            get => this.GetProperty<string>();
            set => this.SetProperty(value);
        }

        public bool IsEasternStyle
        {
            get => this.GetProperty<bool>();
            set => this.SetProperty(value);
        }

        public string FullName => this.IsEasternStyle ?
            $"{this.FamilyName}{this.GivenName}" : $"{this.GivenName} {this.FamilyName}";
    }

    public class ObservableBox : ObservableValidDataObject
    {
        public ObservableBox() { }

        protected override void InitializeRelatedProperties()
        {
            base.InitializeRelatedProperties();
            this.SetRelatedProperties(nameof(this.Length), nameof(this.Size));
            this.SetRelatedProperties(nameof(this.Width), nameof(this.Size));
            this.SetRelatedProperties(nameof(this.Height), nameof(this.Size));
        }

        [Range(0, int.MaxValue)]
        public int Length
        {
            get => this.GetProperty<int>();
            set => this.SetProperty(value);
        }

        [Range(0, int.MaxValue)]
        public int Width
        {
            get => this.GetProperty<int>();
            set => this.SetProperty(value);
        }

        [Range(0, int.MaxValue)]
        public int Height
        {
            get => this.GetProperty<int>();
            set => this.SetProperty(value);
        }

        public int Size => this.Length * this.Width * this.Height;
    }
}
