using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using CSUnsafe = System.Runtime.CompilerServices.Unsafe;

namespace DrNet.Internal.Unsafe
{
    public static class SpanHelpers
    {
        #region IndexOfEqual LastIndexOfEqual

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IndexOfEqualSourceComparer<TSource, TValue>(ref TSource searchSpace, int length, 
            TValue value, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index), value))
                    goto Found;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 1), value))
                    goto Found1;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 2), value))
                    goto Found2;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 3), value))
                    goto Found3;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 4), value))
                    goto Found4;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 5), value))
                    goto Found5;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 6), value))
                    goto Found6;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 7), value))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index), value))
                    goto Found;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 1), value))
                    goto Found1;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 2), value))
                    goto Found2;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index + 3), value))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, index), value))
                    goto Found;

                index += 1;
                length--;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return (int)(byte*)index;
        Found1:
            return (int)(byte*)(index + 1);
        Found2:
            return (int)(byte*)(index + 2);
        Found3:
            return (int)(byte*)(index + 3);
        Found4:
            return (int)(byte*)(index + 4);
        Found5:
            return (int)(byte*)(index + 5);
        Found6:
            return (int)(byte*)(index + 6);
        Found7:
            return (int)(byte*)(index + 7);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IndexOfEqualValueComparer<TSource, TValue>(ref TSource searchSpace, int length, 
            TValue value, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 4)))
                    goto Found4;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 5)))
                    goto Found5;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 6)))
                    goto Found6;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 7)))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index)))
                    goto Found;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 1)))
                    goto Found1;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 2)))
                    goto Found2;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index + 3)))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, index)))
                    goto Found;

                index += 1;
                length--;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return (int)(byte*)index;
        Found1:
            return (int)(byte*)(index + 1);
        Found2:
            return (int)(byte*)(index + 2);
        Found3:
            return (int)(byte*)(index + 3);
        Found4:
            return (int)(byte*)(index + 4);
        Found5:
            return (int)(byte*)(index + 5);
        Found6:
            return (int)(byte*)(index + 6);
        Found7:
            return (int)(byte*)(index + 7);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LastIndexOfEqualSourceComparer<TSource, TValue>(ref TSource searchSpace, int length, 
            TValue value, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            while (length >= 8)
            {
                length -= 8;

                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 7), value))
                    goto Found7;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 6), value))
                    goto Found6;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 5), value))
                    goto Found5;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 4), value))
                    goto Found4;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 3), value))
                    goto Found3;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 2), value))
                    goto Found2;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 1), value))
                    goto Found1;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length), value))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 3), value))
                    goto Found3;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 2), value))
                    goto Found2;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length + 1), value))
                    goto Found1;
                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length), value))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                if (equalityComparer(CSUnsafe.Add(ref searchSpace, length), value))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int LastIndexOfEqualValueComparer<TSource, TValue>(ref TSource searchSpace, int length, 
            TValue value, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            while (length >= 8)
            {
                length -= 8;

                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 7)))
                    goto Found7;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 6)))
                    goto Found6;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 5)))
                    goto Found5;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 4)))
                    goto Found4;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 3)))
                    goto Found3;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 2)))
                    goto Found2;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length + 1)))
                    goto Found1;
                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length)))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                if (equalityComparer(value, CSUnsafe.Add(ref searchSpace, length)))
                    goto Found;
            }
            return -1;

        Found: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return length;
        Found1:
            return length + 1;
        Found2:
            return length + 2;
        Found3:
            return length + 3;
        Found4:
            return length + 4;
        Found5:
            return length + 5;
        Found6:
            return length + 6;
        Found7:
            return length + 7;

        }

        #endregion

        #region IndexOfEqualAny LastIndexOfEqualAny

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfEqualAnySourceComparer<TSource, TValue>(ref TSource searchSpace, int searchSpaceLength,
            ref TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = IndexOfEqualSourceComparer(ref searchSpace, searchSpaceLength, 
                    CSUnsafe.Add(ref value, i), equalityComparer);
                if (tempIndex >= 0)
                {
                    index = tempIndex;
                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    searchSpaceLength = tempIndex;

                    if (tempIndex == 0)
                        break;
                }
            }
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfEqualAnyValueComparer<TSource, TValue>(ref TSource searchSpace, int searchSpaceLength,
            ref TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = IndexOfEqualValueComparer(ref searchSpace, searchSpaceLength, CSUnsafe.Add(ref value, i), equalityComparer);
                if (tempIndex >= 0)
                {
                    index = tempIndex;
                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    searchSpaceLength = tempIndex;

                    if (tempIndex == 0)
                        break;
                }
            }
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualAnySourceComparer<TSource, TValue>(ref TSource searchSpace, 
            int searchSpaceLength, ref TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = LastIndexOfEqualSourceComparer(ref CSUnsafe.Add(ref searchSpace, index + 1), searchSpaceLength, CSUnsafe.Add(ref value, i), equalityComparer);
                if (tempIndex >= 0)
                {
                    tempIndex++;
                    index += tempIndex;

                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    //searchSpace = CSUnsafe.Add(ref searchSpace, tempIndex);
                    searchSpaceLength -= tempIndex;

                    if (searchSpaceLength <= 0)
                        break;
                }
            }
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualAnyValueComparer<TSource, TValue>(ref TSource searchSpace, 
            int searchSpaceLength, ref TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = LastIndexOfEqualValueComparer(ref CSUnsafe.Add(ref searchSpace, index + 1), searchSpaceLength, CSUnsafe.Add(ref value, i), equalityComparer);
                if (tempIndex >= 0)
                {
                    tempIndex++;
                    index += tempIndex;

                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    //searchSpace = CSUnsafe.Add(ref searchSpace, tempIndex);
                    searchSpaceLength -= tempIndex;

                    if (searchSpaceLength <= 0)
                        break;
                }
            }
            return index;
        }

        #endregion

        #region IndexOfNotEqualAll LastIndexOfNotEqualAll

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqualAllSourceComparer<TSource, TValue>(ref TSource searchSpace, 
            int searchSpaceLength, ref TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = 0; i < searchSpaceLength; i++)
                if (IndexOfEqualValueComparer(ref value, valueLength, CSUnsafe.Add(ref searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqualAllValueComparer<TSource, TValue>(ref TSource searchSpace, 
            int searchSpaceLength, ref TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = 0; i < searchSpaceLength; i++)
                if (IndexOfEqualSourceComparer(ref value, valueLength, CSUnsafe.Add(ref searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAllSourceComparer<TSource, TValue>(ref TSource searchSpace, 
            int searchSpaceLength, ref TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = searchSpaceLength - 1; i >= 0; i--)
                if (IndexOfEqualValueComparer(ref value, valueLength, CSUnsafe.Add(ref searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAllValueComparer<TSource, TValue>(ref TSource searchSpace,
            int searchSpaceLength, ref TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = searchSpaceLength - 1; i >= 0; i--)
                if (IndexOfEqualSourceComparer(ref value, valueLength, CSUnsafe.Add(ref searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        #endregion

        #region Sequence

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeq<TFirst, TSecond>(ref TFirst searchSpace, int searchSpaceLength,
            ref TSecond value, int valueLength, Func<TFirst, TSecond, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            //Debug.Assert(searchSpaceLength >= valueLength);
            Debug.Assert(equalityComparer != null);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            TSecond valueHead = value;
            ref TSecond valueTail = ref CSUnsafe.Add(ref value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (; ; )
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength); 
                
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = IndexOfEqualSourceComparer(ref CSUnsafe.Add(ref searchSpace, index), 
                    remainingSearchSpaceLength, valueHead, equalityComparer);
                if (relativeIndex == -1)
                    break;
                index += relativeIndex;

                // Found the first element of "value". See if the tail matches.
                if (EqualToSeq(ref CSUnsafe.Add(ref searchSpace, index + 1), ref valueTail, valueTailLength, 
                    equalityComparer))
                    return index;  // The tail matched. Return a successful find.

                index++;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeqFrom<TFirst, TSecond>(ref TFirst searchSpace, int searchSpaceLength,
            ref TSecond value, int valueLength, Func<TSecond, TFirst, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            //Debug.Assert(searchSpaceLength >= valueLength);
            Debug.Assert(equalityComparer != null);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            TSecond valueHead = value;
            ref TSecond valueTail = ref CSUnsafe.Add(ref value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (; ; )
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength); 
                
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = IndexOfEqualValueComparer(ref CSUnsafe.Add(ref searchSpace, index), 
                    remainingSearchSpaceLength, valueHead, equalityComparer);
                if (relativeIndex == -1)
                    break;
                index += relativeIndex;

                // Found the first element of "value". See if the tail matches.
                if (EqualToSeq(ref valueTail, ref CSUnsafe.Add(ref searchSpace, index + 1), valueTailLength, 
                    equalityComparer))
                    return index;  // The tail matched. Return a successful find.

                index++;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeq<TFirst, TSecond>(ref TFirst searchSpace, int searchSpaceLength,
            ref TSecond value, int valueLength, Func<TFirst, TSecond, bool> equalityComparer)
        {
            Debug.Assert(valueLength > 0);
            Debug.Assert(searchSpaceLength >= valueLength);
            Debug.Assert(equalityComparer != null);

            TSecond valueHead = value;
            ref TSecond valueTail = ref CSUnsafe.Add(ref value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (; ; )
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength);
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = LastIndexOfEqualSourceComparer(ref searchSpace, remainingSearchSpaceLength, valueHead,
                    equalityComparer);
                if (relativeIndex == -1)
                    break;

                // Found the first element of "value". See if the tail matches.
                if (EqualToSeq(ref CSUnsafe.Add(ref searchSpace, relativeIndex + 1), ref valueTail, valueTailLength, 
                    equalityComparer))
                    return relativeIndex;  // The tail matched. Return a successful find.

                index += remainingSearchSpaceLength - relativeIndex;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeqFrom<TFirst, TSecond>(ref TFirst searchSpace, int searchSpaceLength,
            ref TSecond value, int valueLength, Func<TSecond, TFirst, bool> equalityComparer)
        {
            Debug.Assert(valueLength > 0);
            Debug.Assert(searchSpaceLength >= valueLength);
            Debug.Assert(equalityComparer != null);

            TSecond valueHead = value;
            ref TSecond valueTail = ref CSUnsafe.Add(ref value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (; ; )
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength);
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = LastIndexOfEqualValueComparer(ref searchSpace, remainingSearchSpaceLength, valueHead,
                    equalityComparer);
                if (relativeIndex == -1)
                    break;

                // Found the first element of "value". See if the tail matches.
                if (EqualToSeq(ref valueTail, ref CSUnsafe.Add(ref searchSpace, relativeIndex + 1), valueTailLength, 
                    equalityComparer))
                    return relativeIndex;  // The tail matched. Return a successful find.

                index += remainingSearchSpaceLength - relativeIndex;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualToSeq<TFirst, TSecond>(ref TFirst first, ref TSecond second, int length,
            Func<TFirst, TSecond, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            if (CSUnsafe.AreSame(ref first, ref CSUnsafe.As<TSecond, TFirst>(ref second)))
                goto Equal;

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;

                if (!equalityComparer(CSUnsafe.Add(ref first, index), CSUnsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 1), CSUnsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 2), CSUnsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 3), CSUnsafe.Add(ref second, index + 3)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 4), CSUnsafe.Add(ref second, index + 4)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 5), CSUnsafe.Add(ref second, index + 5)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 6), CSUnsafe.Add(ref second, index + 6)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 7), CSUnsafe.Add(ref second, index + 7)))
                    goto NotEqual;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (!equalityComparer(CSUnsafe.Add(ref first, index), CSUnsafe.Add(ref second, index)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 1), CSUnsafe.Add(ref second, index + 1)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 2), CSUnsafe.Add(ref second, index + 2)))
                    goto NotEqual;
                if (!equalityComparer(CSUnsafe.Add(ref first, index + 3), CSUnsafe.Add(ref second, index + 3)))
                    goto NotEqual;

                index += 4;
            }

            while (length > 0)
            {
                if (!equalityComparer(CSUnsafe.Add(ref first, index), CSUnsafe.Add(ref second, index)))
                    goto NotEqual;
                index += 1;
                length--;
            }

        Equal:
            return true;

        NotEqual: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return false;
        }

        #endregion
    }
}
