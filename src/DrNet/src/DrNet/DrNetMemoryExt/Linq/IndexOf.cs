using System;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static int IndexOf<TSource>(this Span<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return DrNetSpanHelpers.IndexOf(in DrNetMarshal.GetReference(span), span.Length, predicate);
        }

        public static int IndexOf<TSource>(this ReadOnlySpan<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return DrNetSpanHelpers.IndexOf(in DrNetMarshal.GetReference(span), span.Length, predicate);
        }

        public static int IndexOf<TSource>(this Span<TSource> span, Func<TSource, int, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return DrNetSpanHelpers.IndexOf(in DrNetMarshal.GetReference(span), span.Length, predicate);
        }

        public static int IndexOf<TSource>(this ReadOnlySpan<TSource> span, Func<TSource, int, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            return DrNetSpanHelpers.IndexOf(in DrNetMarshal.GetReference(span), span.Length, predicate);
        }
    }
}
