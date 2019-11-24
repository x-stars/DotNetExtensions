using System;

namespace XstarS.Reflection.TestTypes
{
    public interface ICreator
    {
        T Create<T>();
    }

    public class Creator : ICreator
    {
        public Creator() { }

        public T Create<T>() => Activator.CreateInstance<T>();
    }

    public interface IFakeClonable : ICloneable
    {
        new object Clone();
    }

    public class FakeClonable : IFakeClonable
    {
        public FakeClonable() { }

        public object Clone() => this;

        object ICloneable.Clone() => this.MemberwiseClone();
    }
}
