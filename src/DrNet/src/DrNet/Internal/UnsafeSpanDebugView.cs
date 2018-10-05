using System;
using System.Diagnostics;

using DrNet.UnSafe;

namespace DrNet.Internal
{
    public class UnsafeSpanDebugView<T>
    {
        public UnsafeSpanDebugView(Span<T> span)
        {
            Items = span.ToArray();
        }

        public UnsafeSpanDebugView(ReadOnlySpan<T> span)
        {
            Items = span.ToArray();
        }

        public UnsafeSpanDebugView(UnsafeSpan<T> span)
        {
            Items = span.AsSpan().ToArray();
        }

        public UnsafeSpanDebugView(UnsafeReadOnlySpan<T> span)
        {
            Items = span.AsSpan().ToArray();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items { get; }
    }
}
