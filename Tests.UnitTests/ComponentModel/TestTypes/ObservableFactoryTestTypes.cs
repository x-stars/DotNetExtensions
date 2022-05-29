using System.ComponentModel;

namespace XNetEx.ComponentModel.TestTypes
{
    public interface IMutableRectangle
    {
        int Height { get; set; }

        int Width { get; set; }

        void Deconstruct(out int height, out int width);
    }

    internal interface IInternalMutableRectangle : IMutableRectangle { }

    public abstract class ObservableRectangleBase : IMutableRectangle, INotifyPropertyChanged
    {
        public ObservableRectangleBase() { }

        public ObservableRectangleBase(int height, int width)
        {
            this.Height = height;
            this.Width = width;
        }

        [RelatedProperties(nameof(Size))]
        public abstract int Height { get; set; }

        [RelatedProperties(nameof(Size))]
        public abstract int Width { get; set; }

        public int Size => this.Height * this.Width;

        public abstract event PropertyChangedEventHandler? PropertyChanged;

        public abstract void Deconstruct(out int height, out int width);
    }

    public sealed class SealedMutableRectangle : IMutableRectangle
    {
        public SealedMutableRectangle() { }

        public SealedMutableRectangle(int height, int width)
        {
            this.Height = height;
            this.Width = width;
        }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Size => this.Height * this.Width;

        public void Deconstruct(out int height, out int width)
        {
            height = this.Height;
            width = this.Width;
        }
    }

    public class ObservableRectangle : ObservableRectangleBase
    {
        public ObservableRectangle() { }

        public ObservableRectangle(int height, int width) : base(height, width) { }

        [RelatedProperties(nameof(Size))]
        public override int Height { get; set; }

        [RelatedProperties(nameof(Size))]
        public override int Width { get; set; }

        public override event PropertyChangedEventHandler? PropertyChanged;

        public override void Deconstruct(out int height, out int width)
        {
            height = this.Height;
            width = this.Width;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            this.PropertyChanged?.Invoke(this, e);
        }
    }

    public class BadObservableRectangle : ObservableRectangleBase
    {
        public BadObservableRectangle() { }

        public BadObservableRectangle(int height, int width) : base(height, width) { }

        [RelatedProperties(nameof(Size))]
        public override int Height { get; set; }

        [RelatedProperties(nameof(Size))]
        public override int Width { get; set; }

#pragma warning disable CS0067
        public sealed override event PropertyChangedEventHandler? PropertyChanged;
#pragma warning restore CS0067

        public override void Deconstruct(out int height, out int width)
        {
            height = this.Height;
            width = this.Width;
        }
    }

    public class NonObservableRectangle : ObservableRectangle
    {
        public NonObservableRectangle() { }

        public NonObservableRectangle(int height, int width) : base(height, width) { }

        public sealed override int Height { get; set; }

        public sealed override int Width { get; set; }
    }
}
