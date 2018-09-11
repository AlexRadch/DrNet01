using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using DrNet.Internal;

namespace DrNet
{
    public static class MemoryExtensions
    {
        #region IndexOf

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int IndexOf<T>(this Span<T> span, T value)
        {
            if (value is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOf(span, value);
            return SpanHelpers.IndexOfObjectEquals(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int IndexOf<T>(this ReadOnlySpan<T> span, T value)
        {
            if (value is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOf(span, value);
            return SpanHelpers.IndexOfObjectEquals(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The equality comparer to compare values.</param>
        public static int IndexOf<T>(this Span<T> span, T value, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), value, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The equality comparer to compare values.</param>
        public static int IndexOf<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), value, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int IndexOf<T>(this Span<T> span, IEquatable<T> value)
        {
            if (value is T tValue && tValue is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOf(span, tValue);
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int IndexOf<T>(this ReadOnlySpan<T> span, IEquatable<T> value)
        {
            if (value is T tValue && tValue is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOf(span, tValue);
            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        #endregion

        #region LastIndexOf

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int LastIndexOf<T>(this Span<T> span, IEquatable<T> value)
        {
            if (value is T tValue && tValue is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOf(span, tValue);
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, IEquatable<T> value)
        {
            if (value is T tValue && tValue is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOf(span, tValue);
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The equality comparer to compare values.</param>
        public static int LastIndexOf<T>(this Span<T> span, T value, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), value, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="equalityComparer">The equality comparer to compare values.</param>
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), value, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int LastIndexOf<T>(this Span<T> span, T value)
        {
            if (value is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOf(span, value);
            return SpanHelpers.LastIndexOfObjectEquals(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int LastIndexOf<T>(this ReadOnlySpan<T> span, T value)
        {
            if (value is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOf(span, value);
            return SpanHelpers.LastIndexOfObjectEquals(ref MemoryMarshal.GetReference(span), value, span.Length);
        }

        #endregion

        #region IndexOfAny

        #region value0, value1

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span,  IEquatable<T> value0, IEquatable<T> value1)
        {
            if (value0 is T tValue0 && tValue0 is IEquatable<T> && value1 is T tValue1)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, tValue0, tValue1);
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, IEquatable<T> value0, IEquatable<T> value1)
        {
            if (value0 is T tValue0 && tValue0 is IEquatable<T> && value1 is T tValue1)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, tValue0, tValue1);
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, value0, value1);
            return SpanHelpers.IndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, value0, value1);
            return SpanHelpers.IndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        #endregion

        #region value0, value1, value2

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2)
        {
            if (value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2  && tValue0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, tValue0, tValue1, tValue2);
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2)
        {
            if (value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2  && tValue0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, tValue0, tValue1, tValue2);
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, T value0, T value1, T value2)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, value0, value1, value2);
            return SpanHelpers.IndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, value0, value1, value2);
            return SpanHelpers.IndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        #endregion

        #region values

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> values)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> values)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values)
        {
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, values);
            return SpanHelpers.IndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        /// <summary>
        /// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int IndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        {
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.IndexOfAny(span, values);
            return SpanHelpers.IndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        #endregion

        #endregion

        #region LastIndexOfAny

        #region value0, value1

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, IEquatable<T> value0, IEquatable<T> value1)
        {
            if (value0 is T tValue0 && tValue0 is IEquatable<T> && value1 is T tValue1)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, tValue0, tValue1);
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        /// <summary>
        /// Searches for the дast index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, IEquatable<T> value0, IEquatable<T> value1)
        {
            if (value0 is T tValue0 && tValue0 is IEquatable<T> && value1 is T tValue1)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, tValue0, tValue1);
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, value0, value1);
            return SpanHelpers.LastIndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, value0, value1);
            return SpanHelpers.LastIndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        }

        #endregion

        #region value0, value1, value2

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2)
        {
            if (value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2  && tValue0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, tValue0, tValue1, tValue2);
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2)
        {
            if (value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2  && tValue0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, tValue0, tValue1, tValue2);
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, equalityComparer, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, T value0, T value1, T value2)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, value0, value1, value2);
            return SpanHelpers.LastIndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value0">One of the values to search for.</param>
        /// <param name="value1">One of the values to search for.</param>
        /// <param name="value2">One of the values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2)
        {
            if (value0 is IEquatable<T>)
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, value0, value1, value2);
            return SpanHelpers.LastIndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        }

        #endregion

        #region values

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> values)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T). 
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> values)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T> equalityComparer)
        {
            return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfAny<T>(this Span<T> span, ReadOnlySpan<T> values)
        {
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, values);
            return SpanHelpers.LastIndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        /// <summary>
        /// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="values">The set of values to search for.</param>
        public static int LastIndexOfAny<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        {
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.LastIndexOfAny(span, values);
            return SpanHelpers.LastIndexOfAnyObjectEquals(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        }

        #endregion

        #endregion

        #region SequenceEqual

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEquatable{T}.Equals(T).
        /// </summary>
        public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> other)
        {
            int length = span.Length;
            return length != other.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEquatable{T}.Equals(T).
        /// </summary>
        public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> other)
        {
            int length = span.Length;
            return length != other.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<T> other, IEqualityComparer<T> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, equalityComparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other, IEqualityComparer<T> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, equalityComparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool SequenceEqual<T>(this Span<T> span, ReadOnlySpan<T> other)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.SequenceEqual(span, other);
            return SpanHelpers.SequenceEqualObjectEquals(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool SequenceEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> other)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.SequenceEqual(span, other);
            return SpanHelpers.SequenceEqualObjectEquals(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length);
        }

        #endregion

        #region StartsWith

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T).
        /// </summary>
        public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> value)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T).
        /// </summary>
        public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> value)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        {
            int valueLength = value.Length;
            return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool StartsWith<T>(this Span<T> span, ReadOnlySpan<T> value)
        {
            int valueLength = value.Length;
            if (valueLength > span.Length)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.SequenceEqual(span.Slice(0, valueLength), value);
            return SpanHelpers.SequenceEqualObjectEquals(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool StartsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        {
            int valueLength = value.Length;
            if (valueLength > span.Length)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.SequenceEqual(span.Slice(0, valueLength), value);
            return SpanHelpers.SequenceEqualObjectEquals(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        }

        #endregion

        #region EndsWith

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T).
        /// </summary>
        public static bool EndsWith<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
                ref MemoryMarshal.GetReference(value), valueLength);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T).
        /// </summary>
        public static bool EndsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
                ref MemoryMarshal.GetReference(value), valueLength);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool EndsWith<T>(this Span<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool EndsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
                ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool EndsWith<T>(this Span<T> span, ReadOnlySpan<T> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.SequenceEqual(span.Slice(spanLength - valueLength), value);
            return SpanHelpers.SequenceEqualObjectEquals(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
                ref MemoryMarshal.GetReference(value), valueLength);
        }

        /// <summary>
        /// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool EndsWith<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        {
            int spanLength = span.Length;
            int valueLength = value.Length;
            if (valueLength > spanLength)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
                return IEquatablePatternMatchingBase<T>.Instance.SequenceEqual(span.Slice(spanLength - valueLength), value);
            return SpanHelpers.SequenceEqualObjectEquals(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
                ref MemoryMarshal.GetReference(value), valueLength);
        }

        #endregion
    }
}
