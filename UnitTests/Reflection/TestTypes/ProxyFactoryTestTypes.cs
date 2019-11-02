using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace XstarS.Reflection.TestTypes
{
    internal static class TestInvokeHandlers
    {
        internal static readonly ProxyInvokeHandler ProxyBindingInvokeHandler =
            delegate (ProxyInvokeInfo info)
            {
                Console.WriteLine(string.Join(", ", info.Arguments));
                var result = info.BaseDelegate.Invoke(info.Instance, info.Arguments);
                if (info.Method.IsSpecialName && info.Method.Name.StartsWith("set_"))
                {
                    ((NotifyPropertyChanged)info.Instance).OnPropertyChanged(
                        info.Method.Name.Substring("set_".Length));
                }
                Console.WriteLine(result);
                return result;
            };

        internal static readonly ProxyInvokeHandler ProxyCollectionInvokeHandler =
            delegate (ProxyInvokeInfo info)
            {
                Console.WriteLine($"{info.Instance.GetType()}#" +
                    $"{RuntimeHelpers.GetHashCode(info.Instance)}.[{info.Method.ToString()}]");
                return info.BaseDelegate.Invoke(info.Instance, info.Arguments);
            };

        internal static readonly ProxyInvokeHandler ProxyEqualityComparerInvokeHandler =
            delegate (ProxyInvokeInfo info)
            {
                Console.WriteLine($"{info.Instance.GetType()}#" +
                    $"{RuntimeHelpers.GetHashCode(info.Instance)}.[{info.Method.ToString()}]");
                return (info.Method.ReturnType.IsValueType && (info.Method.ReturnType != typeof(void))) ?
                    Activator.CreateInstance(info.Method.ReturnType) : null;
            };

        internal static readonly ProxyInvokeHandler IProxyListInvokeHandler =
            delegate (ProxyInvokeInfo info)
            {
                Console.WriteLine($"{info.Instance.GetType()}#" +
                    $"{RuntimeHelpers.GetHashCode(info.Instance)}.[{info.Method.ToString()}]");
                return (info.Method.ReturnType.IsValueType && (info.Method.ReturnType != typeof(void))) ?
                    Activator.CreateInstance(info.Method.ReturnType) : null;
            };
    }

    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        protected NotifyPropertyChanged() { }

        public event PropertyChangedEventHandler PropertyChanged;

        protected internal virtual void OnPropertyChanged(string propertyName) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class ProxyBinding<T> : NotifyPropertyChanged
    {
        public ProxyBinding() { }

        public ProxyBinding(T value)
        {
            this.Value = value;
        }

        public virtual T Value { get; set; }

        public virtual TU Function<TU>(TU value) => value;
    }

    public class ProxyCollection<T> : Collection<T>
    {
        public ProxyCollection() { }

        public ProxyCollection(IList<T> list) : base(list) { }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Microsoft.Design", "CA1012:AbstractTypesShouldNotHaveConstructors")]
    public abstract class ProxyEqualityComparer<T> : EqualityComparer<T>
    {
        public ProxyEqualityComparer() { }
    }

    public interface IProxyList<T> : IList<T> { }
}
