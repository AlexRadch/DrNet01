using System;

using DrNet.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static int Count<TSource>(this Span<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int result = 0;
            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (; ; )
                {
                    if (predicate(current))
                        result++;
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return result;
        }

        public static int Count<TSource>(this ReadOnlySpan<TSource> span, Func<TSource, bool> predicate)
        {
            if (predicate == null)
                throw new ArgumentNullException(nameof(predicate));

            int result = 0;
            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (; ; )
                {
                    if (predicate(current))
                        result++;
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return result;
        }
    }
}
