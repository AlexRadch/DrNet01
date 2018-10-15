using System;
using System.Runtime.CompilerServices;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWithSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            if (valueLength > span.Length)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.StartsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in DrNetMarshal.GetReference(span),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in DrNetMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                in DrNetMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWithSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            if (valueLength > span.Length)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.StartsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in DrNetMarshal.GetReference(span),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in DrNetMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                in DrNetMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWithSeqFrom<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            if (valueLength > span.Length)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.StartsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in DrNetMarshal.GetReference(span),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in DrNetMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                in DrNetMarshal.GetReference(span), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool StartsWithSeqFrom<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            if (valueLength > span.Length)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.StartsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in DrNetMarshal.GetReference(span),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in DrNetMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                in DrNetMarshal.GetReference(span), valueLength, equalityComparer);
        }
    }
}
