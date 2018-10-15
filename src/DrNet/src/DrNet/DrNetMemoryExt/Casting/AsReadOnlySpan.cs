using System;
using System.Runtime.CompilerServices;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array) => new ReadOnlySpan<T>(array);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array, int start)
        {
            if (array == null)
            {
                if (start != 0)
                    throw new ArgumentOutOfRangeException(nameof(start));
                return default;
            }
            return new ReadOnlySpan<T>(array, start, array.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array, int start, int length) =>
            new ReadOnlySpan<T>(array, start, length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment) =>
            new ReadOnlySpan<T>(segment.Array, segment.Offset, segment.Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment, int start)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new ReadOnlySpan<T>(segment.Array, segment.Offset + start, segment.Count - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment, int start, int length)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (((uint)length) > segment.Count - start)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new ReadOnlySpan<T>(segment.Array, segment.Offset + start, length);
        }
    }
}
