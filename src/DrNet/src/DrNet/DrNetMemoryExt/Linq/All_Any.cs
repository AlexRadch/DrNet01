using System;

using DrNet.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static bool All<TSource>(this Span<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (; ; )
                {
                    if (!predicate(current))
                        return false;
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return true;
        }

        public static bool All<TSource>(this ReadOnlySpan<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (; ; )
                {
                    if (!predicate(current))
                        return false;
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return true;
        }

        public static bool Any<TSource>(this Span<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (; ; )
                {
                    if (predicate(current))
                        return true;
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return false;
        }

        public static bool Any<TSource>(this ReadOnlySpan<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (; ; )
                {
                    if (predicate(current))
                        return true;
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return false;
        }
    }
}
