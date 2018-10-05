using System.Runtime.CompilerServices;

namespace DrNet.UnSafe
{
    public static unsafe class DrNetMarshal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetReference<T>(UnsafeSpan<T> span) => ref Unsafe.AsRef<T>(span._pointer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T GetReference<T>(UnsafeReadOnlySpan<T> span) => 
            ref Unsafe.AsRef<T>(span._pointer);
    }
}
