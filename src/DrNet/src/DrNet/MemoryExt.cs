using System;
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

        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array) => new ReadOnlySpan<T>(array);

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

        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this T[] array, int start, int length) =>
            new ReadOnlySpan<T>(array, start, length);

        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment) =>
            new ReadOnlySpan<T>(segment.Array, segment.Offset, segment.Count);

        public static ReadOnlySpan<T> AsReadOnlySpan<T>(this ArraySegment<T> segment, int start)
        {
            if (((uint)start) > segment.Count)
                throw new ArgumentOutOfRangeException(nameof(start));

            return new ReadOnlySpan<T>(segment.Array, segment.Offset + start, segment.Count - start);
        }
        
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
                else
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
                else
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
                else
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
                    return SpanHelpers.IndexOfEqualSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, value, 
                        (sValue, vValue) => ((IEquatable<TValue>)sValue).Equals(vValue));
                else
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
        public static int IndexOfNotEqual<TSource, TValue>(this Span<TSource> span, TValue value)
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

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its first occurrence.
        /// If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
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
                else
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
                else
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
                else
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
        public static int IndexOfEqualAny<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values, 
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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
        public static int IndexOfEqualAny<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> values, 
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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
        public static int IndexOfEqualAnyFrom<TSource, TValue>(this Span<TSource> span, 
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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
        public static int IndexOfEqualAnyFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, 
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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

        #region LastIndexOfAnyEqual

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with 
        /// the logical OR operator. If not found, returns -1. 
        /// Elements are compared using the specified equality comparer or use IEquatable{TSource}.Equals(TSource) or
        /// IEquatable{TValue}.Equals(TValue) or TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfEqualAny<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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
        public static int LastIndexOfEqualAny<TSource, TValue>(this ReadOnlySpan<TSource> span, 
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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
        public static int LastIndexOfEqualAnyFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values,
            Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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
        public static int LastIndexOfEqualAnyFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, 
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer = null)
        {
            if (equalityComparer == null)
            {
                if (typeof(TSource) == typeof(byte) && typeof(TValue) == typeof(byte))
                {
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
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfNotEqualAll<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values)
        {
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                        vValue is IEquatable<TSource> equatable ? equatable.Equals(sValue) : vValue.Equals(sValue));
            }
            else if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                        sValue is IEquatable<TValue> equatable ? equatable.Equals(vValue) : sValue.Equals(vValue));
            else
                return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length, 
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                        vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfNotEqualAll<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values)
        {
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) => 
                        vValue is IEquatable<TSource> equatable ? equatable.Equals(sValue) : vValue.Equals(sValue));
            }
            else if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                        sValue is IEquatable<TValue> equatable ? equatable.Equals(vValue) : sValue.Equals(vValue));
            else
                return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                    ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfNotEqualAllSourceComparer<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer)
        {
            return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfNotEqualAllSourceComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer)
        {
            return SpanHelpers.IndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfNotEqualAllValueComparer<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer)
        {
            return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfNotEqualAllValueComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer)
        {
            return SpanHelpers.IndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        #endregion

        #region LastIndexOfNotEqualAll

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfNotEqualAll<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> values)
        {
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                        vValue is IEquatable<TSource> equatable ? equatable.Equals(sValue) : vValue.Equals(sValue));
            }
            else if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue is IEquatable<TValue> equatable ? equatable.Equals(vValue) : sValue.Equals(vValue));
            else
                return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfNotEqualAll<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values)
        {
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (vValue, sValue) =>
                        vValue is IEquatable<TSource> equatable ? equatable.Equals(sValue) : vValue.Equals(sValue));
            }
            else if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) =>
                        sValue is IEquatable<TValue> equatable ? equatable.Equals(vValue) : sValue.Equals(vValue));
            else
                return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span),
                    span.Length, ref MemoryMarshal.GetReference(values), values.Length, (sValue, vValue) => 
                        vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfNotEqualAllSourceComparer<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer)
        {
            return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfNotEqualAllSourceComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TSource, TValue, bool> equalityComparer)
        {
            return SpanHelpers.LastIndexOfNotEqualAllSourceComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfNotEqualAllValueComparer<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer)
        {
            return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first value that not equal to the specified values. If not found, returns -1. 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfNotEqualAllValueComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> values, Func<TValue, TSource, bool> equalityComparer)
        {
            return SpanHelpers.LastIndexOfNotEqualAllValueComparer(ref MemoryMarshal.GetReference(span), span.Length,
                ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        #endregion

        #region IndexOfSeq

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        public static int IndexOfSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                //if (typeof(TOther) == typeof(T))
                //{

                //    ReadOnlySpan<T> tOther;
                //    unsafe
                //    {
                //        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                //    }
                //    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                //}
                return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => 
                        sValue is IEquatable<TSource> sEquatable ? sEquatable.Equals(vValue) : sValue.Equals(vValue));
            }
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (vValue, sValue) => 
                        vValue is IEquatable<TSource> vEquatable ? vEquatable.Equals(sValue) : vValue.Equals(sValue));
            return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        public static int IndexOfSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                //if (typeof(TOther) == typeof(T))
                //{

                //    ReadOnlySpan<T> tOther;
                //    unsafe
                //    {
                //        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                //    }
                //    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                //}
                return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => 
                        sValue is IEquatable<TSource> sEquatable ? sEquatable.Equals(vValue) : sValue.Equals(vValue));
            }
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (vValue, sValue) => 
                        vValue is IEquatable<TSource> vEquatable ? vEquatable.Equals(sValue) : vValue.Equals(sValue));
            return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TSource, TValue, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TSource, TValue, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.IndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfSeqFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TValue, TSource, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its first occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int IndexOfSeqFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TValue, TSource, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.IndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        #endregion

        #region LastIndexOfSeq

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        public static int LastIndexOfSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                //if (typeof(TOther) == typeof(T))
                //{

                //    ReadOnlySpan<T> tOther;
                //    unsafe
                //    {
                //        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                //    }
                //    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                //}
                return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => 
                        sValue is IEquatable<TSource> sEquatable ? sEquatable.Equals(vValue) : sValue.Equals(vValue));
            }
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (vValue, sValue) => 
                        vValue is IEquatable<TSource> vEquatable ? vEquatable.Equals(sValue) : vValue.Equals(sValue));
            return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        public static int LastIndexOfSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
            {
                //if (typeof(TOther) == typeof(T))
                //{

                //    ReadOnlySpan<T> tOther;
                //    unsafe
                //    {
                //        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                //    }
                //    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                //}
                return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => 
                        sValue is IEquatable<TSource> sEquatable ? sEquatable.Equals(vValue) : sValue.Equals(vValue));
            }
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TValue)))
                return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                    ref MemoryMarshal.GetReference(value), valueLength, (vValue, sValue) => 
                        vValue is IEquatable<TSource> vEquatable ? vEquatable.Equals(sValue) : vValue.Equals(sValue));
            return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, (sValue, vValue) => sValue.Equals(vValue));
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TSource, TValue, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TSource, TValue, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.LastIndexOfSeq(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfSeqFrom<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value, 
            Func<TValue, TSource, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Searches for the specified sequence and returns the index of its last occurrence. If not found, returns -1.
        /// Elements are compared using the equality comparer.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search for.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static int LastIndexOfSeqFrom<TSource, TValue>(this ReadOnlySpan<TSource> span, 
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return -1;
            if (value.Length == 0)
                return 0;

            return SpanHelpers.LastIndexOfSeqFrom(ref MemoryMarshal.GetReference(span), spanLength,
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        #endregion

        #region EqualToSeq

        /// <summary>
        /// Determines whether two sequences are equal.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TOther}.Equals(TOther) or 
        /// TOther.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The sequence to compare with.</param>
        public static bool EqualToSeq<TSource, TOther>(this Span<TSource> span, ReadOnlySpan<TOther> other)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TOther)))
            {
                //if (typeof(TOther) == typeof(T))
                //{

                //    ReadOnlySpan<T> tOther;
                //    unsafe
                //    {
                //        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                //    }
                //    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                //}
                return SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(other), 
                    ref MemoryMarshal.GetReference(span), length, (oValue, sValue) => 
                        oValue is IEquatable<TSource> oEquatable ? oEquatable.Equals(sValue) : oValue.Equals(sValue));
            }
            if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(span), 
                    ref MemoryMarshal.GetReference(other), length, (sValue, oValue) => 
                        sValue is IEquatable<TOther> sEquatable ? sEquatable.Equals(oValue) : sValue.Equals(oValue));
            return SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(other), 
                ref MemoryMarshal.GetReference(span), length, (oValue, sValue) => oValue.Equals(sValue));
        }

        /// <summary>
        /// Determines whether two sequences are equal.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TOther}.Equals(TOther) or 
        /// TOther.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The sequence to compare with.</param>
        public static bool EqualToSeq<TSource, TOther>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TOther> other)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;
            if (typeof(IEquatable<TSource>).IsAssignableFrom(typeof(TOther)))
            {
                //if (typeof(TOther) == typeof(T))
                //{

                //    ReadOnlySpan<T> tOther;
                //    unsafe
                //    {
                //        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                //    }
                //    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                //}
                return SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(other), 
                    ref MemoryMarshal.GetReference(span), length, (oValue, sValue) => 
                        oValue is IEquatable<TSource> oEquatable ? oEquatable.Equals(sValue) : oValue.Equals(sValue));
            }
            if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(TSource)))
                return SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(span), 
                    ref MemoryMarshal.GetReference(other), length, (sValue, oValue) => 
                        sValue is IEquatable<TOther> sEquatable ? sEquatable.Equals(oValue) : sValue.Equals(oValue));
            return SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(other), 
                ref MemoryMarshal.GetReference(span), length, (oValue, sValue) => oValue.Equals(sValue));
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using the equality comparer.
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The second span to compare.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EqualToSeq<TSource, TOther>(this Span<TSource> span, ReadOnlySpan<TOther> other,
            Func<TSource, TOther, bool> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(span),
                ref MemoryMarshal.GetReference(other), length, equalityComparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using the equality comparer.
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The sequence to compare with.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EqualToSeq<TSource, TOther>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TOther> other,
            Func<TSource, TOther, bool> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(span),
                ref MemoryMarshal.GetReference(other), length, equalityComparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using the equality comparer.
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The sequence to compare with.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EqualFromSeq<TSource, TOther>(this Span<TSource> span, ReadOnlySpan<TOther> other,
            Func<TOther, TSource, bool> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(other), 
                ref MemoryMarshal.GetReference(span), length, equalityComparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using the equality comparer.
        /// </summary>
        /// <param name="span">The span to compare.</param>
        /// <param name="other">The sequence to compare with.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EqualFromSeq<TSource, TOther>(this ReadOnlySpan<TSource> span, 
            ReadOnlySpan<TOther> other, Func<TOther, TSource, bool> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(other),
                ref MemoryMarshal.GetReference(span), length, equalityComparer);
        }

        #endregion

        #region StartsWithSeq

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        public static bool StartsWithSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && EqualToSeq(span.Slice(0, valueLength), value);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        public static bool StartsWithSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && EqualToSeq(span.Slice(0, valueLength), value);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool StartsWithSeqSourceComparer<TSource, TValue>(this Span<TSource> span, 
            ReadOnlySpan<TValue> value, Func<TSource, TValue, bool> equalityComparer)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(span), 
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool StartsWithSeqSourceComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> value, Func<TSource, TValue, bool> equalityComparer)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(span),
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool StartsWithSeqValueComparer<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(value),
                ref MemoryMarshal.GetReference(span), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the start of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool StartsWithSeqValueComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(value),
                ref MemoryMarshal.GetReference(span), valueLength, equalityComparer);
        }

        #endregion

        #region EndsWithSeq

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        public static bool EndsWithSeq<TSource, TValue>(this Span<TSource> span, ReadOnlySpan<TValue> value)
        {
            int valueLength = value.Length;
            int spanStart = span.Length - valueLength;
            return spanStart >=0 && EqualToSeq(span.Slice(spanStart, valueLength), value);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared using IEquatable{TSource}.Equals(TSource) or IEquatable{TValue}.Equals(TValue) or 
        /// TValue.Equals(TSource).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        public static bool EndsWithSeq<TSource, TValue>(this ReadOnlySpan<TSource> span, ReadOnlySpan<TValue> value)
        {
            int valueLength = value.Length;
            int spanStart = span.Length - valueLength;
            return spanStart >=0 && EqualToSeq(span.Slice(spanStart, valueLength), value);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EndsWithSeqSourceComparer<TSource, TValue>(this Span<TSource> span, 
            ReadOnlySpan<TValue> value, Func<TSource, TValue, bool> equalityComparer)
        {
            int valueLength = value.Length;
            int spanStart = span.Length - valueLength;
            return spanStart >= 0 && SpanHelpers.EqualToSeq(
                ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanStart),
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EndsWithSeqSourceComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> value, Func<TSource, TValue, bool> equalityComparer)
        {
            int valueLength = value.Length;
            int spanStart = span.Length - valueLength;
            return spanStart >= 0 && SpanHelpers.EqualToSeq(
                ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanStart),
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EndsWithSeqValueComparer<TSource, TValue>(this Span<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer)
        {
            int valueLength = value.Length;
            int spanStart = span.Length - valueLength;
            return spanStart >= 0 && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(value), 
                ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanStart), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span.
        /// Elements are compared by the equality comparer function.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The sequence to search at the end of the span.</param>
        /// <param name="equalityComparer">The function to test each element for a equality.</param>
        public static bool EndsWithSeqValueComparer<TSource, TValue>(this ReadOnlySpan<TSource> span,
            ReadOnlySpan<TValue> value, Func<TValue, TSource, bool> equalityComparer)
        {
            int valueLength = value.Length;
            int spanStart = span.Length - valueLength;
            return spanStart >= 0 && SpanHelpers.EqualToSeq(ref MemoryMarshal.GetReference(value), 
                ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanStart), valueLength, equalityComparer);
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
