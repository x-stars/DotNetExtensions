using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS.Reflection.TestTypes
{
    public static class ProxyFactoryTestHandlers
    {
        public static readonly MethodInvokeHandler WriteMethodAndInvokeBaseHandler =
            (instance, method, arguments, @delegate) =>
            {
                Console.WriteLine($"{instance.GetType()} " +
                    $"#{RuntimeHelpers.GetHashCode(instance)} :: {method}");
                return @delegate.Invoke(instance, arguments);
            };

        public static readonly MethodInvokeHandler WriteMethodAndReturnDefaultHandler =
            (instance, method, arguments, @delegate) =>
            {
                Console.WriteLine($"{instance.GetType()} " +
                    $"#{RuntimeHelpers.GetHashCode(instance)} :: {method}");
                var returnT = method.ReturnType;
                return (returnT.IsByRef || returnT.IsPointer) ? IntPtr.Zero :
                    (returnT.IsValueType && (returnT != typeof(void))) ?
                    Activator.CreateInstance(returnT) : null;
            };
    }
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

    public interface IListCreator<T>
    {
        TList Create<TList>() where TList : IList<T>, new();
    }

    public class ListCreator<T> : IListCreator<T>
    {
        public ListCreator() { }

        public virtual TList Create<TList>()
            where TList : IList<T>, new()
        {
            return Activator.CreateInstance<TList>();
        }
    }
}
