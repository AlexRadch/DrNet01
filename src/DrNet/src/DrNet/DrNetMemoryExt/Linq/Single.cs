using System;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static TSource Single<TSource>(this Span<TSource> span)
        {
            if (span.Length == 1)
                return span[0];
            throw new InvalidOperationException();
        }

        public static TSource Single<TSource>(this ReadOnlySpan<TSource> span)
        {
            if (span.Length == 1)
                return span[0];
            throw new InvalidOperationException();
        }

        public static TSource SingleOrDefault<TSource>(this Span<TSource> span)
        {
            switch (span.Length)
            {
                case 0:
                    return default;
                case 1:
                    return span[0];
            }
            throw new InvalidOperationException();
        }

        public static TSource SingleOrDefault<TSource>(this ReadOnlySpan<TSource> span)
        {
            switch (span.Length)
            {
                case 0:
                    return default;
                case 1:
                    return span[0];
            }
            throw new InvalidOperationException();
        }
    }
}
