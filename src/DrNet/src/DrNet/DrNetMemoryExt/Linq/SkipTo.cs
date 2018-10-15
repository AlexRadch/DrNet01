using System;
using System.Runtime.CompilerServices;
using UnsafeRef = System.Runtime.CompilerServices.Unsafe;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static Span<TSource> SkipToStartOrAll<TSource>(this Span<TSource> span, int start)
        {
            if (start > 0)
                return span.Slice(start);
            if (start == 0)
                return span;
            return span.Slice(span.Length, 0);
        }

        public static Span<TSource> SkipToEndOrAll<TSource>(this Span<TSource> span, int end)
        {
            if (end > 0)
                return span.Slice(0, end);
            return span.Slice(0, 0);
        }
    }
}