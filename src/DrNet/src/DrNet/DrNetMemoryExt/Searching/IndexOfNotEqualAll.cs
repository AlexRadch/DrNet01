using System;
using System.Runtime.CompilerServices;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its first 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqualAll<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its first 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqualAll<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its first 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqualAllFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.IndexOfNotEqualAllValueComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its first 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqualAllFrom<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.IndexOfNotEqualAllSourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.IndexOfNotEqualAllValueComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }
    }
}
