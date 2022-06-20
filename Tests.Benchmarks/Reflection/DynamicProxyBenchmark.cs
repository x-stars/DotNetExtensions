using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using BenchmarkDotNet.Attributes;

namespace XNetEx.Reflection;

public class DynamicProxyBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int InvokeCount;

    private readonly MethodInfo ItemSetMethod =
        typeof(IList<object>).GetProperty("Item")!.SetMethod!;

    private readonly IList<object?> TargetObject;
    private readonly IList<object?> DispatchProxy;
    private readonly IList<object?> FastDispatchProxy;
    private readonly IList<object?> DirectProxy;
    private readonly IList<object?> WrapProxy;

    public DynamicProxyBenchmark()
    {
        this.TargetObject = new Collection<object?>() { null };
        this.DispatchProxy = DispatchProxyServices.CreateProxy(this.TargetObject,
            (instance, method, arguments) => method.Invoke(instance, arguments));
        this.FastDispatchProxy = DispatchProxyServices.CreateProxy(this.TargetObject,
            (instance, method, arguments) => method.InvokeFast(instance, arguments));
        this.DirectProxy = DynamicProxyServices.CreateDirectProxy<Collection<object?>>();
        this.WrapProxy = DynamicProxyServices.CreateWrapProxy(this.TargetObject);
        this.DirectProxy.Add(null);
    }

    [Benchmark(Baseline = true)]
    public void DirectInvoke()
    {
        var list = this.TargetObject;
        var count = this.InvokeCount;
        for (int index = 0; index < count; index++)
        {
            list[0] = null;
        }
    }

    [Benchmark]
    public void ReflectionInvoke()
    {
        var list = this.TargetObject;
        var setMethod = this.ItemSetMethod;
        var count = this.InvokeCount;
        for (int index = 0; index < count; index++)
        {
            setMethod.Invoke(list, new[] { (object)0, null });
        }
    }

    [Benchmark]
    public void DispatchProxyInvoke()
    {
        var list = this.DispatchProxy;
        var count = this.InvokeCount;
        for (int index = 0; index < count; index++)
        {
            list[0] = null;
        }
    }

    [Benchmark]
    public void FastDispatchProxyInvoke()
    {
        var list = this.FastDispatchProxy;
        var count = this.InvokeCount;
        for (int index = 0; index < count; index++)
        {
            list[0] = null;
        }
    }

    [Benchmark]
    public void DirectProxyInvoke()
    {
        var list = this.DirectProxy;
        var count = this.InvokeCount;
        for (int index = 0; index < count; index++)
        {
            list[0] = null;
        }
    }

    [Benchmark]
    public void WrapProxyInvoke()
    {
        var list = this.WrapProxy;
        var count = this.InvokeCount;
        for (int index = 0; index < count; index++)
        {
            list[0] = null;
        }
    }
}
