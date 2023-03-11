using System;

namespace XNetEx.Functions;

public readonly ref struct Placeholder
{
    [CLSCompliant(false)]
#pragma warning disable
    public static Placeholder __ => default;
#pragma warning restore
}
