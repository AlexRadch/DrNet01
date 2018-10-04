using System.Runtime.CompilerServices;

namespace DrNet.InteropServices
{
    public static unsafe class MarshalMemory
    {
        public static ref T GetReference<T>(UnsafeSpan<T> span) => ref Unsafe.AsRef<T>(span._pointer);
        public static ref readonly T GetReference<T>(UnsafeReadOnlySpan<T> span) => ref Unsafe.AsRef<T>(span._pointer);
    }
}
