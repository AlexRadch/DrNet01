using System;
using System.Runtime.CompilerServices;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        /// <summary>
        /// Determines whether two sequences are equal.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The sequence to compare with.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsToSeq<TSource, TOther>(this Span<TSource> span, ReadOnlySpan<TOther> other,
            Func<TSource, TOther, bool> equalityComparer = null)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TOther) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.SequenceEqual(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(other));
                    if (UnsafeIn.AreSame(in DrNetMarshal.GetReference(span),
                        in UnsafeIn.As<TOther, TSource>(in DrNetMarshal.GetReference(other))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TOther)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(other),
                        in DrNetMarshal.GetReference(span), length, (oValue, sValue) =>
                            ((IEquatable<TSource>)oValue).Equals(sValue));
                if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                        in DrNetMarshal.GetReference(other), length, (sValue, oValue) =>
                            ((IEquatable<TOther>)sValue).Equals(oValue));
                return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                    in DrNetMarshal.GetReference(other), length, (sValue, oValue) => sValue.Equals(oValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                in DrNetMarshal.GetReference(other), length, equalityComparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The sequence to compare with.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsToSeq<TSource, TOther>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TOther> other,
            Func<TSource, TOther, bool> equalityComparer = null)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;

            if (equalityComparer == null)
            {
                if (typeof(TOther) == typeof(TSource))
                {
                    if (default(TSource) != null && DrNetMarshal.IsTypeComparableAsBytes<TSource>())
                        return MemoryExtensions.SequenceEqual(DrNetMarshal.UnsafeCastBytes(span),
                            DrNetMarshal.UnsafeCastBytes(other));
                    if (UnsafeIn.AreSame(in DrNetMarshal.GetReference(span),
                        in UnsafeIn.As<TOther, TSource>(in DrNetMarshal.GetReference(other))))
                        return true;
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TOther)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(other),
                        in DrNetMarshal.GetReference(span), length, (oValue, sValue) =>
                            ((IEquatable<TSource>)oValue).Equals(sValue));
                if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                        in DrNetMarshal.GetReference(other), length, (sValue, oValue) =>
                            ((IEquatable<TOther>)sValue).Equals(oValue));
                return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                    in DrNetMarshal.GetReference(other), length, (sValue, oValue) => sValue.Equals(oValue));
            }

            return DrNetSpanHelpers.EqualsToSeq(in DrNetMarshal.GetReference(span),
                in DrNetMarshal.GetReference(other), length, equalityComparer);
        }
    }
}
