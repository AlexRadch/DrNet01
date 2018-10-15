using System;
using System.Runtime.CompilerServices;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with 
        /// the logical OR operator. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualAny<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;
                    return MemoryExtensions.LastIndexOfAny(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(values));
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnyValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with 
        /// the logical OR operator. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualAny<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;
                    return MemoryExtensions.LastIndexOfAny(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(values));
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnyValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with 
        /// the logical OR operator. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualAnyFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;
                    return MemoryExtensions.LastIndexOfAny(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(values));
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnyValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfEqualAnyValueComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with 
        /// the logical OR operator. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualAnyFrom<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;
                    return MemoryExtensions.LastIndexOfAny(DrNetMarshal.UnsafeAs<TSource, byte>(span),
                        DrNetMarshal.UnsafeAs<TValue, byte>(values));
                }
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnyValueComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                        span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return DrNetSpanHelpers.LastIndexOfEqualAnySourceComparer(in DrNetMarshal.GetReference(span),
                    span.Length, in DrNetMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return DrNetSpanHelpers.LastIndexOfEqualAnyValueComparer(in DrNetMarshal.GetReference(span), span.Length,
                in DrNetMarshal.GetReference(values), values.Length, equalityComparer);
        }
    }
}
