using System;
using System.Runtime.CompilerServices;
using UnsafeRef = System.Runtime.CompilerServices.Unsafe;

namespace DrNet.Unsafe
{
    /// Contains generic, low-level functionality for manipulating readonly pointers.
    public static unsafe class UnsafeIn
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Add<T>(in T source, int elementOffset) => 
            ref UnsafeRef.Add(ref UnsafeRef.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Add<T>(in T source, IntPtr elementOffset) => 
            ref UnsafeRef.Add(ref UnsafeRef.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AddByteOffset<T>(in T source, IntPtr byteOffset) =>
            ref UnsafeRef.AddByteOffset(ref UnsafeRef.AsRef(in source), byteOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreSame<T>(in T left, in T right) =>
            UnsafeRef.AreSame(ref UnsafeRef.AsRef(in left), ref UnsafeRef.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly TTo As<TFrom, TTo>(in TFrom source) => 
            ref UnsafeRef.As<TFrom, TTo>(ref UnsafeRef.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AsPointer<T>(in T value) => UnsafeRef.AsPointer(ref UnsafeRef.AsRef(in value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AsRef<T>(void* source) => ref UnsafeRef.AsRef<T>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AsRef<T>(ref T source) => ref source;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr ByteOffset<T>(in T origin, in T target) =>
            UnsafeRef.ByteOffset(ref UnsafeRef.AsRef(in origin), ref UnsafeRef.AsRef(in target));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy<T>(void* destination, in T source) =>
            UnsafeRef.Copy(destination, ref UnsafeRef.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(ref byte destination, in byte source, uint byteCount) =>
            UnsafeRef.CopyBlock(ref destination, ref UnsafeRef.AsRef(in source), byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlockUnaligned(ref byte destination, in byte source, uint byteCount) =>
            UnsafeRef.CopyBlockUnaligned(ref destination, ref UnsafeRef.AsRef(in source), byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressGreaterThan<T>(in T left, in T right) =>
            UnsafeRef.IsAddressGreaterThan(ref UnsafeRef.AsRef(in left), ref UnsafeRef.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressLessThan<T>(in T left, in T right) =>
            UnsafeRef.IsAddressLessThan(ref UnsafeRef.AsRef(in left), ref UnsafeRef.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadUnaligned<T>(in byte source) => UnsafeRef.ReadUnaligned<T>(ref UnsafeRef.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Subtract<T>(in T source, int elementOffset) =>
            ref UnsafeRef.Subtract(ref UnsafeRef.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Subtract<T>(in T source, IntPtr elementOffset) =>
            ref UnsafeRef.Subtract(ref UnsafeRef.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T SubtractByteOffset<T>(in T source, IntPtr byteOffset) =>
            ref UnsafeRef.Subtract(ref UnsafeRef.AsRef(in source), byteOffset);
    }
}
