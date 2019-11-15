﻿using System.ComponentModel;

namespace XstarS.ComponentModel.TestTypes
{
    public interface IMutableRectangle
    {
        int Height { get; set; }

        int Width { get; set; }

        void Deconstruct(out int height, out int width);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    internal interface IInternalMutableRectangle : IMutableRectangle { }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors")]
    public abstract class BindableRectangleBase : IMutableRectangle, INotifyPropertyChanged
    {
        public BindableRectangleBase() { }

        public BindableRectangleBase(int height, int width)
        {
            this.Height = height;
            this.Width = width;
        }

        [RelatedProperties(nameof(Size))]
        public abstract int Height { get; set; }

        [RelatedProperties(nameof(Size))]
        public abstract int Width { get; set; }

        public int Size => this.Height * this.Width;

        public abstract event PropertyChangedEventHandler PropertyChanged;

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

    public class BindableRectangle : BindableRectangleBase
    {
        public BindableRectangle() { }

        public BindableRectangle(int height, int width) : base(height, width) { }

        [RelatedProperties(nameof(Size))]
        public override int Height { get; set; }

        [RelatedProperties(nameof(Size))]
        public override int Width { get; set; }

        public override event PropertyChangedEventHandler PropertyChanged;

        public override void Deconstruct(out int height, out int width)
        {
            height = this.Height;
            width = this.Width;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class BadBindableRectangle : BindableRectangleBase
    {
        public BadBindableRectangle() { }

        public BadBindableRectangle(int height, int width) : base(height, width) { }

        [RelatedProperties(nameof(Size))]
        public override int Height { get; set; }

        [RelatedProperties(nameof(Size))]
        public override int Width { get; set; }

#pragma warning disable CS0067
        public sealed override event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0067

        public override void Deconstruct(out int height, out int width)
        {
            height = this.Height;
            width = this.Width;
        }
    }

    public class NonBindableRectangle : BindableRectangle
    {
        public NonBindableRectangle() { }

        public NonBindableRectangle(int height, int width) : base(height, width) { }

        public sealed override int Height { get; set; }

        public sealed override int Width { get; set; }
    }
}
