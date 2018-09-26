﻿using System;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using DrNet.Internal;
using DrNet.Internal.Unsafe;

namespace DrNet                                                                                                         
{
    public static class MemoryExt
    {
        #region AsReadOnlyMemory

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array) => new ReadOnlyMemory<T>(array);

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array, int start)
        {
            if (array == null)
            {
                if (start != 0)
                    throw new ArgumentOutOfRangeException(nameof(start));
                return new ReadOnlyMemory<T>(array);
            }
            return new ReadOnlyMemory<T>(array, start, array.Length - start);
        }

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this T[] array, int start, int length) => 
            new ReadOnlyMemory<T>(array, start, length);

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this ArraySegment<T> segment) =>
            new ReadOnlyMemory<T>(segment.Array, segment.Offset, segment.Count);

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this ArraySegment<T> segment, int start)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new ReadOnlyMemory<T>(segment.Array, segment.Offset + start, segment.Count - start);
        }

        public static ReadOnlyMemory<T> AsReadOnlyMemory<T>(this ArraySegment<T> segment, int start, int length)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (((uint)length) > segment.Count - start)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new ReadOnlyMemory<T>(segment.Array, segment.Offset + start, length);
        }

        #endregion

        #region AsReadOnlySpan

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array) => new ReadOnlySpan<T>(array);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array, int start)
        {
            if (array == null)
            {
                if (start != 0)
                    throw new ArgumentOutOfRangeException(nameof(start));
                return default;
            }
            return new ReadOnlySpan<T>(array, start, array.Length - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array, int start, int length) =>
            new ReadOnlySpan<T>(array, start, length);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment) =>
            new ReadOnlySpan<T>(segment.Array, segment.Offset, segment.Count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment, int start)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new ReadOnlySpan<T>(segment.Array, segment.Offset + start, segment.Count - start);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment, int start, int length)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));
            if (((uint)length) > segment.Count - start)
                throw new ArgumentOutOfRangeException(nameof(length));

            return new ReadOnlySpan<T>(segment.Array, segment.Offset + start, length);
        }

        #endregion

        #region IndexOfEqual IndexOfNotEqual

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfEqual<TSource, TValue>(this Span<TSource> span, TValue value, 
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                        (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfEqual<TSource, TValue>(this ReadOnlySpan<TSource> span, TValue value, 
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                        (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfEqualFrom<TSource, TValue>(this Span<TSource> span, TValue value, 
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                        (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfEqualFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, TValue value, 
            Func<TValue, TSource, bool> equalityComparer)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                        value, (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
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
        public static int IndexOfNotEqual<TSource, TValue>(this Span<TSource> span, TValue value)
        {
            if (value is IEquatable<TSource> vEquatable)
                return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    vEquatable, (eValue, sValue) => !eValue.Equals(sValue));

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => !((IEquatable<TValue>)sValue).Equals(vValue));

            return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                (vValue, sValue) => !vValue.Equals(sValue));
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
                return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, vEquatable,
                    (eValue, sValue) => !eValue.Equals(sValue));

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => !((IEquatable<TValue>)sValue).Equals(vValue));

            return SpanHelpers.IndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                (vValue, sValue) => !vValue.Equals(sValue));
        }

        #endregion

        #region LastIndexOfEqual LastIndexOfNotEqual

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqual<TSource, TValue>(this Span<TSource> span, TValue value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        value, (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    value, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqual<TSource, TValue>(this ReadOnlySpan<TSource> span, TValue value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        value, (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    value, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualFrom<TSource, TValue>(this Span<TSource> span, TValue value,
            Func<TValue, TSource, bool> equalityComparer)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        value, (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    value, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, TValue value,
            Func<TValue, TSource, bool> equalityComparer)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte) || 
                    typeof(TSource) == typeof(char) && typeof(TValue) == typeof(char))
                {
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, 
                        Unsafe.As<TValue, TSource>(ref value));
                }
                if (value is IEquatable<TSource> vEquatable)
                    return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        vEquatable, (eValue, sValue) => eValue.Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        value, (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    value, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                equalityComparer);
        }

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its last occurrence.
        /// If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqual<TSource, TValue>(this Span<TSource> span, TValue value)
        {
            if (value is IEquatable<TSource> vEquatable)
                return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    vEquatable, (eValue, sValue) => !eValue.Equals(sValue));

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => !((IEquatable<TValue>)sValue).Equals(vValue));

            return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                (vValue, sValue) => !vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its last occurrence.
        /// If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqual<TSource, TValue>(this ReadOnlySpan<TSource> span, TValue value)
        {
            if (value is IEquatable<TSource> vEquatable)
                return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    vEquatable, (eValue, sValue) => !eValue.Equals(sValue));

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.LastIndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (sValue, vValue) => !((IEquatable<TValue>)sValue).Equals(vValue));

            return SpanHelpers.LastIndexOfEqualValueComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                (vValue, sValue) => !vValue.Equals(sValue));
        }

        #endregion

        #region IndexOfEqualAny

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
        public static int IndexOfEqualAny<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values, 
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)), 
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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
        public static int IndexOfEqualAny<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> values, 
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)),
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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
        public static int IndexOfEqualAnyFrom<TSource, TValue>(this Span<TSource> span, 
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)),
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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
        public static int IndexOfEqualAnyFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, 
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    // Work around https://github.com/dotnet/corefx/issues/32334 issue
                    if (values.Length == 0)
                        return -1;

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)),
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        #endregion

        #region LastIndexOfEqualAny

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

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)), 
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)), 
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)), 
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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

                    ReadOnlySpan<TSource> tValues;
                    unsafe
                    {
                        tValues = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)), 
                            values.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOfAny(span, tValues);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                        ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfEqualAnySourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfEqualAnyValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        #endregion

        #region IndexOfNotEqualAll

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
                    return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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
                    return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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
                    return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
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
                    return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        #endregion

        #region LastIndexOfNotEqualAll

        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its last 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAll<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its last 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAll<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its last 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAllFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the value that not equal to the all specified values and returns the index of its last 
        /// occurrence. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAllFrom<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                        span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        #endregion

        #region IndexOfSeq

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length, 
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeqFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeqFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value,
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.IndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
        }

        #endregion

        #region LastIndexOfSeq

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
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
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
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
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
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
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
                {
                    ReadOnlySpan<TSource> tValue;
                    unsafe
                    {
                        tValue = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
                            value.Length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.LastIndexOf(span, tValue);
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                        ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(value), value.Length, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(value), value.Length, equalityComparer);
        }

        #endregion

        #region EqualsToSeq

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
                    if (default(TSource) != null && TypeExt.IsTypeComparableAsBytes<TSource>())
                    {
                        ReadOnlySpan<TSource> tOther;
                        unsafe
                        {
                            tOther = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)),
                                other.Length);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.SequenceEqual(span, tOther);
                    }
                    if (Unsafe.AreSame(ref MemoryMarshal.GetReference(span), ref Unsafe.As<TOther, TSource>(
                        ref MemoryMarshal.GetReference(other))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TOther)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(other), 
                        ref MemoryMarshal.GetReference(span), length, (oValue, sValue) =>
                            ((IEquatable<TSource>)oValue).Equals(sValue));
                if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span), 
                        ref MemoryMarshal.GetReference(other), length, (sValue, oValue) =>
                            ((IEquatable<TOther>)sValue).Equals(oValue));
                return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span), 
                    ref MemoryMarshal.GetReference(other), length, (sValue, oValue) => sValue.Equals(oValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other),
                length, equalityComparer);
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
                    if (default(TSource) != null && TypeExt.IsTypeComparableAsBytes<TSource>())
                    {
                        ReadOnlySpan<TSource> tOther;
                        unsafe
                        {
                            tOther = new ReadOnlySpan<TSource>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)),
                                other.Length);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TSource>.Instance.SequenceEqual(span, tOther);
                    }
                    if (Unsafe.AreSame(ref MemoryMarshal.GetReference(span), ref Unsafe.As<TOther, TSource>(
                        ref MemoryMarshal.GetReference(other))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TOther)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(other),
                        ref MemoryMarshal.GetReference(span), length, (oValue, sValue) =>
                            ((IEquatable<TSource>)oValue).Equals(sValue));
                if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                        ref MemoryMarshal.GetReference(other), length, (sValue, oValue) =>
                            ((IEquatable<TOther>)sValue).Equals(oValue));
                return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                    ref MemoryMarshal.GetReference(other), length, (sValue, oValue) => sValue.Equals(oValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other),
                length, equalityComparer);
        }

        #endregion

        #region StartsWithSeq

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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)),
                                valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref MemoryMarshal.GetReference(span), ref Unsafe.As<TValue, TSource>(
                        ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref MemoryMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value),
                valueLength, equalityComparer);
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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)),
                                valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref MemoryMarshal.GetReference(span), ref Unsafe.As<TValue, TSource>(
                        ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref MemoryMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value),
                valueLength, equalityComparer);
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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)),
                                valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref MemoryMarshal.GetReference(span), ref Unsafe.As<TValue, TSource>(
                        ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref MemoryMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value), ref MemoryMarshal.GetReference(span),
                valueLength, equalityComparer);
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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(span)),
                                valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref MemoryMarshal.GetReference(span), ref Unsafe.As<TValue, TSource>(
                        ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref MemoryMarshal.GetReference(span), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(span),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value), ref MemoryMarshal.GetReference(span),
                valueLength, equalityComparer);
        }

        #endregion

        #region EndsWithSeq

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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref Unsafe.Add(
                                ref MemoryMarshal.GetReference(span), start)), valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), 
                        ref Unsafe.As<TValue, TSource>(ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), 
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref Unsafe.Add(
                                ref MemoryMarshal.GetReference(span), start)), valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                        ref Unsafe.As<TValue, TSource>(ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref Unsafe.Add(
                                ref MemoryMarshal.GetReference(span), start)), valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                        ref Unsafe.As<TValue, TSource>(ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), valueLength, equalityComparer);
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
                    if (default(TValue) != null && TypeExt.IsTypeComparableAsBytes<TValue>())
                    {
                        ReadOnlySpan<TValue> tSource;
                        unsafe
                        {
                            tSource = new ReadOnlySpan<TValue>(Unsafe.AsPointer(ref Unsafe.Add(
                                ref MemoryMarshal.GetReference(span), start)), valueLength);
                        }
                        return MemoryExtensionsEquatablePatternMatching<TValue>.Instance.SequenceEqual(tSource, value);
                    }
                    if (Unsafe.AreSame(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                        ref Unsafe.As<TValue, TSource>(ref MemoryMarshal.GetReference(value))))
                        return true;
                }

                if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                    return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value),
                        ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), valueLength, (vValue, sValue) =>
                            ((IEquatable<TSource>)vValue).Equals(sValue));
                if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                    return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                        ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) =>
                            ((IEquatable<TValue>)sValue).Equals(vValue));
                return SpanHelpers.EqualsToSeq(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start),
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
            }

            return SpanHelpers.EqualsToSeq(ref MemoryMarshal.GetReference(value), 
                ref Unsafe.Add(ref MemoryMarshal.GetReference(span), start), valueLength, equalityComparer);
        }

        #endregion

        //#region Skip

        //public static int Skip<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> skipValues)
        //{

        //}

        //#endregion

        //#region Trim

        //public static Span<TSource> Trim<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> trimValues)
        //{

        //}

        //public static Span<TSource> Trim<TSource>(this Span<TSource> span, int start, int end)
        //{
        //    int length;
        //    if (start > 0)
        //    {
        //        if (end >= 0)
        //            length = end - start;
        //        else
        //            length = span.Length - start;
        //    }
        //    else
        //    {
        //        if (end < 0 || end == span.Length)
        //            return span;
        //        length = end;
        //    }
        //    return span.Slice(start, length);
        //}

        //#endregion
    }
}
