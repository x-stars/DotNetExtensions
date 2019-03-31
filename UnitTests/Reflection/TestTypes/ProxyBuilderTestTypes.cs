using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace XstarS.Reflection.TestTypes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class NotifyPropertyChangedOnPropertyInvokeAttribute : OnMemberInvokeAttribute
    {
        public NotifyPropertyChangedOnPropertyInvokeAttribute() { }

        public override object Invoke(object target, MethodInfo method,
            MethodInvoker invoker, Type[] genericArguments, object[] arguments)
        {
            var result = base.Invoke(target, method, invoker, genericArguments, arguments);
            ((NotifyPropertyChanged)target).OnPropertyChanged(method.Name.Substring("set_".Length));
            return result;
        }

        public override bool FilterMethod(MethodInfo method) => method.IsSpecialName &&
            method.Name.StartsWith("set_") && (method.GetParameters().Length == 1);
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class WriteArugmentsAndResultOnMethodInvokeAttribute : OnMethodInvokeAttribute
    {
        public WriteArugmentsAndResultOnMethodInvokeAttribute() { }

        public override object Invoke(object target, MethodInfo method,
            MethodInvoker invoker, Type[] genericArguments, object[] arguments)
        {
            Console.WriteLine(string.Join(",", arguments));
            var reuslt = base.Invoke(target, method, invoker, genericArguments, arguments);
            Console.WriteLine(reuslt);
            return reuslt;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class WriteTypeRefMethodOnMemberInvokeAttribute : OnMemberInvokeAttribute
    {
        public WriteTypeRefMethodOnMemberInvokeAttribute() { }

        public override object Invoke(object target, MethodInfo method,
            MethodInvoker invoker, Type[] genericArguments, object[] arguments)
        {
            Console.WriteLine($"{target.GetType()}#{RuntimeHelpers.GetHashCode(target)}.[{method.ToString()}]");
            return base.Invoke(target, method, invoker, genericArguments, arguments);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
    public class WriteTypeRefMethodAndReturnsDefaultOnMemberInvokeAttribute : OnMemberInvokeAttribute
    {
        public WriteTypeRefMethodAndReturnsDefaultOnMemberInvokeAttribute() { }

        public override object Invoke(object target, MethodInfo method,
            MethodInvoker invoker, Type[] genericArguments, object[] arguments)
        {
            Console.WriteLine($"{target.GetType()}#{RuntimeHelpers.GetHashCode(target)}.[{method.ToString()}]");
            return (method.ReturnType.IsValueType && (method.ReturnType != typeof(void))) ?
                Activator.CreateInstance(method.ReturnType) : null;
        }
    }

    public abstract class NotifyPropertyChanged : INotifyPropertyChanged
    {
        protected NotifyPropertyChanged() { }

        public event PropertyChangedEventHandler PropertyChanged;

        internal protected virtual void OnPropertyChanged(string propertyName) =>
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    [NotifyPropertyChangedOnPropertyInvoke]
    public class ProxyBinding<T> : NotifyPropertyChanged
    {
        public ProxyBinding() { }

        public ProxyBinding(T value)
        {
            this.Value = value;
        }

        public virtual T Value { get; set; }

        [WriteArugmentsAndResultOnMethodInvoke]
        public virtual TU Function<TU>(TU value) => value;
    }

    [WriteTypeRefMethodOnMemberInvoke]
    public class ProxyCollection<T> : Collection<T>
    {
        public ProxyCollection() { }

        public ProxyCollection(IList<T> list) : base(list) { }
    }

    [WriteTypeRefMethodAndReturnsDefaultOnMemberInvoke]
    public abstract class ProxyEqualityComparer<T> : EqualityComparer<T>
    {
        public ProxyEqualityComparer() { }
    }

    [WriteTypeRefMethodAndReturnsDefaultOnMemberInvoke]
    public interface IProxyList<T> : IList<T> { }
}
