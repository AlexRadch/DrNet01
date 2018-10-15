using System;
using System.Runtime.CompilerServices;
using UnsafeRef = System.Runtime.CompilerServices.Unsafe;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWithSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            int start = span.Length - valueLength;
            if (start < 0)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.EndsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start),
                in DrNetMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWithSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            int start = span.Length - valueLength;
            if (start < 0)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.EndsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                in DrNetMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWithSeqFrom<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            int start = span.Length - valueLength;
            if (start < 0)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.EndsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                in UnsafeRef.Add(ref DrNetMarshal.GetReference(span), start), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EndsWithSeqFrom<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer = null)
        {
            int valueLength = value.Length;
            int start = span.Length - valueLength;
            if (start < 0)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TValue) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.EndsWith(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(value));
                    if (UnsafeIn.AreSame(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                        in UnsafeIn.As<TValue, TSource>(in DrNetMarshal.GetReference(value))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                        in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                        in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.EqualsToSeq(in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start),
                    in DrNetMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(value),
                in UnsafeIn.Add(in DrNetMarshal.GetReference(span), start), valueLength, equalityComparer);
        }
    }
}
