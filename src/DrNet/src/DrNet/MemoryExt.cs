﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using DrNet.Internal;
using DrNet.Internal.Unsafe;

namespace DrNet
{
    public static class MemoryExt
    {
        #region IndexOf

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int IndexOfEqual<T, TValue>(this Span<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
            {
                if (typeof(TValue) == typeof(T) && value is T tValue)
                    return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOf(span, tValue);

                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => eValue.Equals(sValue));
            } 

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? sEquatable.Equals(vValue) : vValue.Equals(sValue));

            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int IndexOfEqual<T, TValue>(this ReadOnlySpan<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
            {
                if (typeof(TValue) == typeof(T) && value is T tValue)
                    return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOf(span, tValue);

                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => eValue.Equals(sValue));
            } 

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? sEquatable.Equals(vValue) : vValue.Equals(sValue));

            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
        public static int IndexOfNotEqual<T, TValue>(this Span<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => !eValue.Equals(sValue));

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? !sEquatable.Equals(vValue) : !vValue.Equals(sValue));

            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => !vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its first occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
        public static int IndexOfNotEqual<T, TValue>(this ReadOnlySpan<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => !eValue.Equals(sValue));

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? !sEquatable.Equals(vValue) : !vValue.Equals(sValue));

            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => !vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for value for witch the equality comparer return true and returns the index of its first occurrence. If not found, returns -1.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="equalityComparer">A function to test each element for a equality.</param>
        public static int IndexOf<T, TValue>(this Span<T> span, TValue value, Func<TValue, T, bool> equalityComparer)
        {
            if (equalityComparer == null)
                throw new ArgumentNullException(nameof(equalityComparer));

            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, equalityComparer);
        }

        /// <summary>
        /// Searches for value for witch the equality comparer return true and returns the index of its first occurrence. If not found, returns -1.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="equalityComparer">A function to test each element for a equality.</param>
        public static int IndexOf<T, TValue>(this ReadOnlySpan<T> span, TValue value, Func<TValue, T, bool> equalityComparer)
        {
            if (equalityComparer == null)
                throw new ArgumentNullException(nameof(equalityComparer));

            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, equalityComparer);
        }

        #endregion

        #region LastIndexOf

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int LastIndexOfEqual<T, TValue>(this Span<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
            {
                if (typeof(TValue) == typeof(T) && value is T tValue)
                    return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOf(span, tValue);

                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => eValue.Equals(sValue));
            } 

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? sEquatable.Equals(vValue) : vValue.Equals(sValue));

            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for.</param>
        public static int LastIndexOfEqual<T, TValue>(this ReadOnlySpan<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
            {
                if (typeof(TValue) == typeof(T) && value is T tValue)
                    return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOf(span, tValue);

                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => eValue.Equals(sValue));
            } 

            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? sEquatable.Equals(vValue) : vValue.Equals(sValue));

            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
        public static int LastIndexOfNotEqual<T, TValue>(this Span<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => !eValue.Equals(sValue));
            
            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? !sEquatable.Equals(vValue) : !vValue.Equals(sValue));

            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => !vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for a value that not equal to the specified value and returns the index of its last occurrence. If not found, returns -1.
        /// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="value">The value to search for not equal value</param>
        public static int LastIndexOfNotEqual<T, TValue>(this ReadOnlySpan<T> span, TValue value)
        {
            if (value is IEquatable<T> vEquatable)
                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, vEquatable, (eValue, sValue) => !eValue.Equals(sValue));
            
            if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, 
                    (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ? !sEquatable.Equals(vValue) : !vValue.Equals(sValue));

            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, (vValue, sValue) => !vValue.Equals(sValue));
        }

        /// <summary>
        /// Searches for value for witch the equality comparer return true and returns the index of its last occurrence. If not found, returns -1.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="equalityComparer">A function to test each element for a equality.</param>
        public static int LastIndexOf<T, TValue>(this Span<T> span, TValue value, Func<TValue, T, bool> equalityComparer)
        {
            if (equalityComparer == null)
                throw new ArgumentNullException(nameof(equalityComparer));

            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, equalityComparer);
        }

        /// <summary>
        /// Searches for value for witch the equality comparer return true and returns the index of its last occurrence. If not found, returns -1.
        /// </summary>
        /// <param name="span">The span to search.</param>
        /// <param name="equalityComparer">A function to test each element for a equality.</param>
        public static int LastIndexOf<T, TValue>(this ReadOnlySpan<T> span, TValue value, Func<TValue, T, bool> equalityComparer)
        {
            if (equalityComparer == null)
                throw new ArgumentNullException(nameof(equalityComparer));

            return SpanHelpers.LastIndexOf(ref MemoryMarshal.GetReference(span), span.Length, value, equalityComparer);
        }

        #endregion

        //#region IndexOfEqualAny

        //#region value0, value1

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int IndexOfEqualAny<T, TValue>(this Span<T> span, TValue value0, TValue value1)
        //{
        //    if (value0 is IEquatable<T> equatable0)
        //    {
        //        if (typeof(TValue) == typeof(T) && value0 is T tValue0 && value1 is T tValue1)
        //            return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOfAny(span, tValue0, tValue1);
        //        else if (value1 is IEquatable<T> equatable1)
        //            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1),
        //                (eValue, sValue) => eValue.equatable0.Equals(sValue) || eValue.equatable1.Equals(sValue));
        //    }
        //    else if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1),
        //            (vValue, sValue) => sValue is IEquatable<TValue> sEquatable ?
        //                sEquatable.Equals(vValue.value0) || sEquatable.Equals(vValue.value1) :
        //                vValue.value0.Equals(sValue) || vValue.value1.Equals(sValue));
        //    else
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1),
        //            (vValue, sValue) => vValue.value0.Equals(sValue) || vValue.value1.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int IndexOfEqualAny<T, TValue>(this ReadOnlySpan<T> span, TValue value0, TValue value1)
        //{
        //    if (value0 is IEquatable<T> equatable0)
        //    {
        //        if (typeof(TValue) == typeof(T) && value0 is T tValue0 && value1 is T tValue1)
        //            return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOfAny(span, tValue0, tValue1);
        //        else if (value1 is IEquatable<T> equatable1)
        //            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1),
        //                (eValue, sValue) => eValue.equatable0.Equals(sValue) || eValue.equatable1.Equals(sValue));
        //    }
        //    return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1),
        //        (vValue, sValue) => vValue.value0.Equals(sValue) || vValue.value1.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of value that not equal to all the specified values similar to calling IndexOfNotEqual several times with the logical AND operator.
        ///// If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int IndexOfNotEqualAll<T, TValue>(this Span<T> span, TValue value0, TValue value1)
        //{
        //    if (value0 is IEquatable<T> equatable0 && value1 is IEquatable<T> equatable1)
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1),
        //            (eValue, sValue) => !eValue.equatable0.Equals(sValue) && !eValue.equatable1.Equals(sValue));
        //    else
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1),
        //            (vValue, sValue) => !vValue.value0.Equals(sValue) && !vValue.value1.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of value that not equal to all the specified values similar to calling IndexOfNotEqual several times with the logical AND operator.
        ///// If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int IndexOfNotEqualAll<T, TValue>(this ReadOnlySpan<T> span, TValue value0, TValue value1)
        //{
        //    if (value0 is IEquatable<T> equatable0 && value1 is IEquatable<T> equatable1)
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1),
        //            (eValue, sValue) => !eValue.equatable0.Equals(sValue) && !eValue.equatable1.Equals(sValue));
        //    else
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1),
        //            (vValue, sValue) => !vValue.value0.Equals(sValue) && !vValue.value1.Equals(sValue));
        //}

        //#endregion

        //#region value0, value1, value2

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int IndexOfEqualAny<T, TValue>(this Span<T> span, TValue value0, TValue value1, TValue value2)
        //{
        //    if (value0 is IEquatable<T> equatable0)
        //    {
        //        if (typeof(TValue) == typeof(T) && value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2)
        //            return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOfAny(span, tValue0, tValue1, tValue2);
        //        else if (value1 is IEquatable<T> equatable1 && value2 is IEquatable<T> equatable2)
        //            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1, equatable2),
        //                (eValue, sValue) => eValue.equatable0.Equals(sValue) || eValue.equatable1.Equals(sValue) || eValue.equatable2.Equals(sValue));
        //    }
        //    return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1, value2),
        //        (vValue, sValue) => vValue.value0.Equals(sValue) || vValue.value1.Equals(sValue) || vValue.value2.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int IndexOfEqualAny<T, TValue>(this ReadOnlySpan<T> span, TValue value0, TValue value1, TValue value2)
        //{
        //    if (value0 is IEquatable<T> equatable0)
        //    {
        //        if (typeof(TValue) == typeof(T) && value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2)
        //            return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOfAny(span, tValue0, tValue1, tValue2);
        //        else if (value1 is IEquatable<T> equatable1 && value2 is IEquatable<T> equatable2)
        //            return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1, equatable2),
        //                (eValue, sValue) => eValue.equatable0.Equals(sValue) || eValue.equatable1.Equals(sValue) || eValue.equatable2.Equals(sValue));
        //    }
        //    return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1, value2),
        //        (vValue, sValue) => vValue.value0.Equals(sValue) || vValue.value1.Equals(sValue) || vValue.value2.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of value that not equal to all the specified values similar to calling IndexOfNotEqual several times with the logical AND operator.
        ///// If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int IndexOfNotEqualAll<T, TValue>(this Span<T> span, TValue value0, TValue value1, TValue value2)
        //{
        //    if (value0 is IEquatable<T> equatable0 && value1 is IEquatable<T> equatable1 && value2 is IEquatable<T> equatable2)
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1, equatable2),
        //            (eValue, sValue) => !eValue.equatable0.Equals(sValue) && !eValue.equatable1.Equals(sValue) && !eValue.equatable1.Equals(sValue));
        //    else
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1, value2),
        //            (vValue, sValue) => !vValue.value0.Equals(sValue) && !vValue.value1.Equals(sValue) && !vValue.value2.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of value that not equal to all the specified values similar to calling IndexOfNotEqual several times with the logical AND operator.
        ///// If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int IndexOfNotEqualAll<T, TValue>(this ReadOnlySpan<T> span, TValue value0, TValue value1, TValue value2)
        //{
        //    if (value0 is IEquatable<T> equatable0 && value1 is IEquatable<T> equatable1 && value2 is IEquatable<T> equatable2)
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (equatable0, equatable1, equatable2),
        //            (eValue, sValue) => !eValue.equatable0.Equals(sValue) && !eValue.equatable1.Equals(sValue) && !eValue.equatable1.Equals(sValue));
        //    else
        //        return SpanHelpers.IndexOf(ref MemoryMarshal.GetReference(span), span.Length, (value0, value1, value2),
        //            (vValue, sValue) => !vValue.value0.Equals(sValue) && !vValue.value1.Equals(sValue) && !vValue.value2.Equals(sValue));
        //}

        //#endregion

        //#region values

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int IndexOfEqualAny<T, TValue>(this Span<T> span, ReadOnlySpan<TValue> values)
        //{
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(TValue)))
        //    {
        //        if (typeof(TValue) == typeof(T))
        //        {
        //            ReadOnlySpan<T> tValues;
        //            unsafe
        //            {
        //                tValues = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)), values.Length);
        //            }
        //            return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOfAny(span, tValues);
        //        }
        //        return SpanHelpers.IndexOfEqualAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length,
        //            (vValue, sValue) => vValue is IEquatable<T> equatable ? equatable.Equals(sValue) : vValue.Equals(sValue));
        //    }
        //    else if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
        //        return SpanHelpers.IndexOfEqualAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length,
        //            (vValue, sValue) => sValue is IEquatable<T> equatable ? equatable.Equals(vValue) : sValue.Equals(vValue));
        //    else
        //        return SpanHelpers.IndexOfEqualAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length,
        //            (vValue, sValue) => vValue.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int IndexOfEqualAny<T, TValue>(this ReadOnlySpan<T> span, ReadOnlySpan<TValue> values)
        //{
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(TValue)))
        //    {
        //        if (typeof(TValue) == typeof(T))
        //        {
        //            ReadOnlySpan<T> tValues;
        //            unsafe
        //            {
        //                tValues = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(values)), values.Length);
        //            }
        //            return MemoryExtensionsEquatablePatternMatching<T>.Instance.IndexOfAny(span, tValues);
        //        }
        //        return SpanHelpers.IndexOfEqualAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length,
        //            (vValue, sValue) => vValue is IEquatable<T> equatable ? equatable.Equals(sValue) : vValue.Equals(sValue));
        //    }
        //    else if (typeof(IEquatable<TValue>).IsAssignableFrom(typeof(T)))
        //        return SpanHelpers.IndexOfEqualAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length,
        //            (vValue, sValue) => sValue is IEquatable<T> equatable ? equatable.Equals(vValue) : sValue.Equals(vValue));
        //    else
        //        return SpanHelpers.IndexOfEqualAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length,
        //            (vValue, sValue) => vValue.Equals(sValue));
        //}

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        ///// <param name="equalityComparer">The set of values to search for.</param>
        //public static int IndexOfEqualAny<T, TValue>(this Span<T> span, ReadOnlySpan<TValue> values, Func<TValue, T, bool> equalityComparer)
        //{
        //    return SpanHelpers.IndexOfEqualAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        //}

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int IndexOfAnyEqual<T, TValue>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T> equalityComparer)
        //{
        //    return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        //}

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int IndexOfAnyEqual<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> values)
        //{
        //    return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        //}

        ///// <summary>
        ///// Searches for the first index of any of the specified values similar to calling IndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int IndexOfAnyEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> values)
        //{
        //    return SpanHelpers.IndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        //}

        //#endregion

        //#endregion

        //#region LastIndexOfAnyEqual

        //#region value0, value1

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, T value0, T value1)
        //{
        //    if (value0 is IEquatable<T>)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, value0, value1);
        //    return SpanHelpers.LastIndexOfAnyObject(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, T value0, T value1)
        //{
        //    if (value0 is IEquatable<T>)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, value0, value1);
        //    return SpanHelpers.LastIndexOfAnyObject(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, T value0, T value1, IEqualityComparer<T> equalityComparer)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, equalityComparer, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, T value0, T value1, IEqualityComparer<T> equalityComparer)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, equalityComparer, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, IEquatable<T> value0, IEquatable<T> value1)
        //{
        //    if (value0 is T tValue0 && tValue0 is IEquatable<T> && value1 is T tValue1)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, tValue0, tValue1);
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        //}

        ///// <summary>
        ///// Searches for the дast index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, IEquatable<T> value0, IEquatable<T> value1)
        //{
        //    if (value0 is T tValue0 && tValue0 is IEquatable<T> && value1 is T tValue1)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, tValue0, tValue1);
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, span.Length);
        //}

        //#endregion

        //#region value0, value1, value2

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, T value0, T value1, T value2)
        //{
        //    if (value0 is IEquatable<T>)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, value0, value1, value2);
        //    return SpanHelpers.LastIndexOfAnyObject(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2)
        //{
        //    if (value0 is IEquatable<T>)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, value0, value1, value2);
        //    return SpanHelpers.LastIndexOfAnyObject(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, equalityComparer, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, T value0, T value1, T value2, IEqualityComparer<T> equalityComparer)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, equalityComparer, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2)
        //{
        //    if (value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2 && tValue0 is IEquatable<T>)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, tValue0, tValue1, tValue2);
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1.
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="value0">One of the values to search for.</param>
        ///// <param name="value1">One of the values to search for.</param>
        ///// <param name="value2">One of the values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, IEquatable<T> value0, IEquatable<T> value1, IEquatable<T> value2)
        //{
        //    if (value0 is T tValue0 && value1 is T tValue1 && value2 is T tValue2 && tValue0 is IEquatable<T>)
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, tValue0, tValue1, tValue2);
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), value0, value1, value2, span.Length);
        //}

        //#endregion

        //#region values

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, ReadOnlySpan<T> values)
        //{
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, values);
        //    return SpanHelpers.LastIndexOfAnyObject(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values)
        //{
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.LastIndexOfAny(span, values);
        //    return SpanHelpers.LastIndexOfAnyObject(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, ReadOnlySpan<T> values, IEqualityComparer<T> equalityComparer)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> values, IEqualityComparer<T> equalityComparer)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length, equalityComparer);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> values)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        //}

        ///// <summary>
        ///// Searches for the last index of any of the specified values similar to calling LastIndexOf several times with the logical OR operator. If not found, returns -1. 
        ///// Values are compared using IEquatable{T}.Equals(T). 
        ///// </summary>
        ///// <param name="span">The span to search.</param>
        ///// <param name="values">The set of values to search for.</param>
        //public static int LastIndexOfAnyEqual<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> values)
        //{
        //    return SpanHelpers.LastIndexOfAny(ref MemoryMarshal.GetReference(span), span.Length, ref MemoryMarshal.GetReference(values), values.Length);
        //}

        //#endregion

        //#endregion

        #region SequenceEqualTo

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool SequenceEqualTo<T, TOther>(this Span<T> span, ReadOnlySpan<TOther> other)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(TOther)))
            {
                if (typeof(TOther) == typeof(T))
                {

                    ReadOnlySpan<T> tOther;
                    unsafe
                    {
                        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                }
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, (tv, ov) => 
                    ov is IEquatable<T> oEquatable ? oEquatable.Equals(tv) : ov.Equals(tv));
            }
            if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, (tv, ov) => 
                    tv is IEquatable<T> tEquatable ? tEquatable.Equals(ov) : ov.Equals(tv));
            return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, (tv, ov) => 
                tv.Equals(ov));
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        /// </summary>
        public static bool SequenceEqualTo<T, TOther>(this ReadOnlySpan<T> span, ReadOnlySpan<TOther> other)
        {
            int length = span.Length;
            if (length != other.Length)
                return false;
            if (typeof(IEquatable<T>).IsAssignableFrom(typeof(TOther)))
            {
                if (typeof(TOther) == typeof(T))
                {

                    ReadOnlySpan<T> tOther;
                    unsafe
                    {
                        tOther = new ReadOnlySpan<T>(Unsafe.AsPointer(ref MemoryMarshal.GetReference(other)), length);
                    }
                    return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span, tOther);

                }
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, (tv, ov) => 
                    ov is IEquatable<T> oEquatable ? oEquatable.Equals(tv) : ov.Equals(tv));
            }
            if (typeof(IEquatable<TOther>).IsAssignableFrom(typeof(T)))
                return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, (tv, ov) => 
                    tv is IEquatable<T> tEquatable ? tEquatable.Equals(ov) : ov.Equals(tv));
            return SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, (tv, ov) => 
                tv.Equals(ov));
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool SequenceEqualTo<T, TOther>(this Span<T> span, ReadOnlySpan<TOther> other, Func<T, TOther, bool> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, equalityComparer);
        }

        /// <summary>
        /// Determines whether two sequences are equal by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        /// </summary>
        public static bool SequenceEqualTo<T, TOther>(this ReadOnlySpan<T> span, ReadOnlySpan<TOther> other, Func<T, TOther, bool> equalityComparer)
        {
            int length = span.Length;
            return length == other.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(other), length, equalityComparer);
        }

        #endregion

        //#region StartsWithSeq

        ///// <summary>
        ///// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        //public static bool StartsWithSeq<T>(this Span<T> span, ReadOnlySpan<T> value)
        //{
        //    int valueLength = value.Length;
        //    if (valueLength > span.Length)
        //        return false;
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span.Slice(0, valueLength), value);
        //    return SpanHelpers.SequenceEqualObject(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        //public static bool StartsWithSeq<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        //{
        //    int valueLength = value.Length;
        //    if (valueLength > span.Length)
        //        return false;
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span.Slice(0, valueLength), value);
        //    return SpanHelpers.SequenceEqualObject(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        //public static bool StartsWithSeq<T>(this Span<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        //{
        //    int valueLength = value.Length;
        //    return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        //public static bool StartsWithSeq<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        //{
        //    int valueLength = value.Length;
        //    return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T).
        ///// </summary>
        //public static bool StartsWithSeq<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> value)
        //{
        //    int valueLength = value.Length;
        //    return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the start of the span by comparing the elements using IEquatable{T}.Equals(T).
        ///// </summary>
        //public static bool StartsWithSeq<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> value)
        //{
        //    int valueLength = value.Length;
        //    return valueLength <= span.Length && SpanHelpers.SequenceEqual(ref MemoryMarshal.GetReference(span), ref MemoryMarshal.GetReference(value), valueLength);
        //}

        //#endregion

        //#region EndsWithSeq

        ///// <summary>
        ///// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        //public static bool EndsWithSeq<T>(this Span<T> span, ReadOnlySpan<T> value)
        //{
        //    int spanLength = span.Length;
        //    int valueLength = value.Length;
        //    if (valueLength > spanLength)
        //        return false;
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span.Slice(spanLength - valueLength), value);
        //    return SpanHelpers.SequenceEqualObject(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
        //        ref MemoryMarshal.GetReference(value), valueLength);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T) or Object.Equals(Object).
        ///// </summary>
        //public static bool EndsWithSeq<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value)
        //{
        //    int spanLength = span.Length;
        //    int valueLength = value.Length;
        //    if (valueLength > spanLength)
        //        return false;
        //    if (typeof(IEquatable<T>).IsAssignableFrom(typeof(T)))
        //        return MemoryExtensionsEquatablePatternMatching<T>.Instance.SequenceEqual(span.Slice(spanLength - valueLength), value);
        //    return SpanHelpers.SequenceEqualObject(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
        //        ref MemoryMarshal.GetReference(value), valueLength);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        //public static bool EndsWithSeq<T>(this Span<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        //{
        //    int spanLength = span.Length;
        //    int valueLength = value.Length;
        //    return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
        //        ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEqualityComparer{T}.Equals(T, T).
        ///// </summary>
        //public static bool EndsWithSeq<T>(this ReadOnlySpan<T> span, ReadOnlySpan<T> value, IEqualityComparer<T> equalityComparer)
        //{
        //    int spanLength = span.Length;
        //    int valueLength = value.Length;
        //    return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
        //        ref MemoryMarshal.GetReference(value), valueLength, equalityComparer);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T).
        ///// </summary>
        //public static bool EndsWithSeq<T>(this Span<T> span, ReadOnlySpan<IEquatable<T>> value)
        //{
        //    int spanLength = span.Length;
        //    int valueLength = value.Length;
        //    return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
        //        ref MemoryMarshal.GetReference(value), valueLength);
        //}

        ///// <summary>
        ///// Determines whether the specified sequence appears at the end of the span by comparing the elements using IEquatable{T}.Equals(T).
        ///// </summary>
        //public static bool EndsWithSeq<T>(this ReadOnlySpan<T> span, ReadOnlySpan<IEquatable<T>> value)
        //{
        //    int spanLength = span.Length;
        //    int valueLength = value.Length;
        //    return valueLength <= spanLength && SpanHelpers.SequenceEqual(ref Unsafe.Add(ref MemoryMarshal.GetReference(span), spanLength - valueLength),
        //        ref MemoryMarshal.GetReference(value), valueLength);
        //}

        //#endregion
    }
}
