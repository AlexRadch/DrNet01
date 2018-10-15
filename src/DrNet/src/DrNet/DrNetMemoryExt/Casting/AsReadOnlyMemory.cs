using System;
using System.Runtime.CompilerServices;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array) => new ReadOnlyMemory<T>(array);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array, int start)
        {
            if (array == null)
            {
                if (start != 0)
                    throw new ArgumentOutOfRangeException(nameof(start));
                return new ReadOnlyMemory<T>(array);
            }
            return new ReadOnlyMemory<T>(array, start, array.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array, int start, int length) =>
            new ReadOnlyMemory<T>(array, start, length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this ArraySegment<T> segment) =>
            new ReadOnlyMemory<T>(segment.Array, segment.Offset, segment.Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this ArraySegment<T> segment, int start)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new ReadOnlyMemory<T>(segment.Array, segment.Offset + start, segment.Count - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this ArraySegment<T> segment, int start, int length)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (((uint)length) > segment.Count - start)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new ReadOnlyMemory<T>(segment.Array, segment.Offset + start, length);
        }
    }
}
