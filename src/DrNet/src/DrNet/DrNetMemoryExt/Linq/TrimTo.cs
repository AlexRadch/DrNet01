using System;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static Span<TSource> TrimStartTo<TSource>(this Span<TSource> span, int start)
        {
            if (start > 0)
                return span.Slice(start);
            if (start == 0)
                return span;
            return span.Slice(span.Length, 0);
        }

        public static Span<TSource> TrimEndTo<TSource>(this Span<TSource> span, int end)
        {
            if (end > 0)
                return span.Slice(0, end);
            return span.Slice(0, 0);
        }

        public static Span<TSource> TrimStartEndTo<TSource>(this Span<TSource> span, int start, int end)
        {
            if (start > 0)
            {
                if (end <= start)
                    return span.Slice(start, 0);
                return span.Slice(start, end - start);
            }
            if (start == 0)
            {
                if (end > 0)
                    return span.Slice(0, end);
                return span.Slice(0, 0);
            }
            return span.Slice(span.Length, 0);
        }
    }
}