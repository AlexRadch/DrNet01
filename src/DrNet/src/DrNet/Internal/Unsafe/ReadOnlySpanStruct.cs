using System;
using System.Runtime.InteropServices;

namespace DrNet.Internal.Unsafe
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe readonly ref struct ReadOnlySpanStruct<T>
    {
        [FieldOffset(0)]
        public readonly ReadOnlySpan<T> _span;

        [FieldOffset(0)]
        public readonly UnsafeReadOnlySpan<T> _unsafeSpan;

        public ReadOnlySpanStruct(void* pointer, int length)
        {
            _span = default;
            _unsafeSpan = new UnsafeReadOnlySpan<T>(pointer, length);
        }
    }
}
