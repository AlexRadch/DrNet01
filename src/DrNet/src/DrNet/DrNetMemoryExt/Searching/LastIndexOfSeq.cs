using System;
using System.Runtime.CompilerServices;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                    return MemoryExtensions.LastIndexOf(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(value));
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfSeqFrom(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                    in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(value), value.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                    return MemoryExtensions.LastIndexOf(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(value));
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfSeqFrom(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                    in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(value), value.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeqFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value,
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                    return MemoryExtensions.LastIndexOf(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(value));
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfSeqFrom(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                    in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfSeqFrom(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(value), value.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeqFrom<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                    return MemoryExtensions.LastIndexOf(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(value));
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfSeqFrom(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                        in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfSeq(in DrNetMarshal.GetReference(span), span.Length,
                    in DrNetMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfSeqFrom(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(value), value.Length, equalityComparer);
        }
    }
}
