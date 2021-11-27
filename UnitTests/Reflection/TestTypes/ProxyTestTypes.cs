﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace XstarS.Reflection.TestTypes
{
    public static class ProxyTestHandlers
    {
        public static readonly MethodInvokeHandler WriteMethodAndInvokeBaseHandler =
            (instance, method, @delegate, arguments) =>
            {
                Console.WriteLine($"{instance.GetType()} " +
                    $"#{RuntimeHelpers.GetHashCode(instance)} :: {method}");
                return @delegate.Invoke(instance, arguments);
            };

        public static readonly MethodInvokeHandler WriteMethodAndReturnDefaultHandler =
            (instance, method, @delegate, arguments) =>
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

    public class Int32Increaser
    {
        public Int32Increaser() { }

        public virtual void Increase(ref int value) => value++;
    }

    public class ByRefValueBox<T>
    {
        private T _Value;

        public ByRefValueBox() { }

        public ByRefValueBox(T value)
        {
            this._Value = value;
        }

        public virtual T Value { get => this._Value; set => this._Value = value; }

        public virtual ref T RefValue => ref this._Value;
    }
}
