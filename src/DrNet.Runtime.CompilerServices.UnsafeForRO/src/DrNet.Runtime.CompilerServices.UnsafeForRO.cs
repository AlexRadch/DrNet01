using System;
using System.Runtime.CompilerServices;

namespace DrNet.Runtime.CompilerServices
{
    /// Contains generic, low-level functionality for manipulating readonly pointers.
    public static unsafe class UnsafeForRO
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AddByteOffset<T>(in T source, IntPtr byteOffset) =>
            ref Unsafe.AddByteOffset(ref Unsafe.AsRef(in source), byteOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Add<T>(in T source, int elementOffset) =>
            ref Unsafe.Add(ref Unsafe.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Add<T>(in T source, IntPtr elementOffset) =>
            ref Unsafe.Add(ref Unsafe.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreSame<T>(in T left, in T right) =>
            Unsafe.AreSame(ref Unsafe.AsRef(in left), ref Unsafe.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void* AsPointer<T>(in T value) => Unsafe.AsPointer(ref Unsafe.AsRef(in value));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T AsRef<T>(void* source) => ref Unsafe.AsRef<T>(source);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly TTo As<TFrom, TTo>(in TFrom source) =>
            ref Unsafe.As<TFrom, TTo>(ref Unsafe.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IntPtr ByteOffset<T>(in T origin, in T target) =>
            Unsafe.ByteOffset(ref Unsafe.AsRef(in origin), ref Unsafe.AsRef(in target));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlock(ref byte destination, in byte source, uint byteCount) =>
            Unsafe.CopyBlock(ref destination, ref Unsafe.AsRef(in source), byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyBlockUnaligned(ref byte destination, in byte source, uint byteCount) =>
            Unsafe.CopyBlockUnaligned(ref destination, ref Unsafe.AsRef(in source), byteCount);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Copy<T>(void* destination, in T source) =>
            Unsafe.Copy(destination, ref Unsafe.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressGreaterThan<T>(in T left, in T right) =>
            Unsafe.IsAddressGreaterThan(ref Unsafe.AsRef(in left), ref Unsafe.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAddressLessThan<T>(in T left, in T right) =>
            Unsafe.IsAddressLessThan(ref Unsafe.AsRef(in left), ref Unsafe.AsRef(in right));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadUnaligned<T>(in byte source) => Unsafe.ReadUnaligned<T>(ref Unsafe.AsRef(in source));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T SubtractByteOffset<T>(in T source, IntPtr byteOffset) =>
            ref Unsafe.Subtract(ref Unsafe.AsRef(in source), byteOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Subtract<T>(in T source, int elementOffset) =>
            ref Unsafe.Subtract(ref Unsafe.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Subtract<T>(in T source, IntPtr elementOffset) =>
            ref Unsafe.Subtract(ref Unsafe.AsRef(in source), elementOffset);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly T Unbox<T>(object box) where T : struct => ref Unsafe.Unbox<T>(box);
    }
}
