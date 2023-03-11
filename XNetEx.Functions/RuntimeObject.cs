#if NETCOREAPP3_0_OR_GREATER
using System.Runtime.CompilerServices;
#endif

namespace XNetEx.Runtime.CompilerServices;

internal static class RuntimeObject
{
    private sealed class MethodBridge
    {
        internal static unsafe object MemberwiseClone(object instance)
        {
#if NETCOREAPP3_0_OR_GREATER
            return Unsafe.As<MethodBridge>(instance).MemberwiseClone();
#else
            static object Identity(object instance) => instance;
            var bridge = ((delegate*<object, MethodBridge>)
                          (delegate*<object, object>)&Identity)(instance);
            return bridge.MemberwiseClone();
#endif
        }
    }

    public static object MemberwiseClone(object instance)
    {
        return MethodBridge.MemberwiseClone(instance);
    }

    public static unsafe ref nint TypeHandleOf(object instance)
    {
#if NETCOREAPP3_0_OR_GREATER
        return ref Unsafe.Add(ref Unsafe.As<StrongBox<nint>>(instance).Value, -1);
#else
        static object Identity(object instance) => instance;
        return ref ((delegate*<object, ref nint>)
                    (delegate*<object, object>)&Identity)(instance);
#endif
    }
}
