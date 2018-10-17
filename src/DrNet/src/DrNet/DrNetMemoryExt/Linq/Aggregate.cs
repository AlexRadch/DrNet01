using DrNet.Unsafe;
using System;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        public static TSource Aggregate<TSource>(this Span<TSource> span, Func<TSource, TSource, TSource> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            int count = span.Length;
            if (count <= 0)
                throw new InvalidOperationException();

            ref readonly TSource current = ref span[0];
            TSource result = current;
            while (--count > 0)
            {
                UnsafeIn.Add(current, 1);
                result = func(result, current);
            }
            return result;
        }

        public static TSource Aggregate<TSource>(this ReadOnlySpan<TSource> span, Func<TSource, TSource, TSource> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            int count = span.Length;
            if (count <= 0)
                throw new InvalidOperationException();

            ref readonly TSource current = ref span[0];
            TSource result = current;
            while (--count > 0)
            {
                UnsafeIn.Add(current, 1);
                result = func(result, current);
            }
            return result;
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this Span<TSource> span, TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            TAccumulate result = seed;
            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (;;)
                {
                    result = func(result, current);
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return result;
        }

        public static TAccumulate Aggregate<TSource, TAccumulate>(this ReadOnlySpan<TSource> span, TAccumulate seed,
            Func<TAccumulate, TSource, TAccumulate> func)
        {
            if (func == null)
                throw new ArgumentNullException(nameof(func));

            if (func == null)
                throw new ArgumentNullException(nameof(func));

            TAccumulate result = seed;
            int count = span.Length;
            if (count > 0)
            {
                ref readonly TSource current = ref DrNetMarshal.GetReference(span);
                for (;;)
                {
                    result = func(result, current);
                    if (--count <= 0)
                        break;
                    UnsafeIn.Add(current, 1);
                }
            }
            return result;
        }
    }
}
