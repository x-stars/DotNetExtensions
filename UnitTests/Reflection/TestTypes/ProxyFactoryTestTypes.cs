using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

namespace XstarS.Reflection.TestTypes
{
    internal static class TestHandlers
    {
        internal static readonly ProxyInvokeHandler WriteMethodAndInvokeBaseHandler =
            (instance, method, arguments, @delegate) =>
            {
                Console.WriteLine($"{instance.GetType()}@" +
                    $"{RuntimeHelpers.GetHashCode(instance)}::{method.ToString()}");
                return @delegate.Invoke(instance, arguments);
            };

        internal static readonly ProxyInvokeHandler WriteMethodAndReturnDefaultHandler =
            (instance, method, arguments, @delegate) =>
            {
                Console.WriteLine($"{instance.GetType()}@" +
                    $"{RuntimeHelpers.GetHashCode(instance)}::{method.ToString()}");
                return (method.ReturnType.IsValueType && (method.ReturnType != typeof(void))) ?
                    Activator.CreateInstance(method.ReturnType) : null;
            };
    }

    public class ProxyCollection<T> : Collection<T>
    {
        public ProxyCollection() { }

        public ProxyCollection(IList<T> list) : base(list) { }
    }

    public class ProxyCreator
    {
        public ProxyCreator() { }

        public virtual T Create<T>() => Activator.CreateInstance<T>();
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors")]
    public abstract class ProxyEqualityComparer<T> : EqualityComparer<T>
    {
        public ProxyEqualityComparer() { }
    }

    public interface IProxyList<T> : IList<T> { }
}
