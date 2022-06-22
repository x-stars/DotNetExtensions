using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;

namespace XNetEx;

public class ArrayEnumerateBenchmark
{
    [CLSCompliant(false)]
    [Params(1, 10, 100, 1000)]
    public int ArrayLength;

#nullable disable
    private object[] TargetArray;
#nullable restore

    [GlobalSetup]
    public void Initialize()
    {
        var length = this.ArrayLength;
        this.TargetArray = new object[length];
    }

    [Benchmark(Baseline = true)]
    public void ForIndexLoop()
    {
        var array = this.TargetArray;
        for (int index = 0; index < array.Length; index++)
        {
            var item = array[index];
        }
    }

    [Benchmark]
    public void ForIndexVarLoop()
    {
        var array = this.TargetArray;
        var length = array.Length;
        for (int index = 0; index < length; index++)
        {
            var item = array[index];
        }
    }

    [Benchmark]
    public void ForeachItemLoop()
    {
        var array = this.TargetArray;
        foreach (var item in array)
        {
            // Do nothing...
        }
    }

    [Benchmark]
    public void ForeachEnumItemLoop()
    {
        var array = this.TargetArray;
        var arrayEnum = (IEnumerable<object?>)array;
        foreach (var item in arrayEnum)
        {
            // Do nothing...
        }
    }

    [Benchmark]
    public void RangeForeachIndexLoop()
    {
        var array = this.TargetArray;
        foreach (var index in ..array.Length)
        {
            var item = array[index];
        }
    }
}
