using System;
using System.Runtime.InteropServices;

namespace DrNet.Internal.Unsafe
{
    [StructLayout(LayoutKind.Explicit)]
    public unsafe readonly ref struct SpanStruct<T>
    {
        [FieldOffset(0)]
        public readonly Span<T> _span;

        [FieldOffset(0)]
        public readonly UnsafeSpan<T> _unsafeSpan;

        public SpanStruct(void* pointer, int length)
        {
            _span = default;
            _unsafeSpan = new UnsafeSpan<T>(pointer, length);
        }
    }
}
