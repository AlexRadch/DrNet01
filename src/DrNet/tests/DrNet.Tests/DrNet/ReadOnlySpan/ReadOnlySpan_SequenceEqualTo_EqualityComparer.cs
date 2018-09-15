using System;
using Xunit;

namespace DrNet.Tests.ReadOnlySpan
{
    public abstract class ReadOnlySpan_SequenceEqualTo_EqualityComparer<T>
    {
        public abstract T CreateValue(int value);

        protected Action<T, T> onCompare;

        public bool EqualityComparer(T v1, T v2)
        {
            onCompare?.Invoke(v1, v2);
            if (v1 is IEquatable<T> equatable)
                return equatable.Equals(v2);
            return v1.Equals(v2);
        }

        public bool EqualityComparer(TEquatable<T> v1, TEquatable<T> v2) => EqualityComparer(v1.Value, v2.Value);

        [Fact]
        public void ZeroLengthSequenceEqual()
        {
            T[] a = new T[3];

            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 1, 0);
            ReadOnlySpan<T> second = new ReadOnlySpan<T>(a, 2, 0);
            bool b = first.SequenceEqualTo<T, T>(second, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void SameSpanSequenceEqual()
        {
            T[] a = { CreateValue(4), CreateValue(5), CreateValue(6) };
            ReadOnlySpan<T> span = new ReadOnlySpan<T>(a);
            bool b = span.SequenceEqualTo<T, T>(span, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void SequenceEqualArrayImplicit()
        {
            T[] a = { CreateValue(4), CreateValue(5), CreateValue(6) };
            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 0, 3);
            bool b = first.SequenceEqualTo<T, T>(a, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void SequenceEqualArraySegmentImplicit()
        {
            T[] src = { CreateValue(1), CreateValue(2), CreateValue(3) };
            T[] dst = { CreateValue(5), CreateValue(1), CreateValue(2), CreateValue(3), CreateValue(10) };
            var segment = new ArraySegment<T>(dst, 1, 3);

            ReadOnlySpan<T> first = new ReadOnlySpan<T>(src, 0, 3);
            bool b = first.SequenceEqualTo<T, T>(segment, EqualityComparer);
            Assert.True(b);
        }

        [Fact]
        public void LengthMismatchSequenceEqual()
        {
            T[] a = { CreateValue(4), CreateValue(5), CreateValue(6) };
            ReadOnlySpan<T> first = new ReadOnlySpan<T>(a, 0, 3);
            ReadOnlySpan<T> second = new ReadOnlySpan<T>(a, 0, 2);
            bool b = first.SequenceEqualTo<T, T>(second, EqualityComparer);
            Assert.False(b);
            b = second.SequenceEqualTo<T, T>(first, EqualityComparer);
            Assert.False(b);
        }

        [Fact]
        public void OnSequenceEqualOfEqualSpansMakeSureEveryElementIsCompared()
        {
            for (int length = 0; length < 100; length++)
            {
                TLog<T> log = new TLog<T>();
                onCompare = log.Add;

                T[] first = new T[length];
                T[] second = new T[length];
                for (int i = 0; i < length; i++)
                {
                    first[i] = second[i] = CreateValue(10 * (i + 1));
                }

                ReadOnlySpan<T> firstSpan = new ReadOnlySpan<T>(first);
                ReadOnlySpan<T> secondSpan = new ReadOnlySpan<T>(second);
                bool b = firstSpan.SequenceEqualTo(secondSpan, EqualityComparer);
                Assert.True(b);

                // Make sure each element of the array was compared once. (Strictly speaking, it would not be illegal for 
                // SequenceEqual to compare an element more than once but that would be a non-optimal implementation and 
                // a red flag. So we'll stick with the stricter test.)
                Assert.Equal(first.Length, log.Count);
                foreach (T elem in first)
                {
                    int numCompares = log.CountCompares(elem, elem);
                    Assert.True(numCompares == 1, $"Expected {numCompares} == 1 for element {elem}.");
                }
            }
        }

        [Fact]
        public void SequenceEqualNoMatch()
        {
            for (int length = 1; length < 32; length++)
            {
                for (int mismatchIndex = 0; mismatchIndex < length; mismatchIndex++)
                {
                    TLog<T> log = new TLog<T>();
                    onCompare = log.Add;

                    T[] first = new T[length];
                    T[] second = new T[length];
                    for (int i = 0; i < length; i++)
                    {
                        first[i] = second[i] = CreateValue(10 * (i + 1));
                    }

                    second[mismatchIndex] = CreateValue(10 * (mismatchIndex + 2));

                    ReadOnlySpan<T> firstSpan = new ReadOnlySpan<T>(first);
                    ReadOnlySpan<T> secondSpan = new ReadOnlySpan<T>(second);
                    bool b = firstSpan.SequenceEqualTo(secondSpan, EqualityComparer);
                    Assert.False(b);

                    Assert.Equal(1, log.CountCompares(first[mismatchIndex], second[mismatchIndex]));
                }
            }
        }

        [Fact]
        public void MakeSureNoSequenceEqualChecksGoOutOfRange()
        {
            T GuardValue = CreateValue(77777);
            const int GuardLength = 50;

            Action<T, T> checkForOutOfRangeAccess =
                delegate (T x, T y)
                {
                    if (GuardValue.Equals(x) || GuardValue.Equals(y))
                        throw new Exception("Detected out of range access in IndexOf()");
                };

            for (int length = 0; length < 100; length++)
            {
                TEquatable<T>[] first = new TEquatable<T>[GuardLength + length + GuardLength];
                TEquatable<T>[] second = new TEquatable<T>[GuardLength + length + GuardLength];
                for (int i = 0; i < first.Length; i++)
                {
                    first[i] = second[i] = new TEquatable<T>(GuardValue, checkForOutOfRangeAccess);
                }

                for (int i = 0; i < length; i++)
                {
                    first[GuardLength + i] = second[GuardLength + i] = new TEquatable<T>(CreateValue(10 * (i + 1)), checkForOutOfRangeAccess);
                }

                ReadOnlySpan<TEquatable<T>> firstSpan = new ReadOnlySpan<TEquatable<T>>(first, GuardLength, length);
                ReadOnlySpan<TEquatable<T>> secondSpan = new ReadOnlySpan<TEquatable<T>>(second, GuardLength, length);
                bool b = firstSpan.SequenceEqualTo<TEquatable<T>, TEquatable<T>>(secondSpan, EqualityComparer);
                Assert.True(b);
            }
        }
    }

    public class ReadOnlySpan_SequenceEqualTo_EqualityComparer_int: ReadOnlySpan_SequenceEqualTo_EqualityComparer<int>
    {
        public override int CreateValue(int value) => value;
    }

    public class ReadOnlySpan_SequenceEqualTo_EqualityComparer_string: ReadOnlySpan_SequenceEqualTo_EqualityComparer<string>
    {
        public override string CreateValue(int value) => value.ToString();
    }
}
