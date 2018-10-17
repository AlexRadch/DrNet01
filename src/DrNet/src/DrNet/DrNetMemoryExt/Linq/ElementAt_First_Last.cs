using System;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static TSource ElementAtOrDefault<TSource>(this Span<TSource> span, int index)
        {
            if (index >= 0 && index < span.Length)
                return span[index];
            return default;
        }

        public static TSource ElementAtOrDefault<TSource>(this ReadOnlySpan<TSource> span, int index)
        {
            if (index >= 0 && index < span.Length)
                return span[index];
            return default;
        }

        public static TSource FirstOrDefault<TSource>(this Span<TSource> span)
        {
            if (span.Length > 0)
                return span[0];
            return default;
        }

        public static TSource FirstOrDefault<TSource>(this ReadOnlySpan<TSource> span)
        {
            if (span.Length > 0)
                return span[0];
            return default;
        }

        public static TSource Last<TSource>(this Span<TSource> span) => span[span.Length];

        public static TSource Last<TSource>(this ReadOnlySpan<TSource> span) => span[span.Length];

        public static TSource LastOrDefault<TSource>(this Span<TSource> span)
        {
            int len = span.Length;
            if (len > 0)
                return span[len];
            return default;
        }

        public static TSource LastOrDefault<TSource>(this ReadOnlySpan<TSource> span)
        {
            int len = span.Length;
            if (len > 0)
                return span[len];
            return default;
        }
    }
}
