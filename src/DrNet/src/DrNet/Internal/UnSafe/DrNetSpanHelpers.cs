using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using DrNet.Unsafe;

namespace DrNet.Internal.Unsafe
{
    public static class DrNetSpanHelpers
    {
        #region IndexOfEqual LastIndexOfEqual

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int IndexOfEqualSourceComparer<TSource, TValue>(in TSource searchSpace, int length, 
            TValue value, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index), value))
                    goto Found;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 1), value))
                    goto Found1;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 2), value))
                    goto Found2;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 3), value))
                    goto Found3;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 4), value))
                    goto Found4;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 5), value))
                    goto Found5;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 6), value))
                    goto Found6;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 7), value))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(UnsafeIn.Add(in searchSpace, index), value))
                    goto Found;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 1), value))
                    goto Found1;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 2), value))
                    goto Found2;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index + 3), value))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (equalityComparer(UnsafeIn.Add(in searchSpace, index), value))
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
        public static unsafe int IndexOfEqualValueComparer<TSource, TValue>(in TSource searchSpace, int length, 
            TValue value, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index)))
                    goto Found;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 1)))
                    goto Found1;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 2)))
                    goto Found2;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 3)))
                    goto Found3;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 4)))
                    goto Found4;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 5)))
                    goto Found5;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 6)))
                    goto Found6;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 7)))
                    goto Found7;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index)))
                    goto Found;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 1)))
                    goto Found1;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 2)))
                    goto Found2;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index + 3)))
                    goto Found3;

                index += 4;
            }

            while (length > 0)
            {
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, index)))
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
        public static unsafe int LastIndexOfEqualSourceComparer<TSource, TValue>(in TSource searchSpace, int length, 
            TValue value, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            while (length >= 8)
            {
                length -= 8;

                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 7), value))
                    goto Found7;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 6), value))
                    goto Found6;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 5), value))
                    goto Found5;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 4), value))
                    goto Found4;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 3), value))
                    goto Found3;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 2), value))
                    goto Found2;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 1), value))
                    goto Found1;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length), value))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 3), value))
                    goto Found3;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 2), value))
                    goto Found2;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length + 1), value))
                    goto Found1;
                if (equalityComparer(UnsafeIn.Add(in searchSpace, length), value))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                if (equalityComparer(UnsafeIn.Add(in searchSpace, length), value))
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
        public static unsafe int LastIndexOfEqualValueComparer<TSource, TValue>(in TSource searchSpace, int length, 
            TValue value, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            while (length >= 8)
            {
                length -= 8;

                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 7)))
                    goto Found7;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 6)))
                    goto Found6;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 5)))
                    goto Found5;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 4)))
                    goto Found4;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 3)))
                    goto Found3;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 2)))
                    goto Found2;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 1)))
                    goto Found1;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length)))
                    goto Found;
            }

            if (length >= 4)
            {
                length -= 4;

                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 3)))
                    goto Found3;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 2)))
                    goto Found2;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length + 1)))
                    goto Found1;
                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length)))
                    goto Found;
            }

            while (length > 0)
            {
                length--;

                if (equalityComparer(value, UnsafeIn.Add(in searchSpace, length)))
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
        public static int IndexOfEqualAnySourceComparer<TSource, TValue>(in TSource searchSpace, int searchSpaceLength,
            in TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = IndexOfEqualSourceComparer(in searchSpace, searchSpaceLength, 
                    UnsafeIn.Add(in value, i), equalityComparer);
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
        public static int IndexOfEqualAnyValueComparer<TSource, TValue>(in TSource searchSpace, int searchSpaceLength,
            in TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = IndexOfEqualValueComparer(in searchSpace, searchSpaceLength, UnsafeIn.Add(in value, i), equalityComparer);
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
        public static int LastIndexOfEqualAnySourceComparer<TSource, TValue>(in TSource searchSpace, 
            int searchSpaceLength, in TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = LastIndexOfEqualSourceComparer(in UnsafeIn.Add(in searchSpace, index + 1), searchSpaceLength, UnsafeIn.Add(in value, i), equalityComparer);
                if (tempIndex >= 0)
                {
                    tempIndex++;
                    index += tempIndex;

                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    //searchSpace = CSUnsafeIn.Add(in searchSpace, tempIndex);
                    searchSpaceLength -= tempIndex;

                    if (searchSpaceLength <= 0)
                        break;
                }
            }
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfEqualAnyValueComparer<TSource, TValue>(in TSource searchSpace, 
            int searchSpaceLength, in TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            int index = -1;
            for (int i = 0; i < valueLength; i++)
            {
                var tempIndex = LastIndexOfEqualValueComparer(in UnsafeIn.Add(in searchSpace, index + 1), searchSpaceLength, UnsafeIn.Add(in value, i), equalityComparer);
                if (tempIndex >= 0)
                {
                    tempIndex++;
                    index += tempIndex;

                    // Reduce space for search, cause we don't care if we find the search value after the index of a previously found value
                    //searchSpace = CSUnsafeIn.Add(in searchSpace, tempIndex);
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
        public static int IndexOfNotEqualAllSourceComparer<TSource, TValue>(in TSource searchSpace, 
            int searchSpaceLength, in TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = 0; i < searchSpaceLength; i++)
                if (IndexOfEqualValueComparer(in value, valueLength, UnsafeIn.Add(in searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfNotEqualAllValueComparer<TSource, TValue>(in TSource searchSpace, 
            int searchSpaceLength, in TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = 0; i < searchSpaceLength; i++)
                if (IndexOfEqualSourceComparer(in value, valueLength, UnsafeIn.Add(in searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAllSourceComparer<TSource, TValue>(in TSource searchSpace, 
            int searchSpaceLength, in TValue value, int valueLength, Func<TSource, TValue, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = searchSpaceLength - 1; i >= 0; i--)
                if (IndexOfEqualValueComparer(in value, valueLength, UnsafeIn.Add(in searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfNotEqualAllValueComparer<TSource, TValue>(in TSource searchSpace,
            int searchSpaceLength, in TValue value, int valueLength, Func<TValue, TSource, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);

            for (int i = searchSpaceLength - 1; i >= 0; i--)
                if (IndexOfEqualSourceComparer(in value, valueLength, UnsafeIn.Add(in searchSpace, i), 
                    equalityComparer) < 0)
                    return i;

            return -1;
        }

        #endregion

        #region Sequence

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeq<TFirst, TSecond>(in TFirst searchSpace, int searchSpaceLength,
            in TSecond value, int valueLength, Func<TFirst, TSecond, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);
            //Debug.Assert(searchSpaceLength >= valueLength);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            TSecond valueHead = value;
            ref readonly TSecond valueTail = ref UnsafeIn.Add(in value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (;;)
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength); 
                
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = IndexOfEqualSourceComparer(in UnsafeIn.Add(in searchSpace, index), 
                    remainingSearchSpaceLength, valueHead, equalityComparer);
                if (relativeIndex == -1)
                    break;
                index += relativeIndex;

                // Found the first element of "value". See if the tail matches.
                if (EqualsToSeq(in UnsafeIn.Add(in searchSpace, index + 1), in valueTail, valueTailLength, 
                    equalityComparer))
                    return index;  // The tail matched. Return a successful find.

                index++;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOfSeqFrom<TFirst, TSecond>(in TFirst searchSpace, int searchSpaceLength,
            in TSecond value, int valueLength, Func<TSecond, TFirst, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);
            //Debug.Assert(searchSpaceLength >= valueLength);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            TSecond valueHead = value;
            ref readonly TSecond valueTail = ref UnsafeIn.Add(in value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (;;)
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength); 
                
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = IndexOfEqualValueComparer(in UnsafeIn.Add(in searchSpace, index), 
                    remainingSearchSpaceLength, valueHead, equalityComparer);
                if (relativeIndex == -1)
                    break;
                index += relativeIndex;

                // Found the first element of "value". See if the tail matches.
                if (EqualsToSeq(in valueTail, in UnsafeIn.Add(in searchSpace, index + 1), valueTailLength, 
                    equalityComparer))
                    return index;  // The tail matched. Return a successful find.

                index++;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeq<TFirst, TSecond>(in TFirst searchSpace, int searchSpaceLength,
            in TSecond value, int valueLength, Func<TFirst, TSecond, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);
            //Debug.Assert(searchSpaceLength >= valueLength);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            TSecond valueHead = value;
            ref readonly TSecond valueTail = ref UnsafeIn.Add(in value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (;;)
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength);
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = LastIndexOfEqualSourceComparer(in searchSpace, remainingSearchSpaceLength, valueHead,
                    equalityComparer);
                if (relativeIndex == -1)
                    break;

                // Found the first element of "value". See if the tail matches.
                if (EqualsToSeq(in UnsafeIn.Add(in searchSpace, relativeIndex + 1), in valueTail, valueTailLength, 
                    equalityComparer))
                    return relativeIndex;  // The tail matched. Return a successful find.

                index += remainingSearchSpaceLength - relativeIndex;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndexOfSeqFrom<TFirst, TSecond>(in TFirst searchSpace, int searchSpaceLength,
            in TSecond value, int valueLength, Func<TSecond, TFirst, bool> equalityComparer)
        {
            Debug.Assert(searchSpaceLength >= 0);
            Debug.Assert(valueLength >= 0);
            Debug.Assert(equalityComparer != null);
            //Debug.Assert(searchSpaceLength >= valueLength);

            if (valueLength == 0)
                return 0;  // A zero-length sequence is always treated as "found" at the start of the search space.

            TSecond valueHead = value;
            ref readonly TSecond valueTail = ref UnsafeIn.Add(in value, 1);
            int valueTailLength = valueLength - 1;

            int index = 0;
            for (;;)
            {
                // Ensures no deceptive underflows in the computation of "remainingSearchSpaceLength".
                Debug.Assert(0 <= index && index <= searchSpaceLength);
                int remainingSearchSpaceLength = searchSpaceLength - index - valueTailLength;
                // The unsearched portion is now shorter than the sequence we're looking for. So it can't be there.
                if (remainingSearchSpaceLength <= 0)
                    break;  

                // Do a quick search for the first element of "value".
                int relativeIndex = LastIndexOfEqualValueComparer(in searchSpace, remainingSearchSpaceLength, valueHead,
                    equalityComparer);
                if (relativeIndex == -1)
                    break;

                // Found the first element of "value". See if the tail matches.
                if (EqualsToSeq(in valueTail, in UnsafeIn.Add(in searchSpace, relativeIndex + 1), valueTailLength, 
                    equalityComparer))
                    return relativeIndex;  // The tail matched. Return a successful find.

                index += remainingSearchSpaceLength - relativeIndex;
            }
            return -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsToSeq<TFirst, TSecond>(in TFirst first, in TSecond second, int length,
            Func<TFirst, TSecond, bool> equalityComparer)
        {
            Debug.Assert(length >= 0);
            Debug.Assert(equalityComparer != null);

            IntPtr index = (IntPtr)0; // Use IntPtr for arithmetic to avoid unnecessary 64->32->64 truncations
            while (length >= 8)
            {
                length -= 8;

                if (!equalityComparer(UnsafeIn.Add(in first, index), UnsafeIn.Add(in second, index)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 1), UnsafeIn.Add(in second, index + 1)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 2), UnsafeIn.Add(in second, index + 2)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 3), UnsafeIn.Add(in second, index + 3)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 4), UnsafeIn.Add(in second, index + 4)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 5), UnsafeIn.Add(in second, index + 5)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 6), UnsafeIn.Add(in second, index + 6)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 7), UnsafeIn.Add(in second, index + 7)))
                    goto NotEqual;

                index += 8;
            }

            if (length >= 4)
            {
                length -= 4;

                if (!equalityComparer(UnsafeIn.Add(in first, index), UnsafeIn.Add(in second, index)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 1), UnsafeIn.Add(in second, index + 1)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 2), UnsafeIn.Add(in second, index + 2)))
                    goto NotEqual;
                if (!equalityComparer(UnsafeIn.Add(in first, index + 3), UnsafeIn.Add(in second, index + 3)))
                    goto NotEqual;

                index += 4;
            }

            while (length > 0)
            {
                if (!equalityComparer(UnsafeIn.Add(in first, index), UnsafeIn.Add(in second, index)))
                    goto NotEqual;
                index += 1;
                length--;
            }

        //Equal:
            return true;

        NotEqual: // Workaround for https://github.com/dotnet/coreclr/issues/13549
            return false;
        }

        #endregion
    }
}
