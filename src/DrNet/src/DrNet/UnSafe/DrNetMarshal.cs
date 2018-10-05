using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace DrNet.UnSafe
{
    public static unsafe class DrNetMarshal
    {
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
            MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in reference), length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<byte> UnsafeAsBytes<T>(Span<T> span) => 
            CreateSpan(ref Unsafe.As<T, byte>(ref GetReference(span)), checked(span.Length * Unsafe.SizeOf<T>()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> UnsafeAsBytes<T>(ReadOnlySpan<T> span) =>
            CreateReadOnlySpan(in UnsafeIn.As<T, byte>(in GetReference(span)), 
                checked(span.Length * Unsafe.SizeOf<T>()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<TTo> UnsafeAs<TFrom, TTo>(Span<TFrom> span) => 
            CreateSpan(ref Unsafe.As<TFrom, TTo>(ref GetReference(span)), span.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<TTo> UnsafeAs<TFrom, TTo>(ReadOnlySpan<TFrom> span) => 
            CreateReadOnlySpan(in UnsafeIn.As<TFrom, TTo>(in GetReference(span)), span.Length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetReference<T>(Span<T> span) => ref MemoryMarshal.GetReference(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T GetReference<T>(ReadOnlySpan<T> span) => ref MemoryMarshal.GetReference(span);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetReference<T>(UnsafeSpan<T> span) => ref Unsafe.AsRef<T>(span._pointer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T GetReference<T>(UnsafeReadOnlySpan<T> span) => 
            ref UnsafeIn.AsRef<T>(span._pointer);
    }
}
