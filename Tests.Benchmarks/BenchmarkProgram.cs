﻿using BenchmarkDotNet.Running;

namespace XstarS
{
    internal static class BenchmarkProgram
    {
        internal static void Main(string[] args)
        {
            var assembly = typeof(BenchmarkProgram).Assembly;
            var summaries = BenchmarkRunner.Run(assembly);
        }
    }
}
