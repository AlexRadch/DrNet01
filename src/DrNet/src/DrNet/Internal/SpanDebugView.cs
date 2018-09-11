using System;
using System.Diagnostics;

namespace DrNet.Internal
{
    public class SpanDebugView<T>
    {
        public SpanDebugView(Span<T> span)
        {
            Items = span.ToArray();
        }

        public SpanDebugView(ReadOnlySpan<T> span)
        {
            Items = span.ToArray();
        }

        public SpanDebugView(UnsafeSpan<T> span)
        {
            Items = span.AsSpan().ToArray();
        }

        public SpanDebugView(UnsafeReadOnlySpan<T> span)
        {
            Items = span.AsSpan().ToArray();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        public T[] Items { get; }
    }
}
