using System;
using System.Runtime.CompilerServices;

using DrNet.Unsafe;

using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static partial class DrNetMemoryExt
    {
        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its first occurrence.
        /// If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqual<TSource, TValue>(this Span<TSource> span, TValue value)
        {
            if (value is IEquatable<TSource> vEquatable)
                return DrNetSpanHelpers.IndexOfEqualValueComparer(in DrNetMarshal.GetReference(span), span.Length,
                    vEquatable, (eValue, sValue) => !eValue.Equals(sValue));
            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return DrNetSpanHelpers.IndexOfEqualSourceComparer(in DrNetMarshal.GetReference(span), span.Length,
                    value, (sValue, vValue) => !((IEquatable<TValue>)sValue).Equals(vValue));
            return DrNetSpanHelpers.IndexOfEqualSourceComparer(in DrNetMarshal.GetReference(span), span.Length, value,
                (sValue, vValue) => !sValue.Equals(vValue));
        }

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its first occurrence.
        /// If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqual<TSource, TValue>(this ReadOnlySpan<TSource> span, TValue value)
        {
            if (value is IEquatable<TSource> vEquatable)
                return DrNetSpanHelpers.IndexOfEqualValueComparer(in DrNetMarshal.GetReference(span), span.Length,
                    vEquatable, (eValue, sValue) => !eValue.Equals(sValue));

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return DrNetSpanHelpers.IndexOfEqualSourceComparer(in DrNetMarshal.GetReference(span), span.Length,
                    value, (sValue, vValue) => !((IEquatable<TValue>)sValue).Equals(vValue));

            return DrNetSpanHelpers.IndexOfEqualSourceComparer(in DrNetMarshal.GetReference(span), span.Length, value,
                (sValue, vValue) => !sValue.Equals(vValue));
        }
    }
}
