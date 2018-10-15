using System;
using System.Runtime.CompilerServices;
using UnsafeRef = System.Runtime.CompilerServices.Unsafe;
using System.Runtime.InteropServices;

namespace DrNet.Unsafe
{
    public static unsafe class DrNetMarshal
    {
        #region Span ReadOnlySpan

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<TTo> UnsafeAs<TFrom, TTo>(Span<TFrom> span) => 
            CreateSpan(ref UnsafeRef.As<TFrom, TTo>(ref GetReference(span)), span.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<TTo> UnsafeAs<TFrom, TTo>(ReadOnlySpan<TFrom> span) => 
            CreateReadOnlySpan(in UnsafeIn.As<TFrom, TTo>(in GetReference(span)), span.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<TTo> UnsafeCast<TFrom, TTo>(Span<TFrom> span)
        {
            long longLength = (long)span.Length * UnsafeRef.SizeOf<TFrom>() / UnsafeRef.SizeOf<TTo>();
            int length = checked((int)longLength);
            return CreateSpan(ref UnsafeRef.As<TFrom, TTo>(ref GetReference(span)), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<TTo> UnsafeCast<TFrom, TTo>(ReadOnlySpan<TFrom> span)
        {
            long longLength = (long)span.Length * UnsafeRef.SizeOf<TFrom>() / UnsafeRef.SizeOf<TTo>();
            int length = checked((int)longLength);
            return CreateReadOnlySpan(in UnsafeIn.As<TFrom, TTo>(in GetReference(span)), length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> UnsafeCastBytes<TFrom>(Span<TFrom> span) =>
            CreateSpan(ref UnsafeRef.As<TFrom, byte>(ref GetReference(span)), 
                checked(span.Length * UnsafeRef.SizeOf<TFrom>()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> UnsafeCastBytes<TFrom>(ReadOnlySpan<TFrom> span) =>
            CreateReadOnlySpan(in UnsafeIn.As<TFrom, byte>(in GetReference(span)),
                checked(span.Length * UnsafeRef.SizeOf<TFrom>()));

        /// <summary>
        /// Create a new span over a portion of a regular managed object. This can be useful if part of a managed object
        /// represents a "fixed array." This is dangerous because the <paramref name="length"/> is not checked.
        /// </summary>
        /// <param name="reference">A reference to data.</param>
        /// <param name="length">The number of <typeparamref name="T"/> elements the memory contains.</param>
        /// <returns>The lifetime of the returned span will not be validated for safety by span-aware languages.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> CreateSpan<T>(ref T reference, int length) => 
            MemoryMarshal.CreateSpan(ref reference, length);

        /// <summary>
        /// Create a new read-only span over a portion of a regular managed object. This can be useful if part of
        /// a managed object represents a "fixed array." This is dangerous because the <paramref name="length"/>
        /// is not checked.
        /// </summary>
        /// <param name="reference">A reference to data.</param>
        /// <param name="length">The number of <typeparamref name="T"/> elements the memory contains.</param>
        /// <returns>The lifetime of the returned span will not be validated for safety by span-aware languages.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> CreateReadOnlySpan<T>(in T reference, int length) =>
            MemoryMarshal.CreateReadOnlySpan(ref UnsafeRef.AsRef(in reference), length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetReference<T>(Span<T> span) => ref MemoryMarshal.GetReference(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T GetReference<T>(ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference(span);

        #endregion

        #region UnsafeSpan UnsafeReadOnlySpan

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeSpan<TTo> UnsafeAs<TFrom, TTo>(UnsafeSpan<TFrom> span) => 
            new UnsafeSpan<TTo>(span._pointer, span._length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeReadOnlySpan<TTo> UnsafeAs<TFrom, TTo>(UnsafeReadOnlySpan<TFrom> span) => 
            new UnsafeReadOnlySpan<TTo>(span._pointer, span._length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeSpan<TTo> UnsafeCast<TFrom, TTo>(UnsafeSpan<TFrom> span)
        {
            long longLength = (long)span.Length * UnsafeRef.SizeOf<TFrom>() / UnsafeRef.SizeOf<TTo>();
            int length = checked((int)longLength);
            return new UnsafeSpan<TTo>(span._pointer, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeReadOnlySpan<TTo> UnsafeCast<TFrom, TTo>(UnsafeReadOnlySpan<TFrom> span)
        {
            long longLength = (long)span.Length * UnsafeRef.SizeOf<TFrom>() / UnsafeRef.SizeOf<TTo>();
            int length = checked((int)longLength);
            return new UnsafeReadOnlySpan<TTo>(span._pointer, length);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeSpan<byte> UnsafeCastBytes<TFrom>(UnsafeSpan<TFrom> span) =>
            new UnsafeSpan<byte>(span._pointer, checked(span.Length * UnsafeRef.SizeOf<TFrom>()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UnsafeReadOnlySpan<byte> UnsafeCastBytes<TFrom>(UnsafeReadOnlySpan<TFrom> span) =>
            new UnsafeReadOnlySpan<byte>(span._pointer, checked(span.Length * UnsafeRef.SizeOf<TFrom>()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetReference<T>(UnsafeSpan<T> span) => ref UnsafeRef.AsRef<T>(span._pointer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T GetReference<T>(UnsafeReadOnlySpan<T> span) => 
            ref UnsafeIn.AsRef<T>(span._pointer);

        #endregion

        #region IsTypeComparableAsBytes

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTypeComparableAsBytes<T>()
        {
            return typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte) ||
                typeof(T) == typeof(char) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(int) || typeof(T) == typeof(uint) ||
                typeof(T) == typeof(long) || typeof(T) == typeof(ulong);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTypeComparableAsBytes<T>(out int size)
        {
            if (typeof(T) == typeof(byte) || typeof(T) == typeof(sbyte))
            {
                size = sizeof(byte);
                return true;
            }

            if (typeof(T) == typeof(char) || typeof(T) == typeof(short) || typeof(T) == typeof(ushort))
            {
                size = sizeof(char);
                return true;
            }

            if (typeof(T) == typeof(int) || typeof(T) == typeof(uint))
            {
                size = sizeof(int);
                return true;
            }

            if (typeof(T) == typeof(long) || typeof(T) == typeof(ulong))
            {
                size = sizeof(long);
                return true;
            }

            size = default;
            return false;
        }

        #endregion
    }
}
